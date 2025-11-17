namespace ScoreSharp.Common.Enums;

public enum ManualReviewAction_AuthOut
{
    [EnumIsActive(true)]
    排入核卡 = 5,

    [EnumIsActive(true)]
    排入退件 = 6,

    [EnumIsActive(true)]
    排入補件 = 7,

    [EnumIsActive(true)]
    排入撤件 = 8,
}
