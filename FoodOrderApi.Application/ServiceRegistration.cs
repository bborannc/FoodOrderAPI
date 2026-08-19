using System.Reflection;
using FoodOrderApi.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderApi.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                // Pipeline Behavior'ı MediatR hattına açık generic olarak ekliyoruz:
                cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            });
        }
    }
}