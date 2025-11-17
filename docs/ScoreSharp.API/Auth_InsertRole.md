# InsertRole API - 新增角色 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Role` |
| **HTTP 方法** | POST |
| **功能** | 新增單筆角色資料 - 用於建立系統角色設定 |
| **位置** | `Modules/Auth/Role/InsertRole/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token (從 JWT 中取得 UserId) |

### 請求體 (Body)

```csharp
// 位置: Modules/Auth/Role/InsertRole/Models.cs (Line 3-28)
public class InsertRoleRequest
{
    [Display(Name = "角色PK")]
    [MaxLength(50)]
    [Required]
    public string RoleId { get; set; }  // 英數字,角色主鍵,如:Admin

    [Display(Name = "角色名稱")]
    [MaxLength(30)]
    [Required]
    public string RoleName { get; set; }  // 中文,角色名稱,如:最高權限管理者

    [Display(Name = "是否啟用")]
    [Required]
    [RegularExpression("[YN]")]
    public string IsActive { get; set; }  // Y/N
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 最大長度 | 驗證規則 | 說明 |
|------|------|------|----------|----------|------|
| `RoleId` | string | ✅ | 50 | 無 | 角色主鍵,英數字,如 Admin、Consultant |
| `RoleName` | string | ✅ | 30 | 無 | 角色名稱,中文,如 最高權限管理者 |
| `IsActive` | string | ✅ | - | [YN] | 是否啟用,只能是 Y 或 N |

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "新增成功: Consultant",
  "data": "Consultant",
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

#### 資料已存在 (400 Bad Request - Return Code: 4002)
- RoleId 已存在於資料庫中

```json
{
  "returnCode": 4002,
  "returnMessage": "資料已存在: Consultant",
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
  - RoleName: 必填, 最大長度 30 字元
  - IsActive: 必填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 49-66)

#### 檢查點 1: RoleId 唯一性檢查
```
位置: Line 53-56
檢查 Auth_Role 表中是否已存在相同的 RoleId
├─ 若存在 → 拋出 DataAlreadyExists 錯誤 (Return Code: 4002)
│  └─ 訊息: "資料已存在: {RoleId}"
└─ 若不存在 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 53
- **查詢**: 檢查 RoleId 是否已存在
  ```csharp
  await _context.Auth_Role.AsNoTracking().SingleOrDefaultAsync(x => x.RoleId == insertRoleRequest.RoleId)
  ```

### 2. 資料轉換
- **位置**: Line 58-60
- **轉換邏輯**:
  ```csharp
  var entity = _mapper.Map<Auth_Role>(insertRoleRequest);
  entity.AddTime = DateTime.Now;          // 系統當前時間
  entity.AddUserId = _jwthelper.UserId;  // 從 JWT Token 取得
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查 RoleId 是否已存在 (Line 53-56)
│  ├─ 已存在 → 回傳錯誤 4002
│  └─ 不存在 → 繼續
│
├─ 步驟 2: 建立 Auth_Role 實體 (Line 58-60)
│  ├─ 使用 AutoMapper 對應請求資料
│  ├─ 設定 AddUserId (從 JWT Token)
│  └─ 設定 AddTime (當前時間)
│
├─ 步驟 3: 新增至資料庫 (Line 62-63)
│  └─ 呼叫 AddAsync() 和 SaveChangesAsync()
│
└─ 步驟 4: 回傳成功訊息 (Line 65)
   └─ Return Code: 2000, Data: RoleId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Role` | SELECT, INSERT | 角色主檔 - 查詢唯一性檢查及新增資料 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢: 檢查 RoleId 唯一性**
- **位置**: Line 53
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_Role.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RoleId == insertRoleRequest.RoleId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Role]
  WHERE [RoleId] = @RoleId
  ```

#### 2. 新增操作 (INSERT)

- **位置**: Line 62-63
- **方法**: `AddAsync()` + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  await _context.AddAsync(entity);
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  INSERT INTO [dbo].[Auth_Role]
      ([RoleId], [RoleName], [IsActive], [AddUserId], [AddTime])
  VALUES
      (@RoleId, @RoleName, @IsActive, @AddUserId, @AddTime)
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
用戶發送 POST /Role 請求
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證必填欄位
│  ├─ 驗證欄位長度
│  ├─ 驗證 IsActive 格式 [YN]
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯驗證 (Handler.Handle)
│  │
│  ├─ 檢查 RoleId 唯一性 (Line 53-56)
│  │  ├─ 已存在 → 400 Bad Request (Return Code: 4002)
│  │  └─ 不存在 → 繼續
│  │
│  ├─ 建立 Auth_Role 實體 (Line 58-60)
│  │  ├─ AutoMapper 對應資料
│  │  ├─ 設定 AddUserId (從 JWT Token)
│  │  └─ 設定 AddTime (DateTime.Now)
│  │
│  ├─ 新增至資料庫 (Line 62-63)
│  │  ├─ AddAsync(entity)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 回傳成功回應 (Line 65)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "新增成功: {RoleId}"
│        └─ Data: RoleId
```

---

## 關鍵業務規則

### 1. RoleId 唯一性
- RoleId 是角色的主鍵,必須在系統中唯一
- 新增前必須檢查是否已存在
- 若已存在則拋出錯誤,不允許重複新增

### 2. IsActive 狀態控制
- IsActive 只能是 'Y' 或 'N'
- 'Y' 表示啟用,該角色可被指派給使用者
- 'N' 表示停用,該角色無法被指派

### 3. 審計追蹤
- 新增時自動記錄 AddUserId (從 JWT Token 取得當前使用者)
- 新增時自動記錄 AddTime (系統當前時間)
- 確保每筆資料都有完整的建立追蹤記錄

### 4. 角色命名規範
- RoleId: 使用英數字,建議使用 PascalCase 命名 (如: Admin, Consultant, Reviewer)
- RoleName: 使用中文描述性名稱 (如: 最高權限管理者, 顧問, 徵審人員)
- 命名應清晰明確,便於系統管理

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 3-16)

```json
POST /Role
Content-Type: application/json
Authorization: Bearer {jwt_token}

{
  "roleId": "Consultant",
  "roleName": "顧問",
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
  "returnMessage": "新增成功: Consultant",
  "data": "Consultant",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 資料已存在:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4002,
  "returnMessage": "資料已存在: Consultant",
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
| Endpoint.cs | `Modules/Auth/Role/InsertRole/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Role/InsertRole/Models.cs` | 請求模型定義 |
| Examples.cs | `Modules/Auth/Role/InsertRole/Examples.cs` | Swagger 範例定義 |
| Auth_Role.cs | `Infrastructures/Data/Entities/Auth_Role.cs` | 資料庫實體定義 |
