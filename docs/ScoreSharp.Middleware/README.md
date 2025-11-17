# ScoreSharp.Middleware Reviewer APIs 文檔

本目錄包含 ScoreSharp.Middleware 中 Reviewer 模塊所有核心 API 的商業邏輯文檔。

## 文檔結構

### 📋 API 文檔清單

| 文件名 | API 名稱 | 功能 | 複雜度 |
|--------|---------|------|--------|
| [Reviewer_EcardNewCase.md](./Reviewer_EcardNewCase.md) | EcardNewCase | 新進件 API | ⭐⭐⭐⭐⭐ |
| [Reviewer_EcardSupplement.md](./Reviewer_EcardSupplement.md) | EcardSupplement | 補件 API | ⭐⭐⭐⭐ |
| [Reviewer_EcardMyDataSuccess.md](./Reviewer_EcardMyDataSuccess.md) | EcardMyDataSuccess | MyData 取件成功 API | ⭐⭐⭐⭐ |
| [Reviewer_EcardMyDataFail.md](./Reviewer_EcardMyDataFail.md) | EcardMyDataFail | MyData 取件失敗 API | ⭐⭐⭐ |
| [Reviewer_EcardSupplementMyDataFail.md](./Reviewer_EcardSupplementMyDataFail.md) | EcardSupplementMyDataFail | 補件 MyData 失敗 API | ⭐⭐⭐ |

## 每份文檔的內容

每份文檔都包含以下章節:

### 1. API 基本資訊
- 端點路徑
- HTTP 方法
- Content-Type
- 功能說明
- 源代碼位置

### 2. Request 定義
- 請求參數結構 (C# 代碼)
- 必填和可選欄位說明
- 參數驗證規則
- 範例請求

### 3. Response 定義
- 成功回應格式
- 各種錯誤回應
- 回覆代碼說明

### 4. 驗證資料
- 格式驗證規則
- 商業邏輯驗證規則
- 條件性驗證邏輯

### 5. 資料處理
- 資料準備階段
- 資料映射流程
- 資料庫操作步驟
- 分散式事務管理

### 6. 業務流程說明
- 完整流程圖
- 決策樹
- 狀態轉換邏輯

### 7. 關鍵業務規則
- 進件類型組合
- 狀態轉換規則
- 特殊處理邏輯

### 8. 涉及的資料表
- 新增表 (INSERT)
- 更新表 (UPDATE)
- 查詢表 (SELECT)
- 存儲過程清單

### 9. 異常處理
- 常見異常類型
- 錯誤代碼對照表
- 異常處理方式

### 10. 性能考慮
- 快取策略
- 優化建議
- 批量操作

## API 概覽

### 進件流程

```
新進件 (EcardNewCase)
    │
    ├─ 卡友/非卡友/國旅卡進件
    ├─ 驗證申請書編號/卡片代碼
    ├─ 處理浮水印
    ├─ 新增主檔/處理檔/歷程
    └─ 產生待檢查任務 (A02/NotA02)
         │
         ├─ 補件請求
         │  └─ EcardSupplement API
         │     ├─ 獲取補件檔案 (FTP)
         │     ├─ 壓印浮水印
         │     ├─ 更新卡片狀態
         │     └─ 新增進程記錄
         │          │
         │          ├─ MyData 核驗成功
         │          │  └─ EcardMyDataSuccess API
         │          │     ├─ 獲取 MyData 附件 (FTP)
         │          │     ├─ 壓印浮水印
         │          │     └─ 轉換為待檢核狀態
         │          │
         │          └─ MyData 核驗失敗
         │             └─ EcardSupplementMyDataFail API
         │                ├─ 檢查補件狀態
         │                ├─ 進行狀態轉換
         │                └─ 新增進程記錄
         │
         └─ MyData 核驗
            ├─ 成功: EcardMyDataSuccess API
            │        ├─ 獲取 MyData 附件 (FTP)
            │        ├─ 壓印浮水印
            │        └─ 轉換為待檢核狀態
            │
            └─ 失敗: EcardMyDataFail API
                     ├─ 檢查卡片狀態
                     ├─ 進行狀態轉換
                     └─ 新增進程記錄
```

## 回覆代碼統一對照表

### 通用回覆代碼

| 代碼 | 含義 | 適用 API |
|-----|------|---------|
| 0000 | 匯入成功 | 全部 |
| 0001 | 必要欄位為空值 | 全部 |
| 0002 | 長度過長 | EcardSupplement, MyData系列 |
| 0003 | 其他異常訊息 | 全部 |

### 新進件特定代碼 (EcardNewCase)

| 代碼 | 含義 |
|-----|------|
| 0004 | 無法對應卡片代碼 |
| 0005 | 資料異常非定義值 |
| 0006 | 資料異常資料長度過長 |
| 0007 | 必要欄位不能為空值 |
| 0008 | 申請書異常 |
| 0009 | 附件異常 |
| 0010 | UUID重複 |
| 0012 | 其他異常訊息 |
| 0013 | ECARD_FILE_DB錯誤 |
| 0014 | 查無申請書附件檔案 |

## 關鍵資料表

### 核心業務表

| 表名 | 說明 |
|------|------|
| `Reviewer_ApplyCreditCardInfoMain` | 申請主檔 |
| `Reviewer_ApplyCreditCardInfoHandle` | 進件處理檔 (卡片狀態) |
| `Reviewer_ApplyCreditCardInfoProcess` | 進件歷程 |
| `Reviewer_ApplyCreditCardInfoFile` | 檔案元數據 |
| `Reviewer_ApplyFile` | 實際檔案內容 (File DB) |
| `Reviewer_CardRecord` | 卡片狀態紀錄 |

### 相關業務表

| 表名 | 說明 |
|------|------|
| `Reviewer_BankTrace` | 銀行迹象 |
| `Reviewer_FinanceCheckInfo` | 財務檢查信息 |
| `Reviewer_OutsideBankInfo` | 外部銀行信息 |
| `Reviewer_ApplyNote` | 申請備註 |
| `Reviewer_InternalCommunicate` | 內部溝通紀錄 |

### 待處理表

| 表名 | 說明 |
|------|------|
| `ReviewerPedding_WebApplyCardCheckJobForA02` | A02 檢查任務 |
| `ReviewerPedding_WebApplyCardCheckJobForNotA02` | 非 A02 檢查任務 |
| `ReviewerPedding_WebRetryCase` | 重試案件 |

## 重要概念

### 進件來源 (Source)
- **Ecard**: E-CARD 系統進件
- **APP**: 行動銀行 APP 進件
- **紙本**: 紙本申請進件

### 卡主類別 (CardOwner)
- **1**: 正卡人
- **2**: 附卡人
- **3**: 正卡+附卡
- **4**: 附卡2
- **5**: 正卡+附卡2

### 徵信代碼
- **A02**: 原卡友 (簡化驗證規則)
- **空值**: 非卡友 (詳細驗證規則)

### 卡片狀態常見值
- `網路件_等待MyData附件`: 非卡友進件待 MyData
- `網路件_非卡友_待檢核`: 非卡友進件待檢核
- `補件作業中`: 補件進行中
- `補回件`: 補件完成待核卡
- `紙本件_待月收入預審`: 紙本進件待月收入預審
- `網路件_待月收入預審`: 網路進件待月收入預審

### CardStep (卡片步驟)
- **月收入確認**: 待月收入預審階段
- **人工徵審**: 人工審查階段

## 數據流向

### 新進件到檢核的完整流程

```
1. 新進件 (EcardNewCase)
   ├─ Input: 申請書編號, 身份證, 卡別等
   ├─ Validation: 格式/重複性/卡片代碼驗證
   ├─ Processing: 浮水印, 主檔/處理檔新增
   └─ Output: 進件成功, 產生待檢查任務

2. 補件 (EcardSupplement)
   ├─ Input: 身份證, 補件編號, 附件檔名
   ├─ Processing: FTP 獲取檔案, 浮水印
   ├─ Update: CardStatus 轉換
   └─ Output: 補件完成

3. MyData 核驗 (Success / Fail)
   ├─ Input: 申請書編號, MyData 案件編號
   ├─ Processing: 檔案獲取 (成功時)
   ├─ Update: CardStatus 轉換
   └─ Output: 轉換為待檢核或重試狀態

4. 補件 MyData 失敗 (EcardSupplementMyDataFail)
   ├─ Input: 身份證號
   ├─ Processing: 批量查詢補件案件
   ├─ Update: 多筆 CardStatus 轉換
   └─ Output: 進程記錄
```

## 配置項參考

重要的系統配置項:

| 配置項 | 說明 | 位置 |
|--------|------|------|
| `WatermarkText` | 浮水印文本 | appsettings.json |
| `ValidationSetting:AMLProfessionCode_Version` | AML 職業代碼版本 | 數據庫參數表 |
| `FTPOption:FixedEcardSupplementFolderPath` | FTP 補件目錄 | appsettings.json |
| CITS 卡列表 | 國旅卡列表 | 快取/參數表 |

## 常見問題

### Q: 如何判斷進件是否需要壓印浮水印?
A: 根據 `EcardNewCase.md` 中的浮水印策略:
- 非卡友進件: 需要
- 國旅卡進件: 需要
- A02 原卡友進件: 不需要

### Q: EcardNewCase 涉及多少個資料表?
A: 新增 12+ 個表, 查詢多個參數表。詳見 `Reviewer_EcardNewCase.md` 中的涉及表章節。

### Q: 補件和 MyData 的區別是什麼?
A:
- 補件: 申請人主動提交補充資料
- MyData: 從政府 MyData 平臺核驗的結果回傳

### Q: 一個身份證號可以對應多少個進件?
A: 理論上無限制。但補件和 MyData 失敗 API 可一次查詢多筆案件。

### Q: 事務失敗時會自動回滾嗎?
A: 是的。所有 API 都使用 try-catch 的事務管理, 失敗時自動回滾。

## 編寫風格參考

這些文檔參考了 `ScoreSharp.PaperMiddleware` 的編寫風格:
- 使用表格清晰展示數據結構
- 使用流程圖說明業務邏輯
- 包含代碼示例說明實現細節
- 詳細的驗證規則和邏輯說明

## 版本歷史

| 版本 | 日期 | 說明 |
|------|------|------|
| 1.0 | 2025-11-17 | 初版: 5 個 API 完整文檔 |

## 相關文檔

- [ScoreSharp.PaperMiddleware 文檔](../ScoreSharp.PaperMiddleware/Reviewer/README.md)
- [Middleware 業務邏輯文檔](./Reviewer-APIs.md) (已移除, 由本文檔替代)
- 源代碼: `src/ScoreSharp.Middleware/Modules/Reviewer/ReviewerCore/`

## 貢獻指南

修改這些文檔時:
1. 保持章節結構一致性
2. 更新表格中的信息
3. 如有新的回覆代碼, 更新回覆代碼章節
4. 更新版本歷史和相關日期

