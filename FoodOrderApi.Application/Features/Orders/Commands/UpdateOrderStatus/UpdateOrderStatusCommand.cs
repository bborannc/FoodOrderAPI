using System.Text.Json.Serialization;
using FoodOrderApi.Application.Security;
using FoodOrderApi.Core.Constants;
using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Enums;
using MediatR;

namespace FoodOrderApi.Application.Features.Orders.Commands.UpdateOrderStatus
{
    [HasPermission(Permissions.Orders.UpdateStatus)]
    public record UpdateOrderStatusCommand(
        OrderStatus Status,
        string? CancellationReason = null
    ) : IRequest<CustomResponseDto<NoContentDto>>
    {
        [JsonIgnore]
        public int OrderId { get; set; }

        [JsonIgnore]
        public int CurrentUserId { get; set; }

        [JsonIgnore]
        public string CurrentUserRole { get; set; } = string.Empty;
    }
}
