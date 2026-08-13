using FluentValidation;

namespace FoodOrderApi.Application.Features.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
    {
        public CreateRestaurantCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Restoran adı boş geçilemez.")
                .NotNull().WithMessage("Restoran adı zorunludur.")
                .MaximumLength(100).WithMessage("Restoran adı en fazla 100 karakter olabilir.")
                .MinimumLength(2).WithMessage("Restoran adı en az 2 karakter olmalıdır.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Restoran adresi boş geçilemez.")
                .MaximumLength(250).WithMessage("Restoran adresi en fazla 250 karakter olabilir.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası boş geçilemez.")
                .Matches(@"^\+?[0-9\s\-]{10,15}$").WithMessage("Lütfen geçerli bir telefon numarası giriniz.");
        }
    }
}