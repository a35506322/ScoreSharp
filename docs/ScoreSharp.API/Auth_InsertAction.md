# InsertAction API - 新增操作 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Action` |
| **HTTP 方法** | POST |
| **功能** | 新增單筆操作資料 - 用於建立系統 API 操作設定 |
| **位置** | `Modules/Auth/Action/InsertAction/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token (從 JWT 中取得 UserId) |

### 請求體 (Body)

```csharp
// 位置: Modules/Auth/Action/InsertAction/Models.cs (Line 3-45)
public class InsertActionRequest
{
    [Display(Name = "API Action名稱(英文)")]
    [MaxLength(100)]
    [Required]
    public string ActionId { get; set; }  // 英數字，API Action 名稱

    [Display(Name = "API Action 中文")]
    [MaxLength(30)]
    [Required]
    public string ActionName { get; set; }  // 中文，前端顯示功能

    [Display(Name = "是否是通用資料")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsCommon { get; set; }  // Y/N，如果是Y 不檢查權限

    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }  // Y/N

    [Display(Name = "路由PK")]
    [MaxLength(50)]
    [Required]
    public string RouterId { get; set; }  // 關聯Auth_Router
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 最大長度 | 驗證規則 | 說明 |
|------|------|------|----------|----------|------|
| `ActionId` | string | ✅ | 100 | 無 | API Action 名稱,英數字 |
| `ActionName` | string | ✅ | 30 | 無 | API Action 中文名稱,前端顯示功能 |
| `IsCommon` | string | ✅ | - | [YN] | 是否是通用資料,Y 表示不檢查權限 |
| `IsActive` | string | ✅ | - | [YN] | 是否啟用,只能是 Y 或 N |
| `RouterId` | string | ✅ | 50 | 無 | 路由主鍵,關聯 Auth_Router |

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "新增成功: GetBillDayByQueryString",
  "data": "GetBillDayByQueryString",
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
    "ActionId": ["ActionId 為必填欄位"],
    "IsActive": ["IsActive 必須符合正則表達式 [YN]"]
  }
}
```

#### 資料已存在 (400 Bad Request - Return Code: 4002)
- ActionId 已存在於資料庫中

```json
{
  "returnCode": 4002,
  "returnMessage": "資料已存在: GetBillDayByQueryString",
  "data": null
}
```

#### 前端傳入關聯資料有誤 (400 Bad Request - Return Code: 4003)
- RouterId 不存在於 Auth_Router 表中
- 或該 RouterId 的 IsActive 不是 'Y'

```json
{
  "returnCode": 4003,
  "returnMessage": "前端傳入關聯資料有誤,欄位:路由Id,值:InsertAction",
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
  - ActionId: 必填, 最大長度 100 字元
  - ActionName: 必填, 最大長度 30 字元
  - IsCommon: 必填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)
  - IsActive: 必填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)
  - RouterId: 必填, 最大長度 50 字元

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 51-77)

#### 檢查點 1: ActionId 唯一性檢查
```
位置: Line 55-58
檢查 Auth_Action 表中是否已存在相同的 ActionId
├─ 若存在 → 拋出 DataAlreadyExists 錯誤 (Return Code: 4002)
│  └─ 訊息: "資料已存在: {ActionId}"
└─ 若不存在 → 繼續執行
```

#### 檢查點 2: RouterId 關聯性檢查
```
位置: Line 60-63
查詢 Auth_Router 表
├─ 條件: RouterId = {dto.RouterId} AND IsActive = 'Y'
├─ 若查無資料 → 拋出前端傳入關聯資料有誤錯誤 (Return Code: 4003)
│  └─ 訊息: "前端傳入關聯資料有誤,欄位:路由Id,值:{RouterId}"
└─ 若有資料 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 55, Line 60
- **查詢 1**: 檢查 ActionId 是否已存在
  ```csharp
  await _context.Auth_Action.SingleOrDefaultAsync(x => x.ActionId == dto.ActionId)
  ```
- **查詢 2**: 驗證 RouterId 是否有效
  ```csharp
  await _context.Auth_Router.SingleOrDefaultAsync(x =>
      x.RouterId == dto.RouterId && x.IsActive == "Y")
  ```

### 2. 資料轉換
- **位置**: Line 65-68
- **轉換邏輯**:
  ```csharp
  var entity = _mapper.Map<Auth_Action>(dto);
  entity.AddTime = DateTime.Now;          // 系統當前時間
  entity.AddUserId = _jwthelper.UserId;  // 從 JWT Token 取得
  entity.RouterId = router.RouterId;
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查 ActionId 是否已存在 (Line 55-58)
│  ├─ 已存在 → 回傳錯誤 4002
│  └─ 不存在 → 繼續
│
├─ 步驟 2: 驗證 RouterId 有效性 (Line 60-63)
│  ├─ 無效或未啟用 → 回傳錯誤 4003
│  └─ 有效 → 繼續
│
├─ 步驟 3: 建立 Auth_Action 實體 (Line 65-68)
│  ├─ 使用 AutoMapper 對應資料
│  ├─ 設定 AddUserId (從 JWT Token)
│  ├─ 設定 AddTime (當前時間)
│  └─ 設定 RouterId
│
├─ 步驟 4: 新增至資料庫 (Line 70-71)
│  └─ 呼叫 AddAsync() 和 SaveChangesAsync()
│
├─ 步驟 5: 清除快取 (Line 73-74)
│  └─ 移除 FusionCache 中的 Action 快取
│
└─ 步驟 6: 回傳成功訊息 (Line 76)
   └─ Return Code: 2000, Data: ActionId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Action` | SELECT, INSERT | 操作主檔 - 查詢唯一性檢查及新增資料 |
| `Auth_Router` | SELECT | 路由主檔 - 驗證關聯性 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢 1: 檢查 ActionId 唯一性**
- **位置**: Line 55
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_Action.SingleOrDefaultAsync(x => x.ActionId == dto.ActionId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Action]
  WHERE [ActionId] = @ActionId
  ```

**查詢 2: 驗證 RouterId**
- **位置**: Line 60
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var router = await _context.Auth_Router.SingleOrDefaultAsync(x =>
      x.RouterId == dto.RouterId && x.IsActive == "Y");
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Router]
  WHERE [RouterId] = @RouterId
    AND [IsActive] = 'Y'
  ```

#### 2. 新增操作 (INSERT)

- **位置**: Line 70-71
- **方法**: `AddAsync()` + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  await _context.AddAsync(entity);
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  INSERT INTO [dbo].[Auth_Action]
      ([ActionId], [ActionName], [IsCommon], [IsActive],
       [AddUserId], [AddTime], [RouterId])
  VALUES
      (@ActionId, @ActionName, @IsCommon, @IsActive,
       @AddUserId, @AddTime, @RouterId)
  ```

#### 3. 快取操作 (Cache Remove)

- **位置**: Line 73-74
- **方法**: `FusionCache.RemoveAsync()`
- **程式碼**:
  ```csharp
  var cacheKey = $"{SecurityConstants.PolicyRedisKey.Action}:{dto.ActionId}";
  await _fusionCache.RemoveAsync(cacheKey);
  ```
- **說明**: 移除 FusionCache 中的 Action 快取,確保快取一致性

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
用戶發送 POST /Action 請求
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證必填欄位
│  ├─ 驗證欄位長度
│  ├─ 驗證 IsCommon 格式 [YN]
│  ├─ 驗證 IsActive 格式 [YN]
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯驗證 (Handler.Handle)
│  │
│  ├─ 檢查 ActionId 唯一性 (Line 55-58)
│  │  ├─ 已存在 → 400 Bad Request (Return Code: 4002)
│  │  └─ 不存在 → 繼續
│  │
│  ├─ 檢查 RouterId 有效性 (Line 60-63)
│  │  ├─ 查無資料或未啟用 → 400 Bad Request (Return Code: 4003)
│  │  └─ 有效 → 繼續
│  │
│  ├─ 建立 Auth_Action 實體 (Line 65-68)
│  │  ├─ 設定基本資料 (使用 AutoMapper)
│  │  ├─ 設定 AddUserId (從 JWT Token)
│  │  ├─ 設定 AddTime (DateTime.Now)
│  │  └─ 設定 RouterId
│  │
│  ├─ 新增至資料庫 (Line 70-71)
│  │  ├─ AddAsync(entity)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  ├─ 清除快取 (Line 73-74)
│  │  └─ FusionCache.RemoveAsync(cacheKey)
│  │
│  └─ 回傳成功回應 (Line 76)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "新增成功: {ActionId}"
│        └─ Data: ActionId
```

---

## 關鍵業務規則

### 1. ActionId 唯一性
- ActionId 是操作的主鍵,必須在系統中唯一
- 新增前必須檢查是否已存在
- 若已存在則拋出錯誤,不允許重複新增

### 2. RouterId 關聯性
- RouterId 必須存在於 Auth_Router 表中
- 且該 Router 的 IsActive 必須為 'Y' (啟用狀態)
- 確保路由的有效性和一致性

### 3. IsCommon 權限控制
- IsCommon 只能是 'Y' 或 'N'
- 'Y' 表示通用資料,不檢查權限,所有使用者都可存取
- 'N' 表示需要權限檢查,依據角色權限設定控制存取

### 4. IsActive 狀態控制
- IsActive 只能是 'Y' 或 'N'
- 'Y' 表示啟用,該操作可以被使用
- 'N' 表示停用,該操作不可被使用

### 5. 審計追蹤
- 新增時自動記錄 AddUserId (從 JWT Token 取得當前使用者)
- 新增時自動記錄 AddTime (系統當前時間)
- 確保每筆資料都有完整的建立追蹤記錄

### 6. 快取一致性
- 新增操作後,會清除 FusionCache 中對應的快取
- 快取鍵格式: `{SecurityConstants.PolicyRedisKey.Action}:{ActionId}`
- 確保快取資料與資料庫資料一致

### 7. ActionId 命名規範
- ActionId 通常使用英數字命名
- 建議使用有意義的命名,如 "GetBillDayByQueryString"
- 最大長度 100 字元

### 8. ActionName 說明
- ActionName 使用中文命名,方便前端顯示
- 用於前端介面顯示功能名稱
- 最大長度 30 字元

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 8-18)

```json
POST /Action
Content-Type: application/json
Authorization: Bearer {jwt_token}

{
  "actionId": "GetBillDayByQueryString",
  "actionName": "查詢多筆帳單日",
  "isCommon": "Y",
  "isActive": "Y",
  "routerId": "SetUpBillDay"
}
```

### 回應範例

**成功回應:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "新增成功: GetBillDayByQueryString",
  "data": "GetBillDayByQueryString",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 資料已存在:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4002,
  "returnMessage": "資料已存在: GetBillDayByQueryString",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 路由不存在:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4003,
  "returnMessage": "前端傳入關聯資料有誤,欄位:路由Id,值:InsertAction",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Action/InsertAction/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Action/InsertAction/Models.cs` | 請求模型定義 |
| Examples.cs | `Modules/Auth/Action/InsertAction/Examples.cs` | Swagger 範例定義 |
| Auth_Action.cs | `Infrastructures/Data/Entities/Auth_Action.cs` | 資料庫實體定義 |
