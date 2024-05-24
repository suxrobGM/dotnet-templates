namespace Company.Project.API.Configurations;

public static class CorsConfiguration
{
    public static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("DefaultCors", cors =>
            {
                cors.WithOrigins(
                        "https://{your production website}.com",
                        "https://*.{your production website}.com")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
            options.AddPolicy("AnyCors", cors =>
            {
                cors.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}
