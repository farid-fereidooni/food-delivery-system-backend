 using Identity.Infrastructure.Database;
 using Microsoft.EntityFrameworkCore;

 namespace Identity.Api.Pipelines;

public static class DatabasePipeline
{
    public static WebApplicationBuilder AddCustomEntityFrameworkCore(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(dbBuilder => dbBuilder
            .UseNpgsql(builder.Configuration.GetConnectionString("RelationalDatabase"))
            .UseSnakeCaseNamingConvention()
            .UseOpenIddict());

        return builder;
    }
}
