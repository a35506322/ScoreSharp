# DeleteRouterById API - 刪除路由 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Router/{routerId}` |
| **HTTP 方法** | DELETE |
| **功能** | 刪除單筆路由資料 - 根據 RouterId 刪除指定路由 (僅限未被使用的路由) |
| **位置** | `Modules/Auth/Router/DeleteRouterById/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token (用於驗證使用者身份) |

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `routerId` | string | ✅ | 路由主鍵,要刪除的路由識別碼 |

### 請求範例

```
DELETE /Router/SetUpBillDay
Authorization: Bearer {jwt_token}
```

或完整路由 (來自 Endpoint.cs Line 14):

```
DELETE /Router/DeleteRouterById/SetUpBillDay
Authorization: Bearer {jwt_token}
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "刪除成功: SetUpBillDay",
  "data": "SetUpBillDay",
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- 指定的 RouterId 不存在於資料庫中

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此資料: 刪除的ID",
  "data": null,
  "traceId": "{traceId}"
}
```

#### 此資源已被使用 (400 Bad Request - Return Code: 4003)
- 該路由已被 Auth_Action 表引用,不允許刪除
- 確保資料完整性,避免破壞關聯性

```json
{
  "returnCode": 4003,
  "returnMessage": "此資源已被使用: SetUpBillDay",
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
- **位置**: ASP.NET Core Model Binding (自動執行)
- **驗證內容**:
  - routerId: 從路由參數自動綁定,必填

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 50-66)

#### 檢查點 1: RouterId 存在性檢查
```
位置: Line 52-55
查詢 Auth_Router 表中是否存在指定的 RouterId
├─ 若不存在 (single is null) → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此資料: {routerId}"
└─ 若存在 → 繼續執行
```

#### 檢查點 2: 關聯性檢查 (防止刪除被使用的資源)
```
位置: Line 57-60
檢查 Auth_Action 表中是否有引用此 RouterId 的記錄
├─ 若存在引用 (isExist = true) → 拋出此資源已被使用錯誤 (Return Code: 4003)
│  └─ 訊息: "此資源已被使用: {routerId}"
└─ 若無引用 → 允許刪除
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 52, Line 57
- **查詢 1**: 檢查 RouterId 是否存在
  ```csharp
  await _context.Auth_Router.SingleOrDefaultAsync(x => x.RouterId == request.routerId)
  ```
- **查詢 2**: 檢查是否被 Auth_Action 引用
  ```csharp
  await _context.Auth_Action.AnyAsync(x => x.RouterId == request.routerId)
  ```

### 2. 處理邏輯
```
接收請求 DELETE /Router/{routerId}
│
├─ 步驟 1: 檢查 RouterId 是否存在 (Line 52-55)
│  ├─ 查詢 Auth_Router 表
│  │
│  ├─ 若不存在 → 回傳錯誤 4001 (查無此資料)
│  └─ 若存在 → 繼續
│
├─ 步驟 2: 檢查關聯性 (Line 57-60)
│  ├─ 查詢 Auth_Action 表是否有引用
│  │
│  ├─ 若有引用 → 回傳錯誤 4003 (此資源已被使用)
│  └─ 若無引用 → 繼續
│
├─ 步驟 3: 刪除資料 (Line 62-63)
│  ├─ 標記實體為刪除狀態: Remove(single)
│  ├─ 儲存變更: SaveChangesAsync()
│  │
│  ├─ 成功 → 繼續
│  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│
└─ 步驟 4: 回傳成功回應 (Line 65)
   └─ 200 OK (Return Code: 2000)
      ├─ Message: "刪除成功: {routerId}"
      └─ Data: routerId
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Router` | SELECT, DELETE | 路由主檔 - 查詢資料是否存在及刪除資料 |
| `Auth_Action` | SELECT | 操作主檔 - 檢查是否有引用該路由 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢 1: 檢查 RouterId 是否存在**
- **位置**: Line 52
- **方法**: `SingleOrDefaultAsync()`
- **EF Core 程式碼**:
  ```csharp
  var single = await _context.Auth_Router.SingleOrDefaultAsync(x => x.RouterId == request.routerId);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_Router]
  WHERE [RouterId] = @routerId
  ```
- **說明**: 使用 SingleOrDefaultAsync() 確保只回傳一筆或 null

**查詢 2: 檢查關聯性**
- **位置**: Line 57
- **方法**: `AnyAsync()`
- **EF Core 程式碼**:
  ```csharp
  var isExist = await _context.Auth_Action.AnyAsync(x => x.RouterId == request.routerId);
  ```
- **等效 SQL**:
  ```sql
  SELECT CASE
      WHEN EXISTS (
          SELECT 1
          FROM [dbo].[Auth_Action]
          WHERE [RouterId] = @routerId
      )
      THEN CAST(1 AS bit)
      ELSE CAST(0 AS bit)
  END
  ```
- **說明**:
  - 使用 AnyAsync() 檢查是否存在引用
  - 效能優於 Count(),因為只要找到一筆就立即返回
  - 回傳 true 表示有引用,false 表示無引用

#### 2. 刪除操作 (DELETE)

- **位置**: Line 62-63
- **方法**: `Remove()` + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  _context.Auth_Router.Remove(single);
  await _context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  DELETE FROM [dbo].[Auth_Router]
  WHERE [RouterId] = @routerId
  ```
- **說明**:
  - Remove() 標記實體為刪除狀態
  - SaveChangesAsync() 執行實際的資料庫刪除操作
  - 這是 EF Core 的物理刪除,資料會從資料庫中永久移除

### Auth_Router 資料表結構

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

### Auth_Action 資料表關聯

| 欄位名稱 | 資料型別 | 說明 |
|---------|---------|------|
| `ActionId` | string | 主鍵 - 操作識別碼 |
| `RouterId` | string | 外鍵 - 關聯到 Auth_Router.RouterId |
| ... | ... | 其他欄位 |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 DELETE /Router/{routerId} 請求
│
├─ ASP.NET Core Model Binding
│  └─ 從路由參數綁定 routerId
│
├─ 商業邏輯驗證 (Handler.Handle)
│  │
│  ├─ 步驟 1: 檢查 RouterId 是否存在 (Line 52-55)
│  │  ├─ 查詢 Auth_Router 表
│  │  │
│  │  ├─ single is null (資料不存在)
│  │  │  └─ 回傳 400 Bad Request (Return Code: 4001)
│  │  │     ├─ Message: "查無此資料: {routerId}"
│  │  │     └─ Data: null
│  │  │
│  │  └─ single is not null (資料存在) → 繼續
│  │
│  ├─ 步驟 2: 檢查關聯性 (Line 57-60)
│  │  ├─ 查詢 Auth_Action 表
│  │  │  └─ AnyAsync(x => x.RouterId == routerId)
│  │  │
│  │  ├─ isExist = true (有引用)
│  │  │  └─ 回傳 400 Bad Request (Return Code: 4003)
│  │  │     ├─ Message: "此資源已被使用: {routerId}"
│  │  │     └─ Data: null
│  │  │
│  │  └─ isExist = false (無引用) → 繼續
│  │
│  ├─ 步驟 3: 刪除資料 (Line 62-63)
│  │  ├─ _context.Auth_Router.Remove(single)
│  │  ├─ await _context.SaveChangesAsync()
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 步驟 4: 回傳成功回應 (Line 65)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "刪除成功: {routerId}"
│        └─ Data: routerId
```

---

## 關鍵業務規則

### 1. 級聯刪除限制
- 不允許刪除已被 Auth_Action 引用的路由
- 必須先刪除所有引用該路由的 Action,才能刪除路由
- 這是資料完整性的重要保護機制

### 2. 物理刪除
- 使用 EF Core Remove() 執行物理刪除
- 資料會從資料庫中永久移除,無法恢復
- 不是軟刪除 (Soft Delete),沒有使用 IsDeleted 欄位

### 3. 兩階段驗證
- 第一階段: 檢查資料是否存在
- 第二階段: 檢查是否有關聯性引用
- 確保刪除操作的安全性

### 4. 關聯性檢查效能優化
- 使用 AnyAsync() 而非 Count()
- AnyAsync() 只要找到一筆就立即返回,效能更好
- 不需要知道引用的數量,只需要知道是否有引用

### 5. 錯誤訊息明確性
- 查無資料: 明確告知哪個 RouterId 不存在
- 資源已被使用: 明確告知哪個 RouterId 被引用
- 幫助前端或管理員快速定位問題

### 6. RESTful 設計
- 使用 HTTP DELETE 方法刪除資源
- 路由參數直接指定要刪除的資源 ID
- 成功回傳 200 OK 而非 204 No Content,因為有回傳資料

---

## 使用場景

### 1. 路由管理 - 刪除測試路由
- 管理員在測試完成後刪除測試用路由
- 必須確保該路由沒有被任何 Action 使用
- 保持系統路由清單的整潔

### 2. 路由管理 - 移除過時路由
- 系統功能調整後,某些路由不再需要
- 刪除前需要檢查是否有 Action 依賴
- 避免破壞現有功能的權限設定

### 3. 錯誤處理 - 資源已被使用
- 嘗試刪除被使用的路由時,系統提示錯誤
- 管理員需要先處理 Action 的引用
- 確保資料完整性不被破壞

### 4. 清理無效資料
- 刪除建立錯誤的路由資料
- 僅限未被使用的路由
- 保持資料庫的資料品質

---

## 刪除流程建議

### 安全刪除路由的步驟

```
準備刪除路由
│
├─ 步驟 1: 檢查關聯性
│  └─ 查詢 Auth_Action 是否有引用
│     ├─ 有引用 → 需要先處理 Action
│     └─ 無引用 → 可以安全刪除
│
├─ 步驟 2: 處理關聯的 Action (若有)
│  ├─ 方案 1: 刪除相關的 Action
│  ├─ 方案 2: 將 Action 關聯到其他 Router
│  └─ 方案 3: 停用路由而非刪除 (修改 IsActive = 'N')
│
├─ 步驟 3: 執行刪除
│  └─ 呼叫 DELETE /Router/{routerId}
│
└─ 步驟 4: 驗證結果
   ├─ 成功 → 路由已刪除
   └─ 失敗 → 檢查錯誤訊息並修正
```

---

## 替代方案: 軟刪除

### 為什麼可以考慮軟刪除

目前此 API 執行的是物理刪除,資料會永久移除。在某些情況下,軟刪除可能是更好的選擇:

**軟刪除的優點:**
- 可以恢復誤刪的資料
- 保留歷史記錄供稽核
- 不需要處理級聯刪除問題

**軟刪除的實作方式:**
- 新增 IsDeleted 欄位 (預設為 'N')
- 刪除時設定 IsDeleted = 'Y'
- 查詢時過濾 IsDeleted = 'N' 的資料

**目前採用物理刪除的原因:**
- 資料庫設計中沒有 IsDeleted 欄位
- 使用 IsActive = 'N' 可達到類似效果 (停用而非刪除)
- 簡化資料庫結構和查詢邏輯

---

## 請求範例

### 請求範例

```
DELETE /Router/SetUpBillDay
Authorization: Bearer {jwt_token}
```

### 回應範例

**成功回應:** (來自 Examples.cs Line 15-18)
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "刪除成功: 刪除的ID",
  "data": "刪除的ID",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此資料:** (來自 Examples.cs Line 6-9)
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

**失敗回應 - 此資源已被使用:** (來自 Examples.cs Line 24-27)
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4003,
  "returnMessage": "此資源已被使用: SetUpBillDay",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 注意事項

### 1. 不可逆操作
- 刪除後資料無法恢復
- 執行前應該再次確認
- 建議前端加入確認對話框

### 2. 權限控制
- 刪除操作應該有嚴格的權限控制
- 建議只允許管理員執行
- 可以考慮記錄刪除操作的審計日誌

### 3. 級聯影響
- 刪除路由可能影響使用者權限
- 需要評估對系統的影響
- 建議在非營運時間執行

### 4. 替代方案
- 考慮使用 IsActive = 'N' 停用而非刪除
- 停用可以隨時恢復
- 保留歷史資料供稽核

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Router/DeleteRouterById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Examples.cs | `Modules/Auth/Router/DeleteRouterById/Examples.cs` | Swagger 範例定義 |
| Auth_Router.cs | `Infrastructures/Data/Entities/Auth_Router.cs` | 資料庫實體定義 |
| Auth_Action.cs | `Infrastructures/Data/Entities/Auth_Action.cs` | 操作實體定義 (關聯檢查) |
