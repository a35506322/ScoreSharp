# GetActionsByQueryString API - 查詢多筆操作 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Action` |
| **HTTP 方法** | GET |
| **功能** | 查詢多筆操作資料 - 支援 IsCommon, IsActive, RouterId 多條件動態篩選 |
| **位置** | `Modules/Auth/Action/GetActionsByQueryString/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `Authorization` | string | ✅ | JWT Token |

### 查詢參數 (Query Parameters)

```csharp
// 位置: Modules/Auth/Action/GetActionsByQueryString/Models.cs (Line 3-25)
public class GetActionByQueryStringRequest
{
    [Display(Name = "是否是通用資料")]
    [RegularExpression("[YN]")]
    public string? IsCommon { get; set; }  // Y/N，如果是Y 不檢查權限

    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    public string? IsActive { get; set; }  // Y/N

    [Display(Name = "路由PK")]
    [MaxLength(50)]
    public string? RouterId { get; set; }  // 關聯Auth_Router
}
```

| 欄位 | 型別 | 必填 | 最大長度 | 驗證規則 | 說明 |
|------|------|------|----------|----------|------|
| `IsCommon` | string | ❌ | - | [YN] | 是否是通用資料,Y 表示不檢查權限 |
| `IsActive` | string | ❌ | - | [YN] | 是否啟用,只能是 Y 或 N |
| `RouterId` | string | ❌ | 50 | 無 | 路由主鍵,關聯 Auth_Router |

### 請求範例

```
GET /Action?IsActive=Y&RouterId=SetUpBillDay
Authorization: Bearer {jwt_token}
```

**查詢範例說明 (來自 Endpoint.cs Line 13):**
- 查詢啟用的操作: `?IsActive=Y`
- 查詢指定路由的操作: `?RouterId=SetUpBillDay`
- 組合查詢: `?IsActive=Y&RouterId=SetUpBillDay`
- 查詢通用操作: `?IsCommon=Y`
- 查詢所有操作: 不帶任何參數

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "查詢成功",
  "data": [
    {
      "actionId": "GetRouterCatregoriesByQueryString",
      "actionName": "查詢路由類別ByQueryString",
      "isCommon": "Y",
      "isActive": "Y",
      "addUserId": "admin",
      "addTime": "2025-01-15T10:30:00",
      "updateUserId": "admin",
      "updateTime": "2025-01-15T14:20:00",
      "routerId": "SetUpRouterCategory"
    }
  ],
  "traceId": "{traceId}"
}
```

### 回應欄位說明

```csharp
// 位置: Modules/Auth/Action/GetActionsByQueryString/Models.cs (Line 27-73)
public class GetActionsByQueryStringResponse
{
    public string ActionId { get; set; }        // 英數字，API Action 名稱
    public string ActionName { get; set; }      // 中文，前端顯示功能
    public string IsCommon { get; set; }        // Y/N，如果是Y 不檢查權限
    public string IsActive { get; set; }        // Y/N
    public string AddUserId { get; set; }       // 新增員工
    public DateTime AddTime { get; set; }       // 新增時間
    public string? UpdateUserId { get; set; }   // 修正員工
    public DateTime? UpdateTime { get; set; }   // 修正時間
    public string RouterId { get; set; }        // 關聯Auth_Router
}
```

| 欄位 | 型別 | 允許NULL | 說明 |
|------|------|---------|------|
| `actionId` | string | ❌ | API Action 識別碼 |
| `actionName` | string | ❌ | API Action 中文名稱 |
| `isCommon` | string | ❌ | 是否是通用資料 (Y/N) |
| `isActive` | string | ❌ | 是否啟用 (Y/N) |
| `addUserId` | string | ❌ | 新增員工編號 |
| `addTime` | DateTime | ❌ | 新增時間 |
| `updateUserId` | string | ✅ | 修改員工編號 |
| `updateTime` | DateTime | ✅ | 修改時間 |
| `routerId` | string | ❌ | 路由主鍵 |

### 錯誤回應

#### 格式驗證失敗 (400 Bad Request - Return Code: 4000)
- 查詢參數格式不符合定義
- 欄位格式不符合驗證規則

```json
{
  "returnCode": 4000,
  "returnMessage": "格式驗證失敗",
  "data": {
    "IsActive": ["IsActive 必須符合正則表達式 [YN]"]
  }
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
  - IsCommon: 選填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)
  - IsActive: 選填, 必須符合正則表達式 `[YN]` (只能是 Y 或 N)
  - RouterId: 選填, 最大長度 50 字元

### 2. 商業邏輯處理
- **位置**: `Handle()` 方法 (Endpoint.cs Line 50-70)
- **說明**: 此 API 不需要特殊的商業邏輯驗證,主要執行動態查詢

---

## 資料處理

### 1. 動態查詢建構
- **位置**: Line 54-65
- **查詢邏輯**:
  ```csharp
  var entities = await _context.Auth_Action.AsNoTracking()
      .Where(x =>
          string.IsNullOrEmpty(getActionByQueryStringRequest.IsCommon) ||
          x.IsCommon == getActionByQueryStringRequest.IsCommon
      )
      .Where(x =>
          string.IsNullOrEmpty(getActionByQueryStringRequest.IsActive) ||
          x.IsActive == getActionByQueryStringRequest.IsActive
      )
      .Where(x =>
          string.IsNullOrEmpty(getActionByQueryStringRequest.RouterId) ||
          x.RouterId == getActionByQueryStringRequest.RouterId
      )
      .ToListAsync();
  ```
- **說明**:
  - 使用多個 Where 子句實現動態查詢
  - 每個參數都是選填的,若為 null 或空字串則不加入篩選條件
  - 使用 OR 邏輯: `參數為空 OR 欄位等於參數值`

### 2. 資料轉換
- **位置**: Line 67
- **轉換邏輯**:
  ```csharp
  var result = _mapper.Map<List<GetActionsByQueryStringResponse>>(entities);
  ```
- **說明**: 使用 AutoMapper 將 Auth_Action 實體列表轉換為 Response DTO 列表

### 3. 處理邏輯
```
接收請求
│
├─ 步驟 1: 建構動態查詢 (Line 54-65)
│  ├─ 使用 AsNoTracking() 提升查詢效能
│  ├─ 條件 1: IsCommon (若有提供)
│  ├─ 條件 2: IsActive (若有提供)
│  └─ 條件 3: RouterId (若有提供)
│
├─ 步驟 2: 執行查詢 (Line 65)
│  └─ ToListAsync() 取得所有符合條件的資料
│
├─ 步驟 3: 資料轉換 (Line 67)
│  └─ 使用 AutoMapper 轉換為 Response 列表
│
└─ 步驟 4: 回傳成功訊息 (Line 69)
   └─ Return Code: 2000, Data: List<GetActionsByQueryStringResponse>
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Action` | SELECT | 操作主檔 - 查詢多筆資料 |

### 操作類型

#### 1. 動態查詢操作 (SELECT)

**查詢: 根據多條件動態查詢**
- **位置**: Line 54-65
- **方法**: `AsNoTracking().Where().ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var entities = await _context.Auth_Action.AsNoTracking()
      .Where(x =>
          string.IsNullOrEmpty(getActionByQueryStringRequest.IsCommon) ||
          x.IsCommon == getActionByQueryStringRequest.IsCommon
      )
      .Where(x =>
          string.IsNullOrEmpty(getActionByQueryStringRequest.IsActive) ||
          x.IsActive == getActionByQueryStringRequest.IsActive
      )
      .Where(x =>
          string.IsNullOrEmpty(getActionByQueryStringRequest.RouterId) ||
          x.RouterId == getActionByQueryStringRequest.RouterId
      )
      .ToListAsync();
  ```

- **等效 SQL (所有參數都提供時)**:
  ```sql
  SELECT *
  FROM [dbo].[Auth_Action]
  WHERE [IsCommon] = @IsCommon
    AND [IsActive] = @IsActive
    AND [RouterId] = @RouterId
  ```

- **等效 SQL (只提供 IsActive 和 RouterId)**:
  ```sql
  SELECT *
  FROM [dbo].[Auth_Action]
  WHERE [IsActive] = @IsActive
    AND [RouterId] = @RouterId
  ```

- **等效 SQL (不提供任何參數)**:
  ```sql
  SELECT *
  FROM [dbo].[Auth_Action]
  ```

- **說明**:
  - 使用 AsNoTracking() 因為是唯讀查詢,提升效能
  - 動態查詢會根據提供的參數自動組合 WHERE 條件
  - 若參數為 null 或空字串,該條件不會加入 SQL
  - 可以靈活組合不同的查詢條件

### 動態查詢範例

| 查詢參數 | 產生的 WHERE 條件 | 說明 |
|---------|------------------|------|
| 無參數 | 無 WHERE 條件 | 返回所有操作 |
| IsActive=Y | WHERE IsActive = 'Y' | 返回所有啟用的操作 |
| RouterId=SetUpBillDay | WHERE RouterId = 'SetUpBillDay' | 返回指定路由的操作 |
| IsActive=Y&RouterId=SetUpBillDay | WHERE IsActive = 'Y' AND RouterId = 'SetUpBillDay' | 返回指定路由且啟用的操作 |
| IsCommon=Y&IsActive=Y | WHERE IsCommon = 'Y' AND IsActive = 'Y' | 返回通用且啟用的操作 |

### Auth_Action 資料表結構

| 欄位名稱 | 資料型別 | 允許NULL | 說明 | 可篩選 |
|---------|---------|---------|------|--------|
| `ActionId` | string(100) | ❌ | 主鍵 - API Action 識別碼 | ❌ |
| `ActionName` | string(30) | ❌ | API Action 名稱 | ❌ |
| `IsCommon` | string(1) | ❌ | 是否是通用資料 (Y/N) | ✅ |
| `IsActive` | string(1) | ❌ | 是否啟用 (Y/N) | ✅ |
| `AddUserId` | string | ❌ | 新增員工編號 | ❌ |
| `AddTime` | DateTime | ❌ | 新增時間 | ❌ |
| `UpdateUserId` | string | ✅ | 修改員工編號 | ❌ |
| `UpdateTime` | DateTime | ✅ | 修改時間 | ❌ |
| `RouterId` | string(50) | ❌ | 路由主鍵 (FK) | ✅ |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 GET /Action 請求 (可帶 QueryString)
│
├─ ASP.NET Core Model Validation
│  ├─ 驗證 IsCommon 格式 [YN] (若有提供)
│  ├─ 驗證 IsActive 格式 [YN] (若有提供)
│  ├─ 驗證 RouterId 長度 ≤ 50 (若有提供)
│  │
│  ├─ 驗證失敗 → 400 Bad Request (Return Code: 4000)
│  └─ 驗證通過 → 繼續
│
├─ 商業邏輯處理 (Handler.Handle)
│  │
│  ├─ 建構動態查詢 (Line 54-65)
│  │  ├─ 使用 AsNoTracking() 提升效能
│  │  ├─ 加入 IsCommon 篩選條件 (若有提供)
│  │  ├─ 加入 IsActive 篩選條件 (若有提供)
│  │  └─ 加入 RouterId 篩選條件 (若有提供)
│  │
│  ├─ 執行查詢 (Line 65)
│  │  └─ ToListAsync() 取得所有符合條件的資料
│  │
│  ├─ 資料轉換 (Line 67)
│  │  └─ 使用 AutoMapper 轉換為 Response 列表
│  │
│  └─ 回傳成功回應 (Line 69)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "查詢成功"
│        └─ Data: List<GetActionsByQueryStringResponse>
```

---

## 關鍵業務規則

### 1. 動態查詢設計
- 所有查詢參數都是選填的
- 支援單一條件查詢或多條件組合查詢
- 若不提供任何參數,返回所有操作資料
- 使用 OR 邏輯判斷是否加入篩選條件

### 2. AsNoTracking 查詢優化
- 使用 AsNoTracking() 因為是唯讀查詢
- 不需要追蹤實體變更狀態,提升查詢效能
- 減少記憶體使用,適合大量查詢場景

### 3. 多條件篩選支援
- IsCommon: 篩選通用/非通用操作
- IsActive: 篩選啟用/停用操作
- RouterId: 篩選指定路由的操作
- 三個條件可任意組合使用

### 4. 查詢結果說明
- 返回資料為陣列,可能包含 0 到多筆資料
- 即使查無資料也返回 Return Code 2000 (成功)
- Data 欄位為空陣列 [] 表示查無符合條件的資料

### 5. AutoMapper 資料轉換
- 使用 AutoMapper 將實體列表轉換為 Response DTO 列表
- 避免直接回傳實體,符合 DTO 模式
- 提供彈性的資料結構轉換

### 6. RESTful API 設計
- GET /Action 用於查詢多筆資料
- 使用 QueryString 傳遞篩選條件
- 符合 RESTful API 設計原則

### 7. IsCommon 應用場景
- 前端可查詢通用操作 (IsCommon=Y),這些操作不需要權限檢查
- 或查詢需要權限檢查的操作 (IsCommon=N)
- 方便權限管理和前端過濾

### 8. IsActive 應用場景
- 前端通常只查詢啟用的操作 (IsActive=Y)
- 管理後台可能需要查詢所有操作 (包含停用的)
- 提供彈性的狀態篩選

### 9. RouterId 應用場景
- 前端可根據路由查詢該路由下的所有操作
- 方便建立路由與操作的關聯關係
- 支援動態權限配置

### 10. 效能考量
- 使用 AsNoTracking() 提升查詢效能
- 多個 Where 子句在 SQL 轉譯時會合併為一個 WHERE 條件
- 建議在資料庫中為常用篩選欄位建立索引 (IsCommon, IsActive, RouterId)

---

## 請求範例

### 請求範例 1: 查詢所有啟用的操作

```
GET /Action?IsActive=Y
Authorization: Bearer {jwt_token}
```

### 請求範例 2: 查詢指定路由的操作

```
GET /Action?RouterId=SetUpBillDay
Authorization: Bearer {jwt_token}
```

### 請求範例 3: 組合查詢

```
GET /Action?IsActive=Y&RouterId=SetUpBillDay
Authorization: Bearer {jwt_token}
```

### 請求範例 4: 查詢通用且啟用的操作

```
GET /Action?IsCommon=Y&IsActive=Y
Authorization: Bearer {jwt_token}
```

### 請求範例 5: 查詢所有操作 (不帶參數)

```
GET /Action
Authorization: Bearer {jwt_token}
```

### 回應範例

**成功回應 (來自 Examples.cs Line 9-23):**
```json
HTTP/1.1 200 OK
Content-Type: application/json

{
  "returnCode": 2000,
  "returnMessage": "查詢成功",
  "data": [
    {
      "actionName": "查詢路由類別ByQueryString",
      "actionId": "GetRouterCatregoriesByQueryString",
      "routerId": "SetUpRouterCategory",
      "isCommon": "Y",
      "isActive": "Y",
      "addUserId": "admin",
      "addTime": "2025-01-15T10:30:00",
      "updateUserId": "admin",
      "updateTime": "2025-01-15T14:20:00"
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
  "returnMessage": "查詢成功",
  "data": [],
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Action/GetActionsByQueryString/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Action/GetActionsByQueryString/Models.cs` | 請求及回應模型定義 |
| Examples.cs | `Modules/Auth/Action/GetActionsByQueryString/Examples.cs` | Swagger 範例定義 |
| Auth_Action.cs | `Infrastructures/Data/Entities/Auth_Action.cs` | 資料庫實體定義 |
