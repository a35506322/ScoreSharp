namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryIBM7020Request
{
    [JsonPropertyName("revE_CODE")]
    public string Code { get; set; }

    [JsonPropertyName("revE_TERM_ID")]
    public string TermID { get; set; }

    [JsonPropertyName("revE_ID_NO")]
    public string ID { get; set; }

    [JsonPropertyName("revE_FILLER")]
    public string Filler { get; set; }
}
