using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Models
{
    public class Category : Entity
    {
        [ForeignKey("ParentCategoryId")]
        public Guid ParentCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public string Alt { get; set; }
        public string Slug { get; set; }

    }
}   
