using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.Restaurants.Queries.GetRestaurants
{
    // MediatR'a diyoruz ki: "Bu bir Query isteğidir ve geriye List<RestaurantDto> döner."
    public class GetRestaurantsQuery : IRequest<CustomResponseDto<List<RestaurantDto>>>
    {
        // Tüm listeyi getireceğimiz için parametresizdir.
    }
}
