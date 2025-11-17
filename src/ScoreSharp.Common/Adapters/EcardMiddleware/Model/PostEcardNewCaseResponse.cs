namespace ScoreSharp.Common.Adapters.EcardMiddleware.Model;

public class PostEcardNewCaseResponse
{
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    public PostEcardNewCaseResult Result { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;
}

public class PostEcardNewCaseResult
{
    [JsonPropertyName("ID")]
    public string ID { get; set; }

    [JsonPropertyName("RESULT")]
    public string Result { get; set; }
}
