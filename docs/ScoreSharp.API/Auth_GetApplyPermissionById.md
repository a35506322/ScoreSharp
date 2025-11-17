# GetApplyPermissionById API - 查詢申請書權限 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerPermission/{applyNo}` |
| **HTTP 方法** | GET |
| **功能** | 查詢單筆申請書的權限設定 - 根據申請書編號動態計算當前使用者對該申請書的所有操作權限 |
| **位置** | `Modules/Auth/ReviewerPermission/GetApplyPermissionById/Endpoint.cs` |
| **複雜度** | ⭐⭐⭐⭐⭐ (最複雜的 API,涉及多表查詢、動態權限計算、反射合併) |

---

## API 設計理念

此 API 是整個徵審權限系統的核心,它解決了以下業務需求:

1. **動態權限計算**: 根據申請書的當前狀態和階段,動態決定使用者可以執行的操作
2. **多狀態合併**: 一個申請書可能同時有多個處理記錄,需要合併所有相關權限
3. **當前經辦檢查**: 某些權限只開放給當前經辦人員,需要身份驗證
4. **異常處理**: 當權限設定缺失或發生錯誤時,回傳安全的預設權限
5. **前端友好**: 回傳簡單明瞭的 Y/N 權限標記,前端無需複雜邏輯

---

## Request 定義

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `applyNo` | string | ✅ | 申請書編號,用於識別要查詢權限的申請書 |

### 請求範例

```
GET /ReviewerPermission/20250123X0003
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

#### 情況 1: 唯讀權限 (非當前經辦或查無權限)

```json
{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": {
    "monthlyIncome_IsShowChangeCaseType": "N",
    "monthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard": "N",
    "monthlyIncome_IsShowInPermission": "N",
    "monthlyIncome_IsShowMonthlyIncome": "N",
    "isShowNameCheck": "N",
    "isShowUpdatePrimaryInfo": "N",
    "isShowQueryBranchInfo": "N",
    "isShowQuery929": "N",
    "isShowInsertFileAttachment": "N",
    "isShowUpdateApplyNote": "N",
    "insertReviewerSummary": "N",
    "isShowFocus1": "N",
    "isShowFocus2": "N",
    "isShowWebMobileRequery": "N",
    "isShowWebEmailRequery": "N",
    "isShowUpdateReviewerSummary": "N",
    "isShowDeleteReviewerSummary": "N",
    "isShowDeleteApplyFileAttachment": "N",
    "isShowCommunicationNotes": "N",
    "manualReview_IsShowOutPermission": "N",
    "manualReview_IsShowChangeCaseType": "N",
    "manualReview_IsShowInPermission": "N",
    "manualReview_IsShowReturnReview": "N",
    "isShowUpdateSameIPCheckRecord": "N",
    "isShowUpdateWebEmailCheckRecord": "N",
    "isShowUpdateWebMobileCheckRecord": "N",
    "isShowUpdateInternalIPCheckRecord": "N",
    "isShowUpdateShortTimeIDCheckRecord": "N",
    "isShowInternalEmail": "N",
    "isShowInternalMobile": "N",
    "isShowUpdateInternalEmailCheckRecord": "N",
    "isShowUpdateInternalMobileCheckRecord": "N",
    "isShowUpdateSupplementaryInfo": "N",
    "isShowKYCSync": "N"
  },
  "traceId": "{traceId}"
}
```

#### 情況 2: 完整權限 (當前經辦且有完整權限)

```json
{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": {
    "monthlyIncome_IsShowChangeCaseType": "Y",
    "monthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard": "Y",
    "monthlyIncome_IsShowInPermission": "Y",
    "monthlyIncome_IsShowMonthlyIncome": "Y",
    "isShowNameCheck": "Y",
    "isShowUpdatePrimaryInfo": "Y",
    "isShowQueryBranchInfo": "Y",
    "isShowQuery929": "Y",
    "isShowInsertFileAttachment": "Y",
    "isShowUpdateApplyNote": "Y",
    "insertReviewerSummary": "Y",
    "isShowFocus1": "Y",
    "isShowFocus2": "Y",
    "isShowWebMobileRequery": "Y",
    "isShowWebEmailRequery": "Y",
    "isShowUpdateReviewerSummary": "Y",
    "isShowDeleteReviewerSummary": "Y",
    "isShowDeleteApplyFileAttachment": "Y",
    "isShowCommunicationNotes": "Y",
    "manualReview_IsShowOutPermission": "N",
    "manualReview_IsShowChangeCaseType": "N",
    "manualReview_IsShowInPermission": "N",
    "manualReview_IsShowReturnReview": "N",
    "isShowUpdateSameIPCheckRecord": "Y",
    "isShowUpdateWebEmailCheckRecord": "Y",
    "isShowUpdateWebMobileCheckRecord": "Y",
    "isShowUpdateInternalIPCheckRecord": "Y",
    "isShowUpdateShortTimeIDCheckRecord": "Y",
    "isShowInternalEmail": "Y",
    "isShowInternalMobile": "Y",
    "isShowUpdateInternalEmailCheckRecord": "Y",
    "isShowUpdateInternalMobileCheckRecord": "Y",
    "isShowUpdateSupplementaryInfo": "Y",
    "isShowKYCSync": "N"
  },
  "traceId": "{traceId}"
}
```

### Response 欄位說明

```csharp
// 位置: Modules/Auth/ReviewerPermission/GetApplyPermissionById/Model.cs (Line 3-243)
public class GetApplyPermissionByIdResponse
{
    // 只包含 37 個權限欄位,不包含主鍵和審計欄位
    // 所有欄位都是 string 型別,值為 "Y" 或 "N"

    // 月收入確認權限 (4 個)
    public string MonthlyIncome_IsShowChangeCaseType { get; set; }
    public string MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard { get; set; }
    public string MonthlyIncome_IsShowInPermission { get; set; }
    public string MonthlyIncome_IsShowMonthlyIncome { get; set; }

    // 人工徵審權限 (4 個)
    public string ManualReview_IsShowChangeCaseType { get; set; }
    public string ManualReview_IsShowInPermission { get; set; }
    public string ManualReview_IsShowOutPermission { get; set; }
    public string ManualReview_IsShowReturnReview { get; set; }

    // 一般功能權限 (29 個)
    // ... (與其他 API 相同)
}
```

> **重要差異**: 此 Response 不包含 SeqNo, CardStatus, CardStep 等識別欄位,也不包含審計欄位,只回傳純粹的權限設定。

---

### 錯誤回應

#### 查無此申請書 (400 Bad Request - Return Code: 4001)
- 指定的 ApplyNo 不存在於 Reviewer_ApplyCreditCardInfoMain 表中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此ID: 20250101X0001",
  "data": null
}
```

---

## 驗證資料

### 1. 路由參數驗證
- **位置**: ASP.NET Core 路由系統 (自動執行)
- **驗證內容**:
  - applyNo: 必須為非空字串

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 43-177)

#### 檢查點: 申請書存在性檢查
```
位置: Line 47, 55-56
查詢 Reviewer_ApplyCreditCardInfoMain 表中是否存在指定的 ApplyNo
├─ 若不存在 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此ID: {ApplyNo}"
└─ 若存在 → 繼續執行
```

---

## 資料處理

### 核心處理流程

此 API 的處理邏輯非常複雜,涉及多個步驟:

```
步驟 1: 查詢申請書主檔 (Line 47)
├─ 取得 CurrentHandleUserId (當前經辦人員)
└─ 若查無 → 回傳錯誤 4001

步驟 2: 查詢所有處理記錄 (Line 49-53)
├─ 根據 ApplyNo 查詢 Reviewer_ApplyCreditCardInfoHandle
├─ 取得所有 CardStatus 和 CardStep 組合
└─ 可能有多筆記錄 (一個申請書可能有多個處理階段)

步驟 3: 建立預設權限物件 (Line 60-96)
├─ 所有權限預設為 "N" (除了部分檢查記錄權限為 "Y")
├─ 用於異常處理和無權限情況
└─ 確保安全的最小權限原則

步驟 4: 查詢相關的權限設定 (Line 100-105) [在 try-catch 內]
├─ 根據所有 CardStatus 查詢 Auth_ReviewerPermission
├─ 使用 IN 條件一次查詢所有相關權限
└─ 若查無任何權限 → 直接回傳預設權限 (Line 108-111)

步驟 5: 逐一處理每個 Handle 記錄 (Line 116-150)
├─ 對於每個 CardStatus + CardStep 組合:
│  ├─ 查找匹配的權限設定 (Line 118)
│  │  ├─ 若查無 → 加入預設權限 (Line 121-125)
│  │  └─ 若有多筆 (代表有 CardStep 區分) → 根據 CardStep 精確匹配 (Line 130-137)
│  │
│  ├─ 檢查是否需要當前經辦身份 (Line 139)
│  │  └─ IsCurrentHandleUserId == "Y"
│  │
│  ├─ 身份驗證 (Line 141-144)
│  │  ├─ 若需要當前經辦 AND 當前使用者不是經辦 → 加入預設權限
│  │  └─ 否則 → 加入實際權限
│  │
│  └─ 使用 AutoMapper 轉換權限 (Line 147-148)
│
└─ 收集所有權限到 collectedPermissions List

步驟 6: 反射合併所有權限 (Line 152-168)
├─ 取得所有 string 型別的屬性 (權限欄位)
├─ 對於每個屬性:
│  ├─ 檢查是否有任何一個收集的權限為 "Y"
│  ├─ 若有 → 設為 "Y"
│  └─ 若無 → 設為 "N"
└─ 合併邏輯: 任一為 "Y" 即為 "Y" (OR 邏輯)

步驟 7: 回傳合併後的權限 (Line 170)
└─ Return Code: 2000, Data: 合併後的權限物件

異常處理 (Line 172-176)
├─ 捕獲任何異常
├─ 記錄錯誤日誌
└─ 回傳預設權限 (所有權限為 "N")
```

### 詳細程式碼分析

#### 1. 查詢申請書主檔與處理記錄

```csharp
// Line 47: 查詢申請書主檔
var main = await context.Reviewer_ApplyCreditCardInfoMain.AsNoTracking()
    .SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

// Line 49-53: 查詢所有處理記錄
var handleList = await context.Reviewer_ApplyCreditCardInfoHandle.AsNoTracking()
    .Where(x => x.ApplyNo == applyNo)
    .Select(x => new { cardStatus = x.CardStatus, cardStep = x.CardStep })
    .ToListAsync();

// Line 55-56: 檢查申請書是否存在
if (main == null)
    return ApiResponseHelper.NotFound<GetApplyPermissionByIdResponse>(null, applyNo);

// Line 58: 取得當前經辦人員
var currentHandlerUserId = main.CurrentHandleUserId;
```

#### 2. 建立預設權限物件

```csharp
// Line 60-96: 預設權限 (所有功能權限為 "N",檢查記錄權限為 "Y")
var defaultApplyPermission = new GetApplyPermissionByIdResponse()
{
    // 月收入確認權限: 全部 "N"
    MonthlyIncome_IsShowChangeCaseType = "N",
    MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = "N",
    MonthlyIncome_IsShowInPermission = "N",
    MonthlyIncome_IsShowMonthlyIncome = "N",

    // 一般功能權限: 全部 "N"
    IsShowNameCheck = "N",
    IsShowUpdatePrimaryInfo = "N",
    // ... 其他功能權限

    // 檢查記錄權限: 全部 "Y" (允許儲存檢查記錄)
    IsShowUpdateSameIPCheckRecord = "Y",
    IsShowUpdateWebEmailCheckRecord = "Y",
    IsShowUpdateWebMobileCheckRecord = "Y",
    IsShowUpdateInternalIPCheckRecord = "Y",
    IsShowUpdateShortTimeIDCheckRecord = "Y",
    IsShowUpdateInternalEmailCheckRecord = "Y",
    IsShowUpdateInternalMobileCheckRecord = "Y",

    // 再查詢權限: 全部 "N"
    IsShowInternalEmail = "N",
    IsShowInternalMobile = "N",
    IsShowUpdateSupplementaryInfo = "N",
    IsShowKYCSync = "N",
};
```

#### 3. 查詢相關權限設定

```csharp
// Line 100-105: 批次查詢所有相關的權限設定
var handleCardStatuses = handleList.Select(x => x.cardStatus).ToList();

var authReviewerPermissions = await context.Auth_ReviewerPermission.AsNoTracking()
    .Where(x => handleCardStatuses.Contains(x.CardStatus))
    .ToListAsync();

// Line 108-111: 若查無任何權限,直接回傳預設權限
if (authReviewerPermissions.Count == 0)
{
    return ApiResponseHelper.Success(defaultApplyPermission);
}
```

#### 4. 逐一處理並收集權限

```csharp
// Line 113: 初始化權限收集列表
List<GetApplyPermissionByIdResponse> collectedPermissions = new();

// Line 116-150: 逐一處理每個 Handle 記錄
foreach (var handle in handleList)
{
    // Line 118: 查找匹配此 CardStatus 的所有權限
    var matchingPermissions = authReviewerPermissions
        .Where(x => x.CardStatus == handle.cardStatus).ToList();

    // Line 121-125: 若查無權限,加入預設權限
    if (!matchingPermissions.Any())
    {
        collectedPermissions.Add(defaultApplyPermission);
        continue;
    }

    var reviewerPermission = new Auth_ReviewerPermission();

    // Line 130-137: 處理多個權限設定 (有 CardStep 區分)
    if (matchingPermissions.Count > 1)
    {
        // 根據 CardStep 精確匹配
        reviewerPermission = matchingPermissions.SingleOrDefault(x => x.CardStep == handle.cardStep);
    }
    else
    {
        // 只有一個權限,直接使用
        reviewerPermission = matchingPermissions.FirstOrDefault();
    }

    // Line 139: 檢查是否需要當前經辦身份
    bool isNeedToCheckCurrentHandler = reviewerPermission.IsCurrentHandleUserId == "Y";

    // Line 141-149: 身份驗證並加入權限
    if (isNeedToCheckCurrentHandler && jwtHelper.UserId != currentHandlerUserId)
    {
        // 不是當前經辦,加入預設權限 (唯讀)
        collectedPermissions.Add(defaultApplyPermission);
    }
    else
    {
        // 是當前經辦或不需檢查,加入實際權限
        var applyCasePermissionResponse = mapper.Map<GetApplyPermissionByIdResponse>(reviewerPermission);
        collectedPermissions.Add(applyCasePermissionResponse);
    }
}
```

#### 5. 反射合併所有權限 (核心邏輯)

```csharp
// Line 152-153: 建立最終權限物件
var finalPermissionResponse = new GetApplyPermissionByIdResponse();

// Line 155: 取得所有 string 型別的屬性 (權限欄位)
var propertyInfos = typeof(GetApplyPermissionByIdResponse).GetProperties()
    .Where(p => p.PropertyType == typeof(string));

// Line 157-168: 逐一檢查每個屬性並合併
foreach (var property in propertyInfos)
{
    // Line 160-164: 檢查是否有任何一個權限為 "Y"
    var isPermitted = collectedPermissions.Any(permission =>
    {
        var value = property.GetValue(permission) as string;
        return value == "Y";
    });

    // Line 167: 設置最終權限值 (OR 邏輯)
    property.SetValue(finalPermissionResponse, isPermitted ? "Y" : "N");
}

// Line 170: 回傳合併後的權限
return ApiResponseHelper.Success(finalPermissionResponse);
```

#### 6. 異常處理

```csharp
// Line 172-176: 捕獲所有異常並回傳預設權限
catch (Exception ex)
{
    logger.LogError(ex, "查詢申請書權限時發生錯誤,申請書編號: {ApplyNo}", applyNo);
    return ApiResponseHelper.Success(defaultApplyPermission);
}
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Reviewer_ApplyCreditCardInfoMain` | SELECT | 申請書主檔 - 查詢當前經辦人員 |
| `Reviewer_ApplyCreditCardInfoHandle` | SELECT | 申請書處理記錄 - 查詢所有 CardStatus 和 CardStep |
| `Auth_ReviewerPermission` | SELECT | 徵審權限主檔 - 查詢相關權限設定 |

### 操作類型

#### 1. 查詢申請書主檔

- **位置**: Line 47
- **方法**: `AsNoTracking()` + `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var main = await context.Reviewer_ApplyCreditCardInfoMain.AsNoTracking()
      .SingleOrDefaultAsync(x => x.ApplyNo == applyNo);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Reviewer_ApplyCreditCardInfoMain]
  WHERE [ApplyNo] = @ApplyNo
  ```

#### 2. 查詢處理記錄

- **位置**: Line 49-53
- **方法**: `AsNoTracking()` + `Where()` + `Select()` + `ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var handleList = await context.Reviewer_ApplyCreditCardInfoHandle.AsNoTracking()
      .Where(x => x.ApplyNo == applyNo)
      .Select(x => new { cardStatus = x.CardStatus, cardStep = x.CardStep })
      .ToListAsync();
  ```
- **等效 SQL**:
  ```sql
  SELECT [CardStatus], [CardStep]
  FROM [dbo].[Reviewer_ApplyCreditCardInfoHandle]
  WHERE [ApplyNo] = @ApplyNo
  ```

#### 3. 批次查詢權限設定

- **位置**: Line 102-105
- **方法**: `AsNoTracking()` + `Where(Contains)` + `ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var authReviewerPermissions = await context.Auth_ReviewerPermission.AsNoTracking()
      .Where(x => handleCardStatuses.Contains(x.CardStatus))
      .ToListAsync();
  ```
- **等效 SQL**:
  ```sql
  SELECT *
  FROM [dbo].[Auth_ReviewerPermission]
  WHERE [CardStatus] IN (@CardStatus1, @CardStatus2, ...)
  ```

### 性能優化

1. **使用 AsNoTracking()**: 所有查詢都是唯讀的,不需要變更追蹤
2. **批次查詢**: 使用 IN 條件一次查詢所有相關權限,避免 N+1 問題
3. **只查詢需要的欄位**: Handle 查詢只 SELECT CardStatus 和 CardStep
4. **記憶體內處理**: 權限匹配和合併在記憶體中進行,減少資料庫往返

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 GET /ReviewerPermission/{applyNo} 請求
│
├─ 步驟 1: 查詢申請書主檔 (Line 47)
│  ├─ Reviewer_ApplyCreditCardInfoMain
│  ├─ 取得 CurrentHandleUserId
│  │
│  ├─ 查無資料 → 400 Bad Request (Return Code: 4001)
│  └─ 有資料 → 繼續
│
├─ 步驟 2: 查詢所有處理記錄 (Line 49-53)
│  ├─ Reviewer_ApplyCreditCardInfoHandle
│  ├─ 取得所有 CardStatus + CardStep 組合
│  └─ 可能有 0~N 筆記錄
│
├─ 步驟 3: Try-Catch 包裝 (Line 98-176)
│  │
│  ├─ 步驟 4: 批次查詢權限設定 (Line 100-105)
│  │  ├─ Auth_ReviewerPermission
│  │  ├─ 使用 IN 條件查詢所有相關 CardStatus
│  │  │
│  │  ├─ 查無任何權限 → 回傳預設權限 (Line 108-111)
│  │  └─ 有權限 → 繼續
│  │
│  ├─ 步驟 5: 逐一處理 Handle 記錄 (Line 116-150)
│  │  │
│  │  ├─ For each Handle:
│  │  │  ├─ 匹配 CardStatus (Line 118)
│  │  │  │  ├─ 無匹配 → 加入預設權限
│  │  │  │  └─ 有匹配 → 繼續
│  │  │  │
│  │  │  ├─ 多個匹配? (Line 130)
│  │  │  │  ├─ 是 → 根據 CardStep 精確匹配
│  │  │  │  └─ 否 → 直接使用唯一匹配
│  │  │  │
│  │  │  ├─ 需要當前經辦檢查? (Line 139)
│  │  │  │  ├─ 是 → 檢查 JWT UserId vs CurrentHandleUserId
│  │  │  │  │  ├─ 不符 → 加入預設權限 (唯讀)
│  │  │  │  │  └─ 符合 → 加入實際權限
│  │  │  │  └─ 否 → 加入實際權限
│  │  │  │
│  │  │  └─ 收集到 collectedPermissions List
│  │  │
│  │  └─ 收集完成
│  │
│  ├─ 步驟 6: 反射合併所有權限 (Line 152-168)
│  │  ├─ 取得所有 string 屬性 (權限欄位)
│  │  ├─ For each 屬性:
│  │  │  ├─ 檢查所有收集的權限
│  │  │  ├─ 任一為 "Y" → 設為 "Y"
│  │  │  └─ 全部為 "N" → 設為 "N"
│  │  └─ 合併完成
│  │
│  ├─ 步驟 7: 回傳合併後的權限 (Line 170)
│  │  └─ 200 OK (Return Code: 2000)
│  │
│  └─ 異常處理 (Line 172-176)
│     ├─ 捕獲任何異常
│     ├─ 記錄錯誤日誌
│     └─ 回傳預設權限 (安全降級)
```

---

## 關鍵業務規則

### 1. 動態權限計算

此 API 不是簡單地查詢權限,而是根據申請書的當前狀態動態計算:

- **多狀態合併**: 一個申請書可能同時處於多個處理階段 (例如補件中同時進行月收入確認)
- **權限取最大**: 使用 OR 邏輯合併,只要任一階段允許,最終就允許
- **安全降級**: 發生錯誤時回傳最小權限,確保系統安全

### 2. 當前經辦身份驗證

某些權限只開放給當前經辦人員:

```
判斷邏輯:
├─ Auth_ReviewerPermission.IsCurrentHandleUserId == "Y"
│  └─ 需要檢查當前經辦身份
│
├─ JWT.UserId == Main.CurrentHandleUserId
│  ├─ 是 → 給予實際權限
│  └─ 否 → 給予預設權限 (唯讀)
│
└─ Auth_ReviewerPermission.IsCurrentHandleUserId == "N"
   └─ 不需檢查,直接給予實際權限
```

這確保了:
- 只有當前負責的人員才能執行特定操作
- 其他人員只能查看,不能修改
- 工作流程的嚴格控管

### 3. CardStep 的作用

CardStep 用於區分同一 CardStatus 在不同階段的權限:

```
範例: CardStatus = "補件作業中"
├─ CardStep = "月收入確認"
│  └─ 權限設定 A (月收入確認相關功能)
│
└─ CardStep = "人工徵審"
   └─ 權限設定 B (人工徵審相關功能)
```

匹配邏輯:
1. 先匹配 CardStatus
2. 若有多個結果,再用 CardStep 精確匹配
3. 確保取得正確的權限設定

### 4. 預設權限的設計哲學

預設權限採用最小權限原則:

- **一般功能**: 全部 "N" (不允許任何操作)
- **檢查記錄**: 全部 "Y" (允許儲存檢查記錄)

為什麼檢查記錄權限預設為 "Y"?
- 檢查記錄是審計功能,不應受權限限制
- 即使是唯讀使用者,也應該能記錄查看的軌跡
- 符合合規性要求

### 5. 反射合併的巧妙設計

使用反射進行權限合併的優點:

```csharp
// 不需要寫 37 次這樣的程式碼:
finalPermission.IsShowNameCheck = collectedPermissions.Any(p => p.IsShowNameCheck == "Y") ? "Y" : "N";
finalPermission.IsShowQuery929 = collectedPermissions.Any(p => p.IsShowQuery929 == "Y") ? "Y" : "N";
// ...

// 而是用反射自動處理所有欄位:
foreach (var property in propertyInfos)
{
    var isPermitted = collectedPermissions.Any(permission =>
    {
        var value = property.GetValue(permission) as string;
        return value == "Y";
    });
    property.SetValue(finalPermissionResponse, isPermitted ? "Y" : "N");
}
```

好處:
- 程式碼簡潔,易於維護
- 新增權限欄位時無需修改合併邏輯
- 減少人為錯誤的可能性

### 6. 異常處理策略

此 API 採用「安全降級」策略:

```csharp
try
{
    // 複雜的權限計算邏輯
}
catch (Exception ex)
{
    logger.LogError(ex, "查詢申請書權限時發生錯誤...");
    return ApiResponseHelper.Success(defaultApplyPermission);  // 回傳最小權限
}
```

為什麼這樣設計?
- **不拋出異常**: 避免因權限問題導致前端無法顯示申請書
- **記錄日誌**: 保留錯誤資訊供後續排查
- **降級為唯讀**: 即使權限計算失敗,使用者仍可查看資料
- **符合業務需求**: 查看優先於編輯

### 7. 批次查詢優化

使用 IN 條件批次查詢,避免 N+1 問題:

```csharp
// 不好的做法 (N+1 問題):
foreach (var handle in handleList)
{
    var permission = await context.Auth_ReviewerPermission
        .SingleOrDefaultAsync(x => x.CardStatus == handle.cardStatus);
}

// 好的做法 (批次查詢):
var handleCardStatuses = handleList.Select(x => x.cardStatus).ToList();
var authReviewerPermissions = await context.Auth_ReviewerPermission
    .Where(x => handleCardStatuses.Contains(x.CardStatus))
    .ToListAsync();
```

效能提升:
- N+1 問題: N+1 次資料庫查詢
- 批次查詢: 2 次資料庫查詢 (Handle + Permission)

### 8. 權限合併的 OR 邏輯

為什麼使用 OR 邏輯而非 AND 邏輯?

```
假設申請書有兩個處理記錄:
├─ 記錄 1: IsShowNameCheck = "Y"
└─ 記錄 2: IsShowNameCheck = "N"

OR 邏輯 (實際採用): 最終 = "Y" (任一允許即允許)
AND 邏輯 (未採用): 最終 = "N" (全部允許才允許)
```

業務理由:
- 使用者可能同時處理多個階段
- 應該給予所有階段的權限總和
- 避免因某個階段限制而無法執行必要操作

---

## 請求與回應範例

### 請求範例

```
GET /ReviewerPermission/20250123X0003
```

### 回應範例

**成功回應 - 唯讀權限 (來自 Examples.cs Line 6-50):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": {
    "monthlyIncome_IsShowChangeCaseType": "N",
    "monthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard": "N",
    "monthlyIncome_IsShowInPermission": "N",
    "monthlyIncome_IsShowMonthlyIncome": "N",
    "isShowNameCheck": "N",
    "isShowUpdatePrimaryInfo": "N",
    "isShowQueryBranchInfo": "N",
    "isShowQuery929": "N",
    "isShowInsertFileAttachment": "N",
    "isShowUpdateApplyNote": "N",
    "insertReviewerSummary": "N",
    "isShowFocus1": "N",
    "isShowFocus2": "N",
    "isShowWebMobileRequery": "N",
    "isShowWebEmailRequery": "N",
    "isShowUpdateReviewerSummary": "N",
    "isShowDeleteReviewerSummary": "N",
    "isShowDeleteApplyFileAttachment": "N",
    "isShowCommunicationNotes": "N",
    "manualReview_IsShowOutPermission": "N",
    "manualReview_IsShowChangeCaseType": "N",
    "manualReview_IsShowInPermission": "N",
    "manualReview_IsShowReturnReview": "N",
    "isShowUpdateSameIPCheckRecord": "N",
    "isShowUpdateWebEmailCheckRecord": "N",
    "isShowUpdateWebMobileCheckRecord": "N",
    "isShowUpdateInternalIPCheckRecord": "N",
    "isShowUpdateShortTimeIDCheckRecord": "N",
    "isShowInternalEmail": "N",
    "isShowInternalMobile": "N",
    "isShowUpdateInternalEmailCheckRecord": "N",
    "isShowUpdateInternalMobileCheckRecord": "N",
    "isShowUpdateSupplementaryInfo": "N",
    "isShowKYCSync": "N"
  },
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**成功回應 - 完整權限 (來自 Examples.cs Line 53-101):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": {
    "monthlyIncome_IsShowChangeCaseType": "Y",
    "monthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard": "Y",
    "monthlyIncome_IsShowInPermission": "Y",
    "monthlyIncome_IsShowMonthlyIncome": "Y",
    "isShowNameCheck": "Y",
    "isShowUpdatePrimaryInfo": "Y",
    "isShowQueryBranchInfo": "Y",
    "isShowQuery929": "Y",
    "isShowInsertFileAttachment": "Y",
    "isShowUpdateApplyNote": "Y",
    "insertReviewerSummary": "Y",
    "isShowFocus1": "Y",
    "isShowFocus2": "Y",
    "isShowWebMobileRequery": "Y",
    "isShowWebEmailRequery": "Y",
    "isShowUpdateReviewerSummary": "Y",
    "isShowDeleteReviewerSummary": "Y",
    "isShowDeleteApplyFileAttachment": "Y",
    "isShowCommunicationNotes": "Y",
    "manualReview_IsShowOutPermission": "N",
    "manualReview_IsShowChangeCaseType": "N",
    "manualReview_IsShowInPermission": "N",
    "manualReview_IsShowReturnReview": "N",
    "isShowUpdateSameIPCheckRecord": "Y",
    "isShowUpdateWebEmailCheckRecord": "Y",
    "isShowUpdateWebMobileCheckRecord": "Y",
    "isShowUpdateInternalIPCheckRecord": "Y",
    "isShowUpdateShortTimeIDCheckRecord": "Y",
    "isShowInternalEmail": "Y",
    "isShowInternalMobile": "Y",
    "isShowUpdateInternalEmailCheckRecord": "Y",
    "isShowUpdateInternalMobileCheckRecord": "Y",
    "isShowUpdateSupplementaryInfo": "Y",
    "isShowKYCSync": "N"
  },
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此申請書:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此ID: 20250101X0001",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 前端使用範例

### React/Vue 範例

```typescript
// 1. 呼叫 API 取得權限
const response = await fetch(`/api/ReviewerPermission/${applyNo}`);
const result = await response.json();
const permissions = result.data;

// 2. 根據權限控制 UI 顯示
{permissions.isShowNameCheck === "Y" && (
  <button onClick={handleNameCheck}>姓名檢核</button>
)}

{permissions.isShowUpdatePrimaryInfo === "Y" && (
  <button onClick={handleUpdatePrimaryInfo}>更新正卡人資料</button>
)}

{permissions.manualReview_IsShowInPermission === "Y" && (
  <button onClick={handleInPermission}>權限內</button>
)}

// 3. 動態生成按鈕
const buttons = [
  { key: 'isShowNameCheck', label: '姓名檢核', action: handleNameCheck },
  { key: 'isShowQuery929', label: '查詢929', action: handleQuery929 },
  // ...
];

return (
  <div>
    {buttons.map(btn =>
      permissions[btn.key] === "Y" && (
        <button key={btn.key} onClick={btn.action}>
          {btn.label}
        </button>
      )
    )}
  </div>
);
```

---

## 效能考量與優化建議

### 當前效能

- **資料庫查詢次數**: 3 次 (Main, Handle, Permission)
- **記憶體操作**: 反射遍歷 37 個屬性
- **時間複雜度**: O(n*m), n=Handle數量, m=Permission數量

### 優化建議

#### 1. 快取機制

```csharp
// 快取權限設定 (Auth_ReviewerPermission 不常變更)
private static MemoryCache _permissionCache = new MemoryCache(new MemoryCacheOptions());

var cacheKey = $"Permissions_{string.Join("_", handleCardStatuses)}";
var authReviewerPermissions = await _permissionCache.GetOrCreateAsync(cacheKey, async entry =>
{
    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
    return await context.Auth_ReviewerPermission.AsNoTracking()
        .Where(x => handleCardStatuses.Contains(x.CardStatus))
        .ToListAsync();
});
```

#### 2. 反射結果快取

```csharp
// 快取屬性資訊 (避免每次查詢都反射)
private static readonly PropertyInfo[] _permissionProperties =
    typeof(GetApplyPermissionByIdResponse).GetProperties()
        .Where(p => p.PropertyType == typeof(string))
        .ToArray();
```

#### 3. 批次查詢優化

```csharp
// 使用單一查詢取得所有需要的資料 (JOIN)
var result = await (
    from main in context.Reviewer_ApplyCreditCardInfoMain
    join handle in context.Reviewer_ApplyCreditCardInfoHandle
        on main.ApplyNo equals handle.ApplyNo
    join permission in context.Auth_ReviewerPermission
        on handle.CardStatus equals permission.CardStatus
    where main.ApplyNo == applyNo
    select new { main, handle, permission }
).ToListAsync();
```

#### 4. 快取前端結果

前端可以快取權限結果:
- 同一個申請書在一個 Session 內權限通常不變
- 使用 React Query 或 SWR 進行快取
- 減少重複的 API 呼叫

---

## 測試建議

### 單元測試重點

```csharp
[Fact]
public async Task Handle_申請書不存在_應回傳4001()
{
    // Arrange
    var applyNo = "NonExistent";

    // Act
    var result = await handler.Handle(new Query(applyNo), CancellationToken.None);

    // Assert
    Assert.Equal(4001, result.ReturnCode);
}

[Fact]
public async Task Handle_無處理記錄_應回傳預設權限()
{
    // Arrange
    var applyNo = "TestApply";
    // Mock: Main 存在但沒有 Handle

    // Act
    var result = await handler.Handle(new Query(applyNo), CancellationToken.None);

    // Assert
    Assert.Equal(2000, result.ReturnCode);
    Assert.Equal("N", result.Data.IsShowNameCheck);
}

[Fact]
public async Task Handle_非當前經辦_應回傳唯讀權限()
{
    // Arrange
    var applyNo = "TestApply";
    // Mock: CurrentHandleUserId != JWT.UserId
    // Mock: IsCurrentHandleUserId = "Y"

    // Act
    var result = await handler.Handle(new Query(applyNo), CancellationToken.None);

    // Assert
    Assert.Equal("N", result.Data.IsShowNameCheck);
}

[Fact]
public async Task Handle_當前經辦_應回傳完整權限()
{
    // Arrange
    var applyNo = "TestApply";
    // Mock: CurrentHandleUserId == JWT.UserId

    // Act
    var result = await handler.Handle(new Query(applyNo), CancellationToken.None);

    // Assert
    Assert.Equal("Y", result.Data.IsShowNameCheck);
}

[Fact]
public async Task Handle_多個Handle_應合併權限()
{
    // Arrange
    var applyNo = "TestApply";
    // Mock:
    //   Handle 1: IsShowNameCheck = "Y"
    //   Handle 2: IsShowNameCheck = "N"

    // Act
    var result = await handler.Handle(new Query(applyNo), CancellationToken.None);

    // Assert
    Assert.Equal("Y", result.Data.IsShowNameCheck);  // OR 邏輯
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/ReviewerPermission/GetApplyPermissionById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Model.cs | `Modules/Auth/ReviewerPermission/GetApplyPermissionById/Model.cs` | 回應模型定義 |
| Example.cs | `Modules/Auth/ReviewerPermission/GetApplyPermissionById/Example.cs` | Swagger 範例定義 |
| Auth_ReviewerPermission.cs | `Infrastructures/Data/Entities/Auth_ReviewerPermission.cs` | 徵審權限實體定義 |
| Reviewer_ApplyCreditCardInfoMain.cs | `Infrastructures/Data/Entities/Reviewer_ApplyCreditCardInfoMain.cs` | 申請書主檔實體定義 |
| Reviewer_ApplyCreditCardInfoHandle.cs | `Infrastructures/Data/Entities/Reviewer_ApplyCreditCardInfoHandle.cs` | 申請書處理記錄實體定義 |
| Auth_ReviewerPermissionProfiler.cs | `Infrastructures/Data/Mapper/Auth_ReviewerPermissionProfiler.cs` | AutoMapper 映射設定 |
