using System.Text.Json.Serialization;

namespace AppCenterService;

public record Commit
{
    [JsonPropertyName("sha")]
    public string Sha { get; init; }
    [JsonPropertyName("url")]
    public string Url { get; init; }
}