using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventBus;

public static class Constants
{
    static Constants()
    {
        JsonSerializerOptions = new JsonSerializerOptions();
        JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public static JsonSerializerOptions JsonSerializerOptions { get; }
}
