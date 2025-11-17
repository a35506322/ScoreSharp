# CompareMissingCasesJob - 每2小時比對漏進案件批次

## Job 基本資訊

- **Job 名稱**: CompareMissingCasesJob
- **顯示名稱**: 每2小時比對漏進案件批次 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 每2小時比對漏進案件批次

## 功能說明

每2小時比對漏進案件批次排程負責定期比對 Ecard 系統與徵審系統之間的案件差異，找出應該進入徵審系統但尚未進入的案件（漏進案件），並透過呼叫 MW3 API 取得案件資料，再發送到 Middleware API 進行補進作業。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.CompareMissingCases_IsEnabled` 是否為 "Y"
- 若為 "N"，則記錄日誌並結束排程

### 處理日期驗證
- 若未提供 `processDate` 參數，預設使用當前日期 (yyyyMMdd)

## 商業邏輯

### 主要流程

1. **檢查系統參數設定**
   - 驗證排程是否啟用
   - 取得處理日期

2. **從 FTP 下載進件比對檔**
   - 連接到 Ecard FTP 伺服器
   - 下載當日的進件總檔 (`APPLYCard_Total_{processDate}.txt`)
   - 儲存到本地資料夾

3. **比對本地檔案找出新進件**
   - 讀取最新兩次的比對檔
   - 找出新增的申請書編號（差異資料）
   - 首次執行時直接使用全部資料

4. **比對徵審系統已進件資料**
   - 查詢當日徵審系統已進件的申請書編號
   - 與 Ecard 進件資料比對，找出漏進案件

5. **取得漏進案件詳細資料**
   - 呼叫 MW3 eCardService API 取得申請資料
   - 驗證 API 回應代碼

6. **發送 Ecard 新件 API**
   - 將取得的申請資料發送到 Middleware
   - 記錄發送結果

### FTP 連線設定

從 `EcardFtpOption` 取得：
- Host: FTP 伺服器位址
- Port: FTP 連接埠
- Username: FTP 帳號
- Mima: FTP 密碼
- UseSsl: 是否使用 SSL
- ConnectTimeout: 連線逾時時間
- DataConnectionTimeout: 資料連線逾時時間

### FTP 檔案路徑

- **遠端路徑**: `CompareMissingCasesRemoteFolderPath/APPLYCard_Total_{processDate}.txt`
- **本地路徑**: `CompareMissingCasesLocalFolderPath/{processDate}/APPLYCard_Total_{processDate}_{fileCount}.txt`

### 比對檔案格式

- 格式: `ID,申請書編號`
- 使用整列資料（包含 ID 和申請書編號）作為比對基準
- 找出差異後取出申請書編號

### MW3 API 呼叫

**查詢 eCardService 申請資料**:
- API: `QueryEcardNewCase`
- 成功代碼: `MW3RtnCodeConst.查詢eCardService_作業成功`
- 回傳: `EcardNewCaseRequest` 物件

**發送 Ecard 新件**:
- API: `PostEcardNewCaseAsync`
- 成功代碼: `ID = "0000"`

### 錯誤處理

- FTP 檔案不存在會記錄錯誤並結束
- FTP 連線失敗會拋出例外
- MW3 API 呼叫失敗會記錄詳細錯誤訊息
- Middleware API 發送失敗會記錄錯誤訊息

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 查詢 `Reviewer_ApplyCreditCardInfoMain` 取得當日已進件的申請書編號

### 外部 API 呼叫

- **MW3 eCardService API** (`QueryEcardNewCase`): 查詢申請資料
- **Middleware API** (`PostEcardNewCaseAsync`): 發送 Ecard 新件

### FTP 操作

- 使用 FluentFTP 套件進行 FTP 操作
- 支援 SSL/TLS 加密連線
- 自動重試機制（最多 3 次）
- 完整的連線狀態管理

### 檔案處理邏輯

**首次執行**:
1. 本地資料夾只有 1 個檔案
2. 直接讀取所有申請書編號
3. 與系統資料比對

**後續執行**:
1. 讀取最新 2 個檔案
2. 比對找出新增的資料
3. 提取申請書編號差異

### 查詢系統已進件 SQL

```csharp
var mainCaseApplyNos = await _context
    .Reviewer_ApplyCreditCardInfoMain.AsNoTracking()
    .Where(x => dateTime <= x.ApplyDate && x.ApplyDate < nextDateTime)
    .Select(x => x.ApplyNo)
    .ToListAsync();
```

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| SysParamManage_BatchSet | 查詢 | 取得批次設定參數 |
| Reviewer_ApplyCreditCardInfoMain | 查詢 | 查詢當日已進件的申請書編號 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 建議執行頻率：每 2 小時執行一次
- 可指定特定日期進行比對（手動觸發）

## 注意事項

1. FTP 檔案下載失敗會結束排程，不會繼續後續處理
2. 本地檔案會累積儲存，建議定期清理舊檔案
3. 使用 HashSet 進行差異比對，提升效能
4. FTP 連線支援 SSL/TLS 加密
5. FTP 操作有完整的重試機制（最多 3 次）
6. MW3 API 呼叫失敗只會記錄錯誤，不會中斷其他案件處理
7. Middleware API 發送失敗也只會記錄錯誤
8. 首次執行會處理當日所有進件資料
9. 後續執行只處理新增的進件資料
10. 比對基準使用「ID,申請書編號」組合，確保唯一性
