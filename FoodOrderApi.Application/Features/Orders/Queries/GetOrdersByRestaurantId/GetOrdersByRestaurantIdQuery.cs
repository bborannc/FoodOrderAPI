using FoodOrderApi.Application.Features.Orders.Dtos;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Enums;
using MediatR;

namespace FoodOrderApi.Application.Features.Orders.Queries.GetOrdersByRestaurantId
{
    public record GetOrdersByRestaurantIdQuery(
        int RestaurantId,
        OrderStatus? Status = null
    ) : IRequest<CustomResponseDto<List<OrderDto>>>;
}
