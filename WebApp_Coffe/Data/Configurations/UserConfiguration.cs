using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Coffe.Models;
using System;

namespace WebApp_Coffe.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();
        
        // Seed Admin User (Password: Admin@123)
        // In a real application, you MUST hash the password. For simplicity in this prototype, we'll store it as plain text or a simple hash.
        // Let's store plain text for the sake of the prototype login matching "Admin@123", or we can hash it.
        // We'll use BCrypt or similar later. For now, let's just store "Admin@123" and check exactly that in AuthService.
        builder.HasData(
            new User 
            { 
                Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), 
                Email = "admin@coffeeshop.com", 
                PasswordHash = "$2a$11$q2LzNjZBwt754ssEkedFN.dNj4FT7PWJrYsnRmMAgwFl4MWGECg9i", // Hardcoded BCrypt hash of "Admin@123" to prevent EF Core migration issues
                FullName = "System Admin",
                Role = "Admin",
                IsActive = true, 
                CreatedAt = DateTime.Parse("2026-04-07T00:00:00Z").ToUniversalTime() 
            }
        );
    }
}
