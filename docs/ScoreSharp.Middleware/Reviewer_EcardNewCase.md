# EcardNewCase API - 新進件 API

## API 基本資訊

| 項目 | 說明 |
|------|------|
| **API 端點** | `/ReviewerCore/EcardNewCase` |
| **HTTP 方法** | POST |
| **Content-Type** | application/json |
| **功能** | 處理來自 E-CARD 系統的新信用卡申請案件 |
| **位置** | `Modules/Reviewer/ReviewerCore/EcardNewCase/Endpoint.cs` |

---

## Request 定義

### 請求體 (Body) - JSON 格式

```csharp
public class EcardNewCaseRequest
{
    // 核心必填欄位
    [JsonPropertyName("APPLY_NO")]
    public string? ApplyNo { get; set; }  // 申請書編號 (長度必須=13, 格式: ^\d{8}[A-Z]{1}\d{4}$)

    [JsonPropertyName("P_ID")]
    public string? ID { get; set; }  // 身份證號

    [JsonPropertyName("CARD_OWNER")]
    public string? CardOwner { get; set; }  // 卡主類別: 1(正卡), 2(附卡), 3(正卡+附卡), 4(附卡2), 5(正卡+附卡2)

    [JsonPropertyName("MCARD_SER")]
    public string? ApplyCardType { get; set; }  // 申請卡別 (可多選, 用/分隔)

    [JsonPropertyName("FORM_ID")]
    public string? FormCode { get; set; }  // 申請書代號

    [JsonPropertyName("CH_NAME")]
    public string? CHName { get; set; }  // 中文姓名

    [JsonPropertyName("BIRTHDAY")]
    public string? Birthday { get; set; }  // 出生年月日 (民國年格式 YYYMMDD)

    [JsonPropertyName("SOURCE_TYPE")]
    public string Source { get; set; }  // 進件來源: Ecard/APP

    // 選擇性欄位
    [JsonPropertyName("CREDIT_ID")]
    public string? CreditCheckCode { get; set; }  // 徵信代碼 (只能"A02"或空值)

    [JsonPropertyName("SEX")]
    public string? Sex { get; set; }  // 性別: 1(男)/2(女)

    [JsonPropertyName("ID_TYPE")]
    public string? IDType { get; set; }  // 身份類別: 卡友/存户/持他行卡/自然人凭证

    [JsonPropertyName("EMAIL")]
    public string? EMail { get; set; }  // 電子郵件

    [JsonPropertyName("CELL_PHONE_NO")]
    public string? Mobile { get; set; }  // 行動電話 (格式: 09\d{8})

    // 戶籍地址 (必要時)
    public string? Reg_Zip { get; set; }
    public string? Reg_City { get; set; }
    public string? Reg_District { get; set; }
    public string? Reg_Road { get; set; }
    public string? Reg_Lane { get; set; }
    public string? Reg_Alley { get; set; }
    public string? Reg_Number { get; set; }
    public string? Reg_SubNumber { get; set; }
    public string? Reg_Floor { get; set; }
    public string? Reg_Other { get; set; }

    // 居住地址 (必要時)
    public string? Home_Zip { get; set; }
    public string? Home_City { get; set; }
    public string? Home_District { get; set; }
    public string? Home_Road { get; set; }
    public string? Home_Lane { get; set; }
    public string? Home_Alley { get; set; }
    public string? Home_Number { get; set; }
    public string? Home_SubNumber { get; set; }
    public string? Home_Floor { get; set; }
    public string? Home_Other { get; set; }

    // 公司地址 (必要時)
    public string? Comp_Zip { get; set; }
    public string? Comp_City { get; set; }
    public string? Comp_District { get; set; }
    public string? Comp_Road { get; set; }
    public string? Comp_Lane { get; set; }
    public string? Comp_Alley { get; set; }
    public string? Comp_Number { get; set; }
    public string? Comp_SubNumber { get; set; }
    public string? Comp_Floor { get; set; }
    public string? Comp_Other { get; set; }

    // 財務資訊
    [JsonPropertyName("SALARY")]
    public string? CurrentMonthIncome { get; set; }  // 本月薪資

    public string? HUOCUN_Balance { get; set; }  // 活存餘額
    public string? DINGCUN_Balance { get; set; }  // 定存餘額
    public string? HUOCUN_Balance_90 { get; set; }  // 活存90天平均餘額
    public string? DINGCUN_Balance_90 { get; set; }  // 定存90天平均餘額

    [JsonPropertyName("MAIN_INCOME")]
    public string? MainIncomeAndFundCodes { get; set; }  // 主要收入與基金代碼 (逗號分隔)

    // 其他資訊
    [JsonPropertyName("P_ID_GETDATE")]
    public string? IDIssueDate { get; set; }  // 身份證發行日期

    [JsonPropertyName("APPLICATION_FILE_NAME")]
    public string? CardAppId { get; set; }  // 申請書檔名

    [JsonPropertyName("UUID")]
    public string? LineBankUUID { get; set; }  // 線銀唯一碼

    [JsonPropertyName("STUDENT_FLG")]
    public string? IsStudent { get; set; }  // 學生旗標: Y/N

    // 家長相關
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    public string? ParentLive_City { get; set; }
    public string? ParentLive_District { get; set; }
    // ... 其他家長欄位
}
```

### 核心必填欄位驗證清單

| 欄位 | JSON 鍵 | 驗證規則 | 錯誤碼 |
|------|--------|--------|--------|
| ApplyNo | APPLY_NO | 長度=13, 格式 `^\d{8}[A-Z]{1}\d{4}$` | 0001/0003 |
| ID | P_ID | 身份證格式 (台灣格式) | 0007 |
| CardOwner | CARD_OWNER | 不能為空 | 0007 |
| ApplyCardType | MCARD_SER | 不能為空, 需在參數表中 | 0004/0007 |
| FormCode | FORM_ID | 不能為空 | 0007 |
| CHName | CH_NAME | 不能為空 | 0007 |
| Birthday | BIRTHDAY | 民國年格式 YYYMMDD | 0007 |
| Source | SOURCE_TYPE | 必須 Ecard 或 APP | 0007 |

### 條件性必填欄位

- **CreditCheckCode**: 如提供必須為 "A02" 或空值
- **Email**: 如提供必須符合格式 `^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$`
- **Mobile**: 如提供必須為 `09\d{8}` 格式

---

## Response 定義

### 成功回應 (200 OK - 回覆代碼: 0000)

```json
{
  "ID": "K12798732",
  "RESULT": "匯入成功"
}
```

### 錯誤回應

#### 申請書編號長度不符 (回覆代碼: 0001)
```json
{
  "ID": "K12798732",
  "RESULT": "申請書編號長度不符"
}
```

#### 申請書編號重複進件或申請書編號不對 (回覆代碼: 0003)
```json
{
  "ID": "K12798732",
  "RESULT": "申請書編號重複進件或申請書編號不對"
}
```

#### 無法對應卡片代碼 (回覆代碼: 0004)
```json
{
  "ID": "K12798732",
  "RESULT": "無法對應卡片代碼"
}
```

#### 資料異常非定義值 (回覆代碼: 0005)
```json
{
  "ID": "K12798732",
  "RESULT": "資料異常非定義值"
}
```

#### 資料異常資料長度過長 (回覆代碼: 0006)
```json
{
  "ID": "K12798732",
  "RESULT": "資料異常資料長度過長"
}
```

#### 必要欄位不能為空值 (回覆代碼: 0007)
```json
{
  "ID": "K12798732",
  "RESULT": "必要欄位不能為空值"
}
```

#### 申請書異常 (回覆代碼: 0008)
```json
{
  "ID": "K12798732",
  "RESULT": "申請書異常"
}
```

#### 附件異常 (回覆代碼: 0009)
```json
{
  "ID": "K12798732",
  "RESULT": "附件異常"
}
```

#### UUID重複 (回覆代碼: 0010)
```json
{
  "ID": "K12798732",
  "RESULT": "UUID重複"
}
```

#### 其他異常訊息 (回覆代碼: 0012)
```json
{
  "ID": "K12798732",
  "RESULT": "其他異常訊息"
}
```

#### ECARD_FILE_DB錯誤 (回覆代碼: 0013)
```json
{
  "ID": "K12798732",
  "RESULT": "ECARD_FILE_DB錯誤"
}
```

#### 查無申請書附件檔案 (回覆代碼: 0014)
```json
{
  "ID": "K12798732",
  "RESULT": "查無申請書附件檔案"
}
```

---

## 驗證資料

### 1. 格式驗證 (FormatValidation)

#### 申請書編號驗證
```
檢查點: ApplyNo 長度和格式
├─ 長度驗證: 必須 = 13 字元
│  └─ 錯誤 → 回覆代碼 0001
├─ 格式驗證: ^\d{8}[A-Z]{1}\d{4}$
│  ├─ 前 8 位: 數字
│  ├─ 第 9 位: 英文大寫字母
│  ├─ 後 4 位: 數字
│  └─ 錯誤 → 回覆代碼 0003
└─ 重複性檢查: 查詢 Reviewer_ApplyCreditCardInfoMain
   └─ 已存在 → 回覆代碼 0003
```

#### 身份證驗證
```
支持格式:
├─ 台灣國民身分證: [A-Z]{1}[1-2]{1}\d{8}
├─ 舊制外籍護照: [A-Z]{2}\d{8}
└─ 新制外籍護照: [A-Z]{1}[8-9]{1}\d{8}
```

#### Email 驗證
```
正則表達式: ^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$
```

#### 手機號碼驗證
```
格式: 09\d{8} (台灣手機號)
```

### 2. 商業邏輯驗證 (BusinessValidation)

#### 必填欄位檢查
```
檢查 8 個核心必填欄位:
├─ ApplyNo: 不能為空
├─ ID: 不能為空
├─ CardOwner: 不能為空
├─ ApplyCardType: 不能為空
├─ FormCode: 不能為空
├─ CHName: 不能為空
├─ Birthday: 不能為空
└─ Source: 不能為空

錯誤 → 回覆代碼 0007
```

#### 徵信代碼驗證
```
檢查點: CreditCheckCode
├─ 如果提供: 只能是 "A02" 或空值
├─ 其他值 → 回覆代碼 0005
└─ 值用來區分原卡友 (A02) vs 非卡友
```

#### 卡片代碼驗證
```
檢查點: ApplyCardType
├─ 從參數表 (SetUp_ApplyCardType) 查詢所有有效的卡片代碼
├─ 檢查 ApplyCardType 中的每個卡片代碼是否在參數表中
├─ 無法對應 → 回覆代碼 0004
└─ 資料異常 → 回覆代碼 0005
```

#### UUID 重複檢查
```
檢查點: LineBankUUID
├─ 如果提供 UUID:
│  ├─ 查詢 Reviewer_ApplyCreditCardInfoMain 中是否已存在
│  ├─ 已存在 → 回覆代碼 0010
│  └─ UUID 用於線銀系統的唯一追蹤
└─ 如果未提供: 略過此檢查
```

#### 國旅卡檢查
```
檢查點: 申請卡別是否為國旅卡 (CITS Card)
├─ 從快取 (IFusionCache) 查詢 CITS 卡列表
├─ 如果是國旅卡:
│  ├─ 簡化驗證規則
│  └─ 強制壓印浮水印
└─ 影響後續資料處理邏輯
```

#### 原卡友 vs 非卡友驗證差異
```
原卡友 (CreditCheckCode = "A02")
├─ 簡化驗證規則
└─ 較少必填欄位限制

非卡友 (CreditCheckCode 為空或無)
├─ 詳細驗證規則
├─ 需驗證身份別
├─ 需驗證性別
├─ 需驗證出生日期
├─ 需驗證國籍
├─ 需驗證地址對應關係
└─ 強制壓印浮水印
```

---

## 資料處理

### 1. 資料準備階段

#### 半寬字符轉換
```csharp
// 位置: MapHelper.ToHalfWidthRequest()
// 將全寬字符轉換為半寬
├─ 半寬數字: １２３ → 123
├─ 半寬英文: ＡＢＣ → ABC
└─ 半寬空格: 　 → (空格)
```

#### 參數查詢
```
從快取/數據庫查詢:
├─ SetUp_ApplyCardType: 所有有效的申請卡別代碼
├─ SetUp_MainIncomeAndFund: 主要收入與基金代碼
├─ SetUp_JobCode: 職業代碼
├─ SetUp_BusinessType: 行業類別
├─ SetUp_EducationLevel: 教育程度
├─ SetUp_MaritalStatus: 婚姻狀況
└─ SysParamManage_SysParam.AMLProfessionCode_Version: AML 職業別版本
```

#### 申請書與附件查詢
```
查詢來源: eCard_file 數據庫
├─ 查詢申請書 PDF: ApplyFile 表
├─ 如無申請書 → 回覆代碼 0008
├─ 查詢附件檔案 (如有提供)
├─ 如無附件 → 回覆代碼 0014
└─ 檔案內容取得二進位數據
```

#### 浮水印處理
```
條件:
├─ 非卡友進件 (無 A02 徵信碼): 必須壓印浮水印
├─ 國旅卡進件: 必須壓印浮水印
└─ 原卡友 (A02): 不壓印浮水印

處理方式:
├─ PDF 格式: IWatermarkHelper.PdfWatermarkAndGetBytes()
│  └─ 浮水印文本: 配置中 "WatermarkText"
├─ 圖片格式 (JPG/PNG): IWatermarkHelper.ImageWatermarkAndGetBytes()
│  └─ 斜體浮水印效果
└─ 異常 → 回覆代碼 0009
```

### 2. 資料映射

#### CaseContext 映射
```csharp
// 位置: MapHelper.MapToCaseContext()
CaseContext {
    ApplyNo = req.ApplyNo,
    Source = MapSource(req.Source),  // Ecard → Enum
    UserType = MapCardOwner(req.CardOwner),  // 1→正卡, 2→附卡等
    CaseType = IsCITSCard ? 國旅卡 : 一般件,
    ApplyDate = DateTime.Now,
    IsHistory = "N",
    ApplyCardTypes = ParseCardTypes(req.ApplyCardType),  // 分割字符串
    CreditCheckCode = req.CreditCheckCode,
    // ... 其他映射
}
```

#### 實體新增

**Reviewer_ApplyCreditCardInfoMain** (申請主檔)
```csharp
new Reviewer_ApplyCreditCardInfoMain {
    ApplyNo = req.ApplyNo,
    ID = req.ID,
    CHName = req.CHName,
    Birthday = ConvertBirthdayFormat(req.Birthday),  // 民國 → 西元
    IDIssueDate = ConvertIDIssueDateFormat(req.IDIssueDate),
    Sex = MapSex(req.Sex),
    IDType = MapIDType(req.IDType),
    Source = MapSource(req.Source),
    UserType = MapCardOwner(req.CardOwner),
    CaseType = IsCITSCard ? "國旅卡" : "一般件",
    EMail = req.EMail,
    Mobile = req.Mobile,
    CurrentMonthIncome = req.CurrentMonthIncome,

    // 地址設置
    Reg_Zip = req.Reg_Zip,
    Reg_City = req.Reg_City,
    Home_Zip = req.Home_Zip,
    Home_City = req.Home_City,

    // 系統欄位
    ApplyDate = DateTime.Now,
    IsHistory = "N",
    AMLProfessionCode_Version = systemVersion,
    LastUpdateUserId = "SYSTEM",
    LastUpdateTime = DateTime.Now
}
```

**Reviewer_ApplyCreditCardInfoHandle** (進件處理檔)
```csharp
new Reviewer_ApplyCreditCardInfoHandle {
    ApplyNo = req.ApplyNo,
    CardOwner = MapCardOwner(req.CardOwner),
    ApplyCardType = req.ApplyCardType.First(),  // 取第一個卡別
    CardStatus = MapInitialCardStatus(req),  // 根據進件來源和卡友狀態決定
    CardStep = "初始進件",
    Source = MapSource(req.Source),
    CreatedTime = DateTime.Now,
    LastUpdateTime = DateTime.Now
}
```

**Reviewer_ApplyCreditCardInfoProcess** (進件歷程)
```csharp
new Reviewer_ApplyCreditCardInfoProcess {
    ApplyNo = req.ApplyNo,
    Process = "進件初始",  // 根據進件來源轉換狀態字串
    ProcessUserId = "SYSTEM",
    StartTime = DateTime.Now,
    EndTime = DateTime.Now,
    Notes = "初始進件"
}
```

**Reviewer_ApplyCreditCardInfoFile** (申請書與附件歷程)
```csharp
// 申請書
new Reviewer_ApplyCreditCardInfoFile {
    ApplyNo = req.ApplyNo,
    FileId = GenerateGuid(),
    Page = 0,
    FileName = Path.GetFileName(req.CardAppId),
    FileType = "申請書",
    FileSize = applicationPdfBytes.Length,
    IsDelete = "N",
    AddTime = DateTime.Now,
    AddUserId = "SYSTEM",
    Note = "新進件申請書",
    DBName = "ScoreSharp_File"
}

// 附件
foreach(var attachment in attachments) {
    new Reviewer_ApplyCreditCardInfoFile {
        ApplyNo = req.ApplyNo,
        FileId = GenerateGuid(),
        Page = pageNumber++,
        FileName = attachment.FileName,
        FileType = "附件",
        // ... 其他欄位
    }
}
```

**Reviewer_ApplyFile** (實際檔案內容 - 儲存至 File DB)
```csharp
new Reviewer_ApplyFile {
    FileId = GenerateGuid(),
    ApplyNo = req.ApplyNo,
    FileContent = watermarkedBytes,  // 已壓印浮水印的二進位數據
    FileName = "{ApplyNo}_申請書.pdf",
    FileType = "申請書",
    AddTime = DateTime.Now
}
```

### 3. 資料庫操作

#### 兩階段分散式事務

```csharp
// Phase 1: 準備所有實體
var mainEntities = new List<Reviewer_ApplyCreditCardInfoMain> { /* ... */ };
var handleEntities = new List<Reviewer_ApplyCreditCardInfoHandle> { /* ... */ };
var processEntities = new List<Reviewer_ApplyCreditCardInfoProcess> { /* ... */ };
var fileEntities = new List<Reviewer_ApplyCreditCardInfoFile> { /* ... */ };
var applyFileEntities = new List<Reviewer_ApplyFile> { /* ... */ };

// Phase 2: 執行事務
using var fileTransaction = await scoreSharpFileContext.Database.BeginTransactionAsync();
using var mainTransaction = await scoreSharpContext.Database.BeginTransactionAsync();

try
{
    // 步驟 1: 存檔案至 File DB
    await scoreSharpFileContext.Reviewer_ApplyFile.AddRangeAsync(applyFileEntities);
    await scoreSharpFileContext.SaveChangesAsync();

    // 步驟 2: 新增主檔
    await scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.AddRangeAsync(mainEntities);

    // 步驟 3: 新增處理檔
    await scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.AddRangeAsync(handleEntities);

    // 步驟 4: 新增歷程
    await scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processEntities);

    // 步驟 5: 新增檔案歷程
    await scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.AddRangeAsync(fileEntities);

    // 步驟 6: 其他業務表 (BankTrace, Finance等)
    // ...

    // 步驟 7: 新增代理工作任務
    if (isA02) {
        // A02 原卡友檢查任務
        await scoreSharpContext.ReviewerPedding_WebApplyCardCheckJobForA02.AddAsync(...);
    } else {
        // 非 A02 檢查任務
        await scoreSharpContext.ReviewerPedding_WebApplyCardCheckJobForNotA02.AddAsync(...);
    }

    // 提交事務
    await scoreSharpContext.SaveChangesAsync();
    await fileTransaction.CommitAsync();
    await mainTransaction.CommitAsync();

    return new EcardNewCaseResponse {
        ID = req.ID,
        RESULT = "匯入成功"
    };
}
catch (Exception ex)
{
    logger.LogError("分散式事務失敗: {@Error}", ex.ToString());
    await mainTransaction.RollbackAsync();
    await fileTransaction.RollbackAsync();

    // 記錄錯誤
    await RecordSystemError(...);

    return new EcardNewCaseResponse {
        ID = req.ID,
        RESULT = MapErrorCode(ex)
    };
}
```

---

## 業務流程說明

### 完整進件流程圖

```
請求 EcardNewCase
│
├─ 步驟 1: 半寬字符轉換
│  └─ MapHelper.ToHalfWidthRequest()
│
├─ 步驟 2: 檢查國旅卡類型
│  └─ IFusionCache.GetAsync("CITS_Card_List")
│
├─ 步驟 3: 申請書編號驗證
│  ├─ 長度驗證: Length = 13
│  ├─ 格式驗證: ^\d{8}[A-Z]{1}\d{4}$
│  └─ 重複檢查: Reviewer_ApplyCreditCardInfoMain.ApplyNo
│
├─ 步驟 4: 卡片代碼驗證
│  ├─ 查詢參數表: SetUp_ApplyCardType
│  └─ 驗證每個卡別是否有效
│
├─ 步驟 5: UUID 重複檢查 (如提供)
│  └─ Reviewer_ApplyCreditCardInfoMain.LineBankUUID
│
├─ 步驟 6: 必填欄位檢查
│  ├─ ApplyNo, ID, CardOwner, ApplyCardType
│  ├─ FormCode, CHName, Birthday, Source
│  └─ 任一缺失 → 回覆代碼 0007
│
├─ 步驟 7: 徵信代碼驗證
│  └─ CreditCheckCode 只能 "A02" 或空值
│
├─ 步驟 8: 取得系統參數
│  └─ AMLProfessionCode_Version
│
├─ 步驟 9: 資料異常驗證
│  ├─ 驗證身份別、性別、出生日期
│  ├─ 驗證地址對應關係
│  ├─ 分卡友/非卡友規則驗證
│  └─ Email、Mobile 格式驗證
│
├─ 步驟 10: 查詢申請書和附件
│  └─ eCard_file DB 中查詢:
│     ├─ ApplyFile 表 (申請書 PDF)
│     └─ 附件檔案 (如有提供)
│
├─ 步驟 11: 浮水印處理
│  ├─ 判斷是否需要浮水印:
│  │  ├─ 非卡友 → 需要
│  │  ├─ 國旅卡 → 需要
│  │  └─ A02 原卡友 → 不需要
│  └─ 壓印浮水印:
│     ├─ PDF: PdfWatermark()
│     └─ 圖片: ImageWatermark()
│
├─ 步驟 12: 映射業務實體
│  ├─ CaseContext 映射
│  ├─ Main, Handle, Process, File 實體
│  ├─ BankTrace, FinanceCheckInfo 等
│  └─ 產生待處理任務 (A02/NotA02)
│
└─ 步驟 13: 分散式事務執行
   ├─ Phase 1: File DB 保存檔案
   ├─ Phase 2: Main DB 保存業務數據
   ├─ 成功 → 回覆代碼 0000
   └─ 失敗 → 自動回滾, 回覆代碼 0012
```

### 回覆代碼流程決策樹

```
驗證失敗判斷:
│
├─ ApplyNo.Length ≠ 13
│  └─ → 0001 (申請書編號長度不符)
│
├─ ApplyNo 重複或格式錯誤
│  └─ → 0003 (申請書編號重複進件或申請書編號不對)
│
├─ 卡片代碼無法對應參數表
│  └─ → 0004 (無法對應卡片代碼)
│
├─ 字段值不在預定義値 (性別、身份類別等)
│  └─ → 0005 (資料異常非定義值)
│
├─ 字段長度超過 SQL 欄位長度 (2628 錯誤)
│  └─ → 0006 (資料異常資料長度過長)
│
├─ 核心必填欄位缺失 (8 個欄位)
│  └─ → 0007 (必要欄位不能為空值)
│
├─ 申請書 PDF 為 null
│  └─ → 0008 (申請書異常)
│
├─ 浮水印處理失敗
│  └─ → 0009 (附件異常)
│
├─ LineBankUUID 已存在
│  └─ → 0010 (UUID 重複)
│
├─ 查詢 eCard_file DB 異常
│  └─ → 0013 (ECARD_FILE_DB 錯誤)
│
├─ eCard_file DB 中查無申請書
│  └─ → 0014 (查無申請書附件檔案)
│
└─ 未捕獲的異常或事務失敗
   └─ → 0012 (其他異常訊息)
```

---

## 關鍵業務規則

### 1. 進件來源和卡友類型組合

| 進件來源 | 卡主類別 | 初始卡片狀態 | 必要驗證 |
|---------|--------|-----------|---------|
| Ecard | 正卡 | Ecard_新進件 | 詳細驗證 |
| Ecard | 附卡 | Ecard_新進件 | 詳細驗證 |
| APP | 正卡 | APP_新進件 | 詳細驗證 |
| APP | 附卡 | APP_新進件 | 詳細驗證 |

### 2. 徵信代碼對應規則

| 徵信代碼 | 進件類型 | 驗證複雜度 | 浮水印 |
|---------|---------|----------|--------|
| "A02" | 原卡友 | 低 (簡化規則) | 不需要 |
| 空值 | 非卡友 | 高 (詳細規則) | 需要 |
| 其他 | 無效 | - | 回覆代碼 0005 |

### 3. 浮水印策略

```
浮水印壓印條件:
├─ 非卡友進件 (CreditCheckCode = 空值): YES
├─ 國旅卡進件 (ApplyCardType 包含 CITS): YES
├─ A02 原卡友進件: NO
└─ 浮水印文本: 取自配置 "WatermarkText"
```

### 4. 民國年日期轉換

```
輸入格式: YYYMMDD (民國年)
轉換規則:
├─ YYY: 民國年 (001~999)
├─ MM: 月份 (01~12)
├─ DD: 日期 (01~31)

轉換邏輯:
├─ 西元年 = 民國年 + 1911
├─ 若民國年 < 100: 西元年 = 民國年 + 2011
├─ 若民國年 >= 100: 西元年 = 民國年 + 1911
└─ 存儲為: YYYY-MM-DD 格式
```

### 5. 申請卡別多選處理

```
輸入: "02/05/08" (多個卡別用 / 分隔)

處理:
├─ 分割字符串: ["02", "05", "08"]
├─ 逐一驗證每個卡別是否在參數表中
├─ Main 表: 存儲所有卡別 (以逗號或原格式)
├─ Handle 表: 初始取第一個卡別
└─ 在後續工作流中可能複製多條 Handle 記錄
```

---

## 涉及的資料表

### 新增表 (INSERT)

| 資料庫 | 表名 | 筆數 | 說明 |
|-------|------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoMain | 1 | 申請主檔 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoHandle | 1~N | 進件處理檔 (可複數卡別) |
| ScoreSharp | Reviewer_ApplyCreditCardInfoProcess | 1~N | 進件歷程 |
| ScoreSharp | Reviewer_ApplyCreditCardInfoFile | N | 檔案歷程 |
| ScoreSharp | Reviewer_BankTrace | 1~N | 銀行迹象 |
| ScoreSharp | Reviewer_InternalCommunicate | 1~N | 內部溝通 |
| ScoreSharp | Reviewer_FinanceCheckInfo | 1 | 財務檢查 |
| ScoreSharp | Reviewer_OutsideBankInfo | 1~N | 外部銀行信息 |
| ScoreSharp | Reviewer_ApplyNote | 1~N | 申請備註 |
| ScoreSharp | ReviewerPedding_WebApplyCardCheckJobForA02 | 1 | A02 檢查任務 (如是原卡友) |
| ScoreSharp | ReviewerPedding_WebApplyCardCheckJobForNotA02 | 1 | 非 A02 檢查任務 (如是非卡友) |
| ScoreSharp_File | Reviewer_ApplyFile | N | 實際檔案內容 |

### 查詢表 (SELECT)

| 資料庫 | 表名 | 用途 |
|-------|------|------|
| ScoreSharp | Reviewer_ApplyCreditCardInfoMain | 檢查 ApplyNo 重複性 |
| ScoreSharp | SetUp_ApplyCardType | 查詢有效的申請卡別 |
| ScoreSharp | SetUp_MainIncomeAndFund | 查詢主要收入代碼 |
| ScoreSharp | SetUp_JobCode | 查詢職業代碼參數 |
| ScoreSharp | SysParamManage_SysParam | 查詢 AML 版本和系統參數 |
| eCard_file | ApplyFile | 查詢申請書和附件 |

---

## 異常處理

### 常見異常及處理方式

| 異常類型 | 原因 | 回覆代碼 | 處理方式 |
|---------|------|--------|--------|
| SqlException (2628) | 欄位數據截斷 | 0006 | 記錄錯誤日誌, 通知調整數據 |
| FormatException | 日期/數字格式錯誤 | 0005 | 驗證失敗, 要求重新提交 |
| InvalidOperationException | 查詢無結果 | 0014 | 檔案查詢失敗, 檢查 eCard_file DB |
| WatermarkException | 浮水印壓印失敗 | 0009 | 記錄錯誤, 聯繫技術支持 |
| DbUpdateException | 事務執行失敗 | 0012 | 回滾事務, 記錄詳細錯誤 |

### 錯誤日誌記錄

所有錯誤會記錄至 `System_ErrorLog` 表:
```csharp
new System_ErrorLog {
    ErrorLogId = GenerateGuid(),
    ErrorSource = "EcardNewCase",
    ErrorType = ErrorTypeConst.新進件資料驗證失敗,
    ErrorMessage = errorMessage,
    RequestData = JsonHelper.Serialize(request),
    ResponseData = JsonHelper.Serialize(response),
    ErrorStackTrace = ex.StackTrace,
    CreatedTime = DateTime.Now
}
```

---

## 性能考慮

### 快取策略
- **CITS 卡列表**: 使用 IFusionCache 快取 (默認 24 小時)
- **參數表數據**: 使用快取, 減少數據庫查詢
- **系統參數**: 啟動時加載, 定期更新

### 數據庫操作優化
- 使用分散式事務時確保兩個數據庫連接同時提交
- 批量新增操作使用 `AddRangeAsync` 以減少往返
- 避免在迴圈中進行數據庫查詢

---

## TODO 項目

| 項目 | 狀態 | 備註 |
|------|------|------|
| 浮水印加工 | ⚠️ 暫定 | 浮水印文本和樣式待業務確認 |
| 檔案命名規則 | ⚠️ 暫定 | 目前為 `{ApplyNo}_{原檔名}`, 待確認 |
| A02 卡友身份驗證 | ✅ 已實作 | 支持簡化驗證規則 |
| 國旅卡特殊處理 | ✅ 已實作 | 支持 CITS 卡特殊驗證 |

