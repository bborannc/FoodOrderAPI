using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FoodOrderApi.Core.Constants;
using FoodOrderApi.Core.Entities;
using FoodOrderApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FoodOrderApi.Application.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenDto CreateToken(User user)
        {
            // 1. Temel Kullanıcı Claim'leri
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // 2. Rol Bazlı İzin (Permission) Claim'leri
            claims.AddRange(GetPermissionsForRole(user.Role));

            // 3. Konfigürasyondan Key ve Parametreleri Oku
            var secretKey = _configuration["JwtSettings:SecretKey"] ?? _configuration["JwtSettings:SecurityKey"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var expirationMinutes = Convert.ToInt32(_configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "60");
            var expirationDate = DateTime.UtcNow.AddMinutes(expirationMinutes);

            // 4. Token Oluşturma (Tüm claim'ler ClaimsIdentity olarak paketlenir)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationDate,
                SigningCredentials = credentials,
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            var refreshToken = CreateRefreshToken();

            return new TokenDto
            {
                AccessToken = accessToken,
                AccessTokenExpiration = expirationDate,
                RefreshToken = refreshToken
            };
        }

        private static string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private static IEnumerable<Claim> GetPermissionsForRole(UserRole role)
        {
            var permissions = new List<string>();

            switch (role)
            {
                case UserRole.Admin:
                    permissions.AddRange(new[]
                    {
                        Permissions.Orders.View, Permissions.Orders.Create, Permissions.Orders.UpdateStatus, Permissions.Orders.Cancel,
                        Permissions.MenuItems.View, Permissions.MenuItems.Create, Permissions.MenuItems.Edit, Permissions.MenuItems.Delete,
                        Permissions.Restaurants.Create, Permissions.Restaurants.Edit, Permissions.Restaurants.Delete
                    });
                    break;

                case UserRole.RestaurantOwner:
                    permissions.AddRange(new[]
                    {
                        Permissions.Orders.View, Permissions.Orders.UpdateStatus,
                        Permissions.MenuItems.View, Permissions.MenuItems.Create, Permissions.MenuItems.Edit, Permissions.MenuItems.Delete
                    });
                    break;

                case UserRole.Courier:
                    permissions.AddRange(new[]
                    {
                        Permissions.Orders.View, Permissions.Orders.UpdateStatus
                    });
                    break;

                case UserRole.Customer:
                    permissions.AddRange(new[]
                    {
                        Permissions.Orders.View, Permissions.Orders.Create, Permissions.Orders.Cancel,
                        Permissions.MenuItems.View
                    });
                    break;
            }

            return permissions.Select(p => new Claim("permission", p));
        }
    }
}