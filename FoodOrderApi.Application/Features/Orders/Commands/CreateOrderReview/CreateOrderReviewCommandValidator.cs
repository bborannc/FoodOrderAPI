using FluentValidation;

namespace FoodOrderApi.Application.Features.Orders.Commands.CreateOrderReview
{
    public class CreateOrderReviewCommandValidator : AbstractValidator<CreateOrderReviewCommand>
    {
        public CreateOrderReviewCommandValidator()
        {
            RuleFor(x => x.Score)
                .InclusiveBetween(1, 10)
                .WithMessage("Puan 1 ile 10 arasında olmalıdır.");

            RuleFor(x => x.Comment)
                .MaximumLength(500)
                .WithMessage("Yorum en fazla 500 karakter olabilir.");
        }
    }
}
