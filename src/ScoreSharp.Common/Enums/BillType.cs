namespace ScoreSharp.Common.Enums;

public enum BillType
{
    [EnumIsActive(true)]
    電子帳單 = 1,

    [EnumIsActive(true)]
    簡訊帳單 = 2,

    [EnumIsActive(true)]
    紙本帳單 = 3,

    [EnumIsActive(true)]
    LINE帳單 = 4,
}
