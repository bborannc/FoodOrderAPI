using FoodOrderApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderApi.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TotalPrice).HasPrecision(18, 2);
            builder.Property(x => x.CancellationReason).HasMaxLength(500);

            // Restaurant - Order İlişkisi (Tek ve net foreign key)
            builder.HasOne(x => x.Restaurant)
                   .WithMany(r => r.Orders)
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.Restrict);

            // User - Order İlişkisi
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Courier - Order İlişkisi
            builder.HasOne(x => x.Courier)
                   .WithMany()
                   .HasForeignKey(x => x.CourierId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            // Value Object (Owned Entity)
            builder.OwnsOne(x => x.DeliveryAddress, address =>
            {
                address.Property(a => a.City).HasMaxLength(50).IsRequired();
                address.Property(a => a.District).HasMaxLength(50).IsRequired();
                address.Property(a => a.Neighborhood).HasMaxLength(100).IsRequired();
                address.Property(a => a.Street).HasMaxLength(100).IsRequired();
                address.Property(a => a.BuildingNumber).HasMaxLength(20).IsRequired();
                address.Property(a => a.DoorNumber).HasMaxLength(20);
                address.Property(a => a.AddressDirections).HasMaxLength(250);
            });
        }
    }
}