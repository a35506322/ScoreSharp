namespace ScoreSharp.Common.Enums;

/// <summary>
/// 人員指派變更狀態
/// </summary>
/// <remarks>
/// 強制派案
/// </remarks>
public enum AssignmentChangeStatus
{
    [EnumIsActive(true)]
    拒件重審 = 10251,

    [EnumIsActive(true)]
    補回件 = 10232,

    [EnumIsActive(true)]
    人工徵信中 = 10201,

    [EnumIsActive(true)]
    紙本件_待月收入預審 = 3,

    [EnumIsActive(true)]
    網路件_待月收入預審 = 30010,

    [EnumIsActive(true)]
    製卡失敗 = 10302,
}
