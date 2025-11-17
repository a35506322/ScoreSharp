# GetReviewerPermissionsByQueryString API - 查詢多筆徵審權限 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerPermission` |
| **HTTP 方法** | GET |
| **功能** | 查詢多筆徵審權限資料 - 取得所有徵審權限設定的完整清單 |
| **位置** | `Modules/Auth/ReviewerPermission/GetReviewerPermissionsByQueryString/Endpoint.cs` |

---

## Request 定義

### 查詢參數 (Query String)

```csharp
// 位置: Modules/Auth/ReviewerPermission/GetReviewerPermissionsByQueryString/Model.cs (Line 3)
public class GetReviewerPermissionsByQueryStringRequest { }
```

> **說明**: 此 API 目前不接受任何查詢參數,會回傳所有徵審權限資料。

### 請求範例

```
GET /ReviewerPermission
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": [
    {
      "seqNo": 4,
      "cardStatus": "網路件_待月收入預審",
      "cardStatusName": "網路件_待月收入預審",
      "monthlyIncome_IsShowChangeCaseType": "N",
      "monthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard": "N",
      "monthlyIncome_IsShowInPermission": "N",
      "monthlyIncome_IsShowMonthlyIncome": "N",
      "isShowNameCheck": "N",
      "isShowUpdatePrimaryInfo": "N",
      "isShowQueryBranchInfo": "Y",
      "isShowQuery929": "Y",
      "isShowInsertFileAttachment": "Y",
      "isShowUpdateApplyNote": "Y",
      "isCurrentHandleUserId": "N",
      "insertReviewerSummary": "Y",
      "addUserId": "superadmin",
      "addTime": "2025-01-15T10:30:00",
      "updateUserId": null,
      "updateTime": null,
      "isShowFocus1": "N",
      "isShowFocus2": "N",
      "isShowWebMobileRequery": "N",
      "isShowWebEmailRequery": "N",
      "isShowUpdateReviewerSummary": "Y",
      "isShowDeleteReviewerSummary": "Y",
      "isShowDeleteApplyFileAttachment": "Y",
      "isShowCommunicationNotes": "N",
      "cardStep": null,
      "cardStepName": null,
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
    },
    {
      "seqNo": 2,
      "cardStatus": "補件_等待完成本案徵審",
      "cardStatusName": "補件_等待完成本案徵審",
      "monthlyIncome_IsShowChangeCaseType": "N",
      "monthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard": "N",
      "monthlyIncome_IsShowInPermission": "N",
      "monthlyIncome_IsShowMonthlyIncome": "Y",
      "isShowNameCheck": "Y",
      "isShowUpdatePrimaryInfo": "Y",
      "isShowQueryBranchInfo": "Y",
      "isShowQuery929": "Y",
      "isShowInsertFileAttachment": "Y",
      "isShowUpdateApplyNote": "Y",
      "isCurrentHandleUserId": "Y",
      "insertReviewerSummary": "Y",
      "addUserId": "superadmin",
      "addTime": "2025-01-15T09:20:00",
      "updateUserId": null,
      "updateTime": null,
      "isShowFocus1": "N",
      "isShowFocus2": "N",
      "isShowWebMobileRequery": "N",
      "isShowWebEmailRequery": "N",
      "isShowUpdateReviewerSummary": "Y",
      "isShowDeleteReviewerSummary": "Y",
      "isShowDeleteApplyFileAttachment": "Y",
      "isShowCommunicationNotes": "N",
      "cardStep": null,
      "cardStepName": null,
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
  ],
  "traceId": "{traceId}"
}
```

### Response 欄位說明

```csharp
// 位置: Modules/Auth/ReviewerPermission/GetReviewerPermissionsByQueryString/Model.cs (Line 5-312)
public class GetReviewerPermissionsByQueryStringResponse
{
    // 主鍵與狀態資訊
    public int SeqNo { get; set; }  // 主鍵
    public CardStatus CardStatus { get; set; }  // 案件狀態
    public string CardStatusName { get; set; }  // 案件狀態名稱
    public CardStep? CardStep { get; set; }  // 卡片階段
    public string? CardStepName { get; set; }  // 卡片階段名稱

    // 審計欄位
    public string AddUserId { get; set; }  // 新增員工
    public DateTime AddTime { get; set; }  // 新增時間
    public string? UpdateUserId { get; set; }  // 修正員工
    public DateTime? UpdateTime { get; set; }  // 修正時間

    // 所有權限欄位 (37 個) - 與 GetById 完全相同
    // ...
}
```

> **說明**: Response 結構與 `GetReviewerPermissionById` 完全相同,但回傳的是陣列 (List)。

---

## 驗證資料

### 無需驗證
- 此 API 不接受任何查詢參數
- 無需進行參數驗證

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 46
- **查詢**: 查詢所有徵審權限資料
  ```csharp
  var entities = await context.Auth_ReviewerPermission.AsNoTracking().ToListAsync();
  ```

### 2. 資料轉換
- **位置**: Line 48
- **轉換方式**: 使用 AutoMapper 進行批次物件映射
  ```csharp
  var response = mapper.Map<List<GetReviewerPermissionsByQueryStringResponse>>(entities);
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 查詢所有徵審權限資料 (Line 46)
│  ├─ 使用 AsNoTracking() 唯讀查詢
│  └─ 取得所有記錄
│
├─ 步驟 2: 批次物件映射 (Line 48)
│  ├─ 使用 AutoMapper 將 Entity List 轉換為 Response List
│  ├─ 自動映射所有欄位
│  └─ 自動產生 CardStatusName 和 CardStepName
│
└─ 步驟 3: 回傳成功訊息 (Line 50)
   └─ Return Code: 2000, Data: Response List
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_ReviewerPermission` | SELECT | 徵審權限主檔 - 查詢所有資料 |

### 操作類型

#### 查詢操作 (SELECT)

**查詢: 取得所有徵審權限資料**
- **位置**: Line 46
- **方法**: `AsNoTracking()` + `ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var entities = await context.Auth_ReviewerPermission.AsNoTracking().ToListAsync();
  ```
- **等效 SQL**:
  ```sql
  SELECT
      [SeqNo], [CardStatus], [CardStep],
      [AddUserId], [AddTime], [UpdateUserId], [UpdateTime],
      [MonthlyIncome_IsShowChangeCaseType],
      [MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard],
      [MonthlyIncome_IsShowInPermission],
      [MonthlyIncome_IsShowMonthlyIncome],
      [IsShowNameCheck], [IsShowUpdatePrimaryInfo],
      [IsShowQueryBranchInfo], [IsShowQuery929],
      [IsShowInsertFileAttachment], [IsShowUpdateApplyNote],
      [IsCurrentHandleUserId], [InsertReviewerSummary],
      [IsShowFocus1], [IsShowFocus2],
      [IsShowWebMobileRequery], [IsShowWebEmailRequery],
      [IsShowUpdateReviewerSummary], [IsShowDeleteReviewerSummary],
      [IsShowDeleteApplyFileAttachment], [IsShowCommunicationNotes],
      [ManualReview_IsShowChangeCaseType], [ManualReview_IsShowInPermission],
      [ManualReview_IsShowOutPermission], [ManualReview_IsShowReturnReview],
      [IsShowUpdateSameIPCheckRecord], [IsShowUpdateWebEmailCheckRecord],
      [IsShowUpdateWebMobileCheckRecord], [IsShowUpdateInternalIPCheckRecord],
      [IsShowUpdateShortTimeIDCheckRecord], [IsShowInternalEmail],
      [IsShowInternalMobile], [IsShowUpdateInternalEmailCheckRecord],
      [IsShowUpdateInternalMobileCheckRecord], [IsShowUpdateSupplementaryInfo],
      [IsShowKYCSync]
  FROM [dbo].[Auth_ReviewerPermission]
  ```

### 性能考量
- 使用 `AsNoTracking()` 提升唯讀查詢性能
- 一次性載入所有資料,適合資料量不大的場景
- 若資料量龐大,建議未來加入分頁功能

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 GET /ReviewerPermission 請求
│
├─ Handler.Handle 處理
│  │
│  ├─ 資料庫查詢 (Line 46)
│  │  ├─ 使用 AsNoTracking() 唯讀查詢
│  │  └─ 取得所有徵審權限資料
│  │
│  ├─ 批次資料轉換 (Line 48)
│  │  ├─ 使用 AutoMapper 批次映射
│  │  ├─ Entity List → Response List
│  │  └─ 自動產生 CardStatusName 和 CardStepName
│  │
│  └─ 回傳成功回應 (Line 50)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "Success"
│        └─ Data: List<GetReviewerPermissionsByQueryStringResponse>
```

---

## 關鍵業務規則

### 1. 無條件查詢全部資料
- 此 API 設計為查詢所有徵審權限資料
- 不接受任何篩選條件
- 適用於取得完整的權限設定清單
- 通常用於管理介面的權限設定頁面

### 2. Request 參數預留擴展性
- 雖然目前 Request 是空類別,但保留了查詢參數的結構
- 未來可以輕鬆擴展加入篩選條件,例如:
  - 按 CardStatus 篩選
  - 按 CardStep 篩選
  - 按 IsActive 篩選
  - 加入分頁參數 (PageSize, PageNumber)

### 3. 資料量考量
- 目前設計假設徵審權限資料量不大
- 所有案件狀態的權限設定通常在幾十筆以內
- 若未來資料量增長,建議加入分頁機制
- 或改為按需查詢 (按 CardStatus 或 CardStep 篩選)

### 4. 前端使用場景
典型的前端使用場景:
- **權限設定管理頁面**: 顯示所有權限設定的列表
- **權限設定下拉選單**: 提供所有可用的權限設定供選擇
- **權限設定比對**: 比較不同案件狀態的權限差異
- **快取資料源**: 前端可快取此資料,減少重複查詢

### 5. AutoMapper 批次映射
- 使用 `Map<List<T>>()` 進行批次映射
- AutoMapper 自動處理集合轉換
- 效能優於逐筆手動映射
- 程式碼簡潔,易於維護

### 6. 空列表處理
- 若資料庫中沒有任何徵審權限資料
- 會回傳空陣列 `[]`,而非錯誤
- Return Code 仍為 2000 (成功)
- 符合 RESTful API 的設計慣例

### 7. 排序考量
- 目前查詢結果沒有明確的排序
- 預設可能按 SeqNo 排序 (主鍵順序)
- 建議未來加入明確的排序邏輯:
  - 按 CardStatus 排序
  - 按 AddTime 排序 (最新優先)
  - 按自訂的優先級排序

---

## 請求與回應範例

### 請求範例

```
GET /ReviewerPermission
```

### 回應範例

**成功回應 (來自 Examples.cs Line 8-99):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": [
    {
      "seqNo": 4,
      "cardStatus": "網路件_待月收入預審",
      "cardStatusName": "網路件_待月收入預審",
      "monthlyIncome_IsShowChangeCaseType": "N",
      "monthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard": "N",
      "monthlyIncome_IsShowInPermission": "N",
      "monthlyIncome_IsShowMonthlyIncome": "N",
      "isShowNameCheck": "N",
      "isShowUpdatePrimaryInfo": "N",
      "isShowQueryBranchInfo": "Y",
      "isShowQuery929": "Y",
      "isShowInsertFileAttachment": "Y",
      "isShowUpdateApplyNote": "Y",
      "isCurrentHandleUserId": "N",
      "insertReviewerSummary": "Y",
      "addUserId": "superadmin",
      "addTime": "2025-01-15T10:30:00",
      "isShowFocus1": "N",
      "isShowFocus2": "N",
      "isShowWebMobileRequery": "N",
      "isShowWebEmailRequery": "N",
      "isShowUpdateReviewerSummary": "Y",
      "isShowDeleteReviewerSummary": "Y",
      "isShowDeleteApplyFileAttachment": "Y",
      "isShowCommunicationNotes": "N",
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
      "isShowInternalEmail": "N",
      "isShowInternalMobile": "N",
      "isShowUpdateInternalEmailCheckRecord": "Y",
      "isShowUpdateInternalMobileCheckRecord": "Y",
      "isShowUpdateSupplementaryInfo": "N",
      "isShowKYCSync": "N"
    },
    {
      "seqNo": 2,
      "cardStatus": "補件_等待完成本案徵審",
      "cardStatusName": "補件_等待完成本案徵審",
      "monthlyIncome_IsShowChangeCaseType": "N",
      "monthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard": "N",
      "monthlyIncome_IsShowInPermission": "N",
      "monthlyIncome_IsShowMonthlyIncome": "Y",
      "isShowNameCheck": "Y",
      "isShowUpdatePrimaryInfo": "Y",
      "isShowQueryBranchInfo": "Y",
      "isShowQuery929": "Y",
      "isShowInsertFileAttachment": "Y",
      "isShowUpdateApplyNote": "Y",
      "isCurrentHandleUserId": "Y",
      "insertReviewerSummary": "Y",
      "addUserId": "superadmin",
      "addTime": "2025-01-15T09:20:00",
      "isShowFocus1": "N",
      "isShowFocus2": "N",
      "isShowWebMobileRequery": "N",
      "isShowWebEmailRequery": "N",
      "isShowUpdateReviewerSummary": "Y",
      "isShowDeleteReviewerSummary": "Y",
      "isShowDeleteApplyFileAttachment": "Y",
      "isShowCommunicationNotes": "N",
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
      "isShowInternalEmail": "N",
      "isShowInternalMobile": "N",
      "isShowUpdateInternalEmailCheckRecord": "Y",
      "isShowUpdateInternalMobileCheckRecord": "Y",
      "isShowUpdateSupplementaryInfo": "N",
      "isShowKYCSync": "N"
    }
  ],
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**空資料回應:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": [],
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 未來擴展建議

### 1. 加入分頁功能
```csharp
public class GetReviewerPermissionsByQueryStringRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
```

### 2. 加入篩選條件
```csharp
public class GetReviewerPermissionsByQueryStringRequest
{
    public CardStatus? CardStatus { get; set; }  // 按案件狀態篩選
    public CardStep? CardStep { get; set; }      // 按卡片階段篩選
    public string? IsCurrentHandleUserId { get; set; }  // 按當前經辦篩選
}
```

### 3. 加入排序功能
```csharp
public class GetReviewerPermissionsByQueryStringRequest
{
    public string SortBy { get; set; } = "SeqNo";  // 排序欄位
    public string SortOrder { get; set; } = "asc";  // 排序方向
}
```

### 4. 加入欄位選擇
```csharp
public class GetReviewerPermissionsByQueryStringRequest
{
    public string[] Fields { get; set; }  // 指定要回傳的欄位
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/ReviewerPermission/GetReviewerPermissionsByQueryString/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Model.cs | `Modules/Auth/ReviewerPermission/GetReviewerPermissionsByQueryString/Model.cs` | 請求與回應模型定義 |
| Example.cs | `Modules/Auth/ReviewerPermission/GetReviewerPermissionsByQueryString/Example.cs` | Swagger 範例定義 |
| Auth_ReviewerPermission.cs | `Infrastructures/Data/Entities/Auth_ReviewerPermission.cs` | 資料庫實體定義 |
| Auth_ReviewerPermissionProfiler.cs | `Infrastructures/Data/Mapper/Auth_ReviewerPermissionProfiler.cs` | AutoMapper 映射設定 |
