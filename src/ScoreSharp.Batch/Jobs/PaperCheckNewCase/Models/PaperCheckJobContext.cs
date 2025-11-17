namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

public class PaperCheckJobContext
{
    public string ApplyNo { get; set; } = string.Empty;
    public CardOwner CardOwner { get; set; }

    /// <summary>
    /// 是否檢驗完畢
    /// 完成 = 1
    /// 未完成 = 2
    /// </summary>
    public CaseCheckedStatus IsChecked { get; set; }

    /// <summary>
    /// 錯誤次數
    /// 預設 0
    /// 上述檢核有錯誤記一次
    /// 如果成功則變為0
    /// </summary>
    public int ErrorCount { get; set; }

    public List<UserCheckResult> UserCheckResults { get; set; } = new List<UserCheckResult>();

    // 計算屬性：根據使用者檢核結果來判斷案件是否需要檢核特定項目
    public bool 案件是否檢核原持卡人 => UserCheckResults.Any(x => x.是否檢核原持卡人);
    public bool 案件是否檢核行內Email => UserCheckResults.Any(x => x.是否檢核行內Email);
    public bool 案件是否檢核行內Mobile => UserCheckResults.Any(x => x.是否檢核行內Mobile);
    public bool 案件是否檢核姓名檢核 => UserCheckResults.Any(x => x.是否檢核姓名檢核);
    public bool 案件是否檢核929 => UserCheckResults.Any(x => x.是否檢核929);
    public bool 案件是否檢核分行資訊 => UserCheckResults.Any(x => x.是否檢核分行資訊);
    public bool 案件是否檢核關注名單 => UserCheckResults.Any(x => x.是否檢核關注名單);
    public bool 案件是否檢核頻繁ID => UserCheckResults.Any(x => x.是否檢核頻繁ID);
    public bool 案件是否檢查重覆進件 => UserCheckResults.Any(x => x.是否檢查重覆進件);

    /// <summary>
    /// 檢查是否有任何檢核失敗
    /// </summary>
    public bool HasAnyCheckFailed()
    {
        return UserCheckResults.Any(user => user.HasAnyCheckFailed());
    }

    /// <summary>
    /// 檢查是否有任何檢核失敗
    /// </summary>
    public List<System_ErrorLog> GetFailedSystemErrorLogs()
    {
        var failedSystemErrorLogs = new List<System_ErrorLog>();

        foreach (var userCheckResult in UserCheckResults)
        {
            if (userCheckResult.是否檢核原持卡人成功 == 檢核結果.失敗)
                failedSystemErrorLogs.AddRange(userCheckResult.原持卡人查詢結果.ErrorData);

            if (userCheckResult.是否檢核行內Email成功 == 檢核結果.失敗)
                failedSystemErrorLogs.AddRange(userCheckResult.行內Email檢核結果.ErrorData);

            if (userCheckResult.是否檢核行內Mobile成功 == 檢核結果.失敗)
                failedSystemErrorLogs.AddRange(userCheckResult.行內Mobile檢核結果.ErrorData);

            if (userCheckResult.是否檢核姓名檢核成功 == 檢核結果.失敗)
                failedSystemErrorLogs.AddRange(userCheckResult.姓名檢核結果.ErrorData);

            if (userCheckResult.是否檢核929成功 == 檢核結果.失敗)
                failedSystemErrorLogs.AddRange(userCheckResult.查詢929結果.ErrorData);

            if (userCheckResult.是否檢核分行資訊成功 == 檢核結果.失敗)
                failedSystemErrorLogs.AddRange(userCheckResult.分行資訊查詢結果.ErrorData);

            if (userCheckResult.是否檢核關注名單成功 == 檢核結果.失敗)
                failedSystemErrorLogs.AddRange(userCheckResult.關注名單查詢結果.ErrorData);

            if (userCheckResult.是否檢核頻繁ID成功 == 檢核結果.失敗)
                failedSystemErrorLogs.AddRange(userCheckResult.頻繁ID檢核結果.ErrorData);

            if (userCheckResult.是否檢查重覆進件成功 == 檢核結果.失敗)
                failedSystemErrorLogs.AddRange(userCheckResult.重覆進件檢核結果.ErrorData);
        }

        return failedSystemErrorLogs.Distinct().ToList();
    }
}

public class UserCheckResult
{
    public string ID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string IsOriginalCardholder { get; set; } = string.Empty;
    public UserType UserType { get; set; }

    public bool 是否檢核原持卡人 { get; set; }
    public bool 是否檢核行內Email { get; set; }
    public bool 是否檢核行內Mobile { get; set; }
    public bool 是否檢核姓名檢核 { get; set; }
    public bool 是否檢核929 { get; set; }
    public bool 是否檢核分行資訊 { get; set; }
    public bool 是否檢核關注名單 { get; set; }
    public bool 是否檢核頻繁ID { get; set; }
    public bool 是否檢查重覆進件 { get; set; }

    public 檢核結果 是否檢核原持卡人成功 { get; set; }
    public 檢核結果 是否檢核行內Email成功 { get; set; }
    public 檢核結果 是否檢核行內Mobile成功 { get; set; }
    public 檢核結果 是否檢核姓名檢核成功 { get; set; }
    public 檢核結果 是否檢核929成功 { get; set; }
    public 檢核結果 是否檢核分行資訊成功 { get; set; }
    public 檢核結果 是否檢核關注名單成功 { get; set; }
    public 檢核結果 是否檢核頻繁ID成功 { get; set; }
    public 檢核結果 是否檢查重覆進件成功 { get; set; }

    public 命中檢核結果 命中檢核原持卡人 { get; set; }
    public 命中檢核結果 命中檢核行內Email { get; set; }
    public 命中檢核結果 命中檢核行內Mobile { get; set; }
    public 命中檢核結果 命中檢核姓名檢核 { get; set; }
    public 命中檢核結果 命中檢核929 { get; set; }
    public 命中檢核結果 命中檢核分行資訊 { get; set; }
    public 命中檢核結果 命中檢核關注名單1 { get; set; }
    public 命中檢核結果 命中檢核關注名單2 { get; set; }
    public 命中檢核結果 命中檢核頻繁ID { get; set; }
    public 命中檢核結果 命中檢查重覆進件 { get; set; }
    public CheckCaseRes<QueryOriginalCardholderData> 原持卡人查詢結果 { get; set; } = new();
    public CheckCaseRes<QueryCheckName> 姓名檢核結果 { get; set; } = new();
    public CheckCaseRes<List<Reviewer_BankInternalSameLog>> 行內Email檢核結果 { get; set; } = new();
    public CheckCaseRes<List<Reviewer_BankInternalSameLog>> 行內Mobile檢核結果 { get; set; } = new();
    public CheckCaseRes<Query929Info> 查詢929結果 { get; set; } = new();
    public CheckCaseRes<QueryBranchInfo> 分行資訊查詢結果 { get; set; } = new();
    public CheckCaseRes<ConcernDetailInfo> 關注名單查詢結果 { get; set; } = new();
    public CheckCaseRes<List<Reviewer_CheckTrace>> 頻繁ID檢核結果 { get; set; } = new();
    public CheckCaseRes<string> 重覆進件檢核結果 { get; set; } = new();

    /// <summary>
    /// 使用工廠方法建立 UserCheckResult
    /// </summary>
    public static UserCheckResult Create(
        string id,
        string name,
        string email,
        string mobile,
        UserType userType,
        CheckConfiguration config,
        string isOriginalCardholder
    )
    {
        var result = new UserCheckResult
        {
            ID = id,
            Name = name,
            Email = email,
            Mobile = mobile,
            UserType = userType,
            IsOriginalCardholder = isOriginalCardholder,
        };

        result.ApplyCheckConfiguration(config);
        return result;
    }

    /// <summary>
    /// 套用檢核配置
    /// </summary>
    public void ApplyCheckConfiguration(CheckConfiguration config)
    {
        是否檢核原持卡人 = config.是否檢核原持卡人;
        是否檢核行內Email = config.是否檢核行內Email;
        是否檢核行內Mobile = config.是否檢核行內Mobile;
        是否檢核姓名檢核 = config.是否檢核姓名檢核;
        是否檢核929 = config.是否檢核929;
        是否檢核分行資訊 = config.是否檢核分行資訊;
        是否檢核關注名單 = config.是否檢核關注名單;
        是否檢核頻繁ID = config.是否檢核頻繁ID;
        是否檢查重覆進件 = config.是否檢查重覆進件;

        // 根據檢核配置設置初始狀態
        InitializeCheckResults();
    }

    /// <summary>
    /// 初始化檢核結果
    /// </summary>
    private void InitializeCheckResults()
    {
        是否檢核原持卡人成功 = 是否檢核原持卡人 ? 檢核結果.等待 : 檢核結果.不須檢核;
        是否檢核行內Email成功 = 是否檢核行內Email ? 檢核結果.等待 : 檢核結果.不須檢核;
        是否檢核行內Mobile成功 = 是否檢核行內Mobile ? 檢核結果.等待 : 檢核結果.不須檢核;
        是否檢核姓名檢核成功 = 是否檢核姓名檢核 ? 檢核結果.等待 : 檢核結果.不須檢核;
        是否檢核929成功 = 是否檢核929 ? 檢核結果.等待 : 檢核結果.不須檢核;
        是否檢核分行資訊成功 = 是否檢核分行資訊 ? 檢核結果.等待 : 檢核結果.不須檢核;
        是否檢核關注名單成功 = 是否檢核關注名單 ? 檢核結果.等待 : 檢核結果.不須檢核;
        是否檢核頻繁ID成功 = 是否檢核頻繁ID ? 檢核結果.等待 : 檢核結果.不須檢核;
        是否檢查重覆進件成功 = 是否檢查重覆進件 ? 檢核結果.等待 : 檢核結果.不須檢核;

        命中檢核原持卡人 = 是否檢核原持卡人 ? 命中檢核結果.等待 : 命中檢核結果.不須檢核;
        命中檢核行內Email = 是否檢核行內Email ? 命中檢核結果.等待 : 命中檢核結果.不須檢核;
        命中檢核行內Mobile = 是否檢核行內Mobile ? 命中檢核結果.等待 : 命中檢核結果.不須檢核;
        命中檢核姓名檢核 = 是否檢核姓名檢核 ? 命中檢核結果.等待 : 命中檢核結果.不須檢核;
        命中檢核929 = 是否檢核929 ? 命中檢核結果.等待 : 命中檢核結果.不須檢核;
        命中檢核分行資訊 = 是否檢核分行資訊 ? 命中檢核結果.等待 : 命中檢核結果.不須檢核;
        命中檢核關注名單1 = 是否檢核關注名單 ? 命中檢核結果.等待 : 命中檢核結果.不須檢核;
        命中檢核關注名單2 = 是否檢核關注名單 ? 命中檢核結果.等待 : 命中檢核結果.不須檢核;
        命中檢核頻繁ID = 是否檢核頻繁ID ? 命中檢核結果.等待 : 命中檢核結果.不須檢核;
        命中檢查重覆進件 = 是否檢查重覆進件 ? 命中檢核結果.等待 : 命中檢核結果.不須檢核;
    }

    /// <summary>
    /// 檢查所有檢核是否完成
    /// </summary>
    public bool IsAllCheckComplete()
    {
        var checkResults = new[]
        {
            (是否檢核原持卡人, 是否檢核原持卡人成功),
            (是否檢核行內Email, 是否檢核行內Email成功),
            (是否檢核行內Mobile, 是否檢核行內Mobile成功),
            (是否檢核姓名檢核, 是否檢核姓名檢核成功),
            (是否檢核929, 是否檢核929成功),
            (是否檢核分行資訊, 是否檢核分行資訊成功),
            (是否檢核關注名單, 是否檢核關注名單成功),
            (是否檢核頻繁ID, 是否檢核頻繁ID成功),
            (是否檢查重覆進件, 是否檢查重覆進件成功),
        };

        return checkResults.All(x => x.Item1 || (x.Item2 == 檢核結果.成功 || x.Item2 == 檢核結果.不須檢核));
    }

    /// <summary>
    /// 取得檢核失敗的項目
    /// </summary>
    public List<string> GetFailedCheckItems()
    {
        var failedItems = new List<string>();

        if (是否檢核原持卡人 && 是否檢核原持卡人成功 == 檢核結果.失敗)
            failedItems.Add($"原持卡人查詢({UserType}_{ID})");

        if (是否檢核行內Email && 是否檢核行內Email成功 == 檢核結果.失敗)
            failedItems.Add($"行內Email檢核({UserType}_{ID})");

        if (是否檢核行內Mobile && 是否檢核行內Mobile成功 == 檢核結果.失敗)
            failedItems.Add($"行內Mobile檢核({UserType}_{ID})");

        if (是否檢核姓名檢核 && 是否檢核姓名檢核成功 == 檢核結果.失敗)
            failedItems.Add($"姓名檢核({UserType}_{ID})");

        if (是否檢核929 && 是否檢核929成功 == 檢核結果.失敗)
            failedItems.Add($"929查詢({UserType}_{ID})");

        if (是否檢核分行資訊 && 是否檢核分行資訊成功 == 檢核結果.失敗)
            failedItems.Add($"分行資訊查詢({UserType}_{ID})");

        if (是否檢核關注名單 && 是否檢核關注名單成功 == 檢核結果.失敗)
            failedItems.Add($"關注名單查詢({UserType}_{ID})");

        if (是否檢核頻繁ID && 是否檢核頻繁ID成功 == 檢核結果.失敗)
            failedItems.Add($"頻繁ID檢核({UserType}_{ID})");

        if (是否檢查重覆進件 && 是否檢查重覆進件成功 == 檢核結果.失敗)
            failedItems.Add($"重覆進件檢核({UserType}_{ID})");

        return failedItems;
    }

    /// <summary>
    /// 檢查是否有任何檢核失敗
    /// </summary>
    public bool HasAnyCheckFailed()
    {
        return GetFailedCheckItems().Any();
    }

    public void 設定原持卡人查詢結果(檢核結果 檢核結果, 命中檢核結果 命中檢核結果)
    {
        是否檢核原持卡人成功 = 檢核結果;
        命中檢核原持卡人 = 命中檢核結果;
    }

    public void 設定姓名檢核結果(檢核結果 檢核結果, 命中檢核結果 命中檢核結果)
    {
        是否檢核姓名檢核成功 = 檢核結果;
        命中檢核姓名檢核 = 命中檢核結果;
    }

    public void 設定行內Email檢核結果(檢核結果 檢核結果, 命中檢核結果 命中檢核結果)
    {
        是否檢核行內Email成功 = 檢核結果;
        命中檢核行內Email = 命中檢核結果;
    }

    public void 設定行內Mobile檢核結果(檢核結果 檢核結果, 命中檢核結果 命中檢核結果)
    {
        是否檢核行內Mobile成功 = 檢核結果;
        命中檢核行內Mobile = 命中檢核結果;
    }

    public void 設定929查詢結果(檢核結果 檢核結果, 命中檢核結果 命中檢核結果)
    {
        是否檢核929成功 = 檢核結果;
        命中檢核929 = 命中檢核結果;
    }

    public void 設定分行資訊查詢結果(檢核結果 檢核結果, 命中檢核結果 命中檢核結果)
    {
        是否檢核分行資訊成功 = 檢核結果;
        命中檢核分行資訊 = 命中檢核結果;
    }

    public void 設定關注名單查詢結果(檢核結果 檢核結果, 命中檢核結果 命中關注名單1結果, 命中檢核結果 命中關注名單2結果)
    {
        是否檢核關注名單成功 = 檢核結果;
        命中檢核關注名單1 = 命中關注名單1結果;
        命中檢核關注名單2 = 命中關注名單2結果;
    }

    public void 設定頻繁ID檢核結果(檢核結果 檢核結果, 命中檢核結果 命中檢核結果)
    {
        是否檢核頻繁ID成功 = 檢核結果;
        命中檢核頻繁ID = 命中檢核結果;
    }

    public void 設定重覆進件檢核結果(檢核結果 檢核結果, 命中檢核結果 命中檢核結果)
    {
        是否檢查重覆進件成功 = 檢核結果;
        命中檢查重覆進件 = 命中檢核結果;
    }
}

public enum 檢核結果
{
    成功 = 1,
    失敗 = 2,
    等待 = 3,
    不須檢核 = 4,
}

public enum 命中檢核結果
{
    未命中 = 1,
    命中 = 2,
    不須檢核 = 3,
    等待 = 4,
}
