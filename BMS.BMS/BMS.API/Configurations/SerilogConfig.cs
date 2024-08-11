using Serilog;

namespace BMS.API.Configurations;

public static class SerilogConfig
{
   public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
   {
      builder.Host.UseSerilog((context, configuration) =>
      {
         configuration.ReadFrom.Configuration(context.Configuration);
      });

      return builder;
   } 
}