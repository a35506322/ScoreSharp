# InsertRoleAuthById API - 新增角色權限 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Role/{roleId}` |
| **HTTP 方法** | POST |
| **功能** | 新增單筆角色權限 - 批次設定角色的路由與操作權限 |
| **位置** | `Modules/Auth/Role/InsertRoleAuthById/Endpoint.cs` |

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
// 位置: Modules/Auth/Role/InsertRoleAuthById/Models.cs (Line 3-22)
public class InsertRoleAuthByIdRequest
{
    [Required]
    public string RoleId { get; set; }    // 關聯 Auth_Role

    [Required]
    public string RouterId { get; set; }   // 關聯 Auth_Router

    [Required]
    public string ActionId { get; set; }   // 關聯 Auth_Action
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `RoleId` | string | ✅ | 角色主鍵,必須與路由參數 roleId 一致,關聯 Auth_Role 表 |
| `RouterId` | string | ✅ | 路由主鍵,關聯 Auth_Router 表 |
| `ActionId` | string | ✅ | 操作主鍵,關聯 Auth_Action 表,且必須與 RouterId 對應 |

### 重要提示

1. **路由參數一致性**: URL 中的 `roleId` 必須與 Request Body 中每筆資料的 `RoleId` 完全一致
2. **ActionId 與 RouterId 關聯**: 每個 ActionId 必須屬於對應的 RouterId,系統會驗證 Auth_Action 表中的關聯性

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "新增成功: Admin",
  "data": "Admin",
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 格式驗證失敗 (400 Bad Request - Return Code: 4000)
- 請求格式不符合定義
- 缺少必填欄位

```json
{
  "returnCode": 4000,
  "returnMessage": "格式驗證失敗",
  "data": {
    "RoleId": ["RoleId 為必填欄位"]
  }
}
```

#### Router RoleId 不符合 (400 Bad Request - Return Code: 4003)
- URL 路由參數 roleId 與 Request Body 中的 RoleId 不一致

```json
{
  "returnCode": 4003,
  "returnMessage": "Router RoleId 不符合,請檢查",
  "data": null
}
```

#### 查無此 RoleId (400 Bad Request - Return Code: 4001)
- RoleId 不存在於 Auth_Role 表中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料,欄位:RoleId,值:Admin",
  "data": null
}
```

#### ActionId 與 RouterId 不符合 (400 Bad Request - Return Code: 4003)
- ActionId 與 RouterId 的關聯不正確
- ActionId 在 Auth_Action 表中對應的 RouterId 與請求中的 RouterId 不一致

```json
{
  "returnCode": 4003,
  "returnMessage": "ActionId 與 RoleId 不符合,請檢查",
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
  - RoleId: 必填
  - RouterId: 必填
  - ActionId: 必填

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 57-102)

#### 檢查點 1: Request Body 資料去重
```
位置: Line 59-62
使用 JSON 序列化/反序列化去除重複資料
├─ 將每筆 InsertRoleAuthByIdRequest 序列化為 JSON 字串
├─ 使用 Distinct() 去除重複的 JSON 字串
└─ 反序列化回 InsertRoleAuthByIdRequest 物件
```

#### 檢查點 2: 路由參數與 Request Body RoleId 一致性
```
位置: Line 64-68
檢查 URL 路由參數 roleId 是否與所有 Request Body 中的 RoleId 一致
├─ 若有任何一筆 RoleId 與路由參數不一致 → 拋出 BusinessLogicFailed 錯誤 (Return Code: 4003)
│  └─ 訊息: "Router RoleId 不符合,請檢查"
└─ 若全部一致 → 繼續執行
```

#### 檢查點 3: RoleId 存在性檢查
```
位置: Line 70-72
查詢 Auth_Role 表確認 RoleId 是否存在
├─ 若查無資料 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料,欄位:RoleId,值:{roleId}"
└─ 若有資料 → 繼續執行
```

#### 檢查點 4: ActionId 與 RouterId 關聯性檢查
```
位置: Line 74-87
驗證每個 ActionId 是否屬於對應的 RouterId
├─ 查詢 Auth_Action 表,取得所有啟用的 Action (IsActive = 'Y')
├─ 建立 ActionId -> RouterId 的對應字典
├─ 逐筆檢查 Request 中的 ActionId 與 RouterId 是否對應
├─ 若有任何一筆不對應 → 拋出 BusinessLogicFailed 錯誤 (Return Code: 4003)
│  └─ 訊息: "ActionId 與 RoleId 不符合,請檢查"
└─ 若全部對應正確 → 繼續執行
```

---

## 資料處理

### 1. 去重處理 (重要功能)
- **位置**: Line 59-62
- **處理邏輯**:
  ```csharp
  var insertRequest = request.insertRoleAuthByIdRequests
      .Select(x => JsonSerializer.Serialize(x))  // 序列化為 JSON
      .Distinct()                                 // 去除重複
      .Select(x => JsonSerializer.Deserialize<InsertRoleAuthByIdRequest>(x));  // 反序列化
  ```
- **目的**: 避免前端傳入重複的權限設定,確保資料唯一性

### 2. 資料庫查詢
- **位置**: Line 70, Line 74, Line 91
- **查詢 1**: 檢查 RoleId 是否存在
  ```csharp
  await _context.Auth_Role.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RoleId == routerWithRoleId)
  ```
- **查詢 2**: 取得所有啟用的 Action
  ```csharp
  await _context.Auth_Action.AsNoTracking()
      .Where(x => x.IsActive == "Y")
      .ToListAsync()
  ```
- **查詢 3**: 取得 Role 資料準備更新
  ```csharp
  await _context.Auth_Role.SingleOrDefaultAsync(x => x.RoleId == request.roleId)
  ```

### 3. 資料轉換與批次操作
- **位置**: Line 89-97
- **轉換邏輯**:
  ```csharp
  // 1. 使用 AutoMapper 轉換為實體
  var entities = _mapper.Map<IEnumerable<Auth_Role_Router_Action>>(insertRequest);

  // 2. 更新角色的修改時間與修改人
  entity.UpdateTime = DateTime.Now;
  entity.UpdateUserId = _jwthelper.UserId;

  // 3. 批次刪除該角色的所有舊權限 (重要: 先刪除後新增,確保資料一致性)
  await _context.Auth_Role_Router_Action
      .Where(x => x.RoleId == routerWithRoleId)
      .ExecuteDeleteAsync();

  // 4. 批次新增新的權限設定
  await _context.Auth_Role_Router_Action.AddRangeAsync(entities);

  // 5. 儲存變更
  await _context.SaveChangesAsync();
  ```

### 4. 快取清除 (重要功能)
- **位置**: Line 99
- **清除邏輯**:
  ```csharp
  await _fusionCache.RemoveByTagAsync(SecurityConstants.PolicyRedisTag.RoleAction);
  ```
- **目的**: 清除 RoleAction 相關的快取,確保權限變更立即生效

### 5. 處理邏輯流程
```
接收請求
│
├─ 步驟 1: 去重處理 (Line 59-62)
│  └─ 使用 JSON 序列化/反序列化去除重複資料
│
├─ 步驟 2: 檢查路由參數與 Request Body RoleId 一致性 (Line 64-68)
│  ├─ 不一致 → 回傳錯誤 4003
│  └─ 一致 → 繼續
│
├─ 步驟 3: 檢查 RoleId 是否存在 (Line 70-72)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 4: 驗證 ActionId 與 RouterId 關聯性 (Line 74-87)
│  ├─ 有錯誤 → 回傳錯誤 4003
│  └─ 全部正確 → 繼續
│
├─ 步驟 5: 資料轉換與批次操作 (Line 89-97)
│  ├─ 轉換為實體
│  ├─ 更新 Role 修改時間與修改人
│  ├─ 批次刪除舊權限 (ExecuteDeleteAsync)
│  ├─ 批次新增新權限 (AddRangeAsync)
│  └─ 儲存變更 (SaveChangesAsync)
│
├─ 步驟 6: 清除快取 (Line 99)
│  └─ 清除 RoleAction 相關快取
│
└─ 步驟 7: 回傳成功訊息 (Line 101)
   └─ Return Code: 2000, Data: RoleId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Role` | SELECT, UPDATE | 角色主檔 - 檢查存在性及更新修改時間 |
| `Auth_Action` | SELECT | 操作主檔 - 驗證 ActionId 與 RouterId 關聯性 |
| `Auth_Role_Router_Action` | DELETE, INSERT | 角色權限關聯表 - 批次刪除舊權限並新增新權限 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢 1: 檢查 RoleId 存在性**
- **位置**: Line 70
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_Role.AsNoTracking()
      .SingleOrDefaultAsync(x => x.RoleId == routerWithRoleId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Role]
  WHERE [RoleId] = @RoleId
  ```

**查詢 2: 取得所有啟用的 Action**
- **位置**: Line 74
- **方法**: `ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var actions = await _context.Auth_Action.AsNoTracking()
      .Where(x => x.IsActive == "Y")
      .ToListAsync();
  ```
- **等效 SQL**:
  ```sql
  SELECT *
  FROM [dbo].[Auth_Action]
  WHERE [IsActive] = 'Y'
  ```

**查詢 3: 取得 Role 資料**
- **位置**: Line 91
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

#### 2. 批次刪除操作 (DELETE)

**重要: ExecuteDeleteAsync - EF Core 7 批次刪除功能**
- **位置**: Line 95
- **方法**: `ExecuteDeleteAsync()`
- **EF Core 程式碼**:
  ```csharp
  await _context.Auth_Role_Router_Action
      .Where(x => x.RoleId == routerWithRoleId)
      .ExecuteDeleteAsync();
  ```
- **等效 SQL**:
  ```sql
  DELETE FROM [dbo].[Auth_Role_Router_Action]
  WHERE [RoleId] = @RoleId
  ```
- **優點**: 直接在資料庫執行刪除,不需先查詢再刪除,效能更佳

#### 3. 更新操作 (UPDATE)

- **位置**: Line 91-93
- **方法**: 直接修改實體屬性 + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  entity.UpdateTime = DateTime.Now;
  entity.UpdateUserId = _jwthelper.UserId;
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  UPDATE [dbo].[Auth_Role]
  SET [UpdateTime] = @UpdateTime,
      [UpdateUserId] = @UpdateUserId
  WHERE [RoleId] = @RoleId
  ```

#### 4. 批次新增操作 (INSERT)

- **位置**: Line 96
- **方法**: `AddRangeAsync()`
- **EF Core 程式碼**:
  ```csharp
  await _context.Auth_Role_Router_Action.AddRangeAsync(entities);
  ```
- **等效 SQL**:
  ```sql
  INSERT INTO [dbo].[Auth_Role_Router_Action]
      ([RoleId], [RouterId], [ActionId])
  VALUES
      (@RoleId1, @RouterId1, @ActionId1),
      (@RoleId2, @RouterId2, @ActionId2),
      ...
  ```

### Auth_Role_Router_Action 資料表結構

| 欄位名稱 | 資料型別 | 允許NULL | 說明 |
|---------|---------|---------|------|
| `RoleId` | string(50) | ❌ | 主鍵 - 角色識別碼 (FK to Auth_Role) |
| `RouterId` | string(50) | ❌ | 主鍵 - 路由識別碼 (FK to Auth_Router) |
| `ActionId` | string(50) | ❌ | 主鍵 - 操作識別碼 (FK to Auth_Action) |

**複合主鍵**: (RoleId, RouterId, ActionId)

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 POST /Role/{roleId} 請求
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證必填欄位
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯驗證與處理 (Handler.Handle)
│  │
│  ├─ 步驟 1: 去重處理 (Line 59-62)
│  │  └─ JSON 序列化/反序列化去除重複資料
│  │
│  ├─ 步驟 2: 檢查路由參數一致性 (Line 64-68)
│  │  ├─ RoleId 不一致 → 400 Bad Request (Return Code: 4003)
│  │  └─ 一致 → 繼續
│  │
│  ├─ 步驟 3: 檢查 RoleId 存在性 (Line 70-72)
│  │  ├─ 不存在 → 400 Bad Request (Return Code: 4001)
│  │  └─ 存在 → 繼續
│  │
│  ├─ 步驟 4: 驗證 ActionId 與 RouterId 關聯性 (Line 74-87)
│  │  ├─ 查詢所有啟用的 Action
│  │  ├─ 建立 ActionId -> RouterId 字典
│  │  ├─ 逐筆驗證關聯性
│  │  ├─ 有錯誤 → 400 Bad Request (Return Code: 4003)
│  │  └─ 全部正確 → 繼續
│  │
│  ├─ 步驟 5: 資料轉換與批次操作 (Line 89-97)
│  │  ├─ AutoMapper 轉換為實體
│  │  ├─ 更新 Role 的修改時間與修改人
│  │  ├─ 批次刪除該角色的所有舊權限 (ExecuteDeleteAsync)
│  │  ├─ 批次新增新的權限設定 (AddRangeAsync)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  ├─ 步驟 6: 清除快取 (Line 99)
│  │  └─ 清除 RoleAction 相關快取標籤
│  │
│  └─ 步驟 7: 回傳成功回應 (Line 101)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "新增成功: {RoleId}"
│        └─ Data: RoleId
```

---

## 關鍵業務規則

### 1. 路由參數一致性驗證
- URL 路由參數 `roleId` 必須與 Request Body 中所有資料的 `RoleId` 一致
- 這是為了防止前端錯誤或惡意請求,確保修改的是正確的角色
- 若不一致會立即拋出錯誤,不進行後續處理

### 2. ActionId 與 RouterId 關聯性驗證
- 每個 ActionId 必須屬於對應的 RouterId
- 系統會查詢 Auth_Action 表建立對應字典進行驗證
- 只有啟用的 Action (IsActive = 'Y') 才會被納入驗證
- 確保權限設定的正確性,避免權限錯誤

### 3. 去重處理機制
- 使用 JSON 序列化/反序列化方式去除重複資料
- 避免前端傳入重複的權限設定
- 確保最終新增的資料唯一性
- 減少不必要的資料庫操作

### 4. 批次刪除後新增策略
- **先刪除該角色的所有舊權限,再批次新增新權限**
- 使用 EF Core 7 的 `ExecuteDeleteAsync()` 提升效能
- 這種策略確保權限設定的完整性與一致性
- 避免複雜的新增/更新/刪除判斷邏輯

### 5. 快取清除機制
- 權限變更後立即清除 RoleAction 相關快取
- 使用 `RemoveByTagAsync()` 清除帶有特定標籤的所有快取
- 確保權限變更立即生效,不會因快取導致延遲
- 對於權限系統至關重要,避免安全漏洞

### 6. 審計追蹤
- 更新 Auth_Role 的 UpdateTime 與 UpdateUserId
- 記錄權限變更的時間與操作人
- 確保完整的操作追蹤記錄

### 7. 交易一致性
- 所有資料庫操作在同一個 DbContext 中執行
- SaveChangesAsync() 會將所有變更包裝在交易中
- 確保批次刪除、更新、新增的原子性
- 若任何步驟失敗,整個交易會回滾

---

## 請求範例

### 請求範例 (來自 Examples.cs Line 3-24)

```json
POST /Role/Admin
Content-Type: application/json
Authorization: Bearer {jwt_token}

[
  {
    "roleId": "Admin",
    "routerId": "SetUpBillDay",
    "actionId": "GetBillDayById"
  },
  {
    "roleId": "Admin",
    "routerId": "SetUpBillDay",
    "actionId": "GetBillDayByQueryString"
  }
]
```

### 回應範例

**成功回應:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "新增成功: Admin",
  "data": "Admin",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - Router RoleId 不符合:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4003,
  "returnMessage": "Router RoleId 不符合,請檢查",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此 RoleId:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此資料,欄位:RoleId,值:Admin",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - ActionId 與 RouterId 不符合:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4003,
  "returnMessage": "ActionId 與 RoleId 不符合,請檢查",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Role/InsertRoleAuthById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Role/InsertRoleAuthById/Models.cs` | 請求模型定義 |
| Examples.cs | `Modules/Auth/Role/InsertRoleAuthById/Examples.cs` | Swagger 範例定義 |
| Auth_Role.cs | `Infrastructures/Data/Entities/Auth_Role.cs` | 角色資料庫實體定義 |
| Auth_Action.cs | `Infrastructures/Data/Entities/Auth_Action.cs` | 操作資料庫實體定義 |
| Auth_Role_Router_Action.cs | `Infrastructures/Data/Entities/Auth_Role_Router_Action.cs` | 角色權限關聯資料庫實體定義 |
