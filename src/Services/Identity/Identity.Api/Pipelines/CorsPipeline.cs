using Identity.Core.Models;

namespace Identity.Api.Pipelines;

public static class CorsPipeline
{
    public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
    {
        var clients = builder.Configuration
            .GetSection(nameof(ClientConfiguration))
            .GetSection(nameof(ClientConfiguration.Clients))
            .Get<IEnumerable<ClientItem>>()?
            .ToList();

        if (clients is null || !clients.Any())
        {
            Console.WriteLine("No client exists for configuring CORS policy");
            return builder;
        }

        builder.Services.AddCors(options => options
            .AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(clients
                        .Where(w => !string.IsNullOrWhiteSpace(w.Host))
                        .Select(s => s.Host!.TrimEnd('/'))
                        .ToArray())
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            }));
        return builder;
    }
}
