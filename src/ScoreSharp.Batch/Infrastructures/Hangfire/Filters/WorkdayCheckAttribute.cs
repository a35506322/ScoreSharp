using System;

namespace ScoreSharp.Batch.Infrastructures.Hangfire.Filters;

/// <summary>
/// 工作日檢查標記屬性
/// 套用此屬性的 Job 將在執行前檢查是否為工作日
/// 若為假日則跳過執行
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class WorkdayCheckAttribute : Attribute
{
    /// <summary>
    /// 初始化工作日檢查屬性
    /// </summary>
    public WorkdayCheckAttribute() { }
}
