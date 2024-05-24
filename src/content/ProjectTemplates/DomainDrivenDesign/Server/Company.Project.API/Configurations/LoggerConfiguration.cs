﻿using Serilog;

namespace Company.Project.API.Configurations;

public static class LoggerConfiguration
{
    public static void ConfigureLogger(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => 
            lc.ReadFrom.Configuration(ctx.Configuration));
    }
}
