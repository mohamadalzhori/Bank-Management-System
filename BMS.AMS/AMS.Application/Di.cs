﻿using AMS.Application.Services;
using AMS.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AMS.Application;

public static class Di
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Persistence Layer
        services.AddPersistence(configuration);

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