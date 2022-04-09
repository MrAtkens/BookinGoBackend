namespace Booking.Contracts.Responses
{
    public class CategoryAllResponse
    {
        public int Count { get; set; }
        public List<CategoryResponse> Data { get; set; }
    }

}
