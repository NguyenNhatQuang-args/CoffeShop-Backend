using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Coffe.Models;

namespace WebApp_Coffe.Data.Configurations;

public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.ToTable("ProductTags");
        builder.HasKey(pt => pt.Id);
        
        builder.Property(pt => pt.TagName).IsRequired().HasMaxLength(50);

        builder.HasOne(pt => pt.Product)
            .WithMany(p => p.ProductTags)
            .HasForeignKey(pt => pt.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
