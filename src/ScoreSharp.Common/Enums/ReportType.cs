namespace ScoreSharp.Common.Enums;

public enum ReportType
{
    [EnumIsActive(true)]
    核卡通知書 = 1,

    [EnumIsActive(true)]
    拒件函_信用卡 = 2,

    [EnumIsActive(true)]
    補件函_包含簽名函 = 3,

    [EnumIsActive(true)]
    補件函_不包含簽名函 = 4,
}
