# GetRoleAuthById API - 取得單筆角色權限 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/Role/{roleId}` |
| **HTTP 方法** | GET |
| **功能** | 取得單筆角色權限 - 查詢角色的完整權限設定並以階層式結構呈現 |
| **位置** | `Modules/Auth/Role/GetRoleAuthById/Endpoint.cs` |

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
/Role/GetRoleAuthById/Admin
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
      "routerCategoryId": "SetUp",
      "routerCategoryName": "設定作業",
      "routers": [
        {
          "routerId": "SetUpBlackListReason",
          "routerName": "取有單筆黑名單理由",
          "actions": [
            {
              "actionId": "GetBlackListReasonById",
              "actionName": "取有單筆黑名單理由",
              "hasPermission": "Y"
            }
          ]
        },
        {
          "routerId": "SetUpBillDay",
          "routerName": "帳單日期",
          "actions": [
            {
              "actionId": "GetBillDayByQueryString",
              "actionName": "取帳單日期",
              "hasPermission": "N"
            }
          ]
        }
      ]
    }
  ],
  "traceId": "{traceId}"
}
```

### Response 資料結構 (階層式)

```csharp
// 位置: Modules/Auth/Role/GetRoleAuthById/Models.cs (Line 1-33)

// 第一層: 路由類別
public class GetRoleAuthByIdResponse
{
    public string RouterCategoryId { get; set; }
    public string RouterCategoryName { get; set; }
    public List<Router> Routers { get; set; }
}

// 第二層: 路由
public class Router
{
    public string RouterId { get; set; }
    public string RouterName { get; set; }
    public List<Action> Actions { get; set; }
}

// 第三層: 操作
public class Action
{
    public string ActionId { get; set; }
    public string ActionName { get; set; }
    public string HasPermission { get; set; }  // Y: 有權限, N: 無權限
}

// Dapper 查詢使用的 DTO
public class RoleAuthDto
{
    public string RouterCategoryId { get; set; }
    public string RouterCategoryName { get; set; }
    public string RouterId { get; set; }
    public string RouterName { get; set; }
    public string ActionId { get; set; }
    public string ActionName { get; set; }
}
```

### 階層結構說明

```
RouterCategory (路由類別)
│
├─ Router (路由)
│  │
│  ├─ Action (操作) - HasPermission: Y/N
│  ├─ Action (操作) - HasPermission: Y/N
│  └─ ...
│
├─ Router (路由)
│  └─ ...
│
└─ ...
```

---

## 驗證資料

### 1. 格式驗證
- **位置**: ASP.NET Core Route Parameter Binding (自動執行)
- **驗證內容**:
  - roleId: 必填,從路由參數取得

### 2. 商業邏輯驗證
- **位置**: `Handle()` 方法 (Endpoint.cs Line 48-94)
- **說明**: 此 API 不驗證 RoleId 是否存在,即使 RoleId 不存在也會回傳空陣列

---

## 資料處理

### 1. 資料庫查詢 (使用 Dapper)

#### 查詢 1: 取得所有 Action 與關聯資料
- **位置**: Line 50, Line 96-116
- **查詢方法**: 使用 Dapper 執行原生 SQL
- **SQL 語句**:
  ```sql
  SELECT  C.RouterCategoryId
         , RouterCategoryName
         , B.RouterId
         , RouterName
         , A.ActionId
         , ActionName
  FROM  [ScoreSharp].[dbo].[Auth_Action] A
  JOIN [ScoreSharp].[dbo].[Auth_Router] B
    ON A.RouterId = B.RouterId
  JOIN [ScoreSharp].[dbo].[Auth_RouterCategory] C
    ON B.RouterCategoryId = C.RouterCategoryId
  WHERE A.ActionId NOT IN ('Login','GetUserAuthBySelf')
  ORDER BY C.Sort, B.Sort
  ```
- **說明**:
  - 三表 JOIN 查詢: Auth_Action -> Auth_Router -> Auth_RouterCategory
  - 排除特定 ActionId: Login 與 GetUserAuthBySelf (不納入權限管理)
  - 依 RouterCategory.Sort 與 Router.Sort 排序
  - 回傳所有系統中的 Action 及其關聯的 Router 與 RouterCategory 資訊

#### 查詢 2: 取得該角色已擁有的權限
- **位置**: Line 51-54
- **查詢方法**: EF Core LINQ
- **程式碼**:
  ```csharp
  var roleRouterAction = await _context.Auth_Role_Router_Action
      .Where(x => x.RoleId == request.roleId)
      .Select(x => x.ActionId)
      .ToListAsync();
  ```
- **說明**: 取得該角色已被授權的所有 ActionId 清單

### 2. 資料轉換與階層建構 (重要邏輯)

#### 步驟 1: 依 RouterCategory 分組
- **位置**: Line 56
- **程式碼**:
  ```csharp
  var groupedEntities = entities.GroupBy(e => new {
      e.RouterCategoryId,
      e.RouterCategoryName
  });
  ```
- **說明**: 將所有 Action 依 RouterCategoryId 與 RouterCategoryName 分組

#### 步驟 2: 建立 RouterCategory 層級
- **位置**: Line 60-67
- **程式碼**:
  ```csharp
  foreach (var categoryGroup in groupedEntities)
  {
      var categoryResponse = new GetRoleAuthByIdResponse
      {
          RouterCategoryId = categoryGroup.Key.RouterCategoryId,
          RouterCategoryName = categoryGroup.Key.RouterCategoryName,
          Routers = new List<Router>(),
      };
      // ...
  }
  ```

#### 步驟 3: 依 Router 分組並建立 Router 層級
- **位置**: Line 69-87
- **程式碼**:
  ```csharp
  var routerGroups = categoryGroup.GroupBy(e => new {
      e.RouterId,
      e.RouterName
  });

  foreach (var routerGroup in routerGroups)
  {
      var router = new Router
      {
          RouterId = routerGroup.Key.RouterId,
          RouterName = routerGroup.Key.RouterName,
          Actions = routerGroup.Select(a => new Action
          {
              ActionId = a.ActionId,
              ActionName = a.ActionName,
              HasPermission = roleRouterAction.SingleOrDefault(x => x == a.ActionId) != null
                  ? "Y"
                  : "N",
          }).ToList(),
      };

      categoryResponse.Routers.Add(router);
  }
  ```

#### 步驟 4: 建立 Action 層級並設定 HasPermission
- **位置**: Line 77-84
- **邏輯**:
  ```
  對於每個 Action:
  ├─ 檢查該 ActionId 是否存在於 roleRouterAction 清單中
  ├─ 若存在 → HasPermission = "Y" (該角色有此權限)
  └─ 若不存在 → HasPermission = "N" (該角色沒有此權限)
  ```

### 3. 處理邏輯流程

```
接收請求
│
├─ 步驟 1: 使用 Dapper 查詢所有 Action 及其關聯資料 (Line 50)
│  └─ 三表 JOIN: Action -> Router -> RouterCategory
│
├─ 步驟 2: 使用 EF Core 查詢該角色已擁有的權限 (Line 51-54)
│  └─ 取得 ActionId 清單
│
├─ 步驟 3: 依 RouterCategory 分組 (Line 56)
│  └─ GroupBy RouterCategoryId 與 RouterCategoryName
│
├─ 步驟 4: 建立階層式資料結構 (Line 60-90)
│  │
│  ├─ 第一層迴圈: 遍歷每個 RouterCategory
│  │  ├─ 建立 GetRoleAuthByIdResponse 物件
│  │  │
│  │  ├─ 第二層迴圈: 遍歷該 Category 下的每個 Router
│  │  │  ├─ 依 RouterId 與 RouterName 分組
│  │  │  ├─ 建立 Router 物件
│  │  │  │
│  │  │  ├─ 第三層迴圈: 遍歷該 Router 下的每個 Action
│  │  │  │  ├─ 建立 Action 物件
│  │  │  │  ├─ 檢查該 ActionId 是否在已授權清單中
│  │  │  │  └─ 設定 HasPermission (Y/N)
│  │  │  │
│  │  │  └─ 將 Router 加入 Category
│  │  │
│  │  └─ 將 Category 加入結果清單
│  │
│  └─ 回傳完整的階層式結構
│
└─ 步驟 5: 回傳成功回應 (Line 93)
   └─ Return Code: 2000, Data: List<GetRoleAuthByIdResponse>
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Auth_Action` | SELECT | 操作主檔 - 使用 Dapper 查詢 |
| `Auth_Router` | SELECT | 路由主檔 - 使用 Dapper 查詢 |
| `Auth_RouterCategory` | SELECT | 路由類別主檔 - 使用 Dapper 查詢 |
| `Auth_Role_Router_Action` | SELECT | 角色權限關聯表 - 使用 EF Core 查詢 |

### 操作類型

#### 1. Dapper 原生 SQL 查詢 (重要功能)

**查詢: 取得所有 Action 與關聯資料**
- **位置**: Line 96-115
- **查詢框架**: Dapper
- **SQL 語句**:
  ```sql
  SELECT  C.RouterCategoryId
         , RouterCategoryName
         , B.RouterId
         , RouterName
         , A.ActionId
         , ActionName
  FROM  [ScoreSharp].[dbo].[Auth_Action] A
  JOIN [ScoreSharp].[dbo].[Auth_Router] B
    ON A.RouterId = B.RouterId
  JOIN [ScoreSharp].[dbo].[Auth_RouterCategory] C
    ON B.RouterCategoryId = C.RouterCategoryId
  WHERE A.ActionId NOT IN ('Login','GetUserAuthBySelf')
  ORDER BY C.Sort, B.Sort
  ```
- **為何使用 Dapper**:
  - 複雜的多表 JOIN 查詢
  - 需要精確控制 SQL 語句
  - Dapper 效能優於 EF Core LINQ (對於複雜查詢)
  - 直接對應到 RoleAuthDto 物件

#### 2. EF Core LINQ 查詢

**查詢: 取得角色已擁有的權限**
- **位置**: Line 51-54
- **方法**: `Where()` + `Select()` + `ToListAsync()`
- **EF Core 程式碼**:
  ```csharp
  var roleRouterAction = await _context.Auth_Role_Router_Action
      .Where(x => x.RoleId == request.roleId)
      .Select(x => x.ActionId)
      .ToListAsync();
  ```
- **等效 SQL**:
  ```sql
  SELECT [ActionId]
  FROM [dbo].[Auth_Role_Router_Action]
  WHERE [RoleId] = @RoleId
  ```

### 資料表結構

#### Auth_Action
| 欄位名稱 | 資料型別 | 說明 |
|---------|---------|------|
| `ActionId` | string(50) | 主鍵 - 操作識別碼 |
| `ActionName` | string(50) | 操作名稱 |
| `RouterId` | string(50) | 外鍵 - 關聯 Auth_Router |
| `IsActive` | string(1) | 是否啟用 (Y/N) |

#### Auth_Router
| 欄位名稱 | 資料型別 | 說明 |
|---------|---------|------|
| `RouterId` | string(50) | 主鍵 - 路由識別碼 |
| `RouterName` | string(30) | 路由名稱 |
| `RouterCategoryId` | string(50) | 外鍵 - 關聯 Auth_RouterCategory |
| `Sort` | int | 排序 |

#### Auth_RouterCategory
| 欄位名稱 | 資料型別 | 說明 |
|---------|---------|------|
| `RouterCategoryId` | string(50) | 主鍵 - 路由類別識別碼 |
| `RouterCategoryName` | string(30) | 路由類別名稱 |
| `Sort` | int | 排序 |

#### Auth_Role_Router_Action
| 欄位名稱 | 資料型別 | 說明 |
|---------|---------|------|
| `RoleId` | string(50) | 主鍵 - 角色識別碼 |
| `RouterId` | string(50) | 主鍵 - 路由識別碼 |
| `ActionId` | string(50) | 主鍵 - 操作識別碼 |

---

## 業務流程說明

### 完整業務流程圖

```
用戶發送 GET /Role/GetRoleAuthById/{roleId} 請求
│
├─ 商業邏輯處理 (Handler.Handle)
│  │
│  ├─ 步驟 1: 使用 Dapper 查詢所有 Action 資料 (Line 50, 96-115)
│  │  ├─ 建立資料庫連線
│  │  ├─ 執行三表 JOIN SQL
│  │  ├─ 排除 Login 與 GetUserAuthBySelf
│  │  ├─ 依 Sort 排序
│  │  └─ 對應到 RoleAuthDto 物件
│  │
│  ├─ 步驟 2: 使用 EF Core 查詢角色權限 (Line 51-54)
│  │  └─ 取得該角色已授權的 ActionId 清單
│  │
│  ├─ 步驟 3: 依 RouterCategory 分組 (Line 56)
│  │  └─ GroupBy RouterCategoryId & RouterCategoryName
│  │
│  ├─ 步驟 4: 建立階層式資料結構 (Line 60-90)
│  │  │
│  │  ├─ 遍歷每個 RouterCategory
│  │  │  ├─ 建立 GetRoleAuthByIdResponse
│  │  │  │
│  │  │  ├─ 依 Router 分組
│  │  │  │  ├─ 遍歷每個 Router
│  │  │  │  │  ├─ 建立 Router 物件
│  │  │  │  │  │
│  │  │  │  │  ├─ 遍歷該 Router 下的每個 Action
│  │  │  │  │  │  ├─ 建立 Action 物件
│  │  │  │  │  │  ├─ 檢查是否在已授權清單
│  │  │  │  │  │  └─ 設定 HasPermission (Y/N)
│  │  │  │  │  │
│  │  │  │  │  └─ 加入 Router 到 Category
│  │  │  │
│  │  │  └─ 加入 Category 到結果清單
│  │
│  └─ 步驟 5: 回傳成功回應 (Line 93)
│     └─ 200 OK (Return Code: 2000)
│        ├─ Message: "成功"
│        └─ Data: 階層式權限結構
```

---

## 關鍵業務規則

### 1. 階層式資料結構
- **三層架構**: RouterCategory -> Router -> Action
- 完整呈現系統權限樹狀結構
- 方便前端以樹狀或階層式 UI 呈現
- 讓使用者清楚了解權限的組織方式

### 2. HasPermission 標記機制
- 每個 Action 都有 HasPermission 欄位 (Y/N)
- 'Y' 表示該角色已被授權此操作
- 'N' 表示該角色未被授權此操作
- 前端可用於勾選框的勾選狀態

### 3. 排除特定 Action
- 排除 'Login' 與 'GetUserAuthBySelf' 這兩個 ActionId
- 這些是系統內建功能,不需要權限管理
- 所有角色都應該有這些基本功能

### 4. 排序規則
- 先依 RouterCategory.Sort 排序
- 再依 Router.Sort 排序
- 確保前端顯示順序一致且有意義
- Sort 值越小,顯示越前面

### 5. Dapper 與 EF Core 混合使用
- **Dapper 用於複雜查詢**:
  - 三表 JOIN 查詢
  - 需要精確 SQL 控制
  - 效能考量
- **EF Core 用於簡單查詢**:
  - 單表查詢
  - 簡單條件篩選
  - 型別安全

### 6. 不驗證 RoleId 存在性
- 即使 RoleId 不存在,也會正常執行
- 回傳所有 Action 但 HasPermission 都是 'N'
- 適合新建角色時顯示可選權限清單

### 7. 使用 SingleOrDefault 檢查權限
- Line 82: `roleRouterAction.SingleOrDefault(x => x == a.ActionId)`
- 檢查 ActionId 是否在已授權清單中
- 若找到則為 'Y',找不到則為 'N'
- 簡潔高效的權限檢查方式

### 8. 完整性與全面性
- 回傳**所有**系統 Action (除了排除的)
- 不只是回傳已授權的 Action
- 讓前端可以顯示完整的權限樹
- 使用者可以看到哪些有權限、哪些沒有

---

## 請求範例

### 請求範例

```http
GET /Role/GetRoleAuthById/Admin
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
      "routerCategoryId": "SetUp",
      "routerCategoryName": "設定作業",
      "routers": [
        {
          "routerId": "SetUpBlackListReason",
          "routerName": "取有單筆黑名單理由",
          "actions": [
            {
              "actionId": "GetBlackListReasonById",
              "actionName": "取有單筆黑名單理由",
              "hasPermission": "Y"
            }
          ]
        },
        {
          "routerId": "SetUpBillDay",
          "routerName": "帳單日期",
          "actions": [
            {
              "actionId": "GetBillDayByQueryString",
              "actionName": "取帳單日期",
              "hasPermission": "N"
            }
          ]
        }
      ]
    }
  ],
  "traceId": "0HMVB8FJK4L3E:00000001"
}
```

---

## 相關檔案

| 檔案 | 路徑 | 說明 |
|------|------|------|
| Endpoint.cs | `Modules/Auth/Role/GetRoleAuthById/Endpoint.cs` | API 端點定義及業務邏輯處理 |
| Models.cs | `Modules/Auth/Role/GetRoleAuthById/Models.cs` | 回應模型定義 (階層式結構) |
| Example.cs | `Modules/Auth/Role/GetRoleAuthById/Example.cs` | Swagger 範例定義 |
| Auth_Action.cs | `Infrastructures/Data/Entities/Auth_Action.cs` | 操作資料庫實體定義 |
| Auth_Router.cs | `Infrastructures/Data/Entities/Auth_Router.cs` | 路由資料庫實體定義 |
| Auth_RouterCategory.cs | `Infrastructures/Data/Entities/Auth_RouterCategory.cs` | 路由類別資料庫實體定義 |
| Auth_Role_Router_Action.cs | `Infrastructures/Data/Entities/Auth_Role_Router_Action.cs` | 角色權限關聯資料庫實體定義 |
