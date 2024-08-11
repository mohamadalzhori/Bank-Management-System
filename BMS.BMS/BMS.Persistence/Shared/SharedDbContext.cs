using BMS.Domain.Branches;
using Microsoft.EntityFrameworkCore;

namespace BMS.Persistence.Shared;

public class SharedDbContext(DbContextOptions<SharedDbContext> options) : DbContext(options)
{
    public DbSet<Branch> Branches { get; set; } 
}