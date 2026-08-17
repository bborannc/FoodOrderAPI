namespace FoodOrderApi.Core.Entities
{
    public class Review : BaseEntity
    {
        public int Score { get; set; } // 1 - 10 arası puan
        public string? Comment { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
