# GetRolesByQueryString API - 查詢多筆角色 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Role` |
| **HTTP 方法** | GET |
| **功能** | 查詢多筆角色資料 - 支援依啟用狀態篩選 |
| **位置** | `Modules/Auth/Role/GetRolesByQueryString/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token |

### 查詢參數 (Query String)

```csharp
// 位置: Modules/Auth/Role/GetRolesByQueryString/Models.cs (Line 3-11)
public class GetRolesByQueryStringRequest
{
    /// <summary>
    /// 是否啟用,Y|N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }  // 選填,篩選條件
}
```

### 參數說明表格

| 欄位 | 型別 | 必填 | 驗證規則 | 說明 |
|------|------|------|----------|------|
| `IsActive` | string | ❌ | [YN] | 是否啟用,只能是 Y 或 N,選填 |

### 範例查詢字串

```
?IsActive=Y              # 只查詢啟用的角色
?IsActive=N              # 只查詢停用的角色
(不帶參數)                # 查詢所有角色
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "成功",
  "data": [
    {
      "roleId": "Reviewer",
      "roleName": "徵審人員",
      "isActive": "Y",
      "addUserId": "ADMIN",
      "addTime": "2024-01-01T00:00:00",
      "updateUserId": "ADMIN",
      "updateTime": "2024-01-01T00:00:00"
    }
  ],
  "traceId": "{traceId}"
}
```

### Response 資料結構

```csharp
// 位置: Modules/Auth/Role/GetRolesByQueryString/Models.cs (Line 13-49)
public class GetRolesByQueryStringResponse
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

#### 格式驗證失敗 (400 Bad Request - Return Code: 4000)
- IsActive 參數格式不符合 [YN] 規則

```json
{
  "returnCode": 4000,
  "returnMessage": "格式驗證失敗",
  "data": {
    "IsActive": ["IsActive 必須符合正則表達式 [YN]"]
  },
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
- **位置**: ASP.NET Core Model Validation (自動執行)
- **驗證內容**:
  - IsActive: 選填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 51-62)
- **說明**: 無特殊商業邏輯驗證,直接執行查詢

---

## 資料處理

### 1. 資料庫查詢 (動態條件查詢)
- **位置**: Line 55-57
- **查詢方法**: EF Core LINQ with Conditional Where
- **程式碼**:
  ```csharp
  var entities = await _context.Auth_Role
      .Where(x => string.IsNullOrEmpty(dto.IsActive) || x.IsActive == dto.IsActive)
      .ToListAsync();
  ```
- **動態查詢邏輯**:
  ```
  IF IsActive 參數為空或 null
  THEN 不套用 IsActive 篩選條件 (查詢所有)
  ELSE 套用 IsActive 篩選條件 (查詢符合的)
  ```

### 2. 資料轉換
- **位置**: Line 59
- **轉換邏輯**:
  ```csharp
  var result = _mapper.Map<List<GetRolesByQueryStringResponse>>(entities);
  ```
- **說明**: 使用 AutoMapper 將 Auth_Role 實體集合轉換為 Response DTO 集合

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 解析查詢參數 (Line 53)
│  └─ 取得 IsActive 參數值 (可能為 null)
│
├─ 步驟 2: 執行動態條件查詢 (Line 55-57)
│  ├─ 若 IsActive 為 null/empty → 不篩選,查詢所有角色
│  └─ 若 IsActive 有值 → 篩選該狀態的角色
│
├─ 步驟 3: 資料轉換 (Line 59)
│  └─ AutoMapper 轉換為 Response DTO 集合
│
└─ 步驟 4: 回傳成功回應 (Line 61)
   └─ Return Code: 2000, Data: List<GetRolesByQueryStringResponse>
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Role` | SELECT | 角色主檔 - 查詢多筆資料 |

### 操作類型

#### 1. 查詢操作 (SELECT) - 動態條件

**查詢: 依 IsActive 動態篩選角色**
- **位置**: Line 55-57
- **方法**: `Where()` + `ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var entities = await _context.Auth_Role
      .Where(x => string.IsNullOrEmpty(dto.IsActive) || x.IsActive == dto.IsActive)
      .ToListAsync();
  ```
- **等效 SQL (當 IsActive 有值時)**:
  ```sql
  SELECT *
  FROM [dbo].[Auth_Role]
  WHERE [IsActive] = @IsActive
  ```
- **等效 SQL (當 IsActive 為空時)**:
  ```sql
  SELECT *
  FROM [dbo].[Auth_Role]
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
用戶發送 GET /Role?IsActive={value} 請求
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證 IsActive 格式 [YN]
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯處理 (Handler.Handle)
│  │
│  ├─ 解析查詢參數 (Line 53)
│  │  └─ 取得 IsActive 參數
│  │
│  ├─ 執行動態條件查詢 (Line 55-57)
│  │  ├─ 檢查 IsActive 是否有值
│  │  ├─ 若無值 → 查詢所有角色
│  │  ├─ 若有值 → 只查詢該狀態的角色
│  │  └─ ToListAsync() 執行查詢
│  │
│  ├─ 資料轉換 (Line 59)
│  │  └─ AutoMapper 轉換為 Response DTO
│  │
│  └─ 回傳成功回應 (Line 61)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "成功"
│        └─ Data: List<GetRolesByQueryStringResponse>
```

---

## 關鍵業務規則

### 1. 動態查詢條件
- IsActive 參數為選填
- 若不提供 IsActive 參數,查詢所有角色
- 若提供 IsActive 參數,只查詢符合該狀態的角色
- 實現彈性的資料篩選功能

### 2. 查詢邏輯實作方式
- 使用 LINQ 的條件運算子實作動態查詢
- `string.IsNullOrEmpty(dto.IsActive) || x.IsActive == dto.IsActive`
- 當參數為空時,前半條件為 true,整個 OR 條件成立,不篩選
- 當參數有值時,前半條件為 false,檢查後半條件,套用篩選
- 簡潔高效的動態查詢寫法

### 3. 完整審計資訊回傳
- 回傳包含 AddUserId, AddTime, UpdateUserId, UpdateTime
- 讓前端可以顯示完整的建立與修改記錄
- 適合用於管理畫面的資料列表

### 4. AutoMapper 批次轉換
- 使用 AutoMapper 批次轉換實體集合
- 分離資料庫實體與 API 回應結構
- 遵循分層架構設計原則

### 5. 無分頁機制
- 此 API 回傳所有符合條件的資料
- 未實作分頁功能
- 適用於角色數量有限的情境
- 若資料量大,建議後續加入分頁機制

### 6. IsActive 狀態篩選
- 'Y': 查詢啟用的角色
- 'N': 查詢停用的角色
- 不提供: 查詢所有角色
- 常用於下拉選單或清單頁面

### 7. 效能考量
- 查詢所有欄位 (SELECT *)
- 未使用 AsNoTracking() (因為預設不追蹤)
- 若資料量大,建議:
  - 使用 AsNoTracking() 提升效能
  - 只選取必要欄位
  - 實作分頁機制

---

## 請求範例

### 請求範例 1: 查詢啟用的角色

```http
GET /Role?IsActive=Y
Authorization: Bearer {jwt_token}
```

### 請求範例 2: 查詢停用的角色

```http
GET /Role?IsActive=N
Authorization: Bearer {jwt_token}
```

### 請求範例 3: 查詢所有角色

```http
GET /Role
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
  "data": [
    {
      "roleId": "Reviewer",
      "roleName": "徵審人員",
      "isActive": "Y",
      "addUserId": "ADMIN",
      "addTime": "2024-01-01T00:00:00",
      "updateUserId": "ADMIN",
      "updateTime": "2024-01-01T00:00:00"
    }
  ],
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

**成功回應 - 查無資料:**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "成功",
  "data": [],
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
| Endpoint.cs | `Modules/Auth/Role/GetRolesByQueryString/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Role/GetRolesByQueryString/Models.cs` | 請求與回應模型定義 |
| Example.cs | `Modules/Auth/Role/GetRolesByQueryString/Example.cs` | Swagger 範例定義 |
| Auth_Role.cs | `Infrastructures/Data/Entities/Auth_Role.cs` | 資料庫實體定義 |
