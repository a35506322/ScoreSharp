# GetRouterCategoryById API - 取得單筆路由類別 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/RouterCategory/{routerCategoryId}` |
| **HTTP 方法** | GET |
| **功能** | 取得單筆路由類別資料 - 用於查詢特定路由分類的詳細資訊 |
| **位置** | `Modules/Auth/RouterCategory/GetRouterCategoryById/Endpoint.cs` |

---

## Request 定義

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `routerCategoryId` | string | ✅ | 路由類別主鍵 |

### 範例路由

```
/RouterCategory/GetRouterCategoryById/1
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "查詢成功",
  "data": {
    "routerCategoryId": "RouterCategory",
    "routerCategoryName": "路由類別",
    "isActive": "Y",
    "addUserId": "ADMIN",
    "addTime": "2024-01-01T12:00:00",
    "updateUserId": "ADMIN",
    "updateTime": "2024-01-01T12:00:00",
    "icon": "pi pi-user",
    "sort": 99
  },
  "traceId": "{traceId}"
}
```

### 回應欄位說明

```csharp
// 位置: Modules/Auth/RouterCategory/GetRouterCategoryById/Models.cs (Line 3-49)
public class GetRouterCategoryByIdResponse
{
    public string RouterCategoryId { get; set; }  // 路由類別主鍵
    public string RouterCategoryName { get; set; }  // 路由類別名稱
    public string IsActive { get; set; }  // 是否啟用 (Y/N)
    public string AddUserId { get; set; }  // 新增員工
    public DateTime AddTime { get; set; }  // 新增時間
    public string? UpdateUserId { get; set; }  // 修改員工
    public DateTime? UpdateTime { get; set; }  // 修改時間
    public string? Icon { get; set; }  // Icon 名稱
    public int Sort { get; set; }  // 排序
}
```

| 欄位 | 型別 | 允許NULL | 說明 |
|------|------|---------|------|
| `routerCategoryId` | string | ❌ | 路由類別主鍵,英數字,前端顯示網址 |
| `routerCategoryName` | string | ❌ | 路由類別名稱,中文,前端顯示SideBar類別 |
| `isActive` | string | ❌ | 是否啟用 (Y/N) |
| `addUserId` | string | ❌ | 新增員工編號 |
| `addTime` | DateTime | ❌ | 新增時間 |
| `updateUserId` | string | ✅ | 修改員工編號 |
| `updateTime` | DateTime | ✅ | 修改時間 |
| `icon` | string | ✅ | Icon 名稱,用於裝飾前端網頁 |
| `sort` | int | ❌ | 排序 |

### 錯誤回應

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- RouterCategoryId 不存在於資料庫中

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
- **位置**: `Handle()` 方法 (Endpoint.cs Line 50-64)

#### 檢查點: RouterCategoryId 存在性檢查
```
位置: Line 52-59
查詢 Auth_RouterCategory 表
├─ 條件: RouterCategoryId = {request.routerCategoryId}
├─ 若查無資料 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料: {RouterCategoryId}"
└─ 若有資料 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 52-54
- **查詢**: 取得單筆 RouterCategory 資料
  ```csharp
  var entity = await _context
      .Auth_RouterCategory.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RouterCategoryId == request.routerCategoryId);
  ```

### 2. 資料轉換
- **位置**: Line 61
- **轉換方法**: AutoMapper
- **轉換邏輯**:
  ```csharp
  var response = _mapper.Map<GetRouterCategoryByIdResponse>(entity);
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 查詢資料庫 (Line 52-54)
│  └─ 使用 AsNoTracking() 進行唯讀查詢
│     (AsNoTracking 表示不追蹤實體變更,提升查詢效能)
│
├─ 步驟 2: 檢查資料是否存在 (Line 56-59)
│  ├─ 查無資料 → 回傳錯誤 4001
│  └─ 有資料 → 繼續
│
├─ 步驟 3: 使用 AutoMapper 轉換資料 (Line 61)
│  └─ 將 Auth_RouterCategory 實體轉換為 GetRouterCategoryByIdResponse
│
└─ 步驟 4: 回傳成功訊息 (Line 63)
   └─ Return Code: 2000, Data: GetRouterCategoryByIdResponse
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_RouterCategory` | SELECT | 路由類別主檔 - 查詢單筆資料 |

### 操作類型

#### 查詢操作 (SELECT)

**查詢: 取得單筆 RouterCategory**
- **位置**: Line 52-54
- **方法**: `AsNoTracking()` + `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var entity = await _context
      .Auth_RouterCategory.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RouterCategoryId == request.routerCategoryId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_RouterCategory]
  WHERE [RouterCategoryId] = @RouterCategoryId
  ```

### Auth_RouterCategory 資料表結構

| 欄位名稱 | 資料型別 | 允許NULL | 說明 |
|---------|---------|---------|------|
| `RouterCategoryId` | string | ❌ | 主鍵 - 路由類別識別碼 |
| `RouterCategoryName` | string | ❌ | 路由類別名稱 |
| `IsActive` | string(1) | ❌ | 是否啟用 (Y/N) |
| `AddUserId` | string | ❌ | 新增員工編號 |
| `AddTime` | DateTime | ❌ | 新增時間 |
| `UpdateUserId` | string | ✅ | 修改員工編號 |
| `UpdateTime` | DateTime | ✅ | 修改時間 |
| `Icon` | string | ✅ | Icon 名稱 |
| `Sort` | int | ❌ | 排序 |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 GET /RouterCategory/{routerCategoryId} 請求
│
├─ 商業邏輯處理 (Handler.Handle)
│  │
│  ├─ 查詢資料庫 (Line 52-54)
│  │  ├─ 使用 AsNoTracking() 唯讀查詢
│  │  └─ SingleOrDefaultAsync()
│  │
│  ├─ 檢查資料是否存在 (Line 56-59)
│  │  ├─ 查無資料 → 400 Bad Request (Return Code: 4001)
│  │  │  └─ Message: "查無此資料: {RouterCategoryId}"
│  │  └─ 有資料 → 繼續
│  │
│  ├─ 使用 AutoMapper 轉換資料 (Line 61)
│  │  └─ Auth_RouterCategory → GetRouterCategoryByIdResponse
│  │
│  └─ 回傳成功回應 (Line 63)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "查詢成功"
│        └─ Data: GetRouterCategoryByIdResponse
```

---

## 關鍵業務規則

### 1. AsNoTracking 查詢優化
- 此 API 使用 `AsNoTracking()` 進行資料庫查詢
- 優點:
  - 不追蹤實體變更,減少記憶體使用
  - 提升查詢效能
  - 適合唯讀查詢場景
- 注意事項:
  - 查詢後的實體無法直接用於 EF Core 更新操作
  - 適合用於 GET 操作

### 2. 使用 AutoMapper 進行資料轉換
- 使用 AutoMapper 將資料庫實體轉換為回應模型
- 優點:
  - 簡化資料轉換程式碼
  - 維護物件間的映射關係
  - 減少手動賦值錯誤
- 確保 AutoMapper 配置正確映射所有欄位

### 3. 資料完整性
- 回應包含完整的審計追蹤資訊:
  - AddUserId: 建立者
  - AddTime: 建立時間
  - UpdateUserId: 最後修改者 (可能為 null)
  - UpdateTime: 最後修改時間 (可能為 null)

### 4. 資料不存在處理
- 若 RouterCategoryId 不存在,立即回傳 NotFound 錯誤
- 提供清晰的錯誤訊息,包含查詢的 RouterCategoryId
- 幫助前端快速識別問題

### 5. IsActive 狀態回傳
- 回傳資料包含 IsActive 狀態
- 前端可根據此狀態決定是否顯示或啟用該路由類別
- 'Y' 表示啟用, 'N' 表示停用

---

## 請求範例

### 請求範例

```http
GET /RouterCategory/RouterCategory
```

### 回應範例

**成功回應 (來自 Examples.cs Line 17-30):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "查詢成功",
  "data": {
    "routerCategoryId": "RouterCategory",
    "routerCategoryName": "路由類別",
    "isActive": "Y",
    "addUserId": "ADMIN",
    "addTime": "2024-01-01T12:00:00",
    "updateUserId": "ADMIN",
    "updateTime": "2024-01-01T12:00:00",
    "icon": "pi pi-user",
    "sort": 99
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
| Endpoint.cs | `Modules/Auth/RouterCategory/GetRouterCategoryById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/RouterCategory/GetRouterCategoryById/Models.cs` | 回應模型定義 |
| Examples.cs | `Modules/Auth/RouterCategory/GetRouterCategoryById/Examples.cs` | Swagger 範例定義 |
| Auth_RouterCategory.cs | `Infrastructures/Data/Entities/Auth_RouterCategory.cs` | 資料庫實體定義 |
