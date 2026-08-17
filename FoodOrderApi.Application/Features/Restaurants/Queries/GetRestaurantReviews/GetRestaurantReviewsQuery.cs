using FoodOrderApi.Application.Features.Restaurants.Dtos;
using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.Restaurants.Queries.GetRestaurantReviews
{
    public record GetRestaurantReviewsQuery(int RestaurantId) : IRequest<CustomResponseDto<RestaurantReviewSummaryDto>>;
}
