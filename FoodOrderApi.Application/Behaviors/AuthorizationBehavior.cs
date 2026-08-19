using System.Reflection;
using FoodOrderApi.Application.Security;
using MediatR;

namespace FoodOrderApi.Application.Behaviors
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ICurrentUserService _currentUserService;

        public AuthorizationBehavior(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requiredPermissions = request.GetType()
                .GetCustomAttributes<HasPermissionAttribute>()
                .Select(x => x.Permission)
                .ToList();

            if (!requiredPermissions.Any())
            {
                return await next();
            }

            if (_currentUserService.UserId == null)
            {
                throw new UnauthorizedAccessException("Bu işlem için oturum açmanız gerekmektedir.");
            }

            var userPermissions = _currentUserService.Permissions;

            var hasAccess = requiredPermissions.All(p => userPermissions.Contains(p));
            if (!hasAccess)
            {
                throw new UnauthorizedAccessException("Bu işlemi gerçekleştirmek için yetkiniz bulunmamaktadır.");
            }

            return await next();
        }
    }
}
