using FoodOrderApi.Core.Dtos;
using FoodOrderApi.Core.Enums;
using MediatR;

namespace FoodOrderApi.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<CustomResponseDto<int>>
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; } = UserRole.Customer;
    }
}
