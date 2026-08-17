using FoodOrderApi.Core.Enums;

namespace FoodOrderApi.Application.Features.Orders.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public string? CancellationReason { get; set; } // <-- EKLENDİ
        public DateTime CreatedDate { get; set; }
        public AddressDto DeliveryAddress { get; set; } = new();
        public List<OrderItemDto> Items { get; set; } = new();
    }

    
}