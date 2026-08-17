using FluentValidation;
using FoodOrderApi.Application.Features.Orders.Dtos;
using FoodOrderApi.Application.Security;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, CustomResponseDto<List<OrderDto>>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetMyOrdersQueryHandler(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CustomResponseDto<List<OrderDto>>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
                throw new ValidationException("Kullanıcı kimliği doğrulanamadı.");

            var orders = await _context.Orders
                .AsNoTracking()
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Where(o => o.UserId == userId.Value && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedDate)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    RestaurantId = o.RestaurantId,
                    RestaurantName = o.Restaurant != null ? o.Restaurant.Name : string.Empty,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    CreatedDate = o.CreatedDate,
                    DeliveryAddress = new AddressDto
                    {
                        City = o.DeliveryAddress.City,
                        District = o.DeliveryAddress.District,
                        Neighborhood = o.DeliveryAddress.Neighborhood,
                        Street = o.DeliveryAddress.Street,
                        BuildingNumber = o.DeliveryAddress.BuildingNumber,
                        DoorNumber = o.DeliveryAddress.DoorNumber,
                        AddressDirections = o.DeliveryAddress.AddressDirections
                    },
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        MenuItemId = oi.MenuItemId,
                        MenuItemName = oi.MenuItem != null ? oi.MenuItem.Name : string.Empty,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return CustomResponseDto<List<OrderDto>>.Success(200, orders);
        }
    }
}
