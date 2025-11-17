# GetRouterById API - 取得路由 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Router/{routerId}` |
| **HTTP 方法** | GET |
| **功能** | 根據 RouterId 取得單筆路由資料 - 用於查詢指定路由的詳細設定 |
| **位置** | `Modules/Auth/Router/GetRouterById/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token (用於驗證使用者身份) |

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `routerId` | string | ✅ | 路由主鍵,要查詢的路由識別碼 |

### 請求範例

```
GET /Router/SetUpInternalIP
Authorization: Bearer {jwt_token}
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": {
    "routerId": "SetUpInternalIP",
    "routerName": "行內IP設定",
    "dynamicParams": null,
    "isActive": "Y",
    "addUserId": "admin",
    "addTime": "2024-11-17T10:30:00",
    "updateUserId": "admin",
    "updateTime": "2024-11-17T15:45:00",
    "routerCategoryId": "SetUp",
    "icon": "pi pi-user",
    "sort": 99
  },
  "traceId": "{traceId}"
}
```

### 回應資料結構

```csharp
// 位置: Modules/Auth/Router/GetRouterById/Models.cs (Line 3-59)
public class GetRouterByIdResponse
{
    /// <summary>
    /// 英數字,前端顯示不在網址,如:Todo
    /// </summary>
    public string RouterId { get; set; }

    /// <summary>
    /// 中文,前端顯示SideBar頁面名稱
    /// </summary>
    public string RouterName { get; set; }

    /// <summary>
    /// 給前端串接參數使用,如:/Todo/1 或 /Todo?params=
    /// </summary>
    public string? DynamicParams { get; set; }

    /// <summary>
    /// Y/N
    /// </summary>
    public string IsActive { get; set; }

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; }

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 修正員工
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 修正時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 關聯Auth_RouterCategory
    /// </summary>
    public string RouterCategoryId { get; set; }

    /// <summary>
    /// 用於裝飾前端網頁
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
```

### 回應欄位說明

| 欄位 | 型別 | 允許NULL | 說明 |
|------|------|---------|------|
| `routerId` | string | ❌ | 路由主鍵,英數字,前端顯示不在網址 |
| `routerName` | string | ❌ | 路由名稱,中文,前端顯示SideBar頁面名稱 |
| `dynamicParams` | string | ✅ | 動態參數,給前端串接參數使用 |
| `isActive` | string | ❌ | 是否啟用 (Y/N) |
| `addUserId` | string | ❌ | 新增員工編號 |
| `addTime` | DateTime | ❌ | 新增時間 |
| `updateUserId` | string | ✅ | 修改員工編號 |
| `updateTime` | DateTime | ✅ | 修改時間 |
| `routerCategoryId` | string | ❌ | 路由類別主鍵 |
| `icon` | string | ✅ | Icon 名稱,用於裝飾前端網頁 |
| `sort` | int | ❌ | 排序 |

### 錯誤回應

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- 指定的 RouterId 不存在於資料庫中

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

---

## 驗證資料

### 1. 格式驗證
- **位置**: ASP.NET Core Model Binding (自動執行)
- **驗證內容**:
  - routerId: 從路由參數自動綁定,必填

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 50-62)

#### 檢查點: RouterId 存在性檢查
```
位置: Line 52-57
查詢 Auth_Router 表中是否存在指定的 RouterId
├─ 若不存在 (entity == null) → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料: {routerId}"
└─ 若存在 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 52
- **查詢**: 根據 RouterId 查詢單筆路由資料
  ```csharp
  await _context.Auth_Router.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RouterId == request.routerId)
  ```
- **說明**: 使用 AsNoTracking() 提升查詢效能,因為是唯讀操作

### 2. 資料轉換
- **位置**: Line 59
- **轉換邏輯**:
  ```csharp
  var response = _mapper.Map<GetRouterByIdResponse>(entity);
  ```
- **說明**: 使用 AutoMapper 將 Auth_Router 實體轉換為 GetRouterByIdResponse

### 3. 處理邏輯
```
接收請求 GET /Router/{routerId}
│
├─ 步驟 1: 查詢資料庫 (Line 52)
│  ├─ 使用 AsNoTracking() 查詢 Auth_Router
│  └─ 條件: RouterId = {routerId}
│
├─ 步驟 2: 檢查查詢結果 (Line 54-57)
│  ├─ 若 entity == null
│  │  └─ 回傳錯誤 4001 (查無此資料)
│  │
│  └─ 若 entity != null → 繼續
│
├─ 步驟 3: 資料轉換 (Line 59)
│  └─ 使用 AutoMapper 轉換 entity → response
│
└─ 步驟 4: 回傳成功回應 (Line 61)
   └─ 200 OK (Return Code: 2000)
      ├─ Message: "Success"
      └─ Data: GetRouterByIdResponse 物件
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Router` | SELECT | 路由主檔 - 根據 RouterId 查詢單筆資料 |

### 操作類型

#### 查詢操作 (SELECT)

- **位置**: Line 52
- **方法**: `AsNoTracking().SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var entity = await _context.Auth_Router.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RouterId == request.routerId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) [RouterId], [RouterName], [DynamicParams], [IsActive],
                [AddUserId], [AddTime], [UpdateUserId], [UpdateTime],
                [RouterCategoryId], [Icon], [Sort]
  FROM [dbo].[Auth_Router] WITH (NOLOCK)
  WHERE [RouterId] = @routerId
  ```
- **說明**:
  - 使用 `SingleOrDefaultAsync()` 確保只回傳一筆或 null
  - 使用 `AsNoTracking()` 提升查詢效能 (不追蹤實體變更)
  - TOP(2) 用於檢測是否有重複資料 (理論上不應該發生,因為 RouterId 是主鍵)

### Auth_Router 資料表結構

| 欄位名稱 | 資料型別 | 允許NULL | 說明 |
|---------|---------|---------|------|
| `RouterId` | string(50) | ❌ | 主鍵 - 路由識別碼 |
| `RouterName` | string(30) | ❌ | 路由名稱 |
| `DynamicParams` | string(100) | ✅ | 動態參數 |
| `IsActive` | string(1) | ❌ | 是否啟用 (Y/N) |
| `AddUserId` | string | ❌ | 新增員工編號 |
| `AddTime` | DateTime | ❌ | 新增時間 |
| `UpdateUserId` | string | ✅ | 修改員工編號 |
| `UpdateTime` | DateTime | ✅ | 修改時間 |
| `RouterCategoryId` | string(50) | ❌ | 路由類別主鍵 (FK) |
| `Icon` | string(15) | ✅ | Icon 名稱 |
| `Sort` | int | ❌ | 排序 |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 GET /Router/{routerId} 請求
│
├─ ASP.NET Core Model Binding
│  └─ 從路由參數綁定 routerId
│
├─ 查詢資料庫 (Handler.Handle)
│  │
│  ├─ 步驟 1: 查詢 Auth_Router (Line 52)
│  │  ├─ 使用 AsNoTracking() 提升效能
│  │  ├─ WHERE RouterId = @routerId
│  │  └─ 取得 entity (可能為 null)
│  │
│  ├─ 步驟 2: 檢查查詢結果 (Line 54-57)
│  │  │
│  │  ├─ entity == null
│  │  │  └─ 回傳 400 Bad Request (Return Code: 4001)
│  │  │     ├─ Message: "查無此資料: {routerId}"
│  │  │     └─ Data: null
│  │  │
│  │  └─ entity != null → 繼續
│  │
│  ├─ 步驟 3: 資料轉換 (Line 59)
│  │  └─ AutoMapper: Auth_Router → GetRouterByIdResponse
│  │
│  └─ 步驟 4: 回傳成功回應 (Line 61)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "Success"
│        └─ Data: GetRouterByIdResponse 物件
│           ├─ routerId
│           ├─ routerName
│           ├─ dynamicParams
│           ├─ isActive
│           ├─ addUserId
│           ├─ addTime
│           ├─ updateUserId (可能為 null)
│           ├─ updateTime (可能為 null)
│           ├─ routerCategoryId
│           ├─ icon (可能為 null)
│           └─ sort
```

---

## 關鍵業務規則

### 1. 唯讀查詢優化
- 使用 `AsNoTracking()` 提升查詢效能
- 因為是 GET 操作,不需要追蹤實體變更
- 減少記憶體使用和 EF Core 追蹤開銷

### 2. 單筆資料查詢
- 使用 `SingleOrDefaultAsync()` 確保只回傳一筆資料
- 若找到多筆資料會拋出例外 (理論上不應發生,因為 RouterId 是主鍵)
- 若找不到資料則回傳 null

### 3. 完整資料回傳
- 回傳所有欄位,包含審計欄位 (AddUserId, AddTime, UpdateUserId, UpdateTime)
- 前端可以根據需要顯示或隱藏特定欄位
- 提供完整的資料追蹤資訊

### 4. 資料轉換
- 使用 AutoMapper 進行物件轉換
- 確保回應模型與實體模型解耦
- 避免直接暴露資料庫實體結構

### 5. 錯誤處理
- 查無資料時回傳明確的錯誤訊息
- 包含查詢的 RouterId,方便前端除錯
- 使用標準的 Return Code 4001 (查無此資料)

### 6. RESTful 設計
- 使用 HTTP GET 方法進行查詢
- 路由參數直接指定資源 ID
- 回傳 200 OK 表示成功,資料在 data 欄位中

---

## 使用場景

### 1. 前端路由編輯
- 使用者點擊編輯按鈕時,先呼叫此 API 取得完整資料
- 將回傳的資料填入編輯表單
- 使用者修改後呼叫 UpdateRouterById API 更新

### 2. 路由詳情查看
- 使用者查看路由詳細資訊
- 顯示所有欄位,包含建立/修改追蹤資訊
- 供管理員檢視路由設定

### 3. 資料驗證
- 在執行其他操作前,先確認路由是否存在
- 檢查路由的啟用狀態 (IsActive)
- 確認路由的分類和排序

---

## 請求範例

### 請求範例 (來自 Endpoint.cs Line 13)

```
GET /Router/GetRouterById/SetUpInternalIP
Authorization: Bearer {jwt_token}
```

或簡化路由:

```
GET /Router/SetUpInternalIP
Authorization: Bearer {jwt_token}
```

### 回應範例

**成功回應:** (來自 Examples.cs Line 17-32)
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": {
    "routerCategoryId": "SetUp",
    "routerId": "SetUpInternalIP",
    "routerName": "行內IP設定",
    "isActive": "Y",
    "addUserId": "admin",
    "addTime": "2024-11-17T10:30:00",
    "updateUserId": "admin",
    "updateTime": "2024-11-17T15:45:00",
    "icon": "pi pi-user",
    "sort": 99,
    "dynamicParams": null
  },
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此資料:** (來自 Examples.cs Line 6-9)
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
| Endpoint.cs | `Modules/Auth/Router/GetRouterById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Router/GetRouterById/Models.cs` | 回應模型定義 |
| Examples.cs | `Modules/Auth/Router/GetRouterById/Examples.cs` | Swagger 範例定義 |
| Auth_Router.cs | `Infrastructures/Data/Entities/Auth_Router.cs` | 資料庫實體定義 |
