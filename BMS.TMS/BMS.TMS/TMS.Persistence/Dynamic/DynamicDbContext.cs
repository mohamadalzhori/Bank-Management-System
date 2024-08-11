using Microsoft.EntityFrameworkCore;
using TMS.Domain.Accounts;
using TMS.Domain.Transactions;
using TMS.Persistence.Dynamic.Config;

namespace TMS.Persistence.Dynamic;

public class DynamicDbContext(DbContextOptions<DynamicDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new AccountConfig());
    }
}