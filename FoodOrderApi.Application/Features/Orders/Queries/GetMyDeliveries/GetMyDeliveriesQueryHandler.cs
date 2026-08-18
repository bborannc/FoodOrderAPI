using FluentValidation;
using FoodOrderApi.Application.Features.Orders.Dtos;
using FoodOrderApi.Application.Security;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Enums;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Orders.Queries.GetMyDeliveries
{
    public class GetMyDeliveriesQueryHandler : IRequestHandler<GetMyDeliveriesQuery, CustomResponseDto<List<CourierOrderDto>>>
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetMyDeliveriesQueryHandler(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CustomResponseDto<List<CourierOrderDto>>> Handle(GetMyDeliveriesQuery request, CancellationToken cancellationToken)
        {
            var courierId = _currentUserService.UserId;
            if (courierId == null)
                throw new ValidationException("Kurye kimliği doğrulanamadı.");

            var query = _context.Orders
                .AsNoTracking()
                .Include(o => o.Restaurant)
                .Include(o => o.User)
                .Where(o => o.CourierId == courierId.Value && !o.IsDeleted);

            // Eğer kurye belirli bir duruma göre filtrelemek isterse (örneğin sadece InTransit veya sadece Delivered)
            if (request.Status.HasValue)
            {
                query = query.Where(o => o.Status == request.Status.Value);
            }
            else
            {
                // Varsayılan olarak kuryenin aktif (InTransit) ve tamamlanmış (Delivered) siparişlerini getir
                query = query.Where(o => o.Status == OrderStatus.InTransit || o.Status == OrderStatus.Delivered);
            }

            var deliveries = await query
                .OrderByDescending(o => o.CreatedDate)
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

            return CustomResponseDto<List<CourierOrderDto>>.Success(200, deliveries);
        }
    }
}