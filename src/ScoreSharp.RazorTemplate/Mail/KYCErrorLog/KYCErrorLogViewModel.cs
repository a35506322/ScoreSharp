namespace ScoreSharp.RazorTemplate.Mail.KYCErrorLog;

public class KYCErrorLogViewModel
{
    public List<KYCErrorLogDto> KYCErrorLogViewModels { get; set; } = new();
}

public class KYCErrorLogDto
{
    public string SeqNo { get; set; } = string.Empty;
    public string ApplyNo { get; set; } = string.Empty;
    public string CardStatus { get; set; } = string.Empty;
    public string CurrentHandler { get; set; } = string.Empty;
    public string ID { get; set; } = string.Empty;
    public string KYCCode { get; set; } = string.Empty;
    public string KYCRank { get; set; } = string.Empty;
    public string KYCMsg { get; set; } = string.Empty;
    public string Request { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public string AddTime { get; set; } = string.Empty;
    public string APIName { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}
