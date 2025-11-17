using System.ComponentModel.DataAnnotations;

namespace ScoreSharp.Batch.Infrastructures.Options;

public class TemplateReportOption
{
    [Required]
    public string SupplementTemplateId { get; set; }
}
