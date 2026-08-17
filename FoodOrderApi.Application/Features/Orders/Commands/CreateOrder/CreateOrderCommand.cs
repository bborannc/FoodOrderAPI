using FoodOrderApi.Application.Features.Orders.Dtos;
using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(
        int RestaurantId,
        AddressDto DeliveryAddress,
        List<CreateOrderItemDto> Items
    ) : IRequest<CustomResponseDto<int>>;
}
