using FortunaDigital.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FortunaDigital.Infraistructure.Configurations;

public class RaffleNumberConfiguration : IEntityTypeConfiguration<RaffleNumbers>
{
    public void Configure(EntityTypeBuilder<RaffleNumbers> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(rn => rn.IdUser)
            .IsRequired();

        builder.HasOne(rn => rn.User)
            .WithMany(u => u.RaffleNumbers)
            .HasForeignKey(rn => rn.IdUser);

        builder.Property(rn => rn.IdRaffle)
            .IsRequired();

        builder.HasOne(rn => rn.Raffle)
            .WithMany(r => r.RaffleNumbers)
            .HasForeignKey(rn => rn.IdRaffle);

        builder.Property(rn => rn.Amount)
            .HasColumnType("int(10)")
            .IsRequired();

        builder.Property(rn => rn.TotalValue)
            .HasColumnType("decimal(7,2)")
            .IsRequired();

        builder.HasOne(rn => rn.Payment)
            .WithOne(p => p.RaffleNumber)
            .HasForeignKey<RaffleNumbers>(rn => rn.IdPayment);
    }
}

