using FoodOrderApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderApi.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Quantity).IsRequired();

            // İlişkiler
            builder.HasOne(x => x.MenuItem)
                   .WithMany()
                   .HasForeignKey(x => x.MenuItemId)
                   .OnDelete(DeleteBehavior.Restrict); // Yemek silinse bile geçmiş sipariş kalemi silinmesin
        }
    }
}
