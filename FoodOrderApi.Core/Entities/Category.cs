namespace FoodOrderApi.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        // Navigation Property: Bir kategoride birden fazla menü elemanı bulunabilir (1:N)
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}