using FoodOrderApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderApi.Data.Seeds
{
    public class RestaurantSeed : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            var staticDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Restaurant { Id = 1, Name = "Dönerci Ali Usta", Address = "Kadıköy / İstanbul", PhoneNumber = "02161112233", IsActive = true, CreatedDate = staticDate, IsDeleted = false },
                new Restaurant { Id = 2, Name = "Pizza Italia", Address = "Beşiktaş / İstanbul", PhoneNumber = "02124445566", IsActive = true, CreatedDate = staticDate, IsDeleted = false }
            );
        }
    }
}
