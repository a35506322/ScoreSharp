namespace ScoreSharp.Common.Enums;

/// <summary>
/// 確認關係
/// </summary>
public enum SameDataRelation
{
    [EnumIsActive(true)]
    父母 = 1,

    [EnumIsActive(true)]
    子女 = 2,

    [EnumIsActive(true)]
    配偶 = 3,

    [EnumIsActive(true)]
    兄弟姊妹 = 4,

    [EnumIsActive(true)]
    配偶父母 = 5,

    [EnumIsActive(true)]
    其他關係 = 6,

    [EnumIsActive(true)]
    無關係 = 7,
}
