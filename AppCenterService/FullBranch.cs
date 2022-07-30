using System.Text.Json.Serialization;

namespace AppCenterService;

public record FullBranch
{
    [JsonPropertyName("branch")]
    public BranchInfo Branch { get; init; }
    [JsonPropertyName("configured")]
    public bool Configured { get; init; }
    [JsonPropertyName("lastBuild")]
    public Build LastBuild { get; init; }
    [JsonPropertyName("trigger")]
    public string Trigger { get; init; }
}