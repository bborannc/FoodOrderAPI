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
                .Include(o => o.Restaurant)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId && !o.IsDeleted, cancellationToken);

            if (order == null)
                throw new ValidationException("Sipariş bulunamadı.");

            if (order.Status == request.Status)
                throw new ValidationException($"Sipariş zaten '{order.Status}' durumundadır.");

            // Tamamlanmış veya iptal edilmiş siparişin durumu değiştirilemez
            if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled)
                throw new ValidationException("Teslim edilmiş veya iptal edilmiş siparişlerin durumu değiştirilemez.");

            // -------------------------------------------------------------
            // Rol Bazlı Durum Geçiş Kuralları (State Machine Constraints)
            // -------------------------------------------------------------
            var role = request.CurrentUserRole;

            if (role == nameof(UserRole.Courier))
            {
                // Kurye sadece InTransit (3) ve Delivered (4) yapabilir
                if (request.Status != OrderStatus.InTransit && request.Status != OrderStatus.Delivered)
                    throw new ValidationException("Kuryeler siparişi yalnızca 'Kuryede/Yolda' veya 'Teslim Edildi' durumuna geçirebilir.");

                // 2 -> 3 (Siparişi Teslim Alma / Yola Çıkma)
                if (request.Status == OrderStatus.InTransit)
                {
                    if (order.Status != OrderStatus.Preparing)
                        throw new ValidationException("Yalnızca 'Hazırlanıyor' aşamasındaki siparişler kurye tarafından teslim alınabilir.");

                    if (order.CourierId.HasValue && order.CourierId != request.CurrentUserId)
                        throw new ValidationException("Bu sipariş başka bir kurye tarafından teslim alınmış.");

                    order.CourierId = request.CurrentUserId;
                }

                // 3 -> 4 (Müşteriye Teslim Etme)
                if (request.Status == OrderStatus.Delivered)
                {
                    if (order.Status != OrderStatus.InTransit)
                        throw new ValidationException("Sipariş teslim edilmeden önce 'Kuryede/Yolda' aşamasında olmalıdır.");

                    if (order.CourierId != request.CurrentUserId)
                        throw new ValidationException("Bu sipariş sizin üzerinize zimmetli değil, teslim edemezsiniz.");
                }
            }
            else if (role == nameof(UserRole.RestaurantOwner))
            {
                // Restoran sahibi yalnızca kendi restoranının siparişini yönetebilir
                if (order.Restaurant != null && order.Restaurant.UserId != request.CurrentUserId)
                    throw new ValidationException("Bu siparişi yönetme yetkiniz bulunmamaktadır.");

                // Restoran sahibi InTransit veya Delivered yapamaz
                if (request.Status == OrderStatus.InTransit || request.Status == OrderStatus.Delivered)
                    throw new ValidationException("Siparişi yola çıkarma ve teslim etme yetkisi yalnızca Kuryelere aittir.");

                // Restoran sahibi Pending -> Preparing veya İptal yapabilir
                if (request.Status == OrderStatus.Preparing && order.Status != OrderStatus.Pending)
                    throw new ValidationException("Yalnızca 'Bekleyen' siparişler 'Hazırlanıyor' durumuna alınabilir.");

                if (request.Status == OrderStatus.Cancelled && (order.Status == OrderStatus.InTransit || order.Status == OrderStatus.Delivered))
                    throw new ValidationException("Yola çıkmış veya teslim edilmiş siparişler iptal edilemez.");
            }
            else if (role == nameof(UserRole.Customer))
            {
                // Müşteri yalnızca henüz hazırlanmaya başlamamış siparişini iptal edebilir
                if (request.Status != OrderStatus.Cancelled)
                    throw new ValidationException("Müşteriler sipariş durumunu yalnızca iptal edebilir.");

                if (order.UserId != request.CurrentUserId)
                    throw new ValidationException("Yalnızca kendi siparişinizi iptal edebilirsiniz.");

                if (order.Status != OrderStatus.Pending)
                    throw new ValidationException("Hazırlanmaya başlanmış veya yola çıkmış siparişler müşteri tarafından iptal edilemez.");
            }
            // Admin için tüm geçişler serbesttir

            // İptal gerekçesi yönetimi
            if (request.Status == OrderStatus.Cancelled)
            {
                order.CancellationReason = string.IsNullOrWhiteSpace(request.CancellationReason)
                    ? "Belirtilmedi"
                    : request.CancellationReason;
            }

            order.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponseDto<NoContentDto>.Success(204);
        }
    }
}