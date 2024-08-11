using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMS.Infrastructure.gRPC.Services;
using TMS.Infrastructure.RabbitMQ.Accounts;
using TMS.Infrastructure.RabbitMQ.Branches;

namespace TMS.Infrastructure;

public static class Di
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        // RabbitMQ
        services.AddSingleton<RabbitMqAccountConsumer>(); 
        services.AddSingleton<RabbitMqBranchConsumer>(); 
        
        // Background Services
        services.AddHostedService<RabbitMqAccountConsumer>();
        services.AddHostedService<RabbitMqBranchConsumer>();
      
        // gRPC Services
        services.AddGrpc();
         
        return services;
    }
    
    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        // Map gRPC services
        app.MapGrpcService<RollBackService>();

        return app;
    }
}