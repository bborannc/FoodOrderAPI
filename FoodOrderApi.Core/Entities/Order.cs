using FoodOrderApi.Core.Enums;
using FoodOrderApi.Core.ValueObjects;

namespace FoodOrderApi.Core.Entities
{
    public class Order : BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? CancellationReason { get; set; }

        // Value Object olarak Teslimat Adresi
        public Address DeliveryAddress { get; set; } = new();

        // Kullanıcı İlişkisi
        public int UserId { get; set; }
        public User? User { get; set; }

        // Restoran İlişkisi
        public int RestaurantId { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        public Review? Review { get; set; }

        public int? CourierId { get; set; }
        public User? Courier { get; set; }

        // Sipariş Kalemleri
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}