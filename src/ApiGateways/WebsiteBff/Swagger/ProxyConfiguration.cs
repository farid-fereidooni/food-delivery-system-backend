namespace WebsiteBff.Swagger;

public class ProxyConfiguration
{
    public Dictionary<string, Route> Routes { get; set; } = new();
    public Dictionary<string, Cluster> Clusters { get; set; } = new();
}

public class Destination
{
    public required string Address { get; set; }
}

public class HttpRequest
{
    public TimeSpan Timeout { get; set; }
}

public class Match
{
    public required string Path { get; set; }
    public List<string> Methods { get; set; } = [];
}

public class Route
{
    public required string ClusterId { get; set; }
    public required Match Match { get; set; }
    public List<Dictionary<string, string>> Transforms { get; set; } = [];
}

public class Cluster
{
    public Dictionary<string, Destination> Destinations { get; set; } = new();
    public HttpRequest? HttpRequest { get; set; }
    public Swagger? Swagger { get; set; }
}

public class Swagger
{
    public required string Path { get; set; }
}

public struct ConfigPair
{
    public KeyValuePair<string ,Route> Route { get; set; }
    public Cluster? Cluster { get; set; }
}
