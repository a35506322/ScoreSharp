namespace ScoreSharp.Common.Enums;

public enum KYCLastSendStatus
{
    [EnumIsActive(true)]
    不需發送 = 1,

    [EnumIsActive(true)]
    等待 = 2,

    [EnumIsActive(true)]
    成功 = 3,

    [EnumIsActive(true)]
    失敗 = 4,
}
