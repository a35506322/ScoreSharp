namespace ScoreSharp.Common.Enums;

public enum CaseCheckStatus
{
    [EnumIsActive(true)]
    需檢核_未完成 = 1,

    [EnumIsActive(true)]
    需檢核_成功 = 2,

    [EnumIsActive(true)]
    需檢核_失敗 = 3,

    [EnumIsActive(true)]
    不需檢核 = 4,
}
