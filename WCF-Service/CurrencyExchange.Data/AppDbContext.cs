using CurrencyExchange.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Balance> Balances { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Balance>()
            .HasOne(b => b.User)
            .WithMany(u => u.Balances)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Balance>()
            .HasIndex(b => new { b.UserId, b.Currency })
            .IsUnique();
    }
}