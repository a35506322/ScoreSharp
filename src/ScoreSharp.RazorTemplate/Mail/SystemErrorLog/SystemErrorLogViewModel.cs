namespace ScoreSharp.RazorTemplate.Mail.SystemErrorLog;

public class SystemErrorLogViewModel
{
    public List<SystemErrorLogDto> SystemErrorLogViewModels { get; set; } = new();
}

public class SystemErrorLogDto
{
    public string SeqNo { get; set; } = string.Empty;
    public string ApplyNo { get; set; } = string.Empty;
    public string Project { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public string ErrorDetail { get; set; } = string.Empty;
    public string Request { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public string AddTime { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}
