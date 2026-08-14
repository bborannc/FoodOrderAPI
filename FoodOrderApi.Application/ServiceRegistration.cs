using System.Reflection;
using FluentValidation;
using FoodOrderApi.Application.Behaviors; 
using FoodOrderApi.Application.Security;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderApi.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // MediatR Kaydı
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
            });

            // FluentValidation Kaydı
            services.AddValidatorsFromAssembly(assembly);

            // JWT Token Servis Kaydı
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}