using AMS.Domain.Branches;
using AMS.Domain.Customers;
using AMS.Persistence.Shared.Config;
using Microsoft.EntityFrameworkCore;

namespace AMS.Persistence.Shared;

public class SharedDbContext(DbContextOptions<SharedDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Branch> Branches { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new BranchConfig());
    }
}