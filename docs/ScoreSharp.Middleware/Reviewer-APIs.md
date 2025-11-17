# ScoreSharp.Middleware - Reviewer APIs 商業邏輯文件

## 目錄
- [概述](#概述)
- [API 清單](#api-清單)
- [1. EcardNewCase - ECARD 進件 API](#1-ecardnewcase---ecard-進件-api)
- [2. EcardSupplement - ECARD 補件 API](#2-ecardsupplement---ecard-補件-api)
- [3. EcardMyDataSuccess - MyData 取件成功 API](#3-ecardmydatasuccess---mydata-取件成功-api)
- [4. EcardMyDataFail - MyData 取件失敗 API](#4-ecardmydatafail---mydata-取件失敗-api)
- [5. EcardSupplementMyDataFail - 補件 MyData 失敗 API](#5-ecardsupplementmydatafail---補件-mydata-失敗-api)

---

## 概述

本文件說明 ScoreSharp.Middleware 專案中 Reviewer 模組的所有 API 端點的商業邏輯，包括：
- Request/Response 定義
- 資料驗證規則
- 資料處理流程

**基礎路由**: `[controller]/[action]`
**控制器**: `ReviewerCoreController`
**所在位置**: `src/ScoreSharp.Middleware/Modules/Reviewer/ReviewerCore/`

---

## API 清單

| API 名稱 | HTTP 方法 | 用途 | Content-Type |
|---------|----------|------|-------------|
| EcardNewCase | POST | ECARD 進件 | application/json |
| EcardSupplement | POST | ECARD 補件 | application/x-www-form-urlencoded |
| EcardMyDataSuccess | POST | MyData 取件成功 | application/json |
| EcardMyDataFail | POST | MyData 取件失敗 | application/json |
| EcardSupplementMyDataFail | POST | 補件 MyData 失敗 | application/json |

---

## 1. EcardNewCase - ECARD 進件 API

### 1.1 API 資訊
- **端點**: `/ReviewerCore/EcardNewCase`
- **HTTP 方法**: POST
- **Content-Type**: application/json
- **說明**: 處理 ECARD 新進件申請，支援卡友與非卡友申請

### 1.2 Request 定義

#### EcardNewCaseRequest

**必填欄位**:
| 欄位名稱 | JSON 屬性名 | 資料型別 | 說明 |
|---------|------------|---------|------|
| ApplyNo | APPLY_NO | string | 申請書編號（長度必須為 13） |
| CardOwner | CARD_OWNER | string | 主附卡別 (1=正卡, 2=附卡, 3=正卡+附卡, 4=附卡2, 5=正卡+附卡2) |
| ApplyCardType | MCARD_SER | string | 卡片種類（可多選，以 / 分隔，如 JA00/JC00） |
| FormCode | FORM_ID | string | 表單代碼 |
| CHName | CH_NAME | string | 中文姓名 |
| Birthday | BIRTHDAY | string | 出生日期（YYYMMDD 民國年） |
| ID | P_ID | string | 身分證字號 |
| Source | SOURCE_TYPE | string | 進件方式（Ecard/APP） |
| CardAppId | APPLICATION_FILE_NAME | string | 申請書檔名（ECard 流水序號） |

**選填欄位** (部分):
- IDType: 身分別（卡友/存戶/持他行卡/自然人憑證）
- CreditCheckCode: 徵信代碼（卡友=A02, 非卡友=空值）
- Sex: 性別（1=男, 2=女）
- Mobile: 行動電話
- EMail: 電子信箱
- AMLProfessionCode: AML 職業類別
- AMLJobLevelCode: AML 職級別
- BirthPlace: 出生地
- 地址相關欄位 (戶籍、居住、公司)
- IsStudent: 是否學生身分
- LineBankUUID: UUID

**完整欄位說明**: 參見 `EcardNewCase/Models/EcardNewCaseRequest.cs`（共約 850 行屬性定義）

### 1.3 Response 定義

#### EcardNewCaseResponse

| 欄位名稱 | JSON 屬性名 | 資料型別 | 說明 |
|---------|------------|---------|------|
| ID | ID | string | 回覆代碼 |
| RESULT | RESULT | string | 回覆結果（中文） |

**回覆代碼對照表**:

| 代碼 | 說明 |
|------|------|
| 0000 | 匯入成功 |
| 0001 | 申請書編號長度不符 |
| 0003 | 申請書編號重複進件或申請書編號不對 |
| 0004 | 無法對應卡片代碼 |
| 0005 | 資料異常非定義值 |
| 0006 | 資料異常資料長度過長 |
| 0007 | 必要欄位不能為空值 |
| 0008 | 申請書異常 |
| 0009 | 附件異常 |
| 0010 | UUID 重複 |
| 0012 | 其它異常訊息 |
| 0013 | ECARD_FILE_DB 錯誤 |
| 0014 | 查無申請書附件檔案 |

### 1.4 資料驗證

#### 1.4.1 基本驗證
1. **申請書編號長度檢查**
   - 必須為 13 碼
   - 檢查方法: `VerifyHelper.檢查_申請書編號是否長度為13`

2. **申請書編號重複檢查**
   - 查詢 `Reviewer_ApplyCreditCardInfoMain` 表
   - 不可重複進件

3. **申請書編號格式檢查**
   - 檢查方法: `VerifyHelper.檢查_申請書編號格式是否正確`

4. **卡片代碼驗證**
   - 對照參數表的卡片種類（Type = 卡片種類, IsActive = Y）
   - 支援多選，以 / 分隔

5. **LineBankUUID 重複檢查**
   - 若提供 UUID，不可重複

#### 1.4.2 必要欄位驗證
檢查以下欄位不能為空:
- ApplyNo（申請書編號）
- CardOwner（主附卡別）
- ApplyCardType（卡片種類）
- FormCode（表單代碼）
- CHName（中文姓名）
- Birthday（出生日期）
- ID（身分證字號）
- Source（進件方式）

#### 1.4.3 徵信代碼驗證
- 只能為 "A02" 或空值
- 檢查方法: `VerifyHelper.檢查_徵信代碼只能為A02或者空值`

#### 1.4.4 資料異常檢查（非卡友）
執行方法: `檢查_資料異常非定義值_非卡友`

**驗證項目包括**:
- 身分別: 需存在於 IDType 列舉
- 主附卡別: 需存在於 CardOwner 列舉
- 性別: 需存在於 Sex 列舉（1=男, 2=女）
- 出生日期: 民國年格式檢查
- 出生地: 需在縣市參數表中或為"其它"
- 國籍: 需在國籍參數表中
- 身分證: 格式檢查（`VerifyHelper.檢查是否身分證格式`）
- 身分證發證日期: 民國年格式檢查
- 身分證發證地點: 需在 `SetUp_IDCardRenewalLocation` 中
- 身分證請領狀態: 1=初發, 2=補發, 3=換發
- 帳單地址/寄卡地址: 1=同戶籍, 2=同居住, 3=同公司
- 行動電話: 台灣手機格式（09 開頭）
- Email: Email 格式驗證
- AML 職業類別: 需在參數表中
- AML 職級別: 需在參數表中
- 現職月收入: 數字檢查
- 主要所得及資金來源: 需在參數表中（支援多選，以逗號分隔）
- 帳單形式: 1=電子帳單, 2=簡訊帳單, 3=紙本帳單, 4=LINE 帳單
- 自動扣繳繳款方式: 10=最低, 20=全額
- 進件方式: Ecard/APP
- 來源 IP: IPv4 格式驗證
- OTP 手機: 台灣手機格式
- OTP 時間: DateTime 格式檢查
- 申請卡種: 1=實體, 2=數位, 3=實體+數位
- 附件註記: 1=附件異常, 2=MYDATA 後補
- 學歷: 1=博士, 2=碩士, 3=大學, 4=專科, 5=高中高職, 6=其他
- 居住地所有權人: 1=本人, 2=配偶, 3=父母親, 4=親屬, 5=宿舍, 6=租貸, 7=其他
- 統一編號: 統一編號格式驗證
- 年資: 數字檢查
- **地址完整性檢查**: 戶籍地址、居住地址必須包含縣市/區域/路名

**學生身分額外驗證**:
若 `IsStudent = "Y"`:
- ParentName（家長姓名）不能為空
- ParentPhone（家長電話）不能為空
- 家長電話格式: 09 開頭為手機格式，否則為市話格式
- 家長居住地址必須包含縣市/區域/路名

#### 1.4.5 資料異常檢查（卡友）
執行方法: `檢查_資料異常非定義值_卡友`

**驗證項目** (相對於非卡友較簡化):
- 主附卡別
- 性別
- 出生日期
- 出生地
- 國籍
- 身分證格式
- 行動電話
- Email
- AML 職業類別
- AML 職級別
- 主要所得及資金來源
- 帳單形式
- 進件方式
- 來源 IP
- OTP 相關
- 申請卡種
- **CardAddr（寄卡地址）**: 必須填寫

### 1.5 資料處理流程

#### 1.5.1 前置處理
1. **全形轉半形**: `MapHelper.ToHalfWidthRequest`
2. **案件類型判斷**:
   - 檢查是否為國旅卡: `檢查_是否為國旅卡`
   - 建立 CaseContext: `MapHelper.MapToCaseContext`
3. **查詢參數**: `查詢_參數` (使用 Cache)

#### 1.5.2 申請書及附件處理
1. **查詢申請書及附件**:
   - 查詢 eCard_file.dbo.ApplyFile 表
   - 使用 `CardAppId` 關聯

2. **轉換申請書**:
   - 檢查 `UploadPDF` 是否存在
   - 若不存在，記錄錯誤代碼 0008

3. **壓印附件浮水印** (條件: 非卡友或國旅卡):
   - 處理附件: idPic1, idPic2, upload1-6
   - 使用 `IWatermarkHelper.ImageWatermarkAndGetBytes`
   - 浮水印文字來自 Configuration: `WatermarkText`
   - 若所有附件皆無資料，記錄錯誤代碼 0009

#### 1.5.3 資料對映
將 Request 對映至以下資料表實體:
- `Reviewer_ApplyFile`: 申請書檔案（存至 ScoreSharp_File）
- `Reviewer_ApplyCreditCardInfoFile`: 申請書檔案記錄
- `Reviewer_BankTrace`: 銀行追蹤資訊
- `Reviewer_InternalCommunicate`: 內部溝通記錄
- `Reviewer_FinanceCheckInfo`: 財務檢查資訊
- `Reviewer_ApplyCreditCardInfoHandle`: 處理資訊
- `Reviewer_ApplyCreditCardInfoMain`: 主要申請資訊
- `Reviewer_OutsideBankInfo`: 外部銀行資訊
- `Reviewer_ApplyCreditCardInfoProcess`: 流程記錄
- `Reviewer_ApplyNote`: 申請備註
- `System_ErrorLog`: 錯誤記錄（如有）
- `ReviewerPedding_WebApplyCardCheckJobForA02`: 卡友審核作業
- `ReviewerPedding_WebApplyCardCheckJobForNotA02`: 非卡友審核作業

#### 1.5.4 特殊欄位處理
1. **出生地國籍處理**:
   ```csharp
   if (BirthPlace 在台灣縣市中)
   {
       BirthCitizenshipCode = "中華民國";
       IsFATCAIdentity = null;
   }
   else
   {
       BirthCitizenshipCode = "其他";
       BirthCitizenshipCodeOther = BirthPlaceOther;
   }
   ```

2. **身分證換發地點代碼轉換**:
   - 從參數表取得 Name 對 Code 的對應

#### 1.5.5 分散式交易處理
執行方法: `ExecuteDistributedTransaction`

**兩階段提交**:
1. **Phase 1: 準備階段**
   - ScoreSharp_File Context: 新增 `Reviewer_ApplyFile`
   - ScoreSharp Context: 新增所有其他資料表記錄

2. **Phase 2: 提交階段**
   - SaveChanges ScoreSharp_File
   - SaveChanges ScoreSharp
   - Commit 兩個 Transaction

**錯誤處理**:
- **資料長度過長錯誤** (SQL Server Error 2628):
  - 回滾交易
  - 執行 `Retry處理`
  - 回覆代碼 0006

- **其他錯誤**:
  - 回滾交易
  - 拋出例外

#### 1.5.6 Retry 處理
當遇到特定錯誤代碼時 (0005/0006/0007/0013/0014)，執行 `Retry處理`:
1. 新增基本申請資訊至 Handle、Main 表
2. 新增 Process 記錄
3. 新增 `ReviewerPedding_WebRetryCase` 記錄
   - 保留原始 Request JSON
   - 記錄錯誤代碼和錯誤訊息
   - IsSend = "N"

### 1.6 錯誤處理
- 所有驗證失敗會寫入 `System_ErrorLog`
- Project: "MIDDLEWARE"
- Source: "EcardNewCase"
- Type: 依錯誤類型分類
- 包含 Request/Response JSON

### 1.7 日誌記錄
- 使用 Serilog 的 `PushProperties` 記錄 ApplyNo
- 使用 `DiagnosticContext` 記錄關鍵屬性
- 記錄所有驗證失敗和異常

---

## 2. EcardSupplement - ECARD 補件 API

### 2.1 API 資訊
- **端點**: `/ReviewerCore/EcardSupplement`
- **HTTP 方法**: POST
- **Content-Type**: application/x-www-form-urlencoded
- **說明**: 處理 ECARD 補件作業

### 2.2 Request 定義

#### EcardSupplementRequest

| 欄位名稱 | Form 欄位名 | 資料型別 | 必填 | 說明 |
|---------|------------|---------|------|------|
| ID | P_ID | string | 是 | 正卡身分證字號（最長 11 碼） |
| SupplementNo | SUP_NO | string | 是 | 補件編號（最長 20 碼） |
| AppendixFileName_01 | APPENDIX_FILE_NAME_01 | string | 否 | 附件檔名 1（最長 100 碼） |
| AppendixFileName_02 | APPENDIX_FILE_NAME_02 | string | 否 | 附件檔名 2（最長 100 碼） |
| AppendixFileName_03 | APPENDIX_FILE_NAME_03 | string | 否 | 附件檔名 3（最長 100 碼） |
| AppendixFileName_04 | APPENDIX_FILE_NAME_04 | string | 否 | 附件檔名 4（最長 100 碼） |
| AppendixFileName_05 | APPENDIX_FILE_NAME_05 | string | 否 | 附件檔名 5（最長 100 碼） |
| AppendixFileName_06 | APPENDIX_FILE_NAME_06 | string | 否 | 附件檔名 6（最長 100 碼） |
| MyDataNo | MYDATA_NO | string | 否 | MyData 編號（最長 36 碼） |

**範例**:
```
P_ID=K12798732&
SUP_NO=20250409175522259&
APPENDIX_FILE_NAME_01=20250409_17552282159__3.jpg&
APPENDIX_FILE_NAME_02=&
APPENDIX_FILE_NAME_03=&
APPENDIX_FILE_NAME_04=&
APPENDIX_FILE_NAME_05=&
APPENDIX_FILE_NAME_06=&
MYDATA_NO=
```

### 2.3 Response 定義

#### EcardSupplementResponse

| 欄位名稱 | JSON 屬性名 | 資料型別 | 說明 |
|---------|------------|---------|------|
| ID | ID | string | 回覆代碼 |
| RESULT | RESULT | string | 回覆結果（中文） |

**回覆代碼對照表**:

| 代碼 | 說明 |
|------|------|
| 0000 | 匯入成功 |
| 0001 | 必要欄位為空值 |
| 0002 | 長度過長 |
| 0003 | 其他錯誤訊息 |

### 2.4 資料驗證

#### 2.4.1 必要欄位檢查
```csharp
必要欄位不能為空值 = string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(SupplementNo)
```
- ID（身分證字號）必填
- SupplementNo（補件編號）必填

#### 2.4.2 長度檢查
```csharp
欄位值長度是否過長 =
    ID.Length > 11 ||
    SupplementNo.Length > 20 ||
    MyDataNo?.Length > 36

檔案名稱是否過長 =
    任一 AppendixFileName 長度 > 100
```

#### 2.4.3 業務規則檢查
1. **查無 ID 對應資料**:
   - 執行 Stored Procedure: `Usp_GetECardSupplementInfo`
   - 若查無資料，回覆代碼 0003

### 2.5 資料處理流程

#### 2.5.1 查詢補件資訊
1. **執行 SP**: `Usp_GetECardSupplementInfo`
   - 參數: ID
   - 回傳: `List<ECardSupplementInfo>`
     - HandleSeqNo: 處理序號
     - ApplyNo: 申請書編號
     - PrePageNo: 前一頁號
     - CardStatus: 卡片狀態
     - UserType: 使用者類型（正卡人/附卡人）
     - CardStep: 卡片步驟（月收入確認/人工徵審）
     - Source: 來源（紙本/APP/ECARD）
     - ApplyCardType: 申請卡片種類

2. **GroupBy ApplyNo**:
   - 依申請書編號分組處理

#### 2.5.2 FTP 檔案處理
1. **取出檔案名稱**:
   - 使用反射取得所有 `AppendixFileName_XX` 且有值的屬性

2. **FTP 下載檔案**:
   - 使用 `IFTPHelper.GetMultipleFilesBytesAsync`
   - 路徑: `FTPOption.FixedEcardSupplementFolderPath`
   - 若下載失敗，回覆代碼 0003

3. **浮水印加工**: `浮水印加工`
   - PDF 檔案: `_watermarkHelper.PdfWatermarkAndGetBytes`
   - 圖片檔案: `_watermarkHelper.ImageWatermarkAndGetBytes`
   - 浮水印文字: 從 Configuration 取得 `WatermarkText`

#### 2.5.3 產生補件資訊 Context
執行方法: `GenerateECardSupplementInfoContext`

**針對每個 ApplyNo**:
1. **處理 Handle 資訊**:
   - 篩選 CardStatus = 補件作業中 的記錄
   - 計算補件後卡片狀態: `計算補件後卡片狀態`
     - (紙本, 月收入確認) → 紙本件_待月收入預審
     - (紙本, 人工徵審) → 補回件
     - (APP, 月收入確認) → 網路件_待月收入預審
     - (APP, 人工徵審) → 補回件
     - (ECARD, 月收入確認) → 網路件_待月收入預審
     - (ECARD, 人工徵審) → 補回件

2. **新增 Process 記錄**:
   - Process = "補回件"
   - Notes = `完成附件補檔FROM ECard;補件編號:{SupplementNo};MyData編號:{MyDataNo}`

3. **新增 CardRecord 記錄**:
   - CardStatus = 計算後的卡片狀態
   - HandleNote = `完成附件補檔FROM ECard;(正卡/附卡_{ApplyCardType})`

4. **新增檔案記錄**:
   - `Reviewer_ApplyCreditCardInfoFile`: 檔案 Log
     - Page = PrePageNo + 1
     - Process = "補回件"
     - AddUserId = "SYSTEM"
     - IsHistory = "N"
     - DBName = "ScoreSharp_File"
   - `Reviewer_ApplyFile`: 實際檔案內容
     - FileName = `{ApplyNo}_{原檔名}`
     - FileContent = 浮水印加工後的 bytes
     - FileType = "申請書相關"

#### 2.5.4 分散式交易處理
執行方法: `ExecuteDistributedTransaction`

**準備階段**:
1. **ScoreSharp_File Context**:
   - 新增 `Reviewer_ApplyFile`

2. **ScoreSharp Context**:
   - 新增 `Reviewer_ApplyCreditCardInfoFile`
   - 新增 `Reviewer_ApplyCreditCardInfoProcess`
   - 新增 `Reviewer_CardRecord`
   - 更新 `Reviewer_ApplyCreditCardInfoHandle`:
     ```sql
     UPDATE Reviewer_ApplyCreditCardInfoHandle
     SET CardStatus = {AfterCardStatus},
         ApproveUserId = NULL,
         ApproveTime = NULL
     WHERE SeqNo = {HandleSeqNo}
     ```
   - 更新 `Reviewer_ApplyCreditCardInfoMain`:
     ```csharp
     SetProperty(x => x.LastUpdateUserId, "SYSTEM")
     SetProperty(x => x.LastUpdateTime, now)
     ```

**提交階段**:
- SaveChanges 兩個 Context
- Commit 兩個 Transaction

**錯誤處理**:
- 回滾交易
- 記錄錯誤日誌

### 2.6 錯誤處理
- 所有錯誤會寫入 `System_ErrorLog`
- Project: "MIDDLEWARE"
- Source: "EcardSupplement"
- Type 類型:
  - 補件資料驗證失敗
  - 補件FTP下載失敗
  - 內部程式錯誤

### 2.7 日誌記錄
- 記錄: SupplementNo, ID, MyDataNo
- 使用 Serilog Activity 記錄浮水印處理

---

## 3. EcardMyDataSuccess - MyData 取件成功 API

### 3.1 API 資訊
- **端點**: `/ReviewerCore/EcardMyDataSuccess`
- **HTTP 方法**: POST
- **Content-Type**: application/json
- **說明**: 處理 MyData 取件成功後的附件上傳

### 3.2 Request 定義

#### EcardMyDataSuccessRequest

| 欄位名稱 | JSON 屬性名 | 資料型別 | 必填 | 說明 |
|---------|------------|---------|------|------|
| ID | P_ID | string | 是 | 正卡身分證字號（最長 11 碼） |
| ApplyNo | APPLY_NO | string | 是 | 申請書編號（最長 13 碼） |
| MyDataNo | MYDATA_NO | string | 是 | MyData 案件編號（最長 36 碼） |
| AppendixFileName_01 | APPENDIX_FILE_NAME_01 | string | 否 | 附件檔名 1（最長 100 碼） |
| AppendixFileName_02 | APPENDIX_FILE_NAME_02 | string | 否 | 附件檔名 2（最長 100 碼） |
| AppendixFileName_03 | APPENDIX_FILE_NAME_03 | string | 否 | 附件檔名 3（最長 100 碼） |
| AppendixFileName_04 | APPENDIX_FILE_NAME_04 | string | 否 | 附件檔名 4（最長 100 碼） |
| AppendixFileName_05 | APPENDIX_FILE_NAME_05 | string | 否 | 附件檔名 5（最長 100 碼） |
| AppendixFileName_06 | APPENDIX_FILE_NAME_06 | string | 否 | 附件檔名 6（最長 100 碼） |

**範例**:
```
P_ID=K12798732&
APPLY_NO=20250508H5563&
APPENDIX_FILE_NAME_01=20250409_17552282159__3.jpg&
APPENDIX_FILE_NAME_02=&
APPENDIX_FILE_NAME_03=&
APPENDIX_FILE_NAME_04=&
APPENDIX_FILE_NAME_05=&
APPENDIX_FILE_NAME_06=&
MYDATA_NO=e37b48ca-82da-49da-a605-bdc23b082186
```

### 3.3 Response 定義

#### EcardMyDataSuccessResponse

| 欄位名稱 | JSON 屬性名 | 資料型別 | 說明 |
|---------|------------|---------|------|
| ID | ID | string | 回覆代碼 |
| RESULT | RESULT | string | 回覆結果（中文） |

**回覆代碼對照表**:

| 代碼 | 說明 |
|------|------|
| 0000 | 匯入成功 |
| 0001 | 必要欄位為空值 |
| 0002 | 長度過長 |
| 0003 | 其他錯誤訊息 |

### 3.4 資料驗證

#### 3.4.1 必要欄位檢查
```csharp
檢查必填 =
    string.IsNullOrEmpty(ID) ||
    string.IsNullOrEmpty(ApplyNo) ||
    string.IsNullOrEmpty(MyDataNo)
```

#### 3.4.2 長度檢查
```csharp
長度過長檢查 =
    ID.Length > 11 ||
    ApplyNo.Length > 13 ||
    MyDataNo.Length > 36 ||
    任一 AppendixFileName 長度 > 100
```

#### 3.4.3 業務規則檢查
1. **查詢附件所需相關資訊**:
   - 執行 SP: `Usp_GetECardMyDataInfo`
   - 參數: ID, ApplyNo, MyDataNo
   - 回傳: `EcardMyDataInfoDto`
     - HandleSeqNo
     - PrePageNo
     - CardStatus
     - ApplyCardType

2. **檢查資料存在性**:
   - 若 CardStatus、ApplyCardType、HandleSeqNo 任一為 null
   - 表示查無等待MyData附件或書面申請等待MyData的申請書資料
   - 回覆代碼 0003

### 3.5 資料處理流程

#### 3.5.1 FTP 檔案處理
與 EcardSupplement 相同:
1. 取出檔案名稱
2. FTP 下載檔案（`FTPOption.FixedEcardSupplementFolderPath`）
3. 浮水印加工

#### 3.5.2 卡片狀態轉換
執行方法: `轉換取件後卡片狀態`

**轉換規則**:
- 網路件_等待MyData附件 → 網路件_非卡友_待檢核
- 網路件_書面申請等待MyData → 網路件_書面申請等待列印申請書及回郵信封

#### 3.5.3 產生 Context
執行方法: `ExecuteDistributedTransaction`

**建立記錄**:
1. **Process 記錄 1**: MyData 取回成功
   - Process = "網路件_MyData取回成功"
   - Notes = `完成MyData取回FROM ECard;MyData編號:{MyDataNo}`

2. **Process 記錄 2**: 狀態轉移
   - Process = 轉換後的卡片狀態
   - Notes = `完成MyData取回FROM ECard;(正卡_{ApplyCardType})`
   - StartTime/EndTime = now + 1 秒

3. **檔案記錄**:
   - `Reviewer_ApplyCreditCardInfoFile`:
     - Page = PrePageNo + pageCount
     - Process = "網路件_MyData取回成功"
     - AddUserId = "SYSTEM"
   - `Reviewer_ApplyFile`:
     - FileName = `{ApplyNo}_{原檔名}`
     - FileContent = 浮水印加工後的 bytes

#### 3.5.4 分散式交易處理

**準備階段**:
1. **ScoreSharp_File Context**:
   - 新增 `Reviewer_ApplyFile`

2. **ScoreSharp Context**:
   - 新增 `Reviewer_ApplyCreditCardInfoProcess` (2 筆)
   - 新增 `Reviewer_ApplyCreditCardInfoFile`
   - 更新 `Reviewer_ApplyCreditCardInfoHandle`:
     ```csharp
     Attach(handle)
     Entry(handle).Property(u => u.CardStatus).IsModified = true
     ```
   - 更新 `Reviewer_ApplyCreditCardInfoMain`:
     ```csharp
     Attach(main)
     Entry(main).Property(u => u.LastUpdateUserId).IsModified = true
     Entry(main).Property(u => u.LastUpdateTime).IsModified = true
     ```

**提交階段**:
- SaveChanges 兩個 Context
- Commit 兩個 Transaction

### 3.6 錯誤處理
- 所有錯誤會寫入 `System_ErrorLog`
- Project: "MIDDLEWARE"
- Source: "EcardMyDataSuccess"
- Type: MyData進件成功資料驗證失敗 / 內部程式錯誤

### 3.7 日誌記錄
- 記錄: ApplyNo, ID, MyDataNo
- 使用 Serilog Activity 記錄浮水印處理

---

## 4. EcardMyDataFail - MyData 取件失敗 API

### 4.1 API 資訊
- **端點**: `/ReviewerCore/EcardMyDataFail`
- **HTTP 方法**: POST
- **Content-Type**: application/json
- **說明**: 處理 MyData 取件失敗的狀態更新

### 4.2 Request 定義

#### EcardMyDataFailRequest

| 欄位名稱 | JSON 屬性名 | 資料型別 | 必填 | 說明 |
|---------|------------|---------|------|------|
| ID | P_ID | string | 是 | 正卡身分證字號（最長 11 碼） |
| ApplyNo | APPLY_NO | string | 是 | 申請書編號（最長 13 碼） |
| MyDataNo | MYDATA_NO | string | 是 | MyData 案件編號（最長 36 碼） |

**範例**:
```json
{
  "P_ID": "K12798732",
  "APPLY_NO": "20250508H5563",
  "MYDATA_NO": "e37b48ca-82da-49da-a605-bdc23b082186"
}
```

### 4.3 Response 定義

#### EcardMyDataFailResponse

| 欄位名稱 | JSON 屬性名 | 資料型別 | 說明 |
|---------|------------|---------|------|
| ID | ID | string | 回覆代碼 |
| RESULT | RESULT | string | 回覆結果（中文） |

**回覆代碼對照表**:

| 代碼 | 說明 |
|------|------|
| 0000 | 匯入成功 |
| 0001 | 必要欄位為空值 |
| 0002 | 長度過長 |
| 0003 | 其他錯誤訊息 |

### 4.4 資料驗證

#### 4.4.1 必要欄位檢查
所有欄位皆為必填:
- ID（身分證字號）
- ApplyNo（申請書編號）
- MyDataNo（MyData 案件編號）

#### 4.4.2 長度檢查
```csharp
長度過長檢查 =
    ID.Length > 11 ||
    ApplyNo.Length > 13 ||
    MyDataNo.Length > 36
```

#### 4.4.3 業務規則檢查
與 EcardMyDataSuccess 相同:
- 執行 SP: `Usp_GetECardMyDataInfo`
- 檢查 CardStatus、ApplyCardType、HandleSeqNo 是否存在

### 4.5 資料處理流程

#### 4.5.1 卡片狀態轉換
執行方法: `傳換取件後卡片狀態`

**轉換規則** (與成功相同):
- 網路件_等待MyData附件 → 網路件_非卡友_待檢核
- 網路件_書面申請等待MyData → 網路件_書面申請等待列印申請書及回郵信封

#### 4.5.2 資料庫更新

**新增 Process 記錄**:
1. **Process 記錄 1**: MyData 取回失敗
   - Process = "網路件_MyData取回失敗"
   - Notes = `MyData取件失敗FROM ECard;MyData編號：{MyDataNo}`

2. **Process 記錄 2**: 狀態轉移
   - Process = 轉換後的卡片狀態
   - Notes = `MyData取件失敗FROM ECard;(正卡_{ApplyCardType})`
   - StartTime/EndTime = now + 1 秒

**更新 Handle**:
```csharp
Attach(handle)
Entry(handle).Property(u => u.CardStatus).IsModified = true
```

**更新 Main**:
```csharp
Attach(main)
Entry(main).Property(u => u.LastUpdateUserId).IsModified = true
Entry(main).Property(u => u.LastUpdateTime).IsModified = true
```

**執行 SaveChanges**

### 4.6 錯誤處理
- 所有錯誤會寫入 `System_ErrorLog`
- Project: "MIDDLEWARE"
- Source: "EcardMyDataFail"
- Type: MyData進件失敗資料驗證失敗 / 內部程式錯誤

### 4.7 日誌記錄
- 記錄: ApplyNo, ID, MyDataNo

---

## 5. EcardSupplementMyDataFail - 補件 MyData 失敗 API

### 5.1 API 資訊
- **端點**: `/ReviewerCore/EcardSupplementMyDataFail`
- **HTTP 方法**: POST
- **Content-Type**: application/json
- **說明**: 處理補件時 MyData 取件失敗的狀態更新

### 5.2 Request 定義

#### EcardSupplementMyDataFailRequest

| 欄位名稱 | JSON 屬性名 | 資料型別 | 必填 | 說明 |
|---------|------------|---------|------|------|
| ID | P_ID | string | 是 | 正卡身分證字號（最長 11 碼） |

**範例**:
```json
{
  "P_ID": "K12798732"
}
```

### 5.3 Response 定義

#### EcardSupplementMyDataFailResponse

| 欄位名稱 | JSON 屬性名 | 資料型別 | 說明 |
|---------|------------|---------|------|
| ID | ID | string | 回覆代碼 |
| RESULT | RESULT | string | 回覆結果（中文） |

**回覆代碼對照表**:

| 代碼 | 說明 |
|------|------|
| 0000 | 匯入成功 |
| 0001 | 必要欄位為空值 |
| 0002 | 長度過長 |
| 0003 | 其他錯誤訊息 |

### 5.4 資料驗證

#### 5.4.1 必要欄位檢查
```csharp
ID 不能為空
```

#### 5.4.2 長度檢查
```csharp
ID.Length > 11
```

#### 5.4.3 業務規則檢查
1. **查詢補件資訊**:
   - 執行 SP: `Usp_GetECardSupplementInfo`
   - 參數: ID, MyDataNo = String.Empty
   - 回傳: `List<ECardSupplementInfo>`

2. **檢查資料存在性**:
   - GroupBy ApplyNo
   - 若查無資料，回覆代碼 0003

### 5.5 資料處理流程

#### 5.5.1 產生補件資訊 Context

**針對每個 ApplyNo**:
1. **處理 Handle 資訊**:
   - 篩選 CardStatus = 補件作業中
   - 計算補件後卡片狀態（與 EcardSupplement 相同）

2. **新增 Process 記錄**:
   - **Record 1**: MyData 取回失敗
     - Process = "MyData取回失敗"
     - Notes = `補件MyData取回失敗FROM ECard;`
   - **Record 2+**: 每個 Handle 的狀態轉移
     - Process = 計算後的卡片狀態
     - Notes = `補件MyData取回失敗FROM ECard;(正卡/附卡_{ApplyCardType})`

3. **新增 CardRecord 記錄**:
   - CardStatus = 計算後的卡片狀態
   - HandleNote = `完成附件補檔FROM ECard;(正卡/附卡_{ApplyCardType})`

#### 5.5.2 資料庫更新

**新增記錄**:
- `Reviewer_ApplyCreditCardInfoProcess`
- `Reviewer_CardRecord`

**更新 Handle**:
```sql
UPDATE Reviewer_ApplyCreditCardInfoHandle
SET CardStatus = {AfterCardStatus},
    ApproveUserId = NULL,
    ApproveTime = NULL
WHERE SeqNo = {HandleSeqNo}
```

**更新 Main**:
```csharp
ExecuteUpdateAsync(setters =>
    setters.SetProperty(x => x.LastUpdateUserId, "SYSTEM")
           .SetProperty(x => x.LastUpdateTime, now)
)
```

**執行 SaveChanges**

### 5.6 錯誤處理
- 所有錯誤會寫入 `System_ErrorLog`
- Project: "MIDDLEWARE"
- Source: "EcardSupplementMyDataFail"
- Type: Ecard補件MyData失敗資料驗證失敗 / 補件資料驗證失敗 / 內部程式錯誤

### 5.7 日誌記錄
- 記錄: ID

---

## 附錄

### A. 共用常數定義

#### 回覆代碼 (所有 API 共用結構)
```csharp
public static class 回覆代碼
{
    public static readonly string 匯入成功 = "0000";
    public static readonly string 必要欄位為空值 = "0001";
    public static readonly string 長度過長 = "0002";
    public static readonly string 其它異常訊息 = "0003";
}
```

#### UserIdConst
```csharp
public static class UserIdConst
{
    public static readonly string SYSTEM = "SYSTEM";
}
```

#### SystemErrorLogProjectConst
```csharp
public static class SystemErrorLogProjectConst
{
    public static readonly string MIDDLEWARE = "MIDDLEWARE";
}
```

### B. 資料表結構參考

#### Reviewer_ApplyCreditCardInfoMain
主要申請資訊表

#### Reviewer_ApplyCreditCardInfoHandle
處理資訊表（含 CardStatus）

#### Reviewer_ApplyCreditCardInfoProcess
流程記錄表

#### Reviewer_ApplyCreditCardInfoFile
檔案記錄表

#### Reviewer_ApplyFile
檔案實體表（存於 ScoreSharp_File）

#### Reviewer_CardRecord
卡別記錄表

#### System_ErrorLog
系統錯誤記錄表

### C. 列舉定義

#### CardStatus (部分)
- 補件作業中
- 補回件
- 紙本件_待月收入預審
- 網路件_待月收入預審
- 網路件_等待MyData附件
- 網路件_書面申請等待MyData
- 網路件_非卡友_待檢核
- 網路件_書面申請等待列印申請書及回郵信封
- 網路件_MyData取回成功
- 網路件_MyData取回失敗
- MyData取回失敗

#### Source
- 紙本
- APP
- ECARD

#### CardStep
- 月收入確認
- 人工徵審

#### UserType
- 正卡人
- 附卡人

#### FileType
- 申請書相關

### D. 參考連結

#### 相關檔案位置
- **Endpoint**: `src/ScoreSharp.Middleware/Modules/Reviewer/ReviewerCore/*/Endpoint.cs`
- **Model**: `src/ScoreSharp.Middleware/Modules/Reviewer/ReviewerCore/*/Model.cs`
- **Helper**: `src/ScoreSharp.Middleware/Modules/Reviewer/ReviewerCore/EcardNewCase/Helpers/`

#### Stored Procedures
- `Usp_GetApplyCreditCardInfoWithParams`: 取得參數表
- `Usp_GetECardSupplementInfo`: 取得補件資訊
- `Usp_GetECardMyDataInfo`: 取得 MyData 資訊

---

**文件版本**: 1.0
**最後更新**: 2025-11-17
**維護者**: Claude Code Agent
