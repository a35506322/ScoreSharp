# UpdateRouterCategoryById API - 更新路由類別 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/RouterCategory/{routerCategoryId}` |
| **HTTP 方法** | PUT |
| **功能** | 更新單筆路由類別資料 - 用於修改系統前端路由分類設定 |
| **位置** | `Modules/Auth/RouterCategory/UpdateRouterCategoryById/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token (從 JWT 中取得 UserId) |

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `routerCategoryId` | string | ✅ | 路由類別主鍵 |

### 請求體 (Body)

```csharp
// 位置: Modules/Auth/RouterCategory/UpdateRouterCategoryById/Models.cs (Line 3-40)
public class UpdateRouterCategoryByIdRequest
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
| `RouterCategoryId` | string | ✅ | 無 | 路由類別主鍵,必須與路由參數一致 |
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
  "returnMessage": "更新成功: Auth",
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

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- RouterCategoryId 不存在於資料庫中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料: Auth",
  "data": null
}
```

#### 路由與Req比對錯誤 (400 Bad Request - Return Code: 4003)
- 路由參數中的 routerCategoryId 與請求體中的 RouterCategoryId 不一致

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
  - RouterCategoryId: 必填
  - RouterCategoryName: 必填
  - IsActive: 必填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)
  - Icon: 選填
  - Sort: 必填, 範圍 1-99

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 50-76)

#### 檢查點 1: 路由參數與請求體一致性檢查
```
位置: Line 52-53
檢查路由參數中的 RouterCategoryId 是否與請求體中的 RouterCategoryId 一致
├─ 若不一致 → 拋出路由與Req比對錯誤 (Return Code: 4003)
│  └─ 訊息: "路由與Req比對錯誤"
└─ 若一致 → 繼續執行
```

#### 檢查點 2: RouterCategoryId 存在性檢查
```
位置: Line 55-58
檢查 Auth_RouterCategory 表中是否存在該 RouterCategoryId
├─ 若不存在 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料: {RouterCategoryId}"
└─ 若存在 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 55
- **查詢**: 檢查 RouterCategoryId 是否存在
  ```csharp
  var single = await _context.Auth_RouterCategory.SingleOrDefaultAsync(x => x.RouterCategoryId == request.RouterCategoryId);
  ```

### 2. 資料轉換
- **位置**: Line 62-71
- **轉換邏輯**:
  ```csharp
  Auth_RouterCategory entity = new()
  {
      RouterCategoryId = dto.RouterCategoryId,
      RouterCategoryName = dto.RouterCategoryName,
      IsActive = dto.IsActive,
      Icon = dto.Icon,
      UpdateTime = DateTime.Now,          // 系統當前時間
      UpdateUserId = _jwtProfilerHelper.UserId,  // 從 JWT Token 取得
      Sort = dto.Sort,
  }
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查路由參數與請求體一致性 (Line 52-53)
│  ├─ 不一致 → 回傳錯誤 4003
│  └─ 一致 → 繼續
│
├─ 步驟 2: 檢查 RouterCategoryId 是否存在 (Line 55-58)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 3: 建立 Auth_RouterCategory 實體 (Line 62-71)
│  ├─ 複製請求資料
│  ├─ 設定 UpdateUserId (從 JWT Token)
│  └─ 設定 UpdateTime (當前時間)
│
├─ 步驟 4: 使用 Dapper 更新資料庫 (Line 73)
│  └─ 呼叫 UpdateRouterCategoryById()
│
└─ 步驟 5: 回傳成功訊息 (Line 75)
   └─ Return Code: 2000, Data: RouterCategoryId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_RouterCategory` | SELECT, UPDATE | 路由類別主檔 - 查詢存在性檢查及更新資料 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢: 檢查 RouterCategoryId 存在性**
- **位置**: Line 55
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_RouterCategory.SingleOrDefaultAsync(x => x.RouterCategoryId == request.RouterCategoryId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_RouterCategory]
  WHERE [RouterCategoryId] = @RouterCategoryId
  ```

#### 2. 更新操作 (UPDATE) - 使用 Dapper

- **位置**: Line 78-93
- **方法**: Dapper `ExecuteAsync()`
- **Dapper 程式碼**:
  ```csharp
  private async Task<bool> UpdateRouterCategoryById(Auth_RouterCategory entity)
  {
      var sql =
          @"UPDATE [dbo].[Auth_RouterCategory]
                 SET [RouterCategoryName] = @RouterCategoryName
                    ,[IsActive] = @IsActive
                    ,[UpdateUserId] = @UpdateUserId
                    ,[UpdateTime] = @UpdateTime
                    ,[Icon] = @Icon
                    ,[Sort] = @Sort
                  WHERE RouterCategoryId = @RouterCategoryId";

      using var conn = _dapperContext.CreateScoreSharpConnection();
      var result = await conn.ExecuteAsync(sql, entity);
      return result > 0;
  }
  ```
- **原生 SQL**:
  ```sql
  UPDATE [dbo].[Auth_RouterCategory]
     SET [RouterCategoryName] = @RouterCategoryName
        ,[IsActive] = @IsActive
        ,[UpdateUserId] = @UpdateUserId
        ,[UpdateTime] = @UpdateTime
        ,[Icon] = @Icon
        ,[Sort] = @Sort
    WHERE RouterCategoryId = @RouterCategoryId
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
用戶發送 PUT /RouterCategory/{routerCategoryId} 請求
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
│  ├─ 檢查路由參數與請求體一致性 (Line 52-53)
│  │  ├─ 不一致 → 400 Bad Request (Return Code: 4003)
│  │  └─ 一致 → 繼續
│  │
│  ├─ 檢查 RouterCategoryId 存在性 (Line 55-58)
│  │  ├─ 不存在 → 400 Bad Request (Return Code: 4001)
│  │  └─ 存在 → 繼續
│  │
│  ├─ 建立 Auth_RouterCategory 實體 (Line 62-71)
│  │  ├─ 設定基本資料 (從 Request)
│  │  ├─ 設定 UpdateUserId (從 JWT Token)
│  │  └─ 設定 UpdateTime (DateTime.Now)
│  │
│  ├─ 使用 Dapper 更新資料庫 (Line 73, Line 78-93)
│  │  ├─ 執行原生 SQL UPDATE
│  │  ├─ ExecuteAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 回傳成功回應 (Line 75)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "更新成功: {RouterCategoryId}"
│        └─ Data: RouterCategoryId
```

---

## 關鍵業務規則

### 1. 路由參數與請求體一致性
- 路由參數中的 routerCategoryId 必須與請求體中的 RouterCategoryId 完全一致
- 此檢查確保前端傳送的資料一致性,避免誤更新
- 若不一致則立即拋出錯誤,不執行任何資料庫操作

### 2. RouterCategoryId 存在性
- 更新前必須先確認該 RouterCategoryId 存在於資料庫中
- 若不存在則拋出 NotFound 錯誤
- 確保不會嘗試更新不存在的資料

### 3. 使用 Dapper 執行原生 SQL
- 此 API 使用 Dapper 而非 EF Core 進行更新操作
- 優點:
  - 更精確的 SQL 控制
  - 可能有較好的效能表現
  - 明確指定要更新的欄位
- 注意事項:
  - WHERE 條件只使用 RouterCategoryId
  - 不更新 AddUserId 和 AddTime (保留原始建立資訊)

### 4. IsActive 狀態控制
- IsActive 只能是 'Y' 或 'N'
- 'Y' 表示啟用,前端會顯示該路由類別
- 'N' 表示停用,前端不會顯示該路由類別

### 5. 排序規則
- Sort 欄位用於控制前端顯示順序
- 範圍限制在 1-99 之間
- 數字越小,在前端顯示越前面

### 6. 審計追蹤
- 更新時自動記錄 UpdateUserId (從 JWT Token 取得當前使用者)
- 更新時自動記錄 UpdateTime (系統當前時間)
- 確保每筆資料都有完整的修改追蹤記錄

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 8-17)

```json
PUT /RouterCategory/Auth
Content-Type: application/json
Authorization: Bearer {jwt_token}

{
  "routerCategoryId": "Auth",
  "routerCategoryName": "權限類別",
  "isActive": "Y",
  "icon": "pi pi-user",
  "sort": 9
}
```

### 回應範例

**成功回應:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "更新成功: Auth",
  "data": "Auth",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此資料:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此資料: Auth",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 路由與Req比對錯誤:**
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
| Endpoint.cs | `Modules/Auth/RouterCategory/UpdateRouterCategoryById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/RouterCategory/UpdateRouterCategoryById/Models.cs` | 請求模型定義 |
| Examples.cs | `Modules/Auth/RouterCategory/UpdateRouterCategoryById/Examples.cs` | Swagger 範例定義 |
| Auth_RouterCategory.cs | `Infrastructures/Data/Entities/Auth_RouterCategory.cs` | 資料庫實體定義 |
