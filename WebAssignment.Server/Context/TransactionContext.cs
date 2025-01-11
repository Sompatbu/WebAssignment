using Microsoft.EntityFrameworkCore;
using WebAssignment.Server.Entities;
using WebAssignment.Server.Enums;

namespace WebAssignment.Server.Context;

public class TransactionContext(DbContextOptions<TransactionContext> options) : DbContext(options)
{
    public DbSet<TransactionEntity> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.HasPostgresEnum<TransactionStatus>();
        _ = modelBuilder.Entity<TransactionEntity>()
            .ToTable("transactions")
            .HasIndex(e => new
            {
                e.TransactionDate,
                e.CurrencyCode,
                e.Status,
            })
            .IsDescending(true, false, false)
            .IsUnique()
            .AreNullsDistinct(false)
            .IncludeProperties(e => new
            {
                e.TransactionId,
                e.AccountNumber,
                e.Amount,
            });
    }
}
