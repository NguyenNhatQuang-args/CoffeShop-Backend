using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Coffe.Models;
using System;

namespace WebApp_Coffe.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Slug).IsRequired().HasMaxLength(100);
        builder.HasIndex(c => c.Slug).IsUnique();
        
        // Seed 5 categories
        builder.HasData(
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Coffee", Slug = "coffee", Description = "Premium roasted coffee", IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Category { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Tea", Slug = "tea", Description = "Freshly brewed tea", IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Category { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Smoothies", Slug = "smoothies", Description = "Fruit smoothies", IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Category { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Cakes", Slug = "cakes", Description = "Delicious cakes", IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Category { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Snacks", Slug = "snacks", Description = "Quick bites", IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() }
        );
    }
}
