using System.Text.Json.Serialization;

namespace AppCenterService;

public record BranchInfo
{
    [JsonPropertyName("name")]
    public string Name { get; init; }
    [JsonPropertyName("commit")]
    public Commit Commit { get; init; }
    [JsonPropertyName("protected")]
    public bool Protected { get; init; }
}