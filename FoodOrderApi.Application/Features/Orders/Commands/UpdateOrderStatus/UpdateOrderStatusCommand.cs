using System.Text.Json.Serialization;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Enums;
using MediatR;

namespace FoodOrderApi.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(
        OrderStatus Status,
        string? Reason = null
    ) : IRequest<CustomResponseDto<NoContentDto>>
    {
        [JsonIgnore]
        public int OrderId { get; set; }
    }
}
