# UpdateRoleById API - 更新角色 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Role/{roleId}` |
| **HTTP 方法** | PUT |
| **功能** | 更新單筆角色資料 - 用於修改角色名稱及啟用狀態 |
| **位置** | `Modules/Auth/Role/UpdateRoleById/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token (從 JWT 中取得 UserId) |

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `roleId` | string | ✅ | 角色主鍵,必須與 Request Body 中的 RoleId 一致 |

### 請求體 (Body)

```csharp
// 位置: Modules/Auth/Role/UpdateRoleById/Models.cs (Line 3-26)
public class UpdateRoleByIdRequest
{
    [Display(Name = "角色PK")]
    [MaxLength(50)]
    [Required]
    public string RoleId { get; set; }  // 必須與路由參數一致

    [Display(Name = "角色名稱")]
    [MaxLength(30)]
    public string? RoleName { get; set; }  // 選填,更新角色名稱

    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }  // 選填,更新啟用狀態 Y/N
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 最大長度 | 驗證規則 | 說明 |
|------|------|------|----------|----------|------|
| `RoleId` | string | ✅ | 50 | 無 | 角色主鍵,必須與路由參數一致 |
| `RoleName` | string | ❌ | 30 | 無 | 角色名稱,中文 |
| `IsActive` | string | ❌ | - | [YN] | 是否啟用,只能是 Y 或 N |

### 重要提示

1. **路由參數一致性**: URL 中的 `roleId` 必須與 Request Body 中的 `RoleId` 完全一致
2. **選填欄位**: RoleName 與 IsActive 皆為選填,可只更新其中一個欄位

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "依PK修改成功: Agent",
  "data": "Agent",
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
    "RoleId": ["RoleId 為必填欄位"],
    "IsActive": ["IsActive 必須符合正則表達式 [YN]"]
  }
}
```

#### 路由與 Req 比對錯誤 (400 Bad Request - Return Code: 4003)
- URL 路由參數 roleId 與 Request Body 中的 RoleId 不一致

```json
{
  "returnCode": 4003,
  "returnMessage": "路由與Req比對錯誤",
  "data": null
}
```

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- RoleId 不存在於 Auth_Role 表中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料,欄位:RoleId,值:Agent",
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
  - RoleId: 必填, 最大長度 50 字元
  - RoleName: 選填, 最大長度 30 字元
  - IsActive: 選填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 50-70)

#### 檢查點 1: 路由參數與 Request Body RoleId 一致性
```
位置: Line 54-55
檢查 URL 路由參數 roleId 是否與 Request Body 中的 RoleId 一致
├─ 若不一致 → 拋出路由與Req比對錯誤 (Return Code: 4003)
│  └─ 訊息: "路由與Req比對錯誤"
└─ 若一致 → 繼續執行
```

#### 檢查點 2: RoleId 存在性檢查
```
位置: Line 57-60
查詢 Auth_Role 表確認 RoleId 是否存在
├─ 若查無資料 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料,欄位:RoleId,值:{roleId}"
└─ 若有資料 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 57
- **查詢**: 取得要更新的 Role 資料
  ```csharp
  await _context.Auth_Role.SingleOrDefaultAsync(x => x.RoleId == request.roleId)
  ```

### 2. 資料更新
- **位置**: Line 62-65
- **更新邏輯**:
  ```csharp
  entity.IsActive = updateRoleByIdRequest.IsActive;
  entity.RoleName = updateRoleByIdRequest.RoleName;
  entity.UpdateTime = DateTime.Now;          // 系統當前時間
  entity.UpdateUserId = _jwthelper.UserId;  // 從 JWT Token 取得
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查路由參數與 Request Body RoleId 一致性 (Line 54-55)
│  ├─ 不一致 → 回傳錯誤 4003
│  └─ 一致 → 繼續
│
├─ 步驟 2: 查詢 RoleId 是否存在 (Line 57-60)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 3: 更新實體資料 (Line 62-65)
│  ├─ 更新 IsActive
│  ├─ 更新 RoleName
│  ├─ 設定 UpdateTime (當前時間)
│  └─ 設定 UpdateUserId (從 JWT Token)
│
├─ 步驟 4: 儲存變更 (Line 67)
│  └─ 呼叫 SaveChangesAsync()
│
└─ 步驟 5: 回傳成功訊息 (Line 69)
   └─ Return Code: 2000, Data: RoleId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Role` | SELECT, UPDATE | 角色主檔 - 查詢資料及更新 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢: 取得要更新的 Role 資料**
- **位置**: Line 57
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var entity = await _context.Auth_Role
      .SingleOrDefaultAsync(x => x.RoleId == request.roleId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Role]
  WHERE [RoleId] = @RoleId
  ```

#### 2. 更新操作 (UPDATE)

- **位置**: Line 62-67
- **方法**: 直接修改實體屬性 + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  entity.IsActive = updateRoleByIdRequest.IsActive;
  entity.RoleName = updateRoleByIdRequest.RoleName;
  entity.UpdateTime = DateTime.Now;
  entity.UpdateUserId = _jwthelper.UserId;
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  UPDATE [dbo].[Auth_Role]
  SET [IsActive] = @IsActive,
      [RoleName] = @RoleName,
      [UpdateTime] = @UpdateTime,
      [UpdateUserId] = @UpdateUserId
  WHERE [RoleId] = @RoleId
  ```

### Auth_Role 資料表結構

| 欄位名稱 | 資料型別 | 允許NULL | 說明 |
|---------|---------|---------|------|
| `RoleId` | string(50) | ❌ | 主鍵 - 角色識別碼 |
| `RoleName` | string(30) | ❌ | 角色名稱 |
| `IsActive` | string(1) | ❌ | 是否啟用 (Y/N) |
| `AddUserId` | string | ❌ | 新增員工編號 |
| `AddTime` | DateTime | ❌ | 新增時間 |
| `UpdateUserId` | string | ✅ | 修改員工編號 |
| `UpdateTime` | DateTime | ✅ | 修改時間 |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 PUT /Role/{roleId} 請求
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證必填欄位
│  ├─ 驗證欄位長度
│  ├─ 驗證 IsActive 格式 [YN]
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯驗證與處理 (Handler.Handle)
│  │
│  ├─ 檢查路由參數一致性 (Line 54-55)
│  │  ├─ 不一致 → 400 Bad Request (Return Code: 4003)
│  │  └─ 一致 → 繼續
│  │
│  ├─ 檢查 RoleId 是否存在 (Line 57-60)
│  │  ├─ 不存在 → 400 Bad Request (Return Code: 4001)
│  │  └─ 存在 → 繼續
│  │
│  ├─ 更新實體資料 (Line 62-65)
│  │  ├─ 更新 IsActive
│  │  ├─ 更新 RoleName
│  │  ├─ 設定 UpdateTime (DateTime.Now)
│  │  └─ 設定 UpdateUserId (從 JWT Token)
│  │
│  ├─ 儲存變更 (Line 67)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 回傳成功回應 (Line 69)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "依PK修改成功: {RoleId}"
│        └─ Data: RoleId
```

---

## 關鍵業務規則

### 1. 路由參數一致性驗證
- URL 路由參數 `roleId` 必須與 Request Body 中的 `RoleId` 一致
- 這是為了防止前端錯誤或惡意請求,確保修改的是正確的角色
- 若不一致會立即拋出錯誤,不進行後續處理

### 2. RoleId 不可修改
- RoleId 是主鍵,雖然在 Request 中必須提供,但不會被更新
- 只能更新 RoleName 與 IsActive 欄位
- 確保主鍵的穩定性與資料一致性

### 3. 選填欄位彈性更新
- RoleName 與 IsActive 皆為選填
- 可以只更新其中一個欄位,另一個保持原值
- 提供更靈活的更新操作

### 4. IsActive 狀態控制
- IsActive 只能是 'Y' 或 'N'
- 'Y' 表示啟用,該角色可被指派給使用者
- 'N' 表示停用,該角色無法被指派
- 常用於軟刪除或暫時停用角色

### 5. 審計追蹤
- 更新時自動記錄 UpdateUserId (從 JWT Token 取得當前使用者)
- 更新時自動記錄 UpdateTime (系統當前時間)
- 確保每筆修改都有完整的追蹤記錄

### 6. EF Core 變更追蹤
- 使用 EF Core 的變更追蹤機制
- 先查詢實體,再修改屬性,最後 SaveChangesAsync()
- EF Core 會自動偵測變更並產生對應的 UPDATE SQL

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 3-11)

```json
PUT /Role/Agent
Content-Type: application/json
Authorization: Bearer {jwt_token}

{
  "roleId": "Agent",
  "isActive": "Y",
  "roleName": "Agent"
}
```

### 回應範例

**成功回應:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "依PK修改成功: Agent",
  "data": "Agent",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此資料:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此資料,欄位:RoleId,值:Agent",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 路由與 Req 比對錯誤:**
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

**失敗回應 - 格式驗證失敗:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4000,
  "returnMessage": "格式驗證失敗",
  "data": {
    "IsActive": ["IsActive 必須符合正則表達式 [YN]"]
  },
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Role/UpdateRoleById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Role/UpdateRoleById/Models.cs` | 請求模型定義 |
| Examples.cs | `Modules/Auth/Role/UpdateRoleById/Examples.cs` | Swagger 範例定義 |
| Auth_Role.cs | `Infrastructures/Data/Entities/Auth_Role.cs` | 資料庫實體定義 |
