# EcardNotA02CheckNewCaseJob - 網路件非卡友檢核新案件排程

## Job 基本資訊

- **Job 名稱**: EcardNotA02CheckNewCaseJob
- **顯示名稱**: 網路件非卡友檢核新案件排程 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 網路件非卡友檢核新案件排程
- **工作日檢查**: 是

## 功能說明

網路件非卡友檢核新案件排程負責自動檢核新進的網路申請案件（非原卡友），包括呼叫 MW3 姓名檢核 API、驗證申請人身份資料，並根據檢核結果更新案件狀態。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.EcardNotA02CheckNewCase_IsEnabled` 是否為 "Y"
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
   - 查詢狀態為「網路件_非卡友_待檢核」的案件
   - 數量由 `EcardNotA02CheckNewCase_BatchSize` 參數控制

3. **執行姓名檢核**
   - 呼叫 MW3 姓名檢核 API
   - 驗證申請人姓名與身份證字號

4. **處理檢核結果**
   - **檢核成功**: 更新狀態為「網路件_非卡友_待 KYC 入檔」，記錄檢核日誌
   - **檢核失敗**: 更新狀態為「網路件_非卡友_姓名檢核失敗」
   - 記錄檢核歷程到資料庫

### 檢核邏輯

**姓名檢核流程**:
1. 呼叫 MW3 姓名檢核 API (`IMW3APAPIAdapter.CheckName`)
2. 驗證回傳的姓名檢核結果
3. 記錄檢核結果到 `Reviewer3rd_NameCheckLog`

**狀態轉換規則**:
- 檢核通過 → `CardStatus.網路件_非卡友_待KYC入檔`
- 檢核失敗 → `CardStatus.網路件_非卡友_姓名檢核失敗`

### 錯誤處理

- MW3 API 呼叫失敗會記錄到 `System_ErrorLog`
- 錯誤類型包含：
  - MW3 API 連線錯誤
  - 姓名檢核失敗
  - 內部程式錯誤
- 發生錯誤時會記錄 `SendStatus.等待`，等待後續郵件通知

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 查詢 `SysParamManage_SysParam` 取得排程參數
- 使用 Dapper 查詢待檢核的案件清單

**更新操作**:
- 批次更新 `Reviewer_ApplyCreditCardInfoHandle.CardStatus`
- 更新 `Reviewer_ApplyCreditCardInfoMain.LastUpdateTime`

**新增操作**:
- 新增 `Reviewer3rd_NameCheckLog` 姓名檢核日誌
- 新增 `Reviewer_ApplyCreditCardInfoProcess` 處理歷程
- 新增 `System_ErrorLog` 錯誤日誌

### 外部 API 呼叫

- **MW3 姓名檢核 API**: 驗證申請人姓名與身份證字號

### 使用 Dapper 進行資料查詢

```sql
SELECT TOP (@Limit)
    Handle.SeqNo,
    Handle.ApplyNo,
    Handle.ID,
    Handle.UserType,
    Handle.CardStatus,
    Main.CHName,
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
| Reviewer3rd_NameCheckLog | 新增 | 姓名檢核日誌 |
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
6. 姓名檢核結果會詳細記錄在 `Reviewer3rd_NameCheckLog` 中
7. 檢核失敗的案件會記錄錯誤日誌並等待郵件通知
8. 批次處理數量可透過系統參數動態調整
