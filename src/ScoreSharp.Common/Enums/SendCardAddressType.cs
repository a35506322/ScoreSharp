namespace ScoreSharp.Common.Enums;

public enum SendCardAddressType
{
    [EnumIsActive(true)]
    同戶籍地址 = 1,

    [EnumIsActive(true)]
    同居住地址 = 2,

    [EnumIsActive(true)]
    同帳單地址 = 3,

    [EnumIsActive(true)]
    同公司地址 = 4,

    [EnumIsActive(true)]
    親領 = 5,

    [EnumIsActive(true)]
    其他 = 6,
}
