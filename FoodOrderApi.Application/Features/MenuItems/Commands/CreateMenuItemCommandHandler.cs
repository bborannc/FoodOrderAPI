using FluentValidation;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Entities;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.MenuItems.Commands.CreateMenuItem
{
    public class CreateMenuItemCommandHandler : IRequestHandler<CreateMenuItemCommand, CustomResponseDto<int>>
    {
        private readonly AppDbContext _context;

        public CreateMenuItemCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<int>> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
        {
            // 1. Restoran kontrolü
            var restaurantExists = await _context.Restaurants
                .AnyAsync(r => r.Id == request.RestaurantId && !r.IsDeleted, cancellationToken);

            if (!restaurantExists)
                throw new ValidationException("Belirtilen restoran bulunamadı.");

            // 2. Kategori kontrolü
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == request.CategoryId && !c.IsDeleted, cancellationToken);

            if (!categoryExists)
                throw new ValidationException("Belirtilen kategori bulunamadı.");

            var menuItem = new MenuItem
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                RestaurantId = request.RestaurantId,
                CategoryId = request.CategoryId,
                IsAvailable = true
            };

            await _context.MenuItems.AddAsync(menuItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponseDto<int>.Success(201, menuItem.Id);
        }
    }
}
