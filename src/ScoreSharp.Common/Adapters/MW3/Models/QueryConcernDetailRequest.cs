namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryConcernDetailRequest
{
    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("ms_Headers")]
    public object Headers { get; set; } = new();

    [JsonPropertyName("ms_Provider")]
    public string Provider { get; set; } = "ccs-uwl";

    [JsonPropertyName("ms_APIRouter")]
    public string APIRouter { get; set; } = "ConcernDetail";

    [JsonPropertyName("ms_Info")]
    public ConcernDetailInfo Info { get; set; } = new();
}

public class ConcernDetailInfo
{
    [JsonPropertyName("iD")]
    public string ID { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("agent")]
    public string Agent { get; set; }
}
