using Booking.Contracts.Attributes;
using Booking.Contracts.Parameters;
using Booking.Contracts.Responses;
using Booking.Contracts.Responses.Category;
using Booking.DataAccess.Providers.Abstract;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookingApp.Controllers.Admin
{
    [Route("api/admin/categories")]
    [ApiExplorerSettings(GroupName = "v1-admin")]
    [AdminAuthorized]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryProvider _categoryProvider;      

        public CategoryController(ICategoryProvider categoryProvider)
        {
            _categoryProvider = categoryProvider;
     
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _categoryProvider.GetAll();

                var result = new List<CategoryResponse>();
                foreach (var category in data)
                {
                    var index = result.FindIndex(x => x.Id.Equals(category.Id));
                    if (index == -1)
                    {
                        result.Add(new CategoryResponse
                        {
                            Id = category.Id,
                            ImageUrl = category.ImageUrl,
                            ParentId = category.ParentCategoryId,
                            Name = category.CategoryName
                        });
                        continue;
                    }


                }

                return Ok(InitTreeData(new Guid(), result));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            try
            {
                var category = await _categoryProvider.GetBySlug(slug);
                if (category is null)
                    return BadRequest("Category is null");
       
                return Ok(new CategoryResponse
                {
                    Id = category.Id,
                    ParentId = category.ParentCategoryId,
                    Slug = category.Slug,
                    ImageUrl = category.ImageUrl,
                    Name = category.CategoryName
                });
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var category = await _categoryProvider.GetById(id);
                if (category is null)
                    return BadRequest("Category is null");

            
                return Ok(new CategoryResponse
                {
                    Id = category.Id,
                    ParentId = category.ParentCategoryId,
                    Slug = category.Slug,
                    ImageUrl = category.ImageUrl,
                    Name = category.CategoryName
                });
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryParameter parameter)
        {
            if (Regex.IsMatch(parameter.Slug, @"\p{IsCyrillic}"))
            {
                return BadRequest("There is at least one cyrillic character in the slug");
            }
            try
            {
               

                var parentCategory = await _categoryProvider.FirstOrDefault(x => x.Id.Equals(parameter.ParentId));
                if (parentCategory is null && !parameter.ParentId.Equals(Guid.Empty))
                {
                    return NotFound("No such parent category");
                }
                var category = new Category
                {
                    ImageUrl = parameter.ImageUrl,
                    Alt = parameter.Name,
                    ParentCategoryId = parameter.ParentId,
                    Slug = parameter.Slug,
                    CategoryName = parameter.Name
                };

                await _categoryProvider.Add(category);

                return Ok(category.Id);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] CategoryParameter parameter)
        {
            if (Regex.IsMatch(parameter.Slug, @"\p{IsCyrillic}"))
            {
                return BadRequest("There is at least one cyrillic character in the slug");
            }

            if (await _categoryProvider.FirstOrDefault(x => x.Slug == parameter.Slug && x.Id != id) is not null)
            {
                return BadRequest("Category with current slug existed");
            }

            try
            {
                var category = await _categoryProvider.GetById(id) ??
                               throw new ArgumentException("Category is not found");
                var parentCategory = await _categoryProvider.FirstOrDefault(x => x.Id.Equals(parameter.ParentId));
                if (parentCategory is null && !parameter.ParentId.Equals(Guid.Empty))
                {
                    return NotFound("No such parent category");
                }
                category.ImageUrl = parameter.ImageUrl;
                category.ParentCategoryId = parameter.ParentId;
                category.Slug = parameter.Slug;
                category.CategoryName = parameter.Name;
                await _categoryProvider.Edit(category);
          
            }
            catch (ArgumentException e)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Guid parentCategoryId = Guid.Empty;
            try
            {
                var category = await _categoryProvider.GetById(id) ??
                               throw new ArgumentException("Category is not found");
                parentCategoryId = category.ParentCategoryId;
                await _categoryProvider.Delete(category);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }

            return NoContent();
        }

        private List<CategoryFullResponse> InitTreeData(Guid parentId, List<CategoryResponse> categories)
        {
            return categories.Where(x => x.ParentId.Equals(parentId))
                .Select(x => new CategoryFullResponse
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Alt = x.Alt,
                    Children = InitTreeData(x.Id, categories.Where(a => !a.ParentId.Equals(parentId)).ToList()),
                    Slug = x.Slug,
                    Name = x.Name
                }).ToList();
        }

        private async Task<bool> IsCheckChildCategories(Guid parentId, Guid childId)
        {
            if (childId.Equals(Guid.Empty))
            {
                return false;
            }

            var category = await _categoryProvider.GetById(childId);
            if (category.ParentCategoryId.Equals(parentId))
            {
                return true;
            }

            if (childId.Equals(parentId))
            {
                return true;
            }

            return await IsCheckChildCategories(parentId, category.ParentCategoryId);
        }
    }

}
