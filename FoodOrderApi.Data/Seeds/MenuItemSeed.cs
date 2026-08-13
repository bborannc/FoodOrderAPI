using FoodOrderApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderApi.Data.Seeds
{
    public class MenuItemSeed : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            var staticDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new MenuItem { Id = 1, RestaurantId = 1, CategoryId = 1, Name = "Et Döner Dürüm", Description = "100gr Yaprak Et Döner", Price = 220, CreatedDate = staticDate, IsDeleted = false },
                new MenuItem { Id = 2, RestaurantId = 1, CategoryId = 3, Name = "Ayran 300ml", Description = "Açık Yayık Ayranı", Price = 35, CreatedDate = staticDate, IsDeleted = false },
                new MenuItem { Id = 3, RestaurantId = 2, CategoryId = 2, Name = "Margherita Pizza", Description = "Mozzarella, Domates Sos, Fesleğen", Price = 280, CreatedDate = staticDate, IsDeleted = false }
            );
        }
    }
}
