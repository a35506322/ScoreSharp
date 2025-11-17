# UpdateActionById API - 更新操作 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Action/{actionId}` |
| **HTTP 方法** | PUT |
| **功能** | 更新單筆操作資料 By Id - 用於修改系統 API 操作設定 |
| **位置** | `Modules/Auth/Action/UpdateActionById/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token (從 JWT 中取得 UserId) |

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `actionId` | string | ✅ | 操作主鍵,用於指定要更新的操作 |

### 請求體 (Body)

```csharp
// 位置: Modules/Auth/Action/UpdateActionById/Models.cs (Line 3-44)
public class UpdateActionByIdRequest
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
| `actionId` (路由) | string | ✅ | - | 無 | 路由參數,指定要更新的操作 ID |
| `ActionId` (Body) | string | ✅ | 100 | 無 | API Action 名稱,必須與路由參數一致 |
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
  "returnMessage": "更新成功: GetBillDayById",
  "data": "GetBillDayById",
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

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- ActionId 不存在於資料庫中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料: SetUpBill",
  "data": null
}
```

#### 路由與Req比對錯誤 (400 Bad Request - Return Code: 4003)
- 路由參數 actionId 與 Request Body 的 ActionId 不一致

```json
{
  "returnCode": 4003,
  "returnMessage": "路由與Req比對錯誤",
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
- **位置**: `Handle()` 方法 (Endpoint.cs Line 60-85)

#### 檢查點 1: 路由參數與請求體一致性檢查
```
位置: Line 62-63
檢查路由參數 actionId 是否與 Request Body 的 ActionId 一致
├─ 若不一致 → 拋出路由與Req比對錯誤 (Return Code: 4003)
│  └─ 訊息: "路由與Req比對錯誤"
└─ 若一致 → 繼續執行
```

#### 檢查點 2: ActionId 存在性檢查
```
位置: Line 65-68
檢查 Auth_Action 表中是否存在指定的 ActionId
├─ 若不存在 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料: {actionId}"
└─ 若存在 → 繼續執行
```

#### 檢查點 3: RouterId 關聯性檢查
```
位置: Line 72-74
查詢 Auth_Router 表
├─ 條件: RouterId = {dto.RouterId} AND IsActive = 'Y'
├─ 若查無資料 → 拋出前端傳入關聯資料有誤錯誤 (Return Code: 4003)
│  └─ 訊息: "前端傳入關聯資料有誤,欄位:路由Id,值:{RouterId}"
└─ 若有資料 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 65, Line 72
- **查詢 1**: 檢查 ActionId 是否存在
  ```csharp
  await _context.Auth_Action.SingleOrDefaultAsync(x => x.ActionId == request.actionId)
  ```
- **查詢 2**: 驗證 RouterId 是否有效 (使用 AsNoTracking)
  ```csharp
  await _context.Auth_Router.AsNoTracking().SingleOrDefaultAsync(x =>
      x.RouterId == dto.RouterId && x.IsActive == "Y")
  ```

### 2. 資料轉換
- **位置**: Line 76-78
- **轉換邏輯**:
  ```csharp
  var entity = _mapper.Map<Auth_Action>(dto);
  entity.UpdateTime = DateTime.Now;          // 系統當前時間
  entity.UpdateUserId = _jwthelper.UserId;  // 從 JWT Token 取得
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查路由參數與請求體一致性 (Line 62-63)
│  ├─ 不一致 → 回傳錯誤 4003 (路由與Req比對錯誤)
│  └─ 一致 → 繼續
│
├─ 步驟 2: 檢查 ActionId 是否存在 (Line 65-68)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 3: 驗證 RouterId 有效性 (Line 72-74)
│  ├─ 無效或未啟用 → 回傳錯誤 4003
│  └─ 有效 → 繼續
│
├─ 步驟 4: 建立 Auth_Action 實體 (Line 76-78)
│  ├─ 使用 AutoMapper 對應資料
│  ├─ 設定 UpdateUserId (從 JWT Token)
│  └─ 設定 UpdateTime (當前時間)
│
├─ 步驟 5: 使用 Dapper 更新資料 (Line 80)
│  └─ 呼叫 UpdateActionById() 方法執行原生 SQL UPDATE
│
├─ 步驟 6: 清除快取 (Line 82)
│  └─ 移除 FusionCache 中的 Action 快取
│
└─ 步驟 7: 回傳成功訊息 (Line 84)
   └─ Return Code: 2000, Data: actionId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Action` | SELECT, UPDATE | 操作主檔 - 查詢存在性檢查及更新資料 |
| `Auth_Router` | SELECT | 路由主檔 - 驗證關聯性 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢 1: 檢查 ActionId 是否存在**
- **位置**: Line 65
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_Action.SingleOrDefaultAsync(x => x.ActionId == request.actionId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Action]
  WHERE [ActionId] = @ActionId
  ```

**查詢 2: 驗證 RouterId**
- **位置**: Line 72
- **方法**: `AsNoTracking().SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var router = await _context.Auth_Router.AsNoTracking().SingleOrDefaultAsync(x =>
      x.RouterId == dto.RouterId && x.IsActive == "Y");
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Router]
  WHERE [RouterId] = @RouterId
    AND [IsActive] = 'Y'
  ```
- **說明**: 使用 AsNoTracking() 因為只需要驗證,不需要追蹤實體變更

#### 2. 更新操作 (UPDATE - 使用 Dapper)

- **位置**: Line 87-102
- **方法**: `Dapper ExecuteAsync()`
- **Dapper 程式碼**:
  ```csharp
  string sql = @"UPDATE [dbo].[Auth_Action]
                      SET  [ActionName] = @ActionName
                          ,[IsCommon] = @IsCommon
                          ,[IsActive] = @IsActive
                          ,[UpdateUserId] = @UpdateUserId
                          ,[UpdateTime] = @UpdateTime
                          ,[RouterId] = @RouterId
                      WHERE [ActionId] = @ActionId";

  using var conn = _dapperContext.CreateScoreSharpConnection();
  var result = await conn.ExecuteAsync(sql, entity);
  ```
- **原生 SQL**:
  ```sql
  UPDATE [dbo].[Auth_Action]
  SET [ActionName] = @ActionName
     ,[IsCommon] = @IsCommon
     ,[IsActive] = @IsActive
     ,[UpdateUserId] = @UpdateUserId
     ,[UpdateTime] = @UpdateTime
     ,[RouterId] = @RouterId
  WHERE [ActionId] = @ActionId
  ```
- **說明**:
  - 使用 Dapper 執行原生 SQL UPDATE,提供更好的效能
  - 只更新必要欄位,不更新 AddUserId 和 AddTime
  - 使用參數化查詢防止 SQL Injection

#### 3. 快取操作 (Cache Remove)

- **位置**: Line 82
- **方法**: `FusionCache.RemoveAsync()`
- **程式碼**:
  ```csharp
  await _fusionCache.RemoveAsync($"{SecurityConstants.PolicyRedisKey.Action}:{request.actionId}");
  ```
- **說明**: 移除 FusionCache 中的 Action 快取,確保快取一致性

### Auth_Action 資料表結構

| 欄位名稱 | 資料型別 | 允許NULL | 說明 | 是否更新 |
|---------|---------|---------|------|---------|
| `ActionId` | string(100) | ❌ | 主鍵 - API Action 識別碼 | ❌ (WHERE 條件) |
| `ActionName` | string(30) | ❌ | API Action 名稱 | ✅ |
| `IsCommon` | string(1) | ❌ | 是否是通用資料 (Y/N) | ✅ |
| `IsActive` | string(1) | ❌ | 是否啟用 (Y/N) | ✅ |
| `AddUserId` | string | ❌ | 新增員工編號 | ❌ (保持原值) |
| `AddTime` | DateTime | ❌ | 新增時間 | ❌ (保持原值) |
| `UpdateUserId` | string | ✅ | 修改員工編號 | ✅ |
| `UpdateTime` | DateTime | ✅ | 修改時間 | ✅ |
| `RouterId` | string(50) | ❌ | 路由主鍵 (FK) | ✅ |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 PUT /Action/{actionId} 請求
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
│  ├─ 檢查路由參數與請求體一致性 (Line 62-63)
│  │  ├─ 不一致 → 400 Bad Request (Return Code: 4003)
│  │  │  └─ 訊息: "路由與Req比對錯誤"
│  │  └─ 一致 → 繼續
│  │
│  ├─ 檢查 ActionId 是否存在 (Line 65-68)
│  │  ├─ 不存在 → 400 Bad Request (Return Code: 4001)
│  │  │  └─ 訊息: "查無此資料: {actionId}"
│  │  └─ 存在 → 繼續
│  │
│  ├─ 檢查 RouterId 有效性 (Line 72-74)
│  │  ├─ 查無資料或未啟用 → 400 Bad Request (Return Code: 4003)
│  │  │  └─ 訊息: "前端傳入關聯資料有誤,欄位:路由Id,值:{RouterId}"
│  │  └─ 有效 → 繼續
│  │
│  ├─ 建立 Auth_Action 實體 (Line 76-78)
│  │  ├─ 設定基本資料 (使用 AutoMapper)
│  │  ├─ 設定 UpdateUserId (從 JWT Token)
│  │  └─ 設定 UpdateTime (DateTime.Now)
│  │
│  ├─ 使用 Dapper 更新資料庫 (Line 80, Line 87-102)
│  │  ├─ 建立原生 SQL UPDATE 語句
│  │  ├─ 建立資料庫連線
│  │  ├─ ExecuteAsync(sql, entity)
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  ├─ 清除快取 (Line 82)
│  │  └─ FusionCache.RemoveAsync(cacheKey)
│  │
│  └─ 回傳成功回應 (Line 84)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "更新成功: {actionId}"
│        └─ Data: actionId
```

---

## 關鍵業務規則

### 1. 路由參數一致性檢查
- 路由參數 actionId 必須與 Request Body 的 ActionId 一致
- 這是 RESTful API 的最佳實踐,確保資料一致性
- 若不一致則拋出錯誤,防止誤操作

### 2. ActionId 存在性檢查
- 更新前必須確認 ActionId 存在於資料庫中
- 若不存在則拋出錯誤,不允許更新不存在的資料

### 3. RouterId 關聯性
- RouterId 必須存在於 Auth_Router 表中
- 且該 Router 的 IsActive 必須為 'Y' (啟用狀態)
- 確保路由的有效性和一致性

### 4. IsCommon 權限控制
- IsCommon 只能是 'Y' 或 'N'
- 'Y' 表示通用資料,不檢查權限,所有使用者都可存取
- 'N' 表示需要權限檢查,依據角色權限設定控制存取

### 5. IsActive 狀態控制
- IsActive 只能是 'Y' 或 'N'
- 'Y' 表示啟用,該操作可以被使用
- 'N' 表示停用,該操作不可被使用

### 6. 審計追蹤
- 更新時自動記錄 UpdateUserId (從 JWT Token 取得當前使用者)
- 更新時自動記錄 UpdateTime (系統當前時間)
- 保留原始的 AddUserId 和 AddTime,維持完整追蹤記錄

### 7. 使用 Dapper 執行更新
- 使用 Dapper 執行原生 SQL UPDATE,提供更好的效能
- 只更新需要變更的欄位,不更新 AddUserId 和 AddTime
- 使用參數化查詢防止 SQL Injection 攻擊

### 8. AsNoTracking 查詢優化
- 驗證 RouterId 時使用 AsNoTracking(),提升查詢效能
- 因為只需要驗證資料存在,不需要追蹤實體變更狀態

### 9. 快取一致性
- 更新操作後,會清除 FusionCache 中對應的快取
- 快取鍵格式: `{SecurityConstants.PolicyRedisKey.Action}:{actionId}`
- 確保快取資料與資料庫資料一致

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 8-17)

```json
PUT /Action/GetBillDayById
Content-Type: application/json
Authorization: Bearer {jwt_token}

{
  "actionId": "GetBillDayById",
  "actionName": "查詢單筆帳單日",
  "routerId": "SetUpBillDay",
  "isCommon": "Y",
  "isActive": "Y"
}
```

### 回應範例

**成功回應:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "更新成功: GetBillDayById",
  "data": "GetBillDayById",
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

**失敗回應 - 查無路由:**
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
| Endpoint.cs | `Modules/Auth/Action/UpdateActionById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Action/UpdateActionById/Models.cs` | 請求模型定義 |
| Examples.cs | `Modules/Auth/Action/UpdateActionById/Examples.cs` | Swagger 範例定義 |
| Auth_Action.cs | `Infrastructures/Data/Entities/Auth_Action.cs` | 資料庫實體定義 |
