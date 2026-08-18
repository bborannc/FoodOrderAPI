using FoodOrderApi.Application.Features.Orders.Dtos;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Enums;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Orders.Queries.GetAvailableOrdersForCourier
{
    public class GetAvailableOrdersForCourierQueryHandler : IRequestHandler<GetAvailableOrdersForCourierQuery, CustomResponseDto<List<CourierOrderDto>>>
    {
        private readonly AppDbContext _context;

        public GetAvailableOrdersForCourierQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<List<CourierOrderDto>>> Handle(GetAvailableOrdersForCourierQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Restaurant)
                .Include(o => o.User)
                .Where(o => o.Status == OrderStatus.Preparing && o.CourierId == null && !o.IsDeleted)
                .OrderBy(o => o.CreatedDate)
                .Select(o => new CourierOrderDto
                {
                    Id = o.Id,
                    RestaurantId = o.RestaurantId,
                    RestaurantName = o.Restaurant != null ? o.Restaurant.Name : string.Empty,
                    RestaurantAddress = o.Restaurant != null ? o.Restaurant.Address : string.Empty,
                    RestaurantPhoneNumber = o.Restaurant != null ? o.Restaurant.PhoneNumber : string.Empty,
                    CustomerName = o.User != null ? o.User.FullName : "Müşteri",
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    StatusName = o.Status.ToString(),
                    CreatedDate = o.CreatedDate,
                    City = o.DeliveryAddress.City,
                    District = o.DeliveryAddress.District,
                    Neighborhood = o.DeliveryAddress.Neighborhood,
                    Street = o.DeliveryAddress.Street,
                    BuildingNumber = o.DeliveryAddress.BuildingNumber,
                    DoorNumber = o.DeliveryAddress.DoorNumber,
                    AddressDirections = o.DeliveryAddress.AddressDirections
                })
                .ToListAsync(cancellationToken);

            return CustomResponseDto<List<CourierOrderDto>>.Success(200, orders);
        }
    }
}
