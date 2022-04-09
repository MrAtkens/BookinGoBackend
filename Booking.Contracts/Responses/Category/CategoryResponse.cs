
namespace Booking.Contracts.Responses
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public Guid ParentId { get; set; }
    
    }

}
