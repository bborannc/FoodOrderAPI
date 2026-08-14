namespace FoodOrderApi.Core.Entities
{
    public class MenuItem : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        // EKLENEN ALANLAR:
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;

        // İlişkiler (Foreign Keys & Navigation Properties)
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}