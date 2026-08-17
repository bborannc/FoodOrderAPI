using FoodOrderApi.Application.Features.Orders.Dtos;
using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.Orders.Queries.GetMyOrders
{
    public record GetMyOrdersQuery : IRequest<CustomResponseDto<List<OrderDto>>>;
}