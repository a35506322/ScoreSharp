# SyncApplyInfoWebWhite API - 網路件小白同步案件資料 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerCore/SyncApplyInfoWebWhite` |
| **HTTP 方法** | POST |
| **功能** | 網路非卡友 (小白) 信用卡申請案件資料同步 - 更新申請者資料並轉換卡片狀態 |
| **位置** | `Modules/Reviewer/ReviewerCore/SyncApplyInfoWebWhite/Endpoint.cs` |
| **複雜度** | ⭐⭐⭐ (中等複雜度 - 902 行 Request 定義) |

---

## Request 定義

### 請求頭 (Headers)

| 欄位 | 型別 | 必填 | 說明 |
|------|------|------|------|
| `X-APPLYNO` | string | ✅ | 申請書編號 (必須符合格式: 8個數字 + 1個字母(X/Y/Z) + 4個數字) |
| `X-SYNCUSERID` | string | ✅ | 同步員工編號 (Max: 30 chars) |

### 申請書編號格式驗證

```
正規表達式: ^\d{8}[X-Z]{1}\d{4}$

格式說明:
├─ \d{8}    → 8 個數字 (YYYYMMDD)
├─ [X-Z]{1} → 1 個字母 X, Y 或 Z
└─ \d{4}    → 4 個數字 (序號)

範例:
✓ 20250115X0001  (正確)
✓ 20250115Y9999  (正確)
✗ 20250115A0001  (格式錯誤 - A 不在 X-Z 範圍)
✗ 202501150001   (格式錯誤 - 缺少字母)
```

### 請求體 - 基本資訊

```csharp
public class SyncApplyInfoWebWhiteRequest : IValidatableObject
{
    [Required]
    [MaxLength(14)]
    public string ApplyNo { get; set; }  // 申請書編號 (格式如上)

    [Required]
    [MaxLength(30)]
    public string SyncUserId { get; set; }  // 同步員編

    [Required]
    [ValidEnumValue]
    public CardOwner CardOwner { get; set; }  // 正附卡別: 正卡=1, 附卡=2, 正卡+附卡=3

    [Required]
    [MaxLength(30)]
    public string M_CHName { get; set; }  // 正卡_中文姓名

    // ... (以下省略，詳見欄位列表)
}
```

### 正卡人資料 (M_ 前綴)

**個人基本資訊** (與 SyncApplyInfoPaper 類似，但部分欄位必填):

```
✓ 中文姓名 (M_CHName) - 必填, Max: 30
✓ 性別 (M_Sex) - Enum: 男=1, 女=2, 可選
✓ 出生日期 (M_BirthDay) - 民國格式 YYYMMDD, 可選
✓ 英文姓名 (M_ENName) - Max: 100, 可選
✓ 出生地 (M_BirthCitizenshipCode) - 中華民國=1, 其他=2, 可選
✓ 出生地其他 (M_BirthCitizenshipCodeOther) - Max: 16, 可選
✓ 國籍 (M_CitizenshipCode) - Max: 5, 可選
✓ 身分證字號 (M_ID) - Max: 11, 可選
✓ 身分證發證日期 (M_IDIssueDate) - 民國格式 YYYMMDD, 可選
✓ 身分證發證地點 (M_IDCardRenewalLocationCode) - Max: 20, 可選
✓ 身分證請領狀態 (M_IDTakeStatus) - Enum, 可選
✓ 學歷 (M_Education) - Enum, 可選
✓ 是否畢業國小 (M_GraduatedElementarySchool) - Boolean, 可選
```

**地址相關** (四個地址):
```
戶籍地址 (M_Reg_*)
├─ ZipCode, City, District, Road, Lane, Alley, Number, SubNumber, Floor, Other

居住地址 (M_Live_*)
├─ AddressType, ZipCode, City, District, Road, Lane, Alley, Number, SubNumber, Floor, Other

帳單地址 (M_Bill_*)
├─ AddressType, ZipCode, City, District, Road, Lane, Alley, Number, SubNumber, Floor, Other

寄卡地址 (M_SendCard_*)
├─ AddressType, ZipCode, City, District, Road, Lane, Alley, Number, SubNumber, Floor, Other
```

**聯絡資訊**:
```
✓ 戶籍電話 (M_HouseRegPhone) - Max: 20, 可選
✓ 居住電話 (M_LivePhone) - Max: 20, 可選
✓ 行動電話 (M_Mobile) - Max: 20, 可選
✓ 電子郵件 (M_EMail) - Max: 100, 可選
```

**公司資訊**:
```
✓ 公司名稱 (M_CompName) - Max: 120, 可選
✓ 公司電話 (M_CompPhone) - Max: 20, 可選
✓ 公司地址 (M_Comp_*) - 結構同前
✓ 工作年資 (M_WorkYear) - 整數, 可選
✓ 職位 (M_Position) - Max: 30, 可選
✓ AML 職業別 (M_AMLProfessionCode) - 可選
✓ AML 職業別其他 (M_AMLProfessionOther) - Max: 50, 可選
✓ AML 職級別 (M_AMLJobLevelCode) - 可選
✓ 主要收入來源 (M_MainIncomeAndFundCodes) - 逗號分隔, 可選
✓ 主要收入來源其他 (M_MainIncomeAndFundOther) - Max: 50, 可選
```

**其他資訊**:
```
✓ 公司主要營業項目 (M_CompMainBusiness) - Max: 100, 可選
✓ 投資現狀 (M_InvestmentStatus) - Enum, 可選
✓ 基金投資金額 (M_FundInvestmentAmount) - 十進位, 可選
✓ 股票投資金額 (M_StockInvestmentAmount) - 十進位, 可選
✓ 月收入 (M_MonthlyIncome) - 十進位, 可選
```

### 申請卡別資料 (CardInfo 陣列) - 必填

```csharp
public class CardInfoDto
{
    [Required]
    public UserType UserType { get; set; }  // 用卡人類型: 正卡人=1, 附卡人=2

    [Required]
    public string ApplyCardType { get; set; }  // 申請卡別, 須符合系統定義

    [Required]
    public string ApplyCardKind { get; set; }  // 申請卡種, 待確認邏輯

    public string? ID { get; set; }  // 身分證字號
}
```

### 申請流程資料 (ApplyProcess 陣列) - 必填

```csharp
public class ApplyProcessDto
{
    [Required]
    public string Process { get; set; }  // 流程名稱

    [Required]
    public DateTime StartTime { get; set; }  // 開始時間

    [Required]
    public DateTime EndTime { get; set; }  // 結束時間

    [Required]
    public string ProcessUserId { get; set; }  // 流程操作員工編號

    public string? Notes { get; set; }  // 備註
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
- 申請書編號格式錯誤
- 缺少必填欄位 (M_CHName, CardOwner, CardInfo, ApplyProcess)
- 日期格式錯誤
- 國籍代碼無效

#### 資料庫定義值錯誤 (400 Bad Request - Return Code: 4001)
- 出生地不存在於系統定義
- 國籍代碼無效
- 身分證發證地點不存在
- AML 職業別不存在
- AML 職級別不存在
- 主要收入來源代碼無效
- 卡別不存在

#### 商業邏輯有誤 (400 Bad Request - Return Code: 4003)
- 申請書編號格式不符合 `^\d{8}[X-Z]{1}\d{4}$`
- 卡片狀態不符合要求 (非 20012 或 20014)

#### 標頭驗證失敗 (401 Unauthorized - Return Code: 4004)
- 缺少 X-APPLYNO 或 X-SYNCUSERID 標頭

#### 查無此資料 (404 Not Found - Return Code: 4002)
- 申請書編號不存在於主檔
- 申請書編號不存在於處理檔

#### 內部程式失敗 (500 Internal Server Error - Return Code: 5000)
- 系統內部處理錯誤

#### 資料庫執行失敗 (500 Internal Server Error - Return Code: 5002)
- 資料庫操作失敗

---

## 驗證資料

### 1. 格式驗證 (Format Validation)
- **位置**: `資料驗證格式()` 方法 (Line 130-136)
- **驗證方式**: ASP.NET Core ModelState 驗證
- **涵蓋內容**:
  - 必填欄位驗證
  - 欄位長度驗證
  - 日期格式驗證 (民國格式 YYYMMDD)

### 2. 申請書編號格式驗證
- **位置**: `驗證商業邏輯()` 方法 (Line 298-303)
- **驗證規則**:
  ```
  正規表達式: ^\d{8}[X-Z]{1}\d{4}$
  └─ 若不符合 → 拋出 BusinessBadRequestException
     └─ 訊息: "申請書編號格式有誤: {ApplyNo}"
  ```

### 3. 資料庫定義值驗證 (Database Defined Value Validation)
- **位置**: `資料庫定義值驗證格式()` 方法 (Line 138-272)
- **說明**: 驗證所有 Enum 型別欄位是否存在於系統定義參數表

#### 驗證項目清單

```
✓ 出生地其他 (M_BirthCitizenshipCodeOther)
  └─ 類型=國籍, 需存在於參數表

✓ 正卡國籍 (M_CitizenshipCode)
  └─ 類型=國籍, 可選欄位

✓ 身分證發證地點 (M_IDCardRenewalLocationCode)
  └─ 類型=身分證換發地點

✓ AML 職業別 (M_AMLProfessionCode)
  └─ 類型=AML職業別, 可選欄位

✓ AML 職級別 (M_AMLJobLevelCode)
  └─ 類型=AML職級別

✓ 主要收入來源 (M_MainIncomeAndFundCodes)
  └─ 類型=主要收入來源
  └─ 驗證規則:
     ├─ 逗號分隔值，逐一驗證
     ├─ 若包含「其他」代碼，M_MainIncomeAndFundOther 必填
     └─ 若不包含「其他」，M_MainIncomeAndFundOther 應為空

✓ 申請卡別 (CardInfo[].ApplyCardType)
  └─ 類型=卡片種類
  └─ 逐一驗證每個卡別
```

**半形轉換**:
- **位置**: `MapHelper.ToHalfWidthRequest()` (MapHelper.cs)
- **應用**: 同 SyncApplyInfoPaper API

### 4. 商業邏輯驗證 (Business Logic Validation)
- **位置**: `驗證商業邏輯()` 方法 (Line 292-322)

```
檢查點1: 申請書編號格式
├─ 正規表達式: ^\d{8}[X-Z]{1}\d{4}$
├─ 若不符合 → 拋出 BusinessBadRequestException
│  └─ 訊息: "申請書編號格式有誤: {ApplyNo}"
└─ 若符合 → 繼續

檢查點2: 主檔存在性
├─ 查詢 Reviewer_ApplyCreditCardInfoMain
├─ 若為 null → 拋出 NotFoundException
│  └─ 訊息: "查無申請書編號為{ApplyNo}的主檔資料。"
└─ 否則繼續

檢查點3: 處理檔存在性
├─ 查詢 Reviewer_ApplyCreditCardInfoHandle
├─ 若為 null → 拋出 NotFoundException
│  └─ 訊息: "查無申請書編號為{ApplyNo}的處理檔資料。"
└─ 否則繼續

檢查點4: 卡片狀態驗證
├─ 允許的狀態:
│  ├─ 20012: 網路件_書面申請等待MyData
│  └─ 20014: 網路件_書面申請等待列印申請書及回郵信封
├─ 若不符合 → 拋出 BusinessBadRequestException
│  └─ 訊息: "申請書編號 {ApplyNo} 的卡片狀態不符合要求: {現有狀態}"
└─ 若符合 → 驗證通過
```

---

## 資料處理

### 1. 執行流程 (Handle 方法)

```
Request 進入
    │
    ▼
①  資料驗證格式 (資料驗證格式)
    │
    ▼
②  資料庫定義值驗證 (資料庫定義值驗證格式)
    │
    ▼
③  查詢現有資料
    ├─ 查詢 Main (主檔)
    └─ 查詢 Handle (處理檔)
    │
    ▼
④  商業邏輯驗證 (驗證商業邏輯)
    ├─ 申請書編號格式檢查
    ├─ 主檔存在性檢查
    ├─ 處理檔存在性檢查
    └─ 卡片狀態檢查
    │
    ▼
⑤  卡片狀態轉換 (轉換卡片狀態)
    ├─ 若現有狀態 = 20012 (書面申請等待MyData)
    │  └─ 新狀態 = 網路件_等待MyData附件
    └─ 若現有狀態 = 20014 (書面申請等待列印申請書及回郵信封)
       └─ 新狀態 = 網路件_非卡友_待檢核
    │
    ▼
⑥  修改主檔資料 (修改主檔資料)
    ├─ 更新基本資訊 (姓名, 性別, 出生日期等)
    ├─ 更新地址資訊 (四個地址)
    ├─ 更新聯絡資訊 (電話, 郵箱)
    ├─ 更新公司資訊
    └─ 更新時間戳 (LastUpdateTime, LastUpdateUserId)
    │
    ▼
⑦  修改處理檔資料 (修改處理檔資料)
    ├─ CardStatus = 轉換後的狀態
    ├─ ID = 正卡人的身分證字號
    ├─ ApplyCardType = 申請卡別
    └─ ApplyCardKind = 申請卡種
    │
    ▼
⑧  產生流程歷程 (產生歷程)
    ├─ 遍歷 ApplyProcess 陣列
    ├─ 新增所有既有流程記錄
    └─ 新增系統轉換記錄
       ├─ Process = 轉換後的卡片狀態
       ├─ StartTime = now
       ├─ ProcessUserId = req.SyncUserId
       └─ Notes = "網路件小白同步案件資料"
    │
    ▼
⑨  資料庫提交 (SaveChangesAsync)
    └─ 單一資料庫連線，無分散式交易
    │
    ▼
Response 200 OK (Return Code: 2000)
```

### 2. 卡片狀態轉換邏輯

```csharp
轉換卡片狀態 (Handle.CardStatus 作為輸入):

BeforeCardStatus = handle.CardStatus

If BeforeCardStatus == 20012 (網路件_書面申請等待MyData)
├─ AfterCardStatus = 網路件_等待MyData附件
│  └─ 說明: 升級至等待 MyData 附件提供的狀態
│
Else (BeforeCardStatus == 20014)
├─ AfterCardStatus = 網路件_非卡友_待檢核 (30100)
   └─ 說明: 進入待檢核狀態
```

### 3. 主檔更新欄位 (修改主檔資料)

```
修改項目:

基本資訊:
├─ UserType = 正卡人 (固定)
├─ CHName, ID, Sex, BirthDay, ENName
├─ BirthCitizenshipCode, CitizenshipCode
├─ IDIssueDate, IDCardRenewalLocationCode
├─ IDTakeStatus, Education, GraduatedElementarySchool
└─ LastUpdateTime, LastUpdateUserId (SYSTEM)

地址資訊 (四個地址):
├─ 戶籍地址 (Reg_*)
├─ 居住地址 (Live_*)
├─ 帳單地址 (Bill_*)
└─ 寄卡地址 (SendCard_*)

聯絡資訊:
├─ Mobile, EMail
├─ HouseRegPhone, LivePhone
└─ 各類型地址屬性 (AddressType)

公司資訊:
├─ CompName, CompPhone
├─ Comp_* (公司地址)
├─ WorkYear, Position
├─ AMLProfessionCode, AMLProfessionOther
├─ AMLJobLevelCode, MainIncomeAndFundCodes
├─ MainIncomeAndFundOther
├─ CompMainBusiness, InvestmentStatus
├─ FundInvestmentAmount, StockInvestmentAmount
└─ MonthlyIncome
```

### 4. 處理檔更新欄位 (修改處理檔資料)

```
修改項目:

✓ CardStatus → 轉換後的卡片狀態
✓ ID → CardInfo 中正卡人的 ID
✓ ApplyCardType → CardInfo 中正卡人的 ApplyCardType
✓ ApplyCardKind → CardInfo 中正卡人的 ApplyCardKind (邏輯待確認)
```

### 5. 流程歷程新增 (產生歷程)

```
新增 Process 記錄:

①  遍歷 Request.ApplyProcess 陣列
    ├─ 為每個 Process 新增記錄
    └─ 字段直接映射:
       ├─ ApplyNo, Process, StartTime, EndTime, ProcessUserId, Notes
       └─ 保持原始值

②  新增系統轉換記錄
    ├─ ApplyNo = req.ApplyNo
    ├─ Process = cardStatusChangeResult.AfterCardStatus.ToString()
    ├─ StartTime = now
    ├─ EndTime = now
    ├─ ProcessUserId = req.SyncUserId
    └─ Notes = "網路件小白同步案件資料"
```

---

## 資料庫操作

### 涉及的資料表

| 資料表 | 操作 | 說明 |
|--------|------|------|
| `Reviewer_ApplyCreditCardInfoMain` | UPDATE | 更新申請者基本資訊 |
| `Reviewer_ApplyCreditCardInfoHandle` | UPDATE | 更新卡片狀態和申請卡別 |
| `Reviewer_ApplyCreditCardInfoProcess` | INSERT | 新增流程歷程 |

### 交易管理

```csharp
// 單一資料庫交易 (Main DB)
await scoreSharpContext.SaveChangesAsync();

// 所有操作在一個隱含交易內
// 若任何操作失敗，整個事務自動回滾
```

---

## 業務流程說明

### 網路非卡友申請流程

```
初始狀態: 網路小白申請案件
    │
    ├─ 卡片狀態: 20012 (書面申請等待MyData)
    │             或 20014 (書面申請等待列印申請書及回郵信封)
    │
    ▼
調用 SyncApplyInfoWebWhite API
    │
    ├─ 更新申請者個人資訊
    ├─ 更新地址資訊
    ├─ 更新聯絡方式
    ├─ 更新公司資訊 (如有)
    │
    ├─────────────────────────────────────────┬──────────────────────┐
    │                                         │                      │
    ▼                                         ▼                      ▼
狀態1: 書面申請等待MyData (20012)    狀態2: 書面申請等待列印 (20014)
    │                                │
    └─ 轉換至:                        └─ 轉換至:
       ├─ 網路件_等待MyData附件         └─ 網路件_非卡友_待檢核 (30100)
       └─ 等待申請者提供 MyData
    │
    ▼
記錄所有申請流程
    ├─ 來自外部系統的流程資訊 (ApplyProcess)
    └─ 同步系統的狀態轉換記錄
    │
    ▼
案件進入檢核階段或等待 MyData 附件
```

### 卡片狀態轉換決策樹

```
當前 CardStatus:

20012 (書面申請等待MyData)
    │
    └─ 操作: 小白申請人已提供相關資訊
       └─ 新狀態: 網路件_等待MyData附件
          └─ 含義: 等待申請人授權並提供 MyData

20014 (書面申請等待列印申請書及回郵信封)
    │
    └─ 操作: 小白申請人資訊已完成填寫
       └─ 新狀態: 網路件_非卡友_待檢核 (30100)
          └─ 含義: 進入人工檢核階段
```

---

## 關鍵業務規則

### 1. 申請書編號約束
- 必須符合特定格式: `^\d{8}[X-Z]{1}\d{4}$`
- 用於識別網路非卡友的申請案件
- X/Y/Z 的含義待業務確認

### 2. 卡片狀態必須檢查
- 只允許兩個特定狀態的案件進行同步
- 這確保了案件在正確的生命周期階段進行同步

### 3. 主檔與處理檔同時更新
- 主檔: 存儲申請者的詳細資訊
- 處理檔: 存儲卡片的當前狀態
- 兩者必須保持同步

### 4. 流程歷程完整記錄
- ApplyProcess: 來自外部系統 (電簽系統) 的歷史流程
- 系統流程: 由徵審系統自動記錄的轉換
- 兩種流程合併存儲以保留完整歷史

### 5. UserType 固定為正卡人
- 網路小白申請只能以正卡人身分進行
- 不支援附卡人申請

### 6. 申請卡別驗證
- CardInfo 陣列包含一個或多個申請卡別
- 每個卡別必須存在於系統參數
- UserType 用於區分是正卡人還是附卡人的卡別

---

## 已知待確認項目

| 項目 | 說明 | 狀態 |
|------|------|------|
| 申請卡種邏輯 | ApplyCardKind 欄位的賦值邏輯 | ❓ TODO 待確認 |

---

## 與其他 API 的關聯

### 與 SyncApplyFile 的關係

```
SyncApplyFile (圖檔傳送)
└─ 網路小白件 (Status = 3)
   ├─ 檢查條件:
   │  ├─ Handle 存在
   │  ├─ Main 存在
   │  └─ CardStatus ∈ [20012, 20014]
   │
   └─ 新增檔案記錄
      └─ Process = 保持原 CardStatus

          ↓ (之後)

SyncApplyInfoWebWhite
└─ 更新申請者資訊並轉換卡片狀態
   └─ CardStatus 轉換完成
```

### 與 SyncApplyInfoPaper 的區別

```
SyncApplyInfoPaper (紙本同步)
├─ 支援複雜的附卡人資訊
├─ 完成時產生檢核準備資料 (10+ 表)
└─ 通常用於業務人員錄入紙本案件

SyncApplyInfoWebWhite (網路小白同步)
├─ 簡化的資訊結構
├─ 主要用於更新外來資訊和狀態轉換
├─ 無檢核準備資料的產生
└─ 面向自動化的在線申請流程
```

---

## 資料量與複雜度指標

| 項目 | 數值 | 說明 |
|------|------|------|
| **Request 模型欄位數** | ~200+ | 包含個人資訊、地址、公司資訊等 |
| **Model.cs 程式碼行數** | 902 | 相對於 SyncApplyInfoPaper 的 1387 行較簡化 |
| **Handler 邏輯行數** | ~505 | 相對簡潔的驗證和更新邏輯 |
| **涉及資料表** | 3 | Main, Handle, Process |
| **驗證項目數** | ~30+ | 格式、定義值、業務邏輯驗證 |

---

## 測試建議

### 單元測試場景

1. **格式驗證**:
   - 申請書編號不符合格式 (`^\d{8}[X-Z]{1}\d{4}$`)
   - 缺少必填欄位 (M_CHName, CardOwner, CardInfo, ApplyProcess)
   - 日期格式錯誤

2. **定義值驗證**:
   - 無效的國籍代碼
   - 不存在的卡別代碼
   - 無效的主要收入來源代碼

3. **業務邏輯**:
   - 申請書編號不存在
   - 處理檔不存在
   - 卡片狀態既不是 20012 也不是 20014

4. **狀態轉換**:
   - 20012 → 網路件_等待MyData附件
   - 20014 → 網路件_非卡友_待檢核

5. **資料更新**:
   - 驗證主檔的所有欄位已正確更新
   - 驗證處理檔的卡片狀態已轉換
   - 驗證流程歷程已完整記錄

