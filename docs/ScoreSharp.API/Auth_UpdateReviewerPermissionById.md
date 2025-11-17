# UpdateReviewerPermissionById API - 更新徵審權限 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerPermission/{seqNo}` |
| **HTTP 方法** | PUT |
| **功能** | 更新單筆徵審權限資料 - 根據 SeqNo 更新指定徵審權限的所有設定 |
| **位置** | `Modules/Auth/ReviewerPermission/UpdateReviewerPermissionById/Endpoint.cs` |

---

## Request 定義

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `seqNo` | int | ✅ | 徵審權限主鍵 (PK),用於識別要更新的資料 |

### 請求體 (Body)

```csharp
// 位置: Modules/Auth/ReviewerPermission/UpdateReviewerPermissionById/Model.cs (Line 3-338)
public class UpdateReviewerPermissionByIdRequest
{
    // 主鍵欄位
    public int SeqNo { get; set; }  // 必須與路由參數一致
    public CardStep? CardStep { get; set; }  // 卡片階段 (選填)

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

    // 一般功能權限 (29 個欄位) - 與 Insert 相同
    public string IsShowNameCheck { get; set; }
    public string IsShowUpdatePrimaryInfo { get; set; }
    public string IsShowQueryBranchInfo { get; set; }
    public string IsShowQuery929 { get; set; }
    public string IsShowInsertFileAttachment { get; set; }
    public string IsShowUpdateApplyNote { get; set; }
    public string IsCurrentHandleUserId { get; set; }
    public string InsertReviewerSummary { get; set; }
    public string IsShowFocus1 { get; set; }
    public string IsShowFocus2 { get; set; }
    public string IsShowWebMobileRequery { get; set; }
    public string IsShowWebEmailRequery { get; set; }
    public string IsShowUpdateReviewerSummary { get; set; }
    public string IsShowDeleteReviewerSummary { get; set; }
    public string IsShowDeleteApplyFileAttachment { get; set; }
    public string IsShowCommunicationNotes { get; set; }
    public string IsShowUpdateSameIPCheckRecord { get; set; }
    public string IsShowUpdateWebEmailCheckRecord { get; set; }
    public string IsShowUpdateWebMobileCheckRecord { get; set; }
    public string IsShowUpdateInternalIPCheckRecord { get; set; }
    public string IsShowUpdateShortTimeIDCheckRecord { get; set; }
    public string IsShowInternalMobile { get; set; }
    public string IsShowInternalEmail { get; set; }
    public string IsShowUpdateInternalMobileCheckRecord { get; set; }
    public string IsShowUpdateInternalEmailCheckRecord { get; set; }
    public string IsShowUpdateSupplementaryInfo { get; set; }
    public string IsShowKYCSync { get; set; }
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 驗證規則 | 說明 |
|------|------|------|----------|------|
| **路由參數** | | | | |
| `seqNo` (路由) | int | ✅ | 無 | 徵審權限主鍵 |
| **請求體** | | | | |
| `SeqNo` | int | ✅ | 無 | 必須與路由參數 seqNo 一致 |
| `CardStep` | CardStep? | ❌ | 無 | 卡片階段 (1.月收入確認 2.人工徵審) |
| **所有權限欄位** | string | ✅ | [YN] | 共 37 個權限欄位,格式與 Insert 相同 |

> **重要**: Request Body 中的 SeqNo 必須與路由參數的 seqNo 一致,否則會回傳錯誤 4003。

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "更新成功: 123",
  "data": "123",
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 格式驗證失敗 (400 Bad Request - Return Code: 4000)
- 請求格式不符合定義
- 缺少必填欄位
- 欄位格式不符合驗證規則

```json
{
  "returnCode": 4000,
  "returnMessage": "格式驗證失敗",
  "data": {
    "SeqNo": ["SeqNo 為必填欄位"],
    "IsShowNameCheck": ["IsShowNameCheck 必須符合正則表達式 [YN]"]
  }
}
```

#### 查無此 ID (400 Bad Request - Return Code: 4001)
- 指定的 SeqNo 不存在於資料庫中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此ID: 123",
  "data": null
}
```

#### 路由與 Request 比對錯誤 (400 Bad Request - Return Code: 4003)
- 路由參數的 seqNo 與 Request Body 的 SeqNo 不一致

```json
{
  "returnCode": 4003,
  "returnMessage": "路由與Req比對錯誤",
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
  - SeqNo: 必填
  - CardStep: 選填, 若有值必須為有效的 CardStep 列舉值
  - 所有權限欄位 (37 個): 必填, 必須符合正則表達式 `[YN]`

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 39-92)

#### 檢查點 1: 路由與 Request 一致性檢查
```
位置: Line 44-45
檢查路由參數 seqNo 是否與 Request Body 的 SeqNo 一致
├─ 若不一致 → 拋出路由與Req比對錯誤 (Return Code: 4003)
│  └─ 訊息: "路由與Req比對錯誤"
└─ 若一致 → 繼續執行
```

#### 檢查點 2: 資料存在性檢查
```
位置: Line 47-50
查詢 Auth_ReviewerPermission 表中是否存在指定的 SeqNo
├─ 若不存在 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此ID: {SeqNo}"
└─ 若存在 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 47
- **查詢**: 根據 SeqNo 查詢要更新的實體
  ```csharp
  var entity = await context.Auth_ReviewerPermission.SingleOrDefaultAsync(x => x.SeqNo == seqNo);
  ```

### 2. 資料更新
- **位置**: Line 52-87
- **更新邏輯**: 使用 EF Core 的 Change Tracking 機制
  ```csharp
  // 更新所有權限欄位
  entity.MonthlyIncome_IsShowChangeCaseType = dto.MonthlyIncome_IsShowChangeCaseType;
  entity.MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = dto.MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard;
  // ... (共 37 個權限欄位)
  entity.CardStep = dto.CardStep;
  entity.IsShowKYCSync = dto.IsShowKYCSync;
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查路由參數與 Request Body 一致性 (Line 44-45)
│  ├─ 不一致 → 回傳錯誤 4003
│  └─ 一致 → 繼續
│
├─ 步驟 2: 查詢資料是否存在 (Line 47-50)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 3: 更新實體的所有權限欄位 (Line 52-87)
│  ├─ 更新 CardStep
│  ├─ 更新月收入確認權限 (4 個)
│  ├─ 更新人工徵審權限 (4 個)
│  └─ 更新一般功能權限 (29 個)
│
├─ 步驟 4: 儲存變更至資料庫 (Line 89)
│  └─ 呼叫 SaveChangesAsync()
│
└─ 步驟 5: 回傳成功訊息 (Line 91)
   └─ Return Code: 2000, Data: SeqNo
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_ReviewerPermission` | SELECT, UPDATE | 徵審權限主檔 - 查詢及更新資料 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢: 根據 SeqNo 查詢資料**
- **位置**: Line 47
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var entity = await context.Auth_ReviewerPermission.SingleOrDefaultAsync(x => x.SeqNo == seqNo);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_ReviewerPermission]
  WHERE [SeqNo] = @SeqNo
  ```

#### 2. 更新操作 (UPDATE)

- **位置**: Line 52-89
- **方法**: EF Core Change Tracking + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  // EF Core 自動追蹤實體變更
  entity.MonthlyIncome_IsShowChangeCaseType = dto.MonthlyIncome_IsShowChangeCaseType;
  // ... 更新所有欄位
  await context.SaveChangesAsync(cancellationToken);
  ```
- **等效 SQL**:
  ```sql
  UPDATE [dbo].[Auth_ReviewerPermission]
  SET
      [CardStep] = @CardStep,
      [MonthlyIncome_IsShowChangeCaseType] = @MonthlyIncome_IsShowChangeCaseType,
      [MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard] = @MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard,
      [MonthlyIncome_IsShowInPermission] = @MonthlyIncome_IsShowInPermission,
      [MonthlyIncome_IsShowMonthlyIncome] = @MonthlyIncome_IsShowMonthlyIncome,
      [IsShowNameCheck] = @IsShowNameCheck,
      [IsShowUpdatePrimaryInfo] = @IsShowUpdatePrimaryInfo,
      [IsShowQueryBranchInfo] = @IsShowQueryBranchInfo,
      [IsShowQuery929] = @IsShowQuery929,
      [IsShowInsertFileAttachment] = @IsShowInsertFileAttachment,
      [IsShowUpdateApplyNote] = @IsShowUpdateApplyNote,
      [IsCurrentHandleUserId] = @IsCurrentHandleUserId,
      [InsertReviewerSummary] = @InsertReviewerSummary,
      [IsShowFocus1] = @IsShowFocus1,
      [IsShowFocus2] = @IsShowFocus2,
      [IsShowWebMobileRequery] = @IsShowWebMobileRequery,
      [IsShowWebEmailRequery] = @IsShowWebEmailRequery,
      [IsShowUpdateReviewerSummary] = @IsShowUpdateReviewerSummary,
      [IsShowDeleteReviewerSummary] = @IsShowDeleteReviewerSummary,
      [IsShowDeleteApplyFileAttachment] = @IsShowDeleteApplyFileAttachment,
      [IsShowCommunicationNotes] = @IsShowCommunicationNotes,
      [ManualReview_IsShowChangeCaseType] = @ManualReview_IsShowChangeCaseType,
      [ManualReview_IsShowInPermission] = @ManualReview_IsShowInPermission,
      [ManualReview_IsShowOutPermission] = @ManualReview_IsShowOutPermission,
      [ManualReview_IsShowReturnReview] = @ManualReview_IsShowReturnReview,
      [IsShowUpdateSameIPCheckRecord] = @IsShowUpdateSameIPCheckRecord,
      [IsShowUpdateWebEmailCheckRecord] = @IsShowUpdateWebEmailCheckRecord,
      [IsShowUpdateWebMobileCheckRecord] = @IsShowUpdateWebMobileCheckRecord,
      [IsShowUpdateInternalIPCheckRecord] = @IsShowUpdateInternalIPCheckRecord,
      [IsShowUpdateShortTimeIDCheckRecord] = @IsShowUpdateShortTimeIDCheckRecord,
      [IsShowInternalEmail] = @IsShowInternalEmail,
      [IsShowInternalMobile] = @IsShowInternalMobile,
      [IsShowUpdateInternalEmailCheckRecord] = @IsShowUpdateInternalEmailCheckRecord,
      [IsShowUpdateInternalMobileCheckRecord] = @IsShowUpdateInternalMobileCheckRecord,
      [IsShowUpdateSupplementaryInfo] = @IsShowUpdateSupplementaryInfo,
      [IsShowKYCSync] = @IsShowKYCSync
  WHERE [SeqNo] = @SeqNo
  ```

### 變更追蹤機制
- EF Core 使用 Change Tracking 自動偵測實體變更
- 不需要明確標記為 Modified 狀態
- SaveChangesAsync() 時自動產生 UPDATE SQL 語句
- 只更新有變更的欄位 (若有設定)

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 PUT /ReviewerPermission/{seqNo} 請求
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證路由參數 seqNo 為有效整數
│  ├─ 驗證 Request Body 格式
│  ├─ 驗證所有權限欄位符合 [YN] 格式
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯驗證 (Handler.Handle)
│  │
│  ├─ 檢查路由與 Request 一致性 (Line 44-45)
│  │  ├─ seqNo != dto.SeqNo → 400 Bad Request (Return Code: 4003)
│  │  └─ 一致 → 繼續
│  │
│  ├─ 查詢資料是否存在 (Line 47-50)
│  │  ├─ 查無資料 → 400 Bad Request (Return Code: 4001)
│  │  └─ 有資料 → 繼續
│  │
│  ├─ 更新實體欄位 (Line 52-87)
│  │  ├─ 更新 CardStep
│  │  ├─ 更新所有月收入確認權限
│  │  ├─ 更新所有人工徵審權限
│  │  └─ 更新所有一般功能權限
│  │
│  ├─ 儲存至資料庫 (Line 89)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 回傳成功回應 (Line 91)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "更新成功: {SeqNo}"
│        └─ Data: SeqNo
```

---

## 關鍵業務規則

### 1. 路由參數與 Request Body 一致性
- 路由中的 seqNo 必須與 Request Body 的 SeqNo 完全一致
- 這是 RESTful API 的最佳實踐,確保請求的明確性
- 防止錯誤的路由和資料不匹配問題

### 2. 不允許更新 CardStatus
- CardStatus 是業務主鍵的一部分,不可更新
- 只能更新 CardStep 和所有權限欄位
- 若需要變更 CardStatus,應該刪除後重新新增

### 3. CardStep 可以更新
- CardStep 雖然也是業務主鍵的一部分,但允許更新
- 可以將 CardStep 從 null 更新為具體階段,或相反
- 這提供了權限設定的靈活性

### 4. 全量更新機制
- 此 API 採用全量更新模式
- 必須提供所有 37 個權限欄位的值
- 不支援部分欄位更新 (PATCH)
- 確保權限設定的完整性和一致性

### 5. 更新不檢查唯一性
- 更新時不檢查 CardStatus + CardStep 的唯一性
- 因為是根據 SeqNo 更新,SeqNo 本身就是唯一主鍵
- CardStatus 不可更新,所以不會產生重複問題

### 6. 審計追蹤自動化
- UpdateUserId 和 UpdateTime 由資料庫觸發器或預設值自動設定
- API 層不需要處理這些欄位
- 確保審計資訊的準確性

### 7. EF Core Change Tracking 優化
- 使用 EF Core 的變更追蹤機制
- 自動偵測欄位變更並產生最優化的 UPDATE 語句
- 提升性能並減少程式碼複雜度

### 8. 冪等性
- 此 API 具有冪等性
- 使用相同的參數多次呼叫,結果相同
- 符合 HTTP PUT 方法的語義

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 8-48)

```json
PUT /ReviewerPermission/123
Content-Type: application/json

{
  "seqNo": 123,
  "cardStep": "月收入確認",
  "monthlyIncome_IsShowChangeCaseType": "N",
  "monthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard": "N",
  "monthlyIncome_IsShowInPermission": "N",
  "monthlyIncome_IsShowMonthlyIncome": "N",
  "isShowNameCheck": "Y",
  "isShowUpdatePrimaryInfo": "Y",
  "isShowQueryBranchInfo": "Y",
  "isShowQuery929": "Y",
  "isShowInsertFileAttachment": "Y",
  "isShowUpdateApplyNote": "Y",
  "isCurrentHandleUserId": "Y",
  "insertReviewerSummary": "Y",
  "isShowFocus1": "N",
  "isShowFocus2": "N",
  "isShowWebMobileRequery": "N",
  "isShowWebEmailRequery": "N",
  "isShowUpdateReviewerSummary": "Y",
  "isShowDeleteReviewerSummary": "Y",
  "isShowDeleteApplyFileAttachment": "Y",
  "isShowCommunicationNotes": "Y",
  "manualReview_IsShowInPermission": "N",
  "manualReview_IsShowOutPermission": "N",
  "manualReview_IsShowReturnReview": "N",
  "manualReview_IsShowChangeCaseType": "N",
  "isShowUpdateSameIPCheckRecord": "Y",
  "isShowUpdateWebEmailCheckRecord": "Y",
  "isShowUpdateWebMobileCheckRecord": "Y",
  "isShowUpdateInternalIPCheckRecord": "Y",
  "isShowUpdateShortTimeIDCheckRecord": "Y",
  "isShowInternalEmail": "N",
  "isShowInternalMobile": "N",
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
  "returnMessage": "更新成功: 123",
  "data": "123",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此 ID:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此ID: 30011",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 路由與 Req 比對錯誤:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4003,
  "returnMessage": "路由與Req比對錯誤",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/ReviewerPermission/UpdateReviewerPermissionById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Model.cs | `Modules/Auth/ReviewerPermission/UpdateReviewerPermissionById/Model.cs` | 請求模型定義 |
| Example.cs | `Modules/Auth/ReviewerPermission/UpdateReviewerPermissionById/Example.cs` | Swagger 範例定義 |
| Auth_ReviewerPermission.cs | `Infrastructures/Data/Entities/Auth_ReviewerPermission.cs` | 資料庫實體定義 |
