using FoodOrderApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderApi.Data.Configurations
{
    public class AuthenticationLogConfiguration : IEntityTypeConfiguration<AuthenticationLog>
    {
        public void Configure(EntityTypeBuilder<AuthenticationLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AccessToken).IsRequired();
            builder.Property(x => x.RefreshToken).IsRequired().HasMaxLength(256);
            builder.Property(x => x.ExpireDate).IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(u => u.AuthenticationLogs)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
