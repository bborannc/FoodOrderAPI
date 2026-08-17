using FluentValidation;
using FoodOrderApi.Application.Features.Orders.Dtos;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Orders.Queries.GetOrdersByRestaurantId
{
    public class GetOrdersByRestaurantIdQueryHandler : IRequestHandler<GetOrdersByRestaurantIdQuery, CustomResponseDto<List<OrderDto>>>
    {
        private readonly AppDbContext _context;

        public GetOrdersByRestaurantIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<List<OrderDto>>> Handle(GetOrdersByRestaurantIdQuery request, CancellationToken cancellationToken)
        {
            var restaurantExists = await _context.Restaurants
                .AnyAsync(r => r.Id == request.RestaurantId && !r.IsDeleted, cancellationToken);

            if (!restaurantExists)
                throw new ValidationException("Belirtilen restoran bulunamadı.");

            // 1. Temel sorgu
            var query = _context.Orders
                .AsNoTracking()
                .Where(o => o.RestaurantId == request.RestaurantId && !o.IsDeleted);

            // 2. Opsiyonel Durum Filtresi
            if (request.Status.HasValue)
            {
                query = query.Where(o => o.Status == request.Status.Value);
            }

            // 3. Projeksiyon ve Listeleme
            var orders = await query
                .Include(o => o.Restaurant)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .OrderByDescending(o => o.CreatedDate)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    RestaurantId = o.RestaurantId,
                    RestaurantName = o.Restaurant != null ? o.Restaurant.Name : string.Empty,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status,
                    CancellationReason = o.CancellationReason,
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