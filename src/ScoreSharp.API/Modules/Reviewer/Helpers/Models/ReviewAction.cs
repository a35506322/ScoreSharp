namespace ScoreSharp.Common.Enums;

public enum ReviewAction
{
    [EnumIsActive(true)]
    核卡作業 = 1,

    [EnumIsActive(true)]
    退件作業 = 2,

    [EnumIsActive(true)]
    補件作業 = 3,

    [EnumIsActive(true)]
    撤件作業 = 4,

    [EnumIsActive(true)]
    排入核卡 = 5,

    [EnumIsActive(true)]
    排入退件 = 6,

    [EnumIsActive(true)]
    排入補件 = 7,

    [EnumIsActive(true)]
    排入撤件 = 8,

    [EnumIsActive(true)]
    完成月收入確認 = 9,
}

