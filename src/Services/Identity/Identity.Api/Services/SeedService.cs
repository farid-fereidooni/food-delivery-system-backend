using Identity.Api.Utilities;
using Identity.Core.Models;
using Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.Packaging;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Api.Services;

public class SeedService : IHostedService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<SeedService> _logger;
    private static readonly string[] AvailableClientTypes = ["public"];

    public SeedService(IServiceProvider services, ILogger<SeedService> logger)
    {
        _services = services;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var clientConfiguration = scope.ServiceProvider.GetRequiredService<IOptions<ClientConfiguration>>();

        await MigrateDatabase(dbContext, _logger, cancellationToken);
        await BuildClients(manager, clientConfiguration, cancellationToken);
    }

    private static async Task MigrateDatabase(DbContext dbContext, ILogger logger, CancellationToken cancellationToken)
    {
        await PollyHelper.HandleSqlNotReady(logger).ExecuteAsync(async () =>
        {
            if ((await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private async Task BuildClients(
        IOpenIddictApplicationManager manager,
        IOptions<ClientConfiguration> config,
        CancellationToken cancellationToken)
    {
        foreach (var clientConfig in config.Value.Clients)
        {
            if (!ValidateClient(clientConfig))
                continue;

            if (await manager.FindByClientIdAsync(clientConfig.ClientId!, cancellationToken) is not null)
                continue;

            var client = new OpenIddictApplicationDescriptor
            {
                ClientId = clientConfig.ClientId,
                DisplayName = clientConfig.DisplayName,
                ClientType = clientConfig.ClientType,
                ConsentType = ConsentTypes.Implicit,
                RedirectUris =
                {
                    new Uri(Path.Combine(clientConfig.Host!, clientConfig.RedirectPath!.TrimStart('/')))
                },
                PostLogoutRedirectUris =
                {
                    new Uri(Path.Combine(clientConfig.Host!, clientConfig.LogoutPath!.TrimStart('/')))
                }
            };
            client.Permissions.AddRange(BuildPermissions(clientConfig.ClientType!));
            await manager.CreateAsync(client, cancellationToken);
        }
    }

    private bool ValidateClient(ClientItem client)
    {
        var hasError = false;

        if (!Uri.TryCreate(client.Host, UriKind.Absolute, out _))
        {
            _logger.LogError(
                "Invalid base URL: \"{baseUrl}\" for client ID \"{ClientId}\"", client.Host, client.ClientId);
            hasError = true;
        }

        if (string.IsNullOrWhiteSpace(client.ClientId))
        {
            _logger.LogError("Invalid client ID: \"{ClientId}\"", client.ClientId);
            hasError = true;
        }

        if (string.IsNullOrWhiteSpace(client.DisplayName))
        {
            _logger.LogError(
                "Invalid display name: \"{DisplayName}\" for client ID \"{ClientId}\"",
                client.DisplayName,
                client.ClientId);
            hasError = true;
        }

        if (string.IsNullOrWhiteSpace(client.ClientType)
            || !AvailableClientTypes.Contains(client.ClientType))
        {
            _logger.LogError(
                "Invalid Client Type: \"{ClientType}\" for client ID \"{ClientId}\"\n" +
                "Available client types: {ClientTypes}",
                client.ClientType,
                client.ClientId,
                string.Join(", ", AvailableClientTypes));
            hasError = true;
        }

        if (!Uri.TryCreate(client.RedirectPath, UriKind.Relative, out _))
        {
            _logger.LogError(
                "Invalid redirect path: \"{RedirectPath}\" for client ID \"{clientId}\"",
                client.RedirectPath,
                client.ClientId);
            hasError = true;
        }

        if (!Uri.TryCreate(client.LogoutPath, UriKind.Relative, out _))
        {
            _logger.LogError(
                "Invalid logout path: \"{RedirectPath}\" for client ID \"{clientId}\"",
                client.RedirectPath,
                client.ClientId);
            hasError = true;
        }

        return !hasError;
    }

    private static IEnumerable<string> BuildPermissions(string clientType) => clientType switch
    {
        "public" =>
        [
            Permissions.Endpoints.Authorization,
            Permissions.Endpoints.Logout,
            Permissions.Endpoints.Token,
            Permissions.GrantTypes.AuthorizationCode,
            Permissions.ResponseTypes.Code,
            Permissions.Scopes.Email,
            Permissions.Scopes.Profile
        ],
        _ => throw new ArgumentException($"Invalid client type: {clientType}")
    };

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
