# InsertReviewerPermission API - 新增徵審權限 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerPermission` |
| **HTTP 方法** | POST |
| **功能** | 新增單筆徵審權限資料 - 用於建立不同案件狀態和階段的徵審權限設定 |
| **位置** | `Modules/Auth/ReviewerPermission/InsertReviewerPermission/Endpoint.cs` |

---

## Request 定義

### 請求體 (Body)

```csharp
// 位置: Modules/Auth/ReviewerPermission/InsertReviewerPermission/Model.cs (Line 3-339)
public class InsertReviewerPermissionRequest
{
    // 主鍵欄位
    public CardStatus CardStatus { get; set; }  // 案件狀態 (必填)
    public CardStep? CardStep { get; set; }      // 卡片階段 (選填)

    // 月收入確認相關權限 (4 個欄位)
    public string MonthlyIncome_IsShowChangeCaseType { get; set; }  // Y/N
    public string MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard { get; set; }  // Y/N
    public string MonthlyIncome_IsShowInPermission { get; set; }  // Y/N
    public string MonthlyIncome_IsShowMonthlyIncome { get; set; }  // Y/N

    // 人工徵審相關權限 (4 個欄位)
    public string ManualReview_IsShowChangeCaseType { get; set; }  // Y/N
    public string ManualReview_IsShowInPermission { get; set; }  // Y/N
    public string ManualReview_IsShowOutPermission { get; set; }  // Y/N
    public string ManualReview_IsShowReturnReview { get; set; }  // Y/N

    // 一般功能權限 (29 個欄位)
    public string IsShowNameCheck { get; set; }  // Y/N - 姓名檢核
    public string IsShowUpdatePrimaryInfo { get; set; }  // Y/N - 更新正卡人基本資料
    public string IsShowQueryBranchInfo { get; set; }  // Y/N - 查詢分行資訊
    public string IsShowQuery929 { get; set; }  // Y/N - 查詢929
    public string IsShowInsertFileAttachment { get; set; }  // Y/N - 新增圖檔
    public string IsShowUpdateApplyNote { get; set; }  // Y/N - 編輯附件備註
    public string IsCurrentHandleUserId { get; set; }  // Y/N - 是否為當前經辦
    public string InsertReviewerSummary { get; set; }  // Y/N - 新增照會摘要
    public string IsShowFocus1 { get; set; }  // Y/N - 再查詢關注名單1
    public string IsShowFocus2 { get; set; }  // Y/N - 再查詢關注名單2
    public string IsShowWebMobileRequery { get; set; }  // Y/N - 再查詢檢驗手機號碼相同
    public string IsShowWebEmailRequery { get; set; }  // Y/N - 再查詢檢驗電子信箱相同
    public string IsShowUpdateReviewerSummary { get; set; }  // Y/N - 徵審照會摘要-編輯
    public string IsShowDeleteReviewerSummary { get; set; }  // Y/N - 徵審照會摘要-刪除
    public string IsShowDeleteApplyFileAttachment { get; set; }  // Y/N - 圖檔刪除
    public string IsShowCommunicationNotes { get; set; }  // Y/N - 溝通備註
    public string IsShowUpdateSameIPCheckRecord { get; set; }  // Y/N - 儲存相同 IP確認紀錄
    public string IsShowUpdateWebMobileCheckRecord { get; set; }  // Y/N - 儲存網路手機號碼確認紀錄
    public string IsShowUpdateWebEmailCheckRecord { get; set; }  // Y/N - 儲存網路電子信箱確認紀錄
    public string IsShowUpdateInternalIPCheckRecord { get; set; }  // Y/N - 儲存行內 IP確認紀錄
    public string IsShowUpdateShortTimeIDCheckRecord { get; set; }  // Y/N - 儲存短時間頻繁確認紀錄
    public string IsShowInternalMobile { get; set; }  // Y/N - 再查詢行內手機
    public string IsShowInternalEmail { get; set; }  // Y/N - 再查詢行內Email
    public string IsShowUpdateInternalMobileCheckRecord { get; set; }  // Y/N - 儲存行內手機確認紀錄
    public string IsShowUpdateInternalEmailCheckRecord { get; set; }  // Y/N - 儲存行內Email確認紀錄
    public string IsShowUpdateSupplementaryInfo { get; set; }  // Y/N - 更新附卡人基本資料
    public string IsShowKYCSync { get; set; }  // Y/N - KYC入檔
}
```

### 參數說明表格 (主要欄位)

| 欄位 | 型別 | 必填 | 驗證規則 | 說明 |
|------|------|------|----------|------|
| `CardStatus` | CardStatus | ✅ | ValidEnumValue | 案件狀態,作為業務主鍵 |
| `CardStep` | CardStep? | ❌ | 無 | 卡片階段 (1.月收入確認 2.人工徵審) |
| **月收入確認權限組** | | | | |
| `MonthlyIncome_IsShowChangeCaseType` | string | ✅ | [YN] | 月收入確認_是否顯示變更案件種類 |
| `MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard` | string | ✅ | [YN] | 月收入確認_是否顯示變更卡別僅國旅卡 |
| `MonthlyIncome_IsShowInPermission` | string | ✅ | [YN] | 月收入確認_是否顯示權限內 |
| `MonthlyIncome_IsShowMonthlyIncome` | string | ✅ | [YN] | 月收入確認_是否顯示月收入確認 |
| **人工徵審權限組** | | | | |
| `ManualReview_IsShowChangeCaseType` | string | ✅ | [YN] | 人工徵審_是否顯示變更案件種類 |
| `ManualReview_IsShowInPermission` | string | ✅ | [YN] | 人工徵審_是否顯示權限內 |
| `ManualReview_IsShowOutPermission` | string | ✅ | [YN] | 人工徵審_是否顯示權限外 |
| `ManualReview_IsShowReturnReview` | string | ✅ | [YN] | 人工徵審_是否顯示退回重審 |
| **基本功能權限** | | | | |
| `IsShowNameCheck` | string | ✅ | [YN] | 姓名檢核 |
| `IsShowUpdatePrimaryInfo` | string | ✅ | [YN] | 更新正卡人基本資料 |
| `IsShowQueryBranchInfo` | string | ✅ | [YN] | 查詢分行資訊 |
| `IsShowQuery929` | string | ✅ | [YN] | 查詢929 |
| `IsShowInsertFileAttachment` | string | ✅ | [YN] | 新增圖檔 |
| `IsShowUpdateApplyNote` | string | ✅ | [YN] | 編輯附件備註_備註資料 |
| `IsCurrentHandleUserId` | string | ✅ | [YN] | 是否為當前經辦 |
| `InsertReviewerSummary` | string | ✅ | [YN] | 新增照會摘要 |
| **再查詢功能權限** | | | | |
| `IsShowFocus1` | string | ✅ | [YN] | 再查詢關注名單1 |
| `IsShowFocus2` | string | ✅ | [YN] | 再查詢關注名單2 |
| `IsShowWebMobileRequery` | string | ✅ | [YN] | 再查詢檢驗手機號碼相同 |
| `IsShowWebEmailRequery` | string | ✅ | [YN] | 再查詢檢驗電子信箱相同 |
| `IsShowInternalMobile` | string | ✅ | [YN] | 再查詢行內手機 |
| `IsShowInternalEmail` | string | ✅ | [YN] | 再查詢行內Email |
| **照會與附件權限** | | | | |
| `IsShowUpdateReviewerSummary` | string | ✅ | [YN] | 徵審照會摘要-編輯 |
| `IsShowDeleteReviewerSummary` | string | ✅ | [YN] | 徵審照會摘要-刪除 |
| `IsShowDeleteApplyFileAttachment` | string | ✅ | [YN] | 圖檔刪除 |
| `IsShowCommunicationNotes` | string | ✅ | [YN] | 溝通備註 |
| **檢查紀錄權限** | | | | |
| `IsShowUpdateSameIPCheckRecord` | string | ✅ | [YN] | 儲存相同 IP確認紀錄 |
| `IsShowUpdateWebMobileCheckRecord` | string | ✅ | [YN] | 儲存網路手機號碼確認紀錄 |
| `IsShowUpdateWebEmailCheckRecord` | string | ✅ | [YN] | 儲存網路電子信箱確認紀錄 |
| `IsShowUpdateInternalIPCheckRecord` | string | ✅ | [YN] | 儲存行內 IP確認紀錄 |
| `IsShowUpdateShortTimeIDCheckRecord` | string | ✅ | [YN] | 儲存短時間頻繁確認紀錄 |
| `IsShowUpdateInternalMobileCheckRecord` | string | ✅ | [YN] | 儲存行內手機確認紀錄 |
| `IsShowUpdateInternalEmailCheckRecord` | string | ✅ | [YN] | 儲存行內Email確認紀錄 |
| **其他權限** | | | | |
| `IsShowUpdateSupplementaryInfo` | string | ✅ | [YN] | 更新附卡人基本資料 |
| `IsShowKYCSync` | string | ✅ | [YN] | KYC入檔 (在人工徵信中顯示) |

> **說明**: 此 API 共包含 37 個權限欄位,所有權限欄位皆為必填,且必須為 'Y' 或 'N'。

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "新增成功: 1",
  "data": "1",
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 格式驗證失敗 (400 Bad Request - Return Code: 4000)
- 請求格式不符合定義
- 缺少必填欄位
- 欄位格式不符合驗證規則 ([YN] 正則表達式)

```json
{
  "returnCode": 4000,
  "returnMessage": "格式驗證失敗",
  "data": {
    "CardStatus": ["CardStatus 為必填欄位"],
    "IsShowNameCheck": ["IsShowNameCheck 必須符合正則表達式 [YN]"]
  }
}
```

#### 資料已存在 (400 Bad Request - Return Code: 4002)
- CardStatus 和 CardStep 的組合已存在於資料庫中

```json
{
  "returnCode": 4002,
  "returnMessage": "資料已存在: 1",
  "data": null
}
```

#### 內部程式失敗 (500 Internal Server Error - Return Code: 5000)
- 系統內部處理錯誤

#### 資料庫執行失敗 (500 Internal Server Error - Return Code: 5002)
- 資料庫操作失敗

---

## 驗證資料

### 1. 格式驗證
- **位置**: ASP.NET Core Model Validation (自動執行)
- **驗證內容**:
  - CardStatus: 必填, 必須為有效的 CardStatus 列舉值
  - CardStep: 選填, 若有值必須為有效的 CardStep 列舉值
  - 所有權限欄位 (36 個): 必填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 44-100)

#### 檢查點: CardStatus + CardStep 唯一性檢查
```
位置: Line 48-53
檢查 Auth_ReviewerPermission 表中是否已存在相同的 CardStatus 和 CardStep 組合
├─ 查詢條件: CardStatus = {dto.CardStatus} AND CardStep = {dto.CardStep}
├─ 若存在 → 拋出 DataAlreadyExists 錯誤 (Return Code: 4002)
│  └─ 訊息: "資料已存在: {SeqNo}"
└─ 若不存在 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 48-50
- **查詢**: 檢查 CardStatus + CardStep 是否已存在
  ```csharp
  await _context.Auth_ReviewerPermission.AsNoTracking()
      .SingleOrDefaultAsync(x => x.CardStatus == dto.CardStatus && x.CardStep == dto.CardStep)
  ```

### 2. 資料轉換
- **位置**: Line 55-94
- **轉換邏輯**:
  ```csharp
  Auth_ReviewerPermission auth_ReviewerPermission = new Auth_ReviewerPermission()
  {
      // 主鍵欄位
      CardStatus = dto.CardStatus,
      CardStep = dto.CardStep,

      // 月收入確認權限 (4 個)
      MonthlyIncome_IsShowChangeCaseType = dto.MonthlyIncome_IsShowChangeCaseType,
      MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = dto.MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard,
      MonthlyIncome_IsShowInPermission = dto.MonthlyIncome_IsShowInPermission,
      MonthlyIncome_IsShowMonthlyIncome = dto.MonthlyIncome_IsShowMonthlyIncome,

      // 人工徵審權限 (4 個)
      ManualReview_IsShowChangeCaseType = dto.ManualReview_IsShowChangeCaseType,
      ManualReview_IsShowInPermission = dto.ManualReview_IsShowInPermission,
      ManualReview_IsShowOutPermission = dto.ManualReview_IsShowOutPermission,
      ManualReview_IsShowReturnReview = dto.ManualReview_IsShowReturnReview,

      // 一般功能權限 (29 個欄位)
      IsShowNameCheck = dto.IsShowNameCheck,
      IsShowUpdatePrimaryInfo = dto.IsShowUpdatePrimaryInfo,
      IsShowQueryBranchInfo = dto.IsShowQueryBranchInfo,
      IsShowQuery929 = dto.IsShowQuery929,
      IsShowInsertFileAttachment = dto.IsShowInsertFileAttachment,
      IsShowUpdateApplyNote = dto.IsShowUpdateApplyNote,
      IsCurrentHandleUserId = dto.IsCurrentHandleUserId,
      InsertReviewerSummary = dto.InsertReviewerSummary,
      IsShowFocus1 = dto.IsShowFocus1,
      IsShowFocus2 = dto.IsShowFocus2,
      IsShowWebMobileRequery = dto.IsShowWebMobileRequery,
      IsShowWebEmailRequery = dto.IsShowWebEmailRequery,
      IsShowUpdateReviewerSummary = dto.IsShowUpdateReviewerSummary,
      IsShowDeleteReviewerSummary = dto.IsShowDeleteReviewerSummary,
      IsShowDeleteApplyFileAttachment = dto.IsShowDeleteApplyFileAttachment,
      IsShowCommunicationNotes = dto.IsShowCommunicationNotes,
      IsShowUpdateSameIPCheckRecord = dto.IsShowUpdateSameIPCheckRecord,
      IsShowUpdateWebEmailCheckRecord = dto.IsShowUpdateWebEmailCheckRecord,
      IsShowUpdateWebMobileCheckRecord = dto.IsShowUpdateWebMobileCheckRecord,
      IsShowUpdateInternalIPCheckRecord = dto.IsShowUpdateInternalIPCheckRecord,
      IsShowUpdateShortTimeIDCheckRecord = dto.IsShowUpdateShortTimeIDCheckRecord,
      IsShowInternalEmail = dto.IsShowInternalEmail,
      IsShowInternalMobile = dto.IsShowInternalMobile,
      IsShowUpdateInternalEmailCheckRecord = dto.IsShowUpdateInternalEmailCheckRecord,
      IsShowUpdateInternalMobileCheckRecord = dto.IsShowUpdateInternalMobileCheckRecord,
      IsShowUpdateSupplementaryInfo = dto.IsShowUpdateSupplementaryInfo,
      IsShowKYCSync = dto.IsShowKYCSync,
  }
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查 CardStatus + CardStep 組合是否已存在 (Line 48-53)
│  ├─ 已存在 → 回傳錯誤 4002
│  └─ 不存在 → 繼續
│
├─ 步驟 2: 建立 Auth_ReviewerPermission 實體 (Line 55-94)
│  ├─ 設定 CardStatus 和 CardStep
│  ├─ 設定所有 37 個權限欄位
│  └─ AddUserId 和 AddTime 由資料庫自動設定 (預設值或觸發器)
│
├─ 步驟 3: 新增至資料庫 (Line 96-97)
│  └─ 呼叫 AddAsync() 和 SaveChangesAsync()
│
└─ 步驟 4: 回傳成功訊息 (Line 99)
   └─ Return Code: 2000, Data: SeqNo
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_ReviewerPermission` | SELECT, INSERT | 徵審權限主檔 - 查詢唯一性檢查及新增資料 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢: 檢查 CardStatus + CardStep 唯一性**
- **位置**: Line 48-50
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_ReviewerPermission.AsNoTracking()
      .SingleOrDefaultAsync(x => x.CardStatus == dto.CardStatus && x.CardStep == dto.CardStep);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_ReviewerPermission]
  WHERE [CardStatus] = @CardStatus
    AND [CardStep] = @CardStep
  ```

#### 2. 新增操作 (INSERT)

- **位置**: Line 96-97
- **方法**: `AddAsync()` + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  await _context.Auth_ReviewerPermission.AddAsync(auth_ReviewerPermission);
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  INSERT INTO [dbo].[Auth_ReviewerPermission]
      ([CardStatus], [CardStep],
       [MonthlyIncome_IsShowChangeCaseType], [MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard],
       [MonthlyIncome_IsShowInPermission], [MonthlyIncome_IsShowMonthlyIncome],
       [IsShowNameCheck], [IsShowUpdatePrimaryInfo], [IsShowQueryBranchInfo], [IsShowQuery929],
       [IsShowInsertFileAttachment], [IsShowUpdateApplyNote], [IsCurrentHandleUserId],
       [InsertReviewerSummary], [IsShowFocus1], [IsShowFocus2], [IsShowWebMobileRequery],
       [IsShowWebEmailRequery], [IsShowUpdateReviewerSummary], [IsShowDeleteReviewerSummary],
       [IsShowDeleteApplyFileAttachment], [IsShowCommunicationNotes],
       [ManualReview_IsShowChangeCaseType], [ManualReview_IsShowInPermission],
       [ManualReview_IsShowOutPermission], [ManualReview_IsShowReturnReview],
       [IsShowUpdateSameIPCheckRecord], [IsShowUpdateWebEmailCheckRecord],
       [IsShowUpdateWebMobileCheckRecord], [IsShowUpdateInternalIPCheckRecord],
       [IsShowUpdateShortTimeIDCheckRecord], [IsShowInternalEmail], [IsShowInternalMobile],
       [IsShowUpdateInternalEmailCheckRecord], [IsShowUpdateInternalMobileCheckRecord],
       [IsShowUpdateSupplementaryInfo], [IsShowKYCSync])
  VALUES
      (@CardStatus, @CardStep,
       @MonthlyIncome_IsShowChangeCaseType, @MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard,
       @MonthlyIncome_IsShowInPermission, @MonthlyIncome_IsShowMonthlyIncome,
       @IsShowNameCheck, @IsShowUpdatePrimaryInfo, @IsShowQueryBranchInfo, @IsShowQuery929,
       @IsShowInsertFileAttachment, @IsShowUpdateApplyNote, @IsCurrentHandleUserId,
       @InsertReviewerSummary, @IsShowFocus1, @IsShowFocus2, @IsShowWebMobileRequery,
       @IsShowWebEmailRequery, @IsShowUpdateReviewerSummary, @IsShowDeleteReviewerSummary,
       @IsShowDeleteApplyFileAttachment, @IsShowCommunicationNotes,
       @ManualReview_IsShowChangeCaseType, @ManualReview_IsShowInPermission,
       @ManualReview_IsShowOutPermission, @ManualReview_IsShowReturnReview,
       @IsShowUpdateSameIPCheckRecord, @IsShowUpdateWebEmailCheckRecord,
       @IsShowUpdateWebMobileCheckRecord, @IsShowUpdateInternalIPCheckRecord,
       @IsShowUpdateShortTimeIDCheckRecord, @IsShowInternalEmail, @IsShowInternalMobile,
       @IsShowUpdateInternalEmailCheckRecord, @IsShowUpdateInternalMobileCheckRecord,
       @IsShowUpdateSupplementaryInfo, @IsShowKYCSync)
  ```

### Auth_ReviewerPermission 資料表結構

| 欄位名稱 | 資料型別 | 允許NULL | 說明 |
|---------|---------|---------|------|
| `SeqNo` | int | ❌ | 主鍵 - 自動遞增 |
| `CardStatus` | CardStatus | ❌ | 案件狀態 (業務主鍵) |
| `CardStep` | CardStep? | ✅ | 卡片階段 |
| `AddUserId` | string | ❌ | 新增員工編號 |
| `AddTime` | DateTime | ❌ | 新增時間 |
| `UpdateUserId` | string | ✅ | 修改員工編號 |
| `UpdateTime` | DateTime | ✅ | 修改時間 |
| **月收入確認權限** | | | |
| `MonthlyIncome_IsShowChangeCaseType` | string(1) | ❌ | 月收入確認_是否顯示變更案件種類 (Y/N) |
| `MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard` | string(1) | ❌ | 月收入確認_是否顯示變更卡別僅國旅卡 (Y/N) |
| `MonthlyIncome_IsShowInPermission` | string(1) | ❌ | 月收入確認_是否顯示權限內 (Y/N) |
| `MonthlyIncome_IsShowMonthlyIncome` | string(1) | ❌ | 月收入確認_是否顯示月收入確認 (Y/N) |
| **人工徵審權限** | | | |
| `ManualReview_IsShowChangeCaseType` | string(1) | ❌ | 人工徵審_是否顯示變更案件種類 (Y/N) |
| `ManualReview_IsShowInPermission` | string(1) | ❌ | 人工徵審_是否顯示權限內 (Y/N) |
| `ManualReview_IsShowOutPermission` | string(1) | ❌ | 人工徵審_是否顯示權限外 (Y/N) |
| `ManualReview_IsShowReturnReview` | string(1) | ❌ | 人工徵審_是否顯示退回重審 (Y/N) |
| **一般功能權限** | | | |
| `IsShowNameCheck` | string(1) | ❌ | 姓名檢核 (Y/N) |
| `IsShowUpdatePrimaryInfo` | string(1) | ❌ | 更新正卡人基本資料 (Y/N) |
| `IsShowQueryBranchInfo` | string(1) | ❌ | 查詢分行資訊 (Y/N) |
| `IsShowQuery929` | string(1) | ❌ | 查詢929 (Y/N) |
| `IsShowInsertFileAttachment` | string(1) | ❌ | 新增圖檔 (Y/N) |
| `IsShowUpdateApplyNote` | string(1) | ❌ | 編輯附件備註 (Y/N) |
| `IsCurrentHandleUserId` | string(1) | ❌ | 是否為當前經辦 (Y/N) |
| `InsertReviewerSummary` | string(1) | ❌ | 新增照會摘要 (Y/N) |
| `IsShowFocus1` | string(1) | ❌ | 再查詢關注名單1 (Y/N) |
| `IsShowFocus2` | string(1) | ❌ | 再查詢關注名單2 (Y/N) |
| `IsShowWebMobileRequery` | string(1) | ❌ | 再查詢檢驗手機號碼相同 (Y/N) |
| `IsShowWebEmailRequery` | string(1) | ❌ | 再查詢檢驗電子信箱相同 (Y/N) |
| `IsShowUpdateReviewerSummary` | string(1) | ❌ | 徵審照會摘要-編輯 (Y/N) |
| `IsShowDeleteReviewerSummary` | string(1) | ❌ | 徵審照會摘要-刪除 (Y/N) |
| `IsShowDeleteApplyFileAttachment` | string(1) | ❌ | 圖檔刪除 (Y/N) |
| `IsShowCommunicationNotes` | string(1) | ❌ | 溝通備註 (Y/N) |
| `IsShowUpdateSameIPCheckRecord` | string(1) | ❌ | 儲存相同 IP確認紀錄 (Y/N) |
| `IsShowUpdateWebMobileCheckRecord` | string(1) | ❌ | 儲存網路手機號碼確認紀錄 (Y/N) |
| `IsShowUpdateWebEmailCheckRecord` | string(1) | ❌ | 儲存網路電子信箱確認紀錄 (Y/N) |
| `IsShowUpdateInternalIPCheckRecord` | string(1) | ❌ | 儲存行內 IP確認紀錄 (Y/N) |
| `IsShowUpdateShortTimeIDCheckRecord` | string(1) | ❌ | 儲存短時間頻繁確認紀錄 (Y/N) |
| `IsShowInternalMobile` | string(1) | ❌ | 再查詢行內手機 (Y/N) |
| `IsShowInternalEmail` | string(1) | ❌ | 再查詢行內Email (Y/N) |
| `IsShowUpdateInternalMobileCheckRecord` | string(1) | ❌ | 儲存行內手機確認紀錄 (Y/N) |
| `IsShowUpdateInternalEmailCheckRecord` | string(1) | ❌ | 儲存行內Email確認紀錄 (Y/N) |
| `IsShowUpdateSupplementaryInfo` | string(1) | ❌ | 更新附卡人基本資料 (Y/N) |
| `IsShowKYCSync` | string(1) | ❌ | KYC入檔 (Y/N) |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 POST /ReviewerPermission 請求
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證 CardStatus 為有效列舉值
│  ├─ 驗證所有權限欄位符合 [YN] 格式
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯驗證 (Handler.Handle)
│  │
│  ├─ 檢查 CardStatus + CardStep 唯一性 (Line 48-53)
│  │  ├─ 已存在 → 400 Bad Request (Return Code: 4002)
│  │  └─ 不存在 → 繼續
│  │
│  ├─ 建立 Auth_ReviewerPermission 實體 (Line 55-94)
│  │  ├─ 設定 CardStatus 和 CardStep
│  │  └─ 設定所有 37 個權限欄位
│  │
│  ├─ 新增至資料庫 (Line 96-97)
│  │  ├─ AddAsync(auth_ReviewerPermission)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 回傳成功回應 (Line 99)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "新增成功: {SeqNo}"
│        └─ Data: SeqNo
```

---

## 關鍵業務規則

### 1. CardStatus + CardStep 唯一性
- CardStatus 和 CardStep 的組合作為業務主鍵,必須在系統中唯一
- 新增前必須檢查是否已存在相同組合
- 若已存在則拋出錯誤,不允許重複新增

### 2. CardStep 的意義
- CardStep 為選填欄位,用於區分相同 CardStatus 在不同階段的權限設定
- 1. 月收入確認 - 適用於月收入確認階段的權限
- 2. 人工徵審 - 適用於人工徵審階段的權限
- 某些狀態在各階段重複使用,透過 CardStep 區分不同的業務流程

### 3. 權限欄位分組邏輯
權限欄位分為四大組:
- **月收入確認權限組** (4 個): 控制月收入確認階段的功能顯示
- **人工徵審權限組** (4 個): 控制人工徵審階段的功能顯示
- **基本功能權限** (18 個): 控制一般操作功能的顯示
- **檢查紀錄權限** (11 個): 控制各類檢查紀錄的儲存和查詢功能

### 4. IsCurrentHandleUserId 的重要性
- 此欄位控制權限是否僅限當前經辦人員使用
- 'Y': 權限僅開放給當前經辦人員 (CurrentHandleUserId)
- 'N': 權限開放給所有有權限的人員
- 這是實現精細化權限控制的關鍵機制

### 5. 所有權限欄位必填且為 Y/N
- 所有 37 個權限欄位都是必填的
- 每個欄位只能是 'Y' (啟用) 或 'N' (停用)
- 確保權限設定的完整性和明確性

### 6. 月收入確認與人工徵審的權限分離
- 系統設計了兩組獨立的審核權限
- MonthlyIncome_ 前綴: 專用於月收入確認階段
- ManualReview_ 前綴: 專用於人工徵審階段
- 允許同一案件在不同階段有不同的權限控制

### 7. 檢查紀錄的全面性
- 系統支援多種類型的檢查紀錄儲存
- 包括: IP檢查、手機號碼檢查、Email檢查、行內資料檢查、時間頻率檢查
- 每種檢查都有獨立的權限控制,實現細緻的風險管控

### 8. KYC入檔的特殊性
- IsShowKYCSync 控制 KYC (Know Your Customer) 入檔功能
- 僅在人工徵信階段顯示
- 這是合規性要求的重要功能

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 8-48)

```json
POST /ReviewerPermission
Content-Type: application/json

{
  "cardStatus": "退件作業中_終止狀態",
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
  "isCurrentHandleUserId": "N",
  "insertReviewerSummary": "Y",
  "isShowFocus1": "Y",
  "isShowFocus2": "Y",
  "isShowWebMobileRequery": "Y",
  "isShowWebEmailRequery": "Y",
  "isShowUpdateReviewerSummary": "Y",
  "isShowDeleteReviewerSummary": "Y",
  "isShowDeleteApplyFileAttachment": "Y",
  "isShowCommunicationNotes": "Y",
  "cardStep": null,
  "manualReview_IsShowInPermission": "N",
  "manualReview_IsShowOutPermission": "N",
  "manualReview_IsShowReturnReview": "N",
  "manualReview_IsShowChangeCaseType": "N",
  "isShowUpdateSameIPCheckRecord": "Y",
  "isShowUpdateWebEmailCheckRecord": "Y",
  "isShowUpdateWebMobileCheckRecord": "Y",
  "isShowUpdateInternalIPCheckRecord": "Y",
  "isShowUpdateShortTimeIDCheckRecord": "Y",
  "isShowInternalEmail": "Y",
  "isShowInternalMobile": "Y",
  "isShowUpdateInternalEmailCheckRecord": "Y",
  "isShowUpdateInternalMobileCheckRecord": "Y",
  "isShowUpdateSupplementaryInfo": "N",
  "isShowKYCSync": "N"
}
```

### 回應範例

**成功回應:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "新增成功: 1",
  "data": "1",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 資料已存在:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4002,
  "returnMessage": "資料已存在: 4",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/ReviewerPermission/InsertReviewerPermission/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Model.cs | `Modules/Auth/ReviewerPermission/InsertReviewerPermission/Model.cs` | 請求模型定義 |
| Example.cs | `Modules/Auth/ReviewerPermission/InsertReviewerPermission/Example.cs` | Swagger 範例定義 |
| Auth_ReviewerPermission.cs | `Infrastructures/Data/Entities/Auth_ReviewerPermission.cs` | 資料庫實體定義 |
