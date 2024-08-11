using BMS.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace BMS.Persistence.Dynamic;

public class DynamicDbContext(DbContextOptions<DynamicDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; } 
}