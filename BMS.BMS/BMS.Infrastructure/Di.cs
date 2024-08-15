using BMS.Infrastructure.Grpc.Services;
using BMS.Infrastructure.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BMS.Infrastructure;

public static class Di
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();

        services.AddSingleton<RollBackService>();
        
        return services;
    }
}