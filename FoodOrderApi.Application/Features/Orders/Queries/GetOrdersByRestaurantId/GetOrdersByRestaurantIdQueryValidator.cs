using FluentValidation;

namespace FoodOrderApi.Application.Features.Orders.Queries.GetOrdersByRestaurantId
{
    public class GetOrdersByRestaurantIdQueryValidator : AbstractValidator<GetOrdersByRestaurantIdQuery>
    {
        public GetOrdersByRestaurantIdQueryValidator()
        {
            RuleFor(x => x.RestaurantId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir restoran ID'si belirtilmelidir.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .When(x => x.Status.HasValue)
                .WithMessage("Geçersiz sipariş durumu belirtildi.");
        }
    }
}
