namespace FoodOrderApi.Application.Features.Restaurants.Dtos
{
    public class RestaurantReviewSummaryDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<RestaurantReviewDetailDto> Reviews { get; set; } = new();
    }

    public class RestaurantReviewDetailDto
    {
        public int ReviewId { get; set; }
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}