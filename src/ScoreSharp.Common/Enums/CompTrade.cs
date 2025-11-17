namespace ScoreSharp.Common.Enums;

public enum CompTrade
{
    [EnumIsActive(true)]
    [EnumName("金融業")]
    金融業 = 1,

    [EnumIsActive(true)]
    [EnumName("公務機關")]
    公務機關 = 2,

    [EnumIsActive(true)]
    [EnumName("營造/製造/運輸業")]
    營造_製造_運輸業 = 3,

    [EnumIsActive(true)]
    [EnumName("一般商業")]
    一般商業 = 4,

    [EnumIsActive(true)]
    [EnumName("休閒/娛樂/服務業")]
    休閒_娛樂_服務業 = 5,

    [EnumIsActive(true)]
    [EnumName("軍警消防業")]
    軍警消防業 = 6,

    [EnumIsActive(true)]
    [EnumName("非營利團體")]
    非營利團體 = 7,

    [EnumIsActive(true)]
    [EnumName("學生")]
    學生 = 8,

    [EnumIsActive(true)]
    [EnumName("自由業/其他")]
    自由業_其他 = 9,
}
