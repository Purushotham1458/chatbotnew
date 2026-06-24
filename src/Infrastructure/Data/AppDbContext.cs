using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ApiClient> ApiClients => Set<ApiClient>();
    public DbSet<LoanDetail> LoanDetails => Set<LoanDetail>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>();
    public DbSet<ApiAuditLog> ApiAuditLogs => Set<ApiAuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApiClient>().HasIndex(c => c.ClientId).IsUnique();
        modelBuilder.Entity<LoanDetail>().HasIndex(l => l.LnNo).IsUnique();
        modelBuilder.Entity<LoanDetail>().Property(l => l.LoanAmount).HasPrecision(18, 2);
        modelBuilder.Entity<LoanDetail>().Property(l => l.InterestRate).HasPrecision(5, 2);
        modelBuilder.Entity<LoanDetail>().Property(l => l.EmiAmount).HasPrecision(18, 2);
        modelBuilder.Entity<LoanDetail>().Property(l => l.OutstandingBalance).HasPrecision(18, 2);
    }
}
