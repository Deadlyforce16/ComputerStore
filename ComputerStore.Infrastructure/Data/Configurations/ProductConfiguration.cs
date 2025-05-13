using ComputerStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Description).IsRequired(false);
        builder.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
        builder.HasMany(p => p.Categories)
               .WithMany(c => c.Products);
        builder.HasOne(p => p.Stock)
               .WithOne(s => s.Product)
               .HasForeignKey<Stock>(s => s.ProductId);
    }
} 