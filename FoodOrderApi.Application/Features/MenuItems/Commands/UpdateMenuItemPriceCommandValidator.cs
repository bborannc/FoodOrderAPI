using FluentValidation;

namespace FoodOrderApi.Application.Features.MenuItems.Commands.UpdateMenuItemPrice
{
    public class UpdateMenuItemPriceCommandValidator : AbstractValidator<UpdateMenuItemPriceCommand>
    {
        public UpdateMenuItemPriceCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçersiz ürün kimliği.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Yeni fiyat 0'dan büyük olmalıdır.");
        }
    }
}
