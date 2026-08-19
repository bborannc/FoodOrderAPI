using FluentValidation;

namespace FoodOrderApi.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.RestaurantId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir restoran seçilmelidir.");

            RuleFor(x => x.DeliveryAddress)
                .NotNull()
                .WithMessage("Teslimat adresi zorunludur.");

            When(x => x.DeliveryAddress != null, () =>
            {
                RuleFor(x => x.DeliveryAddress.City)
                    .NotEmpty().WithMessage("Şehir alanı zorunludur.")
                    .MaximumLength(50).WithMessage("Şehir en fazla 50 karakter olabilir.");

                RuleFor(x => x.DeliveryAddress.District)
                    .NotEmpty().WithMessage("İlçe alanı zorunludur.")
                    .MaximumLength(50).WithMessage("İlçe en fazla 50 karakter olabilir.");

                RuleFor(x => x.DeliveryAddress.Neighborhood)
                    .NotEmpty().WithMessage("Mahalle alanı zorunludur.")
                    .MaximumLength(100).WithMessage("Mahalle en fazla 100 karakter olabilir.");

                RuleFor(x => x.DeliveryAddress.Street)
                    .NotEmpty().WithMessage("Sokak/Cadde alanı zorunludur.")
                    .MaximumLength(150).WithMessage("Sokak en fazla 150 karakter olabilir.");

                RuleFor(x => x.DeliveryAddress.BuildingNumber)
                    .NotEmpty().WithMessage("Bina numarası zorunludur.")
                    .MaximumLength(20).WithMessage("Bina no en fazla 20 karakter olabilir.");

                RuleFor(x => x.DeliveryAddress.DoorNumber)
                    .NotEmpty().WithMessage("Daire/Kapı numarası zorunludur.")
                    .MaximumLength(20).WithMessage("Kapı no en fazla 20 karakter olabilir.");

                RuleFor(x => x.DeliveryAddress.AddressDirections)
                    .MaximumLength(250).WithMessage("Adres tarifi en fazla 250 karakter olabilir.");
            });

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Sipariş sepeti boş olamaz.")
                .Must(items => items != null && items.Count > 0).WithMessage("En az bir ürün eklenmelidir.")
                .Must(items => items == null || items.Select(i => i.MenuItemId).Distinct().Count() == items.Count)
                .WithMessage("Sepette aynı üründen birden fazla satır bulunamaz, lütfen adet artırımı yapınız.");

            RuleForEach(x => x.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.MenuItemId)
                    .GreaterThan(0)
                    .WithMessage("Geçersiz ürün kimliği.");

                items.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Ürün adedi en az 1 olmalıdır.")
                    .LessThanOrEqualTo(100).WithMessage("Tek bir siparişte aynı üründen en fazla 100 adet sipariş verilebilir.");
            });
        }
    }
}