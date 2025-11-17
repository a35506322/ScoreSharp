using System.Text.RegularExpressions;
using ScoreSharp.Common.Constant;

namespace ScoreSharp.Common.Helpers.KYC;

public class KYCHelper
{
    private static readonly Regex TaiwanIdRegex = new(@"^[A-Z][12]\d{8}$", RegexOptions.Compiled);
    private static readonly Regex OldForeignIdRegex = new(@"^[A-Z]{2}\d{8}$", RegexOptions.Compiled);
    private static readonly Regex NewForeignIdRegex = new(@"^[A-Z]{1}[89]{1}\d{8}$", RegexOptions.Compiled);

    public static string 標註強押記號(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        return $"*{value}";
    }

    public static string 轉換ID(string iD)
    {
        if (TaiwanIdRegex.IsMatch(iD) || OldForeignIdRegex.IsMatch(iD) || NewForeignIdRegex.IsMatch(iD))
            return $"{iD}A";
        else
            return $"{iD}C";
    }

    public static object 產生KYC加強審核執行表(string version, string id, string name, string riskLevel)
    {
        if (version == "20210501")
        {
            var result = new KYCStrongReDetail_20210501();
            result.基本資料.所建立業務關係.信用卡業務 = true;
            result.基本資料.ID = id;
            result.基本資料.姓名 = name;
            if (riskLevel == KYCRiskLevelConst.高風險)
            {
                result.基本資料.客戶風險等級 = KYCRiskLevelConst.高風險;
            }
            else if (riskLevel == KYCRiskLevelConst.中風險)
            {
                result.基本資料.客戶風險等級 = KYCRiskLevelConst.中風險;
            }
            return result;
        }
        else
        {
            throw new Exception($"不支援的版本: {version}");
        }
    }
}
