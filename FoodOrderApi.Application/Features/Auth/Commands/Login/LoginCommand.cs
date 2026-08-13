using FoodOrderApi.Application.Security;
using FoodOrderApi.Core.Dtos;
using MediatR;

namespace FoodOrderApi.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<CustomResponseDto<TokenDto>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
