namespace ScoreSharp.Common.Enums;

public enum SupplementContactRecordsResult
{
    [EnumIsActive(true)]
    客戶願意補件 = 1,

    [EnumIsActive(true)]
    客戶願意補件_且已線上補回資料 = 2,

    [EnumIsActive(true)]
    客戶不願意補件_以現有資料處理 = 3,

    [EnumIsActive(true)]
    客戶不願意補件_要求撤件 = 4,

    [EnumIsActive(true)]
    客戶考慮中 = 5,

    [EnumIsActive(true)]
    客戶未接電話 = 6,

    [EnumIsActive(true)]
    客戶要求稍後再聯繫 = 7,
}
