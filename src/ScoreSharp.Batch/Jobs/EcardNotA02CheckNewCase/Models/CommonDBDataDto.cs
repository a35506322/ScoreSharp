namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class CommonDBDataDto
{
    /// <summary>
    /// 行內IP
    /// </summary>
    public List<string> InternalIPs { get; set; } = new();

    /// <summary>
    /// 過去3天案件資料
    /// </summary>
    public List<GetHistoryApplyCreditInfoForCheck> HistoryApplyCreditInfo { get; set; } = new();

    /// <summary>
    /// 地址資訊
    /// </summary>
    public List<SetUp_AddressInfo> AddressInfos { get; set; } = new();

    /// <summary>
    /// 系統參數
    /// </summary>
    public SysParamManage_SysParam SysParam { get; set; } = new();
}
