# SyncApplyInfoPaper API - 紙本建檔同步案件資料 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerCore/SyncApplyInfoPaper` |
| **HTTP 方法** | POST |
| **功能** | 紙本信用卡申請案件資料同步 - 同步正卡人、附卡人、申請卡別及相關詳細資訊 |
| **位置** | `Modules/Reviewer/ReviewerCore/SyncApplyInfoPaper/Endpoint.cs` |
| **複雜度** | ⭐⭐⭐⭐⭐ (最複雜的 API - 包含超過 1387 行的數據定義) |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `X-APPLYNO` | string | ✅ | 申請書編號 (Max: 14 chars) |
| `X-SYNCUSERID` | string | ✅ | 同步員工編號 (Max: 30 chars) |

### 請求體 - 基本資訊

```csharp
public class SyncApplyInfoPaperRequest : IValidatableObject
{
    [Required]
    [MaxLength(14)]
    public string ApplyNo { get; set; }  // 申請書編號

    [Required]
    [ValidEnumValue]
    public SyncStatus SyncStatus { get; set; }  // 同步狀態: 修改=1, 完成=2

    [Required]
    [MaxLength(30)]
    public string SyncUserId { get; set; }  // 同步員工編號

    [ValidEnumValue]
    public CardOwner? CardOwner { get; set; }  // 正附卡別: 正卡=1, 附卡=2, 正卡+附卡=3
}
```

### 同步狀態 (SyncStatus Enum)

| 值 | 名稱 | 說明 |
|----|------|------|
| 1 | 修改 | 暫時保存案件資料，尚未完成 |
| 2 | 完成 | 案件資料已完成填寫，轉入檢核狀態 |

### 正卡人資料 (M_ 前綴)

**個人基本資訊**:
```
✓ 中文姓名 (M_CHName) - 必填, Max: 30
✓ 身分證字號 (M_ID) - Max: 11, 台灣身分證格式驗證
✓ 身分證發證日期 (M_IDIssueDate) - 民國格式 YYYMMDD
✓ 身分證發證地點 (M_IDCardRenewalLocationCode) - Max: 20, 須符合系統設定
✓ 身分證請領狀態 (M_IDTakeStatus) - 初發=1, 補發=2, 換發=3
✓ 性別 (M_Sex) - 男=1, 女=2
✓ 婚姻狀況 (M_MarriageState) - 已婚=1, 未婚=2, 其他=3
✓ 子女數 (M_ChildrenCount) - 整數
✓ 出生日期 (M_BirthDay) - 民國格式 YYYMMDD
✓ 英文姓名 (M_ENName) - Max: 100
```

**國籍與出生地**:
```
✓ 出生地 (M_BirthCitizenshipCode) - 中華民國=1, 其他=2
✓ 出生地其他 (M_BirthCitizenshipCodeOther) - Max: 16, 須符合國籍設定
✓ 國籍 (M_CitizenshipCode) - Max: 5, 須符合系統定義
✓ 學歷 (M_Education) - Enum 值
✓ 是否畢業國小 (M_GraduatedElementarySchool) - Boolean
```

**地址相關 (三個地址: 戶籍、居住、帳單、寄卡)**:
```
戶籍地址 (M_Reg_*)
├─ ZipCode (郵遞區號) - Max: 5
├─ City (縣市) - 須符合系統定義
├─ District (區里)
├─ Road (路街)
├─ Lane (巷)
├─ Alley (弄)
├─ Number (號)
├─ SubNumber (之號)
├─ Floor (樓)
└─ Other (其他地址)

同樣的結構應用於:
├─ 居住地址 (M_Live_*)
├─ 帳單地址 (M_Bill_*)
└─ 寄卡地址 (M_SendCard_*)
```

**聯絡資訊**:
```
✓ 戶籍電話 (M_HouseRegPhone) - Max: 20
✓ 居住電話 (M_LivePhone) - Max: 20
✓ 行動電話 (M_Mobile) - Max: 20
✓ 居住房主 (M_LiveOwner) - Enum
✓ 居住年數 (M_LiveYear) - 整數
✓ 電子郵件 (M_EMail) - Max: 100
```

**公司資訊**:
```
✓ 公司名稱 (M_CompName) - Max: 120
✓ 公司電話 (M_CompPhone) - Max: 20
✓ 公司地址 (M_Comp_*) - 與地址結構相同
✓ 工作年資 (M_WorkYear) - 整數
✓ 職位 (M_Position) - Max: 30
✓ AML 職業別 (M_AMLProfessionCode) - 須符合當前版本定義
✓ AML 職業別其他 (M_AMLProfessionOther) - Max: 50
✓ AML 職級別 (M_AMLJobLevelCode) - 須符合系統定義
```

**財務資訊**:
```
✓ 月收入 (M_MonthlyIncome) - 十進位
✓ 所得及資金來源 (M_MainIncomeAndFundCodes) - 逗號分隔, 須符合定義
✓ 所得及資金來源其他 (M_MainIncomeAndFundOther) - Max: 50
✓ 申請年費 (M_ApplyAnnualFee) - 十進位
✓ 年費收取方式 (AnnualFeePaymentType) - 須符合系統定義
```

**其他資訊**:
```
✓ 公司主要營業項目 (M_CompMainBusiness) - Max: 100
✓ 投資現狀 (M_InvestmentStatus) - Enum
✓ 基金投資金額 (M_FundInvestmentAmount) - 十進位
✓ 股票投資金額 (M_StockInvestmentAmount) - 十進位
```

### 附卡人資料 (S1_ 前綴 ~ S9_ 前綴)

- 支援最多 9 名附卡人 (S1_CHName, S2_CHName, ... S9_CHName)
- 欄位結構與正卡人類似，但以 S{序號}_ 前綴
- 附卡人資訊存儲在 `Reviewer_ApplyCreditCardInfoSupplementary` 表

### 申請卡別資料 (CardInfo 陣列)

```csharp
public class CardInfoDto
{
    [Required]
    public string ApplyCardType { get; set; }  // 申請卡別, 須符合系統定義
}
```

---

## Response 定義

### 成功回應 (200 OK - Return Code: 2000)

```json
{
  "returnCode": 2000,
  "returnMessage": "同步成功: {ApplyNo}",
  "data": "{ApplyNo}",
  "traceId": "{traceId}"
}
```

### 錯誤回應

#### 格式驗證失敗 (400 Bad Request - Return Code: 4000)
- 請求格式不符合定義
- 缺少必填欄位
- 日期格式錯誤
- 身分證格式錯誤

#### 資料庫定義值錯誤 (400 Bad Request - Return Code: 4001)
- 出生地不存在於系統定義
- 國籍代碼無效
- 身分證發證地點不存在
- AML 職業別不存在
- AML 職級別不存在
- 主要收入來源代碼無效
- 卡別不存在
- 年費收取方式無效

#### 商業邏輯有誤 (400 Bad Request - Return Code: 4003)
- 申請書編號不存在於主檔
- 申請書編號不存在於處理檔
- Handle 狀態不符合預期

#### 標頭驗證失敗 (401 Unauthorized - Return Code: 4004)
- 缺少 X-APPLYNO 或 X-SYNCUSERID 標頭

#### 查無此資料 (404 Not Found - Return Code: 4002)
- 申請書編號不存在於系統

#### 內部程式失敗 (500 Internal Server Error - Return Code: 5000)
- 查無對應版本號的 AML 職業別

#### 資料庫執行失敗 (500 Internal Server Error - Return Code: 5002)
- 資料庫操作失敗

---

## 驗證資料

### 1. 格式驗證 (Format Validation)
- **位置**: `ValidateModelState()` 方法 (Line 205-211)
- **驗證方式**: ASP.NET Core ModelState 驗證
- **涵蓋內容**:
  - 必填欄位驗證
  - 欄位長度驗證
  - 日期格式驗證 (民國格式 YYYMMDD)
  - 台灣身分證字號格式驗證

### 2. 資料庫定義值驗證 (Database Defined Value Validation)
- **位置**: `ValidateDbDefinedValues()` 方法 (Line 213-390)
- **說明**: 驗證所有 Enum 型別欄位是否存在於系統定義參數表

#### 驗證項目清單

**基本參數驗證**:
```
✓ 出生地其他 (M_BirthCitizenshipCodeOther)
  └─ 檢查: 類型=國籍, 值=request.M_BirthCitizenshipCodeOther
  └─ 條件: 若 M_BirthCitizenshipCode = 其他, 必須填寫且存在於定義

✓ 正卡國籍 (M_CitizenshipCode)
  └─ 檢查: 類型=國籍, 值必須存在

✓ 身分證發證地點 (M_IDCardRenewalLocationCode)
  └─ 檢查: 類型=身分證換發地點, 值必須存在

✓ AML 職業別 (M_AMLProfessionCode)
  └─ 檢查: 表=SetUp_AMLProfession, Version=currentCaseVersion
  └─ 條件: IsActive = Y
  ├─ 可選欄位, 若填寫須存在
  └─ 若選擇「其他」, 則 M_AMLProfessionOther 必填 (目前此驗證已註釋)

✓ AML 職級別 (M_AMLJobLevelCode)
  └─ 檢查: 類型=AML職級別, 值必須存在

✓ 主要收入來源 (M_MainIncomeAndFundCodes)
  └─ 說明: 逗號分隔的多值欄位
  └─ 檢查: 類型=主要收入來源
  └─ 驗證規則:
     ├─ 每個代碼必須存在於定義
     ├─ 若包含「其他」代碼, M_MainIncomeAndFundOther 必填
     └─ 若不包含「其他」, M_MainIncomeAndFundOther 應為空

✓ 申請卡別 (CardInfo[].ApplyCardType)
  └─ 檢查: 類型=卡片種類
  └─ 驗證方式: 遍歷 CardInfo 陣列, 逐一驗證

✓ 年費收取方式 (AnnualFeePaymentType)
  └─ 檢查: 類型=年費收取方式, 值必須存在
```

**附卡人驗證** (S1_ ~ S9_):
```
✓ 附卡人身分證發證地點 (S1_IDCardRenewalLocationCode, etc.)
  └─ 檢查: 類型=身分證換發地點

✓ 附卡人出生地其他 (S1_BirthCitizenshipCodeOther, etc.)
  └─ 檢查: 類型=國籍
  └─ 條件: 若 S1_BirthCitizenshipCode = 其他, 必須填寫且存在

✓ 附卡人國籍 (S1_CitizenshipCode, etc.)
  └─ 檢查: 類型=國籍
```

**半形轉換**:
- **位置**: `MapHelper.ToHalfWidthRequest()` (MapHelper.cs)
- **說明**: 將所有中文字元 (全形) 轉換為半形
- **應用項目**:
  - 所有姓名欄位 (CHName, ENName)
  - 所有地址欄位 (City, District, Road, etc.)
  - 所有電話欄位
  - 所有文字輸入欄位
- **特殊處理**: 縣市「台」轉換為「臺」 (使用 `AddressHelper.將縣市台字轉換為臺字()`)

### 3. 商業邏輯驗證 (Business Logic Validation)
- **位置**: `ValidateBusinessLogic()` 方法 (非直接提供，但在 Handle 中執行)
- **驗證內容**:

```
檢查點1: 申請書編號是否存在
├─ 查詢 Main (主檔)
├─ 若為 null → 拋出 NotFoundException
│  └─ 訊息: "申請書編號 {ApplyNo} 不存在"
└─ 否則繼續

檢查點2: Handle 的狀態驗證
├─ 查詢第一筆 Handle 記錄
├─ 檢查是否處於「約定狀態」(目前尚未明確定義的狀態檢查)
└─ 若不符合 → 拋出 BusinessBadRequestException
```

---

## 資料處理

### 1. 執行流程 (Handle 方法)

```
Request 進入
    │
    ▼
①  資料驗證格式 (ValidateModelState)
    │
    ▼
②  查詢既有資料 (GetExistingData)
    ├─ 查詢 Handle
    ├─ 查詢 Main
    └─ 查詢 Processes
    │
    ▼
③  驗證資料是否存在 (ValidateAMLNotFoundData)
    ├─ 檢查 Main ≠ null
    ├─ 取得 AMLProfessionCode_Version
    └─ 取得驗證代碼 (AMLProfessionOtherCode, MainIncomeAndFundOtherCode)
    │
    ▼
④  驗證資料庫定義值 (ValidateDbDefinedValues)
    ├─ 查詢所有參數值
    ├─ 逐項驗證 Enum 代碼
    └─ 若有錯誤 → 拋出 DatabaseDefinitionException
    │
    ▼
⑤  商業邏輯驗證 (ValidateBusinessLogic)
    ├─ 驗證 Handle 狀態
    └─ 檢查狀態轉換合法性
    │
    ▼
⑥  資料轉換 (MapData)
    ├─ 將 Request 轉換為 Handle 物件
    ├─ 將 Request 轉換為 Supplementary 物件
    └─ 根據 CardOwner 判斷是否新增/修改
    │
    ▼
⑦  狀態轉換 (同步狀態 = 完成)
    ├─ 若 SyncStatus = 完成
    └─ 所有 Handle 的 CardStatus → 紙本件_待檢核 (20100)
    │
    ▼
⑧  產生流程歷程 (GetProcessesForAdd)
    ├─ 比對既有 Process
    ├─ 新增或修改 Process 記錄
    └─ 若 SyncStatus = 完成, 額外新增一條 Process
    │
    ▼
⑨  準備檢核相關資訊 (SyncStatus = 完成)
    ├─ 新增 OutsideBankInfo (外部銀行資訊)
    ├─ 新增 ApplyNote (申請備註)
    ├─ 新增 InternalCommunicate (內部溝通)
    ├─ 新增 BankTrace (銀行查詢紀錄)
    ├─ 新增 FinanceCheckInfo (財務檢查資訊)
    └─ 新增 PaperApplyCardCheckJob (紙本卡片檢核工作)
    │
    ▼
⑩  資料庫操作 (Database Operations)
    ├─ 刪除舊 Handle 記錄
    ├─ 刪除舊 Supplementary 記錄
    ├─ 新增 Handle
    ├─ 新增 Supplementary (若有附卡人)
    ├─ 新增 Process 記錄
    └─ SaveChangesAsync() 提交
    │
    ▼
Response 200 OK (Return Code: 2000)
```

### 2. 資料映射 (MapData)

#### Handle 映射邏輯

```csharp
MapData() 返回:
├─ newHandleDataList: List<Reviewer_ApplyCreditCardInfoHandle>
│  └─ 根據 CardOwner 產生:
│     ├─ CardOwner = 正卡 → 產生 1 個 Handle (正卡人)
│     ├─ CardOwner = 附卡 → 產生 1 個 Handle (附卡人, 取 S1 資訊)
│     └─ CardOwner = 正卡+附卡 → 產生多個 Handle (正卡人 + 所有附卡人)
│
└─ updatedSupplementaryData: Reviewer_ApplyCreditCardInfoSupplementary
   └─ 若存在附卡人資料, 轉換為 Supplementary 物件
   └─ 若 CardOwner 不包含附卡, Supplementary = null
```

#### Main 更新邏輯 (MapToMainForUpdate)

```
更新項目涵蓋:
├─ 基本資訊: CardOwner, CHName, ID, Sex, 婚姻狀況, 子女數, 等等
├─ 地址資訊: 戶籍地址、居住地址、帳單地址、寄卡地址
├─ 聯絡資訊: 電話, 郵箱, 居住房主, 居住年數
├─ 公司資訊: 公司名稱、電話、地址、職位、工作年資
├─ 財務資訊: 月收入, 所得來源, AML 職業別, 職級別
├─ 投資資訊: 投資狀況, 基金金額, 股票金額
└─ 時間戳: LastUpdateTime, LastUpdateUserId (SYSTEM)
```

### 3. 過程歷程 (Process) 新增

```
GetProcessesForAdd() 邏輯:

比對既有 Process 與新請求的 CardInfo 陣列
├─ 若既有 Process 不存在的 CardInfo → 新增 Process
├─ 若新 CardInfo 不在既有 Process 中 → 新增 Process
└─ 若兩者一致 → 無需修改

若 SyncStatus = 完成:
├─ 新增額外 Process 記錄
│  ├─ Process = 紙本件_待檢核 (CardStatus.紙本件_待檢核)
│  ├─ ProcessUserId = SYSTEM
│  └─ Notes = "紙本同步完成"
└─ 所有已存在的 Handle → CardStatus = 紙本件_待檢核
```

### 4. 檢核準備資料 (SyncStatus = 完成)

```
PrepareApplyCaseEntity() 產生:

①  OutsideBankInfo (外部銀行資訊)
    ├─ 為每個申請卡別創建銀行資訊記錄
    └─ 初始狀態: 待檢核

②  ApplyNote (申請備註)
    ├─ 根據 Request 中的備註欄位創建
    └─ 記錄初始備註

③  InternalCommunicate (內部溝通)
    ├─ 創建初始溝通記錄
    └─ 標記為待聯繫

④  BankTrace (銀行查詢紀錄)
    ├─ 根據 Request 的銀行相關資訊創建
    └─ 待查詢狀態

⑤  FinanceCheckInfo (財務檢查資訊)
    ├─ 根據月收入、資金來源創建
    └─ 待檢查狀態

⑥  PaperApplyCardCheckJob (紙本卡片檢核工作)
    ├─ 為每個申請卡別創建檢核工作
    ├─ 分派給對應的檢核人員
    └─ 優先級: 根據案件複雜度設定
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Reviewer_ApplyCreditCardInfoMain` | UPDATE | 更新正卡人基本資訊 |
| `Reviewer_ApplyCreditCardInfoHandle` | DELETE + INSERT | 刪除舊記錄並新增新記錄 |
| `Reviewer_ApplyCreditCardInfoSupplementary` | DELETE + INSERT | 刪除舊附卡記錄並新增新記錄 |
| `Reviewer_ApplyCreditCardInfoProcess` | INSERT | 新增流程歷程 |
| `Reviewer_OutsideBankInfo` | INSERT | 新增外部銀行資訊 (完成時) |
| `Reviewer_ApplyNote` | INSERT | 新增申請備註 (完成時) |
| `Reviewer_InternalCommunicate` | INSERT | 新增內部溝通記錄 (完成時) |
| `Reviewer_BankTrace` | INSERT | 新增銀行查詢紀錄 (完成時) |
| `Reviewer_FinanceCheckInfo` | INSERT | 新增財務檢查資訊 (完成時) |
| `ReviewerPedding_PaperApplyCardCheckJob` | INSERT | 新增檢核工作 (完成時) |

### 交易管理

```csharp
// 單一資料庫交易 (Main DB 只有一個連線)
await scoreSharpContext.SaveChangesAsync();

// 無分散式交易，所有操作在一個交易內
// 若任何操作失敗，整個事務回滾
```

---

## 業務流程說明

### 流程圖

```
Request SyncApplyInfoPaper
│
├─ 驗證格式 & 半形轉換
├─ 查詢現有 Main, Handle, Process
├─ 驗證資料定義值
├─ 商業邏輯驗證
│
├─────────────────────────────────┬──────────────────────┐
│                                 │                      │
▼                                 ▼                      ▼
同步狀態 = 修改 (1)          同步狀態 = 完成 (2)
│                            │
├─ 清除舊 Handle           ├─ 清除舊 Handle
├─ 清除舊 Supplementary    ├─ 清除舊 Supplementary
├─ 新增 Handle             ├─ 新增 Handle
├─ 新增 Supplementary      ├─ 新增 Supplementary
├─ 新增 Process            │
│  (根據 CardInfo 比對)    ├─ 更新 Main 資訊
│                          ├─ 新增額外 Process
└─ 更新 Main 資訊          │  (紙本件_待檢核)
                           │
                           ├─ 新增檢核準備資料:
                           │  ├─ OutsideBankInfo
                           │  ├─ ApplyNote
                           │  ├─ InternalCommunicate
                           │  ├─ BankTrace
                           │  ├─ FinanceCheckInfo
                           │  └─ PaperApplyCardCheckJob
                           │
                           └─ 更新 Handle CardStatus
                              → 紙本件_待檢核 (20100)
│
▼
SaveChangesAsync() 提交資料庫
│
└─ Response 200 OK (Return Code: 2000)
```

### 正附卡別決策樹

```
CardOwner 値:

1. 正卡 (CardOwner = 1)
   ├─ 僅使用 M_ (正卡人) 資訊
   ├─ 產生 1 個 Handle (申請類型 = 正卡)
   └─ Supplementary = null

2. 附卡 (CardOwner = 2)
   ├─ 僅使用 S1_ (第一附卡人) 資訊
   ├─ 產生 1 個 Handle (申請類型 = 附卡)
   └─ Supplementary = null

3. 正卡+附卡 (CardOwner = 3)
   ├─ 使用 M_ 資訊產生正卡 Handle
   ├─ 使用 S1_~S9_ 資訊產生多個附卡 Handle
   │  (遍歷 S1_CHName, S2_CHName, ... 直至為 null 或空)
   └─ Supplementary = 最後一個非空的附卡人資訊 (通常是 S1)
```

---

## 關鍵業務規則

### 1. AML 職業別版本控制
- 每個案件在「紙本初始」時鎖定一個 AML 版本 (`AMLProfessionCode_Version`)
- 後續同步時，必須驗證所有 AML 代碼是否存在於該版本中
- 版本存儲在 `SetUp_AMLProfession` 表

### 2. 地址標準化
- 所有地址欄位的縣市名稱，「台」統一轉換為「臺」
- 所有字元統一為半形 (全形轉半形)

### 3. 狀態轉換規則

**修改 (SyncStatus = 1)**:
- 案件保持現有狀態
- Handle 和 Supplementary 完全替換
- Process 只新增差異項

**完成 (SyncStatus = 2)**:
- 所有 Handle 轉換為「紙本件_待檢核」(CardStatus = 20100)
- 新增多個檢核相關資訊表記錄
- 案件進入檢核階段

### 4. 申請卡別管理
- CardInfo 是陣列，可以申請多張卡別
- 每個卡別需要在參數表中定義
- Process 記錄會根據 CardInfo 個數決定

### 5. 主要收入來源多值處理
- `M_MainIncomeAndFundCodes` 是逗號分隔的代碼列表
- 若包含「其他」代碼，則 `M_MainIncomeAndFundOther` 必填
- 逐一驗證每個代碼是否存在

### 6. 附卡人上限
- 支援 9 名附卡人 (S1_~S9_)
- 映射時自動檢測非空的附卡人欄位
- 超過 9 名者無法通過格式驗證

---

## 資料大小與複雜度指標

| 項目 | 數值 | 說明 |
|------|------|------|
| **Request 模型欄位數** | ~300+ | 包含所有正卡人、附卡人(9個)、申請卡別等欄位 |
| **Model.cs 程式碼行數** | 1387 | 業界龐大的 Request 定義 |
| **Handler 邏輯行數** | ~734 | 複雜的驗證和轉換邏輯 |
| **產生的資料表記錄** | 10+ | 完成時產生 10+ 個表的記錄 |
| **驗證項目數** | ~50+ | 包括格式、定義值、業務邏輯 |

---

## 已知限制與註釋

| 項目 | 說明 | 狀態 |
|------|------|------|
| AML 職業別「其他」驗證 | 若選擇「其他」是否必填「其他」說明，目前註釋掉此驗證 | ⚠️ 待確認 |
| 專案代號驗證 | 參數表驗證目前已禁用 | ⚠️ 不驗證 |
| 推廣單位驗證 | 參數表驗證目前已禁用 | ⚠️ 不驗證 |

---

## 測試建議

### 單元測試場景

1. **格式驗證**:
   - 缺少必填欄位
   - 身分證字號格式錯誤
   - 日期格式錯誤

2. **定義值驗證**:
   - 無效的國籍代碼
   - 不存在的 AML 職業別
   - 無效的卡別代碼

3. **業務邏輯**:
   - 申請書編號不存在
   - 修改與完成狀態的差異處理

4. **資料轉換**:
   - 全形轉半形
   - 縣市「台」轉「臺」
   - 正附卡別組合

5. **資料庫操作**:
   - 大數據量的 Handle 和 Supplementary 刪除
   - 檢核資料完整新增

