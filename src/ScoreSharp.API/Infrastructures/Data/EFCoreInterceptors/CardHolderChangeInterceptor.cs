using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ScoreSharp.API.Infrastructures.Data.EFCoreInterceptors;

/// <summary>
/// 正卡人/附卡人資料變更攔截器
/// 只追蹤特定 API 端點的修改操作
/// </summary>
public class CardHolderChangeInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CardHolderChangeInterceptor> _logger;

    // 只追蹤這兩個 API 端點
    private static readonly HashSet<string> TrackedEndpoints = new(StringComparer.OrdinalIgnoreCase)
    {
        "/ReviewerCore/UpdateApplicationInfoById",
        "/ReviewerCore/UpdateSupplementaryInfoByApplyNo",
    };

    private static readonly HashSet<string> FilteredFields = new(StringComparer.OrdinalIgnoreCase) { "LastUpdateTime", "LastUpdateUserId" };

    public CardHolderChangeInterceptor(IHttpContextAccessor httpContextAccessor, ILogger<CardHolderChangeInterceptor> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        var context = eventData.Context;
        if (context == null)
            return result;

        // 檢查是否為追蹤的 API 端點
        var httpContext = _httpContextAccessor.HttpContext;
        var apiEndpoint = httpContext?.Request?.Path.Value;

        if (string.IsNullOrEmpty(apiEndpoint) || !TrackedEndpoints.Any(x => apiEndpoint.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
        {
            // 不在追蹤清單，跳過
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        // 只處理修改操作
        var modifiedEntries = context
            .ChangeTracker.Entries()
            .Where(e =>
                e.State == EntityState.Modified
                && (e.Entity is Reviewer_ApplyCreditCardInfoMain || e.Entity is Reviewer_ApplyCreditCardInfoSupplementary)
            )
            .ToList();

        foreach (var entry in modifiedEntries)
        {
            try
            {
                var log = CreateChangeLog(entry, apiEndpoint);
                if (log != null)
                {
                    context.Add(log);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "建立卡片持有人變更記錄失敗: {EntityType}, ApplyNo: {ApplyNo}, Endpoint: {Endpoint}",
                    entry.Entity.GetType().Name,
                    GetApplyNo(entry),
                    apiEndpoint
                );
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private Reviewer_CardHolderChangeLog? CreateChangeLog(EntityEntry entry, string apiEndpoint)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var userId = httpContext?.User?.FindFirst("UserId")?.Value ?? httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "SYSTEM";
        var userName = httpContext?.User?.FindFirst("UserName")?.Value ?? httpContext?.User?.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
        var ipAddress = httpContext?.Connection?.RemoteIpAddress?.ToString();

        string applyNo;
        ScoreSharp.Common.Enums.UserType userType;
        string? supplementaryId = null;

        if (entry.Entity is Reviewer_ApplyCreditCardInfoMain mainCard)
        {
            applyNo = mainCard.ApplyNo;
            userType = ScoreSharp.Common.Enums.UserType.正卡人; // 正卡人
        }
        else if (entry.Entity is Reviewer_ApplyCreditCardInfoSupplementary suppCard)
        {
            applyNo = suppCard.ApplyNo;
            userType = ScoreSharp.Common.Enums.UserType.附卡人; // 附卡人
            supplementaryId = suppCard.ID;
        }
        else
        {
            return null;
        }

        var modifiedProperties = entry.Properties.Where(p => p.IsModified).ToList();
        if (!modifiedProperties.Any())
            return null;

        var beforeData = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
        var afterData = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
        var changedFields = string.Join(",", modifiedProperties.Select(p => p.Metadata.Name).Where(x => !FilteredFields.Contains(x)).ToList());

        var log = new Reviewer_CardHolderChangeLog
        {
            ApplyNo = applyNo,
            UserType = userType,
            SupplementaryID = supplementaryId,
            ChangeDateTime = DateTime.Now,
            ChangeUserId = userId,
            ChangeUserName = userName,
            ChangeSource = DetermineChangeSource(),
            ChangeAPIEndpoint = ExtractEndpointName(apiEndpoint),
            BeforeData = beforeData,
            AfterData = afterData,
            ChangedFields = changedFields,
            IpAddress = ipAddress,
        };

        // 填入報表專用欄位
        FillReportFields(log, entry);

        return log;
    }

    /// <summary>
    /// 提取端點名稱（只保留最後一段）
    /// 例如：/ReviewerCore/UpdateApplicationInfoById -> UpdateApplicationInfoById
    /// </summary>
    private string ExtractEndpointName(string apiEndpoint)
    {
        if (string.IsNullOrEmpty(apiEndpoint))
            return apiEndpoint;

        var filteredEndpoints = TrackedEndpoints.SingleOrDefault(x => apiEndpoint.StartsWith(x, StringComparison.OrdinalIgnoreCase));
        if (filteredEndpoints == null)
            return apiEndpoint;

        return filteredEndpoints;
    }

    /// <summary>
    /// 填入報表專用欄位（只記錄有變更的欄位）
    /// </summary>
    private void FillReportFields(Reviewer_CardHolderChangeLog log, EntityEntry entry)
    {
        // 行動電話
        if (IsPropertyModified(entry, "Mobile"))
        {
            log.BeforeMobile = GetPropertyValue<string>(entry, "Mobile", isOriginal: true);
            log.AfterMobile = GetPropertyValue<string>(entry, "Mobile", isOriginal: false);
        }

        // Email
        if (IsPropertyModified(entry, "EMail"))
        {
            log.BeforeEmail = GetPropertyValue<string>(entry, "EMail", isOriginal: true);
            log.AfterEmail = GetPropertyValue<string>(entry, "EMail", isOriginal: false);
        }

        // 帳單地址（優先使用 FullAddr，沒有就串接切割地址）
        if (IsAddressModified(entry, "Bill"))
        {
            log.BeforeBillAddress = GetAddress(entry, "Bill", isOriginal: true);
            log.AfterBillAddress = GetAddress(entry, "Bill", isOriginal: false);
        }

        // 寄卡地址（優先使用 FullAddr，沒有就串接切割地址）
        if (IsAddressModified(entry, "SendCard"))
        {
            log.BeforeSendCardAddress = GetAddress(entry, "SendCard", isOriginal: true);
            log.AfterSendCardAddress = GetAddress(entry, "SendCard", isOriginal: false);
        }
    }

    /// <summary>
    /// 判斷地址欄位是否有異動
    /// </summary>
    private bool IsAddressModified(EntityEntry entry, string prefix)
    {
        var addressFields = new[]
        {
            $"{prefix}_FullAddr",
            $"{prefix}_ZipCode",
            $"{prefix}_City",
            $"{prefix}_District",
            $"{prefix}_Road",
            $"{prefix}_Lane",
            $"{prefix}_Alley",
            $"{prefix}_Number",
            $"{prefix}_SubNumber",
            $"{prefix}_Floor",
            $"{prefix}_Other",
        };

        return addressFields.Any(field => IsPropertyModified(entry, field));
    }

    /// <summary>
    /// 取得地址（優先使用 FullAddr，沒有就串接切割地址）
    /// 限制長度 120 字元
    /// </summary>
    private string? GetAddress(EntityEntry entry, string prefix, bool isOriginal)
    {
        // 優先使用 FullAddr
        var fullAddr = GetPropertyValue<string>(entry, $"{prefix}_FullAddr", isOriginal);
        if (!string.IsNullOrWhiteSpace(fullAddr))
        {
            // 限制長度 120 字元
            return fullAddr.Length > 120 ? fullAddr.Substring(0, 120) : fullAddr;
        }

        // 沒有 FullAddr 就串接切割地址
        var sb = new StringBuilder();

        var zipCode = GetPropertyValue<string>(entry, $"{prefix}_ZipCode", isOriginal);
        var city = GetPropertyValue<string>(entry, $"{prefix}_City", isOriginal);
        var district = GetPropertyValue<string>(entry, $"{prefix}_District", isOriginal);
        var road = GetPropertyValue<string>(entry, $"{prefix}_Road", isOriginal);
        var lane = GetPropertyValue<string>(entry, $"{prefix}_Lane", isOriginal);
        var alley = GetPropertyValue<string>(entry, $"{prefix}_Alley", isOriginal);
        var number = GetPropertyValue<string>(entry, $"{prefix}_Number", isOriginal);
        var subNumber = GetPropertyValue<string>(entry, $"{prefix}_SubNumber", isOriginal);
        var floor = GetPropertyValue<string>(entry, $"{prefix}_Floor", isOriginal);
        var other = GetPropertyValue<string>(entry, $"{prefix}_Other", isOriginal);

        if (!string.IsNullOrWhiteSpace(zipCode))
            sb.Append(zipCode);
        if (!string.IsNullOrWhiteSpace(city))
            sb.Append(city);
        if (!string.IsNullOrWhiteSpace(district))
            sb.Append(district);
        if (!string.IsNullOrWhiteSpace(road))
            sb.Append(road);
        if (!string.IsNullOrWhiteSpace(lane))
            sb.Append($"{lane}巷");
        if (!string.IsNullOrWhiteSpace(alley))
            sb.Append($"{alley}弄");
        if (!string.IsNullOrWhiteSpace(number))
            sb.Append($"{number}號");
        if (!string.IsNullOrWhiteSpace(subNumber))
            sb.Append($"之{subNumber}");
        if (!string.IsNullOrWhiteSpace(floor))
            sb.Append($"{floor}樓");
        if (!string.IsNullOrWhiteSpace(other))
            sb.Append(other);

        var result = sb.ToString();
        if (string.IsNullOrWhiteSpace(result))
            return null;

        // 限制長度 120 字元
        return result.Length > 120 ? result.Substring(0, 120) : result;
    }

    private bool IsPropertyModified(EntityEntry entry, string propertyName)
    {
        try
        {
            return entry.Property(propertyName).IsModified;
        }
        catch
        {
            return false;
        }
    }

    private T? GetPropertyValue<T>(EntityEntry entry, string propertyName, bool isOriginal)
    {
        try
        {
            var property = entry.Property(propertyName);
            var value = isOriginal ? property.OriginalValue : property.CurrentValue;
            return value is T typedValue ? typedValue : default;
        }
        catch
        {
            return default;
        }
    }

    private string GetApplyNo(EntityEntry entry) =>
        entry.Entity switch
        {
            Reviewer_ApplyCreditCardInfoMain main => main.ApplyNo,
            Reviewer_ApplyCreditCardInfoSupplementary supp => supp.ApplyNo,
            _ => "Unknown",
        };

    private string DetermineChangeSource()
    {
        var assemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Unknown";
        return assemblyName switch
        {
            "ScoreSharp.API" => "API",
            "ScoreSharp.Batch" => "Batch",
            "ScoreSharp.Middleware" => "Middleware",
            "ScoreSharp.PaperMiddleware" => "PaperMiddleware",
            _ => assemblyName,
        };
    }
}
