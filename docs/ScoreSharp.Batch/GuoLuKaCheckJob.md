# GuoLuKaCheckJob - 國旅卡人士資料檢核排程

## Job 基本資訊

- **Job 名稱**: GuoLuKaCheckJob
- **顯示名稱**: 國旅卡人士資料檢核排程 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 國旅卡人士資料檢核排程
- **工作日檢查**: 是

## 功能說明

國旅卡人士資料檢核排程負責自動檢核國旅卡申請案件的客戶資料，透過呼叫 MW3 API 查詢國旅卡客戶資訊，並根據檢核結果更新案件狀態。若檢核失敗且超過系統設定的撤件天數，則自動將案件更新為系統撤件狀態，並發送郵件通知相關人員。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.GuoLuKaCheck_IsEnabled` 是否為 "Y"
- 若為 "N"，則記錄日誌並結束排程

### 並發控制
- 使用 Semaphore 確保同一時間只有一個批次任務在執行
- 若上一個批次還在執行，則取消本次執行

## 商業邏輯

### 主要流程

1. **檢查系統參數設定**
   - 驗證排程是否啟用
   - 取得系統撤件天數 (`GuoLuKaCaseWithdrawDays`)
   - 取得檢核案件數 (`GuoLuKaCheck_BatchSize`)

2. **取得待檢核案件**
   - 查詢狀態為「國旅人事名冊確認」的案件
   - 依申請日期排序

3. **執行國旅卡客戶資訊檢核**
   - 呼叫 MW3 API 查詢國旅卡客戶資訊
   - 驗證回傳代碼是否為「正確資料」

4. **處理檢核結果**
   - **檢核成功**: 更新狀態為「網路件_非卡友_待檢核」
   - **檢核失敗**:
     - 記錄失敗日誌到 `ReviewerLog_GuoLuKaCheckFail`
     - 若超過撤件天數，更新狀態為「國旅人士名冊確認_系統撤件」

5. **發送失敗通知郵件**
   - 查詢待發送的國旅卡檢核失敗日誌
   - 使用 Razor 模板產生郵件內容
   - 批次發送郵件通知

### 檢核邏輯

**MW3 API 檢核**:
- API 名稱: `QueryTravelCardCustomer`
- 檢核成功代碼: `MW3RtnCodeConst.查詢國旅卡客戶資訊_正確資料`

**狀態轉換規則**:

| 檢核結果 | 是否超過撤件天數 | 案件狀態變更 |
|---------|---------------|------------|
| 成功 | - | 網路件_非卡友_待檢核 |
| 失敗 | 否 | 維持「國旅人事名冊確認」 |
| 失敗 | 是 | 國旅人士名冊確認_系統撤件 |

### 撤件邏輯

- 撤件天數由 `GuoLuKaCaseWithdrawDays` 參數設定
- 計算方式: `ApplyDate + 撤件天數 < 當前時間`
- 超過撤件天數的案件會自動撤件並記錄歷程

### 錯誤處理

- 使用 Transaction 確保資料一致性
- 發生錯誤時會 Rollback 交易
- 錯誤類型包含：
  - MW3 API 呼叫失敗
  - 儲存資料庫錯誤
  - 內部程式錯誤
  - 郵件發送失敗
- 所有錯誤會記錄到 `System_ErrorLog`

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 使用 Dapper 查詢待檢核的國旅卡案件
- 查詢 `SysParamManage_MailSet` 取得郵件設定
- 查詢 `ReviewerLog_GuoLuKaCheckFail` 取得待發送的失敗日誌

**更新操作**:
- 批次更新 `Reviewer_ApplyCreditCardInfoHandle.CardStatus`
- 批次更新 `Reviewer_ApplyCreditCardInfoMain.LastUpdateUserId` 和 `LastUpdateTime`
- 批次更新 `ReviewerLog_GuoLuKaCheckFail.SendStatus`

**新增操作**:
- 新增 `Reviewer_ApplyCreditCardInfoProcess` 處理歷程
- 新增 `ReviewerLog_GuoLuKaCheckFail` 檢核失敗日誌
- 新增 `System_ErrorLog` 錯誤日誌

### 外部 API 呼叫

- **MW3 API** (`QueryTravelCardCustomer`): 查詢國旅卡客戶資訊

### 使用 Dapper 查詢待檢核案件

```sql
SELECT TOP (@Limit)
    Handle.ApplyNo,
    Handle.ID,
    Handle.CardStatus,
    Main.ApplyDate
FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] Handle
JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] Main
    ON Handle.ApplyNo = Main.ApplyNo
    AND Main.ID = Handle.ID
    AND Main.UserType = Handle.UserType
WHERE Handle.CardStatus = @CardStatus
ORDER BY Main.ApplyDate
```

### 郵件通知

**郵件模板欄位** (ReviewerLogGuoLuKaCheckFailViewModel):
- SeqNo: 序號
- ApplyNo: 申請書編號
- ID: 身份證字號
- Response: MW3 回應內容
- CreateTime: 建立時間

**郵件設定**:
- 收件人: `GuoLuKaCheckFailLog_To`
- 主旨: `GuoLuKaCheckFailLog_Title`
- 模板: `GuoLuKaCheckFailLog_Template`

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| SysParamManage_BatchSet | 查詢 | 取得批次設定參數 |
| SysParamManage_MailSet | 查詢 | 取得郵件設定 |
| Reviewer_ApplyCreditCardInfoMain | 查詢/更新 | 申請人主檔資料 |
| Reviewer_ApplyCreditCardInfoHandle | 查詢/更新 | 案件處理資料 |
| Reviewer_ApplyCreditCardInfoProcess | 新增 | 處理歷程記錄 |
| ReviewerLog_GuoLuKaCheckFail | 新增/查詢/更新 | 國旅卡檢核失敗日誌 |
| System_ErrorLog | 新增 | 系統錯誤日誌 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 僅在工作日執行
- 建議執行頻率：每日執行

## 注意事項

1. 使用 Semaphore 確保同一時間只有一個實例運行
2. 使用 Transaction 確保檢核和狀態更新的一致性
3. 檢核失敗會記錄詳細的 MW3 回應內容
4. 超過撤件天數的案件會自動系統撤件
5. 檢核失敗日誌會透過郵件通知相關人員
6. FluentEmail 發送郵件前需清空 `ToAddresses`
7. 批次處理數量可透過系統參數動態調整
8. 撤件天數可透過系統參數動態調整
9. 使用 Dapper 進行資料查詢以提升效能
10. 使用 `ExecuteUpdateAsync` 批次更新提升效能
