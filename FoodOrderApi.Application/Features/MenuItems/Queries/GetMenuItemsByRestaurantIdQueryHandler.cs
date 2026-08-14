using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.MenuItems.Queries.GetMenuItemsByRestaurantId
{
    public class GetMenuItemsByRestaurantIdQueryHandler : IRequestHandler<GetMenuItemsByRestaurantIdQuery, CustomResponseDto<List<MenuItemDto>>>
    {
        private readonly AppDbContext _context;

        public GetMenuItemsByRestaurantIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<List<MenuItemDto>>> Handle(GetMenuItemsByRestaurantIdQuery request, CancellationToken cancellationToken)
        {
            // AsNoTracking ile performanslı okuma yapıyoruz (Tracking overhead yok)
            var menuItems = await _context.MenuItems
                .AsNoTracking()
                .Include(m => m.Category)
                .Where(m => m.RestaurantId == request.RestaurantId && !m.IsDeleted)
                .Select(m => new MenuItemDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    ImageUrl = m.ImageUrl,
                    IsAvailable = m.IsAvailable,
                    RestaurantId = m.RestaurantId,
                    CategoryId = m.CategoryId,
                    CategoryName = m.Category != null ? m.Category.Name : string.Empty
                })
                .ToListAsync(cancellationToken);

            return CustomResponseDto<List<MenuItemDto>>.Success(200, menuItems);
        }
    }
}
