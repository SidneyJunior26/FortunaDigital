using FortunaDigital.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FortunaDigital.Infraistructure.Configurations;

public class PaymentConfigurations : IEntityTypeConfiguration<Payment> {
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(p => p.IdRaffleNumber)
            .IsRequired();

        builder.HasOne(p => p.RaffleNumber)
            .WithOne(rn => rn.Payment)
            .HasForeignKey<Payment>(p => p.IdRaffleNumber);

        builder.Property(p => p.PaymentMethod)
            .HasMaxLength(1)
            .IsRequired();

        builder.Property(p => p.QrCode)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(p => p.PixCode)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(p => p.BarCode)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(p => p.PaymentLink)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(p => p.TotalValue)
            .HasColumnType("float(7,2)")
            .IsRequired();

        builder.Property(p => p.Paid)
            .HasMaxLength(1)
            .IsRequired();
    }
}

