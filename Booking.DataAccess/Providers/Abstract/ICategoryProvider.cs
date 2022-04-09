using Booking.Models;

namespace Booking.DataAccess.Providers.Abstract
{
    public interface ICategoryProvider : IProvider<Category, Guid>
    {
        Task<Category> GetBySlug(string slug);
        Task Delete(Category category);

        Task<List<Category>> GetChildren(Guid categoryId);
        Task<List<Category>> GetParents(Guid parentCategoryId);
    }

}
