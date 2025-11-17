# DeleteReviewerPermissionById API - 刪除徵審權限 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerPermission/{seqNo}` |
| **HTTP 方法** | DELETE |
| **功能** | 刪除單筆徵審權限資料 - 根據 SeqNo 永久刪除指定的徵審權限設定 |
| **位置** | `Modules/Auth/ReviewerPermission/DeleteReviewerPermissionById/Endpoint.cs` |

---

## Request 定義

### 路由參數 (Route Parameters)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `seqNo` | int | ✅ | 徵審權限主鍵 (PK),用於識別要刪除的資料 |

### 請求範例

```
DELETE /ReviewerPermission/5
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "刪除成功: 2",
  "data": "2",
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 查無此資料 (400 Bad Request - Return Code: 4001)
- 指定的 SeqNo 不存在於資料庫中
- 可能已被刪除或從未存在

```json
{
  "returnCode": 4001,
  "returnMessage": "查無此ID: 30010",
  "data": null
}
```

#### 內部程式失敗 (500 Internal Server Error - Return Code: 5000)
- 系統內部處理錯誤

#### 資料庫執行失敗 (500 Internal Server Error - Return Code: 5002)
- 資料庫操作失敗
- 可能原因: 外鍵約束違反 (若有其他表引用此權限設定)

---

## 驗證資料

### 1. 路由參數驗證
- **位置**: ASP.NET Core 路由系統 (自動執行)
- **驗證內容**:
  - seqNo: 必須為有效的整數

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 38-51)

#### 檢查點: 資料存在性檢查
```
位置: Line 42-45
查詢 Auth_ReviewerPermission 表中是否存在指定的 SeqNo
├─ 若不存在 → 拋出 NotFound 錯誤 (Return Code: 4001)
│  └─ 訊息: "查無此ID: {SeqNo}"
└─ 若存在 → 繼續執行刪除
```

---

## 資料處理

### 1. 資料庫查詢
- **位置**: Line 42
- **查詢**: 根據 SeqNo 查詢要刪除的實體
  ```csharp
  var single = await context.Auth_ReviewerPermission.SingleOrDefaultAsync(x => x.SeqNo == seqNo);
  ```

### 2. 資料刪除
- **位置**: Line 47-48
- **刪除邏輯**: 使用 EF Core 的 Remove 方法
  ```csharp
  context.Auth_ReviewerPermission.Remove(single);
  await context.SaveChangesAsync();
  ```

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 驗證路由參數 seqNo 為有效整數
│  └─ 由 ASP.NET Core 路由系統自動處理
│
├─ 步驟 2: 查詢資料是否存在 (Line 42)
│  └─ 使用有追蹤的查詢 (不使用 AsNoTracking)
│
├─ 步驟 3: 檢查資料是否存在 (Line 44-45)
│  ├─ 不存在 → 回傳錯誤 4001
│  └─ 存在 → 繼續
│
├─ 步驟 4: 標記為刪除 (Line 47)
│  └─ 呼叫 Remove() 標記實體為 Deleted 狀態
│
├─ 步驟 5: 儲存變更至資料庫 (Line 48)
│  └─ 呼叫 SaveChangesAsync() 執行實際刪除
│
└─ 步驟 6: 回傳成功訊息 (Line 50)
   └─ Return Code: 2000, Data: SeqNo
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_ReviewerPermission` | SELECT, DELETE | 徵審權限主檔 - 查詢及刪除資料 |

### 操作類型

#### 1. 查詢操作 (SELECT)

**查詢: 根據 SeqNo 查詢資料**
- **位置**: Line 42
- **方法**: `SingleOrDefaultAsync()` (不使用 AsNoTracking)
- **EF Core 程式碼**:
  ```csharp
  var single = await context.Auth_ReviewerPermission.SingleOrDefaultAsync(x => x.SeqNo == seqNo);
  ```
- **等效 SQL**:
  ```sql
  SELECT TOP(2) *
  FROM [dbo].[Auth_ReviewerPermission]
  WHERE [SeqNo] = @SeqNo
  ```

#### 2. 刪除操作 (DELETE)

- **位置**: Line 47-48
- **方法**: `Remove()` + `SaveChangesAsync()`
- **EF Core 程式碼**:
  ```csharp
  context.Auth_ReviewerPermission.Remove(single);
  await context.SaveChangesAsync();
  ```
- **等效 SQL**:
  ```sql
  DELETE FROM [dbo].[Auth_ReviewerPermission]
  WHERE [SeqNo] = @SeqNo
  ```

### 變更追蹤機制
- 查詢時不使用 `AsNoTracking()`,讓 EF Core 追蹤實體
- `Remove()` 標記實體為 Deleted 狀態
- `SaveChangesAsync()` 時產生 DELETE SQL 語句
- 確保刪除操作的正確性

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 DELETE /ReviewerPermission/{seqNo} 請求
│
├─ ASP.NET Core 路由系統
│  ├─ 驗證 seqNo 為有效整數
│  │
│  ├─ 驗證失敗 → 400 Bad Request
│  └─ 驗證通過 → 繼續
│
├─ Handler.Handle 處理
│  │
│  ├─ 資料庫查詢 (Line 42)
│  │  ├─ 使用有追蹤的查詢
│  │  └─ 根據 SeqNo 查詢單筆資料
│  │
│  ├─ 檢查資料是否存在 (Line 44-45)
│  │  ├─ 查無資料 → 400 Bad Request (Return Code: 4001)
│  │  └─ 有資料 → 繼續
│  │
│  ├─ 標記為刪除 (Line 47)
│  │  └─ Remove(single) - 設定實體狀態為 Deleted
│  │
│  ├─ 儲存變更至資料庫 (Line 48)
│  │  ├─ SaveChangesAsync() - 執行 DELETE SQL
│  │  │
│  │  ├─ 成功 → 繼續
│  │  └─ 失敗 → 500 Internal Server Error (Return Code: 5002)
│  │
│  └─ 回傳成功回應 (Line 50)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "刪除成功: {SeqNo}"
│        └─ Data: SeqNo
```

---

## 關鍵業務規則

### 1. 永久刪除
- 此 API 執行的是永久刪除 (Hard Delete)
- 資料會從資料庫中完全移除
- 無法復原,請謹慎使用
- 沒有實作軟刪除 (Soft Delete) 機制

### 2. 刪除前檢查存在性
- 刪除前必須先確認資料是否存在
- 若資料不存在,回傳明確的錯誤訊息
- 避免靜默失敗,確保操作的明確性
- 符合 RESTful API 的設計慣例

### 3. 冪等性
- 此 API 不具有完全的冪等性
- 第一次呼叫: 成功刪除,回傳 2000
- 第二次呼叫: 資料已不存在,回傳 4001
- 雖然結果狀態相同 (資料不存在),但回應碼不同

### 4. 查詢時不使用 AsNoTracking
- 與 Get API 不同,這裡不使用 `AsNoTracking()`
- 因為需要 EF Core 追蹤實體狀態
- `Remove()` 需要追蹤的實體才能正確標記為 Deleted
- 這是刪除操作的必要條件

### 5. 外鍵約束考量
- 若其他表引用此徵審權限設定,刪除可能失敗
- 資料庫會拋出外鍵約束違反錯誤
- API 會回傳 Return Code 5002 (資料庫執行失敗)
- 建議在刪除前檢查是否有關聯資料

### 6. 安全性考量
- 刪除操作風險較高,應該加入額外的權限控制
- 建議只允許管理員執行刪除
- 可考慮加入二次確認機制
- 記錄刪除操作的審計日誌

### 7. 業務影響
刪除徵審權限設定可能產生的影響:
- **正在使用的案件**: 若有案件正在使用此權限設定,可能導致權限判斷失敗
- **歷史紀錄**: 無法回溯查看已刪除的權限設定
- **系統穩定性**: 刪除核心權限設定可能影響系統正常運作

### 8. 替代方案建議
考慮實作軟刪除機制:
- 加入 `IsDeleted` 欄位
- 刪除時只標記為已刪除,不實際刪除
- 查詢時過濾已刪除的資料
- 保留歷史紀錄,可以復原
- 降低誤刪除的風險

---

## 請求與回應範例

### 請求範例

```
DELETE /ReviewerPermission/2
```

### 回應範例

**成功回應 (來自 Examples.cs Line 12-18):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "刪除成功: 2",
  "data": "2",
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 查無此資料:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 4001,
  "returnMessage": "查無此ID: 30010",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**失敗回應 - 外鍵約束違反 (假設):**
```json
HTTP/1.1 500 Internal Server Error
Content-Type: application/json

{
  "returnCode": 5002,
  "returnMessage": "資料庫執行失敗",
  "data": null,
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 安全性建議

### 1. 權限控制
```csharp
// 建議加入權限檢查
[Authorize(Roles = "Admin")]
public async Task<IResult> DeleteReviewerPermissionById([FromRoute] int seqNo)
{
    // ...
}
```

### 2. 關聯檢查
```csharp
// 建議在刪除前檢查是否有關聯資料
var hasRelatedData = await context.SomeRelatedTable
    .AnyAsync(x => x.ReviewerPermissionSeqNo == seqNo);

if (hasRelatedData)
{
    return ApiResponseHelper.Error("此權限設定正在使用中,無法刪除");
}
```

### 3. 審計日誌
```csharp
// 建議記錄刪除操作
await auditLogger.LogAsync(new AuditLog
{
    Action = "DeleteReviewerPermission",
    UserId = currentUserId,
    TargetId = seqNo.ToString(),
    Timestamp = DateTime.Now
});
```

### 4. 軟刪除實作
```csharp
// 建議改為軟刪除
entity.IsDeleted = true;
entity.DeletedUserId = currentUserId;
entity.DeletedTime = DateTime.Now;

await context.SaveChangesAsync();
```

---

## 未來改進建議

### 1. 實作軟刪除
- 保留歷史紀錄
- 支援復原功能
- 降低誤刪風險

### 2. 加入關聯檢查
- 檢查是否有案件使用此權限
- 防止刪除正在使用的權限設定
- 提供更明確的錯誤訊息

### 3. 批次刪除
```csharp
[HttpDelete]
public async Task<IResult> DeleteReviewerPermissions([FromBody] int[] seqNos)
{
    // 批次刪除多筆資料
}
```

### 4. 級聯刪除策略
- 定義清晰的級聯刪除規則
- 或禁止刪除有關聯的資料
- 確保資料完整性

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/ReviewerPermission/DeleteReviewerPermissionById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Example.cs | `Modules/Auth/ReviewerPermission/DeleteReviewerPermissionById/Example.cs` | Swagger 範例定義 |
| Auth_ReviewerPermission.cs | `Infrastructures/Data/Entities/Auth_ReviewerPermission.cs` | 資料庫實體定義 |
