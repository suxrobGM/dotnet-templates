using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Company.Project.API.Configurations;

public static class AuthenticationConfiguration
{
    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var authProvider = configuration["Authentication:Provider"];

        if (string.IsNullOrEmpty(authProvider))
        {
            return;
        }
        
        switch (authProvider)
        {
            case "IdentityServer":
                ConfigureIdentityServer(builder);
                break;
            case "Auth0":
                ConfigureAuth0(builder);
                break;
            default: 
                throw new InvalidOperationException($"Invalid authentication provider, The provider '{authProvider}' is not supported.");
        }
    }
    
    private static void ConfigureAuth0(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{configuration["Authentication:Auth0:Domain"]}/";
                options.Audience = configuration["Authentication:Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
    }
    
    private static void ConfigureIdentityServer(WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Authentication:IdentityServer:Authority"];
                options.Audience = configuration["Authentication:IdentityServer:Audience"];
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.ValidateIssuer = false;
            });
    }
}
