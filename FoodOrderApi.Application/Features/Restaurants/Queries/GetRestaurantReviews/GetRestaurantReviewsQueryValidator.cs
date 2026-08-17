using FluentValidation;

namespace FoodOrderApi.Application.Features.Restaurants.Queries.GetRestaurantReviews
{
    public class GetRestaurantReviewsQueryValidator : AbstractValidator<GetRestaurantReviewsQuery>
    {
        public GetRestaurantReviewsQueryValidator()
        {
            RuleFor(x => x.RestaurantId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir restoran ID'si belirtilmelidir.");
        }
    }
}
