using System.ComponentModel.DataAnnotations;

namespace ScoreSharp.Batch.Infrastructures.Options;

public class ReportPath
{
    [Required]
    public string SupplementSignReportPath { get; set; }

    [Required]
    public string SupplementNoSignReportPath { get; set; }
}
