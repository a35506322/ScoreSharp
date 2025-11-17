# SendKYCErrorLogJob - 寄信 KYC 錯誤

## Job 基本資訊

- **Job 名稱**: SendKYCErrorLogJob
- **顯示名稱**: 寄信系統錯誤 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 寄信KYC錯誤
- **工作日檢查**: 是

## 功能說明

寄信 KYC 錯誤排程負責收集 KYC 查詢過程中發生的錯誤日誌，並透過郵件系統批次發送錯誤通知給相關人員。此排程會使用 Razor 模板引擎產生 HTML 郵件內容，並根據執行環境選擇不同的郵件發送方式。

## 驗證資料

### 系統參數驗證
- 檢查 `SysParamManage_BatchSet.SendKYCErrorLog_IsEnabled` 是否為 "Y"
- 若為 "N"，則記錄日誌並結束排程

### 並發控制
- 使用 Semaphore 確保同一時間只有一個批次任務在執行
- 若上一個批次還在執行，則取消本次執行

### 郵件設定驗證
- 檢查 `SysParamManage_MailSet` 是否存在
- 若不存在，則拋出例外

## 商業邏輯

### 主要流程

1. **檢查系統參數設定**
   - 驗證排程是否啟用

2. **查詢待發送的 KYC 錯誤日誌**
   - 查詢 `Reviewer3rd_KYCQueryLog` 中狀態為「等待」的記錄

3. **取得郵件設定**
   - 從 `SysParamManage_MailSet` 取得收件人、主旨、模板設定

4. **批次發送郵件**
   - 將錯誤日誌依批次大小分組
   - 使用 Razor 模板產生郵件內容
   - 根據環境選擇發送方式（開發環境使用 Adapter，正式環境使用 FluentEmail）

5. **更新發送狀態**
   - 發送成功：更新 `KYCLastSendStatus = 成功`
   - 發送失敗：更新 `KYCLastSendStatus = 失敗`
   - 記錄發送時間 `KYCLastSendTime`

### 郵件批次處理

- 批次大小由 `SendKYCErrorLog_BatchSize` 參數控制
- 使用 `Chunk()` 方法將錯誤日誌分批處理
- 每批次獨立發送，發送失敗不影響其他批次

### 郵件模板欄位

從 `SysParamManage_MailSet` 取得：
- `KYCErrorLog_To`: 收件人清單（逗號分隔）
- `KYCErrorLog_Title`: 郵件主旨
- `KYCErrorLog_Template`: Razor 模板名稱

### 錯誤處理

- 每個批次發送失敗會獨立記錄
- 更新該批次錯誤日誌的 `KYCLastSendStatus = 失敗`
- 記錄詳細錯誤訊息到應用程式日誌

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `SysParamManage_BatchSet` 取得系統參數
- 查詢 `SysParamManage_MailSet` 取得郵件設定
- 查詢 `Reviewer3rd_KYCQueryLog` 取得待發送的錯誤日誌

**更新操作**:
- 批次更新 `Reviewer3rd_KYCQueryLog.KYCLastSendStatus`
- 批次更新 `Reviewer3rd_KYCQueryLog.KYCLastSendTime`

### Razor 模板資料

**KYCErrorLogViewModel** 包含以下欄位：
- SeqNo: 序號
- ApplyNo: 申請書編號
- CardStatus: 案件狀態
- CurrentHandler: 當前處理人員
- ID: 身份證字號
- KYCCode: KYC 代碼
- KYCRank: KYC 風險等級
- KYCMsg: KYC 訊息
- Request: 請求內容
- Response: 回應內容
- AddTime: 新增時間
- APIName: API 名稱
- Source: 來源

### 郵件發送方式

**開發環境** (`IsDevelopment`):
- 使用 `IEmailAdapter.SendEmailAsync()`
- 適用於測試環境

**正式環境**:
- 使用 `IFluentEmail.SendAsync()`
- 發送前需清空 `ToAddresses` 以避免重複收件人

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| SysParamManage_BatchSet | 查詢 | 取得批次設定參數 |
| SysParamManage_MailSet | 查詢 | 取得郵件設定 |
| Reviewer3rd_KYCQueryLog | 查詢/更新 | KYC 查詢錯誤日誌 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 僅在工作日執行
- 建議執行頻率：每小時或每日執行

## 注意事項

1. 使用 Semaphore 確保同一時間只有一個實例運行
2. FluentEmail 的 Data 在同個上下文共用，發送前需清空 `ToAddresses`
3. 批次發送失敗不會影響其他批次
4. 使用 Razor 模板引擎產生郵件內容，需確保模板檔案存在
5. 收件人清單使用逗號分隔，系統會自動分割
6. 發送成功後會更新狀態，避免重複發送
7. 批次大小可透過系統參數動態調整
8. 需確保 `SysParamManage_MailSet` 表已正確設定郵件參數
