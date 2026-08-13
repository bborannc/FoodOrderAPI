using FoodOrderApi.Core.Enums;

namespace FoodOrderApi.Core.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; } // Siparişi veren kullanıcının Id'si
        public string DeliveryAddress { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Navigation Properties
        public User User { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
