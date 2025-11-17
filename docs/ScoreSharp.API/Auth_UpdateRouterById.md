# UpdateRouterById API - 更新路由 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Router/{routerId}` |
| **HTTP 方法** | PUT |
| **功能** | 更新單筆路由資料 - 根據 RouterId 更新指定路由的設定 |
| **位置** | `Modules/Auth/Router/UpdateRouterById/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token (從 JWT 中取得 UserId) |

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `routerId` | string | ✅ | 路由主鍵,要更新的路由識別碼 |

### 請求體 (Body)

```csharp
// 位置: Modules/Auth/Router/UpdateRouterById/Models.cs (Line 3-57)
public class UpdateRouterByIdRequest
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
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }  // Y/N

    [Display(Name = "icon")]
    [MaxLength(15)]
    public string? Icon { get; set; }  // 用於裝飾前端網頁

    [Display(Name = "路由類別(英文)")]
    [Required]
    public string RouterCategoryId { get; set; }  // 關聯Auth_RouterCategory

    [Display(Name = "排序")]
    [Required]
    [Range(1, 99)]
    public int Sort { get; set; }  // 排序
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 最大長度 | 驗證規則 | 說明 |
|------|------|------|----------|----------|------|
| `routerId` (路由) | string | ✅ | - | 無 | 路由參數,要更新的路由識別碼 |
| `RouterId` (Body) | string | ✅ | 50 | 無 | 請求體中的路由主鍵,必須與路由參數一致 |
| `RouterName` | string | ✅ | 30 | 無 | 路由名稱,中文,前端顯示SideBar頁面名稱 |
| `DynamicParams` | string | ❌ | 100 | 無 | 動態參數,給前端串接參數使用 |
| `IsActive` | string | ✅ | - | [YN] | 是否啟用,只能是 Y 或 N |
| `Icon` | string | ❌ | 15 | 無 | Icon 名稱,用於裝飾前端網頁 |
| `RouterCategoryId` | string | ✅ | - | 無 | 路由類別主鍵,關聯 Auth_RouterCategory |
| `Sort` | int | ✅ | - | 1-99 | 排序,範圍 1 到 99 |

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "更新成功: SetUpBillDay",
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

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- 指定的 RouterId 不存在於資料庫中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料: SetUpBill",
  "data": null
}
```

#### 路由與Req比對錯誤 (400 Bad Request - Return Code: 4003)
- 路由參數的 routerId 與請求體的 RouterId 不一致

```json
{
  "returnCode": 4003,
  "returnMessage": "路由與Req比對錯誤",
  "data": null
}
```

#### 前端傳入關聯資料有誤 (400 Bad Request - Return Code: 4003)
- RouterCategoryId 不存在於 Auth_RouterCategory 表中
- 或該 RouterCategoryId 的 IsActive 不是 'Y'

```json
{
  "returnCode": 4003,
  "returnMessage": "前端傳入關聯資料有誤,欄位:路由類別Id,值:SetUpBill",
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
  - RouterCategoryId: 必填
  - Icon: 選填, 最大長度 15 字元
  - Sort: 必填, 範圍 1-99

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 52-77)

#### 檢查點 1: 路由參數與請求體一致性檢查
```
位置: Line 56-57
檢查路由參數 routerId 與請求體 RouterId 是否一致
├─ 若不一致 → 拋出路由與Req比對錯誤 (Return Code: 4003)
│  └─ 訊息: "路由與Req比對錯誤"
└─ 若一致 → 繼續執行
```

#### 檢查點 2: RouterId 存在性檢查
```
位置: Line 54, Line 59-60
查詢 Auth_Router 表中是否存在指定的 RouterId
├─ 若不存在 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料: {routerId}"
└─ 若存在 → 繼續執行
```

#### 檢查點 3: RouterCategoryId 關聯性檢查
```
位置: Line 64-68
查詢 Auth_RouterCategory 表
├─ 條件: RouterCategoryId = {dto.RouterCategoryId} AND IsActive = 'Y'
├─ 若查無資料 → 拋出前端傳入關聯資料有誤錯誤 (Return Code: 4003)
│  └─ 訊息: "前端傳入關聯資料有誤,欄位:路由類別Id,值:{RouterCategoryId}"
└─ 若有資料 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 54, Line 64-66
- **查詢 1**: 檢查 RouterId 是否存在 (使用 AsNoTracking 提升效能)
  ```csharp
  await _context.Auth_Router.AsNoTracking().SingleOrDefaultAsync(x => x.RouterId == request.routerId)
  ```
- **查詢 2**: 驗證 RouterCategoryId 是否有效
  ```csharp
  await _context.Auth_RouterCategory.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RouterCategoryId == dto.RouterCategoryId && x.IsActive == "Y")
  ```

### 2. 資料轉換
- **位置**: Line 70-72
- **轉換邏輯**:
  ```csharp
  var entity = _mapper.Map<Auth_Router>(dto);  // 使用 AutoMapper 轉換
  entity.UpdateTime = DateTime.Now;             // 設定更新時間
  entity.UpdateUserId = _jwthelper.UserId;      // 從 JWT Token 取得當前使用者
  ```

### 3. 處理邏輯
```
接收請求 PUT /Router/{routerId}
│
├─ 步驟 1: 檢查路由參數與請求體一致性 (Line 56-57)
│  ├─ 不一致 → 回傳錯誤 4003 (路由與Req比對錯誤)
│  └─ 一致 → 繼續
│
├─ 步驟 2: 檢查 RouterId 是否存在 (Line 54, 59-60)
│  ├─ 不存在 → 回傳錯誤 4001 (查無此資料)
│  └─ 存在 → 繼續
│
├─ 步驟 3: 驗證 RouterCategoryId 有效性 (Line 64-68)
│  ├─ 無效或未啟用 → 回傳錯誤 4003 (前端傳入關聯資料有誤)
│  └─ 有效 → 繼續
│
├─ 步驟 4: 資料轉換 (Line 70-72)
│  ├─ 使用 AutoMapper 將 Request 轉換為 Entity
│  ├─ 設定 UpdateTime (DateTime.Now)
│  └─ 設定 UpdateUserId (從 JWT Token)
│
├─ 步驟 5: 更新資料庫 (Line 74)
│  ├─ 呼叫 UpdateRouterById() 執行 Dapper SQL 更新
│  │
│  ├─ 成功 → 繼續
│  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│
└─ 步驟 6: 回傳成功回應 (Line 76)
   └─ 200 OK (Return Code: 2000)
      ├─ Message: "更新成功: {routerId}"
      └─ Data: routerId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Router` | SELECT, UPDATE | 路由主檔 - 查詢資料是否存在及更新資料 |
| `Auth_RouterCategory` | SELECT | 路由類別主檔 - 驗證關聯性 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢 1: 檢查 RouterId 是否存在**
- **位置**: Line 54
- **方法**: `AsNoTracking().SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_Router.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RouterId == request.routerId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Router] WITH (NOLOCK)
  WHERE [RouterId] = @routerId
  ```
- **說明**: 使用 AsNoTracking() 提升查詢效能,因為只需要檢查資料是否存在

**查詢 2: 驗證 RouterCategoryId**
- **位置**: Line 64-66
- **方法**: `AsNoTracking().SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var routerCategory = await _context.Auth_RouterCategory.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RouterCategoryId == dto.RouterCategoryId && x.IsActive == "Y");
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_RouterCategory] WITH (NOLOCK)
  WHERE [RouterCategoryId] = @RouterCategoryId
    AND [IsActive] = 'Y'
  ```

#### 2. 更新操作 (UPDATE)

- **位置**: Line 79-96
- **方法**: `Dapper ExecuteAsync()`
- **Dapper 程式碼**:
  ```csharp
  public async Task<bool> UpdateRouterById(Auth_Router entity)
  {
      var sql = @"UPDATE [dbo].[Auth_Router]
                  SET   [DynamicParams] = @DynamicParams
                        ,[Icon] = @Icon
                        ,[UpdateTime] = @UpdateTime
                        ,[UpdateUserId] = @UpdateUserId
                        ,[RouterName] = @RouterName
                        ,[RouterCategoryId] = @RouterCategoryId
                        ,[IsActive] = @IsActive
                        ,[Sort] = @Sort
                  WHERE [RouterId] = @RouterId ";

      using var conn = _dapperContext.CreateScoreSharpConnection();
      var result = await conn.ExecuteAsync(sql, entity);
      return result > 0;
  }
  ```
- **SQL 語句** (Line 82-91):
  ```sql
  UPDATE [dbo].[Auth_Router]
  SET [DynamicParams] = @DynamicParams,
      [Icon] = @Icon,
      [UpdateTime] = @UpdateTime,
      [UpdateUserId] = @UpdateUserId,
      [RouterName] = @RouterName,
      [RouterCategoryId] = @RouterCategoryId,
      [IsActive] = @IsActive,
      [Sort] = @Sort
  WHERE [RouterId] = @RouterId
  ```
- **說明**:
  - 使用 Dapper 執行原生 SQL,效能較 EF Core 更好
  - 只更新可修改的欄位,不更新 RouterId, AddUserId, AddTime
  - 回傳 true 表示更新成功 (影響行數 > 0)

### Auth_Router 資料表結構

| 欄位名稱 | 資料型別 | 允許NULL | 是否更新 | 說明 |
|---------|---------|---------|---------|------|
| `RouterId` | string(50) | ❌ | ❌ | 主鍵 - 路由識別碼 (不可修改) |
| `RouterName` | string(30) | ❌ | ✅ | 路由名稱 |
| `DynamicParams` | string(100) | ✅ | ✅ | 動態參數 |
| `IsActive` | string(1) | ❌ | ✅ | 是否啟用 (Y/N) |
| `AddUserId` | string | ❌ | ❌ | 新增員工編號 (不可修改) |
| `AddTime` | DateTime | ❌ | ❌ | 新增時間 (不可修改) |
| `UpdateUserId` | string | ✅ | ✅ | 修改員工編號 |
| `UpdateTime` | DateTime | ✅ | ✅ | 修改時間 |
| `RouterCategoryId` | string(50) | ❌ | ✅ | 路由類別主鍵 (FK) |
| `Icon` | string(15) | ✅ | ✅ | Icon 名稱 |
| `Sort` | int | ❌ | ✅ | 排序 |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 PUT /Router/{routerId} 請求
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
│  ├─ 檢查路由參數與請求體一致性 (Line 56-57)
│  │  ├─ routerId != request.RouterId → 400 Bad Request (Return Code: 4003)
│  │  └─ 一致 → 繼續
│  │
│  ├─ 檢查 RouterId 是否存在 (Line 54, 59-60)
│  │  ├─ 不存在 → 400 Bad Request (Return Code: 4001)
│  │  └─ 存在 → 繼續
│  │
│  ├─ 檢查 RouterCategoryId 有效性 (Line 64-68)
│  │  ├─ 查無資料或未啟用 → 400 Bad Request (Return Code: 4003)
│  │  └─ 有效 → 繼續
│  │
│  ├─ 資料轉換 (Line 70-72)
│  │  ├─ AutoMapper 轉換 Request → Entity
│  │  ├─ 設定 UpdateTime (DateTime.Now)
│  │  └─ 設定 UpdateUserId (從 JWT Token)
│  │
│  ├─ 執行更新 (Line 74, 79-96)
│  │  ├─ 使用 Dapper 執行 SQL UPDATE
│  │  ├─ WHERE RouterId = @RouterId
│  │  │
│  │  ├─ 成功 (影響行數 > 0) → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 回傳成功回應 (Line 76)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "更新成功: {routerId}"
│        └─ Data: routerId
```

---

## 關鍵業務規則

### 1. 路由參數與請求體一致性
- 路由參數的 routerId 必須與請求體的 RouterId 完全一致
- 這是 RESTful API 的最佳實踐,防止資料不一致
- 檢查發生在所有其他驗證之前

### 2. RouterId 不可修改
- RouterId 是主鍵,一旦建立就不能修改
- 更新時只能更新其他欄位,RouterId 用於定位要更新的記錄
- SQL UPDATE 語句中 RouterId 只出現在 WHERE 條件中

### 3. RouterCategoryId 關聯性
- RouterCategoryId 必須存在於 Auth_RouterCategory 表中
- 且該 RouterCategory 的 IsActive 必須為 'Y' (啟用狀態)
- 確保路由分類的有效性和一致性

### 4. 審計追蹤
- 更新時自動記錄 UpdateUserId (從 JWT Token 取得當前使用者)
- 更新時自動記錄 UpdateTime (系統當前時間)
- AddUserId 和 AddTime 保持不變,維護完整的建立與修改追蹤記錄

### 5. 使用 Dapper 執行更新
- 使用 Dapper 而非 EF Core 執行 UPDATE 操作
- Dapper 效能更好,適合簡單的更新操作
- 明確指定要更新的欄位,避免意外更新

### 6. AsNoTracking 查詢優化
- 驗證查詢使用 AsNoTracking() 提升效能
- 因為只需要檢查資料是否存在,不需要追蹤實體變更
- 減少記憶體使用和 EF Core 追蹤開銷

### 7. IsActive 狀態控制
- IsActive 可以在更新時修改
- 'Y' 表示啟用,前端會顯示該路由
- 'N' 表示停用,前端不會顯示該路由
- 常用於臨時停用路由而不刪除資料

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 8-19)

```json
PUT /Router/SetUpBillDay
Content-Type: application/json
Authorization: Bearer {jwt_token}

{
  "routerId": "SetUpBillDay",
  "routerName": "帳單日設定",
  "dynamicParams": null,
  "routerCategoryId": "SetUp",
  "isActive": "Y",
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
  "returnMessage": "更新成功: SetUpBillDay",
  "data": "SetUpBillDay",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此資料:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此資料: SetUpBill",
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

**失敗回應 - 路由類別不存在:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4003,
  "returnMessage": "前端傳入關聯資料有誤,欄位:路由類別Id,值:SetUpBill",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Router/UpdateRouterById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Router/UpdateRouterById/Models.cs` | 請求模型定義 |
| Examples.cs | `Modules/Auth/Router/UpdateRouterById/Examples.cs` | Swagger 範例定義 |
| Auth_Router.cs | `Infrastructures/Data/Entities/Auth_Router.cs` | 資料庫實體定義 |
