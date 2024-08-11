using AMS.API.Policies;
using AMS.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace AMS.API.Configurations;

public static class KeycloakConfig
{
    public static IServiceCollection ConfigureKeycloak(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["Keycloak:Authority"];
        var client = configuration["Keycloak:ClientId"]; 
        var requireHttps = configuration.GetValue<bool>("Keycloak:RequireHttpsMetadata");
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = authority; // Keycloak URL
                options.Audience = client; // Client ID from Keycloak
                options.RequireHttpsMetadata = requireHttps; // Set to true in production

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                };
            });
        
        
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.BranchPolicy, policy =>
            {
                policy.Requirements.Add(new BranchRequirement());
            });
        });
       
        services.AddSingleton<IAuthorizationHandler, BranchRequirementHandler>();

 
        return services;
    }
}