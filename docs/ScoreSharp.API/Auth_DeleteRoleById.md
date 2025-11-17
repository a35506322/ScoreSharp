# DeleteRoleById API - 刪除角色 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Role/{roleId}` |
| **HTTP 方法** | DELETE |
| **功能** | 刪除單筆角色資料 - 用於移除角色及其相關權限設定 |
| **位置** | `Modules/Auth/Role/DeleteRoleById/Endpoint.cs` |

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
/Role/DeleteRoleById/Consultant
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "依PK刪除成功: 刪除的ID",
  "data": "刪除的ID",
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- RoleId 不存在於 Auth_Role 表中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料,欄位:RoleId,值:刪除的ID",
  "data": null,
  "traceId": "{traceId}"
}
```

#### 此資源已被使用 (400 Bad Request - Return Code: 4003)
- 該角色已被指派給使用者,存在於 Auth_User_Role 表中
- 不允許刪除正在使用的角色

```json
{
  "returnCode": 4003,
  "returnMessage": "此資源已被使用,欄位:RoleId,值:刪除的ID",
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
- **位置**: `Handle()` 方法 (Endpoint.cs Line 51-67)

#### 檢查點 1: RoleId 存在性檢查
```
位置: Line 53-56
查詢 Auth_Role 表確認 RoleId 是否存在
├─ 若查無資料 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料,欄位:RoleId,值:{roleId}"
└─ 若有資料 → 繼續執行
```

#### 檢查點 2: 資源使用檢查 (重要安全機制)
```
位置: Line 58-60
查詢 Auth_User_Role 表確認該角色是否已被使用
├─ 若有使用者被指派此角色 → 拋出此資源已被使用錯誤 (Return Code: 4003)
│  └─ 訊息: "此資源已被使用,欄位:RoleId,值:{roleId}"
└─ 若無使用者使用此角色 → 繼續執行,允許刪除
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 53, Line 58
- **查詢 1**: 取得要刪除的 Role 資料
  ```csharp
  await _context.Auth_Role.SingleOrDefaultAsync(x => x.RoleId == request.roleId)
  ```
- **查詢 2**: 檢查角色是否被使用
  ```csharp
  await _context.Auth_User_Role.AsNoTracking()
      .Where(x => x.RoleId == request.roleId)
      .ToListAsync()
  ```

### 2. 刪除操作
- **位置**: Line 62-63
- **刪除邏輯**:
  ```csharp
  // 刪除角色主檔
  _context.Remove(single);

  // 批次刪除該角色的所有權限設定
  await _context.Auth_Role_Router_Action
      .Where(x => x.RoleId == request.roleId)
      .ExecuteDeleteAsync();
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 查詢 RoleId 是否存在 (Line 53-56)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 2: 檢查角色是否被使用 (Line 58-60)
│  ├─ 已被使用 → 回傳錯誤 4003
│  └─ 未被使用 → 繼續
│
├─ 步驟 3: 執行刪除操作 (Line 62-64)
│  ├─ 刪除角色主檔 (Remove)
│  ├─ 批次刪除角色權限設定 (ExecuteDeleteAsync)
│  └─ 儲存變更 (SaveChangesAsync)
│
└─ 步驟 4: 回傳成功訊息 (Line 66)
   └─ Return Code: 2000, Data: RoleId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Role` | SELECT, DELETE | 角色主檔 - 查詢存在性及刪除 |
| `Auth_User_Role` | SELECT | 使用者角色關聯表 - 檢查是否被使用 |
| `Auth_Role_Router_Action` | DELETE | 角色權限關聯表 - 批次刪除權限設定 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢 1: 取得要刪除的 Role 資料**
- **位置**: Line 53
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_Role
      .SingleOrDefaultAsync(x => x.RoleId == request.roleId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Role]
  WHERE [RoleId] = @RoleId
  ```

**查詢 2: 檢查角色是否被使用**
- **位置**: Line 58
- **方法**: `Where()` + `ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var usedData = await _context.Auth_User_Role.AsNoTracking()
      .Where(x => x.RoleId == request.roleId)
      .ToListAsync();
  ```
- **等效 SQL**:
  ```sql
  SELECT *
  FROM [dbo].[Auth_User_Role]
  WHERE [RoleId] = @RoleId
  ```

#### 2. 刪除操作 (DELETE)

**刪除 1: 刪除角色主檔**
- **位置**: Line 62
- **方法**: `Remove()` + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  _context.Remove(single);
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  DELETE FROM [dbo].[Auth_Role]
  WHERE [RoleId] = @RoleId
  ```

**刪除 2: 批次刪除角色權限設定**
- **位置**: Line 63
- **方法**: `ExecuteDeleteAsync()` (EF Core 7 批次刪除)
- **EF Core 程式碼**:
  ```csharp
  await _context.Auth_Role_Router_Action
      .Where(x => x.RoleId == request.roleId)
      .ExecuteDeleteAsync();
  ```
- **等效 SQL**:
  ```sql
  DELETE FROM [dbo].[Auth_Role_Router_Action]
  WHERE [RoleId] = @RoleId
  ```

### 資料表結構

#### Auth_Role
| 欄位名稱 | 資料型別 | 允許NULL | 說明 |
|---------|---------|---------|------|
| `RoleId` | string(50) | ❌ | 主鍵 - 角色識別碼 |
| `RoleName` | string(30) | ❌ | 角色名稱 |
| `IsActive` | string(1) | ❌ | 是否啟用 (Y/N) |
| `AddUserId` | string | ❌ | 新增員工編號 |
| `AddTime` | DateTime | ❌ | 新增時間 |
| `UpdateUserId` | string | ✅ | 修改員工編號 |
| `UpdateTime` | DateTime | ✅ | 修改時間 |

#### Auth_User_Role
| 欄位名稱 | 資料型別 | 說明 |
|---------|---------|------|
| `UserId` | string(50) | 主鍵 - 使用者識別碼 |
| `RoleId` | string(50) | 主鍵 - 角色識別碼 (FK to Auth_Role) |

#### Auth_Role_Router_Action
| 欄位名稱 | 資料型別 | 說明 |
|---------|---------|------|
| `RoleId` | string(50) | 主鍵 - 角色識別碼 (FK to Auth_Role) |
| `RouterId` | string(50) | 主鍵 - 路由識別碼 |
| `ActionId` | string(50) | 主鍵 - 操作識別碼 |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 DELETE /Role/{roleId} 請求
│
├─ 商業邏輯驗證與處理 (Handler.Handle)
│  │
│  ├─ 步驟 1: 查詢 RoleId 是否存在 (Line 53-56)
│  │  ├─ 不存在 → 400 Bad Request (Return Code: 4001)
│  │  └─ 存在 → 繼續
│  │
│  ├─ 步驟 2: 檢查角色是否被使用 (Line 58-60)
│  │  ├─ 查詢 Auth_User_Role 表
│  │  ├─ 若有使用者被指派此角色 → 400 Bad Request (Return Code: 4003)
│  │  └─ 若無使用者使用此角色 → 繼續
│  │
│  ├─ 步驟 3: 執行刪除操作 (Line 62-64)
│  │  ├─ 刪除角色主檔 (Remove)
│  │  ├─ 批次刪除角色權限設定 (ExecuteDeleteAsync)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 步驟 4: 回傳成功回應 (Line 66)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "依PK刪除成功: {RoleId}"
│        └─ Data: RoleId
```

---

## 關鍵業務規則

### 1. 資源使用檢查 (重要安全機制)
- **刪除前必須檢查角色是否被使用**
- 查詢 Auth_User_Role 表確認是否有使用者被指派此角色
- 若有使用者使用此角色,不允許刪除,拋出錯誤
- 防止誤刪正在使用的角色,導致使用者權限異常

### 2. 級聯刪除權限設定
- 刪除角色時,同時刪除該角色的所有權限設定
- 使用 ExecuteDeleteAsync() 批次刪除 Auth_Role_Router_Action 中的資料
- 確保資料一致性,避免孤兒資料
- 不需要手動處理關聯資料,簡化刪除邏輯

### 3. 兩階段刪除驗證
1. **第一階段**: 檢查角色是否存在
2. **第二階段**: 檢查角色是否被使用
- 兩個檢查點確保刪除操作的安全性
- 提供明確的錯誤訊息,讓前端知道失敗原因

### 4. ExecuteDeleteAsync 批次刪除
- 使用 EF Core 7 的 ExecuteDeleteAsync() 功能
- 直接在資料庫執行刪除,不需先查詢再刪除
- 效能優於傳統的 Remove() 方式
- 適合批次刪除關聯資料

### 5. 交易一致性
- 所有刪除操作在同一個 DbContext 中執行
- SaveChangesAsync() 會將所有變更包裝在交易中
- 確保角色主檔與權限設定的刪除是原子性操作
- 若任何步驟失敗,整個交易會回滾

### 6. 軟刪除 vs 硬刪除
- **此 API 執行硬刪除** (實際從資料庫移除資料)
- 若需要保留歷史記錄,建議改用軟刪除 (設定 IsActive = 'N')
- 硬刪除適用於測試資料或確定不再需要的角色

### 7. 不刪除使用者資料
- 即使刪除角色,也不會刪除 Auth_User_Role 中的資料
- 因為在刪除前已確認該角色未被使用
- 若角色被使用,會阻止刪除操作

### 8. AsNoTracking 效能最佳化
- 檢查角色使用時使用 AsNoTracking()
- 因為只是檢查存在性,不需要變更追蹤
- 提升查詢效能,減少記憶體使用

---

## 請求範例

### 請求範例

```http
DELETE /Role/Consultant
Authorization: Bearer {jwt_token}
```

### 回應範例

**成功回應:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "依PK刪除成功: Consultant",
  "data": "Consultant",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此資料:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此資料,欄位:RoleId,值:Consultant",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 此資源已被使用:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4003,
  "returnMessage": "此資源已被使用,欄位:RoleId,值:Admin",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Role/DeleteRoleById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Examples.cs | `Modules/Auth/Role/DeleteRoleById/Examples.cs` | Swagger 範例定義 |
| Auth_Role.cs | `Infrastructures/Data/Entities/Auth_Role.cs` | 角色資料庫實體定義 |
| Auth_User_Role.cs | `Infrastructures/Data/Entities/Auth_User_Role.cs` | 使用者角色關聯資料庫實體定義 |
| Auth_Role_Router_Action.cs | `Infrastructures/Data/Entities/Auth_Role_Router_Action.cs` | 角色權限關聯資料庫實體定義 |
