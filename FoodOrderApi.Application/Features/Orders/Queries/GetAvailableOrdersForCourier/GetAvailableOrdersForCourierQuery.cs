using FoodOrderApi.Application.Features.Orders.Dtos;
using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.Orders.Queries.GetAvailableOrdersForCourier
{
    public record GetAvailableOrdersForCourierQuery() : IRequest<CustomResponseDto<List<CourierOrderDto>>>;
}
