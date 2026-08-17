using FluentValidation;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Entities;
using FoodOrderApi.Core.Enums;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Orders.Commands.CreateOrderReview
{
    public class CreateOrderReviewCommandHandler : IRequestHandler<CreateOrderReviewCommand, CustomResponseDto<int>>
    {
        private readonly AppDbContext _context;

        public CreateOrderReviewCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<int>> Handle(CreateOrderReviewCommand request, CancellationToken cancellationToken)
        {
            // 1. Siparişi ve varsa mevcut değerlendirmesini getir
            var order = await _context.Orders
                .Include(o => o.Review)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId && !o.IsDeleted, cancellationToken);

            if (order == null)
                throw new ValidationException("Değerlendirilecek sipariş bulunamadı.");

            // 2. İş Kuralı: Sipariş bu kullanıcıya mı ait?
            if (order.UserId != request.UserId)
                throw new ValidationException("Yalnızca kendi verdiğiniz siparişleri değerlendirebilirsiniz.");

            // 3. İş Kuralı: Sipariş teslim edilmiş mi?
            if (order.Status != OrderStatus.Delivered)
                throw new ValidationException("Yalnızca teslim edilmiş (Delivered) siparişlere puan ve yorum bırakabilirsiniz.");

            // 4. İş Kuralı: Daha önce değerlendirilmiş mi?
            if (order.Review != null)
                throw new ValidationException("Bu sipariş için daha önce değerlendirme yapılmıştır.");

            // 5. Yeni değerlendirmeyi oluştur ve kaydet
            var review = new Review
            {
                OrderId = order.Id,
                RestaurantId = order.RestaurantId,
                UserId = request.UserId,
                Score = request.Score,
                Comment = request.Comment
            };

            await _context.Reviews.AddAsync(review, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponseDto<int>.Success(201, review.Id);
        }
    }
}
