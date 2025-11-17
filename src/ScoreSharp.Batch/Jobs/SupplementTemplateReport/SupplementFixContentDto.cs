namespace ScoreSharp.Batch.Jobs.SupplementTemplateReport;

public class SupplementFixContentDto
{
    public string CardUrl { get; }
    public string Email { get; }
    public string Ext { get; }
    public string ServicePhone { get; }
    public string QrCodeUrl { get; }

    public SupplementFixContentDto(string cardUrl, string email, string ext, string servicePhone, string qrCodeUrl)
    {
        CardUrl = cardUrl;
        Email = email;
        Ext = ext;
        ServicePhone = servicePhone;
        QrCodeUrl = qrCodeUrl;
    }
}
