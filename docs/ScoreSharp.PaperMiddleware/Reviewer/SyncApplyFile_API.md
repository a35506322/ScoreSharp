# SyncApplyFile API - 圖檔傳送 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerCore/SyncApplyFile` |
| **HTTP 方法** | POST |
| **功能** | 申請書圖檔同步 - 支援紙本初始、補件、網路小白件三種同步方式 |
| **位置** | `Modules/Reviewer/ReviewerCore/SyncApplyFile/Endpoint.cs` |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `X-APPLYNO` | string | ✅ | 申請書編號 (Max: 14 chars) |
| `X-SYNCUSERID` | string | ✅ | 同步員工編號 (Max: 30 chars) |

### 請求體 (Body)

```csharp
public class SyncApplyFileRequest
{
    [Required]
    [MaxLength(14)]
    public string ApplyNo { get; set; }  // 申請書編號

    [Required]
    [ValidEnumValue]
    public SyncFileStatus SyncStatus { get; set; }  // 同步狀態: 紙本初始=1, 補件=2, 網路小白件=3

    [Required]
    [MaxLength(30)]
    public string SyncUserId { get; set; }  // 同步員編

    [Required]
    public ApplyFileDto[] ApplyFiles { get; set; }  // 檔案陣列
}

public class ApplyFileDto
{
    [Required]
    public byte[] FileContent { get; set; }  // 申請書檔案內容 (二進位)

    [Required]
    [MaxLength(100)]
    public string FileName { get; set; }  // 檔案名稱

    [Required]
    public int FileId { get; set; }  // 檔案 Key (紙本端檔案編號)
}
```

### 同步狀態 (SyncFileStatus Enum)

| 值 | 名稱 | 說明 |
|----|------|------|
| 1 | 紙本初始 | 首次新增紙本申請書檔案 |
| 2 | 補件 | 補充紙本申請書檔案 |
| 3 | 網路小白件 | 網路非卡友申請的檔案同步 |

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "同步成功: {ApplyNo}",
  "data": "{ApplyNo}",
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 格式驗證失敗 (400 Bad Request - Return Code: 4000)
- 請求格式不符合定義
- 缺少必填欄位
- 欄位長度超過限制

```json
{
  "returnCode": 4000,
  "returnMessage": "格式驗證失敗",
  "data": {
    "ApplyNo": ["ApplyNo 為必填欄位"],
    "ApplyFiles": ["ApplyFiles 為必填欄位"]
  }
}
```

#### 商業邏輯有誤 (400 Bad Request - Return Code: 4003)
- 紙本初始: 申請書編號已存在於主檔中
- 網路小白件: 卡片狀態不符合要求 (不是 20012 或 20014)

```json
{
  "returnCode": 4003,
  "returnMessage": "申請書編號 {ApplyNo} 已存在於主檔中。",
  "data": null
}
```

#### 標頭驗證失敗 (401 Unauthorized - Return Code: 4004)
- 缺少 X-APPLYNO 或 X-SYNCUSERID 標頭

#### 查無此資料 (404 Not Found - Return Code: 4002)
- 補件: 申請書編號不存在於主檔中
- 網路小白件: 申請書編號不存在於主檔或處理檔中

#### 內部程式失敗 (500 Internal Server Error - Return Code: 5000)
- 系統內部處理錯誤
- 版本號查詢失敗

#### 資料庫執行失敗 (500 Internal Server Error - Return Code: 5002)
- 分散式交易執行失敗

---

## 驗證資料

### 1. 格式驗證
- **位置**: `資料驗證格式()` 方法 (Line 129-135)
- **驗證內容**:
  - ApplyNo: 必填, 最大長度 14 字元
  - SyncStatus: 必填, 必須是有效的 Enum 值
  - SyncUserId: 必填, 最大長度 30 字元
  - ApplyFiles: 必填, 陣列不可為空
    - FileContent: 必填, 二進位檔案內容
    - FileName: 必填, 最大長度 100 字元
    - FileId: 必填, 整數型

### 2. 商業邏輯驗證
- **位置**: `驗證商業邏輯()` 方法 (Line 143-173)

#### 紙本初始 (SyncStatus = 1)
```
檢查點: 申請書編號是否已存在主檔
├─ 若存在 → 拋出 BusinessBadRequestException
│  └─ 訊息: "申請書編號 {ApplyNo} 已存在於主檔中。"
└─ 若不存在 → 通過驗證
```

#### 補件 (SyncStatus = 2)
```
檢查點: 申請書編號是否存在主檔
├─ 若不存在 → 拋出 NotFoundException
│  └─ 訊息: "申請書編號 {ApplyNo} 不存在於主檔中。"
└─ 若存在 → 通過驗證
```

#### 網路小白件 (SyncStatus = 3)
```
檢查點1: 申請書編號是否同時存在於主檔和處理檔
├─ 若不存在 → 拋出 NotFoundException
│  └─ 訊息: "申請書編號 {ApplyNo} 不存在於主檔或處理檔中。"

檢查點2: 卡片狀態是否為指定狀態
├─ 允許的狀態: 20012 (書面申請等待 MyData) 或 20014 (書面申請等待列印申請書及回郵信封)
├─ 若不符合 → 拋出 BusinessBadRequestException
│  └─ 訊息: "申請書編號 {ApplyNo} 的處理狀態不符合要求。"
└─ 若符合 → 通過驗證
```

---

## 資料處理

### 1. 資料庫查詢
- **查詢主檔**: `Reviewer_ApplyCreditCardInfoMain` (申請書主檔)
- **查詢處理檔**: `Reviewer_ApplyCreditCardInfoHandle` (申請書處理檔)

### 2. 檔案存儲處理
- **位置**: `資料庫分散式交易()` 方法 (Line 198-457)

#### 檔案新增邏輯
```
遍歷 ApplyFiles 陣列
├─ 產生 GUID 作為 FileId
├─ 產生檔案名稱: {ApplyNo}_{原檔名}
├─ 新增至 Reviewer_ApplyFile (檔案儲存表)
│  └─ ApplyNo, FileId (GUID), FileName, FileContent, FileType=申請書相關
└─ 新增至 Reviewer_ApplyCreditCardInfoFile (檔案歷程表)
   ├─ ApplyNo, Page (自動遞增), Process (狀態字串)
   ├─ AddTime, AddUserId (同步員編)
   ├─ Note: "完成附件補檔FROM Paper;紙本FileId:{file.FileId};"
   └─ DBName: "ScoreSharp_File"
```

**檔案計數邏輯**:
- 查詢既有檔案頁數: `COUNT(Reviewer_ApplyCreditCardInfoFile)` 其中 `ApplyNo = req.ApplyNo`
- 新增檔案頁數: 從 (既有最大頁數 + 1) 開始遞增

### 3. Process (流程歷程) 新增
- **位置**: Line 270-281, 283-326
- **表單**: `Reviewer_ApplyCreditCardInfoProcess`

#### 主要 Process 記錄
```csharp
new Reviewer_ApplyCreditCardInfoProcess
{
    ApplyNo = req.ApplyNo,
    Process = 產生ProcessString(req, handles),  // 根據同步狀態轉換
    ProcessUserId = req.SyncUserId,
    StartTime = now,
    EndTime = now,
    Notes = $"完成附件補檔FROM Paper;紙本FileId:{string.Join(',', req.ApplyFiles.Select(x => x.FileId))}"
}
```

#### 狀態轉換邏輯 (產生ProcessString)
```
紙本初始 (Status=1)
  └─ CardStatus.紙本件_初始

補件 (Status=2)
  └─ CardStatus.補回件

網路小白件 (Status=3)
  └─ 取用 Handle 的 CardStatus
```

### 4. 補件時的額外處理 (Status = 2)
- **位置**: Line 283-326

#### 紙本源補件
```
查詢 Handle 中 CardStatus = 補件作業中 的記錄
├─ 若有記錄
│  ├─ 判斷 CardStep 是否為 人工徵審
│  ├─ 是 → 轉換為 CardStatus.補回件
│  └─ 否 → 轉換為 CardStatus.紙本件_待月收入預審
│
│  新增 Process 記錄 (每個 Handle 一條)
│  ├─ Process = 轉換後的狀態
│  ├─ StartTime = now.AddSeconds(1)
│  └─ Notes 包含 (正卡/附卡 + 申請卡類型)
│
│  新增 CardRecord 記錄
│  ├─ CardStatus = 轉換後的狀態
│  ├─ ApproveUserId = req.SyncUserId
│  ├─ AddTime = now.AddSeconds(1)
│  └─ HandleNote = 附件補檔說明
│
└─ 若無記錄 → 跳過此步驟

更新 Main (主檔)
├─ LastUpdateTime = now
└─ LastUpdateUserId = UserIdConst.SYSTEM
```

#### 網路源補件
```
取得唯一的 Handle 記錄
├─ 若 CardStatus = 補件作業中
│  ├─ 判斷 CardStep 是否為 人工徵審
│  ├─ 是 → 轉換為 CardStatus.補回件
│  └─ 否 → 轉換為 CardStatus.網路件_待月收入預審
│
│  新增 Process 和 CardRecord 記錄 (同上)
│
└─ 若 CardStatus ≠ 補件作業中 → 跳過狀態轉換

更新 Main (主檔)
├─ LastUpdateTime = now
└─ LastUpdateUserId = UserIdConst.SYSTEM
```

### 5. 紙本初始時的主檔新增 (Status = 1)
- **位置**: Line 251-268

```csharp
new Reviewer_ApplyCreditCardInfoMain
{
    ApplyNo = req.ApplyNo,
    UserType = UserType.正卡人,
    Source = Source.紙本,
    ApplyDate = now,
    CaseType = CaseType.一般件,
    IsHistory = "N",
    LastUpdateUserId = UserIdConst.SYSTEM,
    LastUpdateTime = now,
    AMLProfessionCode_Version = currentVersion  // 從系統參數取得
}
```

**AMLProfessionCode_Version**:
- 從 `SysParamManage_SysParam` 取得當前版本
- 作為案件的 AML 職業別版本控制

---

## 資料庫操作

### 分散式交易管理
- **位置**: Line 375-456
- **特點**: 使用兩個資料庫連線的分散式交易

```csharp
using var fileTransaction = await scoreSharpFileContext.Database.BeginTransactionAsync();
using var mainTransaction = await scoreSharpContext.Database.BeginTransactionAsync();
try
{
    // 1. 新增檔案至 File 資料庫
    await scoreSharpFileContext.Reviewer_ApplyFile.AddRangeAsync(reviewerApplyFiles);

    // 2. 條件性新增主檔 (紙本初始)
    if (req.SyncStatus == SyncFileStatus.紙本初始)
    {
        await scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.AddAsync(entityMain);
    }

    // 3. 處理補件時的 Handle 狀態更新
    // 4. 新增 Process 歷程
    await scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);

    // 5. 條件性新增 CardRecord (補件且 CardStatus = 補件作業中)
    if (cardRecords.Count > 0)
    {
        await scoreSharpContext.Reviewer_CardRecord.AddRangeAsync(cardRecords);
    }

    // 6. 新增檔案歷程
    await scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.AddRangeAsync(reviewerApplyCreditCardInfoFiles);

    // 提交兩個事務
    await scoreSharpFileContext.SaveChangesAsync();
    await scoreSharpContext.SaveChangesAsync();
    await fileTransaction.CommitAsync();
    await mainTransaction.CommitAsync();
}
catch (Exception ex)
{
    // 發生錯誤時自動回滾
    logger.LogError("分散式交易失敗，開始回滾: {@Error}", ex.ToString());
    await mainTransaction.RollbackAsync();
    await fileTransaction.RollbackAsync();
    throw new DatabaseExecuteException("分散式交易失敗", ex);
}
```

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Reviewer_ApplyFile` | INSERT | 存儲實際的檔案內容 (File DB) |
| `Reviewer_ApplyCreditCardInfoFile` | INSERT | 檔案歷程記錄 |
| `Reviewer_ApplyCreditCardInfoMain` | INSERT/UPDATE | 主檔 (僅初始時新增) |
| `Reviewer_ApplyCreditCardInfoHandle` | UPDATE | 處理檔 (補件時更新狀態) |
| `Reviewer_ApplyCreditCardInfoProcess` | INSERT | 流程歷程 |
| `Reviewer_CardRecord` | INSERT | 卡片狀態紀錄 (補件時) |

---

## 業務流程說明

### 流程圖 (三種同步方式)

```
請求 SyncApplyFile
│
├─ 驗證格式
├─ 查詢主檔 & 處理檔
├─ 商業邏輯驗證
│
├─────────────────────────────────┬─────────────────┬──────────────────┐
│                                 │                 │                  │
▼                                 ▼                 ▼                  ▼
紙本初始 (Status=1)          補件 (Status=2)    網路小白件 (Status=3)
│                            │                  │
├─ 檢查主檔不存在            ├─ 檢查主檔存在    ├─ 檢查主檔+處理檔存在
├─ 新增主檔                  ├─ 更新 Handle      ├─ 檢查 CardStatus
├─ 新增 Process              │   CardStatus      ├─ 新增 Process
│   (紙本件_初始)            ├─ 新增 Process     │   (保持原 CardStatus)
│                            │   & CardRecord    │
└─────────────────────────────┴──────────────────┘
                    │
                    │ 所有情況
                    ▼
        新增檔案 + 檔案歷程
        │
        ├─ 遍歷 ApplyFiles
        ├─ 產生 GUID
        ├─ 檔案名稱: {ApplyNo}_{原檔名}
        ├─ 插入 Reviewer_ApplyFile (File DB)
        └─ 插入 Reviewer_ApplyCreditCardInfoFile (Main DB)
                    │
                    ▼
        分散式交易提交
                    │
            ├─ 成功 → Return 2000
            └─ 失敗 → 自動回滾, Return 5002
```

### 補件的狀態轉換邏輯 (詳細)

```
補件請求 (Status = 2)
│
├─ 紙本源 (main.Source = 紙本)
│  │
│  ├─ 查詢 Handle 中 CardStatus = 補件作業中 的記錄
│  │  │
│  │  ├─ 存在記錄
│  │  │  │
│  │  │  ├─ 遍歷每個 Handle
│  │  │  │  │
│  │  │  │  ├─ 若 CardStep = 人工徵審
│  │  │  │  │  └─ CardStatus → 補回件
│  │  │  │  │
│  │  │  │  └─ 其他 CardStep
│  │  │  │     └─ CardStatus → 紙本件_待月收入預審
│  │  │  │
│  │  │  └─ 為每個轉換新增 Process + CardRecord
│  │  │
│  │  └─ 無相符記錄 → 跳過狀態轉換
│  │
│  └─ 更新 Main 的 LastUpdateTime & LastUpdateUserId
│
└─ 網路源 (main.Source ≠ 紙本)
   │
   ├─ 取得唯一 Handle 記錄
   │  │
   │  ├─ 若 CardStatus = 補件作業中
   │  │  │
   │  │  ├─ 若 CardStep = 人工徵審
   │  │  │  └─ CardStatus → 補回件
   │  │  │
   │  │  └─ 其他 CardStep
   │  │     └─ CardStatus → 網路件_待月收入預審
   │  │
   │  ├─ 新增 Process + CardRecord
   │  │
   │  └─ 若 CardStatus ≠ 補件作業中 → 跳過狀態轉換
   │
   └─ 更新 Main 的 LastUpdateTime & LastUpdateUserId
```

---

## 關鍵業務規則

### 1. 檔案編號與計數
- 檔案 ID: 使用 GUID 以確保全局唯一性
- 頁碼 (Page): 自動遞增，用於檔案排序
- 檔案名稱規範: `{ApplyNo}_{原檔名}`

### 2. AML 職業別版本控制
- 在紙本初始時，從系統參數 `SysParamManage_SysParam.AMLProfessionCode_Version` 取得
- 儲存至 `Reviewer_ApplyCreditCardInfoMain.AMLProfessionCode_Version`
- 作為案件生命周期內的固定版本

### 3. 時間戳設定
- 所有操作使用 `DateTime.Now`
- 補件時多個 Handle 的操作時間遞增 1 秒 (AddSeconds(1))，以保持邏輯順序

### 4. 同步員追蹤
- 所有操作記錄同步員編號 (`SyncUserId`)
- 主檔更新時使用 `UserIdConst.SYSTEM` 表示系統操作

### 5. 備註說明
- Process Note: 記錄紙本端檔案 ID，便於追蹤
- CardRecord Note: 包含 (正卡/附卡) 及 (申請卡類型) 資訊

---

## TODO 項目

| 項目 | 說明 | 狀態 |
|------|------|------|
| 浮水印加工 | 檔案上傳時的浮水印處理方式待確認 | ❌ 未實作 |
| 檔案命名規則 | 目前暫定為 `{ApplyNo}_{檔名}`, 待業務確認 | ⚠️ 暫定 |

