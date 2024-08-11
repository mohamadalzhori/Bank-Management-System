using BMS.Application.Services;
using BMS.Infrastructure;
using BMS.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BMS.Application;

public static class Di
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Persistence Layer
        services.AddPersistence(configuration);

        // Infrastructure Layer
        services.AddInfrastructure(configuration);
        
        // AutoMapper
        services.AddAutoMapper(typeof(Di).Assembly);

        // Mediator
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Di).Assembly));

        // Services
        services.AddSingleton<EncryptionHelper>();
        services.AddScoped<MigrationService>();
        services.AddScoped<DynamicDbContextFactory>();
       
        
        
        return services;
    }
}