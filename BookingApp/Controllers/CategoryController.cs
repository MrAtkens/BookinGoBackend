using Booking.Contracts.Responses;
using Booking.Contracts.Responses.Category;
using Booking.DataAccess.Providers.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Route("api/client/categories")]
    public class CategoryController : Controller
    {

        private readonly ICategoryProvider _categoryProvider;

        public CategoryController(
            ICategoryProvider categoryProvider)
        {
            _categoryProvider = categoryProvider;
        }

        // GET
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
        private List<CategoryFullResponse> InitTreeData(Guid parentId, List<CategoryResponse> categories)
        {
            return categories.Where(x => x.ParentId.Equals(parentId))
                .Select(x => new CategoryFullResponse
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    Children = InitTreeData(x.Id, categories.Where(a => !a.ParentId.Equals(parentId)).ToList()),
                    Name = x.Name
                }).ToList();
        }
    }

}
