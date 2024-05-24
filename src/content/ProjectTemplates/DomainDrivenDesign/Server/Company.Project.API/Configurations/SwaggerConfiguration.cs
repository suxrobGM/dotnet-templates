using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Company.Project.API.Configurations;

public static class SwaggerConfiguration
{
    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var services = builder.Services;
        const string authName = "Client Credentials";
        var authProvider = configuration["Authentication:Provider"];
        
        services.AddEndpointsApiExplorer();

        if (string.IsNullOrEmpty(authProvider))
        {
            services.AddSwaggerGen();
            return;
        }
        
        var authorizationUrl = authProvider switch
        {
            "IdentityServer" => $"{configuration["Authentication:IdentityServer:Authority"]}/connect/authorize",
            "Auth0" => $"https://{configuration["Authentication:Auth0:Domain"]}/authorize",
            _ => throw new InvalidOperationException($"Invalid authentication provider, The provider '{authProvider}' is not supported.")
        };
        
        var tokenUrl = authProvider switch
        {
            "IdentityServer" => $"{configuration["Authentication:IdentityServer:Authority"]}/connect/token",
            "Auth0" => $"https://{configuration["Authentication:Auth0:Domain"]}/oauth/token",
            _ => throw new InvalidOperationException($"Invalid authentication provider, The provider '{authProvider}' is not supported.")
        };
        
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(authName, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(authorizationUrl),
                        TokenUrl = new Uri(tokenUrl),
                        Scopes = new Dictionary<string, string>
                        {
                            { "planner.read", "Planner API Read" },
                            { "planner.write", "Planner API Write" }
                        }
                    }
                }
            });
            
            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = authName
                        },
                        Name = authName,
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            };
            
            options.AddSecurityRequirement(securityRequirement);
        });
    }
    
    public static SwaggerUIOptions GetSwaggerUIOptions(IConfiguration configuration)
    {
        var options = new SwaggerUIOptions();
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Fishbowl Software API");
        
        var authProvider = configuration["Authentication:Provider"];
        
        if (string.IsNullOrEmpty(authProvider))
        {
            return options;
        }
        
        options.OAuthClientId(configuration[$"Authentication:{authProvider}:SwaggerClientId"]);
        options.OAuthClientSecret(configuration[$"Authentication:{authProvider}:SwaggerClientSecret"]);
        options.OAuthAppName("Swagger Client");
        options.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
        {
            { "audience", configuration[$"Authentication:{authProvider}:Audience"]! }
        });
        
        return options;
    }
}
