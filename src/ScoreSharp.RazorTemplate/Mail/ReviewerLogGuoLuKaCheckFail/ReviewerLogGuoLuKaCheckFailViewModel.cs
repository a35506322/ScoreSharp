namespace ScoreSharp.RazorTemplate.Mail.ReviewerLogGuoLuKaCheckFail;

public class ReviewerLogGuoLuKaCheckFailViewModel
{
    public List<ReviewerLogGuoLuKaCheckFailDto> ReviewerLogGuoLuKaCheckFailDtos { get; set; } = new();
}

public class ReviewerLogGuoLuKaCheckFailDto
{
    public string SeqNo { get; set; } = string.Empty;
    public string ApplyNo { get; set; } = string.Empty;
    public string ID { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public string CreateTime { get; set; } = string.Empty;
}
