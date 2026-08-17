using FluentValidation;
using FoodOrderApi.Application.Features.Restaurants.Dtos;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.Restaurants.Queries.GetRestaurantReviews
{
    public class GetRestaurantReviewsQueryHandler : IRequestHandler<GetRestaurantReviewsQuery, CustomResponseDto<RestaurantReviewSummaryDto>>
    {
        private readonly AppDbContext _context;

        public GetRestaurantReviewsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<RestaurantReviewSummaryDto>> Handle(GetRestaurantReviewsQuery request, CancellationToken cancellationToken)
        {
            var restaurant = await _context.Restaurants
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.RestaurantId && !r.IsDeleted, cancellationToken);

            if (restaurant == null)
                throw new ValidationException("Belirtilen restoran bulunamadı.");

            var reviews = await _context.Reviews
                .AsNoTracking()
                .Where(r => r.RestaurantId == request.RestaurantId && !r.IsDeleted)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedDate)
                .Select(r => new RestaurantReviewDetailDto
                {
                    ReviewId = r.Id,
                    OrderId = r.OrderId,
                    CustomerName = r.User != null ? r.User.FullName : "Anonim Müşteri",
                    Score = r.Score,
                    Comment = r.Comment,
                    CreatedDate = r.CreatedDate
                })
                .ToListAsync(cancellationToken);

            var averageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Score), 1) : 0.0;

            var summary = new RestaurantReviewSummaryDto
            {
                RestaurantId = restaurant.Id,
                RestaurantName = restaurant.Name,
                AverageRating = averageRating,
                TotalReviews = reviews.Count,
                Reviews = reviews
            };

            return CustomResponseDto<RestaurantReviewSummaryDto>.Success(200, summary);
        }
    }
}
