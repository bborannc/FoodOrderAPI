using FoodOrderApi.Core.Enums;

namespace FoodOrderApi.Application.Features.Orders.Dtos
{
    public class CourierOrderDto
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public string RestaurantAddress { get; set; } = string.Empty;
        public string RestaurantPhoneNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        // Teslimat Adresi Detayları
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string? DoorNumber { get; set; }
        public string? AddressDirections { get; set; }
    }
}
