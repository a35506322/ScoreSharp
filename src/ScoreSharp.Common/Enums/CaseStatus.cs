namespace ScoreSharp.Common.Enums;

public enum CaseStatus
{
    [EnumIsActive(true)]
    網路件月收入確認 = 1,

    [EnumIsActive(true)]
    網路件人工審查 = 2,

    [EnumIsActive(true)]
    緊急製卡 = 3,

    [EnumIsActive(true)]
    補回件 = 4,

    [EnumIsActive(true)]
    拒件_撤件重審 = 5,

    [EnumIsActive(true)]
    網路件製卡失敗 = 6,

    [EnumIsActive(true)]
    紙本件月收入確認 = 7,

    [EnumIsActive(true)]
    紙本件人工審查 = 8,

    [EnumIsActive(true)]
    急件 = 9,

    [EnumIsActive(true)]
    退回重審 = 10,

    [EnumIsActive(true)]
    未補回 = 11,

    [EnumIsActive(true)]
    紙本件製卡失敗 = 12,
}
