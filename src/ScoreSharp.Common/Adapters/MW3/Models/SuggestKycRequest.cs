using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreSharp.Common.Adapters.MW3.Models;

public class SuggestKycRequest
{
    /// <summary>
    /// API 名稱
    /// </summary>
    /// <value>
    /// 固定值：KYC00CREDIT
    /// </value>
    [JsonPropertyName("apiName")]
    public string ApiName { get; private set; } = "KYC00SECREDIT";

    /// <summary>
    /// 夾帶 header
    /// </summary>
    [JsonPropertyName("headers")]
    public SuggestKycMW3Headers Headers { get; set; } = new();

    /// <summary>
    /// EAIHUB Request
    /// </summary>
    [JsonPropertyName("info")]
    public SuggestKycMW3Info Info { get; set; } = new();
}

public class SuggestKycMW3Headers
{
    /// <summary>
    /// 授權
    /// </summary>
    /// <value>
    /// TEST：Basic Y3JkU1M6Y3JkU1M= <br/>
    /// PROD：
    /// </value>
    [JsonPropertyName("Authorization")]
    public string Authorization { get; set; } = string.Empty;
}

public class SuggestKycMW3Info
{
    /// <summary>
    /// 交易類型
    /// </summary>
    /// <value>
    /// 固定值：KYC00CREDIT
    /// </value>
    [JsonPropertyName("_RestType")]
    public string RestType { get; private set; } = "KYC00SECREDIT";

    /// <summary>
    /// 身份證字號
    /// </summary>
    /// <remarks>
    /// 本國自然人：
    /// 1.身分證字號 + A
    ///
    /// 外國自然人：
    /// 1.舊稅籍編號(西元出生年月日8碼數字＋護照英文名前2碼) + C
    /// 2.統一證號/居留證號(二碼英文＋8碼數字) + A
    /// 3.新式居留證號(1碼英文＋數字8或9＋8碼數字) + A
    /// </remarks>
    [JsonPropertyName("uninumber")]
    public string ID { get; set; } = string.Empty;

    /// <summary>
    /// 經辦審查意見
    /// </summary>
    /// <remarks>
    /// 此為重要欄位攸關是變更還是撤退件
    /// <br/>
    /// 定義值
    /// Y：建議核准
    /// N：建議拒絕
    /// <br/>
    /// 邏輯
    /// 1. 新建立時固定帶Y
    /// 2. 若退件或撤件時加N
    /// 3. 若取消退撤件再報Y
    /// 4. 必要欄住異動重拋時下Y
    /// </remarks>
    [JsonPropertyName("uOpnion")]
    public string UOpnion { get; set; } = string.Empty;
}
