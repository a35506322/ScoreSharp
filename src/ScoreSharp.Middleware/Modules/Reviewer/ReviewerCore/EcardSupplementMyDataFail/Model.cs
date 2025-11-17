using System.Text.Json.Serialization;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardSupplementMyDataFail;

public class EcardSupplementMyDataFailRequest
{
    /// <summary>
    /// 正卡_身份證字號
    /// </summary>
    [JsonPropertyName("P_ID")]
    public string ID { get; set; }
}

public class EcardSupplementMyDataFailResponse
{
    public EcardSupplementMyDataFailResponse(string id)
    {
        this.ID = id;
        this.RESULT = ConvertToChinese(id);
    }

    private string ConvertToChinese(string id) =>
        id switch
        {
            "0000" => "匯入成功",
            "0001" => "必要欄位為空值",
            "0002" => "長度過長",
            "0003" => "其他錯誤訊息",
            _ => throw new Exception($"未知的回覆代碼: {id}"),
        };

    /// <summary>
    /// 回覆代碼
    /// 0000
    /// 0001
    /// 0002
    /// 0003
    /// </summary>
    [JsonPropertyName("ID")]
    public string ID { get; set; }

    /// <summary>
    /// 回覆結果(中文)
    /// 0000 = 「匯入成功」
    /// 0001 = 「必要欄位為空值」
    /// 0001 = 「長度過長」
    /// 0003 = 「其他錯誤訊息」
    /// </summary>
    [JsonPropertyName("RESULT")]
    public string RESULT { get; private set; }
}

public static class 回覆代碼
{
    public static readonly string 匯入成功 = "0000";
    public static readonly string 必要欄位為空值 = "0001";
    public static readonly string 長度過長 = "0002";
    public static readonly string 其它異常訊息 = "0003";
}

public class ECardSupplementInfo
{
    public string HandleSeqNo { get; set; }
    public string ApplyNo { get; set; }
    public int PrePageNo { get; set; }
    public DateTime ApplyDate { get; set; }
    public CardStatus CardStatus { get; set; }
    public UserType UserType { get; set; }
    public string ID { get; set; }
    public CardStep? CardStep { get; set; }
    public Source Source { get; set; }
    public string ApplyCardType { get; set; }
}

public class ECardSupplementInfoGroupByApplyNo
{
    public string ApplyNo { get; set; }
    public List<ECardSupplementInfo> ECardSupplementInfos { get; set; } = new();
}

public class ECardSupplementInfoContext
{
    public string ApplyNo { get; set; }
    public List<HandleInfo> HandleInfos { get; set; } = new();
    public List<Reviewer_ApplyCreditCardInfoProcess> Processes { get; set; } = new();
    public List<Reviewer_CardRecord> CardRecords { get; set; } = new();
}

public class HandleInfo
{
    public string HandleSeqNo { get; set; }
    public string ApplyNo { get; set; }
    public CardStatus OriginCardStatus { get; set; }
    public CardStatus AfterCardStatus { get; set; }
}
