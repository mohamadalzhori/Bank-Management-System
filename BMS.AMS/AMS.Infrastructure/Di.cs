using AMS.Application.RabbitMQ.Account;
using AMS.Common.Constants;
using AMS.Infrastructure.RabbitMQ;
using AMS.Infrastructure.RabbitMQ.Accounts;
using AMS.Infrastructure.RabbitMQ.Branchs;
using AMS.Persistence.Caching;
using AMS.Persistence.Dynamic;
using AMS.Persistence.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AMS.Infrastructure;

public static class Di
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        // RabbitMQ
        services.AddSingleton<RabbitMqConsumer>(); 
        services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();
        
        // Background Services
        services.AddHostedService<RabbitMqConsumer>();
        
        
        return services;
    }
}