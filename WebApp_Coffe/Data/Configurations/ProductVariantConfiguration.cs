using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp_Coffe.Models;

namespace WebApp_Coffe.Data.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants");
        builder.HasKey(pv => pv.Id);
        
        builder.Property(pv => pv.SizeName).IsRequired().HasMaxLength(10);
        builder.Property(pv => pv.Temperature).IsRequired().HasMaxLength(20);
        builder.Property(pv => pv.Price).HasColumnType("decimal(18,2)");

        builder.HasOne(pv => pv.Product)
            .WithMany(p => p.ProductVariants)
            .HasForeignKey(pv => pv.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
