namespace ScoreSharp.Batch.Infrastructures.Adapter.Models;

public class SendEmailRequest
{
    public List<EmailAddressDto> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; } = true;
}

public class EmailAddressDto
{
    public string Name { get; set; }
    public string Address { get; set; }
}
