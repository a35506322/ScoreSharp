# InsertRouterCategory API - 新增路由類別 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/RouterCategory` |
| **HTTP 方法** | POST |
| **功能** | 新增單筆路由類別資料 - 用於建立系統前端路由分類設定 |
| **位置** | `Modules/Auth/RouterCategory/InsertRouterCategory/Endpoint.cs` |

---

## Request 定義

### 請求體 (Body)

```csharp
// 位置: Modules/Auth/RouterCategory/InsertRouterCategory/Models.cs (Line 3-40)
public class InsertRouterCategoryRequest
{
    [Display(Name = "路由類別(英文)")]
    [Required]
    public string RouterCategoryId { get; set; }  // 英數字,前端顯示網址,如:TodoCategory

    [Display(Name = "路由類別(中文)")]
    [Required]
    public string RouterCategoryName { get; set; }  // 中文,前端顯示SideBar類別

    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsActive { get; set; }  // Y/N

    [Display(Name = "Icon")]
    public string? Icon { get; set; }  // 用於裝飾前端網頁

    [Display(Name = "排序")]
    [Required]
    [Range(1, 99)]
    public int Sort { get; set; }  // 排序
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 驗證規則 | 說明 |
|------|------|------|----------|------|
| `RouterCategoryId` | string | ✅ | 無 | 路由類別主鍵,英數字,前端顯示網址 |
| `RouterCategoryName` | string | ✅ | 無 | 路由類別名稱,中文,前端顯示SideBar類別 |
| `IsActive` | string | ✅ | [YN] | 是否啟用,只能是 Y 或 N |
| `Icon` | string | ❌ | 無 | Icon 名稱,用於裝飾前端網頁 |
| `Sort` | int | ✅ | 1-99 | 排序,範圍 1 到 99 |

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "新增成功: Auth",
  "data": "Auth",
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
    "RouterCategoryId": ["RouterCategoryId 為必填欄位"],
    "IsActive": ["IsActive 必須符合正則表達式 [YN]"]
  }
}
```

#### 資料已存在 (400 Bad Request - Return Code: 4002)
- RouterCategoryId 已存在於資料庫中

```json
{
  "returnCode": 4002,
  "returnMessage": "資料已存在: Auth",
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
  - RouterCategoryId: 必填
  - RouterCategoryName: 必填
  - IsActive: 必填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)
  - Icon: 選填
  - Sort: 必填, 範圍 1-99

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 44-66)

#### 檢查點 1: RouterCategoryId 唯一性檢查
```
位置: Line 48-51
檢查 Auth_RouterCategory 表中是否已存在相同的 RouterCategoryId
├─ 若存在 → 拋出 DataAlreadyExists 錯誤 (Return Code: 4002)
│  └─ 訊息: "資料已存在: {RouterCategoryId}"
└─ 若不存在 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 48
- **查詢**: 檢查 RouterCategoryId 是否已存在
  ```csharp
  await _context.Auth_RouterCategory.SingleOrDefaultAsync(x => x.RouterCategoryId == dto.RouterCategoryId)
  ```

### 2. 資料轉換
- **位置**: Line 53-60
- **轉換邏輯**:
  ```csharp
  Auth_RouterCategory auth_RouterCategory = new Auth_RouterCategory()
  {
      RouterCategoryId = dto.RouterCategoryId,
      RouterCategoryName = dto.RouterCategoryName,
      IsActive = dto.IsActive,
      Icon = dto.Icon,
      Sort = dto.Sort,
  }
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查 RouterCategoryId 是否已存在 (Line 48-51)
│  ├─ 已存在 → 回傳錯誤 4002
│  └─ 不存在 → 繼續
│
├─ 步驟 2: 建立 Auth_RouterCategory 實體 (Line 53-60)
│  └─ 複製請求資料
│
├─ 步驟 3: 新增至資料庫 (Line 62-63)
│  └─ 呼叫 AddAsync() 和 SaveChangesAsync()
│
└─ 步驟 4: 回傳成功訊息 (Line 65)
   └─ Return Code: 2000, Data: RouterCategoryId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_RouterCategory` | SELECT, INSERT | 路由類別主檔 - 查詢唯一性檢查及新增資料 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢: 檢查 RouterCategoryId 唯一性**
- **位置**: Line 48
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_RouterCategory.SingleOrDefaultAsync(x => x.RouterCategoryId == dto.RouterCategoryId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_RouterCategory]
  WHERE [RouterCategoryId] = @RouterCategoryId
  ```

#### 2. 新增操作 (INSERT)

- **位置**: Line 62-63
- **方法**: `AddAsync()` + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  await _context.AddAsync(auth_RouterCategory);
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  INSERT INTO [dbo].[Auth_RouterCategory]
      ([RouterCategoryId], [RouterCategoryName], [IsActive], [Icon], [Sort])
  VALUES
      (@RouterCategoryId, @RouterCategoryName, @IsActive, @Icon, @Sort)
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
用戶發送 POST /RouterCategory 請求
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證必填欄位
│  ├─ 驗證 IsActive 格式 [YN]
│  ├─ 驗證 Sort 範圍 (1-99)
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯驗證 (Handler.Handle)
│  │
│  ├─ 檢查 RouterCategoryId 唯一性 (Line 48-51)
│  │  ├─ 已存在 → 400 Bad Request (Return Code: 4002)
│  │  └─ 不存在 → 繼續
│  │
│  ├─ 建立 Auth_RouterCategory 實體 (Line 53-60)
│  │  └─ 設定基本資料 (從 Request)
│  │
│  ├─ 新增至資料庫 (Line 62-63)
│  │  ├─ AddAsync(auth_RouterCategory)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 回傳成功回應 (Line 65)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "新增成功: {RouterCategoryId}"
│        └─ Data: RouterCategoryId
```

---

## 關鍵業務規則

### 1. RouterCategoryId 唯一性
- RouterCategoryId 是路由類別的主鍵,必須在系統中唯一
- 新增前必須檢查是否已存在
- 若已存在則拋出錯誤,不允許重複新增

### 2. IsActive 狀態控制
- IsActive 只能是 'Y' 或 'N'
- 'Y' 表示啟用,前端會顯示該路由類別
- 'N' 表示停用,前端不會顯示該路由類別

### 3. 排序規則
- Sort 欄位用於控制前端顯示順序
- 範圍限制在 1-99 之間
- 數字越小,在前端顯示越前面

### 4. Icon 裝飾
- 選填欄位,用於前端 UI 裝飾
- 通常使用 Icon 庫的類別名稱 (如 PrimeIcons: "pi pi-user")

### 5. 無審計追蹤欄位
- 注意: 此 API 在新增時並未設定 AddUserId 和 AddTime
- 這些欄位可能透過資料庫預設值或觸發器自動填入

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 8-18)

```json
POST /RouterCategory
Content-Type: application/json

{
  "routerCategoryId": "Auth",
  "routerCategoryName": "權限類別",
  "isActive": "Y",
  "icon": "pi pi-user",
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
  "returnMessage": "新增成功: Auth",
  "data": "Auth",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 資料已存在:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4002,
  "returnMessage": "資料已存在: Auth",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/RouterCategory/InsertRouterCategory/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/RouterCategory/InsertRouterCategory/Models.cs` | 請求模型定義 |
| Examples.cs | `Modules/Auth/RouterCategory/InsertRouterCategory/Examples.cs` | Swagger 範例定義 |
| Auth_RouterCategory.cs | `Infrastructures/Data/Entities/Auth_RouterCategory.cs` | 資料庫實體定義 |
