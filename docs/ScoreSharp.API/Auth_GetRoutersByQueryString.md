# GetRoutersByQueryString API - 查詢路由列表 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Router` |
| **HTTP 方法** | GET |
| **功能** | 根據查詢條件取得路由列表 - 支援按啟用狀態和路由類別篩選 |
| **位置** | `Modules/Auth/Router/GetRoutersByQueryString/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token (用於驗證使用者身份) |

### 查詢參數 (Query String)

```csharp
// 位置: Modules/Auth/Router/GetRoutersByQueryString/Models.cs (Line 3-18)
public class GetRoutersByQueryStringRequest
{
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; } = null!;  // Y/N

    [Display(Name = "路由類別PK")]
    [MaxLength(50)]
    public string? RouterCategoryId { get; set; }  // 關聯Auth_RouterCategory
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 最大長度 | 驗證規則 | 說明 |
|------|------|------|----------|----------|------|
| `IsActive` | string | ❌ | - | [YN] | 是否啟用,只能是 Y 或 N,若未提供則不篩選 |
| `RouterCategoryId` | string | ❌ | 50 | 無 | 路由類別主鍵,若未提供則不篩選 |

### 請求範例

```
GET /Router?IsActive=Y&RouterCategoryId=SetUp
Authorization: Bearer {jwt_token}
```

或查詢所有啟用的路由:

```
GET /Router?IsActive=Y
Authorization: Bearer {jwt_token}
```

或查詢所有路由 (不加任何參數):

```
GET /Router
Authorization: Bearer {jwt_token}
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
    }
  ],
  "traceId": "{traceId}"
}
```

### 回應資料結構

```csharp
// 位置: Modules/Auth/Router/GetRoutersByQueryString/Models.cs (Line 20-76)
public class GetRoutersByQueryStringResponse
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

### 空結果回應

若查詢條件沒有符合的資料,回傳空陣列:

```json
{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": [],
  "traceId": "{traceId}"
}
```

---

## 驗證資料

### 1. 格式驗證
- **位置**: ASP.NET Core Model Validation (自動執行)
- **驗證內容**:
  - IsActive: 選填, 若提供則必須符合正則表達式 `[YN]` (只能是 Y 或 N)
  - RouterCategoryId: 選填, 最大長度 50 字元

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 50-66)
- **驗證邏輯**: 此 API 為查詢操作,無額外商業邏輯驗證,所有查詢參數都是選填的

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 54-61
- **查詢邏輯**: 動態組合 WHERE 條件
  ```csharp
  var query = await _context
      .Auth_Router.AsNoTracking()
      .Where(x => String.IsNullOrEmpty(getRouterRequest.IsActive) || x.IsActive == getRouterRequest.IsActive)
      .Where(x => String.IsNullOrEmpty(getRouterRequest.RouterCategoryId) || x.RouterCategoryId == getRouterRequest.RouterCategoryId)
      .OrderBy(x => x.Sort)
      .ToListAsync();
  ```

### 2. 查詢條件邏輯

#### IsActive 篩選
```
若 IsActive 參數為 null 或空字串
├─ 不加入 WHERE 條件 (查詢所有啟用狀態的資料)
└─ 若有值 (Y 或 N)
   └─ 加入條件: WHERE IsActive = @IsActive
```

#### RouterCategoryId 篩選
```
若 RouterCategoryId 參數為 null 或空字串
├─ 不加入 WHERE 條件 (查詢所有路由類別的資料)
└─ 若有值
   └─ 加入條件: WHERE RouterCategoryId = @RouterCategoryId
```

#### 排序規則
- 固定使用 `ORDER BY Sort` 升序排序
- Sort 值越小,在結果中越前面

### 3. 資料轉換
- **位置**: Line 63
- **轉換邏輯**:
  ```csharp
  var result = _mapper.Map<List<GetRoutersByQueryStringResponse>>(query);
  ```
- **說明**: 使用 AutoMapper 將 Auth_Router 實體列表轉換為 GetRoutersByQueryStringResponse 列表

### 4. 處理邏輯
```
接收請求 GET /Router?IsActive=Y&RouterCategoryId=SetUp
│
├─ 步驟 1: 組合查詢條件 (Line 54-61)
│  ├─ AsNoTracking() 提升查詢效能
│  │
│  ├─ 條件 1: IsActive 篩選
│  │  ├─ 若參數為空 → 不篩選
│  │  └─ 若參數有值 → WHERE IsActive = @IsActive
│  │
│  ├─ 條件 2: RouterCategoryId 篩選
│  │  ├─ 若參數為空 → 不篩選
│  │  └─ 若參數有值 → WHERE RouterCategoryId = @RouterCategoryId
│  │
│  ├─ 排序: ORDER BY Sort ASC
│  │
│  └─ 執行查詢: ToListAsync()
│
├─ 步驟 2: 資料轉換 (Line 63)
│  └─ AutoMapper: List<Auth_Router> → List<GetRoutersByQueryStringResponse>
│
└─ 步驟 3: 回傳成功回應 (Line 65)
   └─ 200 OK (Return Code: 2000)
      ├─ Message: "Success"
      └─ Data: List<GetRoutersByQueryStringResponse>
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Router` | SELECT | 路由主檔 - 根據查詢條件查詢多筆資料 |

### 操作類型

#### 查詢操作 (SELECT)

- **位置**: Line 54-61
- **方法**: `AsNoTracking().Where().OrderBy().ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var query = await _context
      .Auth_Router.AsNoTracking()
      .Where(x => String.IsNullOrEmpty(getRouterRequest.IsActive) || x.IsActive == getRouterRequest.IsActive)
      .Where(x => String.IsNullOrEmpty(getRouterRequest.RouterCategoryId) || x.RouterCategoryId == getRouterRequest.RouterCategoryId)
      .OrderBy(x => x.Sort)
      .ToListAsync();
  ```

- **等效 SQL** (當兩個參數都有值時):
  ```sql
  SELECT [RouterId], [RouterName], [DynamicParams], [IsActive],
         [AddUserId], [AddTime], [UpdateUserId], [UpdateTime],
         [RouterCategoryId], [Icon], [Sort]
  FROM [dbo].[Auth_Router] WITH (NOLOCK)
  WHERE [IsActive] = @IsActive
    AND [RouterCategoryId] = @RouterCategoryId
  ORDER BY [Sort] ASC
  ```

- **等效 SQL** (只有 IsActive 參數):
  ```sql
  SELECT [RouterId], [RouterName], [DynamicParams], [IsActive],
         [AddUserId], [AddTime], [UpdateUserId], [UpdateTime],
         [RouterCategoryId], [Icon], [Sort]
  FROM [dbo].[Auth_Router] WITH (NOLOCK)
  WHERE [IsActive] = @IsActive
  ORDER BY [Sort] ASC
  ```

- **等效 SQL** (沒有任何參數):
  ```sql
  SELECT [RouterId], [RouterName], [DynamicParams], [IsActive],
         [AddUserId], [AddTime], [UpdateUserId], [UpdateTime],
         [RouterCategoryId], [Icon], [Sort]
  FROM [dbo].[Auth_Router] WITH (NOLOCK)
  ORDER BY [Sort] ASC
  ```

- **說明**:
  - 使用 `AsNoTracking()` 提升查詢效能 (不追蹤實體變更)
  - 使用動態 WHERE 條件,根據參數是否為空決定是否加入篩選
  - 使用 `String.IsNullOrEmpty()` 判斷參數是否有值
  - 固定按 Sort 欄位升序排序

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
用戶發送 GET /Router?IsActive=Y&RouterCategoryId=SetUp 請求
│
├─ ASP.NET Core Model Binding & Validation
│  ├─ 綁定查詢參數到 GetRoutersByQueryStringRequest
│  ├─ 驗證 IsActive 格式 (若有值則必須是 Y 或 N)
│  ├─ 驗證 RouterCategoryId 長度 (若有值則不超過 50 字元)
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 查詢資料庫 (Handler.Handle)
│  │
│  ├─ 步驟 1: 組合查詢 (Line 54-61)
│  │  │
│  │  ├─ AsNoTracking() - 提升查詢效能
│  │  │
│  │  ├─ 動態 WHERE 條件 1: IsActive
│  │  │  ├─ 若參數為空 → 不加入 WHERE 條件
│  │  │  └─ 若參數有值 → WHERE IsActive = @IsActive
│  │  │
│  │  ├─ 動態 WHERE 條件 2: RouterCategoryId
│  │  │  ├─ 若參數為空 → 不加入 WHERE 條件
│  │  │  └─ 若參數有值 → WHERE RouterCategoryId = @RouterCategoryId
│  │  │
│  │  ├─ 排序: ORDER BY Sort ASC
│  │  │
│  │  └─ 執行查詢: ToListAsync()
│  │     └─ 取得 List<Auth_Router>
│  │
│  ├─ 步驟 2: 資料轉換 (Line 63)
│  │  └─ AutoMapper: List<Auth_Router> → List<GetRoutersByQueryStringResponse>
│  │
│  └─ 步驟 3: 回傳成功回應 (Line 65)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "Success"
│        └─ Data: List<GetRoutersByQueryStringResponse>
│           ├─ 若有符合的資料 → 回傳路由列表
│           └─ 若無符合的資料 → 回傳空陣列 []
```

---

## 關鍵業務規則

### 1. 動態查詢條件
- 所有查詢參數都是選填的
- 若參數為 null 或空字串,則不加入該條件
- 允許組合多個條件進行篩選
- 支援查詢所有資料 (不提供任何參數)

### 2. 唯讀查詢優化
- 使用 `AsNoTracking()` 提升查詢效能
- 因為是 GET 操作,不需要追蹤實體變更
- 減少記憶體使用和 EF Core 追蹤開銷

### 3. 固定排序
- 查詢結果固定按 Sort 欄位升序排序
- 確保前端顯示順序的一致性
- Sort 值越小,在列表中越前面

### 4. 完整資料回傳
- 回傳所有欄位,包含審計欄位
- 前端可以根據需要顯示或隱藏特定欄位
- 提供完整的資料追蹤資訊

### 5. 空結果處理
- 查無資料時回傳空陣列,不回傳 null
- Return Code 仍為 2000 (成功)
- 這是 RESTful API 的最佳實踐

### 6. 資料轉換
- 使用 AutoMapper 進行物件轉換
- 確保回應模型與實體模型解耦
- 避免直接暴露資料庫實體結構

---

## 使用場景

### 1. 前端選單渲染
- 查詢所有啟用的路由: `?IsActive=Y`
- 按 Sort 排序後渲染前端選單
- 提供使用者導航功能

### 2. 路由管理列表
- 管理員查看所有路由: 不提供任何參數
- 查看特定類別的路由: `?RouterCategoryId=SetUp`
- 提供路由管理介面

### 3. 類別篩選
- 按路由類別分組顯示: `?RouterCategoryId=SetUp`
- 按啟用狀態篩選: `?IsActive=Y`
- 組合條件: `?IsActive=Y&RouterCategoryId=SetUp`

### 4. 前端下拉選單
- 提供路由選擇下拉選單的資料來源
- 只顯示啟用的路由: `?IsActive=Y`
- 按 Sort 排序確保順序一致

---

## 查詢範例

### 範例 1: 查詢所有啟用的路由

**請求:**
```
GET /Router?IsActive=Y
Authorization: Bearer {jwt_token}
```

**等效 SQL:**
```sql
SELECT * FROM [dbo].[Auth_Router]
WHERE [IsActive] = 'Y'
ORDER BY [Sort] ASC
```

### 範例 2: 查詢特定類別的路由

**請求:**
```
GET /Router?RouterCategoryId=SetUp
Authorization: Bearer {jwt_token}
```

**等效 SQL:**
```sql
SELECT * FROM [dbo].[Auth_Router]
WHERE [RouterCategoryId] = 'SetUp'
ORDER BY [Sort] ASC
```

### 範例 3: 組合條件查詢

**請求:**
```
GET /Router?IsActive=Y&RouterCategoryId=SetUp
Authorization: Bearer {jwt_token}
```

**等效 SQL:**
```sql
SELECT * FROM [dbo].[Auth_Router]
WHERE [IsActive] = 'Y'
  AND [RouterCategoryId] = 'SetUp'
ORDER BY [Sort] ASC
```

### 範例 4: 查詢所有路由 (來自 Endpoint.cs Line 13)

**請求:**
```
GET /Router
Authorization: Bearer {jwt_token}
```

**等效 SQL:**
```sql
SELECT * FROM [dbo].[Auth_Router]
ORDER BY [Sort] ASC
```

---

## 請求範例

### 回應範例

**成功回應:** (來自 Examples.cs Line 9-26)
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "Success",
  "data": [
    {
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
    }
  ],
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**空結果回應:**
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

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Router/GetRoutersByQueryString/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Router/GetRoutersByQueryString/Models.cs` | 請求和回應模型定義 |
| Examples.cs | `Modules/Auth/Router/GetRoutersByQueryString/Examples.cs` | Swagger 範例定義 |
| Auth_Router.cs | `Infrastructures/Data/Entities/Auth_Router.cs` | 資料庫實體定義 |
