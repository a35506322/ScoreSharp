# GetRoleById API - 查詢單筆角色 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Role/{roleId}` |
| **HTTP 方法** | GET |
| **功能** | 查詢單筆角色資料 - 用於取得指定角色的完整資訊 |
| **位置** | `Modules/Auth/Role/GetRoleById/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token |

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `roleId` | string | ✅ | 角色主鍵 |

### 範例路由

```
/Role/GetRoleById/Admin
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "成功",
  "data": {
    "roleId": "Admin",
    "roleName": "最高權限管理者",
    "isActive": "Y",
    "addUserId": "ADMIN",
    "addTime": "2024-01-01T00:00:00",
    "updateUserId": "ADMIN",
    "updateTime": "2024-01-01T00:00:00"
  },
  "traceId": "{traceId}"
}
```

### Response 資料結構

```csharp
// 位置: Modules/Auth/Role/GetRoleById/Models.cs (Line 3-39)
public class GetRoleByIdResponse
{
    /// <summary>
    /// 角色PK(英數字)
    /// </summary>
    public string RoleId { get; set; }

    /// <summary>
    /// 角色名稱(中文)
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// 是否啟用,範例: Y | N
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
}
```

### 錯誤回應

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- RoleId 不存在於 Auth_Role 表中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料,欄位:RoleId,值:顯示找不到的ID",
  "data": null,
  "traceId": "{traceId}"
}
```

#### 內部程式失敗 (500 Internal Server Error - Return Code: 5000)
- 系統內部處理錯誤

#### 資料庫執行失敗 (500 Internal Server Error - Return Code: 5002)
- 資料庫操作失敗

---

## 驗證資料

### 1. 格式驗證
- **位置**: ASP.NET Core Route Parameter Binding (自動執行)
- **驗證內容**:
  - roleId: 必填,從路由參數取得

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 51-61)

#### 檢查點 1: RoleId 存在性檢查
```
位置: Line 53-56
查詢 Auth_Role 表確認 RoleId 是否存在
├─ 若查無資料 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料,欄位:RoleId,值:{roleId}"
└─ 若有資料 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 53
- **查詢**: 取得指定 RoleId 的資料
  ```csharp
  await _context.Auth_Role.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RoleId == request.roleId)
  ```

### 2. 資料轉換
- **位置**: Line 58
- **轉換邏輯**:
  ```csharp
  var result = _mapper.Map<GetRoleByIdResponse>(single);
  ```
- **說明**: 使用 AutoMapper 將 Auth_Role 實體轉換為 GetRoleByIdResponse

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 查詢 Auth_Role 資料 (Line 53)
│  └─ 使用 AsNoTracking() 提升查詢效能
│
├─ 步驟 2: 檢查資料是否存在 (Line 55-56)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 3: 資料轉換 (Line 58)
│  └─ 使用 AutoMapper 轉換為 Response 物件
│
└─ 步驟 4: 回傳成功回應 (Line 60)
   └─ Return Code: 2000, Data: GetRoleByIdResponse
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Role` | SELECT | 角色主檔 - 查詢單筆資料 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢: 取得單筆 Role 資料**
- **位置**: Line 53
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_Role.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RoleId == request.roleId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Role]
  WHERE [RoleId] = @RoleId
  ```
- **AsNoTracking 說明**:
  - 因為是唯讀查詢,使用 AsNoTracking() 可以提升效能
  - 不會將實體加入 EF Core 的變更追蹤器
  - 減少記憶體使用與處理時間

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
用戶發送 GET /Role/{roleId} 請求
│
├─ ASP.NET Core Route Parameter Binding
│  ├─ 綁定路由參數 roleId
│  │
│  ├─ 綁定失敗 → 400 Bad Request
│  └─ 綁定成功 → 繼續
│
├─ 商業邏輯處理 (Handler.Handle)
│  │
│  ├─ 查詢 Auth_Role 資料 (Line 53)
│  │  └─ 使用 AsNoTracking() 唯讀查詢
│  │
│  ├─ 檢查資料是否存在 (Line 55-56)
│  │  ├─ 不存在 → 400 Bad Request (Return Code: 4001)
│  │  └─ 存在 → 繼續
│  │
│  ├─ 資料轉換 (Line 58)
│  │  └─ AutoMapper 轉換為 GetRoleByIdResponse
│  │
│  └─ 回傳成功回應 (Line 60)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "成功"
│        └─ Data: GetRoleByIdResponse
```

---

## 關鍵業務規則

### 1. 單筆資料查詢
- 使用 SingleOrDefaultAsync() 確保只會回傳一筆資料
- 若查詢結果超過一筆會拋出例外 (理論上不會發生,因為 RoleId 是主鍵)
- 若無資料則回傳 null

### 2. AsNoTracking 效能最佳化
- 因為是唯讀查詢,使用 AsNoTracking() 提升效能
- 不會將實體加入 EF Core 的變更追蹤器
- 適合用於查詢 API,不需要後續更新操作

### 3. 完整審計資訊回傳
- 回傳包含 AddUserId, AddTime, UpdateUserId, UpdateTime
- 讓前端可以顯示完整的建立與修改記錄
- 有助於資料追蹤與稽核

### 4. AutoMapper 資料轉換
- 使用 AutoMapper 將 Entity 轉換為 Response DTO
- 分離資料庫實體與 API 回應結構
- 遵循分層架構設計原則

### 5. IsActive 狀態資訊
- 回傳 IsActive 欄位,讓前端知道角色是否啟用
- 前端可根據此狀態決定是否允許操作
- 'Y' 表示啟用,'N' 表示停用

---

## 請求範例

### 請求範例

```http
GET /Role/Admin
Authorization: Bearer {jwt_token}
```

### 回應範例

**成功回應:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "成功",
  "data": {
    "roleId": "Admin",
    "roleName": "最高權限管理者",
    "isActive": "Y",
    "addUserId": "ADMIN",
    "addTime": "2024-01-01T00:00:00",
    "updateUserId": "ADMIN",
    "updateTime": "2024-01-01T00:00:00"
  },
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此資料:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此資料,欄位:RoleId,值:NotExistRole",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Role/GetRoleById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Role/GetRoleById/Models.cs` | 回應模型定義 |
| Examples.cs | `Modules/Auth/Role/GetRoleById/Examples.cs` | Swagger 範例定義 |
| Auth_Role.cs | `Infrastructures/Data/Entities/Auth_Role.cs` | 資料庫實體定義 |
