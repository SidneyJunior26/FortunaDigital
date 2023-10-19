using FortunaDigital.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FortunaDigital.Infraistructure.Configurations;

public class UserConfigutarions : IEntityTypeConfiguration<User> {

    public void Configure(EntityTypeBuilder<User> builder) {

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Cpf)
            .HasColumnType("varchar(11)")
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(u => u.Name)
            .HasColumnType("varchar(200)")
            .IsRequired();

        builder.Property(u => u.Age)
            .HasColumnType("int(2)")
            .IsRequired();

        builder.Property(u => u.PhoneNumber)
            .HasColumnType("varchar(11)")
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnType("varchar(30)")
            .IsRequired();

        builder.HasMany(u => u.RaffleNumbers)
            .WithOne(rn => rn.User)
            .HasForeignKey(rn => rn.IdUser);
    }
}

