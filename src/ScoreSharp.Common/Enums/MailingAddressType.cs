namespace ScoreSharp.Common.Enums;

public enum MailingAddressType
{
    [EnumIsActive(true)]
    帳單地址 = 1,

    [EnumIsActive(true)]
    戶籍地址 = 2,

    [EnumIsActive(true)]
    公司地址 = 3,

    [EnumIsActive(true)]
    居住地址 = 4,
}
