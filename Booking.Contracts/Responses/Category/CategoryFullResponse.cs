using System.ComponentModel.DataAnnotations;

namespace Booking.Contracts.Responses.Category
{
    public class CategoryFullResponse
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public List<CategoryFullResponse> Children { get; set; }

        [Display(Name = "Слаг")] 
        public string Slug { get; set; }
        [Display(Name = "Наименование")] 
        public string Name { get; set; }
    }

}
