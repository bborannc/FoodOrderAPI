using FluentValidation;

namespace FoodOrderApi.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.RestaurantId)
                .GreaterThan(0).WithMessage("Geçerli bir restoran seçilmelidir.");

            RuleFor(x => x.DeliveryAddress).NotNull().WithMessage("Teslimat adresi boş olamaz.");

            When(x => x.DeliveryAddress != null, () =>
            {
                RuleFor(x => x.DeliveryAddress.City).NotEmpty().WithMessage("Şehir alanı zorunludur.");
                RuleFor(x => x.DeliveryAddress.District).NotEmpty().WithMessage("İlçe alanı zorunludur.");
                RuleFor(x => x.DeliveryAddress.Neighborhood).NotEmpty().WithMessage("Mahalle alanı zorunludur.");
                RuleFor(x => x.DeliveryAddress.Street).NotEmpty().WithMessage("Sokak/Cadde alanı zorunludur.");
                RuleFor(x => x.DeliveryAddress.BuildingNumber).NotEmpty().WithMessage("Bina no zorunludur.");
                RuleFor(x => x.DeliveryAddress.DoorNumber).NotEmpty().WithMessage("Daire/Kapı no zorunludur.");
            });

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Sipariş sepeti boş olamaz.")
                .Must(items => items != null && items.Count > 0).WithMessage("En az bir ürün eklenmelidir.");

            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.MenuItemId).GreaterThan(0).WithMessage("Geçersiz ürün kimliği.");
                items.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Ürün adedi en az 1 olmalıdır.");
            });
        }
    }
}