# SystemAssignmentJob - 系統派案

## Job 基本資訊

- **Job 名稱**: SystemAssignmentJob
- **顯示名稱**: 系統派案 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 系統派案
- **工作日檢查**: 是

## 功能說明

系統派案排程負責自動將信用卡申請案件派送給適當的處理人員。此排程會根據系統參數設定的派案數量，自動分配待處理的案件給相關人員。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.SystemAssignment_IsEnabled` 是否為 "Y"
- 若為 "N"，則記錄日誌並結束排程

### 並發控制
- 使用 Semaphore 確保同一時間只有一個批次任務在執行
- 若上一個批次還在執行，則取消本次執行

## 商業邏輯

### 主要流程

1. **檢查系統參數設定**
   - 驗證排程是否啟用

2. **取得待派案案件**
   - 從資料庫查詢需要派案的案件
   - 數量由 `SystemAssignment_BatchSize` 參數控制

3. **執行派案作業**
   - 根據案件狀態和條件自動分配處理人員
   - 更新案件的 `CurrentHandleUserId`

4. **記錄派案歷程**
   - 新增派案處理歷程到 `Reviewer_ApplyCreditCardInfoProcess`
   - 記錄派案時間和處理人員

### 錯誤處理

- 所有錯誤會記錄到系統日誌
- 發生例外時會寫入 `System_ErrorLog` 表
- 錯誤類型包含：
  - 內部程式錯誤
  - 資料庫操作錯誤

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 查詢待派案的 `Reviewer_ApplyCreditCardInfoHandle`

**更新操作**:
- 更新 `Reviewer_ApplyCreditCardInfoMain.CurrentHandleUserId`
- 更新 `Reviewer_ApplyCreditCardInfoHandle.CardStatus`

**新增操作**:
- 新增 `Reviewer_ApplyCreditCardInfoProcess` 派案歷程記錄
- 異常時新增 `System_ErrorLog` 錯誤日誌

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| SysParamManage_BatchSet | 查詢 | 取得系統派案參數設定 |
| Reviewer_ApplyCreditCardInfoMain | 更新 | 更新當前處理人員 |
| Reviewer_ApplyCreditCardInfoHandle | 查詢/更新 | 查詢待派案案件並更新狀態 |
| Reviewer_ApplyCreditCardInfoProcess | 新增 | 記錄派案處理歷程 |
| System_ErrorLog | 新增 | 記錄錯誤日誌 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 僅在工作日執行
- 執行頻率由系統管理員設定

## 注意事項

1. 使用 Semaphore 確保同一時間只有一個實例運行
2. 所有資料庫操作都有完整的錯誤處理機制
3. 執行結果會詳細記錄在日誌中
4. 派案數量可透過系統參數動態調整
