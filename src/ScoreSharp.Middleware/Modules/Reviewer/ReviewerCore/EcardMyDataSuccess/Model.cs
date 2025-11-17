using System.Text.Json.Serialization;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardMyDataSuccess;

public class EcardMyDataSuccessRequest
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

    /// <summary>
    /// 附件檔名1
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_01")]
    public string? AppendixFileName_01 { get; set; }

    /// <summary>
    /// 附件檔名2
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_02")]
    public string? AppendixFileName_02 { get; set; }

    /// <summary>
    /// 附件檔名3
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_03")]
    public string? AppendixFileName_03 { get; set; }

    /// <summary>
    /// 附件檔名4
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_04")]
    public string? AppendixFileName_04 { get; set; }

    /// <summary>
    /// 附件檔名5
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_05")]
    public string? AppendixFileName_05 { get; set; }

    /// <summary>
    /// 附件檔名6
    /// </summary>
    [JsonPropertyName("APPENDIX_FILE_NAME_06")]
    public string? AppendixFileName_06 { get; set; }
}

public class EcardMyDataSuccessResponse
{
    public EcardMyDataSuccessResponse(string id)
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

    public EcardMyDataSuccessResponse? Response { get; set; }

    public string? ErrorType { get; set; }
}

public static class 回覆代碼
{
    public static readonly string 匯入成功 = "0000";
    public static readonly string 必要欄位為空值 = "0001";
    public static readonly string 長度過長 = "0002";
    public static readonly string 其它異常訊息 = "0003";
}

public class SaveFileInfoDto
{
    public string ApplyNo { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
}

public class EcardMyDataInfoDto
{
    public string HandleSeqNo { get; set; }
    public int? PrePageNo { get; set; }
    public CardStatus CardStatus { get; set; }
    public string ApplyCardType { get; set; }
}

public class EcardMyDataSuccessContext
{
    public string HandleSeqNo { get; set; }
    public string ApplyNo { get; set; }
    public int? PrePageNo { get; set; }
    public string ApplyCardType { get; set; }
    public CardStatus OriginCardStatus { get; set; }
    public CardStatus AfterCardStatus { get; set; }
    public List<Reviewer_ApplyCreditCardInfoFile> FileLogs { get; set; } = new();
    public List<Reviewer_ApplyCreditCardInfoProcess> Processes { get; set; } = new();
    public List<Reviewer_ApplyFile> ApplyFiles { get; set; } = new();
}
