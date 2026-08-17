using FluentValidation;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Enums;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, CustomResponseDto<NoContentDto>>
    {
        private readonly AppDbContext _context;

        public UpdateOrderStatusCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<NoContentDto>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == request.OrderId && !o.IsDeleted, cancellationToken);

            if (order == null)
                throw new ValidationException("Güncellenecek sipariş bulunamadı.");

            if (order.Status == OrderStatus.Cancelled)
                throw new ValidationException("İptal edilmiş bir siparişin durumu değiştirilemez.");

            if (order.Status == OrderStatus.Delivered)
                throw new ValidationException("Teslim edilmiş bir siparişin durumu değiştirilemez.");

            if (request.Status == OrderStatus.Cancelled)
            {
                if (order.Status == OrderStatus.InTransit)
                    throw new ValidationException("Sipariş kuryeye teslim edilmiş ve yola çıkmıştır. Bu aşamadan sonra iptal edilemez.");

                order.CancellationReason = request.Reason ?? "Kullanıcı/Restoran tarafından iptal edildi.";
            }

            order.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponseDto<NoContentDto>.Success(204);
        }
    }
}