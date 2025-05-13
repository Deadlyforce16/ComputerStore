using ComputerStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.ProductId).IsRequired();
        builder.Property(s => s.Quantity).IsRequired();
        builder.HasOne(s => s.Product)
               .WithOne(p => p.Stock)
               .HasForeignKey<Stock>(s => s.ProductId);
    }
} 