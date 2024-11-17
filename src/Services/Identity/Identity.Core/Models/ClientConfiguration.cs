namespace Identity.Core.Models;

public class ClientConfiguration
{
    public Dictionary<string, ClientItem> Clients { get; set; }
    public Dictionary<string, ApiClientItem> ApiClients { get; set; } = [];
}

public class ClientItem
{
    public string? Host { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? DisplayName { get; set; }
    public string? ClientType { get; set; }
    public string? RedirectPath { get; set; }
    public string? LogoutPath { get; set; }
    public string[] AdditionalScopes { get; set; } = [];
}

public class ApiClientItem
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? DisplayName { get; set; }
    public string? ScopeName { get; set; }
}
