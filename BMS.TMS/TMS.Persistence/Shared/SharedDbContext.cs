using Microsoft.EntityFrameworkCore;
using TMS.Domain.Branches;
using TMS.Persistence.Shared.Config;

namespace TMS.Persistence.Shared;

public class SharedDbContext(DbContextOptions<SharedDbContext> options) : DbContext(options)
{
    public DbSet<Branch> Branches { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new BranchConfig());
    }
}