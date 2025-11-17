# GetRouterCategoriesByQueryString API - 取得路由類別清單 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/RouterCategory` |
| **HTTP 方法** | GET |
| **功能** | 取得路由類別清單 - 支援 RouterCategoryName 模糊搜尋及 IsActive 篩選 |
| **位置** | `Modules/Auth/RouterCategory/GetRouterCategoriesByQueryString/Endpoint.cs` |

---

## Request 定義

### 查詢參數 (Query String Parameters)

```csharp
// 位置: Modules/Auth/RouterCategory/GetRouterCategoriesByQueryString/Models.cs (Line 3-17)
public class GetRouterCategoriesByQueryStringRequest
{
    [Display(Name = "路由類別名稱")]
    public string? RouterCategoryName { get; set; }  // 模糊搜尋,支援 Contains

    [Display(Name = "是否啟用")]
    [RegularExpression("^[YN]$", ErrorMessage = "IsActive 只能輸入 Y 或 N")]
    public string? IsActive { get; set; }  // Y/N
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 驗證規則 | 說明 |
|------|------|------|----------|------|
| `RouterCategoryName` | string | ❌ | 無 | 路由類別名稱,支援模糊搜尋 (Contains) |
| `IsActive` | string | ❌ | ^[YN]$ | 是否啟用,只能是 Y 或 N |

### 範例查詢字串

```
?RouterCategoryName=測試
?IsActive=Y
?RouterCategoryName=權限&IsActive=Y
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "查詢成功",
  "data": [
    {
      "routerCategoryId": "RouterCategory",
      "routerCategoryName": "路由類別",
      "isActive": "Y",
      "addUserId": "admin",
      "addTime": "2024-01-01T12:00:00",
      "updateUserId": "admin",
      "updateTime": "2024-01-01T12:00:00",
      "icon": "pi pi-user",
      "sort": 99
    }
  ],
  "traceId": "{traceId}"
}
```

### 回應欄位說明

```csharp
// 位置: Modules/Auth/RouterCategory/GetRouterCategoriesByQueryString/Models.cs (Line 19-65)
public class GetRouterCategoriesByQueryStringResponse
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

#### 格式驗證失敗 (400 Bad Request - Return Code: 4000)
- IsActive 格式不符合規則 (非 Y 或 N)

```json
{
  "returnCode": 4000,
  "returnMessage": "格式驗證失敗",
  "data": {
    "IsActive": ["IsActive 只能輸入 Y 或 N"]
  }
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
  - RouterCategoryName: 選填
  - IsActive: 選填, 若提供則必須符合正則表達式 `^[YN]$` (只能是 Y 或 N)

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 48-78)
- **驗證內容**: 無特別的商業邏輯驗證,直接進行查詢

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 55-75
- **查詢邏輯**:
  ```csharp
  var filterEnties = await _context
      .Auth_RouterCategory.AsNoTracking()
      .Where(x =>
          String.IsNullOrEmpty(getRouterCategoryRequest.RouterCategoryName)
          || x.RouterCategoryName.Contains(getRouterCategoryRequest.RouterCategoryName)
      )
      .Where(x => String.IsNullOrEmpty(getRouterCategoryRequest.IsActive) || x.IsActive == getRouterCategoryRequest.IsActive)
      .Select(x => new GetRouterCategoriesByQueryStringResponse()
      {
          UpdateUserId = x.UpdateUserId,
          UpdateTime = x.UpdateTime,
          AddTime = x.AddTime,
          AddUserId = x.AddUserId,
          IsActive = x.IsActive,
          RouterCategoryId = x.RouterCategoryId,
          RouterCategoryName = x.RouterCategoryName,
          Icon = x.Icon,
          Sort = x.Sort,
      })
      .OrderBy(x => x.Sort)
      .ToListAsync();
  ```

### 2. 篩選條件

#### 條件 1: RouterCategoryName 模糊搜尋
```
位置: Line 57-60
若 RouterCategoryName 為空或 null → 不篩選,回傳所有資料
若 RouterCategoryName 有值 → 使用 Contains 進行模糊搜尋
  └─ 例如: "權限" 可以搜尋到 "權限類別"、"使用者權限" 等
```

#### 條件 2: IsActive 精確比對
```
位置: Line 61
若 IsActive 為空或 null → 不篩選,回傳所有狀態資料
若 IsActive 有值 → 精確比對 (只能是 'Y' 或 'N')
  └─ 例如: "Y" 只會搜尋到 IsActive = 'Y' 的資料
```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 取得查詢參數 (Line 53)
│  └─ RouterCategoryName 和 IsActive (皆為選填)
│
├─ 步驟 2: 建立查詢 (Line 55-75)
│  ├─ 使用 AsNoTracking() 唯讀查詢
│  ├─ 套用 RouterCategoryName 模糊搜尋篩選 (Contains)
│  ├─ 套用 IsActive 精確比對篩選
│  ├─ 使用 Select 投影所需欄位
│  └─ 使用 OrderBy(Sort) 排序
│
├─ 步驟 3: 執行查詢 (Line 75)
│  └─ ToListAsync() 取得結果清單
│
└─ 步驟 4: 回傳成功訊息 (Line 77)
   └─ Return Code: 2000, Data: List<GetRouterCategoriesByQueryStringResponse>
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_RouterCategory` | SELECT | 路由類別主檔 - 查詢清單資料 |

### 操作類型

#### 查詢操作 (SELECT)

**查詢: 取得 RouterCategory 清單**
- **位置**: Line 55-75
- **方法**: `AsNoTracking()` + `Where()` + `Select()` + `OrderBy()` + `ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var filterEnties = await _context
      .Auth_RouterCategory.AsNoTracking()
      .Where(x =>
          String.IsNullOrEmpty(getRouterCategoryRequest.RouterCategoryName)
          || x.RouterCategoryName.Contains(getRouterCategoryRequest.RouterCategoryName)
      )
      .Where(x => String.IsNullOrEmpty(getRouterCategoryRequest.IsActive) || x.IsActive == getRouterCategoryRequest.IsActive)
      .Select(x => new GetRouterCategoriesByQueryStringResponse() { ... })
      .OrderBy(x => x.Sort)
      .ToListAsync();
  ```
- **等效 SQL** (範例: RouterCategoryName="權限", IsActive="Y"):
  ```sql
  SELECT [RouterCategoryId], [RouterCategoryName], [IsActive],
         [AddUserId], [AddTime], [UpdateUserId], [UpdateTime],
         [Icon], [Sort]
  FROM [dbo].[Auth_RouterCategory]
  WHERE [RouterCategoryName] LIKE '%權限%'
    AND [IsActive] = 'Y'
  ORDER BY [Sort]
  ```

- **等效 SQL** (無篩選條件):
  ```sql
  SELECT [RouterCategoryId], [RouterCategoryName], [IsActive],
         [AddUserId], [AddTime], [UpdateUserId], [UpdateTime],
         [Icon], [Sort]
  FROM [dbo].[Auth_RouterCategory]
  ORDER BY [Sort]
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
用戶發送 GET /RouterCategory?RouterCategoryName=測試&IsActive=Y 請求
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證 IsActive 格式 (若有提供)
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯處理 (Handler.Handle)
│  │
│  ├─ 取得查詢參數 (Line 53)
│  │  ├─ RouterCategoryName (選填)
│  │  └─ IsActive (選填)
│  │
│  ├─ 建立資料庫查詢 (Line 55-75)
│  │  ├─ 使用 AsNoTracking() 唯讀查詢
│  │  ├─ 套用 RouterCategoryName 模糊搜尋 (Contains)
│  │  │  └─ 若為空則不篩選
│  │  ├─ 套用 IsActive 精確比對
│  │  │  └─ 若為空則不篩選
│  │  ├─ Select 投影所需欄位
│  │  ├─ OrderBy(Sort) 排序
│  │  └─ ToListAsync() 執行查詢
│  │
│  └─ 回傳成功回應 (Line 77)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "查詢成功"
│        └─ Data: List<GetRouterCategoriesByQueryStringResponse>
│           (若無符合資料則回傳空陣列 [])
```

---

## 關鍵業務規則

### 1. RouterCategoryName 模糊搜尋
- 支援 `Contains` 模糊搜尋
- 搜尋邏輯:
  - 若 RouterCategoryName 為 null 或空字串 → 不套用此篩選條件
  - 若 RouterCategoryName 有值 → 搜尋包含此字串的所有資料
- 範例:
  - 搜尋 "權限" 可找到 "權限類別"、"使用者權限"、"系統權限" 等
  - 搜尋 "類別" 可找到 "權限類別"、"路由類別" 等

### 2. IsActive 篩選
- 支援精確比對
- 篩選邏輯:
  - 若 IsActive 為 null 或空字串 → 不套用此篩選條件,回傳所有狀態
  - 若 IsActive = "Y" → 只回傳啟用的資料
  - 若 IsActive = "N" → 只回傳停用的資料
- 常見使用場景:
  - 前端下拉選單只需要顯示啟用的類別 → 傳入 IsActive=Y
  - 管理後台需要顯示所有類別 → 不傳入 IsActive

### 3. 排序規則
- 固定使用 `OrderBy(x => x.Sort)` 排序
- 依照 Sort 欄位由小到大排序
- 確保前端顯示順序的一致性

### 4. AsNoTracking 查詢優化
- 使用 `AsNoTracking()` 進行資料庫查詢
- 優點:
  - 不追蹤實體變更,減少記憶體使用
  - 提升查詢效能
  - 適合唯讀查詢場景

### 5. 使用 Select 投影
- 使用 `Select` 直接投影為回應物件
- 優點:
  - 只查詢需要的欄位
  - 減少資料傳輸量
  - 避免 AutoMapper 額外開銷

### 6. 空結果處理
- 若沒有符合篩選條件的資料,回傳空陣列 `[]`
- 不會回傳 404 錯誤
- 讓前端可以統一處理回應格式

### 7. 參數皆為選填
- RouterCategoryName 和 IsActive 皆為選填參數
- 若兩個參數都不提供,則回傳所有資料 (依 Sort 排序)
- 靈活支援各種查詢需求

---

## 請求範例

### 請求範例 1: 無篩選條件

```http
GET /RouterCategory
```

回傳所有路由類別,依 Sort 排序

### 請求範例 2: 模糊搜尋

```http
GET /RouterCategory?RouterCategoryName=測試
```

回傳 RouterCategoryName 包含 "測試" 的資料

### 請求範例 3: 狀態篩選

```http
GET /RouterCategory?IsActive=Y
```

回傳所有啟用的路由類別

### 請求範例 4: 組合篩選

```http
GET /RouterCategory?RouterCategoryName=權限&IsActive=Y
```

回傳 RouterCategoryName 包含 "權限" 且狀態為啟用的資料

### 回應範例

**成功回應 (來自 Examples.cs Line 9-26):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "查詢成功",
  "data": [
    {
      "routerCategoryId": "RouterCategory",
      "routerCategoryName": "路由類別",
      "isActive": "Y",
      "addUserId": "admin",
      "addTime": "2024-01-01T12:00:00",
      "updateUserId": "admin",
      "updateTime": "2024-01-01T12:00:00",
      "icon": "pi pi-user",
      "sort": 99
    }
  ],
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**成功回應 - 無符合資料:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "查詢成功",
  "data": [],
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/RouterCategory/GetRouterCategoriesByQueryString/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/RouterCategory/GetRouterCategoriesByQueryString/Models.cs` | 請求及回應模型定義 |
| Examples.cs | `Modules/Auth/RouterCategory/GetRouterCategoriesByQueryString/Examples.cs` | Swagger 範例定義 |
| Auth_RouterCategory.cs | `Infrastructures/Data/Entities/Auth_RouterCategory.cs` | 資料庫實體定義 |
