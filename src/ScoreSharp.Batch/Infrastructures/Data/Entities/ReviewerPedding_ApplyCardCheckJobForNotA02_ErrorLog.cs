using System;
using System.Collections.Generic;

namespace ScoreSharp.Batch.Infrastructures.Data.Entities;

/// <summary>
/// 徵審代辦申請信用卡任務非原卡友_錯誤Log
/// </summary>
public partial class ReviewerPedding_ApplyCardCheckJobForNotA02_ErrorLog
{
    /// <summary>
    /// PK，字增
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// E-CARD申請書編號，
    /// 與ReviewerPedding_ApplyCardCheckJobForNotA02 關聯
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string ErrorMesaage { get; set; } = null!;

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime AddTime { get; set; }
}
