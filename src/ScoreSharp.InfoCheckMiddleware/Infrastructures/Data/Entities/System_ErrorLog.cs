using System;
using System.Collections.Generic;

namespace ScoreSharp.InfoCheckMiddleware.Infrastructures.Data.Entities;

public partial class System_ErrorLog
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string? ApplyNo { get; set; }

    /// <summary>
    /// 專案
    /// - API
    /// - BATCH
    /// - Middlewave
    /// </summary>
    public string Project { get; set; } = null!;

    /// <summary>
    /// 來源
    /// 範例:如非卡友檢核排程
    /// </summary>
    public string Source { get; set; } = null!;

    /// <summary>
    /// 類型
    /// 
    /// 第三方API呼叫
    /// 系統錯誤
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string ErrorMessage { get; set; } = null!;

    /// <summary>
    /// 錯誤詳細資訊
    /// </summary>
    public string? ErrorDetail { get; set; }

    /// <summary>
    /// 可用於放置參數，例如呼叫第三方API
    /// </summary>
    public string? Request { get; set; }

    /// <summary>
    /// 可用於放置回應，例如呼叫第三方API
    /// </summary>
    public string? Response { get; set; }

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 寄信狀態
    /// </summary>
    public SendStatus SendStatus { get; set; }

    /// <summary>
    /// 寄信時間
    /// </summary>
    public DateTime? SendEmailTime { get; set; }

    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string? FailLog { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? Note { get; set; }
}
