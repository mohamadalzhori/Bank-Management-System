using Hangfire;
using Hangfire.PostgreSql;

namespace TMS.API.Configurations;

public static class HangfireConfig
{
    public static IServiceCollection ConfigureHangfire(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(x =>
                x.UseNpgsqlConnection(builder.Configuration.GetConnectionString("Shared"))));

        services.AddHangfireServer();

        return services;
    }
}