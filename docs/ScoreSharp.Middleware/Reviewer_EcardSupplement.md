# EcardSupplement API - 補件 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerCore/EcardSupplement` |
| **HTTP 方法** | POST |
| **Content-Type** | application/x-www-form-urlencoded |
| **功能** | 處理信用卡申請的補件, 允許提交補充的附件文件 |
| **位置** | `Modules/Reviewer/ReviewerCore/EcardSupplement/Endpoint.cs` |

---

## Request 定義

### 請求體 (Form-Urlencoded 格式)

```csharp
public class EcardSupplementRequest
{
    [FormField("P_ID")]
    public string? ID { get; set; }  // 身份證號 (必填, 長度 <= 11)

    [FormField("SUP_NO")]
    public string? SupplementNo { get; set; }  // 補件編號 (必填, 長度 <= 20)

    [FormField("APPENDIX_FILE_NAME_01")]
    public string? AppendixFileName_01 { get; set; }  // 附件檔名 1 (長度 <= 100)

    [FormField("APPENDIX_FILE_NAME_02")]
    public string? AppendixFileName_02 { get; set; }  // 附件檔名 2 (長度 <= 100)

    [FormField("APPENDIX_FILE_NAME_03")]
    public string? AppendixFileName_03 { get; set; }  // 附件檔名 3 (長度 <= 100)

    [FormField("APPENDIX_FILE_NAME_04")]
    public string? AppendixFileName_04 { get; set; }  // 附件檔名 4 (長度 <= 100)

    [FormField("APPENDIX_FILE_NAME_05")]
    public string? AppendixFileName_05 { get; set; }  // 附件檔名 5 (長度 <= 100)

    [FormField("APPENDIX_FILE_NAME_06")]
    public string? AppendixFileName_06 { get; set; }  // 附件檔名 6 (長度 <= 100)

    [FormField("MYDATA_NO")]
    public string? MyDataNo { get; set; }  // MyData 案件編號 (長度 <= 36)
}
```

### 範例請求

```
P_ID=K12798732&
SUP_NO=20250409175522259&
APPENDIX_FILE_NAME_01=20250409_17552282159__3.jpg&
APPENDIX_FILE_NAME_02=&
APPENDIX_FILE_NAME_03=&
APPENDIX_FILE_NAME_04=&
APPENDIX_FILE_NAME_05=&
APPENDIX_FILE_NAME_06=&
MYDATA_NO=
```

### 必填欄位驗證清單

| 欄位 | 驗證規則 | 錯誤碼 |
|------|--------|--------|
| ID | 不能為空, 長度 <= 11 | 0001 / 0002 |
| SupplementNo | 不能為空, 長度 <= 20 | 0001 / 0002 |
| 檔名 (01~06) | 長度 <= 100 (如提供) | 0002 |

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
│  └─ 缺失 → 回覆代碼 0001
└─ SupplementNo: 不能為空
   └─ 缺失 → 回覆代碼 0001

檢查點 2: 長度驗證
├─ ID.Length <= 11
│  └─ 超過 → 回覆代碼 0002
├─ SupplementNo.Length <= 20
│  └─ 超過 → 回覆代碼 0002
└─ 檔名.Length <= 100 (各檔名)
   └─ 超過 → 回覆代碼 0002
```

### 2. 商業邏輯驗證 (BusinessValidation)

#### 補件案件查詢
```
檢查點: 查詢補件中的案件信息
├─ 來源: 存儲過程 Usp_GetECardSupplementInfo
├─ 查詢條件: ID (身份證號)
├─ 回傳: 補件中的所有案件 (ApplyNo, CardOwner, ApplyCardType, Source等)
├─ 若查無補件案件 → 回覆代碼 0003
└─ 續用取回的案件信息進行後續處理
```

#### 補件狀態驗證
```
檢查點: CardStatus 必須為 "補件作業中"
├─ 如果是補件狀態 → 進入補件處理流程
└─ 其他狀態 → 跳過, 只新增檔案歷程
```

#### 檔案來源驗證
```
檢查點: 從 FTP 伺服器獲取補件檔案
├─ 路徑: FixedEcardSupplementFolderPath (配置中)
├─ 檔名: 自 Request 中的 AppendixFileName_01~06 獲取
├─ 讀取二進位數據
└─ 若檔案不存在 → 回覆代碼 0003
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

// 取回的結構:
// ├─ ApplyNo: 申請書編號
// ├─ ID: 身份證號
// ├─ CardOwner: 卡主類別
// ├─ ApplyCardType: 申請卡別
// ├─ Source: 進件來源 (紙本/APP/ECARD)
// ├─ CardStatus: 當前卡片狀態
// ├─ CardStep: 當前卡片步驟
// └─ 其他相關欄位
```

### 2. 檔案獲取與浮水印

```csharp
// 遍歷 6 個補件檔案欄位
var appendixFiles = new List<AppendixFile>();
for (int i = 1; i <= 6; i++) {
    var fileName = req.GetProperty($"AppendixFileName_{i:00}");
    if (string.IsNullOrEmpty(fileName))
        continue;

    // 1. 從 FTP 獲取檔案
    var ftpPath = Path.Combine(_ftpOption.FixedEcardSupplementFolderPath, fileName);
    var fileBytes = await _ftpHelper.DownloadFileAsync(ftpPath);

    // 2. 浮水印處理
    byte[] processedBytes;
    if (Path.GetExtension(fileName).ToLower() == ".pdf") {
        processedBytes = await _watermarkHelper.PdfWatermarkAndGetBytes(
            fileBytes,
            _watermarkText
        );
    } else {
        // 圖片格式 (JPG, PNG 等)
        processedBytes = await _watermarkHelper.ImageWatermarkAndGetBytes(
            fileBytes,
            _watermarkText
        );
    }

    appendixFiles.Add(new AppendixFile {
        FileName = fileName,
        FileContent = processedBytes
    });
}
```

### 3. 狀態轉換邏輯

```csharp
// 根據 Source 和 CardStep 決定新的 CardStatus

if (supplementInfo.Source == "紙本") {
    // 紙本進件補件
    if (supplementInfo.CardStep == "月收入確認") {
        newCardStatus = "紙本件_待月收入預審";
    } else if (supplementInfo.CardStep == "人工徵審") {
        newCardStatus = "補回件";
    }
} else {
    // APP / ECARD 進件補件
    if (supplementInfo.CardStep == "月收入確認") {
        newCardStatus = "網路件_待月收入預審";
    } else if (supplementInfo.CardStep == "人工徵審") {
        newCardStatus = "補回件";
    }
}
```

### 4. 檔案處理和歷程新增

#### 檔案計數邏輯
```csharp
// 查詢既有檔案頁數
var maxPageNumber = await _scoreSharpContext.Reviewer_ApplyCreditCardInfoFile
    .Where(f => f.ApplyNo == supplementInfo.ApplyNo)
    .MaxAsync(f => f.Page);

int nextPageNumber = maxPageNumber + 1;
```

#### 新增實體

**Reviewer_ApplyFile** (檔案儲存 - File DB)
```csharp
new Reviewer_ApplyFile {
    FileId = GenerateGuid(),
    ApplyNo = supplementInfo.ApplyNo,
    FileContent = processedFileBytes,  // 已壓印浮水印
    FileName = $"{supplementInfo.ApplyNo}_{originalFileName}",
    FileType = "補件附件",
    AddTime = DateTime.Now
}
```

**Reviewer_ApplyCreditCardInfoFile** (檔案歷程)
```csharp
new Reviewer_ApplyCreditCardInfoFile {
    ApplyNo = supplementInfo.ApplyNo,
    FileId = GenerateGuid(),
    Page = nextPageNumber++,
    FileName = originalFileName,
    FileType = "補件",
    FileSize = processedFileBytes.Length,
    IsDelete = "N",
    AddTime = DateTime.Now,
    AddUserId = "SYSTEM",
    Note = "補件上傳",
    DBName = "ScoreSharp_File"
}
```

**Reviewer_ApplyCreditCardInfoProcess** (補件歷程)
```csharp
new Reviewer_ApplyCreditCardInfoProcess {
    ApplyNo = supplementInfo.ApplyNo,
    Process = newCardStatus,  // 轉換後的狀態
    ProcessUserId = "SYSTEM",
    StartTime = DateTime.Now,
    EndTime = DateTime.Now,
    Notes = "完成補件上傳"
}
```

**Reviewer_ApplyCreditCardInfoHandle** (更新處理檔)
```csharp
// 更新現有的 Handle 記錄
handle.CardStatus = newCardStatus;
handle.LastUpdateTime = DateTime.Now;
handle.LastUpdateUserId = "SYSTEM";

_scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.Update(handle);
```

**Reviewer_CardRecord** (卡片狀態記錄 - 條件性)
```csharp
// 如果 CardStatus 從 "補件作業中" 轉換為其他狀態
if (originalCardStatus == "補件作業中") {
    new Reviewer_CardRecord {
        ApplyNo = supplementInfo.ApplyNo,
        CardStatus = newCardStatus,
        CardOwner = supplementInfo.CardOwner,
        ApplyCardType = supplementInfo.ApplyCardType,
        ApproveUserId = "SYSTEM",
        AddTime = DateTime.Now,
        HandleNote = "補件完成狀態轉換"
    }
}
```

**Reviewer_ApplyCreditCardInfoMain** (更新主檔)
```csharp
// 更新最後修改時間
main.LastUpdateUserId = "SYSTEM";
main.LastUpdateTime = DateTime.Now;

_scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.Update(main);
```

### 5. 分散式事務執行

```csharp
using var fileTransaction = await scoreSharpFileContext.Database.BeginTransactionAsync();
using var mainTransaction = await scoreSharpContext.Database.BeginTransactionAsync();

try
{
    // 步驟 1: 存檔案至 File DB
    await scoreSharpFileContext.Reviewer_ApplyFile.AddRangeAsync(applyFiles);
    await scoreSharpFileContext.SaveChangesAsync();

    // 步驟 2: 新增檔案歷程
    await scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.AddRangeAsync(fileHistories);

    // 步驟 3: 新增進程記錄
    if (needsStatusTransition) {
        await scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);
        await scoreSharpContext.Reviewer_CardRecord.AddRangeAsync(cardRecords);
    }

    // 步驟 4: 更新 Handle 和 Main
    await scoreSharpContext.SaveChangesAsync();

    // 提交事務
    await fileTransaction.CommitAsync();
    await mainTransaction.CommitAsync();

    return new EcardSupplementResponse {
        ID = req.ID,
        RESULT = "匯入成功"
    };
}
catch (Exception ex)
{
    logger.LogError("補件分散式事務失敗: {@Error}", ex.ToString());
    await mainTransaction.RollbackAsync();
    await fileTransaction.RollbackAsync();

    return new EcardSupplementResponse {
        ID = req.ID,
        RESULT = "其他異常訊息"
    };
}
```

---

## 業務流程說明

### 補件流程圖

```
請求 EcardSupplement
│
├─ 步驟 1: 驗證必填欄位
│  ├─ ID: 不能為空
│  ├─ SupplementNo: 不能為空
│  └─ 失敗 → 回覆代碼 0001
│
├─ 步驟 2: 驗證長度限制
│  ├─ ID.Length <= 11
│  ├─ SupplementNo.Length <= 20
│  ├─ 檔名.Length <= 100
│  └─ 失敗 → 回覆代碼 0002
│
├─ 步驟 3: 查詢補件案件信息
│  ├─ 調用存儲過程: Usp_GetECardSupplementInfo
│  ├─ 傳入參數: P_ID (身份證號)
│  ├─ 回傳: 所有補件中的案件清單
│  └─ 若無結果 → 回覆代碼 0003
│
├─ 步驟 4: 遍歷每個補件案件 (ApplyNo)
│  │
│  └─ 對於每個案件:
│     │
│     ├─ 步驟 4.1: 檢查 CardStatus
│     │  ├─ 若 = "補件作業中" → 進入狀態轉換流程
│     │  └─ 其他 → 跳過狀態轉換, 只新增檔案歷程
│     │
│     ├─ 步驟 4.2: 遍歷補件檔案 (01~06)
│     │  ├─ 檔名為空 → 略過
│     │  ├─ 從 FTP 獲取檔案
│     │  └─ 壓印浮水印 (PDF 或圖片)
│     │
│     ├─ 步驟 4.3: 計算新 CardStatus
│     │  ├─ 根據 Source (紙本/APP/ECARD)
│     │  └─ 根據 CardStep (月收入確認/人工徵審)
│     │
│     └─ 步驟 4.4: 新增各業務實體
│        ├─ Reviewer_ApplyFile (File DB)
│        ├─ Reviewer_ApplyCreditCardInfoFile
│        ├─ Reviewer_ApplyCreditCardInfoProcess
│        ├─ Reviewer_CardRecord (如需要)
│        └─ 更新 Handle 和 Main
│
├─ 步驟 5: 分散式事務提交
│  ├─ File DB: 保存檔案
│  ├─ Main DB: 保存業務數據
│  ├─ 成功 → 回覆代碼 0000
│  └─ 失敗 → 自動回滾, 回覆代碼 0003
│
└─ 回應結果
```

### 狀態轉換決策樹

```
CardStatus 轉換邏輯:

補件作業中
│
├─ 紙本進件 + 月收入確認
│  └─ → 紙本件_待月收入預審
│
├─ 紙本進件 + 人工徵審
│  └─ → 補回件
│
├─ APP/ECARD 進件 + 月收入確認
│  └─ → 網路件_待月收入預審
│
├─ APP/ECARD 進件 + 人工徵審
│  └─ → 補回件
│
└─ 其他狀態
   └─ → 保持現狀 (不轉換)
```

---

## 關鍵業務規則

### 1. FTP 檔案下載

```
FTP 路徑配置:
├─ 基礎路徑: IFTPOption.FixedEcardSupplementFolderPath
├─ 完整路徑: {基礎路徑}/{檔名}
├─ 例如: /ecard/supplement/20250409_17552282159__3.jpg
└─ 若檔案不存在 → 回覆代碼 0003
```

### 2. 浮水印處理

```
浮水印文本:
├─ 來源: 配置中的 "WatermarkText"
├─ 應用於: 所有補件檔案 (PDF 和圖片)
├─ 格式:
│  ├─ PDF: 使用 PdfWatermarkAndGetBytes()
│  └─ 圖片: 使用 ImageWatermarkAndGetBytes()
└─ 異常 → 回覆代碼 0003
```

### 3. 檔案名稱規範

```
存儲名稱: {ApplyNo}_{原檔名}
├─ 例如: 20250508H5563_invoice.jpg
├─ 用途: 便於追蹤和查詢
└─ 在 Reviewer_ApplyFile 中存儲
```

### 4. 頁碼計算

```
新檔案的 Page 號:
├─ 查詢現有最大 Page 號: MAX(Page)
├─ 新檔案 Page = 最大 Page + 1
├─ 從 0 開始編號
└─ 用於檔案排序和管理
```

---

## 涉及的資料表

### 新增表 (INSERT)

| 資料庫 | 表名 | 筆數 | 說明 |
|-------|------|------|------|
| ScoreSharp_File | Reviewer_ApplyFile | N | 補件檔案內容 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoFile | N | 補件檔案歷程 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoProcess | 1~N | 補件進程記錄 |
| ScoreSharp | Reviewer_CardRecord | 1~N | 卡片狀態記錄 (如需要) |

### 更新表 (UPDATE)

| 資料庫 | 表名 | 條件 | 說明 |
|-------|------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoHandle | CardStatus 從補件作業中 | 更新新的 CardStatus |
| ScoreSharp | Reviewer_ApplyCreditCardInfoMain | ApplyNo | 更新 LastUpdateTime/UserId |

### 查詢表 (SELECT)

| 資料庫 | 表名 | 用途 |
|-------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoHandle | 查詢現有卡片狀態 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoMain | 查詢申請主檔信息 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoFile | 計算下一個 Page 號 |

### 存儲過程

| 存儲過程名 | 參數 | 返回值 |
|----------|------|--------|
| Usp_GetECardSupplementInfo | P_ID (身份證號) | 補件中的所有案件信息 |

---

## 異常處理

### 常見異常及處理方式

| 異常類型 | 原因 | 回覆代碼 | 處理方式 |
|---------|------|--------|--------|
| FileNotFoundException | FTP 中檔案不存在 | 0003 | 記錄錯誤, 通知檔案上傳 |
| DbUpdateException | 事務執行失敗 | 0003 | 回滾事務, 記錄詳細錯誤 |
| WatermarkException | 浮水印壓印失敗 | 0003 | 記錄錯誤, 聯繫技術支持 |
| InvalidOperationException | 存儲過程無結果 | 0003 | 檢查身份證號是否正確 |

---

## 性能考慮

### 批量操作
- 使用 `AddRangeAsync()` 批量新增檔案歷程
- 減少數據庫往返次數

### FTP 連接
- 維持 FTP 連接池
- 避免頻繁建立/關閉連接

### 事務管理
- 保持事務時間短
- 避免在事務中進行長時間的 I/O 操作

