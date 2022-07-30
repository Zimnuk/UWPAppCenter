using System.Text.Json.Serialization;

namespace AppCenterService;

public record Build
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    [JsonPropertyName("buildingName")]
    public string BuildingNumber { get; init; }
    [JsonPropertyName("queueTime")]
    public string QueueTime { get; init; }
    [JsonPropertyName("startTime")]
    public string StartTime { get; init; }
    [JsonPropertyName("finishTime")]
    public string FinishTime { get; init; }
    [JsonPropertyName("lastChangedDate")]
    public string LastChangedDate { get; init; }
    [JsonPropertyName("status")]
    public string Status { get; init; }
    [JsonPropertyName("result")]
    public string Result { get; init; }
    [JsonPropertyName("reason")]
    public string Reason { get; init; }
    [JsonPropertyName("sourceBranch")]
    public string SourceBranch { get; init; }
    [JsonPropertyName("sourceVersion")]
    public string SourceVersion { get; init; }
    [JsonPropertyName("tags")]
    public string[] Tags { get; init; }
    [JsonPropertyName("properties")]
    public object Propperties { get; init; }
}