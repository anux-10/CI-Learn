using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderServiceApi.Models;

namespace OrderServiceApi.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(i => new { i.OrderId, i.ProductId });

        builder.Property(i => i.Quantity).IsRequired();
        builder.Property(i => i.UnitPrice).HasPrecision(18, 2).IsRequired();

        builder.Ignore(i => i.Total);
    }
}
