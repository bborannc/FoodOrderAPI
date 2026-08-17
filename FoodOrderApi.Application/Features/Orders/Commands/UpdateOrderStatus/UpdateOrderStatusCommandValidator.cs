using FluentValidation;

namespace FoodOrderApi.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Geçerli bir sipariş numarası belirtilmelidir.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Geçersiz sipariş durumu seçildi.");
        }
    }
}
