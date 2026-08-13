using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Entities;
using FoodOrderApi.Data.Context;
using MediatR;

namespace FoodOrderApi.Application.Features.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, CustomResponseDto<int>>
    {
        private readonly AppDbContext _context;

        // DB Bağlantısını Dependency Injection ile alıyoruz
        public CreateRestaurantCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<int>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            // 1. Gelen Command isteğini Domain Varlığına (Entity) dönüştürüyoruz
            var restaurant = new Restaurant
            {
                Name = request.Name,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                IsActive = true
            };

            // 2. Veritabanına ekliyoruz
            await _context.Restaurants.AddAsync(restaurant, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // 3. Başarılı yanıt ve yeni oluşan Restoran ID'sini dönüyoruz
            return CustomResponseDto<int>.Success(201, restaurant.Id);
        }
    }
}
