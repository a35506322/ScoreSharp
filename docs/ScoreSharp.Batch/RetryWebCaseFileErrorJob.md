# RetryWebCaseFileErrorJob - 網路件申請書檔案異常重新抓取

## Job 基本資訊

- **Job 名稱**: RetryWebCaseFileErrorJob
- **顯示名稱**: 網路件_申請書檔案異常 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 網路件_申請書檔案異常重新抓取

## 功能說明

網路件申請書檔案異常重新抓取排程負責處理網路申請案件中檔案異常的情況，包括申請書PDF遺失、附件檔案異常、連線錯誤等問題。此排程會從 eCard_file 資料庫重新抓取檔案、壓印浮水印、驗證檔案完整性，並使用分散式交易確保資料一致性。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.RetryWebCaseFileErrorJob_IsEnabled` 是否為 "Y"
- 若為 "N"，則記錄日誌並結束排程

### 並發控制
- 使用 Semaphore 確保同一時間只有一個批次任務在執行
- 若上一個批次還在執行，則取消本次執行

## 商業邏輯

### 主要流程

1. **檢查系統參數設定**
   - 驗證排程是否啟用
   - 取得執行案件數量 (`RetryWebCaseFileErrorJob_BatchSize`)

2. **取得申請書檔案異常案件**
   - 查詢檔案異常狀態的案件
   - 使用 Dapper 執行 SQL 查詢

3. **處理每個異常案件**
   - 從 eCard_file DB 取得申請書檔案
   - 驗證檔案取得結果
   - 處理申請書 PDF 檔案
   - 處理附件檔案並壓印浮水印（非卡友或國旅卡）

4. **執行分散式交易**
   - 刪除舊的檔案資料
   - 新增處理後的檔案
   - 更新案件狀態
   - 記錄處理歷程

### 檔案異常狀態

排程處理以下狀態的案件：
- `網路件_非卡友_申請書異常`
- `網路件_卡友_申請書異常`
- `網路件_卡友_檔案連線異常`
- `網路件_非卡友_檔案連線異常`
- `網路件_卡友_查無申請書_附件`
- `網路件_非卡友_查無申請書_附件`

### 檔案處理邏輯

**檔案取得**:
1. 從 eCard_file DB 查詢 `ApplyFile` 表
2. 取得申請書 PDF (`uploadPDF`)
3. 取得附件檔案 (`idPic1`, `idPic2`, `upload1-6`)

**檔案驗證**:
- 檢查 eCard_file DB 連線是否成功
- 檢查 ApplyFile 是否存在
- 檢查 uploadPDF 是否存在

**附件處理條件**:
- 非卡友 (`IDType != 卡友`)
- 或國旅卡 (`IsCITSCard == "Y"`)

**浮水印壓印**:
- 浮水印文字從設定檔取得 (`WatermarkText`)
- 預設值: "聯邦銀行股份授權"
- 處理 8 個附件檔案 (idPic1, idPic2, upload1-6)
- 使用 `IWatermarkHelper.ImageWatermarkAndGetBytes()` 處理

### 案件狀態轉換

**轉換邏輯**:

| 條件 | 新狀態 |
|-----|-------|
| 國旅卡 (`IsCITSCard == "Y"`) | 國旅人事名冊確認 |
| 原卡友 (`IDType == 卡友`) | 網路件_卡友_待檢核 |
| 存戶/持他行卡/自然人憑證 且無 MyData | 網路件_非卡友_待檢核 |
| 存戶/持他行卡/自然人憑證 且有 MyData | 網路件_等待MyData附件 |
| 小白件 (`IDType == null`) 且無 MyData | 網路件_書面申請等待列印申請書及回郵信封 |
| 小白件 (`IDType == null`) 且有 MyData | 網路件_書面申請等待MyData |
| 其他非卡友 且無 MyData | 網路件_非卡友_待檢核 |
| 其他非卡友 且有 MyData | 網路件_等待MyData附件 |

### 錯誤處理

**錯誤類型**:
- `ECARD_FILE_DB_連線錯誤`: eCard_file DB 連線失敗
- `ECARD_FILE_DB_查無申請書附件檔案`: 查無申請書檔案
- `申請書異常`: uploadPDF 不存在
- `附件異常`: 附件檔案壓印浮水印失敗
- `內部程式錯誤`: 資料長度過長或其他程式錯誤

**附件異常處理**:
- 附件異常時仍會更新案件狀態
- 設定 `ECard_AppendixIsException = "Y"`
- 記錄錯誤到 `System_ErrorLog`

**SQL 錯誤處理**:
- SQL Server 錯誤代碼 2628 (資料截斷) 特別處理
- 發生資料長度過長時會 Rollback 並記錄錯誤

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 使用 Dapper 查詢 eCard_file DB 的 `ApplyFile` 表
- 使用 Dapper 查詢異常案件清單

**刪除操作**:
- 刪除 `ScoreSharp_File.Reviewer_ApplyFile` 舊資料 (FileType = 申請書相關)
- 刪除 `ScoreSharp.Reviewer_ApplyCreditCardInfoFile` 舊資料

**新增操作**:
- 新增 `ScoreSharp_File.Reviewer_ApplyFile` 檔案資料
- 新增 `ScoreSharp.Reviewer_ApplyCreditCardInfoFile` 檔案關聯
- 新增 `Reviewer_ApplyCreditCardInfoProcess` 處理歷程
- 新增 `System_ErrorLog` 錯誤日誌

**更新操作**:
- 更新 `Reviewer_ApplyCreditCardInfoHandle.CardStatus`
- 更新 `Reviewer_ApplyCreditCardInfoMain.ECard_AppendixIsException`

### 分散式交易

使用兩個獨立的 DbContext:
- **ScoreSharpFileContext**: 處理檔案資料 (ScoreSharp_File DB)
- **ScoreSharpContext**: 處理案件資料 (ScoreSharp DB)

**交易流程**:
1. 開始兩個獨立的 Transaction
2. Phase 1 (準備階段):
   - 刪除並新增檔案資料 (ScoreSharp_File)
   - 刪除並新增檔案關聯 (ScoreSharp)
   - 新增處理歷程和錯誤日誌
3. Phase 2 (提交階段):
   - SaveChanges 兩個 Context
   - Commit 兩個 Transaction
4. 發生錯誤時 Rollback 兩個 Transaction

### 使用 Dapper 查詢異常案件

```sql
SELECT TOP (@Limit)
    M.ApplyNo,
    M.ApplyDate,
    H.CardStatus,
    M.CardAppId,
    M.IDType,
    C.IsCITSCard,
    M.MyDataCaseNo
FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] M
JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] H
    ON M.ApplyNo = H.ApplyNo AND M.ID = H.ID
JOIN [ScoreSharp].[dbo].[SetUp_Card] C
    ON H.ApplyCardType = C.CardCode
WHERE H.CardStatus IN @CardStatus
```

### 使用 Dapper 查詢 eCard_file

```sql
SELECT [idPic1], [idPic2],
       [upload1], [upload2], [upload3],
       [upload4], [upload5], [upload6],
       [uploadPDF]
FROM [eCard_file].[dbo].[ApplyFile]
WHERE [cCard_AppId] = @CardAppId
```

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| SysParamManage_BatchSet | 查詢 | 取得批次設定參數 |
| Reviewer_ApplyCreditCardInfoMain | 查詢/更新 | 申請人主檔資料 |
| Reviewer_ApplyCreditCardInfoHandle | 查詢/更新 | 案件處理資料 |
| Reviewer_ApplyFile | 刪除/新增 | 申請書檔案 (ScoreSharp_File DB) |
| Reviewer_ApplyCreditCardInfoFile | 刪除/新增 | 檔案關聯資料 |
| Reviewer_ApplyCreditCardInfoProcess | 新增 | 處理歷程記錄 |
| System_ErrorLog | 新增 | 錯誤日誌 |
| eCard_file.ApplyFile | 查詢 | eCard 檔案資料 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 建議執行頻率：每 10-30 分鐘
- 處理量由 `RetryWebCaseFileErrorJob_BatchSize` 控制

## 注意事項

1. 使用 Semaphore 確保同一時間只有一個實例運行
2. 使用分散式交易確保兩個資料庫的資料一致性
3. 附件異常不會阻止案件狀態更新，會設定 `ECard_AppendixIsException = "Y"`
4. 所有附件都會壓印浮水印（非卡友或國旅卡）
5. 使用 Dapper 進行跨資料庫查詢提升效能
6. 使用 `ExecuteDeleteAsync` 批次刪除提升效能
7. 檔案內容以 byte[] 形式儲存
8. 檔案命名格式: `{ApplyNo}_{fileKey}_{fileId}.{contentType}`
9. 處理歷程 Process 固定為「申請書檔案異常重新抓取」
10. SQL Server 資料長度過長錯誤 (2628) 會特別處理並記錄
11. Transaction Rollback 失敗也會記錄錯誤
