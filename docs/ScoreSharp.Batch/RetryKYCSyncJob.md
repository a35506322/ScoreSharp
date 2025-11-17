# RetryKYCSyncJob - 重試 KYC 入檔作業排程

## Job 基本資訊

- **Job 名稱**: RetryKYCSyncJob
- **顯示名稱**: 重試 KYC 入檔作業排程 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 重試 KYC 入檔作業排程

## 功能說明

重試 KYC 入檔作業排程負責重新處理先前 KYC 入檔失敗的案件，包括原卡友和非卡友。此排程會根據案件類型呼叫不同的 MW3 KYC API，並根據檢核結果更新案件狀態、風險等級和 KYC 加強審核資訊。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.RetryKYCSync_IsEnabled` 是否為 "Y"
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

2. **取得需重試案件**
   - 查詢狀態為「30117, 30118」的案件（KYC 入檔失敗狀態）
   - 排除近 1 小時內已成功查詢過的案件
   - 數量由 `RetryKYCSync_BatchSize` 參數控制

3. **判斷案件類型並執行 KYC 入檔**
   - **原卡友**: 直接呼叫 KYC API
   - **非卡友**: 先取得姓名檢核日誌，再呼叫 KYC API
   - **未做姓名檢核**: 更新狀態為「待月收入確認」

4. **處理入檔結果**
   - 更新風險等級和 KYC 相關資訊
   - 根據 KYC 代碼和風險等級判斷是否需要加強審核
   - 記錄處理歷程和查詢日誌

### 案件類型判斷邏輯

| 條件 | 案件類型 | 處理方式 | API 名稱 |
|-----|---------|---------|---------|
| `Source != 紙本` && `IsOriginalCardholder == "Y"` | 原卡友網路件 | 直接 KYC 入檔 | KYC00CREDIT |
| `IsOriginalCardholder == "N"` | 非卡友 | 需先取得姓名檢核日誌 | KYC00CREDIT |
| 非卡友但無姓名檢核日誌 | 資料異常 | 更新狀態為待月收入確認 | - |

### KYC 狀態轉換規則

**紙本件待月收入預審 (30117)**:

| KYC 代碼 | 結果 | 案件狀態變更 |
|---------|------|------------|
| KC001 (入檔成功) | 成功 | 30018 (完成月收入確認) |
| KC008 (主機 TimeOut) | 失敗 | 維持原狀態 (30117) |
| 其他錯誤代碼 | 失敗 | 10012 (紙本件_待月收入預審) |

**網路件待月收入預審 (30118)**:

| KYC 代碼 | 結果 | 案件狀態變更 |
|---------|------|------------|
| KC001 (入檔成功) | 成功 | 30018 (完成月收入確認) |
| KC008 (主機 TimeOut) | 失敗 | 維持原狀態 (30118) |
| 其他錯誤代碼 | 失敗 | 30012 (網路件_待月收入預審) |

### KYC 加強審核判斷

針對非卡友且 KYC 入檔成功的案件：

- **風險等級為 H (高風險) 或 M (中風險)**:
  - 設定 `KYC_StrongReStatus = 未送審`
  - 產生 KYC 加強審核執行表 JSON 資料

- **風險等級為 L (低風險)**:
  - 設定 `KYC_StrongReStatus = 不需檢核`

- **原卡友**:
  - 一律設定 `KYC_StrongReStatus = 不需檢核`

### 錯誤處理

- KYC API 呼叫失敗會記錄到 `System_ErrorLog`
- 錯誤類型包含：
  - MW3 API 連線錯誤
  - KYC 入檔失敗
  - 姓名檢核日誌不存在
  - 未知案件來源
  - 內部程式錯誤

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 查詢 `SysParamManage_SysParam` 取得排程參數
- 使用 SQL 查詢需重試的案件
- 查詢 `Reviewer_ApplyCreditCardInfoMain` 取得主卡人資料
- 查詢 `Reviewer3rd_NameCheckLog` 取得姓名檢核日誌

**更新操作**:
- 更新 `Reviewer_ApplyCreditCardInfoMain.AMLRiskLevel`
- 批次更新 `Reviewer_FinanceCheckInfo` KYC 相關欄位
- 批次更新 `Reviewer_ApplyCreditCardInfoHandle.CardStatus`

**新增操作**:
- 新增 `Reviewer3rd_KYCQueryLog` KYC 查詢日誌
- 新增 `Reviewer_ApplyCreditCardInfoProcess` 處理歷程
- 新增 `System_ErrorLog` 錯誤日誌

### 外部 API 呼叫

- **MW3 KYC API** (`SyncKYC`): 執行 KYC 入檔作業

### 使用 SQL 查詢待處理案件

```sql
SELECT A.[SeqNo]
    ,A.[ApplyNo]
    ,A.[ID]
    ,A.[UserType]
    ,A.[CardStatus]
FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] A
JOIN [ScoreSharp].[dbo].[Reviewer_FinanceCheckInfo] F
    ON A.ApplyNo = F.ApplyNo AND F.UserType = 1
WHERE A.CardStatus in (30117,30118)
AND NOT (F.KYC_RtnCode = 'KC001'
    AND F.KYC_QueryTime IS NOT NULL
    AND DATEDIFF(HOUR, F.KYC_QueryTime, GETDATE()) < 1)
```

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| SysParamManage_BatchSet | 查詢 | 取得批次設定參數 |
| SysParamManage_SysParam | 查詢 | 取得系統參數 |
| Reviewer_ApplyCreditCardInfoMain | 查詢/更新 | 申請人主檔資料 |
| Reviewer_ApplyCreditCardInfoHandle | 查詢/更新 | 案件處理資料 |
| Reviewer_FinanceCheckInfo | 更新 | 更新 KYC 相關檢核資訊 |
| Reviewer3rd_NameCheckLog | 查詢 | 查詢姓名檢核日誌 |
| Reviewer3rd_KYCQueryLog | 新增 | KYC 查詢日誌 |
| Reviewer_ApplyCreditCardInfoProcess | 新增 | 處理歷程記錄 |
| System_ErrorLog | 新增 | 錯誤日誌 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 避開 KYC 系統維護時間
- 建議執行頻率：每 10-15 分鐘
- 已成功入檔的案件會在 1 小時內避免重複查詢

## 注意事項

1. 使用 Semaphore 確保同一時間只有一個實例運行
2. 需檢查 KYC 系統維護時間，避免在維護期間執行
3. 非卡友需先取得姓名檢核日誌才能執行 KYC 入檔
4. 根據風險等級自動產生 KYC 加強審核執行表
5. KYC 查詢結果（含錯誤）都會記錄到 `Reviewer3rd_KYCQueryLog`
6. 使用 `ChangeTracker.Clear()` 確保每個案件處理後清理追蹤
7. 批次處理數量可透過系統參數動態調整
8. 僅在 API 成功時才更新案件狀態和風險等級
