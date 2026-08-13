using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.Restaurants.Commands.CreateRestaurant
{
    // MediatR'a diyoruz ki: "Bu bir Command isteğidir ve işlem bitince geriye CustomResponseDto<int> (yeni eklenen ID) dönecektir."
    public class CreateRestaurantCommand : IRequest<CustomResponseDto<int>>
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
