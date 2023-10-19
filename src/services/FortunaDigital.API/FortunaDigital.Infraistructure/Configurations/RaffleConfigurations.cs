using FortunaDigital.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FortunaDigital.Infraistructure.Configurations;

public class RaffleConfigurations : IEntityTypeConfiguration<Raffle> {
    public void Configure(EntityTypeBuilder<Raffle> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Active)
            .HasMaxLength(1)
            .IsRequired();

        builder.Property(r => r.TotalValue)
            .HasColumnType("decimal(7,2)")
            .IsRequired();

        builder.Property(r => r.TotalByNumber)
            .HasColumnType("decimal(7,2)")
            .IsRequired();

        builder.Property(r => r.Amount)
            .HasColumnType("int(4)")
            .IsRequired();

        builder.Property(r => r.DrawDate)
            .IsRequired();

        builder.Property(r => r.Rules)
            .IsRequired();

        builder.Property(r => r.Level)
            .HasColumnType("int(1)")
            .IsRequired();

        builder.HasMany(r => r.RaffleNumbers)
            .WithOne(rn => rn.Raffle)
            .HasForeignKey(rn => rn.IdRaffle);
    }
}