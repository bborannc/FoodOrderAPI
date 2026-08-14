using FoodOrderApi.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderApi.API.Extensions
{
    public static class DatabaseExtension
    {
        public static async Task ApplyMigrationsAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<AppDbContext>>();

            try
            {
                var context = services.GetRequiredService<AppDbContext>();

                logger.LogInformation("Veritabanı migration kontrolü yapılıyor...");

                // Veritabanı yoksa oluşturur, bekleyen migration'ları otomatik uygular
                await context.Database.MigrateAsync();

                logger.LogInformation("Veritabanı migration işlemi başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Veritabanı migration uygulanırken bir hata oluştu: {Message}", ex.Message);
                throw;
            }
        }
    }
}
