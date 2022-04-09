using Booking.DataAccess.Providers.Abstract;
using Booking.Models;
using Microsoft.EntityFrameworkCore;


namespace Booking.DataAccess.Providers.EntityProviders
{
    public class EntityCategoryProvider : EntityProvider<ApplicationContext, Category, Guid>, ICategoryProvider
    {
        private readonly ApplicationContext _context;

        public EntityCategoryProvider(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Category> GetBySlug(string slug)
        => await FirstOrDefault(x => x.Slug.ToLower().Equals(slug.ToLower()));


        public async Task Delete(Category category)
        {
            var categories = await Get(x => x.ParentCategoryId == category.Id);
            foreach (var variableCategory in categories)
            {
                var categ = await GetById(variableCategory.Id);
                categ.ParentCategoryId = category.ParentCategoryId;
            }

            await _context.SaveChangesAsync();
            await Remove(category);
        }

        public async Task<List<Category>> GetChildren(Guid categoryId)
        {
            var children = await _context.Categories
                .Where(x => x.ParentCategoryId.Equals(categoryId)).ToListAsync();

            foreach (var child in children.ToArray())
            {
                children.AddRange(await GetChildren(child.Id));
            }

            return children;
        }

        public async Task<List<Category>> GetParents(Guid parentCategoryId)
        {
            var parents = new List<Category>();

            var parent = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id.Equals(parentCategoryId));

            if (parent is null)
            {
                parents.Add(new Category
                {
                    Id = Guid.Empty
                });

                return parents;
            }

            parents.Add(parent);

            if (!parentCategoryId.Equals(Guid.Empty))
            {
                parents.AddRange(await GetParents(parent.ParentCategoryId));
            }

            return parents;
        }
    }

}
