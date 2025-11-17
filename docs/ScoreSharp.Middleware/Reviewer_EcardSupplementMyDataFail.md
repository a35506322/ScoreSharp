# EcardSupplementMyDataFail API - 補件 MyData 失敗 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerCore/EcardSupplementMyDataFail` |
| **HTTP 方法** | POST |
| **Content-Type** | application/json |
| **功能** | 處理補件中 MyData 核驗失敗的情況, 更新卡片狀態 |
| **位置** | `Modules/Reviewer/ReviewerCore/EcardSupplementMyDataFail/Endpoint.cs` |

---

## Request 定義

### 請求體 (JSON 格式)

```csharp
public class EcardSupplementMyDataFailRequest
{
    [JsonPropertyName("P_ID")]
    public string? ID { get; set; }  // 身份證號 (必填, 長度 <= 11)
}
```

### 範例請求

```json
{
  "P_ID": "K12798732"
}
```

### 必填欄位驗證清單

| 欄位 | 驗證規則 | 錯誤碼 |
|------|--------|--------|
| ID | 不能為空 | 0001 |
| ID | 長度 <= 11 | 0002 |

---

## Response 定義

### 成功回應 (200 OK - 回覆代碼: 0000)

```json
{
  "ID": "K12798732",
  "RESULT": "匯入成功"
}
```

### 錯誤回應

#### 必要欄位為空值 (回覆代碼: 0001)
```json
{
  "ID": "K12798732",
  "RESULT": "必要欄位為空值"
}
```

#### 長度過長 (回覆代碼: 0002)
```json
{
  "ID": "K12798732",
  "RESULT": "長度過長"
}
```

#### 其他異常訊息 (回覆代碼: 0003)
```json
{
  "ID": "K12798732",
  "RESULT": "其他異常訊息"
}
```

---

## 驗證資料

### 1. 格式驗證 (FormatValidation)

```
檢查點 1: 必填欄位
├─ ID: 不能為空
└─ 缺失 → 回覆代碼 0001

檢查點 2: 長度驗證
├─ ID.Length <= 11
└─ 超過 → 回覆代碼 0002
```

### 2. 商業邏輯驗證 (BusinessValidation)

#### 補件案件查詢
```
檢查點: 查詢補件中的案件信息
├─ 來源: 存儲過程 Usp_GetECardSupplementInfo
├─ 查詢條件: ID (身份證號)
├─ 回傳: 補件中的所有案件 (ApplyNo, CardStatus, CardStep, Source等)
├─ 若查無補件案件 → 回覆代碼 0003
└─ 續用取回的案件信息進行後續分組和處理
```

#### 補件案件分組
```
檢查點: 按 ApplyNo 分組補件案件
├─ 可能一個身份證號對應多筆補件案件
├─ 每筆案件獨立進行狀態轉換
└─ 遍歷每個 ApplyNo 進行處理
```

#### 補件狀態驗證
```
檢查點: CardStatus 是否為 "補件作業中"
├─ 如果是: 進入狀態轉換流程
│  ├─ 根據 Source + CardStep 判斷新狀態
│  └─ 新增進程和卡片記錄
└─ 其他狀態: 不進行轉換, 只記錄
```

---

## 資料處理

### 1. 補件案件查詢

```csharp
// 調用存儲過程
var supplementInfoList = await _scoreSharpDapperContext.ExecuteStoredProcedureAsync(
    "Usp_GetECardSupplementInfo",
    new { P_ID = req.ID }
);

// 取回的結構 (可能多筆):
// ├─ ApplyNo: 申請書編號
// ├─ ID: 身份證號
// ├─ CardStatus: 當前卡片狀態
// ├─ CardStep: 當前卡片步驟
// ├─ Source: 進件來源 (紙本/APP/ECARD)
// ├─ CardOwner: 卡主類別
// ├─ ApplyCardType: 申請卡別
// └─ 其他相關欄位
```

### 2. 補件案件分組

```csharp
// 按 ApplyNo 分組, 便於批量處理
var groupedByApplyNo = supplementInfoList
    .GroupBy(x => x.ApplyNo)
    .ToList();

// 遍歷每個 ApplyNo 組
foreach (var applyNoGroup in groupedByApplyNo) {
    var applyNo = applyNoGroup.Key;
    var supplementInfos = applyNoGroup.ToList();

    // 對每個 ApplyNo 進行後續處理...
}
```

### 3. 狀態轉換邏輯

```csharp
// 對每個補件案件進行狀態轉換

foreach (var supplementInfo in supplementInfos) {
    // 只有在 CardStatus = "補件作業中" 時才進行轉換
    if (supplementInfo.CardStatus != "補件作業中") {
        continue;  // 略過其他狀態
    }

    string newCardStatus;
    bool isPaperSource = (supplementInfo.Source == "紙本");

    if (supplementInfo.CardStep == "月收入確認") {
        newCardStatus = isPaperSource
            ? "紙本件_待月收入預審"
            : "網路件_待月收入預審";
    } else if (supplementInfo.CardStep == "人工徵審") {
        newCardStatus = "補回件";
    } else {
        // 其他步驟保持現狀
        newCardStatus = supplementInfo.CardStatus;
        continue;
    }

    // 進行狀態轉換處理...
}
```

### 4. 歷程記錄新增

#### 新增進程記錄

**Reviewer_ApplyCreditCardInfoProcess** (補件 MyData 失敗歷程)
```csharp
new Reviewer_ApplyCreditCardInfoProcess {
    ApplyNo = supplementInfo.ApplyNo,
    Process = newCardStatus,  // 轉換後的狀態
    ProcessUserId = "SYSTEM",
    StartTime = DateTime.Now,
    EndTime = DateTime.Now,
    Notes = "補件MyData取回失敗"
}
```

#### 新增卡片記錄

**Reviewer_CardRecord** (補件失敗卡片狀態記錄)
```csharp
new Reviewer_CardRecord {
    ApplyNo = supplementInfo.ApplyNo,
    CardStatus = newCardStatus,  // 新狀態
    CardOwner = supplementInfo.CardOwner,
    ApplyCardType = supplementInfo.ApplyCardType,
    ApproveUserId = "SYSTEM",
    AddTime = DateTime.Now,
    HandleNote = "補件MyData失敗狀態轉換"
}
```

#### 更新 Handle

**Reviewer_ApplyCreditCardInfoHandle** (更新處理檔)
```csharp
// 查詢現有的 Handle 記錄
var handle = await _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle
    .FirstOrDefaultAsync(h => h.ApplyNo == supplementInfo.ApplyNo);

if (handle != null) {
    handle.CardStatus = newCardStatus;
    handle.LastUpdateTime = DateTime.Now;
    handle.LastUpdateUserId = "SYSTEM";

    _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.Update(handle);
}
```

#### 更新 Main

**Reviewer_ApplyCreditCardInfoMain** (更新主檔)
```csharp
var main = await _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain
    .FirstOrDefaultAsync(m => m.ApplyNo == supplementInfo.ApplyNo);

if (main != null) {
    main.LastUpdateUserId = "SYSTEM";
    main.LastUpdateTime = DateTime.Now;

    _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.Update(main);
}
```

### 5. 分散式事務執行

```csharp
using var mainTransaction = await scoreSharpContext.Database.BeginTransactionAsync();

try
{
    // 步驟 1: 新增進程記錄 (所有案件)
    await scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess
        .AddRangeAsync(processes);

    // 步驟 2: 新增卡片記錄 (所有需要轉換的案件)
    if (cardRecords.Count > 0) {
        await scoreSharpContext.Reviewer_CardRecord
            .AddRangeAsync(cardRecords);
    }

    // 步驟 3: 更新所有相關的 Handle 和 Main 記錄
    // (已在上面的迴圈中調用 Update)

    // 步驟 4: 提交所有變更
    await scoreSharpContext.SaveChangesAsync();
    await mainTransaction.CommitAsync();

    return new EcardSupplementMyDataFailResponse {
        ID = req.ID,
        RESULT = "匯入成功"
    };
}
catch (Exception ex)
{
    logger.LogError("補件MyData失敗事務失敗: {@Error}", ex.ToString());
    await mainTransaction.RollbackAsync();

    return new EcardSupplementMyDataFailResponse {
        ID = req.ID,
        RESULT = "其他異常訊息"
    };
}
```

---

## 業務流程說明

### 補件 MyData 失敗流程圖

```
請求 EcardSupplementMyDataFail
│
├─ 步驟 1: 驗證必填欄位
│  ├─ ID: 不能為空
│  └─ 缺失 → 回覆代碼 0001
│
├─ 步驟 2: 驗證長度限制
│  ├─ ID.Length <= 11
│  └─ 超過 → 回覆代碼 0002
│
├─ 步驟 3: 查詢補件案件信息
│  ├─ 調用存儲過程: Usp_GetECardSupplementInfo
│  ├─ 傳入參數: P_ID (身份證號)
│  ├─ 可能回傳多筆補件案件
│  └─ 若無結果 → 回覆代碼 0003
│
├─ 步驟 4: 按 ApplyNo 分組補件案件
│  └─ 一個身份證號可對應多筆補件
│
├─ 步驟 5: 遍歷每個補件案件 (ApplyNo)
│  │
│  └─ 對於每個案件:
│     │
│     ├─ 步驟 5.1: 檢查 CardStatus
│     │  ├─ 若 ≠ "補件作業中" → 略過, 不轉換
│     │  └─ 若 = "補件作業中" → 進入轉換流程
│     │
│     ├─ 步驟 5.2: 判斷進件來源
│     │  ├─ Source = "紙本": 轉換為紙本件相關狀態
│     │  └─ Source = "APP"/"ECARD": 轉換為網路件相關狀態
│     │
│     ├─ 步驟 5.3: 判斷卡片步驟
│     │  ├─ CardStep = "月收入確認"
│     │  │  ├─ 紙本 → 紙本件_待月收入預審
│     │  │  └─ 網路 → 網路件_待月收入預審
│     │  │
│     │  └─ CardStep = "人工徵審"
│     │     └─ → 補回件
│     │
│     ├─ 步驟 5.4: 新增進程和卡片記錄
│     │  ├─ Reviewer_ApplyCreditCardInfoProcess
│     │  ├─ Reviewer_CardRecord
│     │  └─ 更新 Handle 和 Main
│     │
│     └─ 步驟 5.5: 移至下一個案件
│
├─ 步驟 6: 事務提交
│  ├─ 保存所有變更
│  ├─ 成功 → 回覆代碼 0000
│  └─ 失敗 → 自動回滾, 回覆代碼 0003
│
└─ 回應結果
```

### 狀態轉換決策樹

```
CardStatus 轉換邏輯:

查詢到的 CardStatus
│
├─ 補件作業中
│  │
│  ├─ CardStep = 月收入確認
│  │  ├─ Source = 紙本 → 紙本件_待月收入預審
│  │  └─ Source = APP/ECARD → 網路件_待月收入預審
│  │
│  └─ CardStep = 人工徵審
│     └─ → 補回件
│
└─ 其他狀態
   └─ → 保持現狀 (不轉換)
```

---

## 關鍵業務規則

### 1. 補件 MyData 失敗的特點

```
特殊性:
├─ 只需要一個身份證號 (ID) 就可查詢多筆補件案件
├─ 一個身份證號可對應多個申請書 (ApplyNo)
├─ 每個申請書獨立進行狀態轉換
├─ 無需檔案處理 (只進行狀態更新)
└─ 整體處理流程較為簡潔
```

### 2. 進件來源判斷

```
根據 Source 欄位決定新狀態:
├─ Source = "紙本": 轉換為紙本件相關狀態
├─ Source = "APP": 轉換為網路件相關狀態
└─ Source = "ECARD": 轉換為網路件相關狀態
```

### 3. CardStep 的重要性

```
CardStep 決定狀態轉換的具體方向:
├─ "月收入確認": 轉向月收入預審 (根據來源決定紙本/網路)
├─ "人工徵審": 轉向補回件 (無論來源)
└─ 其他步驟: 不進行轉換
```

### 4. 批量處理優化

```
單個 ID 可對應多筆補件:
├─ 利用分組減少數據庫查詢
├─ 批量新增進程和卡片記錄
├─ 使用 AddRangeAsync() 提高性能
└─ 統一事務管理確保數據一致性
```

---

## 涉及的資料表

### 新增表 (INSERT)

| 資料庫 | 表名 | 筆數 | 說明 |
|-------|------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoProcess | N | 每筆補件一個進程記錄 |
| ScoreSharp | Reviewer_CardRecord | N | 需要轉換的補件卡片記錄 |

### 更新表 (UPDATE)

| 資料庫 | 表名 | 條件 | 說明 |
|-------|------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoHandle | CardStatus = 補件作業中 | 更新 CardStatus |
| ScoreSharp | Reviewer_ApplyCreditCardInfoMain | 每筆 ApplyNo | 更新 LastUpdateTime/UserId |

### 查詢表 (SELECT)

| 資料庫 | 表名 | 用途 |
|-------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoHandle | 查詢現有卡片狀態 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoMain | 查詢申請主檔信息 |

### 存儲過程

| 存儲過程名 | 參數 | 返回值 |
|----------|------|--------|
| Usp_GetECardSupplementInfo | P_ID (身份證號) | 補件中的所有案件信息 (可多筆) |

---

## 異常處理

### 常見異常及處理方式

| 異常類型 | 原因 | 回覆代碼 | 處理方式 |
|---------|------|--------|--------|
| InvalidOperationException | 存儲過程無結果 (無補件案件) | 0003 | 驗證身份證號是否正確 |
| DbUpdateException | 事務執行失敗 | 0003 | 回滾事務, 記錄詳細錯誤 |
| FormatException | 長度驗證失敗 | 0002 | 驗證輸入格式 |

---

## 與其他 API 的對比

| 項目 | EcardNewCase | EcardSupplement | EcardMyDataSuccess | EcardMyDataFail | EcardSupplementMyDataFail |
|------|-------------|-----------------|------------------|-----------------|--------------------------|
| 必填參數 | ApplyNo, ID, ... | ID, SupNo | ID, ApplyNo, MyDataNo | ID, ApplyNo, MyDataNo | ID |
| 檔案處理 | ✅ 有 | ✅ 有 | ✅ 有 | ❌ 無 | ❌ 無 |
| 批量處理 | ❌ 無 (單筆) | ❌ 無 (單筆) | ❌ 無 (單筆) | ❌ 無 (單筆) | ✅ 有 (多筆) |
| 複雜度 | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐ |
| 涉及表 | 12+ | 6 | 6 | 3 | 4 |
| 事務類型 | 分散式 (2個DB) | 分散式 (2個DB) | 分散式 (2個DB) | 單一事務 | 單一事務 |

---

## 性能考慮

### 批量操作優化
- 使用 `AddRangeAsync()` 批量新增進程和卡片記錄
- 減少數據庫往返次數
- 單次事務提交多筆變更

### 分組優化
- 在內存中按 ApplyNo 分組
- 避免重複查詢數據庫
- 提高邏輯處理效率

### 事務管理
- 所有變更統一提交
- 保證數據一致性
- 異常時自動回滾

### 與檔案處理 API 的對比
- 不涉及 FTP 訪問
- 不涉及浮水印處理
- 整體性能更高
- 適合批量補件失敗通知

---

## 典型使用場景

### 場景 1: 單一補件案件失敗
```
身份證號: K12798732
回傳補件案件:
├─ ApplyNo: 20250508H5563
│  ├─ CardStatus: 補件作業中
│  ├─ CardStep: 月收入確認
│  └─ Source: APP

處理結果:
└─ CardStatus: 網路件_待月收入預審
```

### 場景 2: 同一人多筆補件同時失敗
```
身份證號: K12798732
回傳補件案件:
├─ ApplyNo: 20250508H5563
│  ├─ CardStatus: 補件作業中
│  ├─ CardStep: 月收入確認
│  └─ Source: APP
│  → 轉換為: 網路件_待月收入預審

├─ ApplyNo: 20250509H5564
│  ├─ CardStatus: 補件作業中
│  ├─ CardStep: 人工徵審
│  └─ Source: 紙本
│  → 轉換為: 補回件

└─ ApplyNo: 20250510H5565
   ├─ CardStatus: 不是補件作業中
   └─ → 不進行轉換
```

