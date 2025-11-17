using System.Globalization;

namespace ScoreSharp.Common.Extenstions;

public static class StringExtensions
{
    /// <summary>
    /// 將西元日期字串轉換為民國日期字串
    /// </summary>
    /// <param name="westernDate">西元日期字串 (YYYYMMDD)</param>
    /// <returns>民國日期字串 (YYYMMDD)</returns>
    public static string ToTaiwanDate(this string westernDate)
    {
        if (string.IsNullOrEmpty(westernDate) || westernDate.Length != 8)
        {
            throw new ArgumentException("日期格式不正確，應該是8位數格式 (YYYYMMDD)", nameof(westernDate));
        }

        if (!DateTime.TryParseExact(westernDate, "yyyyMMdd", null, DateTimeStyles.None, out DateTime date))
        {
            throw new ArgumentException("無效的日期格式", nameof(westernDate));
        }

        TaiwanCalendar taiwanCalendar = new TaiwanCalendar();

        return $"{taiwanCalendar.GetYear(date):D3}" + $"{taiwanCalendar.GetMonth(date):D2}" + $"{taiwanCalendar.GetDayOfMonth(date):D2}";
    }

    /// <summary>
    /// 格式化固定格式的電話號碼
    /// 輸入格式：3碼-8碼#5碼 (沒有數字的部分會是空白)
    /// 輸出格式：移除空白，保持結構
    /// </summary>
    /// <param name="phoneNumber">原始電話號碼</param>
    public static string FormatFixedPhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return string.Empty;
        }

        // 分割 # 號前後
        var parts = phoneNumber.Split('#');
        var mainPart = parts[0]; // 前面的部分：3碼-8碼
        var extensionPart = parts.Length > 1 ? parts[1] : ""; // 後面5碼

        // 處理主要部分（3碼-8碼）
        var mainParts = mainPart.Split('-');
        var areaCode = mainParts.Length > 0 ? mainParts[0].Trim() : "";
        var phoneNum = mainParts.Length > 1 ? mainParts[1].Trim() : "";

        // 處理分機號碼（5碼）
        var extension = extensionPart.Trim();

        // 組合結果
        var result = "";

        // 只有當區域碼不為空時才加入
        if (!string.IsNullOrEmpty(areaCode))
        {
            result += areaCode;
        }

        // 只有當電話號碼不為空時才加入
        if (!string.IsNullOrEmpty(phoneNum))
        {
            if (!string.IsNullOrEmpty(result))
            {
                result += "-";
            }
            result += phoneNum;
        }

        // 只有當分機號碼不為空時才加入
        if (!string.IsNullOrEmpty(extension))
        {
            result += "#" + extension;
        }

        return result;
    }

    /// <summary>
    /// 將全形字元轉換為半形字元
    /// 支援數字、英文字母、標點符號和空白字元
    /// 中文字元會保持不變，轉換錯誤時返回原值
    /// </summary>
    /// <param name="input">輸入字串</param>
    /// <returns>轉換後的半形字串，轉換失敗時返回原字串</returns>
    public static string ToHalfWidth(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        try
        {
            var result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                // 全形空白 (U+3000) 轉半形空白 (U+0020)
                if (c == '\u3000')
                {
                    result[i] = ' ';
                }
                // 全形字元範圍 (U+FF01 到 U+FF5E) 轉對應半形字元 (U+0021 到 U+007E)
                else if (c >= '\uFF01' && c <= '\uFF5E')
                {
                    result[i] = (char)(c - 0xFEE0);
                }
                // 其他字元（包含中文）保持不變
                else
                {
                    result[i] = c;
                }
            }

            return new string(result);
        }
        catch
        {
            // 轉換錯誤時返回原值
            return input;
        }
    }

    /// <summary>
    /// /// 將民國日期字串轉換為西元日期字串
    /// </summary>
    /// <param name="taiwanDate">民國日期字串</param>
    /// <param name="inputFormat">輸入格式 (預設: yyyMMdd)</param>
    /// <param name="outputFormat">輸出格式 (預設: yyyyMMdd)</param>
    /// <returns>西元日期字串</returns>
    /// <exception cref="ArgumentException">當日期格式不正確時拋出</exception>
    /// <exception cref="ArgumentOutOfRangeException">當民國年份無效時拋出</exception>
    public static string ToWesternDate(this string taiwanDate, string inputFormat = "yyyMMdd", string outputFormat = "yyyyMMdd")
    {
        if (string.IsNullOrEmpty(taiwanDate))
        {
            throw new ArgumentException("民國日期字串不可為空", nameof(taiwanDate));
        }

        if (string.IsNullOrEmpty(inputFormat))
        {
            throw new ArgumentException("輸入格式不可為空", nameof(inputFormat));
        }

        if (string.IsNullOrEmpty(outputFormat))
        {
            throw new ArgumentException("輸出格式不可為空", nameof(outputFormat));
        }

        try
        {
            // 手動解析民國日期字串
            int taiwanYear,
                month,
                day,
                hour = 0,
                minute = 0,
                second = 0,
                millisecond = 0;

            if (inputFormat == "yyyMMdd" && taiwanDate.Length == 7)
            {
                // 標準格式：民國年3位數 + 月2位數 + 日2位數
                taiwanYear = int.Parse(taiwanDate.Substring(0, 3));
                month = int.Parse(taiwanDate.Substring(3, 2));
                day = int.Parse(taiwanDate.Substring(5, 2));
            }
            else
            {
                // 使用 DateTime.TryParseExact 解析其他格式，但先將民國年轉為西元年
                // 這裡需要更複雜的解析邏輯來處理不同的格式
                // 為了簡化，先創建一個臨時的西元日期字串來解析
                string tempWesternDate = taiwanDate;

                // 如果是標準的民國日期格式，需要特殊處理
                if (inputFormat.Contains("yyy"))
                {
                    // 找出年份部分並轉換
                    var yearStartIndex = inputFormat.IndexOf("yyy");
                    if (yearStartIndex >= 0 && taiwanDate.Length > yearStartIndex + 2)
                    {
                        var yearStr = taiwanDate.Substring(yearStartIndex, 3);
                        if (int.TryParse(yearStr, out int parsedTaiwanYear))
                        {
                            var westernYear = parsedTaiwanYear + 1911;
                            tempWesternDate =
                                taiwanDate.Substring(0, yearStartIndex) + westernYear.ToString("D4") + taiwanDate.Substring(yearStartIndex + 3);
                            var tempFormat = inputFormat.Replace("yyy", "yyyy");

                            if (
                                DateTime.TryParseExact(
                                    tempWesternDate,
                                    tempFormat,
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out DateTime tempDate
                                )
                            )
                            {
                                taiwanYear = parsedTaiwanYear;
                                month = tempDate.Month;
                                day = tempDate.Day;
                                hour = tempDate.Hour;
                                minute = tempDate.Minute;
                                second = tempDate.Second;
                                millisecond = tempDate.Millisecond;
                            }
                            else
                            {
                                throw new ArgumentException($"無法解析民國日期字串 '{taiwanDate}'，預期格式: {inputFormat}", nameof(taiwanDate));
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"無法解析民國日期字串 '{taiwanDate}'，預期格式: {inputFormat}", nameof(taiwanDate));
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"無法解析民國日期字串 '{taiwanDate}'，預期格式: {inputFormat}", nameof(taiwanDate));
                    }
                }
                else
                {
                    throw new ArgumentException($"不支援的輸入格式: {inputFormat}", nameof(inputFormat));
                }
            }

            // 使用 TaiwanCalendar.ToDateTime 將民國年月日轉換為西元 DateTime
            TaiwanCalendar taiwanCalendar = new TaiwanCalendar();
            DateTime westernDate = taiwanCalendar.ToDateTime(taiwanYear, month, day, hour, minute, second, millisecond, TaiwanCalendar.CurrentEra);

            // 輸出指定格式的西元日期字串
            return westernDate.ToString(outputFormat, CultureInfo.InvariantCulture);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new ArgumentOutOfRangeException(nameof(taiwanDate), $"民國日期 '{taiwanDate}' 超出有效範圍: {ex.Message}");
        }
        catch (Exception ex) when (!(ex is ArgumentException || ex is ArgumentOutOfRangeException))
        {
            throw new ArgumentException($"轉換民國日期 '{taiwanDate}' 時發生錯誤: {ex.Message}", nameof(taiwanDate), ex);
        }
    }

    /// <summary>
    /// 將半形字元轉換為全形字元
    /// 支援數字、英文字母、標點符號和空白字元
    /// 中文字元會保持不變，轉換錯誤時返回原值
    /// </summary>
    /// <param name="input">輸入字串</param>
    /// <returns>轉換後的全形字串，轉換失敗時返回原字串</returns>
    public static string ToFullWidth(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        try
        {
            var result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                // 半形空白 (U+0020) 轉全形空白 (U+3000)
                if (c == ' ')
                {
                    result[i] = '\u3000';
                }
                // 半形字元範圍 (U+0021 到 U+007E) 轉對應全形字元 (U+FF01 到 U+FF5E)
                else if (c >= '\u0021' && c <= '\u007E')
                {
                    result[i] = (char)(c + 0xFEE0);
                }
                // 其他字元（包含中文）保持不變
                else
                {
                    result[i] = c;
                }
            }

            return new string(result);
        }
        catch
        {
            // 轉換錯誤時返回原值
            return input;
        }
    }
}
