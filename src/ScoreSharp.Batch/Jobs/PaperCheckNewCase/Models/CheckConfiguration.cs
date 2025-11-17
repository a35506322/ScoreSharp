namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

/// <summary>
/// 檢核配置類別，統一管理各項檢核設定
/// </summary>
public class CheckConfiguration
{
    public bool 是否檢核原持卡人 { get; set; }
    public bool 是否檢核行內Email { get; set; }
    public bool 是否檢核行內Mobile { get; set; }
    public bool 是否檢核姓名檢核 { get; set; }
    public bool 是否檢核929 { get; set; }
    public bool 是否檢核分行資訊 { get; set; }
    public bool 是否檢核關注名單 { get; set; }
    public bool 是否檢核頻繁ID { get; set; }
    public bool 是否檢查重覆進件 { get; set; }

    /// <summary>
    /// 為附卡人調整檢核配置
    /// 附卡人不需要檢核某些項目
    /// </summary>
    public CheckConfiguration ForSupplementary()
    {
        return new CheckConfiguration
        {
            是否檢核原持卡人 = this.是否檢核原持卡人,
            是否檢核行內Email = false, // 附卡不檢核Email
            是否檢核行內Mobile = false, // 附卡不檢核Mobile
            是否檢核姓名檢核 = this.是否檢核姓名檢核,
            是否檢核929 = this.是否檢核929,
            是否檢核分行資訊 = false, // 附卡不檢核分行資訊
            是否檢核關注名單 = this.是否檢核關注名單,
            是否檢核頻繁ID = false, // 附卡不檢核頻繁ID
            是否檢查重覆進件 = this.是否檢查重覆進件,
        };
    }
}
