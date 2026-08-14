using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.MenuItems.Queries.GetMenuItemsByRestaurantId
{
    public record GetMenuItemsByRestaurantIdQuery(int RestaurantId) : IRequest<CustomResponseDto<List<MenuItemDto>>>;
}
