using System.Text.Json.Serialization;

namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.ExecWebRetryCaseBySeqNo.Models;

public class EcardNewCaseResponse
{
    public EcardNewCaseResponse(string id)
    {
        this.ID = id;
        this.RESULT = ConvertToChinese(id);
    }

    private string ConvertToChinese(string id) =>
        id switch
        {
            "0000" => "匯入成功",
            "0001" => "申請書編號長度不符",
            "0003" => "申請書編號重複進件或申請書編號不對",
            "0004" => "無法對應表單代碼",
            "0005" => "資料異常非定義值",
            "0006" => "資料異常資料長度過長",
            "0007" => "必要欄位不能為空值",
            "0008" => "申請書異常",
            "0009" => "附件異常",
            "0010" => "UUID重複",
            "0012" => "其它異常訊息",
            _ => throw new Exception($"未知的回覆代碼: {id}"),
        };

    /// <summary>
    /// 回覆代碼
    /// 0000
    /// 0001
    /// 0003
    /// 0004
    /// 0005
    /// 0006
    /// 0007
    /// 0008
    /// 0009
    /// 0010
    /// 0012
    /// </summary>
    [JsonPropertyName("ID")]
    public string ID { get; set; }

    /// <summary>
    /// 回覆結果(中文)
    /// 0000 = 「匯入成功」
    /// 0001 = 「申請書編號長度不符」
    /// 0003 = 「申請書編號重複進件」或「申請書編號不對」
    /// 0004 =「無法對應表單代碼」
    /// 0005 =「資料異常非定義值」
    /// 0006 =「資料異常資料長度過長」
    /// 0007 =「必要欄位不能為空值」
    /// 0008 =「申請書異常」
    /// 0009 =「附件異常」
    /// 0010 =「UUID重複」
    /// 0012 =「其它異常訊息
    /// </summary>
    [JsonPropertyName("RESULT")]
    public string RESULT { get; private set; }
}
