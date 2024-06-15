namespace Identity.Core.Models;

public class ClientConfiguration
{
    public IEnumerable<ClientItem> Clients { get; set; } = [];
}

public class ClientItem
{
    public string? Host { get; set; }
    public string? ClientId { get; set; }
    public string? DisplayName { get; set; }
    public string? ClientType { get; set; }
    public string? RedirectPath { get; set; }
    public string? LogoutPath { get; set; }
}
