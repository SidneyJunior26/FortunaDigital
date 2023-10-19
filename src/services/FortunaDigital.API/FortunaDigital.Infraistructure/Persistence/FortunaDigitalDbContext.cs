using Microsoft.EntityFrameworkCore;
using FortunaDigital.Core.Entities;
using System.Reflection;

namespace FortunaDigital.Infraistructure.Persistence;

public partial class FortunaDigitalDbContext : DbContext {
    public FortunaDigitalDbContext() {
    }

    public FortunaDigitalDbContext(DbContextOptions<FortunaDigitalDbContext> options) : base(options) {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Raffle> Raffles { get; set; }
    public DbSet<RaffleNumbers> RaffleNumbers { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseMySql("server=localhost;port=8889;database=FortunaDigital;uid=root;pwd=root;connection timeout=300;default command timeout=300", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.34-mysql"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}