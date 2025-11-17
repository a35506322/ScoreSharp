# EcardMyDataFail API - MyData 取件失敗 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerCore/EcardMyDataFail` |
| **HTTP 方法** | POST |
| **Content-Type** | application/json |
| **功能** | 處理 MyData 核驗失敗的情況, 更新卡片狀態並記錄失敗信息 |
| **位置** | `Modules/Reviewer/ReviewerCore/EcardMyDataFail/Endpoint.cs` |

---

## Request 定義

### 請求體 (JSON 格式)

```csharp
public class EcardMyDataFailRequest
{
    [JsonPropertyName("P_ID")]
    public string? ID { get; set; }  // 身份證號 (必填, 長度 <= 11)

    [JsonPropertyName("APPLY_NO")]
    public string? ApplyNo { get; set; }  // 申請書編號 (必填, 長度 <= 13)

    [JsonPropertyName("MYDATA_NO")]
    public string? MyDataNo { get; set; }  // MyData 案件編號 (必填, 長度 <= 36)
}
```

### 範例請求

```json
{
  "P_ID": "K12798732",
  "APPLY_NO": "20250508H5563",
  "MYDATA_NO": "e37b48ca-82da-49da-a605-bdc23b082186"
}
```

### 必填欄位驗證清單

| 欄位 | 驗證規則 | 錯誤碼 |
|------|--------|--------|
| ID | 不能為空, 長度 <= 11 | 0001 / 0002 |
| ApplyNo | 不能為空, 長度 <= 13 | 0001 / 0002 |
| MyDataNo | 不能為空, 長度 <= 36 | 0001 / 0002 |

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
├─ ApplyNo: 不能為空
├─ MyDataNo: 不能為空
└─ 任一缺失 → 回覆代碼 0001

檢查點 2: 長度驗證
├─ ID.Length <= 11
├─ ApplyNo.Length <= 13
├─ MyDataNo.Length <= 36
└─ 任一超過 → 回覆代碼 0002
```

### 2. 商業邏輯驗證 (BusinessValidation)

#### 申請信息查詢
```
檢查點: 查詢 MyData 相關的申請案件
├─ 來源: 存儲過程 Usp_GetECardMyDataInfo
├─ 查詢條件: ApplyNo, ID, MyDataNo
├─ 回傳: 申請主檔信息, 現有卡片狀態, CardStep
├─ 若查無案件 → 回覆代碼 0003
└─ 續用取回的案件信息進行後續處理
```

#### 卡片狀態驗證
```
檢查點: 驗證 CardStatus 是否為等待 MyData 狀態
├─ 允許的狀態:
│  ├─ "網路件_等待MyData附件" 或
│  └─ "網路件_書面申請等待MyData"
├─ 其他狀態 → 不進行轉換, 只記錄失敗
└─ 狀態符合 → 進入狀態轉換流程
```

---

## 資料處理

### 1. 申請信息查詢

```csharp
// 調用存儲過程
var myDataInfo = await _scoreSharpDapperContext.ExecuteStoredProcedureAsync(
    "Usp_GetECardMyDataInfo",
    new {
        P_APPLY_NO = req.ApplyNo,
        P_ID = req.ID,
        P_MYDATA_NO = req.MyDataNo
    }
);

// 取回的結構:
// ├─ ApplyNo: 申請書編號
// ├─ ID: 身份證號
// ├─ MyDataNo: MyData 案件編號
// ├─ CardStatus: 當前卡片狀態
// ├─ Source: 進件來源 (APP/ECARD/紙本)
// ├─ CardStep: 當前卡片步驟
// └─ 其他相關欄位
```

### 2. 狀態轉換邏輯

```csharp
// 根據現有 CardStatus 和 Source 決定新的 CardStatus
string newCardStatus;

// 判斷進件來源
bool isPaperSource = (myDataInfo.Source == "紙本");

if (myDataInfo.CardStatus == "網路件_等待MyData附件" ||
    myDataInfo.CardStatus == "網路件_書面申請等待MyData") {

    // 判斷卡片步驟
    if (myDataInfo.CardStep == "月收入確認") {
        newCardStatus = isPaperSource
            ? "紙本件_待月收入預審"
            : "網路件_待月收入預審";
    } else if (myDataInfo.CardStep == "人工徵審") {
        newCardStatus = "補回件";
    } else {
        // 其他步驟保持現狀
        newCardStatus = myDataInfo.CardStatus;
    }
} else {
    // 不在等待 MyData 狀態: 不轉換
    newCardStatus = myDataInfo.CardStatus;
}
```

### 3. 歷程記錄新增

#### 新增進程記錄

**Reviewer_ApplyCreditCardInfoProcess** (MyData 失敗歷程)
```csharp
new Reviewer_ApplyCreditCardInfoProcess {
    ApplyNo = myDataInfo.ApplyNo,
    Process = newCardStatus,  // 轉換後的狀態 (或原狀態)
    ProcessUserId = "SYSTEM",
    StartTime = DateTime.Now,
    EndTime = DateTime.Now,
    Notes = $"MyData取回失敗;MyDataNo:{req.MyDataNo};FailureReason:系統回傳失敗信息"
}
```

#### 更新 Handle

**Reviewer_ApplyCreditCardInfoHandle** (更新處理檔)
```csharp
// 如果需要狀態轉換 (從等待 MyData 轉為其他狀態)
if (shouldTransition) {
    handle.CardStatus = newCardStatus;
    handle.LastUpdateTime = DateTime.Now;
    handle.LastUpdateUserId = "SYSTEM";

    _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.Update(handle);
}
```

#### 更新 Main

**Reviewer_ApplyCreditCardInfoMain** (更新主檔)
```csharp
main.LastUpdateUserId = "SYSTEM";
main.LastUpdateTime = DateTime.Now;

_scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.Update(main);
```

### 4. 分散式事務執行

```csharp
using var mainTransaction = await scoreSharpContext.Database.BeginTransactionAsync();

try
{
    // 步驟 1: 新增進程記錄
    await scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddAsync(process);

    // 步驟 2: 如需要轉換, 更新 Handle
    if (shouldTransition) {
        _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.Update(handle);
    }

    // 步驟 3: 更新 Main
    _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.Update(main);

    // 提交事務
    await scoreSharpContext.SaveChangesAsync();
    await mainTransaction.CommitAsync();

    return new EcardMyDataFailResponse {
        ID = req.ID,
        RESULT = "匯入成功"
    };
}
catch (Exception ex)
{
    logger.LogError("MyData失敗事務失敗: {@Error}", ex.ToString());
    await mainTransaction.RollbackAsync();

    return new EcardMyDataFailResponse {
        ID = req.ID,
        RESULT = "其他異常訊息"
    };
}
```

---

## 業務流程說明

### MyData 失敗流程圖

```
請求 EcardMyDataFail
│
├─ 步驟 1: 驗證必填欄位
│  ├─ ID, ApplyNo, MyDataNo: 都必填
│  └─ 缺失 → 回覆代碼 0001
│
├─ 步驟 2: 驗證長度限制
│  ├─ ID.Length <= 11
│  ├─ ApplyNo.Length <= 13
│  ├─ MyDataNo.Length <= 36
│  └─ 超過 → 回覆代碼 0002
│
├─ 步驟 3: 查詢 MyData 申請信息
│  ├─ 調用存儲過程: Usp_GetECardMyDataInfo
│  ├─ 傳入參數: ApplyNo, ID, MyDataNo
│  ├─ 回傳: 申請信息, 現有卡片狀態, CardStep
│  └─ 若無結果 → 回覆代碼 0003
│
├─ 步驟 4: 驗證卡片狀態
│  ├─ 檢查是否為 "網路件_等待MyData附件" 或 "網路件_書面申請等待MyData"
│  ├─ 如果是:
│  │  ├─ 根據 Source + CardStep 判斷新狀態
│  │  ├─ 月收入確認 + 紙本 → 紙本件_待月收入預審
│  │  ├─ 月收入確認 + APP/ECARD → 網路件_待月收入預審
│  │  └─ 人工徵審 + 任意 → 補回件
│  └─ 如果否: 不轉換, 只記錄失敗
│
├─ 步驟 5: 新增進程記錄
│  ├─ Reviewer_ApplyCreditCardInfoProcess
│  ├─ Notes: "MyData取回失敗;MyDataNo:..."
│  └─ Process: 新狀態 (或原狀態)
│
├─ 步驟 6: 更新卡片狀態 (如需要)
│  ├─ 更新 Handle: CardStatus = 新狀態
│  └─ 更新 Main: LastUpdateTime/UserId
│
├─ 步驟 7: 事務提交
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
├─ 網路件_等待MyData附件 或 網路件_書面申請等待MyData
│  │
│  ├─ CardStep = 月收入確認
│  │  ├─ Source = 紙本 → 紙本件_待月收入預審
│  │  └─ Source = APP/ECARD → 網路件_待月收入預審
│  │
│  └─ CardStep = 人工徵審
│     └─ → 補回件
│
└─ 其他狀態 (不在上述列表中)
   └─ → 保持現狀 (只記錄失敗, 不轉換)
```

---

## 關鍵業務規則

### 1. MyData 失敗信息記錄

```
在進程記錄中記錄:
├─ Process: 目標狀態
├─ Notes: "MyData取回失敗;MyDataNo:...;FailureReason:..."
├─ ProcessUserId: "SYSTEM"
└─ 保留失敗時間戳便於追蹤
```

### 2. 狀態轉換條件

```
只有在特定卡片狀態下才進行轉換:
├─ "網路件_等待MyData附件": 非卡友進件
└─ "網路件_書面申請等待MyData": 書面申請進件

其他狀態 (如已轉換為檢核狀態) 不再轉換
```

### 3. 進件來源判斷

```
根據 Source 欄位決定新狀態:
├─ Source = "紙本": 轉換為紙本件相關狀態
└─ Source = "APP" 或 "ECARD": 轉換為網路件相關狀態
```

### 4. CardStep 的重要性

```
CardStep 決定狀態轉換的具體方向:
├─ "月收入確認": 轉向月收入預審
├─ "人工徵審": 轉向補回件 (無論來源)
└─ 其他步驟: 不進行轉換
```

---

## 涉及的資料表

### 新增表 (INSERT)

| 資料庫 | 表名 | 筆數 | 說明 |
|-------|------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoProcess | 1 | MyData 失敗進程記錄 |

### 更新表 (UPDATE)

| 資料庫 | 表名 | 條件 | 說明 |
|-------|------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoHandle | 需要狀態轉換 | 更新 CardStatus |
| ScoreSharp | Reviewer_ApplyCreditCardInfoMain | 總是更新 | 更新 LastUpdateTime/UserId |

### 查詢表 (SELECT)

| 資料庫 | 表名 | 用途 |
|-------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoHandle | 查詢現有卡片狀態 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoMain | 查詢申請主檔信息 |

### 存儲過程

| 存儲過程名 | 參數 | 返回值 |
|----------|------|--------|
| Usp_GetECardMyDataInfo | ApplyNo, ID, MyDataNo | MyData 相關的申請信息 |

---

## 異常處理

### 常見異常及處理方式

| 異常類型 | 原因 | 回覆代碼 | 處理方式 |
|---------|------|--------|--------|
| InvalidOperationException | 存儲過程無結果 (申請不存在) | 0003 | 驗證 ApplyNo, ID, MyDataNo 正確性 |
| DbUpdateException | 事務執行失敗 | 0003 | 回滾事務, 記錄詳細錯誤 |
| FormatException | 日期/長度驗證失敗 | 0002/0001 | 驗證輸入格式 |

---

## 與 EcardMyDataSuccess 的對比

| 方面 | 成功情況 | 失敗情況 |
|------|---------|---------|
| 檔案處理 | 保存 MyData 附件檔案 | 無檔案保存 |
| 進程記錄 | "MyData取件成功" | "MyData取回失敗" |
| FTP 訪問 | 需要從 FTP 讀取檔案 | 不需要 FTP 訪問 |
| 數據庫操作 | 分散式事務 (2 個 DB) | 單一事務 (Main DB) |
| 狀態轉換 | 轉換為檢核相關狀態 | 轉換為預審/補回狀態 |
| 複雜度 | 較高 (檔案處理) | 較低 (狀態更新) |

---

## 性能考慮

### 簡化操作
- 不涉及文件 I/O 和浮水印處理
- 僅需更新數據庫狀態
- 整體性能高

### 存儲過程調用
- 依賴存儲過程的查詢性能
- 應確保存儲過程索引充分

### 事務隔離
- 使用單個事務即可
- 避免長時間持有事務鎖

