# GetReviewerPermissionById API - 查詢單筆徵審權限 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerPermission/{seqNo}` |
| **HTTP 方法** | GET |
| **功能** | 查詢單筆徵審權限資料 - 根據 SeqNo 取得完整的徵審權限設定資訊 |
| **位置** | `Modules/Auth/ReviewerPermission/GetReviewerPermissionById/Endpoint.cs` |

---

## Request 定義

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `seqNo` | int | ✅ | 徵審權限主鍵 (PK),用於識別要查詢的資料 |

### 請求範例

```
GET /ReviewerPermission/4
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": {
    "seqNo": 4,
    "cardStatus": "網路件_待月收入預審",
    "cardStatusName": "網路件_待月收入預審",
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
    "isCurrentHandleUserId": "Y",
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
    "isShowCommunicationNotes": "Y",
    "cardStep": null,
    "cardStepName": null,
    "manualReview_IsShowInPermission": "N",
    "manualReview_IsShowOutPermission": "N",
    "manualReview_IsShowReturnReview": "N",
    "manualReview_IsShowChangeCaseType": "N",
    "isShowInternalEmail": "N",
    "isShowInternalMobile": "N",
    "isShowUpdateInternalEmailCheckRecord": "Y",
    "isShowUpdateInternalMobileCheckRecord": "Y",
    "isShowUpdateSupplementaryInfo": "N",
    "isShowKYCSync": "N"
  },
  "traceId": "{traceId}"
}
```

### Response 欄位說明

```csharp
// 位置: Modules/Auth/ReviewerPermission/GetReviewerPermissionById/Model.cs (Line 3-310)
public class GetReviewerPermissionByIdResponse
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

    // 一般功能權限 (29 個) - 與 Insert/Update 相同
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

| 欄位類型 | 數量 | 說明 |
|---------|------|------|
| 主鍵與狀態欄位 | 5 | SeqNo, CardStatus, CardStatusName, CardStep, CardStepName |
| 審計欄位 | 4 | AddUserId, AddTime, UpdateUserId, UpdateTime |
| 權限欄位 | 37 | 與 Insert/Update 相同的所有權限欄位 |
| **總計** | **46** | 完整的徵審權限資訊 |

---

### 錯誤回應

#### 查無此 ID (400 Bad Request - Return Code: 4001)
- 指定的 SeqNo 不存在於資料庫中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此ID: 100",
  "data": null
}
```

#### 內部程式失敗 (500 Internal Server Error - Return Code: 5000)
- 系統內部處理錯誤

---

## 驗證資料

### 1. 路由參數驗證
- **位置**: ASP.NET Core 路由系統 (自動執行)
- **驗證內容**:
  - seqNo: 必須為有效的整數

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 47-59)

#### 檢查點: 資料存在性檢查
```
位置: Line 51-54
查詢 Auth_ReviewerPermission 表中是否存在指定的 SeqNo
├─ 若不存在 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此ID: {SeqNo}"
└─ 若存在 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 51
- **查詢**: 根據 SeqNo 查詢徵審權限資料
  ```csharp
  var entity = await context.Auth_ReviewerPermission.AsNoTracking()
      .SingleOrDefaultAsync(x => x.SeqNo == seqNo);
  ```

### 2. 資料轉換
- **位置**: Line 56
- **轉換方式**: 使用 AutoMapper 進行物件映射
  ```csharp
  var reponse = mapper.Map<GetReviewerPermissionByIdResponse>(entity);
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 驗證路由參數 seqNo 為有效整數
│  └─ 由 ASP.NET Core 路由系統自動處理
│
├─ 步驟 2: 查詢資料 (Line 51)
│  └─ 使用 AsNoTracking() 進行唯讀查詢,提升性能
│
├─ 步驟 3: 檢查資料是否存在 (Line 53-54)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 4: 物件映射 (Line 56)
│  ├─ 使用 AutoMapper 將實體轉換為 Response 物件
│  ├─ 自動映射所有權限欄位
│  └─ 自動產生 CardStatusName 和 CardStepName
│
└─ 步驟 5: 回傳成功訊息 (Line 58)
   └─ Return Code: 2000, Data: Response 物件
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_ReviewerPermission` | SELECT | 徵審權限主檔 - 唯讀查詢 |

### 操作類型

#### 查詢操作 (SELECT)

**查詢: 根據 SeqNo 查詢單筆資料**
- **位置**: Line 51
- **方法**: `AsNoTracking()` + `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var entity = await context.Auth_ReviewerPermission.AsNoTracking()
      .SingleOrDefaultAsync(x => x.SeqNo == seqNo);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2)
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
  WHERE [SeqNo] = @SeqNo
  ```

### AsNoTracking 優化
- 使用 `AsNoTracking()` 進行唯讀查詢
- 不啟用 EF Core 的變更追蹤機制
- 提升查詢性能,減少記憶體使用
- 適合唯讀場景,不需要後續更新操作

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 GET /ReviewerPermission/{seqNo} 請求
│
├─ ASP.NET Core 路由系統
│  ├─ 驗證 seqNo 為有效整數
│  │
│  ├─ 驗證失敗 → 400 Bad Request
│  └─ 驗證通過 → 繼續
│
├─ Handler.Handle 處理
│  │
│  ├─ 資料庫查詢 (Line 51)
│  │  ├─ 使用 AsNoTracking() 唯讀查詢
│  │  └─ 根據 SeqNo 查詢單筆資料
│  │
│  ├─ 檢查資料是否存在 (Line 53-54)
│  │  ├─ 查無資料 → 400 Bad Request (Return Code: 4001)
│  │  └─ 有資料 → 繼續
│  │
│  ├─ 資料轉換 (Line 56)
│  │  ├─ 使用 AutoMapper 映射
│  │  ├─ Entity → Response 物件
│  │  └─ 自動產生 CardStatusName 和 CardStepName
│  │
│  └─ 回傳成功回應 (Line 58)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "Success"
│        └─ Data: GetReviewerPermissionByIdResponse
```

---

## 關鍵業務規則

### 1. 唯讀查詢優化
- 使用 `AsNoTracking()` 進行唯讀查詢
- 不需要 EF Core 追蹤實體狀態
- 提升性能,特別是在高並發場景
- 減少記憶體佔用

### 2. 完整資訊回傳
- 回傳所有權限欄位 (37 個)
- 包含審計資訊 (AddUserId, AddTime, UpdateUserId, UpdateTime)
- 提供 CardStatusName 和 CardStepName 方便前端顯示
- 一次性取得完整權限設定資訊

### 3. AutoMapper 自動映射
- 使用 AutoMapper 進行物件映射
- 減少手動賦值的程式碼
- 自動處理欄位對應關係
- CardStepName 透過計算屬性自動產生 (ToString())

### 4. CardStatusName 和 CardStepName
- 這兩個欄位是衍生欄位,不存在於資料庫
- CardStatusName: 透過 AutoMapper 設定從 CardStatus 列舉轉換
- CardStepName: 透過計算屬性 `CardStep.ToString()` 產生
- 方便前端直接顯示,無需再次轉換

### 5. SingleOrDefaultAsync 語義
- 使用 `SingleOrDefaultAsync()` 而非 `FirstOrDefaultAsync()`
- 確保最多只有一筆資料符合條件
- SeqNo 是唯一主鍵,理論上只會有 0 或 1 筆資料
- 若有多筆會拋出異常,幫助發現資料完整性問題

### 6. 查詢結果為 Null 的處理
- 查詢不到資料時,entity 為 null
- 回傳 Return Code 4001 (查無此ID)
- 不拋出異常,而是回傳明確的錯誤訊息
- 符合 RESTful API 的設計慣例

### 7. 審計資訊的價值
- AddUserId 和 AddTime: 記錄誰在何時建立此權限設定
- UpdateUserId 和 UpdateTime: 記錄最後修改資訊
- 這些資訊對於追蹤權限變更歷史非常重要
- 可用於審計和問題排查

---

## 請求與回應範例

### 請求範例

```
GET /ReviewerPermission/4
```

### 回應範例

**成功回應 (來自 Examples.cs Line 8-48):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": {
    "seqNo": 4,
    "cardStatus": "網路件_待月收入預審",
    "cardStatusName": "網路件_待月收入預審",
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
    "isCurrentHandleUserId": "Y",
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
    "isShowCommunicationNotes": "Y",
    "cardStep": null,
    "cardStepName": null,
    "manualReview_IsShowInPermission": "N",
    "manualReview_IsShowOutPermission": "N",
    "manualReview_IsShowReturnReview": "N",
    "manualReview_IsShowChangeCaseType": "N",
    "isShowInternalEmail": "N",
    "isShowInternalMobile": "N",
    "isShowUpdateInternalEmailCheckRecord": "Y",
    "isShowUpdateInternalMobileCheckRecord": "Y",
    "isShowUpdateSupplementaryInfo": "N",
    "isShowKYCSync": "N"
  },
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此 ID:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此ID: 100",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/ReviewerPermission/GetReviewerPermissionById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Model.cs | `Modules/Auth/ReviewerPermission/GetReviewerPermissionById/Model.cs` | 回應模型定義 |
| Example.cs | `Modules/Auth/ReviewerPermission/GetReviewerPermissionById/Example.cs` | Swagger 範例定義 |
| Auth_ReviewerPermission.cs | `Infrastructures/Data/Entities/Auth_ReviewerPermission.cs` | 資料庫實體定義 |
| Auth_ReviewerPermissionProfiler.cs | `Infrastructures/Data/Mapper/Auth_ReviewerPermissionProfiler.cs` | AutoMapper 映射設定 |
