namespace FoodOrderApi.Core.ValueObjects
{
    public class Address
    {
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string DoorNumber { get; set; } = string.Empty;
        public string? AddressDirections { get; set; }
    }
}