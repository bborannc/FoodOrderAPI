using System.Security.Claims;
using FoodOrderApi.Application.Security;

namespace FoodOrderApi.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? _httpContextAccessor.HttpContext?.User?.FindFirst("nameid")?.Value
                               ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
                return int.TryParse(userIdClaim, out var id) ? id : null;
            }
        }

        public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value
                             ?? _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;

        public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value
                            ?? _httpContextAccessor.HttpContext?.User?.FindFirst("role")?.Value;

        // "permission" claim'lerini doğrudan tüm Claims koleksiyonundan filtreleyelim:
        public List<string> Permissions => _httpContextAccessor.HttpContext?.User?.Claims
            .Where(c => c.Type.Equals("permission", StringComparison.OrdinalIgnoreCase))
            .Select(c => c.Value)
            .ToList() ?? new List<string>();
    }
}