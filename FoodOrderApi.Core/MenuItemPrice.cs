namespace FoodOrderApi.Core.Entities
{
    public class MenuItemPrice : BaseEntity
    {
        public int MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }

        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; } = true;
    }
}
