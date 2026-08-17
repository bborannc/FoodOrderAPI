using FoodOrderApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderApi.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Score).IsRequired();
            builder.Property(x => x.Comment).HasMaxLength(500);

            // Bir siparişe yalnızca 1 değerlendirme yapılabilir (Bire-bir ilişki & Unique Index)
            builder.HasOne(x => x.Order)
                   .WithOne(o => o.Review)
                   .HasForeignKey<Review>(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Restaurant)
                   .WithMany(r => r.Reviews)
                   .HasForeignKey(x => x.RestaurantId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
