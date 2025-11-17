# A02KYCSyncJob - A02 KYC 同步排程

## Job 基本資訊

- **Job 名稱**: A02KYCSyncJob
- **顯示名稱**: A02 KYC 同步排程 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: A02 KYC 同步排程

## 功能說明

A02 KYC 同步排程負責將原卡友的信用卡申請資料同步到 KYC 系統進行風險評估。此排程會呼叫 MW3 KYC API，取得客戶的 AML 風險等級 (RaRank) 和 KYC 代碼，並根據結果更新案件狀態和風險評估資訊。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.A02KYCSyncJob_IsEnabled` 是否為 "Y"
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

2. **取得需入檔案件**
   - 查詢狀態為「30116」的案件（網路件_卡友_待 KYC 入檔）
   - 排除近 1 小時內已成功查詢過的案件
   - 數量由 `A02KYCSyncJob_BatchSize` 參數控制

3. **執行 KYC 入檔作業**
   - 取得主卡人資料
   - 組合 KYC 同步請求資料
   - 呼叫 MW3 KYC API (`IMW3APAPIAdapter.SyncKYC`)

4. **處理入檔結果**
   - 更新風險等級 (AMLRiskLevel)
   - 更新金融檢核資訊 (FinanceCheckInfo)
   - 根據 KYC 代碼更新案件狀態
   - 記錄 KYC 查詢日誌和處理歷程

### KYC 狀態轉換規則

| KYC 代碼 | 結果 | 案件狀態變更 |
|---------|------|------------|
| KC001 (入檔成功) | 成功 | 30117 (網路件_卡友_完成 KYC 入檔作業) |
| KC008 (主機 TimeOut) | 失敗 | 維持原狀態 |
| 其他錯誤代碼 | 失敗 | 30012 (網路件_待月收入預審) |

### 風險等級處理

- 更新 `Reviewer_ApplyCreditCardInfoMain.AMLRiskLevel`
- 更新 `Reviewer_FinanceCheckInfo.AMLRiskLevel`
- 原卡友不需執行 KYC 加強審核，設定 `KYC_StrongReStatus = 不需檢核`

### 錯誤處理

- KYC API 呼叫失敗會記錄到 `System_ErrorLog` 和 `Reviewer3rd_KYCQueryLog`
- 錯誤類型包含：
  - MW3 API 連線錯誤
  - KYC 入檔失敗
  - 內部程式錯誤
- 使用 Transaction 確保資料一致性

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 查詢 `SysParamManage_SysParam` 取得排程參數
- 使用 SQL 查詢需入檔的案件（排除近期已成功的案件）
- 查詢 `Reviewer_ApplyCreditCardInfoMain` 取得主卡人資料

**更新操作**:
- 更新 `Reviewer_ApplyCreditCardInfoMain.AMLRiskLevel`
- 批次更新 `Reviewer_FinanceCheckInfo` KYC 相關欄位
- 批次更新 `Reviewer_ApplyCreditCardInfoHandle.CardStatus`

**新增操作**:
- 新增 `Reviewer3rd_KYCQueryLog` KYC 查詢日誌
- 新增 `Reviewer_ApplyCreditCardInfoProcess` 處理歷程
- 新增 `System_ErrorLog` 錯誤日誌

### 外部 API 呼叫

- **MW3 KYC API** (`SyncKYC`): 執行 KYC 入檔作業，API 名稱為 `KYC00CREDIT`

### 使用 SQL 查詢待處理案件

```sql
SELECT Top({0}) A.[SeqNo]
    ,A.[ApplyNo]
    ,A.[ID]
    ,A.[UserType]
    ,A.[CardStatus]
FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] A
JOIN [ScoreSharp].[dbo].[Reviewer_FinanceCheckInfo] F
    ON A.ApplyNo = F.ApplyNo AND A.ID = F.ID AND F.UserType = 1
WHERE A.CardStatus = 30116
AND NOT (F.KYC_RtnCode = 'KC001'
    AND F.KYC_QueryTime IS NOT NULL
    AND DATEDIFF(HOUR, F.KYC_QueryTime, GETDATE()) < {1})
```

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| SysParamManage_BatchSet | 查詢 | 取得批次設定參數 |
| SysParamManage_SysParam | 查詢 | 取得系統參數 |
| Reviewer_ApplyCreditCardInfoMain | 查詢/更新 | 申請人主檔資料，更新風險等級 |
| Reviewer_ApplyCreditCardInfoHandle | 查詢/更新 | 案件處理資料，更新案件狀態 |
| Reviewer_FinanceCheckInfo | 更新 | 更新 KYC 相關檢核資訊 |
| Reviewer3rd_KYCQueryLog | 新增 | KYC 查詢日誌（含錯誤訊息） |
| Reviewer_ApplyCreditCardInfoProcess | 新增 | 處理歷程記錄 |
| System_ErrorLog | 新增 | 錯誤日誌 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 避開 KYC 系統維護時間
- 建議執行頻率：每 5-10 分鐘
- 已成功入檔的案件會在 1 小時內避免重複查詢

## 注意事項

1. 使用 Semaphore 確保同一時間只有一個實例運行
2. 需檢查 KYC 系統維護時間，避免在維護期間執行
3. KYC 查詢結果（含錯誤）都會記錄到 `Reviewer3rd_KYCQueryLog`
4. 錯誤的 KYC 查詢日誌會等待郵件通知 (`SendStatus.等待`)
5. 原卡友不需執行 KYC 加強審核
6. 使用 `ChangeTracker.Clear()` 確保每個案件處理後清理追蹤
7. 批次處理數量可透過系統參數動態調整
8. 所有 KYC 相關欄位更新使用 Attach + IsModified 提升效能
