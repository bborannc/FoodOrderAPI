using System.Text.Json.Serialization;
using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.Orders.Commands.CreateOrderReview
{
    public record CreateOrderReviewCommand(
        int Score,
        string? Comment = null
    ) : IRequest<CustomResponseDto<int>>
    {
        [JsonIgnore]
        public int OrderId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
