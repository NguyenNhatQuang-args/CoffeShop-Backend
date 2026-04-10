using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Coffe.Models;
using System;

namespace WebApp_Coffe.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Slug).IsRequired().HasMaxLength(200);
        builder.HasIndex(p => p.Slug).IsUnique();

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed 10 products
        builder.HasData(
            new Product { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), Name = "Espresso", Slug = "espresso", CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Product { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"), Name = "Americano", Slug = "americano", CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Product { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"), Name = "Latte", Slug = "latte", CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Product { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"), Name = "Cappuccino", Slug = "cappuccino", CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Product { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"), Name = "Mocha", Slug = "mocha", CategoryId = Guid.Parse("11111111-1111-1111-1111-111111111111"), IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            
            new Product { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6"), Name = "Green Tea", Slug = "green-tea", CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Product { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa7"), Name = "Black Tea", Slug = "black-tea", CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Product { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8"), Name = "Peach Tea", Slug = "peach-tea", CategoryId = Guid.Parse("22222222-2222-2222-2222-222222222222"), IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            
            new Product { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9"), Name = "Mango Smoothie", Slug = "mango-smoothie", CategoryId = Guid.Parse("33333333-3333-3333-3333-333333333333"), IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() },
            new Product { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa10"), Name = "Strawberry Smoothie", Slug = "strawberry-smoothie", CategoryId = Guid.Parse("33333333-3333-3333-3333-333333333333"), IsActive = true, CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() }
        );
    }
}
