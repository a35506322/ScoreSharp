# ScoreSharp.PaperMiddleware - Reviewer APIs 商業邏輯文檔

## 概述

本文檔集合涵蓋 **ScoreSharp.PaperMiddleware** 中 **Reviewer (徵審系統)** 的所有核心 API，提供詳細的商業邏輯、驗證規則、資料處理流程和資料庫操作說明。

### 項目信息

| 項目 | 說明 |
|------|------|
| **模組名稱** | ScoreSharp.PaperMiddleware |
| **子模組** | Modules/Reviewer/ReviewerCore |
| **控制器** | ReviewerCoreController |
| **架構模式** | MediatR CQRS + Vertical Slice |
| **API 個數** | 3 個核心 API + 1 個健康檢查 |

---

## 核心 API 清單

### 1. 🖼️ SyncApplyFile - 圖檔傳送 API

**功能**: 上傳申請書相關圖檔至系統

**端點**: `POST /ReviewerCore/SyncApplyFile`

**用途**: 支援三種同步方式
- 紙本初始 (首次新增申請書)
- 補件 (補充缺失檔案)
- 網路小白件 (網路申請的檔案)

**複雜度**: ⭐⭐⭐ (中等)

**主要特性**:
- ✅ 分散式交易 (File DB + Main DB)
- ✅ 自動檔案編號與頁碼管理
- ✅ 狀態自動轉換 (補件時的 CardStatus 更新)
- ✅ 完整的流程歷程記錄

**相關文檔**: [SyncApplyFile_API.md](./SyncApplyFile_API.md)

---

### 2. 📋 SyncApplyInfoPaper - 紙本建檔同步 API

**功能**: 同步紙本信用卡申請案件的詳細資訊

**端點**: `POST /ReviewerCore/SyncApplyInfoPaper`

**用途**: 紙本申請案件的完整資訊同步
- 正卡人詳細資訊 (基本、地址、公司、財務等)
- 附卡人資訊 (支援最多 9 名)
- 申請卡別資訊
- 案件流程歷程

**複雜度**: ⭐⭐⭐⭐⭐ (最複雜)

**主要特性**:
- ✅ 超過 300+ 個欄位的複雜 Request 模型
- ✅ 50+ 個驗證項目 (格式、定義值、業務邏輯)
- ✅ 多表聯動處理 (10+ 資料表)
- ✅ 完成狀態的檢核準備資訊自動產生
- ✅ 完整的地址標準化與全形轉半形

**狀態轉換**:
- 修改 (SyncStatus=1): 保持現有狀態，替換 Handle 和 Supplementary
- 完成 (SyncStatus=2): 轉換為「紙本件_待檢核」並產生檢核資料

**相關文檔**: [SyncApplyInfoPaper_API.md](./SyncApplyInfoPaper_API.md)

---

### 3. 🌐 SyncApplyInfoWebWhite - 網路件小白同步 API

**功能**: 同步網路非卡友申請者的資訊並進行狀態轉換

**端點**: `POST /ReviewerCore/SyncApplyInfoWebWhite`

**用途**: 線上申請系統的非卡友申請資訊同步
- 申請者個人資訊更新
- 卡片狀態轉換
- 申請流程歷程記錄

**複雜度**: ⭐⭐⭐ (中等)

**主要特性**:
- ✅ 申請書編號格式驗證 (`^\d{8}[X-Z]{1}\d{4}$`)
- ✅ 兩種卡片狀態的條件性轉換
- ✅ 外部流程資訊與系統流程整合
- ✅ 簡化的資訊結構 (無附卡人支援)

**狀態轉換**:
```
20012 (書面申請等待MyData)
  ↓
網路件_等待MyData附件

20014 (書面申請等待列印申請書及回郵信封)
  ↓
網路件_非卡友_待檢核 (30100)
```

**相關文檔**: [SyncApplyInfoWebWhite_API.md](./SyncApplyInfoWebWhite_API.md)

---

### 4. ⚕️ HealthyCheck - 健康檢查 API

**功能**: 檢查系統健康狀態

**端點**: `GET /ReviewerCore/HealthyCheck`

**回應**: `{ "status": "ok" }`

---

## API 對比表

| 特性 | SyncApplyFile | SyncApplyInfoPaper | SyncApplyInfoWebWhite |
|------|---|---|---|
| **HTTP 方法** | POST | POST | POST |
| **主要功能** | 檔案上傳 | 申請資訊同步 | 資訊更新+狀態轉換 |
| **Request 欄位數** | ~10 | ~300+ | ~200+ |
| **涉及資料表** | 6 | 10+ | 3 |
| **驗證項目數** | ~5 | ~50+ | ~30+ |
| **是否支援附卡人** | ❌ | ✅ (最多9名) | ❌ |
| **分散式交易** | ✅ | ❌ | ❌ |
| **完成時是否產生檢核資料** | ❌ | ✅ | ❌ |
| **複雜度** | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ |

---

## 數據流與整合

### 典型的紙本案件生命周期

```
初始化
  │
  ├─ SyncApplyFile (紙本初始, Status=1)
  │  └─ 建立主檔 + 新增初始檔案
  │
  ├─ SyncApplyInfoPaper (修改, Status=1)
  │  └─ 填寫申請者詳細資訊 (多次可修改)
  │
  ├─ SyncApplyFile (補件, Status=2)
  │  └─ 補充缺失檔案 (可能多次)
  │
  ├─ SyncApplyInfoPaper (完成, Status=2)
  │  └─ 標記完成 → 狀態轉為「紙本件_待檢核」
  │     + 產生 OutsideBankInfo, ApplyNote, 等檢核資訊
  │
  └─ 進入檢核階段
```

### 典型的網路非卡友案件生命周期

```
初始化 (來自線上申請系統)
  │
  ├─ SyncApplyFile (網路小白件, Status=3)
  │  └─ 新增申請書檔案
  │
  ├─ SyncApplyInfoWebWhite
  │  └─ 更新申請者資訊並轉換卡片狀態
  │     ├─ 20012 → 網路件_等待MyData附件
  │     └─ 20014 → 網路件_非卡友_待檢核
  │
  └─ 進入 MyData 驗證或檢核階段
```

---

## 共同的驗證邏輯

### 1. 格式驗證 (Format Validation)

所有 API 都使用 ASP.NET Core 的 `ModelState` 驗證:

```csharp
[Required]               // 必填欄位
[MaxLength(N)]          // 最大長度
[ValidEnumValue]        // Enum 值驗證
[TWID]                  // 台灣身分證字號
[ValidDate(format: ...)] // 日期格式驗證
```

### 2. 資料庫定義值驗證 (Database Defined Value Validation)

驗證所有代碼類型欄位是否存在於系統定義的參數表:

- **國籍** (參數類別.國籍)
- **AML 職業別** (SetUp_AMLProfession)
- **AML 職級別** (參數類別.AML職級別)
- **卡別** (參數類別.卡片種類)
- **主要收入來源** (參數類別.主要收入來源)
- **身分證發證地點** (參數類別.身分證換發地點)
- **年費收取方式** (參數類別.年費收取方式)

### 3. 商業邏輯驗證 (Business Logic Validation)

根據 API 特性不同:

**SyncApplyFile**:
- 紙本初始: 檢查申請號不存在
- 補件: 檢查申請號存在
- 網路小白件: 檢查申請號存在且 CardStatus 符合

**SyncApplyInfoPaper**:
- 檢查申請號存在於主檔
- 檢查處理檔狀態合法性

**SyncApplyInfoWebWhite**:
- 檢查申請號格式 (`^\d{8}[X-Z]{1}\d{4}$`)
- 檢查申請號存在於主檔和處理檔
- 檢查 CardStatus ∈ [20012, 20014]

---

## 資料處理特性

### 字元標準化

**SyncApplyInfoPaper 與 SyncApplyInfoWebWhite** 都使用 `MapHelper.ToHalfWidthRequest()`:

```csharp
全形 → 半形轉換 (使用 ToHalfWidth() 擴充方法)
├─ 所有姓名欄位 (CHName, ENName)
├─ 所有地址欄位
├─ 所有文字欄位
└─ 縣市特殊處理: 「台」→「臺」 (AddressHelper)
```

### 時間戳管理

```csharp
所有操作都記錄 DateTime.Now
├─ AddTime / AddUserId (首次新增)
├─ LastUpdateTime / LastUpdateUserId (最後修改)
└─ ProcessUserId (操作員編號)
```

### 使用者追蹤

```csharp
UserIdConst.SYSTEM     // 系統自動操作
req.SyncUserId         // 同步員操作
handle.ProcessUserId   // 處理員操作
```

---

## 錯誤代碼對照表

| HTTP Code | Return Code | 說明 | 應用 API |
|-----------|------------|------|---------|
| 200 | 2000 | 同步成功 | 全部 |
| 400 | 4000 | 格式驗證失敗 | 全部 |
| 400 | 4001 | 資料庫定義值錯誤 | Paper, WebWhite |
| 400 | 4003 | 商業邏輯有誤 | 全部 |
| 401 | 4004 | 標頭驗證失敗 | 全部 |
| 404 | 4002 | 查無此資料 | 全部 |
| 500 | 5000 | 內部程式失敗 | 全部 |
| 500 | 5002 | 資料庫執行失敗 | 全部 |

---

## 相關資料表

### 主要表

| 資料表 | 說明 | 涉及 API |
|--------|------|---------|
| `Reviewer_ApplyCreditCardInfoMain` | 申請案件主檔 | 全部 |
| `Reviewer_ApplyCreditCardInfoHandle` | 申請案件處理檔 | 全部 |
| `Reviewer_ApplyCreditCardInfoFile` | 申請案件檔案記錄 | File, Paper |
| `Reviewer_ApplyFile` | 實際檔案存儲 (File DB) | File |
| `Reviewer_ApplyCreditCardInfoProcess` | 申請流程歷程 | 全部 |
| `Reviewer_ApplyCreditCardInfoSupplementary` | 附卡人資訊 | Paper |
| `Reviewer_CardRecord` | 卡片狀態記錄 | File, Paper |

### 檢核相關表 (Paper API 完成時產生)

| 資料表 | 說明 |
|--------|------|
| `Reviewer_OutsideBankInfo` | 外部銀行資訊 |
| `Reviewer_ApplyNote` | 申請備註 |
| `Reviewer_InternalCommunicate` | 內部溝通記錄 |
| `Reviewer_BankTrace` | 銀行查詢紀錄 |
| `Reviewer_FinanceCheckInfo` | 財務檢查資訊 |
| `ReviewerPedding_PaperApplyCardCheckJob` | 紙本卡片檢核工作 |

### 系統參數表

| 資料表 | 用途 |
|--------|------|
| `SysParamManage_SysParam` | 系統參數 (AML 版本等) |
| `SetUp_AMLProfession` | AML 職業別定義 |
| 其他參數表 | 各類參數代碼 (國籍、卡別等) |

---

## 設計模式與架構

### CQRS 模式

每個 API 都遵循 MediatR CQRS 模式:

```csharp
Controller
  ├─ 接收 Request
  └─ 發送 Command / Query
      │
      ▼
MediatR Pipeline
  ├─ 驗證 (FluentValidation)
  └─ 執行 Handler
      │
      ▼
Handler (實現 IRequestHandler<Command/Query, Response>)
  ├─ 業務邏輯
  ├─ 資料庫操作
  └─ 異常處理
      │
      ▼
Response (ResultResponse<T>)
```

### 垂直切片架構 (Vertical Slice)

```
Reviewer/
├── ReviewerCore/
│   ├── ReviewerCoreController.cs
│   ├── SyncApplyFile/
│   │   ├── Endpoint.cs (Controller + Handler)
│   │   ├── Model.cs (Request/Response)
│   │   └── Example.cs (Swagger 範例)
│   ├── SyncApplyInfoPaper/
│   │   ├── Endpoint.cs
│   │   ├── Model.cs
│   │   ├── MapHelper.cs
│   │   └── Example.cs
│   ├── SyncApplyInfoWebWhite/
│   │   ├── Endpoint.cs
│   │   ├── Model.cs
│   │   ├── MapHelper.cs
│   │   └── Example.cs
│   └── HealthyCheck/
│       └── Endpoint.cs
```

---

## 異常處理

所有 API 都使用統一的異常處理機制:

```csharp
BadRequestException(errors)              // 400 + 4000
DatabaseDefinitionException(errors)      // 400 + 4001
BusinessBadRequestException(message)     // 400 + 4003
NotFoundException(message)               // 404 + 4002
InternalServerException(message)        // 500 + 5000
DatabaseExecuteException(message, ex)   // 500 + 5002
```

---

## 性能與安全考慮

### 1. 資料庫查詢最佳化

- 使用 `AsNoTracking()` 讀取不需修改的資料
- 批量操作使用 `AddRangeAsync`
- 避免 N+1 查詢問題

### 2. 交易管理

- **SyncApplyFile**: 分散式交易 (2 個 DB)
- **SyncApplyInfoPaper & WebWhite**: 單一 DB 交易

### 3. 日誌記錄

```csharp
logger.LogError(message, params)  // 錯誤記錄
logger.LogInformation(message)    // 一般資訊
// 包含 ApplyNo, TraceId 便於追蹤
```

### 4. 驗證順序

1. 格式驗證 (快速篩選)
2. 定義值驗證 (資料庫查詢)
3. 業務邏輯驗證 (複雜檢查)

---

## 已知限制與未來改進

### 待確認項目

| 項目 | API | 狀態 |
|------|-----|------|
| 浮水印處理 | SyncApplyFile | ❌ 未實作 |
| 檔案命名規則 | SyncApplyFile | ⚠️ 暫定 |
| AML 職業別「其他」驗證 | Paper, WebWhite | ❌ 禁用 |
| 專案代號驗證 | Paper, WebWhite | ❌ 禁用 |
| 推廣單位驗證 | Paper, WebWhite | ❌ 禁用 |
| 申請卡種邏輯 | WebWhite | ❓ 待確認 |

### 可能的優化方向

1. 快取參數值以減少資料庫查詢
2. 批量驗證相同代碼的優化
3. 非同步檔案處理
4. 更詳細的審計日誌

---

## 相關文檔

### 詳細 API 文檔

- [SyncApplyFile_API.md](./SyncApplyFile_API.md) - 圖檔傳送 API
- [SyncApplyInfoPaper_API.md](./SyncApplyInfoPaper_API.md) - 紙本建檔同步 API
- [SyncApplyInfoWebWhite_API.md](./SyncApplyInfoWebWhite_API.md) - 網路件小白同步 API

### 程式碼位置

**主要代碼**:
- `/home/user/ScoreSharp/src/ScoreSharp.PaperMiddleware/Modules/Reviewer/ReviewerCore/`

**資料庫實體**:
- `/home/user/ScoreSharp/src/ScoreSharp.PaperMiddleware/Infrastructures/Data/Entities/`

**EF Core 設定**:
- `/home/user/ScoreSharp/src/ScoreSharp.PaperMiddleware/Infrastructures/Data/EFCoreConifg.cs`

---

## 聯絡與支援

如有問題或需要澄清，請參考:
- 各 API 對應的詳細文檔
- 程式碼註解與範例
- Swagger API 文檔 (`/swagger`)

---

**文檔版本**: 1.0
**最後更新**: 2025-11-17
**維護者**: ScoreSharp Development Team
