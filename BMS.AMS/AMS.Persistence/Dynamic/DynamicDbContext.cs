using AMS.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace AMS.Persistence.Dynamic;

public class DynamicDbContext(DbContextOptions<DynamicDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
}