using System.Text.Json.Serialization;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardMyDataFail;

public class EcardMyDataFailRequest
{
    /// <summary>
    /// 正卡_身份證字號
    /// </summary>
    [JsonPropertyName("P_ID")]
    public string ID { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    [JsonPropertyName("APPLY_NO")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// MyData 案件編號
    /// </summary>
    [JsonPropertyName("MYDATA_NO")]
    public string MyDataNo { get; set; }
}

public class EcardMyDataFailResponse
{
    public EcardMyDataFailResponse(string id)
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

public class ValidateRequestResult
{
    public bool IsValid { get; set; }

    public EcardMyDataFailResponse? Response { get; set; }
    public string? ErrorType { get; set; }
}

public static class 回覆代碼
{
    public static readonly string 匯入成功 = "0000";
    public static readonly string 必要欄位為空值 = "0001";
    public static readonly string 長度過長 = "0002";
    public static readonly string 其它異常訊息 = "0003";
}

public class EcardMyDataInfoDto
{
    public string HandleSeqNo { get; set; }
    public int? PrePageNo { get; set; }
    public CardStatus CardStatus { get; set; }
    public string ApplyCardType { get; set; }
}
