using FoodOrderApi.Application.Features.Orders.Commands.CreateOrder;
using FoodOrderApi.Application.Features.Orders.Commands.CreateOrderReview;
using FoodOrderApi.Application.Features.Orders.Commands.UpdateOrderStatus;
using FoodOrderApi.Application.Features.Orders.Queries.GetAvailableOrdersForCourier;
using FoodOrderApi.Application.Features.Orders.Queries.GetMyDeliveries;
using FoodOrderApi.Application.Features.Orders.Queries.GetMyOrders;
using FoodOrderApi.Application.Features.Orders.Queries.GetOrdersByRestaurantId;
using FoodOrderApi.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodOrderApi.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var result = await _mediator.Send(new GetMyOrdersQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{orderId:int}/status")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] UpdateOrderStatusCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(roleClaim))
                return Unauthorized();

            command.OrderId = orderId;
            command.CurrentUserId = int.Parse(userIdClaim);
            command.CurrentUserRole = roleClaim;

            var result = await _mediator.Send(command);

            if (result.StatusCode == 204)
                return NoContent();

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("restaurant/{restaurantId}")]
        [Authorize(Roles = "RestaurantOwner,Admin")]
        public async Task<IActionResult> GetOrdersByRestaurant(
            [FromRoute] int restaurantId,
            [FromQuery] OrderStatus? status = null)
        {
            var response = await _mediator.Send(new GetOrdersByRestaurantIdQuery(restaurantId, status));

            if (response.StatusCode == 204)
                return NoContent();

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("courier/available")]
        [Authorize(Roles = "Courier,Admin")]
        public async Task<IActionResult> GetAvailableOrdersForCourier()
        {
            var response = await _mediator.Send(new GetAvailableOrdersForCourierQuery());
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("{orderId}/review")]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> CreateOrderReview(
            [FromRoute] int orderId,
            [FromBody] CreateOrderReviewCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            command.OrderId = orderId;
            command.UserId = int.Parse(userIdClaim);

            var response = await _mediator.Send(command);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("courier/my-deliveries")]
        [Authorize(Roles = "Courier,Admin")]
        public async Task<IActionResult> GetMyDeliveries([FromQuery] OrderStatus? status = null)
        {
            var response = await _mediator.Send(new GetMyDeliveriesQuery(status));
            return StatusCode(response.StatusCode, response);
        }
    }
}