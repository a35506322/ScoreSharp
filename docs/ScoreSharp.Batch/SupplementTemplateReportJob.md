# SupplementTemplateReportJob - 報表作業補件函批次

## Job 基本資訊

- **Job 名稱**: SupplementTemplateReportJob
- **顯示名稱**: 報表作業_補件函批次 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 報表作業_補件函批次
- **工作日檢查**: 是

## 功能說明

報表作業補件函批次排程負責自動產生補件函報表，包括帶簽名的 Word 文件和不含簽名的文字檔案。此排程會查詢需要補件的案件，使用 Word 模板引擎產生客製化補件通知函，並產生用於簡訊通知的文字檔案，最後將報表儲存到指定路徑供後續列印和發送。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.SupplementTemplateReport_IsEnabled` 是否為 "Y"
- 若為 "N"，則記錄日誌並結束排程

### 模板設定驗證
- 檢查補件函模板檔案是否存在: `Files/補件函含簽名Template.docx`
- 檢查 `SysParamManage_MailSet` 樣板固定值設定是否存在

## 商業邏輯

### 主要流程

1. **檢查系統參數設定**
   - 驗證排程是否啟用

2. **取得需補件案件**
   - 查詢狀態為「10231」(待補件) 的案件
   - 條件: `BatchSupplementStatus = 'N'` 且 `IsPrintSMSAndPaper = 'Y'`
   - 使用 Dapper 執行 SQL 查詢

3. **取得補件原因對照表**
   - 從 `SetUp_SupplementReason` 取得補件代碼與原因名稱對照
   - 將補件代碼轉換為補件原因名稱

4. **取得樣板固定值設定**
   - 從 `SetUp_TemplateFixContent` 取得模板固定值
   - 包含卡片網址、電子郵件、分機、服務電話、QR Code 網址

5. **產生補件函簽名報表**
   - 使用 Word 模板引擎填入資料
   - 產生帶簽名的 Word 文件
   - 儲存到指定路徑

6. **產生補件函不含簽名報表**
   - 產生簡訊通知用的文字檔
   - 格式: `姓名|手機號碼`
   - 儲存到指定路徑

7. **更新資料庫**
   - 使用 TransactionScope 確保資料一致性
   - 新增報表下載記錄到 `Report_BatchReportDownload`
   - 更新案件 `BatchSupplementStatus = 'Y'`
   - 記錄 `BatchSupplementTime`

### 補件函模板欄位

**上文資訊**:
- `@zipCode`: 郵遞區號
- `@fullAddr`: 完整地址
- `@chName`: 申請人姓名
- `@applyNo`: 申請書編號

**補件原因**:
- `@supplementReasonNames`: 補件原因清單（每行一項，前綴「．」）

**日期資訊**:
- `@year`: 民國年
- `@month`: 月份 (兩位數)
- `@day`: 日期 (兩位數)

**聯絡資訊**:
- `@cardUrl`: 卡片網址
- `@email`: 電子郵件
- `@ext`: 分機
- `@servicePhone`: 服務電話
- `@qrCodeUrl`: QR Code 圖片

### 地址組合邏輯

地址欄位依序組合：
1. 城市 (City)
2. 區域 (District)
3. 路段 (Road) + "路"
4. 巷 (Lane) + "巷"
5. 弄 (Alley) + "弄"
6. 號 (Number) + "號"
7. 之 (SubNumber) 前綴"之"
8. 樓 (Floor) + "樓"
9. 其他 (Other) + "號"

### 補件原因轉換

從 `SupplementReasonCode` (逗號分隔) 轉換為 `SupplementReasonNames` 陣列：
```csharp
x.SupplementReasonNames = x
    .SupplementReasonCode.Split(',')
    .Select(code => supplementReasonNamesDic.GetValueOrDefault(code, ""))
    .ToArray();
```

### 報表檔案命名

- **補件函簽名**: `補件函簽名{yyyyMMddHHmm}.docx`
- **補件函不含簽名**: `補件函不含簽名{yyyyMMddHHmm}.txt`

### QR Code 產生

- 使用 `QRCodeGenerator` 產生 QR Code
- 錯誤修正等級: Q
- 輸出格式: PNG (byte[])
- 像素大小: 20

### 錯誤處理

- 報表儲存失敗會記錄到 `System_ErrorLog`
- 資料庫更新使用 TransactionScope 確保一致性
- 任何錯誤都會記錄詳細訊息並中止執行

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 使用 Dapper 查詢需補件案件
- 查詢 `SetUp_SupplementReason` 取得補件原因對照
- 查詢 `SetUp_TemplateFixContent` 取得模板固定值

**新增操作**:
- 新增 `Report_BatchReportDownload` 報表下載記錄
- 新增 `System_ErrorLog` 錯誤日誌

**更新操作**:
- 使用 Dapper 批次更新 `Reviewer_ApplyCreditCardInfoHandle.BatchSupplementStatus`
- 使用 Dapper 批次更新 `Reviewer_ApplyCreditCardInfoHandle.BatchSupplementTime`

### 使用 Dapper 查詢待補件案件

```sql
SELECT H.[ApplyNo], H.[ID], H.[UserType],
       H.[CardStatus], H.[BatchSupplementStatus],
       H.[BatchSupplementTime], H.SupplementReasonCode,
       H.SupplementSendCardAddr,
       M.CHName, M.Mobile,
       CASE H.SupplementSendCardAddr
           WHEN 1 THEN M.Bill_ZipCode
           WHEN 2 THEN M.Reg_ZipCode
           WHEN 3 THEN M.Comp_ZipCode
           WHEN 4 THEN M.Live_ZipCode
       END AS 'ZipCode',
       -- ... (地址各欄位)
       H.SeqNo as 'HandleSeqNo'
FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] H
JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] M
    ON H.ApplyNo = M.ApplyNo
WHERE H.CardStatus = 10231
  AND H.BatchSupplementStatus = 'N'
  AND H.IsPrintSMSAndPaper = 'Y'
```

### 使用 TransactionScope

```csharp
using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
{
    // 新增報表下載記錄
    await conn.ExecuteAsync(insertReportBatchDownloadSql, reportBatchReportDownloads);

    // 更新補件狀態
    await conn.ExecuteAsync(updateChangeStatusSql, updateChangeStatusDtos);

    scope.Complete();
}
```

### Word 模板引擎

使用 `TemplateEngine.Docx` 套件：
- 讀取模板檔案
- 填充資料到 Content Controls
- 支援重複內容 (`RepeatContent`)
- 支援圖片內容 (`ImageContent`)
- 移除 Content Controls 標記

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| SysParamManage_BatchSet | 查詢 | 取得批次設定參數 |
| SetUp_SupplementReason | 查詢 | 補件原因對照表 |
| SetUp_TemplateFixContent | 查詢 | 模板固定值設定 |
| Reviewer_ApplyCreditCardInfoMain | 查詢 | 申請人主檔資料 |
| Reviewer_ApplyCreditCardInfoHandle | 查詢/更新 | 案件處理資料 |
| Report_BatchReportDownload | 新增 | 報表下載記錄 |
| System_ErrorLog | 新增 | 錯誤日誌 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 僅在工作日執行
- 建議執行頻率：每日執行一次

## 注意事項

1. 需確保 Word 模板檔案存在於 `Files/補件函含簽名Template.docx`
2. 報表儲存路徑需要有寫入權限
3. 使用 TransactionScope 確保報表產生和資料庫更新的一致性
4. 補件原因代碼需在 `SetUp_SupplementReason` 表中維護
5. 模板固定值需在 `SetUp_TemplateFixContent` 表中設定
6. QR Code 圖片會內嵌到 Word 文件中
7. 使用民國曆產生日期資訊
8. 地址會根據 `SupplementSendCardAddr` 欄位選擇對應地址
9. 簡訊通知檔案格式為 `姓名|手機號碼`，每行一筆
10. 報表檔案名稱包含產生時間，避免檔名衝突
11. 使用 Dapper 進行批次更新提升效能
12. Word 模板使用 Content Controls 標記資料位置
