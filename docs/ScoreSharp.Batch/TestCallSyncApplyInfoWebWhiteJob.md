# TestCallSyncApplyInfoWebWhiteJob - 測試排程呼叫紙本件同步網路小白件

## Job 基本資訊

- **Job 名稱**: TestCallSyncApplyInfoWebWhiteJob
- **顯示名稱**: 測試排程_呼叫紙本件同步網路小白件 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 測試排程_呼叫紙本件同步網路小白件
- **工作日檢查**: 是

## 功能說明

測試排程呼叫紙本件同步網路小白件負責將網路小白件申請資料同步到紙本件系統。小白件是指無法透過電子方式完成審核的網路申請案件，需要列印紙本申請書並透過郵寄方式完成申請。此排程會呼叫 PaperMiddleware API 將網路申請資料轉換為紙本件格式。

**注意**: 此排程為測試用排程，主要用於開發和測試環境。

## 驗證資料

### 並發控制
- 使用 Semaphore 確保同一時間只有一個批次任務在執行
- 若上一個批次還在執行，則取消本次執行

## 商業邏輯

### 主要流程

1. **查詢待同步案件**
   - 查詢狀態為「網路件_書面申請等待MyData」或「網路件_書面申請等待列印申請書及回郵信封」的案件
   - 這些是需要轉換為紙本件的網路申請案件

2. **建立同步請求**
   - 為每個案件建立同步請求 (`CreateSyncApplyInfoWebWhiteReqRequest`)
   - 設定 SyncUserId 為「SYSTEM」

3. **呼叫 PaperMiddleware API**
   - 先呼叫 `CreateSyncApplyInfoWebWhiteReq` 建立同步請求
   - 再呼叫 `SyncApplyInfoWebWhite` 執行同步作業

4. **處理同步結果**
   - 記錄同步成功或失敗的訊息
   - 失敗時記錄錯誤原因
   - 繼續處理下一個案件（不中斷）

### 小白件案件狀態

排程處理以下狀態的案件：
- `網路件_書面申請等待MyData` (30025)
- `網路件_書面申請等待列印申請書及回郵信封` (30023)

### 同步邏輯

**小白件特性**:
- 申請人無足夠的電子驗證資料
- 需要列印紙本申請書
- 需要郵寄申請書到銀行
- 可能需要等待 MyData 資料

**同步流程**:
1. 建立同步請求 (CreateSyncApplyInfoWebWhiteReq)
2. 執行同步作業 (SyncApplyInfoWebWhite)
3. 同步成功後，案件會轉換為對應的紙本件狀態

### 錯誤處理

- API 呼叫失敗會記錄錯誤訊息
- 回傳代碼非「成功」會記錄失敗原因
- 使用 try-catch 捕捉例外
- 發生錯誤時繼續處理下一個案件（使用 continue）
- 所有錯誤會記錄到應用程式日誌

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `Reviewer_ApplyCreditCardInfoHandle` 取得待同步案件

### 外部 API 呼叫

**PaperMiddleware API**:
- `CreateSyncApplyInfoWebWhiteReq`: 建立同步請求
- `SyncApplyInfoWebWhite`: 執行同步作業

**API 請求物件**:
```csharp
CreateSyncApplyInfoWebWhiteReqRequest
{
    ApplyNo = handle.ApplyNo,
    SyncUserId = UserIdConst.SYSTEM
}
```

**API 回應驗證**:
- 檢查 `ReturnCodeStatus` 是否為「成功」
- 失敗時記錄 `ErrorMessage`

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| Reviewer_ApplyCreditCardInfoHandle | 查詢 | 查詢待同步的小白件案件 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 僅在工作日執行
- **注意**: 此為測試排程，正式環境應謹慎使用

## 注意事項

1. 使用 Semaphore 確保同一時間只有一個實例運行
2. 此排程為測試用途，標籤明確標示「測試排程」
3. API 呼叫失敗不會中斷整個排程，會繼續處理下一個案件
4. SyncUserId 固定使用「SYSTEM」
5. 小白件需要列印紙本申請書和回郵信封
6. 同步後案件會轉換為對應的紙本件狀態
7. 需確保 PaperMiddleware API 可正常連線
8. 建議在測試環境使用，確認邏輯正確後再部署到正式環境
9. 正式環境使用前應確認是否需要此自動化流程
10. 記錄會詳細標註執行人員和案件編號

## 小白件業務說明

**什麼是小白件**:
- 網路申請但無法完成電子驗證的案件
- 申請人可能沒有信用卡歷史或電子驗證資料
- 需要透過紙本方式完成身份驗證

**處理流程**:
1. 申請人在網路上填寫申請資料
2. 系統判定無法完成電子驗證
3. 將案件標示為「書面申請」
4. 透過此排程同步到紙本件系統
5. 列印申請書和回郵信封
6. 郵寄給申請人
7. 申請人填寫簽名後寄回
8. 銀行收到後繼續審核流程
