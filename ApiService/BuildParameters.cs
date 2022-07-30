using System.Text.Json.Serialization;

namespace AppCenterService
{
    public record BuildParameters
    {
        [JsonPropertyName("sourceVersion")]
        public string SourceVersion { get; init; }
        [JsonPropertyName("debug")]
        public bool Debug { get; init; }

        public BuildParameters(string sourceVersion, bool debug)
        {
            SourceVersion = sourceVersion;
            Debug = debug;
        }
    }
}

