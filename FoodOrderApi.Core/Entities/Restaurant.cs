namespace FoodOrderApi.Core.Entities
{
    public class Restaurant : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        // Navigation Property: Bire-Çok (1:N) İlişki
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
