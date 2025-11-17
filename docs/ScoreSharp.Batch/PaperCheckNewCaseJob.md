# PaperCheckNewCaseJob - 紙本件檢核新案件排程

## Job 基本資訊

- **Job 名稱**: PaperCheckNewCaseJob
- **顯示名稱**: 紙本件檢核新案件排程 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 紙本件檢核新案件排程
- **工作日檢查**: 是

## 功能說明

紙本件檢核新案件排程負責自動檢核新進的紙本申請案件，包括驗證申請資料的完整性、呼叫 MW3 API 進行資料檢核，並根據檢核結果更新案件狀態。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.PaperCheckNewCase_IsEnabled` 是否為 "Y"
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
   - 查詢特定狀態的紙本案件
   - 數量由 `PaperCheckNewCase_BatchSize` 參數控制

3. **執行案件檢核**
   - 驗證申請人資料完整性
   - 檢查是否為原卡友
   - 根據不同條件呼叫對應的 MW3 API

4. **處理檢核結果**
   - 根據 API 回應更新案件狀態
   - 記錄檢核歷程
   - 處理異常情況

### 檢核邏輯

**原卡友檢核**:
- 呼叫 MW3 API 驗證卡友資料
- 檢核通過後更新狀態為「待 KYC 入檔」

**非卡友檢核**:
- 呼叫姓名檢核 API
- 檢核通過後更新狀態為「待 KYC 入檔」

### 錯誤處理

- 檢核失敗會記錄到 `System_ErrorLog`
- 錯誤類型包含：
  - MW3 API 呼叫失敗
  - 資料驗證錯誤
  - 內部程式錯誤

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 查詢 `SysParamManage_SysParam` 取得排程參數
- 查詢待檢核的 `Reviewer_ApplyCreditCardInfoHandle`
- 查詢 `Reviewer_ApplyCreditCardInfoMain` 取得申請人資料

**更新操作**:
- 更新 `Reviewer_ApplyCreditCardInfoHandle.CardStatus`
- 更新 `Reviewer_ApplyCreditCardInfoMain` 相關欄位

**新增操作**:
- 新增 `Reviewer_ApplyCreditCardInfoProcess` 處理歷程
- 新增 `Reviewer3rd_NameCheckLog` 姓名檢核日誌
- 新增 `System_ErrorLog` 錯誤日誌

### 外部 API 呼叫

- **MW3 API**: 呼叫卡友資料驗證 API
- **姓名檢核 API**: 驗證非卡友姓名資料

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| SysParamManage_BatchSet | 查詢 | 取得批次設定參數 |
| SysParamManage_SysParam | 查詢 | 取得系統參數 |
| Reviewer_ApplyCreditCardInfoMain | 查詢/更新 | 申請人主檔資料 |
| Reviewer_ApplyCreditCardInfoHandle | 查詢/更新 | 案件處理資料 |
| Reviewer_ApplyCreditCardInfoProcess | 新增 | 處理歷程記錄 |
| Reviewer3rd_NameCheckLog | 新增 | 姓名檢核日誌 |
| System_ErrorLog | 新增 | 錯誤日誌 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 僅在工作日執行
- 避開 KYC 系統維護時間
- 執行頻率由系統管理員設定

## 注意事項

1. 使用 Semaphore 確保同一時間只有一個實例運行
2. 需檢查 KYC 系統維護時間，避免在維護期間執行
3. 所有 MW3 API 呼叫都有完整的錯誤處理機制
4. 檢核結果會詳細記錄在日誌和處理歷程中
5. 批次處理數量可透過系統參數動態調整
