# UpdateReviewManualCaseJob - 更新成人工徵審案件

## Job 基本資訊

- **Job 名稱**: UpdateReviewManualCaseJob
- **顯示名稱**: 更新成人工徵審案件 - 執行人員：{0}
- **Queue**: batch
- **自動重試**: 0 次
- **標籤**: 更新成人工徵審案件
- **工作日檢查**: 是

## 功能說明

更新成人工徵審案件排程負責將已完成月收入確認或完成 KYC 入檔作業的案件自動更新為「人工徵信中」狀態，清除當前處理人員，並記錄處理歷程，讓案件進入人工徵審階段。

**注意**: 此排程為測試用排程，主要用於開發和測試環境。

## 驗證資料

### 並發控制
- 使用 Semaphore 確保同一時間只有一個批次任務在執行
- 若上一個批次還在執行，則取消本次執行

## 商業邏輯

### 主要流程

1. **查詢待更新案件**
   - 查詢狀態為「完成月收入確認」或「網路件_卡友_完成 KYC 入檔作業」的案件
   - 取得案件的 SeqNo 和 ApplyNo

2. **更新案件狀態**
   - 批次更新 `Reviewer_ApplyCreditCardInfoHandle.CardStatus` 為「人工徵信中」
   - 批次更新 `Reviewer_ApplyCreditCardInfoMain.CurrentHandleUserId` 為 null

3. **記錄處理歷程**
   - 新增處理歷程到 `Reviewer_ApplyCreditCardInfoProcess`
   - Process 設定為「人工徵信中」
   - Notes 設定為「測試_更新成人工徵審案件」
   - ProcessUserId 設定為「SYSTEM」

### 案件狀態轉換

| 原狀態 | 新狀態 |
|-------|-------|
| 30018 (完成月收入確認) | 10201 (人工徵信中) |
| 30117 (網路件_卡友_完成 KYC 入檔作業) | 10201 (人工徵信中) |

### 處理邏輯

**清除當前處理人員**:
- 更新 `CurrentHandleUserId = null`
- 表示案件尚未分配給特定人員
- 等待後續派案作業分配

**記錄處理歷程**:
- 每個申請書編號記錄一筆歷程
- StartTime 和 EndTime 使用相同時間
- ProcessUserId 固定為「SYSTEM」

### 錯誤處理

- 所有錯誤會記錄到應用程式日誌
- 使用 try-catch 包覆整個處理流程
- 錯誤不會中斷排程執行

## 資料處理

### 資料庫操作

**查詢操作**:
- 查詢 `Reviewer_ApplyCreditCardInfoHandle` 取得待更新案件
- 查詢 `Reviewer_ApplyCreditCardInfoMain` 取得案件主檔資料

**更新操作**:
- 批次更新 `Reviewer_ApplyCreditCardInfoHandle.CardStatus`
- 批次更新 `Reviewer_ApplyCreditCardInfoMain.CurrentHandleUserId`

**新增操作**:
- 批次新增 `Reviewer_ApplyCreditCardInfoProcess` 處理歷程

### 使用 EF Core 批次更新

```csharp
await context
    .Reviewer_ApplyCreditCardInfoHandle
    .Where(x => updateCase.Select(y => y.SeqNo).Distinct().Contains(x.SeqNo))
    .ExecuteUpdateAsync(x =>
        x.SetProperty(y => y.CardStatus, CardStatus.人工徵信中));

await context
    .Reviewer_ApplyCreditCardInfoMain
    .Where(x => updateMain.Select(y => y.ApplyNo).Distinct().Contains(x.ApplyNo))
    .ExecuteUpdateAsync(x =>
        x.SetProperty(y => y.CurrentHandleUserId, y => null));
```

### 處理歷程記錄

```csharp
new Reviewer_ApplyCreditCardInfoProcess()
{
    ApplyNo = x.ApplyNo,
    ProcessUserId = "SYSTEM",
    StartTime = now,
    EndTime = now,
    Process = CardStatus.人工徵信中.ToString(),
    Notes = "測試_更新成人工徵審案件",
}
```

### 相關資料表

| 資料表名稱 | 操作類型 | 說明 |
|-----------|---------|------|
| Reviewer_ApplyCreditCardInfoHandle | 查詢/更新 | 案件處理資料，更新案件狀態 |
| Reviewer_ApplyCreditCardInfoMain | 查詢/更新 | 申請人主檔資料，清除當前處理人員 |
| Reviewer_ApplyCreditCardInfoProcess | 新增 | 處理歷程記錄 |

## 執行時機

- 由 Hangfire 排程系統定時觸發
- 僅在工作日執行
- **注意**: 此為測試排程，正式環境應謹慎使用

## 注意事項

1. 使用 Semaphore 確保同一時間只有一個實例運行
2. 此排程為測試用途，Notes 欄位標註「測試_更新成人工徵審案件」
3. 使用 `ExecuteUpdateAsync` 批次更新提升效能
4. 處理歷程中的 ProcessUserId 固定為「SYSTEM」
5. 清除當前處理人員後，案件需透過派案排程重新分配
6. 使用 `Distinct()` 避免重複更新相同申請書編號
7. 每個申請書編號只記錄一筆處理歷程（而非每個 SeqNo）
8. 正式環境使用前應確認是否需要此自動化流程
9. 建議在測試環境使用，確認邏輯正確後再部署到正式環境
