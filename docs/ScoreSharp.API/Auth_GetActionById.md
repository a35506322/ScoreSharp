# GetActionById API - 查詢單筆操作 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Action/{actionId}` |
| **HTTP 方法** | GET |
| **功能** | 查詢單筆操作資料 By Id - 用於取得指定的 API 操作設定 |
| **位置** | `Modules/Auth/Action/GetActionById/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token |

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `actionId` | string | ✅ | 操作主鍵,用於指定要查詢的操作 |

### 請求範例

```
GET /Action/GetBillDayByQueryString
Authorization: Bearer {jwt_token}
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "查詢成功",
  "data": {
    "actionId": "GetRouterCatregoriesByQueryString",
    "actionName": "查詢路由類別ByQueryString",
    "isCommon": "Y",
    "isActive": "Y",
    "addUserId": "ADMIN",
    "addTime": "2025-01-15T10:30:00",
    "updateUserId": "ADMIN",
    "updateTime": "2025-01-15T14:20:00",
    "routerId": "SetUpBillDay"
  },
  "traceId": "{traceId}"
}
```

### 回應欄位說明

```csharp
// 位置: Modules/Auth/Action/GetActionById/Model.cs (Line 3-49)
public class GetActionByIdResponse
{
    public string ActionId { get; set; }        // 英數字，API Action 名稱
    public string ActionName { get; set; }      // 中文，前端顯示功能
    public string IsCommon { get; set; }        // Y/N，如果是Y 不檢查權限
    public string IsActive { get; set; }        // Y/N
    public string AddUserId { get; set; }       // 新增員工
    public DateTime AddTime { get; set; }       // 新增時間
    public string? UpdateUserId { get; set; }   // 修正員工
    public DateTime? UpdateTime { get; set; }   // 修正時間
    public string RouterId { get; set; }        // 關聯Auth_Router
}
```

| 欄位 | 型別 | 允許NULL | 說明 |
|------|------|---------|------|
| `actionId` | string | ❌ | API Action 識別碼 |
| `actionName` | string | ❌ | API Action 中文名稱 |
| `isCommon` | string | ❌ | 是否是通用資料 (Y/N) |
| `isActive` | string | ❌ | 是否啟用 (Y/N) |
| `addUserId` | string | ❌ | 新增員工編號 |
| `addTime` | DateTime | ❌ | 新增時間 |
| `updateUserId` | string | ✅ | 修改員工編號 |
| `updateTime` | DateTime | ✅ | 修改時間 |
| `routerId` | string | ❌ | 路由主鍵 |

### 錯誤回應

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- ActionId 不存在於資料庫中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料: 顯示找不到的ID",
  "data": null,
  "traceId": "{traceId}"
}
```

#### 內部程式失敗 (500 Internal Server Error - Return Code: 5000)
- 系統內部處理錯誤

#### 資料庫執行失敗 (500 Internal Server Error - Return Code: 5002)
- 資料庫操作失敗

---

## 驗證資料

### 1. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 50-60)

#### 檢查點 1: ActionId 存在性檢查
```
位置: Line 52-55
檢查 Auth_Action 表中是否存在指定的 ActionId
├─ 若不存在 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料: {actionId}"
└─ 若存在 → 繼續執行,回傳資料
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 52
- **查詢**: 根據 ActionId 查詢單筆操作資料
  ```csharp
  await _context.Auth_Action.AsNoTracking().SingleOrDefaultAsync(x => x.ActionId == request.actionId)
  ```
- **說明**: 使用 AsNoTracking() 因為是唯讀查詢,不需要追蹤實體變更,提升效能

### 2. 資料轉換
- **位置**: Line 57
- **轉換邏輯**:
  ```csharp
  var response = _mapper.Map<GetActionByIdResponse>(single);
  ```
- **說明**: 使用 AutoMapper 將 Auth_Action 實體轉換為 GetActionByIdResponse

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 查詢 Auth_Action (Line 52)
│  └─ 使用 AsNoTracking() 提升查詢效能
│
├─ 步驟 2: 檢查是否存在 (Line 54-55)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 3: 資料轉換 (Line 57)
│  └─ 使用 AutoMapper 轉換為 Response 格式
│
└─ 步驟 4: 回傳成功訊息 (Line 59)
   └─ Return Code: 2000, Data: GetActionByIdResponse
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Action` | SELECT | 操作主檔 - 查詢單筆資料 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢: 根據 ActionId 查詢單筆資料**
- **位置**: Line 52
- **方法**: `AsNoTracking().SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_Action.AsNoTracking().SingleOrDefaultAsync(x => x.ActionId == request.actionId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Action]
  WHERE [ActionId] = @ActionId
  ```
- **說明**:
  - 使用 AsNoTracking() 因為是唯讀查詢,不需要追蹤實體變更
  - 提升查詢效能,減少記憶體使用
  - SingleOrDefaultAsync() 確保最多只返回一筆資料

### Auth_Action 資料表結構

| 欄位名稱 | 資料型別 | 允許NULL | 說明 |
|---------|---------|---------|------|
| `ActionId` | string(100) | ❌ | 主鍵 - API Action 識別碼 |
| `ActionName` | string(30) | ❌ | API Action 名稱 |
| `IsCommon` | string(1) | ❌ | 是否是通用資料 (Y/N) |
| `IsActive` | string(1) | ❌ | 是否啟用 (Y/N) |
| `AddUserId` | string | ❌ | 新增員工編號 |
| `AddTime` | DateTime | ❌ | 新增時間 |
| `UpdateUserId` | string | ✅ | 修改員工編號 |
| `UpdateTime` | DateTime | ✅ | 修改時間 |
| `RouterId` | string(50) | ❌ | 路由主鍵 (FK) |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 GET /Action/{actionId} 請求
│
├─ 商業邏輯處理 (Handler.Handle)
│  │
│  ├─ 查詢 Auth_Action (Line 52)
│  │  ├─ 使用 AsNoTracking() 提升效能
│  │  └─ 根據 ActionId 查詢單筆資料
│  │
│  ├─ 檢查是否存在 (Line 54-55)
│  │  ├─ 不存在 → 400 Bad Request (Return Code: 4001)
│  │  │  └─ 訊息: "查無此資料: {actionId}"
│  │  └─ 存在 → 繼續
│  │
│  ├─ 資料轉換 (Line 57)
│  │  └─ 使用 AutoMapper 轉換為 GetActionByIdResponse
│  │
│  └─ 回傳成功回應 (Line 59)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "查詢成功"
│        └─ Data: GetActionByIdResponse 物件
```

---

## 關鍵業務規則

### 1. ActionId 存在性檢查
- 查詢前必須確認 ActionId 存在於資料庫中
- 若不存在則拋出錯誤 (Return Code: 4001)
- 確保 API 回傳正確的錯誤訊息

### 2. AsNoTracking 查詢優化
- 使用 AsNoTracking() 因為是唯讀查詢
- 不需要追蹤實體變更狀態,提升查詢效能
- 減少記憶體使用,適合大量查詢場景

### 3. SingleOrDefaultAsync 使用
- 使用 SingleOrDefaultAsync() 確保最多只返回一筆資料
- 若查詢結果超過一筆會拋出異常
- 若查詢結果為零則返回 null

### 4. AutoMapper 資料轉換
- 使用 AutoMapper 將 Auth_Action 實體轉換為 Response DTO
- 避免直接回傳實體,符合 DTO 模式
- 提供彈性的資料結構轉換

### 5. 完整資料回傳
- 回傳完整的操作資料,包含審計欄位
- AddUserId, AddTime: 建立追蹤資訊
- UpdateUserId, UpdateTime: 修改追蹤資訊 (可能為 null)
- 提供完整的資料歷史記錄

### 6. 路由設計
- 遵循 RESTful API 設計原則
- GET /Action/{actionId} 用於查詢單筆資料
- 路由參數直接指定資源 ID

### 7. IsCommon 欄位說明
- 'Y' 表示通用資料,不檢查權限
- 'N' 表示需要權限檢查
- 前端可根據此欄位判斷是否需要權限驗證

### 8. IsActive 欄位說明
- 'Y' 表示啟用,該操作可以被使用
- 'N' 表示停用,該操作不可被使用
- 前端可根據此欄位過濾可用的操作

---

## 請求範例

### 請求範例 (來自 Endpoint.cs Line 13)

```
GET /Action/GetBillDayByQueryString
Authorization: Bearer {jwt_token}
```

### 回應範例

**成功回應 (來自 Examples.cs Line 17-28):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "查詢成功",
  "data": {
    "actionId": "GetRouterCatregoriesByQueryString",
    "actionName": "查詢路由類別ByQueryString",
    "isCommon": "Y",
    "routerId": "SetUpBillDay",
    "isActive": "Y",
    "addUserId": "ADMIN",
    "addTime": "2025-01-15T10:30:00",
    "updateUserId": "ADMIN",
    "updateTime": "2025-01-15T14:20:00"
  },
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此資料 (來自 Examples.cs Line 6-9):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此資料: 顯示找不到的ID",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Action/GetActionById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Model.cs | `Modules/Auth/Action/GetActionById/Model.cs` | 回應模型定義 |
| Examples.cs | `Modules/Auth/Action/GetActionById/Examples.cs` | Swagger 範例定義 |
| Auth_Action.cs | `Infrastructures/Data/Entities/Auth_Action.cs` | 資料庫實體定義 |
