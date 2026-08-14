using FoodOrderApi.Application.Features.Auth.Commands.Login;
using FoodOrderApi.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);
            return StatusCode(response.StatusCode, response);
        }
    }
}
