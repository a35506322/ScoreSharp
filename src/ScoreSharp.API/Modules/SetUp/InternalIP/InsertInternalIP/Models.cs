namespace ScoreSharp.API.Modules.SetUp.InternalIP.InsertInternalIP;

public class InsertInternalIPRequest
{
    /// <summary>
    /// PK，範例 : 172.28.234.10
    /// </summary>
    [Display(Name = "IP")]
    [Required]
    [MaxLength(20)]
    public string IP { get; set; }

    /// <summary>
    /// 是否啟用， Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsActive { get; set; }
}
