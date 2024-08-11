using BMS.Common.Constants;
using BMS.Persistence.Caching;
using BMS.Persistence.Dynamic;
using BMS.Persistence.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BMS.Persistence;

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