using FoodOrderApi.Core.Entities;

namespace FoodOrderApi.Application.Security
{
    public interface ITokenService
    {
        TokenDto CreateToken(User user);
    }
}
