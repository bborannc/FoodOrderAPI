namespace FoodOrderApi.Core.Entities
{
    public class MenuItem : BaseEntity
    {
        public int RestaurantId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }

        // Navigation Properties
        public Restaurant Restaurant { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}