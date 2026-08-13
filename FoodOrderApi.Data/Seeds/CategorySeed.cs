using FoodOrderApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FoodOrderApi.Data.Seeds
{
    public class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            var staticDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Category { Id = 1, Name = "Ana Yemek", Description = "Döner, Kebap ve Sıcak Yemekler", CreatedDate = staticDate, IsDeleted = false },
                new Category { Id = 2, Name = "Pizza", Description = "İtalyan Usulü Taş Fırın Pizzalar", CreatedDate = staticDate, IsDeleted = false },
                new Category { Id = 3, Name = "İçecek", Description = "Soğuk ve Sıcak İçecekler", CreatedDate = staticDate, IsDeleted = false }
            );
        }
    }
}
