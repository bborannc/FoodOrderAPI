using FoodOrderApi.Application.Features.MenuItems.Commands.CreateMenuItem;
using FoodOrderApi.Application.Features.MenuItems.Commands.UpdateMenuItemPrice;
using FoodOrderApi.Application.Features.MenuItems.Queries.GetMenuItemsByRestaurantId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MenuItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Restorana ait menüyü listeleme (Herkese Açık)
        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<IActionResult> GetByRestaurantId(int restaurantId)
        {
            var result = await _mediator.Send(new GetMenuItemsByRestaurantIdQuery(restaurantId));
            return StatusCode(result.StatusCode, result);
        }

        // Menüye yeni ürün ekleme (Sadece Admin ve RestaurantOwner)
        [Authorize(Roles = "Admin,RestaurantOwner")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMenuItemCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        // Ürün fiyatı ve durumunu güncelleme (Sadece Admin ve RestaurantOwner)
        [Authorize(Roles = "Admin,RestaurantOwner")]
        [HttpPut("{id:int}/price")]
        public async Task<IActionResult> UpdatePrice(int id, [FromBody] UpdateMenuItemPriceCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("URL'deki ID ile gövdedeki ID uyuşmuyor.");
            }

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}
