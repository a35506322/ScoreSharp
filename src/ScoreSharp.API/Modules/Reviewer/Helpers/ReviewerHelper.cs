using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.Helpers;

public class ReviewerHelper : IReviewerHelper
{
    private readonly ScoreSharpContext _scoreSharpContext;
    private readonly IFusionCache _fusionCache;
    private readonly ILogger<ReviewerHelper> _logger;

    public ReviewerHelper(ScoreSharpContext scoreSharpContext, IFusionCache fusionCache, ILogger<ReviewerHelper> logger)
    {
        _scoreSharpContext = scoreSharpContext;
        _fusionCache = fusionCache;
        _logger = logger;
    }

    public async Task<AddressInfoParamsDto> GetAddressInfoParams(string? isActive = null)
    {
        var addressInfoEntity = await _scoreSharpContext
            .SetUp_AddressInfo.Where(x => String.IsNullOrEmpty(isActive) || isActive == x.IsActive)
            .AsNoTracking()
            .ToListAsync();

        // SetUp_AddressInfo
        var cityDto = addressInfoEntity
            .Select(i => new OptionsDtoTypeString
            {
                Name = i.City,
                Value = i.City,
                IsActive = i.IsActive,
            })
            .Distinct(new OptionsDtoTypeStringICompate())
            .ToList();

        var areaDto = addressInfoEntity
            .GroupBy(i => i.City)
            .Select(i => new AreaDto()
            {
                City = i.Key,
                Areas = i.Select(x => new OptionsDtoTypeString()
                    {
                        Name = x.Area,
                        Value = x.Area,
                        IsActive = x.IsActive,
                    })
                    .Distinct(new OptionsDtoTypeStringICompate())
                    .ToList(),
            })
            .ToList();

        var roadDto = addressInfoEntity
            .GroupBy(i => new { City = i.City, Area = i.Area })
            .Select(i => new RoadDto()
            {
                City = i.Key.City,
                Area = i.Key.Area,
                Roads = i.Select(x => new OptionsDtoTypeString()
                    {
                        Name = x.Road,
                        Value = x.Road,
                        IsActive = x.IsActive,
                    })
                    .Distinct(new OptionsDtoTypeStringICompate())
                    .ToList(),
            })
            .ToList();

        AddressInfoParamsDto addressInfoParamsDto = new AddressInfoParamsDto
        {
            City = cityDto,
            Area = areaDto,
            Road = roadDto,
        };

        return addressInfoParamsDto;
    }

    public async Task<ApplyCreditCardInfoParamsDto> GetCreditCardInfoParams(string? isActive)
    {
        var paramsEntity = await _scoreSharpContext.Procedures.Usp_GetApplyCreditCardInfoWithParamsAsync();

        var groupedDto = paramsEntity
            .Where(p => String.IsNullOrEmpty(isActive) || p.IsActive == isActive)
            .GroupBy(d => d.Type)
            .ToDictionary(g => g.Key, g => g.ToList());

        var props = typeof(ApplyCreditCardInfoParamsDto).GetProperties();

        var resultDto = new ApplyCreditCardInfoParamsDto();

        foreach (var prop in props)
        {
            if (!groupedDto.TryGetValue(prop.Name, out var value))
            {
                continue;
            }
            else if (prop.PropertyType.GetGenericArguments().FirstOrDefault()?.Name == "OptionsDtoTypeString")
            {
                var activeValueString = value
                    .Select(v => new OptionsDtoTypeString
                    {
                        Name = v.Name,
                        Value = v.StringValue,
                        IsActive = v.IsActive,
                    })
                    .ToList();

                prop.SetValue(resultDto, activeValueString);
            }
        }

        return resultDto;
    }

    private class OptionsDtoTypeStringICompate : IEqualityComparer<OptionsDtoTypeString>
    {
        public bool Equals(OptionsDtoTypeString x, OptionsDtoTypeString y) => x.Value == y.Value;

        public int GetHashCode(OptionsDtoTypeString obj) => obj.Value.GetHashCode();
    }

    public (bool, List<string>) 檢查正卡人必填地址(Reviewer_ApplyCreditCardInfoMain main)
    {
        var prefixes = new List<string> { "Reg_", "Bill_", "SendCard_" };
        if (main.IsStudent == "Y")
            prefixes.Add("ParentLive_");

        return 檢查必填地址(main, prefixes, main.IsOriginalCardholder == "Y", GetChineseAddressPrefix);
    }

    public (bool, List<string>) 檢查附卡人必填地址(Reviewer_ApplyCreditCardInfoSupplementary supplementary)
    {
        var prefixes = new List<string> { "SendCard_" };
        return 檢查必填地址(supplementary, prefixes, supplementary.IsOriginalCardholder == "Y", _ => "附卡人寄卡地址");
    }

    /// <summary>
    /// 檢查地址欄位是否必填
    /// </summary>
    /// <param name="data">要檢查的 DTO</param>
    /// <param name="addressPrefixes">地址欄位前綴，例如 ["Reg_", "Bill_"]</param>
    /// <param name="isOriginalCardholder">是否為原卡友</param>
    /// <param name="prefixNameResolver">將欄位前綴轉成中文名稱的方法</param>
    /// <returns>(是否通過驗證, 錯誤訊息清單)</returns>
    private (bool, List<string>) 檢查必填地址(
        object data,
        IEnumerable<string> addressPrefixes,
        bool isOriginalCardholder,
        Func<string, string> prefixNameResolver
    )
    {
        // 判斷檢查的欄位清單
        var validAddressFields = isOriginalCardholder ? new[] { "ZipCode", "FullAddr" } : new[] { "ZipCode", "City", "District", "Road", "Number" };

        bool IsRequiredAddressProp(string name) => addressPrefixes.Any(prefix => validAddressFields.Any(suffix => name.Equals($"{prefix}{suffix}")));

        // 找出符合條件的屬性
        var addressProperties = data.GetType().GetProperties().Where(p => IsRequiredAddressProp(p.Name));

        var errors = new List<string>();

        foreach (var prefix in addressPrefixes.Select(p => p.TrimEnd('_')))
        {
            var props = addressProperties.Where(p => p.Name.StartsWith(prefix + "_"));
            if (props.Any(p => string.IsNullOrWhiteSpace(p.GetValue(data) as string)))
            {
                if (isOriginalCardholder)
                    errors.Add($"{prefixNameResolver(prefix)}之郵遞區號、完整地址為必填");
                else
                    errors.Add($"{prefixNameResolver(prefix)}之郵遞區號、縣市、鄉鎮市區、路、號為必填");
            }
        }

        return (!errors.Any(), errors);
    }

    private string GetChineseAddressPrefix(string prefix) =>
        prefix switch
        {
            "Reg" => "正卡人戶籍地址",
            "Bill" => "正卡人帳單地址",
            "Comp" => "正卡人公司地址",
            "SendCard" => "正卡人寄卡地址",
            "ParentLive" => "正卡人家長居住地址",
            _ => throw new ArgumentException($"Invalid prefix: {prefix}"),
        };

    public bool 檢查對應地址(Reviewer_ApplyCreditCardInfoMain main, MailingAddressType mailingAddressType)
    {
        var addressType = ConvertMailingAddressType(mailingAddressType);

        var IsOriginalCardholder = main.IsOriginalCardholder == "Y";
        var property = main.GetType().GetProperties().Where(x => x.Name.StartsWith(addressType)).ToList();

        var vailAddressProps = new List<string>();
        if (IsOriginalCardholder)
        {
            vailAddressProps = new List<string> { "ZipCode", "FullAddr" };
        }
        else
        {
            vailAddressProps = new List<string> { "ZipCode", "City", "District", "Road", "Number" };
        }

        foreach (var prop in property)
        {
            if (!vailAddressProps.Any(x => prop.Name.Equals($"{addressType}{x}")))
            {
                continue;
            }

            var value = prop.GetValue(main);
            if (string.IsNullOrWhiteSpace(value as string))
            {
                return false;
            }
        }

        return true;
    }

    private string ConvertMailingAddressType(MailingAddressType mailingAddressType) =>
        mailingAddressType switch
        {
            MailingAddressType.戶籍地址 => "Reg_",
            MailingAddressType.帳單地址 => "Bill_",
            MailingAddressType.公司地址 => "Comp_",
            MailingAddressType.居住地址 => "Live_",
            _ => throw new ArgumentException($"Invalid mailing address type: {mailingAddressType}"),
        };

    /// <summary>
    /// 檢查命中銀行追蹤項目需填寫確認紀錄和異常判斷
    /// </summary>
    /// <param name="bankTrace">銀行追蹤資訊</param>
    /// <param name="billType">帳單類型 (用於檢查行內E-Mail)</param>
    /// <remarks>
    /// 檢查規則:
    /// 1. 命中銀行追蹤項目 = Y 時，需檢查填寫:
    ///    - 是否異常 (IsError)
    ///    - 確認紀錄 (CheckRecord)
    /// 2. 特殊規則 - 行內E-Mail 檢查:
    ///    - 帳單類型 = 電子帳單 => 必須有 InternalEmailSame 紀錄
    ///    - 帳單類型 != 電子帳單 => InternalEmailSame 不需要有紀錄
    ///
    /// 檢查欄位:
    /// 1. 與行內IP相同 (EqualInternalIP)
    /// 2. 網路件手機號碼相同 (SameMobile)
    /// 3. 網路件E-Mail相同 (SameEmail)
    /// 4. IP相同 (SameIP)
    /// 5. 行內E-Mail相同 (InternalEmailSame)
    /// 6. 行內手機號碼相同 (InternalMobileSame)
    /// 7. 頻繁申請ID (ShortTimeID)
    /// </remarks>
    /// <returns>(是否通過驗證, 錯誤訊息清單)</returns>
    public (bool, List<string>) 檢查銀行追蹤回覆是否必輸(Reviewer_BankTrace bankTrace, BillType? billType = null)
    {
        var type = bankTrace.GetType();
        var result = new List<string>();

        // 取得所有命中的Flag欄位 (Flag = "Y")
        var hitBankTraceList = type.GetProperties().Where(p => p.Name.EndsWith("_Flag") && p.GetValue(bankTrace)?.ToString() == "Y").ToList();

        foreach (var flagProp in hitBankTraceList)
        {
            var flagName = flagProp.Name.Replace("_Flag", "");

            // 特殊規則: 行內E-Mail 檢查
            if (flagName == "InternalEmailSame")
            {
                // 只有當帳單類型為電子帳單時，才需要檢查 InternalEmailSame 的欄位
                if (billType != null && billType != BillType.電子帳單)
                {
                    // 帳單類型不是電子帳單，不需要檢查此欄位
                    continue;
                }
            }

            // 檢查 IsError 欄位
            var isErrorProp = type.GetProperty(flagName + "_IsError");
            var isErrorValue = isErrorProp?.GetValue(bankTrace)?.ToString();
            if (string.IsNullOrEmpty(isErrorValue))
            {
                result.Add($"命中【{GetHitBankTraceChineseName(flagName)}】，請填寫是否異常。");
            }

            // 檢查 CheckRecord 欄位
            var checkRecordProp = type.GetProperty(flagName + "_CheckRecord");
            var checkRecordValue = checkRecordProp?.GetValue(bankTrace)?.ToString();
            if (string.IsNullOrEmpty(checkRecordValue))
            {
                result.Add($"命中【{GetHitBankTraceChineseName(flagName)}】，請填寫確認紀錄。");
            }
        }

        return (!result.Any(), result);
    }

    private string GetHitBankTraceChineseName(string name) =>
        name switch
        {
            "EqualInternalIP" => "與行內 IP 相同",
            "SameIP" => "IP 相同",
            "SameEmail" => "網路件 E-Mail 相同",
            "SameMobile" => "網路件手機號碼相同",
            "ShortTimeID" => "短期間頻繁申請 ID",
            "InternalEmailSame" => "行內 E-Mail 相同",
            "InternalMobileSame" => "行內手機號碼相同",
            _ => name, // 沒對應到時直接回傳原字串
        };

    public async Task<int> UpdateMainLastModified(string applyNo, string userId, DateTime? updateTime = null)
    {
        var now = updateTime ?? DateTime.Now;

        var sql = """
            UPDATE [dbo].[Reviewer_ApplyCreditCardInfoMain]
               SET
                   LastUpdateTime    = @Now,
                   LastUpdateUserId  = @UpdateUserId
             WHERE
                   ApplyNo = @ApplyNo;
            """;

        var parameters = new[] { new SqlParameter("@Now", now), new SqlParameter("@UpdateUserId", userId), new SqlParameter("@ApplyNo", applyNo) };

        return await _scoreSharpContext.Database.ExecuteSqlRawAsync(sql, parameters);
    }

    public async Task<IEnumerable<GetApplyCreditCardBaseDataResult>> GetApplyCreditCardBaseData(GetApplyCreditCardBaseDataDto dto)
    {
        if (dto.ApplyDateStart is not null && dto.ApplyDateEnd is null || dto.ApplyDateStart is null && dto.ApplyDateEnd is not null)
        {
            throw new ArgumentException("申請日期起和申請日期迄必須同時填寫");
        }

        if (dto.ApplyDateStart is not null && dto.ApplyDateEnd is not null && dto.ApplyDateStart > dto.ApplyDateEnd)
        {
            throw new ArgumentException("申請日期起不能大於申請日期迄");
        }

        var sql =
            @"EXEC [dbo].[Usp_GetApplyCreditCardBaseDataByConditions] @ID = @ID, @CHName = @CHName, @ApplyNo = @ApplyNo, @CardStatus = @CardStatus, @ApplyCardType = @ApplyCardType, @ApplyDateStart = @ApplyDateStart, @ApplyDateEnd = @ApplyDateEnd, @Source = @Source ,@CurrentHandleUserId = @CurrentHandleUserId, @CurrentHandleUserIds = @CurrentHandleUserIds, @Top = @Top";

        var queryAssignmentUsersResponse = await _scoreSharpContext
            .Database.GetDbConnection()
            .QueryAsync<QueryUspGetApplyCreditCardBaseDataByConditionsResult>(
                sql,
                new
                {
                    ID = string.IsNullOrWhiteSpace(dto.ID) ? null : dto.ID,
                    CHName = string.IsNullOrWhiteSpace(dto.CHName) ? null : dto.CHName,
                    ApplyNo = string.IsNullOrWhiteSpace(dto.ApplyNo) ? null : dto.ApplyNo,
                    CardStatus = dto.CardStatus is not null ? string.Join(",", dto.CardStatus.Select(x => (int)x)) : null,
                    ApplyCardType = string.IsNullOrWhiteSpace(dto.ApplyCardType) ? null : dto.ApplyCardType,
                    ApplyDateStart = dto.ApplyDateStart is not null ? dto.ApplyDateStart.Value : (DateTime?)null,
                    ApplyDateEnd = dto.ApplyDateEnd is not null ? dto.ApplyDateEnd.Value : (DateTime?)null,
                    Source = dto.Source is not null && dto.Source.Any() ? string.Join(",", dto.Source.Select(x => (int)x)) : null,
                    CurrentHandleUserId = string.IsNullOrWhiteSpace(dto.CurrentHandleUserId) ? null : dto.CurrentHandleUserId,
                    CurrentHandleUserIds = string.IsNullOrWhiteSpace(dto.CurrentHandleUserIds) ? null : dto.CurrentHandleUserIds,
                    Top = dto.Top is not null && dto.Top > 0 ? dto.Top.Value : (int?)null,
                }
            );

        if (queryAssignmentUsersResponse.Count() == 0)
        {
            return [];
        }

        // 步驟 1：查詢基礎資料
        var cacheKey = RedisKeyConst.GetApplyCreditCardBaseData;
        string baseSql =
            @"
                SELECT CardCode, CardName FROM SetUp_Card WHERE IsActive = 'Y';
                SELECT UserId, UserName FROM OrgSetUp_User WHERE IsActive = 'Y';
            ";

        var cacheData = await _fusionCache.GetOrSetAsync(
            cacheKey,
            async (cacheEntry) =>
            {
                var baseMulti = await _scoreSharpContext.Database.GetDbConnection().QueryMultipleAsync(baseSql);
                var cardList = baseMulti.Read<CardDto>().ToList();
                var userList = baseMulti.Read<UserDto>().ToList();
                var cardDic = cardList.ToDictionary(x => x.CardCode, x => x.CardName);
                var userDic = userList.ToDictionary(x => x.UserId, x => x.UserName);
                return (cardDic, userDic);
            },
            new FusionCacheEntryOptions { Duration = TimeSpan.FromMinutes(10) }
        );

        var (cardDic, userDic) = cacheData;
        if (cardDic is null)
        {
            _logger.LogError("查詢卡片資訊失敗");
        }

        if (userDic is null)
        {
            _logger.LogError("查詢使用者資訊失敗");
        }

        var result = queryAssignmentUsersResponse
            .GroupBy(x => x.ApplyNo)
            .Select(g =>
            {
                var first = g.First();
                return new GetApplyCreditCardBaseDataResult
                {
                    ApplyNo = g.Key,
                    M_ID = first.M_ID,
                    M_CHName = first.M_CHName,
                    M_NameChecked = first.M_NameChecked,
                    ApplyDate = first.ApplyDate,
                    CaseType = first.CaseType is not null ? first.CaseType.Value : null,
                    M_IsRepeatApply = first.M_IsRepeatApply,
                    CardOwner = first.CardOwner,
                    PromotionUnit = first.PromotionUnit,
                    PromotionUser = first.PromotionUser,
                    CurrentHandleUserId = first.CurrentHandleUserId,
                    CurrentHandleUserName = userDic.TryGetValue(first.CurrentHandleUserId, out var userName) ? userName : string.Empty,
                    LastUpdateUserId = first.LastUpdateUserId,
                    LastUpdateUserName = userDic.TryGetValue(first.LastUpdateUserId, out var lastUpdateUserName) ? lastUpdateUserName : string.Empty,
                    LastUpdateTime = first.LastUpdateTime,
                    Source = first.Source,
                    ApplyCardList = g.Select(h => new ApplyCardInfoDto
                        {
                            HandleSeqNo = h.HandleSeqNo,
                            ApplyCardType = h.ApplyCardType,
                            ApplyCardName = cardDic.TryGetValue(h.ApplyCardType, out var cardName) ? cardName : string.Empty,
                            CardStatus = h.CardStatus,
                            CardStep = h.CardStep,
                            MonthlyIncomeCheckUserId = h.MonthlyIncomeCheckUserId,
                            MonthlyIncomeCheckUserName = userDic.TryGetValue(h.MonthlyIncomeCheckUserId, out var monthlyIncomeCheckUserName)
                                ? monthlyIncomeCheckUserName
                                : string.Empty,
                            MonthlyIncomeTime = h.MonthlyIncomeTime,
                            ReviewerUserId = h.ReviewerUserId,
                            ReviewerUserName = userDic.TryGetValue(h.ReviewerUserId, out var reviewerUserName) ? reviewerUserName : string.Empty,
                            ReviewerTime = h.ReviewerTime,
                            ApproveTime = h.ApproveTime,
                            ApproveUserName = userDic.TryGetValue(h.ApproveUserId, out var approveUserName) ? approveUserName : string.Empty,
                            ApproveUserId = h.ApproveUserId,
                            UserType = h.UserType,
                            ID = h.UserType == UserType.正卡人 ? h.M_ID : h.S1_ID,
                        })
                        .OrderBy(x => x.UserType)
                        .ToList(),
                    S1_ID = first.S1_ID,
                    S1_CHName = first.S1_CHName,
                    S1_IsRepeatApply = first.S1_IsRepeatApply,
                    S1_NameChecked = first.S1_NameChecked,
                    S1_IsOriginalCardholder = first.S1_IsOriginalCardholder,
                    M_IsOriginalCardholder = first.M_IsOriginalCardholder,
                };
            });

        return result;
    }
}
