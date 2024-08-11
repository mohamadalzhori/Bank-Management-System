using Microsoft.EntityFrameworkCore;
using TMS.Persistence.Caching;
using TMS.Persistence.Dynamic;
using TMS.Persistence.Shared;

namespace TMS.Application.Services;

public class DynamicDbContextFactory(
    SharedDbContext sharedDbContext,
    EncryptionHelper encryptionHelper,
    ICacheService cacheService)
{
    public async Task<DynamicDbContext> Create(Guid branchId, CancellationToken cancellationToken)
    {
        // Caching the decrypted connection string
        var connectionString = await GetConnectionString(branchId, cancellationToken);
        
        var dbContextOptions = new DbContextOptionsBuilder<DynamicDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        var context = new DynamicDbContext(dbContextOptions);

        return context;
    }

    private async Task<string> GetConnectionString(Guid branchId, CancellationToken cancellationToken)
    {
        string? connectionString;
        var cacheKey = $"TMS_BranchConnectionString_{branchId}";
        var cachedConnectionString = await cacheService.GetAsync<string>(cacheKey, cancellationToken);

        if (cachedConnectionString is not null)
        {
            connectionString = cachedConnectionString;
        }
        else
        {
            connectionString = sharedDbContext.Branches
                .Where(x => x.Id == branchId)
                .Select(x => x.ConnectionString)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"No valid connection string found for branch ID {branchId}");
            }

            connectionString = encryptionHelper.Decrypt(connectionString);

            await cacheService.SetAsync(cacheKey, connectionString, TimeSpan.FromMinutes(30), cancellationToken);
        }
        
        return connectionString;
    }
}