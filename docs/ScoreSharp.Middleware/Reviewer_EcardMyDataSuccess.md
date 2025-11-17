# EcardMyDataSuccess API - MyData 取件成功 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerCore/EcardMyDataSuccess` |
| **HTTP 方法** | POST |
| **Content-Type** | application/x-www-form-urlencoded |
| **功能** | 處理 MyData 核驗成功的情況, 更新申請信息並保存相關附件 |
| **位置** | `Modules/Reviewer/ReviewerCore/EcardMyDataSuccess/Endpoint.cs` |

---

## Request 定義

### 請求體 (Form-Urlencoded 格式)

```csharp
public class EcardMyDataSuccessRequest
{
    [FormField("P_ID")]
    public string? ID { get; set; }  // 身份證號 (必填, 長度 <= 11)

    [FormField("APPLY_NO")]
    public string? ApplyNo { get; set; }  // 申請書編號 (必填, 長度 <= 13)

    [FormField("MYDATA_NO")]
    public string? MyDataNo { get; set; }  // MyData 案件編號 (必填, 長度 <= 36)

    [FormField("APPENDIX_FILE_NAME_01")]
    public string? AppendixFileName_01 { get; set; }  // MyData 附件檔名 1 (長度 <= 100)

    [FormField("APPENDIX_FILE_NAME_02")]
    public string? AppendixFileName_02 { get; set; }  // MyData 附件檔名 2 (長度 <= 100)

    [FormField("APPENDIX_FILE_NAME_03")]
    public string? AppendixFileName_03 { get; set; }  // MyData 附件檔名 3 (長度 <= 100)

    [FormField("APPENDIX_FILE_NAME_04")]
    public string? AppendixFileName_04 { get; set; }  // MyData 附件檔名 4 (長度 <= 100)

    [FormField("APPENDIX_FILE_NAME_05")]
    public string? AppendixFileName_05 { get; set; }  // MyData 附件檔名 5 (長度 <= 100)

    [FormField("APPENDIX_FILE_NAME_06")]
    public string? AppendixFileName_06 { get; set; }  // MyData 附件檔名 6 (長度 <= 100)
}
```

### 範例請求

```
P_ID=K12798732&
APPLY_NO=20250508H5563&
MYDATA_NO=e37b48ca-82da-49da-a605-bdc23b082186&
APPENDIX_FILE_NAME_01=20250409_17552282159__1.jpg&
APPENDIX_FILE_NAME_02=20250409_17552282159__2.jpg&
APPENDIX_FILE_NAME_03=&
APPENDIX_FILE_NAME_04=&
APPENDIX_FILE_NAME_05=&
APPENDIX_FILE_NAME_06=
```

### 必填欄位驗證清單

| 欄位 | 驗證規則 | 錯誤碼 |
|------|--------|--------|
| ID | 不能為空, 長度 <= 11 | 0001 / 0002 |
| ApplyNo | 不能為空, 長度 <= 13 | 0001 / 0002 |
| MyDataNo | 不能為空, 長度 <= 36 | 0001 / 0002 |
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
├─ ApplyNo: 不能為空
├─ MyDataNo: 不能為空
└─ 任一缺失 → 回覆代碼 0001

檢查點 2: 長度驗證
├─ ID.Length <= 11
├─ ApplyNo.Length <= 13
├─ MyDataNo.Length <= 36
├─ 檔名.Length <= 100 (各檔名)
└─ 任一超過 → 回覆代碼 0002
```

### 2. 商業邏輯驗證 (BusinessValidation)

#### 申請信息查詢
```
檢查點: 查詢 MyData 相關的申請案件
├─ 來源: 存儲過程 Usp_GetECardMyDataInfo
├─ 查詢條件: ApplyNo, ID, MyDataNo
├─ 回傳: 申請主檔信息, 處理檔信息, 現有卡片狀態
├─ 若查無案件 → 回覆代碼 0003
└─ 續用取回的案件信息進行後續處理
```

#### 卡片狀態驗證
```
檢查點: 驗證 CardStatus 是否符合預期
├─ 允許的狀態:
│  ├─ "網路件_等待MyData附件" 或
│  └─ "網路件_書面申請等待MyData"
├─ 其他狀態 → 不進行狀態轉換, 只保存檔案
└─ 狀態符合 → 進入狀態轉換流程
```

#### 檔案來源驗證
```
檢查點: 從 FTP 伺服器獲取 MyData 附件檔案
├─ 路徑: FixedEcardSupplementFolderPath (配置中)
├─ 檔名: 自 Request 中的 AppendixFileName_01~06 獲取
├─ 讀取二進位數據
└─ 若檔案不存在 → 回覆代碼 0003
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
// ├─ CardStatus: 當前卡片狀態 (需判斷是否為指定狀態)
// ├─ Source: 進件來源 (APP/ECARD/紙本)
// ├─ CardStep: 當前卡片步驟
// ├─ CardOwner: 卡主類別
// ├─ ApplyCardType: 申請卡別
// └─ 其他相關欄位
```

### 2. 檔案獲取與浮水印

```csharp
// 遍歷 6 個 MyData 附件檔案欄位
var myDataFiles = new List<MyDataFile>();
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

    myDataFiles.Add(new MyDataFile {
        FileName = fileName,
        FileContent = processedBytes
    });
}
```

### 3. 狀態轉換邏輯

```csharp
// 根據現有 CardStatus 決定新的 CardStatus
string newCardStatus;

if (myDataInfo.CardStatus == "網路件_等待MyData附件") {
    // 非卡友進件 → 待檢核
    newCardStatus = "網路件_非卡友_待檢核";
} else if (myDataInfo.CardStatus == "網路件_書面申請等待MyData") {
    // 書面申請進件 → 等待列印申請書及回郵信封
    newCardStatus = "網路件_書面申請等待列印申請書及回郵信封";
} else {
    // 其他狀態: 不進行轉換, 只保存檔案
    newCardStatus = myDataInfo.CardStatus;
}
```

### 4. 檔案處理和歷程新增

#### 檔案計數邏輯
```csharp
// 查詢既有檔案頁數
var maxPageNumber = await _scoreSharpContext.Reviewer_ApplyCreditCardInfoFile
    .Where(f => f.ApplyNo == myDataInfo.ApplyNo)
    .MaxAsync(f => f.Page);

int nextPageNumber = maxPageNumber + 1;
```

#### 新增實體

**Reviewer_ApplyFile** (檔案儲存 - File DB)
```csharp
new Reviewer_ApplyFile {
    FileId = GenerateGuid(),
    ApplyNo = myDataInfo.ApplyNo,
    FileContent = processedFileBytes,  // 已壓印浮水印
    FileName = $"{myDataInfo.ApplyNo}_{originalFileName}",
    FileType = "MyData附件",
    AddTime = DateTime.Now
}
```

**Reviewer_ApplyCreditCardInfoFile** (檔案歷程)
```csharp
new Reviewer_ApplyCreditCardInfoFile {
    ApplyNo = myDataInfo.ApplyNo,
    FileId = GenerateGuid(),
    Page = nextPageNumber++,
    FileName = originalFileName,
    FileType = "MyData",
    FileSize = processedFileBytes.Length,
    IsDelete = "N",
    AddTime = DateTime.Now,
    AddUserId = "SYSTEM",
    Note = $"MyData取件成功;MyDataNo:{req.MyDataNo}",
    DBName = "ScoreSharp_File"
}
```

**Reviewer_ApplyCreditCardInfoProcess** (MyData 成功歷程)
```csharp
// 只在狀態需要轉換時新增
if (needsStatusTransition) {
    new Reviewer_ApplyCreditCardInfoProcess {
        ApplyNo = myDataInfo.ApplyNo,
        Process = newCardStatus,  // 轉換後的狀態
        ProcessUserId = "SYSTEM",
        StartTime = DateTime.Now,
        EndTime = DateTime.Now,
        Notes = $"MyData取件成功;MyDataNo:{req.MyDataNo}"
    }
}
```

**Reviewer_ApplyCreditCardInfoHandle** (更新處理檔)
```csharp
// 更新現有的 Handle 記錄 (只在狀態需要轉換時)
if (needsStatusTransition) {
    handle.CardStatus = newCardStatus;
    handle.LastUpdateTime = DateTime.Now;
    handle.LastUpdateUserId = "SYSTEM";

    _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.Update(handle);
}
```

**Reviewer_ApplyCreditCardInfoMain** (更新主檔)
```csharp
main.LastUpdateUserId = "SYSTEM";
main.LastUpdateTime = DateTime.Now;
main.MyDataNo = req.MyDataNo;  // 記錄 MyData 案件編號

_scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.Update(main);
```

### 5. 分散式事務執行

```csharp
using var fileTransaction = await scoreSharpFileContext.Database.BeginTransactionAsync();
using var mainTransaction = await scoreSharpContext.Database.BeginTransactionAsync();

try
{
    // 步驟 1: 存檔案至 File DB
    if (myDataFiles.Count > 0) {
        await scoreSharpFileContext.Reviewer_ApplyFile.AddRangeAsync(applyFiles);
        await scoreSharpFileContext.SaveChangesAsync();
    }

    // 步驟 2: 新增檔案歷程
    await scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.AddRangeAsync(fileHistories);

    // 步驟 3: 如需要狀態轉換
    if (needsStatusTransition) {
        await scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddAsync(process);
    }

    // 步驟 4: 更新 Handle 和 Main
    await scoreSharpContext.SaveChangesAsync();

    // 提交事務
    await fileTransaction.CommitAsync();
    await mainTransaction.CommitAsync();

    return new EcardMyDataSuccessResponse {
        ID = req.ID,
        RESULT = "匯入成功"
    };
}
catch (Exception ex)
{
    logger.LogError("MyData成功分散式事務失敗: {@Error}", ex.ToString());
    await mainTransaction.RollbackAsync();
    await fileTransaction.RollbackAsync();

    return new EcardMyDataSuccessResponse {
        ID = req.ID,
        RESULT = "其他異常訊息"
    };
}
```

---

## 業務流程說明

### MyData 成功流程圖

```
請求 EcardMyDataSuccess
│
├─ 步驟 1: 驗證必填欄位
│  ├─ ID, ApplyNo, MyDataNo: 都必填
│  └─ 缺失 → 回覆代碼 0001
│
├─ 步驟 2: 驗證長度限制
│  ├─ ID.Length <= 11
│  ├─ ApplyNo.Length <= 13
│  ├─ MyDataNo.Length <= 36
│  ├─ 檔名.Length <= 100
│  └─ 超過 → 回覆代碼 0002
│
├─ 步驟 3: 查詢 MyData 申請信息
│  ├─ 調用存儲過程: Usp_GetECardMyDataInfo
│  ├─ 傳入參數: ApplyNo, ID, MyDataNo
│  ├─ 回傳: 申請信息, 現有卡片狀態
│  └─ 若無結果 → 回覆代碼 0003
│
├─ 步驟 4: 驗證卡片狀態
│  ├─ 允許的狀態:
│  │  ├─ "網路件_等待MyData附件" → 轉換為 "網路件_非卡友_待檢核"
│  │  └─ "網路件_書面申請等待MyData" → 轉換為 "網路件_書面申請等待列印申請書及回郵信封"
│  ├─ 其他狀態 → 不轉換, 只保存檔案
│  └─ CardStatus 不符合 → 不進行狀態轉換
│
├─ 步驟 5: 遍歷 MyData 附件檔案 (01~06)
│  ├─ 檔名為空 → 略過
│  ├─ 從 FTP 獲取檔案
│  └─ 壓印浮水印 (PDF 或圖片)
│
├─ 步驟 6: 計算檔案頁碼
│  ├─ 查詢現有最大 Page 號
│  └─ 新檔案 Page = 最大 Page + 1
│
├─ 步驟 7: 新增各業務實體
│  ├─ Reviewer_ApplyFile (File DB)
│  ├─ Reviewer_ApplyCreditCardInfoFile
│  ├─ Reviewer_ApplyCreditCardInfoProcess (如需轉換)
│  ├─ 更新 Handle (如需轉換)
│  └─ 更新 Main
│
├─ 步驟 8: 分散式事務提交
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

查詢到的 CardStatus
│
├─ 網路件_等待MyData附件
│  └─ → 網路件_非卡友_待檢核
│
├─ 網路件_書面申請等待MyData
│  └─ → 網路件_書面申請等待列印申請書及回郵信封
│
└─ 其他狀態 (不在上述列表中)
   └─ → 保持現狀 (不轉換, 只保存檔案)
```

---

## 關鍵業務規則

### 1. MyData 案件編號追蹤

```
在 Reviewer_ApplyCreditCardInfoMain 中記錄:
├─ MyDataNo: 外部系統的 MyData 案件編號
├─ 用於追蹤 MyData 核驗流程
└─ 檔案歷程中也記錄: "MyDataNo:{req.MyDataNo}"
```

### 2. 狀態轉換條件

```
只有在以下兩個卡片狀態下才進行轉換:
├─ "網路件_等待MyData附件": 表示非卡友進件, 等待 MyData 提供附件
└─ "網路件_書面申請等待MyData": 表示書面申請進件, 等待 MyData 提供附件

其他狀態 (如已轉換為檢核狀態) 不再轉換, 只保存檔案為歷程記錄
```

### 3. 檔案儲存位置

```
所有 MyData 附件存儲在:
├─ File DB: Reviewer_ApplyFile (實際二進位數據)
├─ Main DB: Reviewer_ApplyCreditCardInfoFile (元數據)
└─ 檔案名稱: {ApplyNo}_{原檔名}
```

### 4. 浮水印應用

```
所有 MyData 附件強制壓印浮水印:
├─ PDF: 使用 PdfWatermarkAndGetBytes()
├─ 圖片: 使用 ImageWatermarkAndGetBytes()
└─ 浮水印文本: 配置中的 "WatermarkText"
```

---

## 涉及的資料表

### 新增表 (INSERT)

| 資料庫 | 表名 | 筆數 | 說明 |
|-------|------|------|------|
| ScoreSharp_File | Reviewer_ApplyFile | N | MyData 附件檔案內容 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoFile | N | MyData 檔案歷程 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoProcess | 0~1 | MyData 成功進程 (如需轉換) |

### 更新表 (UPDATE)

| 資料庫 | 表名 | 條件 | 說明 |
|-------|------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoHandle | 需要狀態轉換 | 更新 CardStatus |
| ScoreSharp | Reviewer_ApplyCreditCardInfoMain | 總是更新 | 更新 LastUpdateTime/UserId, 記錄 MyDataNo |

### 查詢表 (SELECT)

| 資料庫 | 表名 | 用途 |
|-------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoHandle | 查詢現有卡片狀態 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoMain | 查詢申請主檔信息 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoFile | 計算下一個 Page 號 |

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
| FileNotFoundException | FTP 中檔案不存在 | 0003 | 檢查檔案是否已上傳至 FTP |
| DbUpdateException | 事務執行失敗 | 0003 | 回滾事務, 記錄詳細錯誤 |
| WatermarkException | 浮水印壓印失敗 | 0003 | 記錄錯誤, 聯繫技術支持 |

---

## 性能考慮

### 批量操作
- 使用 `AddRangeAsync()` 批量新增檔案歷程
- 減少數據庫往返次數

### 狀態轉換判斷
- 利用快取減少數據庫查詢
- 預先加載附件列表

### FTP 連接
- 維持 FTP 連接池
- 避免頻繁建立/關閉連接

