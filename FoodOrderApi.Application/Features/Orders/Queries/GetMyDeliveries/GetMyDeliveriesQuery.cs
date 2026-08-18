using FoodOrderApi.Application.Features.Orders.Dtos;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Enums;
using MediatR;

namespace FoodOrderApi.Application.Features.Orders.Queries.GetMyDeliveries
{
    public record GetMyDeliveriesQuery(OrderStatus? Status = null) : IRequest<CustomResponseDto<List<CourierOrderDto>>>;
}
