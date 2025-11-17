# InsertRouter API - 新增路由 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Router` |
| **HTTP 方法** | POST |
| **功能** | 新增單筆路由資料 - 用於建立系統前端路由設定 |
| **位置** | `Modules/Auth/Router/InsertRouter/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token (從 JWT 中取得 UserId) |

### 請求體 (Body)

```csharp
// 位置: Modules/Auth/Router/InsertRouter/Models.cs (Line 3-58)
public class InsertRouterRequest
{
    [Display(Name = "路由PK")]
    [MaxLength(50)]
    [Required]
    public string RouterId { get; set; }  // 英數字,前端顯示不在網址,如:Todo

    [Display(Name = "路由名稱")]
    [MaxLength(30)]
    [Required]
    public string RouterName { get; set; }  // 中文,前端顯示SideBar頁面名稱

    [Display(Name = "動態參數")]
    [MaxLength(100)]
    public string? DynamicParams { get; set; }  // 給前端串接參數使用,如:/Todo/1 或 /Todo?params=

    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsActive { get; set; }  // Y/N

    [Display(Name = "路由類別PK")]
    [MaxLength(50)]
    [Required]
    public string RouterCategoryId { get; set; }  // 關聯Auth_RouterCategory

    [Display(Name = "icon")]
    [MaxLength(15)]
    public string? Icon { get; set; }  // 用於裝飾前端網頁

    [Display(Name = "排序")]
    [Required]
    [Range(1, 99)]
    public int Sort { get; set; }  // 排序
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 最大長度 | 驗證規則 | 說明 |
|------|------|------|----------|----------|------|
| `RouterId` | string | ✅ | 50 | 無 | 路由主鍵,英數字,前端顯示不在網址 |
| `RouterName` | string | ✅ | 30 | 無 | 路由名稱,中文,前端顯示SideBar頁面名稱 |
| `DynamicParams` | string | ❌ | 100 | 無 | 動態參數,給前端串接參數使用 |
| `IsActive` | string | ✅ | - | [YN] | 是否啟用,只能是 Y 或 N |
| `RouterCategoryId` | string | ✅ | 50 | 無 | 路由類別主鍵,關聯 Auth_RouterCategory |
| `Icon` | string | ❌ | 15 | 無 | Icon 名稱,用於裝飾前端網頁 |
| `Sort` | int | ✅ | - | 1-99 | 排序,範圍 1 到 99 |

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "新增成功: SetUpBillDay",
  "data": "SetUpBillDay",
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 格式驗證失敗 (400 Bad Request - Return Code: 4000)
- 請求格式不符合定義
- 缺少必填欄位
- 欄位長度超過限制
- 欄位格式不符合驗證規則

```json
{
  "returnCode": 4000,
  "returnMessage": "格式驗證失敗",
  "data": {
    "RouterId": ["RouterId 為必填欄位"],
    "IsActive": ["IsActive 必須符合正則表達式 [YN]"]
  }
}
```

#### 資料已存在 (400 Bad Request - Return Code: 4002)
- RouterId 已存在於資料庫中

```json
{
  "returnCode": 4002,
  "returnMessage": "資料已存在: SetUpBillDay",
  "data": null
}
```

#### 前端傳入關聯資料有誤 (400 Bad Request - Return Code: 4003)
- RouterCategoryId 不存在於 Auth_RouterCategory 表中
- 或該 RouterCategoryId 的 IsActive 不是 'Y'

```json
{
  "returnCode": 4003,
  "returnMessage": "前端傳入關聯資料有誤,欄位:路由類別Id,值:Action",
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
  - RouterId: 必填, 最大長度 50 字元
  - RouterName: 必填, 最大長度 30 字元
  - DynamicParams: 選填, 最大長度 100 字元
  - IsActive: 必填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)
  - RouterCategoryId: 必填, 最大長度 50 字元
  - Icon: 選填, 最大長度 15 字元
  - Sort: 必填, 範圍 1-99

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 47-79)

#### 檢查點 1: RouterId 唯一性檢查
```
位置: Line 51-54
檢查 Auth_Router 表中是否已存在相同的 RouterId
├─ 若存在 → 拋出 DataAlreadyExists 錯誤 (Return Code: 4002)
│  └─ 訊息: "資料已存在: {RouterId}"
└─ 若不存在 → 繼續執行
```

#### 檢查點 2: RouterCategoryId 關聯性檢查
```
位置: Line 56-60
查詢 Auth_RouterCategory 表
├─ 條件: RouterCategoryId = {dto.RouterCategoryId} AND IsActive = 'Y'
├─ 若查無資料 → 拋出前端傳入關聯資料有誤錯誤 (Return Code: 4003)
│  └─ 訊息: "前端傳入關聯資料有誤,欄位:路由類別Id,值:{RouterCategoryId}"
└─ 若有資料 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 51, Line 56-58
- **查詢 1**: 檢查 RouterId 是否已存在
  ```csharp
  await _context.Auth_Router.SingleOrDefaultAsync(x => x.RouterId == dto.RouterId)
  ```
- **查詢 2**: 驗證 RouterCategoryId 是否有效
  ```csharp
  await _context.Auth_RouterCategory.SingleOrDefaultAsync(x =>
      x.RouterCategoryId == dto.RouterCategoryId && x.IsActive == "Y")
  ```

### 2. 資料轉換
- **位置**: Line 62-73
- **轉換邏輯**:
  ```csharp
  Auth_Router auth_Router = new Auth_Router()
  {
      RouterId = dto.RouterId,
      RouterName = dto.RouterName,
      DynamicParams = dto.DynamicParams,
      IsActive = dto.IsActive,
      AddUserId = _jwthelper.UserId,  // 從 JWT Token 取得
      AddTime = DateTime.Now,          // 系統當前時間
      RouterCategoryId = routerCategory.RouterCategoryId,
      Icon = dto.Icon,
      Sort = dto.Sort,
  }
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查 RouterId 是否已存在 (Line 51-54)
│  ├─ 已存在 → 回傳錯誤 4002
│  └─ 不存在 → 繼續
│
├─ 步驟 2: 驗證 RouterCategoryId 有效性 (Line 56-60)
│  ├─ 無效或未啟用 → 回傳錯誤 4003
│  └─ 有效 → 繼續
│
├─ 步驟 3: 建立 Auth_Router 實體 (Line 62-73)
│  ├─ 複製請求資料
│  ├─ 設定 AddUserId (從 JWT Token)
│  └─ 設定 AddTime (當前時間)
│
├─ 步驟 4: 新增至資料庫 (Line 75-76)
│  └─ 呼叫 AddAsync() 和 SaveChangesAsync()
│
└─ 步驟 5: 回傳成功訊息 (Line 78)
   └─ Return Code: 2000, Data: RouterId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Router` | SELECT, INSERT | 路由主檔 - 查詢唯一性檢查及新增資料 |
| `Auth_RouterCategory` | SELECT | 路由類別主檔 - 驗證關聯性 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢 1: 檢查 RouterId 唯一性**
- **位置**: Line 51
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_Router.SingleOrDefaultAsync(x => x.RouterId == dto.RouterId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Router]
  WHERE [RouterId] = @RouterId
  ```

**查詢 2: 驗證 RouterCategoryId**
- **位置**: Line 56-58
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var routerCategory = await _context.Auth_RouterCategory.SingleOrDefaultAsync(x =>
      x.RouterCategoryId == dto.RouterCategoryId && x.IsActive == "Y");
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_RouterCategory]
  WHERE [RouterCategoryId] = @RouterCategoryId
    AND [IsActive] = 'Y'
  ```

#### 2. 新增操作 (INSERT)

- **位置**: Line 75-76
- **方法**: `AddAsync()` + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  await _context.AddAsync(auth_Router);
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  INSERT INTO [dbo].[Auth_Router]
      ([RouterId], [RouterName], [DynamicParams], [IsActive],
       [AddUserId], [AddTime], [RouterCategoryId], [Icon], [Sort])
  VALUES
      (@RouterId, @RouterName, @DynamicParams, @IsActive,
       @AddUserId, @AddTime, @RouterCategoryId, @Icon, @Sort)
  ```

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
用戶發送 POST /Router 請求
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證必填欄位
│  ├─ 驗證欄位長度
│  ├─ 驗證 IsActive 格式 [YN]
│  ├─ 驗證 Sort 範圍 (1-99)
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯驗證 (Handler.Handle)
│  │
│  ├─ 檢查 RouterId 唯一性 (Line 51-54)
│  │  ├─ 已存在 → 400 Bad Request (Return Code: 4002)
│  │  └─ 不存在 → 繼續
│  │
│  ├─ 檢查 RouterCategoryId 有效性 (Line 56-60)
│  │  ├─ 查無資料或未啟用 → 400 Bad Request (Return Code: 4003)
│  │  └─ 有效 → 繼續
│  │
│  ├─ 建立 Auth_Router 實體 (Line 62-73)
│  │  ├─ 設定基本資料 (從 Request)
│  │  ├─ 設定 AddUserId (從 JWT Token)
│  │  └─ 設定 AddTime (DateTime.Now)
│  │
│  ├─ 新增至資料庫 (Line 75-76)
│  │  ├─ AddAsync(auth_Router)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 回傳成功回應 (Line 78)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "新增成功: {RouterId}"
│        └─ Data: RouterId
```

---

## 關鍵業務規則

### 1. RouterId 唯一性
- RouterId 是路由的主鍵,必須在系統中唯一
- 新增前必須檢查是否已存在
- 若已存在則拋出錯誤,不允許重複新增

### 2. RouterCategoryId 關聯性
- RouterCategoryId 必須存在於 Auth_RouterCategory 表中
- 且該 RouterCategory 的 IsActive 必須為 'Y' (啟用狀態)
- 確保路由分類的有效性和一致性

### 3. IsActive 狀態控制
- IsActive 只能是 'Y' 或 'N'
- 'Y' 表示啟用,前端會顯示該路由
- 'N' 表示停用,前端不會顯示該路由

### 4. 排序規則
- Sort 欄位用於控制前端顯示順序
- 範圍限制在 1-99 之間
- 數字越小,在前端顯示越前面

### 5. 審計追蹤
- 新增時自動記錄 AddUserId (從 JWT Token 取得當前使用者)
- 新增時自動記錄 AddTime (系統當前時間)
- 確保每筆資料都有完整的建立追蹤記錄

### 6. DynamicParams 用途
- 選填欄位,用於前端路由參數配置
- 支援路徑參數格式: `/Todo/1`
- 支援查詢參數格式: `/Todo?params=`
- 給予前端靈活的路由配置能力

### 7. Icon 裝飾
- 選填欄位,用於前端 UI 裝飾
- 通常使用 Icon 庫的類別名稱 (如 PrimeIcons: "pi pi-user")
- 最大長度 15 字元

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 8-20)

```json
POST /Router
Content-Type: application/json
Authorization: Bearer {jwt_token}

{
  "routerId": "SetUpBillDay",
  "routerName": "帳單日設定",
  "dynamicParams": null,
  "isActive": "Y",
  "routerCategoryId": "SetUp",
  "icon": null,
  "sort": 99
}
```

### 回應範例

**成功回應:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "新增成功: SetUpBillDay",
  "data": "SetUpBillDay",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 資料已存在:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4002,
  "returnMessage": "資料已存在: SetUpBillDay",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 路由類別不存在:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4003,
  "returnMessage": "前端傳入關聯資料有誤,欄位:路由類別Id,值:Action",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Router/InsertRouter/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Router/InsertRouter/Models.cs` | 請求模型定義 |
| Examples.cs | `Modules/Auth/Router/InsertRouter/Examples.cs` | Swagger 範例定義 |
| Auth_Router.cs | `Infrastructures/Data/Entities/Auth_Router.cs` | 資料庫實體定義 |
