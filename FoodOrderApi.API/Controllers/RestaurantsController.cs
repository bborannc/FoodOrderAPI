using FoodOrderApi.Application.Features.Restaurants.Commands.CreateRestaurant;
using FoodOrderApi.Application.Features.Restaurants.Queries.GetRestaurants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RestaurantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetRestaurantsQuery());
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Admin,RestaurantOwner")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantCommand command)
        {
            var response = await _mediator.Send(command);
            return StatusCode(response.StatusCode, response);
        }
    }
}
