namespace ScoreSharp.Batch.Infrastructures.Adapter.Models;

public class SendEmailResponse
{
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    public SendEmailResult Result { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;
}

public class SendEmailResult
{
    public string MessageId { get; set; }

    public IList<string> ErrorMessages { get; set; }

    public bool Successful { get; set; }
}
