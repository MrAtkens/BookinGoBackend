using System.ComponentModel.DataAnnotations;

namespace Booking.Contracts.Parameters
{
    public class CategoryParameter
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public Guid ParentId { get; set; }

        [Required(ErrorMessage = "Slug для категории обязателен, он служит для построения URL")]
        public string Slug { get; set; }
    }

}
