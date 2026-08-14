using FluentValidation;

namespace FoodOrderApi.Application.Features.MenuItems.Commands.CreateMenuItem
{
    public class CreateMenuItemCommandValidator : AbstractValidator<CreateMenuItemCommand>
    {
        public CreateMenuItemCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Yemek adı boş olamaz.")
                .MaximumLength(100).WithMessage("Yemek adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Yemek fiyatı 0'dan büyük olmalıdır.");

            RuleFor(x => x.RestaurantId)
                .GreaterThan(0).WithMessage("Geçerli bir restoran seçilmelidir.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir.");
        }
    }
}
