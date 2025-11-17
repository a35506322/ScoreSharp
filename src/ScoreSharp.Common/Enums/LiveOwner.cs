namespace ScoreSharp.Common.Enums;

public enum LiveOwner
{
    [EnumIsActive(true)]
    本人 = 1,

    [EnumIsActive(true)]
    配偶 = 2,

    [EnumIsActive(true)]
    父母親 = 3,

    [EnumIsActive(true)]
    親屬 = 4,

    [EnumIsActive(true)]
    宿舍 = 5,

    [EnumIsActive(true)]
    租貸 = 6,

    [EnumIsActive(true)]
    其他 = 7,
}
