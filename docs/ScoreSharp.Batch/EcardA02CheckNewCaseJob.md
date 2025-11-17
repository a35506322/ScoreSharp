# EcardA02CheckNewCaseJob - 網路件卡友檢核新案件排程

## Job 基本資訊

- **Job 名稱**: EcardA02CheckNewCaseJob
- **顯示名稱**: 網路件卡友檢核新案件排程 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 網路件卡友檢核新案件排程
- **工作日檢查**: 是

## 功能說明

網路件卡友檢核新案件排程負責自動檢核新進的網路申請案件（原卡友），包括呼叫 MW3 A02 API 驗證卡友身份、檢核申請資料，並根據檢核結果更新案件狀態。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.EcardA02CheckNewCase_IsEnabled` 是否為 "Y"
- 若為 "N"，則記錄日誌並結束排程

### 並發控制
- 使用 Semaphore 確保同一時間只有一個批次任務在執行
- 若上一個批次還在執行，則取消本次執行

### 排程時間檢查
- 檢查是否在 KYC 系統維護時間內 (`KYCFixStartTime` ~ `KYCFixEndTime`)
- 若在維護時間內，則結束排程

## 商業邏輯

### 主要流程

1. **檢查系統參數設定**
   - 驗證排程是否啟用
   - 檢查 KYC 系統維護時間

2. **取得待檢核案件**
   - 查詢狀態為「網路件_卡友_待檢核」的案件
   - 數量由 `EcardA02CheckNewCase_BatchSize` 參數控制

3. **執行卡友身份檢核**
   - 呼叫 MW3 A02 API 驗證卡友身份
   - 檢查卡友資料是否正確

4. **處理檢核結果**
   - **檢核成功**: 更新狀態為「網路件_卡友_待 KYC 入檔」
   - **檢核失敗**: 更新狀態為「網路件_卡友_MW3 A02 檢核失敗」
   - 記錄檢核歷程到資料庫

### 檢核邏輯

**A02 卡友驗證**:
- 驗證身份證字號與卡友資料是否一致
- 檢查卡友是否為有效狀態
- 確認卡友資料完整性

**狀態轉換規則**:
- 檢核通過 → `CardStatus.網路件_卡友_待KYC入檔`
- 檢核失敗 → `CardStatus.網路件_卡友_MW3_A02檢核失敗`

### 錯誤處理

- MW3 API 呼叫失敗會記錄到 `System_ErrorLog`
- 錯誤類型包含：
  - MW3 API 連線錯誤
  - A02 卡友資料驗證失敗
  - 內部程式錯誤

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 查詢 `SysParamManage_SysParam` 取得排程參數
- 使用 SQL 查詢待檢核的案件清單

**更新操作**:
- 批次更新 `Reviewer_ApplyCreditCardInfoHandle.CardStatus`
- 更新 `Reviewer_ApplyCreditCardInfoMain.LastUpdateTime`

**新增操作**:
- 新增 `Reviewer_ApplyCreditCardInfoProcess` 處理歷程
- 新增 `System_ErrorLog` 錯誤日誌

### 外部 API 呼叫

- **MW3 A02 API**: 驗證原卡友身份資料

### 使用 Dapper 進行資料查詢

排程使用 Dapper 執行原生 SQL 查詢，提升大量資料處理效能：

```sql
SELECT TOP (@Limit)
    Handle.SeqNo,
    Handle.ApplyNo,
    Handle.ID,
    Handle.UserType,
    Handle.CardStatus,
    Main.IDType
FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] Handle
JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] Main
    ON Handle.ApplyNo = Main.ApplyNo
WHERE Handle.CardStatus = @CardStatus
ORDER BY Main.ApplyDate
```

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| SysParamManage_BatchSet | 查詢 | 取得批次設定參數 |
| SysParamManage_SysParam | 查詢 | 取得系統參數 |
| Reviewer_ApplyCreditCardInfoMain | 查詢/更新 | 申請人主檔資料 |
| Reviewer_ApplyCreditCardInfoHandle | 查詢/更新 | 案件處理資料 |
| Reviewer_ApplyCreditCardInfoProcess | 新增 | 處理歷程記錄 |
| System_ErrorLog | 新增 | 錯誤日誌 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 僅在工作日執行
- 避開 KYC 系統維護時間
- 建議執行頻率：每 5-10 分鐘

## 注意事項

1. 使用 Semaphore 確保同一時間只有一個實例運行
2. 需檢查 KYC 系統維護時間，避免在維護期間執行
3. 使用 Dapper 進行資料查詢以提升效能
4. 批次更新使用 EF Core 的 `ExecuteUpdateAsync` 提升效能
5. 所有 MW3 API 呼叫都有完整的錯誤處理機制
6. 檢核結果會詳細記錄在處理歷程中
7. 批次處理數量可透過系統參數動態調整
