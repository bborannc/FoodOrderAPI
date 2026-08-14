using FluentValidation;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.Application.Features.MenuItems.Commands.UpdateMenuItemPrice
{
    public class UpdateMenuItemPriceCommandHandler : IRequestHandler<UpdateMenuItemPriceCommand, CustomResponseDto<NoContentDto>>
    {
        private readonly AppDbContext _context;

        public UpdateMenuItemPriceCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CustomResponseDto<NoContentDto>> Handle(UpdateMenuItemPriceCommand request, CancellationToken cancellationToken)
        {
            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == request.Id && !m.IsDeleted, cancellationToken);

            if (menuItem == null)
                throw new ValidationException("Güncellenecek menü ürünü bulunamadı.");

            menuItem.Price = request.Price;
            menuItem.IsAvailable = request.IsAvailable;

            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponseDto<NoContentDto>.Success(204);
        }
    }
}
