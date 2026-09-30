using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentServiceApi.Models;

namespace PaymentServiceApi.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.OrderId).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Amount).HasPrecision(18, 2).IsRequired();
        builder.Property(p => p.Currency).IsRequired().HasMaxLength(10);
        builder.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(50);
        builder.Property(p => p.CardNumber).HasMaxLength(30);
        builder.Property(p => p.CardNameHolder).HasMaxLength(100);
        builder.Property(p => p.TransactionId).HasMaxLength(100);
        builder.Property(p => p.CreatedAt).IsRequired();

        builder.HasIndex(p => p.OrderId);
    }
}
