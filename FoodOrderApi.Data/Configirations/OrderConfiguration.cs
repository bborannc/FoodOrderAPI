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
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.TotalPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CancellationReason).HasMaxLength(300);

            // Value Object (Owned Entity) Yapılandırması
            builder.OwnsOne(x => x.DeliveryAddress, address =>
            {
                address.Property(a => a.City).HasColumnName("DeliveryCity").HasMaxLength(50).IsRequired();
                address.Property(a => a.District).HasColumnName("DeliveryDistrict").HasMaxLength(50).IsRequired();
                address.Property(a => a.Neighborhood).HasColumnName("DeliveryNeighborhood").HasMaxLength(100).IsRequired();
                address.Property(a => a.Street).HasColumnName("DeliveryStreet").HasMaxLength(150).IsRequired();
                address.Property(a => a.BuildingNumber).HasColumnName("DeliveryBuildingNumber").HasMaxLength(20).IsRequired();
                address.Property(a => a.DoorNumber).HasColumnName("DeliveryDoorNumber").HasMaxLength(20).IsRequired();
                address.Property(a => a.AddressDirections).HasColumnName("DeliveryAddressDirections").HasMaxLength(250);
            });

            // Kullanıcı İlişkisi
            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Restoran İlişkisi
            builder.HasOne(x => x.Restaurant)
                   .WithMany()
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Sipariş Kalemleri İlişkisi
            builder.HasMany(x => x.OrderItems)
                   .WithOne(x => x.Order)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Configure metodu içine ekleyin:
            builder.HasOne(x => x.Courier)
                   .WithMany()
                   .HasForeignKey(x => x.CourierId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);
        }
    }
}