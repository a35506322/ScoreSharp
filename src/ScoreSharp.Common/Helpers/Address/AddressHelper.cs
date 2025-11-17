using System.Text.RegularExpressions;

namespace ScoreSharp.Common.Helpers.Address;

public class AddressHelper
{
    public static (string City, string District) GetCityAndDistrict(string fullAddress)
    {
        if (string.IsNullOrEmpty(fullAddress))
            return (string.Empty, string.Empty);

        // 台灣縣市的正規表達式模式
        string cityPattern =
            @"(臺北市|新北市|桃園市|臺中市|臺南市|高雄市|新竹市|嘉義市|基隆市|宜蘭縣|新竹縣|苗栗縣|彰化縣|南投縣|雲林縣|嘉義縣|屏東縣|臺東縣|花蓮縣|澎湖縣|金門縣|連江縣|釣魚臺|南海島)";
        // 區域的正規表達式模式
        string districtPattern = @"([^市縣]+?[區鄉鎮市])";

        string city = string.Empty;
        string district = string.Empty;

        // 提取縣市
        Match cityMatch = Regex.Match(fullAddress, cityPattern);
        if (cityMatch.Success)
        {
            city = cityMatch.Groups[1].Value;

            // 從縣市後面開始尋找區域
            string remainingAddress = fullAddress.Substring(cityMatch.Index + city.Length);
            Match districtMatch = Regex.Match(remainingAddress, districtPattern);

            if (districtMatch.Success)
            {
                district = districtMatch.Groups[1].Value;
            }
        }

        return (city, district);
    }

    public static string ZipCodeFormatZero(string zipCode, int digitsToZero)
    {
        if (string.IsNullOrEmpty(zipCode) || zipCode.Length <= digitsToZero)
            return zipCode;

        string prefix = zipCode.Substring(0, zipCode.Length - digitsToZero);
        string zeros = new string('0', digitsToZero);
        return prefix + zeros;
    }

    public static bool HasValidTaiwanPostalCode(string fullAddress)
    {
        if (string.IsNullOrWhiteSpace(fullAddress))
            return false;

        Match match = Regex.Match(fullAddress.Trim(), @"^(\d{3})\d{0,2}\s+");
        if (!match.Success)
            return false;

        return true;
    }

    public static (string PostalCode, string Address) GetPostalCodeAndAddress(string fullAddress)
    {
        if (string.IsNullOrWhiteSpace(fullAddress))
            return (string.Empty, string.Empty);

        string trimmedAddress = fullAddress.Replace(" ", "").Replace("　", "").Trim();

        // 檢查是否以郵遞區號開頭（3-5位數字）
        Match match = Regex.Match(trimmedAddress, @"^(\d+)?([^\d].*)$");

        if (match.Success)
        {
            string postalCode = match.Groups[1].Value;
            string address = match.Groups[2].Value.Trim();
            address = 將縣市台字轉換為臺字(address);
            return (postalCode, address);
        }

        trimmedAddress = 將縣市台字轉換為臺字(trimmedAddress);
        return (string.Empty, trimmedAddress);
    }

    public static string 將縣市台字轉換為臺字(string noZipCodeFullAddress)
    {
        if (string.IsNullOrWhiteSpace(noZipCodeFullAddress))
            return noZipCodeFullAddress;

        // 只轉換縣市名稱中的台字：台北市、台中市、台南市、台東縣
        noZipCodeFullAddress = noZipCodeFullAddress.Replace("台北市", "臺北市");
        noZipCodeFullAddress = noZipCodeFullAddress.Replace("台中市", "臺中市");
        noZipCodeFullAddress = noZipCodeFullAddress.Replace("台南市", "臺南市");
        noZipCodeFullAddress = noZipCodeFullAddress.Replace("台東縣", "臺東縣");

        return noZipCodeFullAddress;
    }

    public static string FindZipCode(List<AddressInfoDto> addressInfos, SearchAddressInfoDto searchAddressInfo)
    {
        try
        {
            foreach (var rule in addressInfos)
            {
                if (rule.City == searchAddressInfo.City && rule.Area == searchAddressInfo.District && rule.Road == searchAddressInfo.Road)
                {
                    var matchScopeDto = new MatchScopeDto
                    {
                        Scope = rule.ScopeArray,
                        Number = searchAddressInfo.Number,
                        Lane = searchAddressInfo.Lane,
                        SubNumber = searchAddressInfo.SubNumber,
                    };

                    if (MatchScope(matchScopeDto))
                        return rule.ZipCode;
                }
            }

            // Tips: 如果沒有找到符合的郵遞區號，則回傳第一個符合的郵遞區號並後兩碼變零
            var firstMatch = addressInfos.FirstOrDefault(x => x.City == searchAddressInfo.City && x.Area == searchAddressInfo.District);
            if (firstMatch != null)
            {
                return AddressHelper.ZipCodeFormatZero(firstMatch.ZipCode, 2);
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }

    private const string 連 = "c";
    private const string 偶數 = "even";
    private const string 奇數 = "odd";
    private const string 小於 = "lower";
    private const string 大於 = "upper";
    private const string 到 = "to";
    private const string 之 = "of";
    private const string 巷子 = "ln";
    private const string 全部 = "all";

    private static bool MatchScope(MatchScopeDto dto)
    {
        /*
            scope 情況
            1. ["all"]
            2. ["even", "all"]
            3. ["odd", "all"]
            4. ["even", "100", "lower"]
            5. ["even", "100", "upper"]
            6. ["odd", "100", "lower"]
            7. ["odd", "100", "upper"]
            8. ["even", "100", "to", "150"]
            9. ["odd", "100", "to", "150"]
            10. ["114"]
            11. ["92ln","all"]
            12. ["odd","59","to","99","of","11"]
            13. ["even","18","to","34","of","1"]
            14. ["c","78","lower"]
            15. ["c","2","upper"]
            16. ["even","34","of","3","lower"]
            17. ["even","46","of","7","upper"]
            18. ["1","to","1","of","2"]
            19. ["24ln","c","15","lower"]
            20. ["even","206ln","to","258"]
            21. ["65ln","odd","all"]
            22. ["65ln","even","all"]
            23. ["201","of","1","to","of","39"]


            目前先不做
            1. ["167","2","to","4flr"]

            Todo:
            SeqNo	ZIPCode	City	Area	Road	Scope	IsActive	Y
            00988	10358	臺北市	大同區	歸綏街	["even","122","to","160ln"]	Y
            00990	10347	臺北市	大同區	歸綏街	["even","206ln","to","258"]	Y
            00908	10374	臺北市	大同區	重慶北路三段	["252ln","c","8","upper"]	Y
            00846	10374	臺北市	大同區	迪化街二段	["odd","187","to","215","all"]	Y
            00798	10353	臺北市	大同區	南京西路	["25ln","even","18","of","4","upper"]	Y

        */
        bool isEven = dto.Number % 2 == 0;

        (string[] scope, int number, int lane, int subNumber) = dto;

        if (scope.Length == 1)
        {
            if (scope[0] == 全部)
                return true;
            else if (int.TryParse(scope[0], out int number2) && number == number2)
                return true;
            return false;
        }
        else if (scope.Length == 2)
        {
            if (scope[0] == 偶數 && scope[1] == 全部 && isEven)
            {
                return true;
            }
            else if (scope[0] == 奇數 && scope[1] == 全部 && !isEven)
            {
                return true;
            }
            else if (scope[0].EndsWith(巷子) && scope[1] == 全部)
            {
                int _ln = int.Parse(scope[0].Replace(巷子, ""));
                return lane == _ln;
            }
            else if (scope[0] == 連 && scope[1] == 大於)
            {
                return number >= int.Parse(scope[2]);
            }
            else if (scope[0] == 連 && scope[1] == 小於)
            {
                return number <= int.Parse(scope[2]);
            }
        }
        else if (scope.Length == 3)
        {
            if (scope[0] == 偶數 && scope[2] == 小於 && isEven)
            {
                return number <= int.Parse(scope[1]);
            }
            else if (scope[0] == 偶數 && scope[2] == 大於 && isEven)
            {
                return number >= int.Parse(scope[1]);
            }
            else if (scope[0] == 奇數 && scope[2] == 小於 && !isEven)
            {
                return number <= int.Parse(scope[1]);
            }
            else if (scope[0] == 奇數 && scope[2] == 大於 && !isEven)
            {
                return number >= int.Parse(scope[1]);
            }
            else if (scope[0] == 連 && scope[2] == 小於)
            {
                return number <= int.Parse(scope[1]);
            }
            else if (scope[0] == 連 && scope[2] == 大於)
            {
                return number >= int.Parse(scope[1]);
            }
            else if (scope[0].EndsWith(巷子) && scope[2] == 全部 && !isEven)
            {
                return lane == int.Parse(scope[0].Replace(巷子, ""));
            }
            else if (scope[0].EndsWith(巷子) && scope[2] == 全部 && isEven)
            {
                return lane == int.Parse(scope[0].Replace(巷子, ""));
            }
        }
        else if (scope.Length == 4)
        {
            if (scope[0] == 偶數 && scope[2] == 到 && scope[1].EndsWith(巷子) == false && isEven)
            {
                return number >= int.Parse(scope[1]) && number <= int.Parse(scope[3]);
            }
            else if (scope[0] == 奇數 && scope[2] == 到 && !isEven)
            {
                return number >= int.Parse(scope[1]) && number <= int.Parse(scope[3]);
            }
            else if (scope[0].EndsWith(巷子) && scope[1] == 連 && scope[3] == 小於)
            {
                int _ln = int.Parse(scope[0].Replace(巷子, ""));
                return lane == _ln && number <= int.Parse(scope[2]);
            }
            else if (scope[0] == 偶數 && scope[1].EndsWith(巷子) && scope[2] == 到 && isEven)
            {
                int _ln = int.Parse(scope[1].Replace(巷子, ""));
                return lane == _ln && number <= int.Parse(scope[3]);
            }
        }
        else if (scope.Length == 5)
        {
            if (scope[0] == 偶數 && scope[2] == 之 && scope[4] == 小於 && isEven)
            {
                return number <= int.Parse(scope[1]) && subNumber == int.Parse(scope[3]);
            }
            else if (scope[0] == 偶數 && scope[2] == 之 && scope[4] == 大於 && isEven)
            {
                return number >= int.Parse(scope[1]) && subNumber == int.Parse(scope[3]);
            }
            else if (scope[1] == 到 && scope[3] == 之)
            {
                int mainNumber = int.Parse(scope[0]);
                int endMainNumber = int.Parse(scope[2]);
                int endSubNumber = int.Parse(scope[4]);

                return number == mainNumber && number == endMainNumber && (subNumber == 0 || (subNumber >= 1 && subNumber <= endSubNumber));
            }
        }
        else if (scope.Length == 6)
        {
            if (scope[0] == 奇數 && scope[2] == 到 && scope[4] == 之 && !isEven)
            {
                return number >= int.Parse(scope[1]) && number <= int.Parse(scope[3]) && int.Parse(scope[5]) == subNumber;
            }
            else if (scope[0] == 偶數 && scope[2] == 到 && scope[4] == 之 && isEven)
            {
                return number >= int.Parse(scope[1]) && number <= int.Parse(scope[3]) && int.Parse(scope[5]) == subNumber;
            }
            else if (scope[1] == 之 && scope[3] == 到 && scope[4] == 之)
            {
                int mainNumber = int.Parse(scope[0]);
                int startSubNumber = int.Parse(scope[2]);
                int endSubNumber = int.Parse(scope[5]);

                return number == mainNumber && (subNumber == 0 || (subNumber >= startSubNumber && subNumber <= endSubNumber));
            }
        }
        return false;
    }
}
