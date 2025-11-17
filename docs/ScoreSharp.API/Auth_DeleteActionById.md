# DeleteActionById API - 刪除操作 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Action/{actionId}` |
| **HTTP 方法** | DELETE |
| **功能** | 刪除單筆操作資料 By Id - 檢查關聯使用情況後刪除 API 操作設定 |
| **位置** | `Modules/Auth/Action/DeleteActionById/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token |

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `actionId` | string | ✅ | 操作主鍵,用於指定要刪除的操作 |

### 請求範例

```
DELETE /Action/GetBillDayByQueryString
Authorization: Bearer {jwt_token}
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "刪除成功: GetBillDayByQueryString",
  "data": "GetBillDayByQueryString",
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- ActionId 不存在於資料庫中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料: 刪除的ID",
  "data": null,
  "traceId": "{traceId}"
}
```

#### 此資源已被使用 (400 Bad Request - Return Code: 4003)
- 該操作已被角色使用 (存在於 Auth_Role_Router_Action 表中)
- 為保持資料一致性,不允許刪除

```json
{
  "returnCode": 4003,
  "returnMessage": "此資源已被使用: GetBillDayByQueryString",
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

### 1. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 51-71)

#### 檢查點 1: ActionId 存在性檢查
```
位置: Line 53-56
檢查 Auth_Action 表中是否存在指定的 ActionId
├─ 若不存在 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料: {actionId}"
└─ 若存在 → 繼續執行
```

#### 檢查點 2: 關聯使用情況檢查
```
位置: Line 58-63
查詢 Auth_Role_Router_Action 表中是否有角色使用此操作
├─ 步驟 1: 取得所有被使用的 ActionId (Line 58)
│  └─ SELECT DISTINCT ActionId FROM Auth_Role_Router_Action
├─ 步驟 2: 檢查當前 ActionId 是否在使用清單中 (Line 60)
│  └─ 使用 Contains() 方法判斷
├─ 若已被使用 → 拋出此資源已被使用錯誤 (Return Code: 4003)
│  └─ 訊息: "此資源已被使用: {actionId}"
└─ 若未被使用 → 繼續執行刪除
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 53, Line 58
- **查詢 1**: 檢查 ActionId 是否存在
  ```csharp
  await _context.Auth_Action.SingleOrDefaultAsync(x => x.ActionId == request.actionId)
  ```
- **查詢 2**: 取得所有被角色使用的操作 ID
  ```csharp
  await _context.Auth_Role_Router_Action.Select(x => x.ActionId).Distinct().ToListAsync()
  ```

### 2. 關聯檢查
- **位置**: Line 60
- **檢查邏輯**:
  ```csharp
  var isExist = allActionWithRouterAndRole.Contains(request.actionId);
  ```
- **說明**: 使用 Contains() 方法檢查當前 ActionId 是否在被使用的清單中

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查 ActionId 是否存在 (Line 53-56)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 2: 查詢所有被使用的操作 ID (Line 58)
│  └─ 從 Auth_Role_Router_Action 表中取得 DISTINCT ActionId
│
├─ 步驟 3: 檢查當前操作是否被使用 (Line 60-63)
│  ├─ 已被使用 → 回傳錯誤 4003
│  └─ 未被使用 → 繼續
│
├─ 步驟 4: 刪除操作 (Line 65-66)
│  └─ 呼叫 Remove() 和 SaveChangesAsync()
│
├─ 步驟 5: 清除快取 (Line 68)
│  └─ 移除 FusionCache 中的 Action 快取
│
└─ 步驟 6: 回傳成功訊息 (Line 70)
   └─ Return Code: 2000, Data: actionId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Action` | SELECT, DELETE | 操作主檔 - 查詢存在性檢查及刪除資料 |
| `Auth_Role_Router_Action` | SELECT | 角色路由操作關聯表 - 檢查是否有角色使用此操作 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢 1: 檢查 ActionId 是否存在**
- **位置**: Line 53
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

**查詢 2: 取得所有被使用的操作 ID**
- **位置**: Line 58
- **方法**: `Select().Distinct().ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var allActionWithRouterAndRole = await _context.Auth_Role_Router_Action
      .Select(x => x.ActionId)
      .Distinct()
      .ToListAsync();
  ```
- **等效 SQL**:
  ```sql
  SELECT DISTINCT [ActionId]
  FROM [dbo].[Auth_Role_Router_Action]
  ```
- **說明**:
  - 使用 DISTINCT 去除重複的 ActionId
  - 返回所有被角色使用的操作 ID 清單
  - 用於檢查當前要刪除的操作是否被使用

#### 2. 刪除操作 (DELETE)

- **位置**: Line 65-66
- **方法**: `Remove()` + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  _context.Auth_Action.Remove(single);
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  DELETE FROM [dbo].[Auth_Action]
  WHERE [ActionId] = @ActionId
  ```
- **說明**:
  - 使用 EF Core 的 Remove() 方法標記實體為刪除狀態
  - SaveChangesAsync() 執行實際的資料庫刪除操作
  - 只有在通過所有檢查後才執行刪除

#### 3. 快取操作 (Cache Remove)

- **位置**: Line 68
- **方法**: `FusionCache.RemoveAsync()`
- **程式碼**:
  ```csharp
  await _fusionCache.RemoveAsync($"{SecurityConstants.PolicyRedisKey.Action}:{request.actionId}");
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

### Auth_Role_Router_Action 資料表說明

| 欄位名稱 | 資料型別 | 說明 |
|---------|---------|------|
| `RoleId` | string | 角色主鍵 (FK) |
| `RouterId` | string | 路由主鍵 (FK) |
| `ActionId` | string | 操作主鍵 (FK) |

**關聯說明**:
- Auth_Role_Router_Action 是角色-路由-操作的多對多關聯表
- 記錄哪些角色可以存取哪些路由的哪些操作
- 刪除操作前必須確認沒有角色使用此操作

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 DELETE /Action/{actionId} 請求
│
├─ 商業邏輯驗證 (Handler.Handle)
│  │
│  ├─ 檢查 ActionId 是否存在 (Line 53-56)
│  │  ├─ 不存在 → 400 Bad Request (Return Code: 4001)
│  │  │  └─ 訊息: "查無此資料: {actionId}"
│  │  └─ 存在 → 繼續
│  │
│  ├─ 查詢被使用的操作清單 (Line 58)
│  │  └─ 從 Auth_Role_Router_Action 取得 DISTINCT ActionId
│  │
│  ├─ 檢查當前操作是否被使用 (Line 60-63)
│  │  ├─ 已被使用 (Contains 返回 true) → 400 Bad Request (Return Code: 4003)
│  │  │  └─ 訊息: "此資源已被使用: {actionId}"
│  │  └─ 未被使用 → 繼續
│  │
│  ├─ 刪除操作 (Line 65-66)
│  │  ├─ Remove(single)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  ├─ 清除快取 (Line 68)
│  │  └─ FusionCache.RemoveAsync(cacheKey)
│  │
│  └─ 回傳成功回應 (Line 70)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "刪除成功: {actionId}"
│        └─ Data: actionId
```

---

## 關鍵業務規則

### 1. ActionId 存在性檢查
- 刪除前必須確認 ActionId 存在於資料庫中
- 若不存在則拋出錯誤 (Return Code: 4001)
- 防止刪除不存在的資料

### 2. 關聯使用情況檢查
- 刪除前必須檢查是否有角色使用此操作
- 查詢 Auth_Role_Router_Action 表,取得所有被使用的 ActionId
- 若當前操作已被角色使用,則不允許刪除
- 拋出錯誤: "此資源已被使用" (Return Code: 4003)

### 3. 資料一致性保護
- 透過關聯檢查確保資料庫的參照完整性
- 避免刪除被使用的操作,導致角色權限設定錯誤
- 維持系統權限管理的一致性

### 4. 使用 DISTINCT 優化
- 使用 DISTINCT 去除重複的 ActionId
- 提升查詢效能,減少不必要的重複資料
- 使用 Contains() 方法快速判斷是否存在

### 5. 刪除操作流程
- 使用 EF Core 的 Remove() 方法標記實體為刪除狀態
- SaveChangesAsync() 執行實際的資料庫刪除操作
- 只有在通過所有檢查後才執行刪除

### 6. 快取一致性
- 刪除操作後,會清除 FusionCache 中對應的快取
- 快取鍵格式: `{SecurityConstants.PolicyRedisKey.Action}:{actionId}`
- 確保快取資料與資料庫資料一致
- 防止查詢到已刪除的操作資料

### 7. 錯誤訊息設計
- 查無此資料: 返回 4001,明確告知資料不存在
- 此資源已被使用: 返回 4003,告知無法刪除的原因
- 提供清晰的錯誤訊息,方便前端處理

### 8. RESTful API 設計
- DELETE /Action/{actionId} 用於刪除單筆資料
- 遵循 RESTful API 設計原則
- 路由參數直接指定要刪除的資源 ID

### 9. 安全性考量
- 刪除操作需要 JWT Token 驗證
- 確保只有授權使用者可以執行刪除
- 透過關聯檢查防止誤刪重要資料

### 10. 建議的刪除流程
- 前端應在刪除前提示使用者確認
- 若操作已被使用,建議先移除關聯再刪除
- 或考慮使用軟刪除 (將 IsActive 設為 'N') 而非實體刪除

---

## 請求範例

### 請求範例 (來自 Endpoint.cs Line 13)

```
DELETE /Action/GetBillDayByQueryString
Authorization: Bearer {jwt_token}
```

### 回應範例

**成功回應 (來自 Examples.cs Line 15-18):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "刪除成功: GetBillDayByQueryString",
  "data": "GetBillDayByQueryString",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此資料 (來自 Examples.cs Line 6-9):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此資料: 刪除的ID",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 此資源已被使用 (來自 Examples.cs Line 24-27):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4003,
  "returnMessage": "此資源已被使用: GetBillDayByQueryString",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Action/DeleteActionById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Examples.cs | `Modules/Auth/Action/DeleteActionById/Examples.cs` | Swagger 範例定義 |
| Auth_Action.cs | `Infrastructures/Data/Entities/Auth_Action.cs` | 操作資料庫實體定義 |
| Auth_Role_Router_Action.cs | `Infrastructures/Data/Entities/Auth_Role_Router_Action.cs` | 角色路由操作關聯表實體定義 |

---

## 補充說明

### 何時可以刪除操作?

操作可以被刪除的條件:
1. 操作存在於 Auth_Action 表中
2. 操作未被任何角色使用 (Auth_Role_Router_Action 表中沒有此 ActionId 的記錄)

### 若操作已被使用,如何處理?

若需要刪除已被使用的操作,建議採取以下步驟:
1. 先從 Auth_Role_Router_Action 表中移除所有關聯此操作的記錄
2. 確認沒有角色使用此操作後,再執行刪除
3. 或考慮使用軟刪除,將 IsActive 設為 'N' 而非實體刪除

### 關聯檢查的重要性

- 防止刪除被使用的操作,導致角色權限設定錯誤
- 維持資料庫的參照完整性
- 避免前端權限檢查失敗
- 確保系統權限管理的一致性

### 快取清除的時機

- 刪除操作成功後,立即清除對應的快取
- 確保其他使用者無法從快取中查詢到已刪除的操作
- 維持快取與資料庫的一致性
