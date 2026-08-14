using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.MenuItems.Commands.UpdateMenuItemPrice
{
    public record UpdateMenuItemPriceCommand(
        int Id,
        decimal Price,
        bool IsAvailable
    ) : IRequest<CustomResponseDto<NoContentDto>>;
}
