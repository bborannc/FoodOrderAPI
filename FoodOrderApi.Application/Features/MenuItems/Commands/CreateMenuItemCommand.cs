using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.MenuItems.Commands.CreateMenuItem
{
    public record CreateMenuItemCommand(
        string Name,
        string? Description,
        decimal Price,
        string? ImageUrl,
        int RestaurantId,
        int CategoryId
    ) : IRequest<CustomResponseDto<int>>;
}
