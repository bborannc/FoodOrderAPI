using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Restaurants.Queries.GetRestaurants
{
    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, CustomResponseDto<List<RestaurantDto>>>
    {
        private readonly AppDbContext _context;

        public GetRestaurantsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<List<RestaurantDto>>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            // 1. AsNoTracking() ile ChangeTracker'ı kapatıp yüksek performanslı okuma yapıyoruz
            // 2. AppDbContext'e yazdığımız HasQueryFilter sayesinde silinmiş (IsDeleted = true) veriler otomatik elenir
            var restaurants = await _context.Restaurants
                .AsNoTracking()
                .Select(r => new RestaurantDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    PhoneNumber = r.PhoneNumber,
                    IsActive = r.IsActive
                })
                .ToListAsync(cancellationToken);

            // 3. 200 OK ile DTO listesini sarmalayıp dönüyoruz
            return CustomResponseDto<List<RestaurantDto>>.Success(200, restaurants);
        }
    }
}
