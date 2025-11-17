# DeleteRouterCategoryById API - 刪除單筆路由類別 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/RouterCategory/{routerCategoryId}` |
| **HTTP 方法** | DELETE |
| **功能** | 刪除單筆路由類別資料 - 刪除前會檢查是否有 Router 使用此 RouterCategory |
| **位置** | `Modules/Auth/RouterCategory/DeleteRouterCategoryById/Endpoint.cs` |

---

## Request 定義

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `routerCategoryId` | string | ✅ | 路由類別主鍵 |

### 範例路由

```
/RouterCategory/DeleteRouterCategoryById/SetUp
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "刪除成功: SetUp",
  "data": null,
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- RouterCategoryId 不存在於資料庫中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料: SetUp",
  "data": null,
  "traceId": "{traceId}"
}
```

#### 此資源已被使用 (400 Bad Request - Return Code: 4003)
- 有 Router 正在使用此 RouterCategory,無法刪除

```json
{
  "returnCode": 4003,
  "returnMessage": "此資源已被使用: SetUp",
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

### 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 49-65)

#### 檢查點 1: RouterCategoryId 存在性檢查
```
位置: Line 51-54
檢查 Auth_RouterCategory 表中是否存在該 RouterCategoryId
├─ 若不存在 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料: {RouterCategoryId}"
└─ 若存在 → 繼續執行
```

#### 檢查點 2: Router 使用狀況檢查
```
位置: Line 56-59
檢查 Auth_Router 表中是否有 Router 使用此 RouterCategoryId
├─ 若有使用 → 拋出此資源已被使用錯誤 (Return Code: 4003)
│  └─ 訊息: "此資源已被使用: {RouterCategoryId}"
│  └─ 說明: 無法刪除正在被使用的 RouterCategory
└─ 若無使用 → 繼續執行
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 51, Line 56
- **查詢 1**: 檢查 RouterCategoryId 是否存在
  ```csharp
  var single = await _context.Auth_RouterCategory.SingleOrDefaultAsync(x => x.RouterCategoryId == request.RouterCategoryId);
  ```
- **查詢 2**: 檢查是否有 Router 使用此 RouterCategory
  ```csharp
  var isExist = await _context.Auth_Router.AnyAsync(x => x.RouterCategoryId == request.RouterCategoryId);
  ```

### 2. 處理邏輯
```
接收請求
│
├─ 步驟 1: 檢查 RouterCategoryId 是否存在 (Line 51-54)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 2: 檢查是否有 Router 使用此 RouterCategory (Line 56-59)
│  ├─ 有使用 → 回傳錯誤 4003
│  └─ 無使用 → 繼續
│
├─ 步驟 3: 刪除資料 (Line 61-62)
│  └─ 呼叫 Remove() 和 SaveChangesAsync()
│
└─ 步驟 4: 回傳成功訊息 (Line 64)
   └─ Return Code: 2000, Message: "刪除成功: {RouterCategoryId}"
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_RouterCategory` | SELECT, DELETE | 路由類別主檔 - 查詢及刪除資料 |
| `Auth_Router` | SELECT | 路由主檔 - 檢查關聯性 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢 1: 檢查 RouterCategoryId 存在性**
- **位置**: Line 51
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

**查詢 2: 檢查 Router 使用狀況**
- **位置**: Line 56
- **方法**: `AnyAsync()`
- **EF Core 程式碼**:
  ```csharp
  var isExist = await _context.Auth_Router.AnyAsync(x => x.RouterCategoryId == request.RouterCategoryId);
  ```
- **等效 SQL**:
  ```sql
  SELECT CASE
      WHEN EXISTS (
          SELECT 1
          FROM [dbo].[Auth_Router]
          WHERE [RouterCategoryId] = @RouterCategoryId
      )
      THEN CAST(1 AS BIT)
      ELSE CAST(0 AS BIT)
  END
  ```

#### 2. 刪除操作 (DELETE)

- **位置**: Line 61-62
- **方法**: `Remove()` + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  _context.Auth_RouterCategory.Remove(single);
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  DELETE FROM [dbo].[Auth_RouterCategory]
  WHERE [RouterCategoryId] = @RouterCategoryId
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

### Auth_Router 資料表結構 (關聯表)

| 欄位名稱 | 資料型別 | 允許NULL | 說明 |
|---------|---------|---------|------|
| `RouterId` | string(50) | ❌ | 主鍵 - 路由識別碼 |
| `RouterName` | string(30) | ❌ | 路由名稱 |
| `DynamicParams` | string(100) | ✅ | 動態參數 |
| `IsActive` | string(1) | ❌ | 是否啟用 (Y/N) |
| `AddUserId` | string | ❌ | 新增員工編號 |
| `AddTime` | DateTime | ❌ | 新增時間 |
| `UpdateUserId` | string | ✅ | 修改員工編號 |
| `UpdateTime` | DateTime | ✅ | 修改時間 |
| `RouterCategoryId` | string(50) | ❌ | 路由類別主鍵 (FK) |
| `Icon` | string(15) | ✅ | Icon 名稱 |
| `Sort` | int | ❌ | 排序 |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 DELETE /RouterCategory/{routerCategoryId} 請求
│
├─ 商業邏輯驗證 (Handler.Handle)
│  │
│  ├─ 檢查 RouterCategoryId 存在性 (Line 51-54)
│  │  ├─ 不存在 → 400 Bad Request (Return Code: 4001)
│  │  │  └─ Message: "查無此資料: {RouterCategoryId}"
│  │  └─ 存在 → 繼續
│  │
│  ├─ 檢查 Router 使用狀況 (Line 56-59)
│  │  ├─ 查詢 Auth_Router 表
│  │  │  └─ 條件: RouterCategoryId = {request.RouterCategoryId}
│  │  │
│  │  ├─ 有使用 → 400 Bad Request (Return Code: 4003)
│  │  │  └─ Message: "此資源已被使用: {RouterCategoryId}"
│  │  │  └─ 說明: 無法刪除正在被使用的 RouterCategory
│  │  └─ 無使用 → 繼續
│  │
│  ├─ 刪除資料 (Line 61-62)
│  │  ├─ Remove(single)
│  │  ├─ SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 回傳成功回應 (Line 64)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "刪除成功: {RouterCategoryId}"
│        └─ Data: null
```

---

## 關鍵業務規則

### 1. RouterCategoryId 存在性驗證
- 刪除前必須先確認該 RouterCategoryId 存在於資料庫中
- 若不存在則拋出 NotFound 錯誤
- 避免對不存在的資料執行刪除操作

### 2. 參照完整性檢查 (關鍵規則)
- **刪除前必須檢查是否有 Router 使用此 RouterCategory**
- 檢查方法: 查詢 Auth_Router 表中是否有資料的 RouterCategoryId 等於要刪除的 RouterCategoryId
- 若有任何 Router 使用此 RouterCategory,則拋出 "此資源已被使用" 錯誤
- 目的:
  - 維護資料完整性
  - 避免 Foreign Key 參照錯誤
  - 防止刪除後導致 Router 資料孤立或無效
- 這是一種軟體層級的參照完整性檢查 (類似資料庫的 Foreign Key Constraint)

### 3. 刪除順序
```
必須遵循以下順序:
1. 先刪除所有使用此 RouterCategory 的 Router
2. 再刪除 RouterCategory
```

### 4. 使用 AnyAsync 進行存在性檢查
- 使用 `AnyAsync()` 而非 `CountAsync()` 或 `FirstOrDefaultAsync()`
- 優點:
  - 只要找到第一筆符合條件的資料即停止查詢
  - 效能較佳
  - 適合用於存在性檢查

### 5. EF Core Remove 操作
- 使用 EF Core 的 `Remove()` 方法標記實體為刪除狀態
- 必須呼叫 `SaveChangesAsync()` 才會真正執行 DELETE SQL
- 這是 EF Core 的 Unit of Work 模式

### 6. 錯誤訊息清晰
- "查無此資料" (4001): 資料不存在
- "此資源已被使用" (4003): 資料存在但無法刪除
- 兩種錯誤訊息清楚區分不同的失敗原因
- 幫助前端或管理員快速理解問題

### 7. 刪除操作無需審計追蹤
- 直接刪除資料,不記錄刪除者和刪除時間
- 若需要審計追蹤,應考慮改用 "軟刪除" (Soft Delete) 方式:
  - 新增 IsDeleted 欄位
  - 刪除時只更新 IsDeleted = 'Y' 而非實際刪除資料
  - 查詢時過濾 IsDeleted = 'N' 的資料

---

## 請求範例

### 請求範例

```http
DELETE /RouterCategory/SetUp
```

### 回應範例

**成功回應 (來自 Examples.cs Line 15-18):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "刪除成功: 刪除的ID",
  "data": null,
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
  "returnMessage": "此資源已被使用: SetUp",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 使用場景說明

### 場景 1: 成功刪除

```
假設資料庫狀態:
- Auth_RouterCategory 中存在 RouterCategoryId = "TestCategory"
- Auth_Router 中沒有任何 Router 使用 "TestCategory"

執行 DELETE /RouterCategory/TestCategory

結果:
→ 200 OK (Return Code: 2000)
→ "TestCategory" 從 Auth_RouterCategory 表中被刪除
```

### 場景 2: 資料不存在

```
假設資料庫狀態:
- Auth_RouterCategory 中不存在 RouterCategoryId = "NonExistent"

執行 DELETE /RouterCategory/NonExistent

結果:
→ 400 Bad Request (Return Code: 4001)
→ "查無此資料: NonExistent"
→ 不執行任何刪除操作
```

### 場景 3: 資源已被使用

```
假設資料庫狀態:
- Auth_RouterCategory 中存在 RouterCategoryId = "SetUp"
- Auth_Router 中有 3 個 Router 使用 "SetUp"
  - Router 1: RouterId = "SetUpBillDay", RouterCategoryId = "SetUp"
  - Router 2: RouterId = "SetUpPayment", RouterCategoryId = "SetUp"
  - Router 3: RouterId = "SetUpNotify", RouterCategoryId = "SetUp"

執行 DELETE /RouterCategory/SetUp

結果:
→ 400 Bad Request (Return Code: 4003)
→ "此資源已被使用: SetUp"
→ 不執行任何刪除操作
→ 需要先刪除使用 "SetUp" 的 3 個 Router,才能刪除 "SetUp" RouterCategory
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/RouterCategory/DeleteRouterCategoryById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Examples.cs | `Modules/Auth/RouterCategory/DeleteRouterCategoryById/Examples.cs` | Swagger 範例定義 |
| Auth_RouterCategory.cs | `Infrastructures/Data/Entities/Auth_RouterCategory.cs` | 路由類別資料庫實體定義 |
| Auth_Router.cs | `Infrastructures/Data/Entities/Auth_Router.cs` | 路由資料庫實體定義 |
