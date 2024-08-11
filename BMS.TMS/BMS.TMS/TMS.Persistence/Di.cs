using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMS.Common.Constants;
using TMS.Persistence.Caching;
using TMS.Persistence.Dynamic;
using TMS.Persistence.Shared;

namespace TMS.Persistence;

public static class Di
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SharedDbContext>(
            opts => opts.UseNpgsql(configuration.GetConnectionString(Schemas.Shared)));

        services.AddDbContext<DynamicDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString(Schemas.Shared)));

        // Cache service
        services.AddStackExchangeRedisCache(opts => opts.Configuration = "localhost:6379");
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}