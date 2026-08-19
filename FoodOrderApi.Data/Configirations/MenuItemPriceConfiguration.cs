using FoodOrderApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderApi.Data.Configurations
{
    public class MenuItemPriceConfiguration : IEntityTypeConfiguration<MenuItemPrice>
    {
        public void Configure(EntityTypeBuilder<MenuItemPrice> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Price).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.IsCurrent).IsRequired().HasDefaultValue(true);

            builder.HasOne(x => x.MenuItem)
                   .WithMany(m => m.Prices)
                   .HasForeignKey(x => x.MenuItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
