using FoodOrderApi.Application.Features.Orders.Commands.CreateOrder;
using FoodOrderApi.Application.Features.Orders.Commands.UpdateOrderStatus;
using FoodOrderApi.Application.Features.Orders.Queries.GetMyOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // Sipariş Durumu Güncelleme (Sadece Admin ve Restoran Sahibi)
        [Authorize(Roles = "Admin,RestaurantOwner")]
        [HttpPatch("{orderId:int}/status")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] UpdateOrderStatusCommand command)
        {
            command.OrderId = orderId;
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}