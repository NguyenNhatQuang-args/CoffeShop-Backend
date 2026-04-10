using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Coffe.Models;

namespace WebApp_Coffe.Data.Configurations;

public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.ToTable("BlogPosts");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Title).IsRequired().HasMaxLength(255);
        builder.Property(b => b.Slug).IsRequired().HasMaxLength(255);
        builder.HasIndex(b => b.Slug).IsUnique();
        builder.Property(b => b.AuthorName).HasMaxLength(100);
    }
}
