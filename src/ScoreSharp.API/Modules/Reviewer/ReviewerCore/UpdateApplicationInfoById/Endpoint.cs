using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateApplicationInfoById;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        ///  更改申請書資料 ById
        /// </summary>
        /// <param name="applyNo">PK</param>
        /// <param name="request"></param>
        /// <response code="400">
        ///
        ///</response>
        /// <remarks>
        ///
        /// <para>🔑 **關鍵欄位**</para>
        ///
        /// 從**取得單筆申請信用卡資料** API 中取得的資料，並根據以下欄位進行更新：
        /// - 【來源 Source】：
        ///     用來區分網路件、紙本件。
        /// - 【是否為原持卡人 IsOriginalCardholder】：
        ///     用來區分卡友、非卡友。
        /// - 【姓名檢核結果 NameChecked】
        /// - 【AML職業別_版本 AMLProfessionCode_Version】
        ///
        ///
        /// ✅ 欄位驗證說明：
        ///
        /// <para>📌【個人基本資料】</para>
        ///
        /// 1.  【申請書編號 ApplyNo】
        ///     - 必填。
        ///     - 長度限制：14。
        ///     - 格式：只能輸入英數等。
        ///
        /// 2. 【中文姓名 CHName】
        ///    - 必填。
        ///    - 長度限制：25。
        ///    - 轉半形。
        ///
        /// 3. 【英文姓名 ENName】
        ///    - 必填。
        ///    - 長度限制：100 。
        ///    - 轉半形。
        ///
        /// 4. 【身份證字號 ID】
        ///    - 必填。
        ///    - 長度限制：10。
        ///    - 轉半形。
        ///    - 符合下列格式之一：
        ///         - ① 台灣國民身分證號（現行）
        ///             - 格式：1 碼英文字母 + 9 碼數字（共 10 碼）
        ///             - 第 2 碼數字為性別：1（男）、2（女）
        ///         - ② 舊制外籍人士統一證號
        ///             - 格式：2 碼英文字母 + 8 碼數字（共 10 碼）
        ///         - ③ 新制外籍人士統一證號（2021 年起）
        ///             - 格式：1 碼英文字母 + 第 2 碼為 8 或 9 + 8 碼數字（共 10 碼）
        ///
        /// 5. 【性別 Sex】
        ///     - 必填。
        ///
        /// 6. 【生日 BirthDay】
        ///     - 必填。
        ///     - 長度限制：7。
        ///     - 轉半形。
        ///     - 格式：民國格式 `YYYMMDD`。
        ///
        /// 7. 【是否學生 IsStudent】
        ///     - 必填。
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///
        /// 8. 【國籍 CitizenshipCode】
        ///     - 必填。
        ///     - 長度限制：2。
        ///
        /// 9. 【出生地國籍 BirthCitizenshipCode】
        ///     - 必填。
        ///     - 當選擇「其他（代碼為 2）」時，需同時填寫「出生地國籍_其他」。
        ///
        /// 10. 【出生地國籍_其他 BirthCitizenshipCodeOther】
        ///     - 當「出生地國籍」為「其他（代碼為 2）」時必填。
        ///     - 長度限制：16。
        ///
        /// 11. 【身分證發證地點 IDCardRenewalLocationCode】
        ///     - 長度限制：8。
        ///     - 當【國籍 CitizenshipCode】為 TW（台灣）且為【非卡友】時必填。
        ///
        /// 12. 【身分證發證日期 IDIssueDate】
        ///     - 長度限制：7。
        ///     - 格式：民國格式 `YYYMMDD`。
        ///     - 轉半形。
        ///     - 當【國籍 CitizenshipCode】為 TW（台灣）且為【非卡友】時必填。
        ///
        /// 13. 【身分證請領狀態 IDTakeStatus】
        ///     - 當【國籍 CitizenshipCode】為 TW（台灣）且為【非卡友】時必填。
        ///
        /// 14. 【FATCA身份 IsFATCAIdentity】
        ///     - 必填。
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ 或 null。
        ///     - 當【出生地國籍_其他 BirthCitizenshipCodeOther】為 US（美國）時必填，預設值為 Y，也可選 N。
        ///     - 當【出生地國籍_其他 BirthCitizenshipCodeOther】不為 US 時，應為 null。
        ///
        /// 15. 【社會安全號碼 SocialSecurityCode】
        ///     - 長度限制：30。
        ///     - 轉半形。
        ///     - 當【是否為 FATCA 身份 IsFATCAIdentity】為 Y 時必填。
        ///
        /// 16. 【居留證發證日期 ResidencePermitIssueDate】
        ///     - 長度限制：8。
        ///     - 格式：西元格式 `YYYYMMDD`。
        ///     - 轉半形。
        ///     - 當【國籍 CitizenshipCode】不為 TW（台灣）時必填。
        ///
        /// 17. 【居留證期限 ResidencePermitDeadline】
        ///     - 長度限制：8。
        ///     - 格式：西元格式 `YYYYMM`。
        ///     - 轉半形。
        ///     - 當【國籍 CitizenshipCode】不為 TW（台灣）時必填。
        ///
        /// 18. 【居留證背面號碼 ResidencePermitBackendNum】
        ///     - 長度限制：10。
        ///     - 轉半形。
        ///     - 格式：前兩碼大寫英文 + 8 碼數字，範例ＹＺ80000001。
        ///     - 當【國籍 CitizenshipCode】不為 TW（台灣）時必填。
        ///
        /// 19. 【是否為永久居留證 IsForeverResidencePermit】
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///     - 當選擇 N 時，則需填寫「外籍人士指定效期」。
        ///     - 當【國籍 CitizenshipCode】不為 TW（台灣）時必填。
        ///
        /// 20. 【外籍人士指定效期 ExpatValidityPeriod】
        ///     - 長度限制：8。
        ///     - 轉半形。
        ///     - 格式：西元格式 `YYYYMM`。
        ///     - 當「是否為永久居留證」選擇 N 時必填。
        ///.
        /// 21. 【護照號碼 PassportNo】
        ///     - 長度限制：20。
        ///     - 轉半形。
        ///
        /// 22. 【護照日期 PassportDate】
        ///     - 長度限制：8。
        ///     - 轉半形。
        ///     - 格式：西元格式 `YYYYMMDD`。
        ///
        /// 23. 【舊照查驗 OldCertificateVerified】
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///
        /// 24. 【畢業國小 GraduatedElementarySchool】
        ///     - 長度限制：15。
        ///     - 轉半形。
        ///
        /// 25. 【是否申請數位卡 IsApplyDigtalCard】
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///
        /// 26. 【是否轉換卡別 IsConvertCard】
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///
        /// 27. 【行動電話 Mobile】
        ///     - 必填。
        ///     - 長度限制：10。
        ///     - 轉半形。
        ///     - 格式：09 開頭、共 10 碼數字。
        ///
        /// 28. 【電子信箱 Email】
        ///     - 長度限制：100。
        ///     - 格式：需符合 Email 標準格式（例如：example@domain.com）。
        ///
        /// 29. 【帳單形式 BillType】
        ///     - 必填。
        ///
        /// 30. 【戶籍電話 HouseRegPhone】
        ///     - 長度限制：18。
        ///     - 轉半形。
        ///     - 格式：3碼-七八碼(範例020-28572463)
        ///
        /// 31. 【居住電話 LivePhone】
        ///     - 長度限制：18。
        ///     - 轉半形。
        ///     - 格式：3碼-七八碼(範例020-28572463)
        ///
        /// 32. 【安麗編號 AnliNo】
        ///     - 長度限制：20。
        ///     - 轉半形。
        ///
        ///<para>
        ///
        ///     🔔 重要提醒 - 【姓名檢核邏輯說明】：
        ///
        ///     ✔ 當【是否為原持卡人 IsOriginalCardholder】= Y 時：
        ///     。 不需進行姓名檢核，可略過相關欄位。
        ///
        ///     ✘ 當【是否為原持卡人 IsOriginalCardholder】= N 時：
        ///     。 需進行姓名檢核，依照下列規則進行：
        ///
        ///         。 若【姓名檢核結果 NameChecked】= Y：
        ///             。 必填【敦陽系統黑名單是否相符 IsDunyangBlackList】
        ///             。 並根據下列情況填寫【姓名檢核理由碼 NameCheckedReasonCodes】：
        ///                 。 若【敦陽系統黑名單是否相符 IsDunyangBlackList】= Y：
        ///                     。 必填 NameCheckedReasonCodes
        ///                     。 不得勾選「5：無」
        ///                 。 若【敦陽系統黑名單是否相符 IsDunyangBlackList】= N：
        ///                     。 NameCheckedReasonCodes 僅能勾選「5：無」
        ///
        ///         。 若【姓名檢核結果 NameChecked】= N：
        ///             。 不需填寫 【敦陽系統黑名單是否相符 IsDunyangBlackList】 與 【姓名檢核理由碼 NameCheckedReasonCodes】
        /// </para>
        ///
        /// 33. 【敦陽系統黑名單是否相符 IsDunyangBlackList】
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///     - 當正卡人【姓名檢核結果 NameChecked】= Y 時，此欄位必填。
        ///     - `當此欄位為 "Y" 時，【姓名檢核理由碼 NameCheckedReasonCodes】需必填且不得勾選無：5；反之，只能勾選無：5`。
        ///
        /// 34. 【姓名檢核理由碼 NameCheckedReasonCodes】
        ///     - 長度限制：20。
        ///     - 格式：逗號分隔多選值（例如：`1,6`）
        ///     - 驗證規則：
        ///         - 勾選 6：RCA 時，**必須同時勾選** 1：PEP，例如 `1,6`
        ///         - RCA (6) **不可與除 1 以外的項目並存**（如 `1,3,6` 無效）
        ///         - 勾選 7~10 時，**必須同時勾選**  1：PEP
        ///         - 勾選 5：無 時，**不可與其他任一項目混選**（需單選）
        ///
        ///     - 提示建議：
        ///         - 勾選 RCA 未勾 PEP → 顯示「勾選 RCA 時，需同時勾選 PEP」
        ///
        /// 35. 【是否為現任 PEP ISRCAForCurrentPEP】
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///     - 預設為 N。
        ///     - 當【姓名檢核理由碼 NameCheckedReasonCodes】中包含 PEP（1）+卸任 PEP（10）時必填。
        ///
        /// 36. 【現任職位是否與 PEP 職務相關 IsCurrentPositionRelatedPEPPosition】
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///     - 當【姓名檢核理由碼 NameCheckedReasonCodes】中包含 PEP（1）+卸任 PEP（10）時必填。
        ///
        /// 37. 【擔任 PEP 範圍 PEPRange】
        ///     - 當【姓名檢核理由碼 NameCheckedReasonCodes】中包含 PEP（1）+卸任 PEP（10）時必填。
        ///
        /// 38. 【是否已辭去 PEP 職位 ResignPEPKind】
        ///     - 當【姓名檢核理由碼 NameCheckedReasonCodes】中包含 PEP（1）+卸任 PEP（10）時必填。
        ///
        /// 39. 【郵遞區號(戶籍) Reg_ZipCode】
        ///     - 必填。
        ///     - 轉半形。
        ///
        /// 40. 【戶籍地址_縣市 Reg_City】
        ///     - 當為【非卡友】時必填。
        ///
        /// 41. 【戶籍地址_鄉鎮市區 Reg_District】
        ///     - 當為【非卡友】時必填。
        ///
        /// 42. 【戶籍地址_路 Reg_Road】
        ///     - 當為【非卡友】時必填。
        ///
        /// 43. 【戶藉地址_巷 Reg_Lane】
        ///     - 轉半形。
        ///
        /// 44. 【戶藉地址_弄 Reg_Alley】
        ///     - 轉半形。
        ///
        /// 45. 【戶藉地址_號 Reg_Number】
        ///     - 當為【非卡友】時必填。
        ///     - 轉半形。
        ///
        /// 46. 【戶藉地址_之號 Reg_SubNumber】
        ///     - 轉半形。
        ///
        /// 47. 【戶藉地址_樓 Reg_Floor】
        ///     - 轉半形。
        ///
        /// 48. 【戶藉地址_其他 Reg_Other】
        ///     - 轉半形。
        ///
        /// 49. 【戶藉地址_完整地址 Reg_FullAddr】
        ///     - 當為【卡友】時必填。
        ///     - 長度限制：100。
        ///     - 轉半形。
        ///     - 正則表達式：^(臺北市|新北市|桃園市|臺中市|臺南市|高雄市|新竹市|嘉義市|基隆市|宜蘭縣|新竹縣|苗栗縣|彰化縣|南投縣|雲林縣|嘉義縣|屏東縣|臺東縣|花蓮縣|澎湖縣|金門縣|連江縣|釣魚臺|南海島)([^市縣]+?[區鄉鎮市]).*$
        ///     - 範例：臺北市大安區忠孝東路12巷5弄8號
        ///
        /// 50. 【寄卡地址類型 SendCardAddressType】
        ///     - 若有填寫，將檢查【寄卡地址 SendCard_ 】是否與所選地址類型一致（如選擇同戶籍地址，則需相同）。
        ///
        /// 51. 【郵遞區號(寄卡) SendCard_ZipCode】
        ///     - 必填。
        ///     - 轉半形。
        ///
        /// 52. 【寄卡地址_縣市 SendCard_City】
        ///     - 當為【非卡友】時必填。
        ///
        /// 53. 【寄卡地址_鄉鎮市區 SendCard_District】
        ///     - 當為【非卡友】時必填。
        ///
        /// 54. 【寄卡地址_路 SendCard_Road】
        ///     - 當為【非卡友】時必填。
        ///
        /// 55. 【寄卡地址_巷 SendCard_Lane】
        ///     - 轉半形。
        ///
        /// 56. 【寄卡地址_弄 SendCard_Alley】
        ///     - 轉半形。
        ///
        /// 57. 【寄卡地址_號 SendCard_Number】
        ///     - 當為【非卡友】時必填。
        ///     - 轉半形。
        ///
        /// 58. 【寄卡地址_之號 SendCard_SubNumber】
        ///     - 轉半形。
        ///
        /// 59. 【寄卡地址_樓 SendCard_Floor】
        ///     - 轉半形。
        ///
        /// 60. 【寄卡地址_其他 SendCard_Other】
        ///     - 轉半形。
        ///
        /// 61. 【寄卡地址_完整地址 SendCard_FullAddr】
        ///     - 當為【卡友】時必填。
        ///     - 長度限制：100。
        ///     - 轉半形。
        ///     - 正則表達式：^(臺北市|新北市|桃園市|臺中市|臺南市|高雄市|新竹市|嘉義市|基隆市|宜蘭縣|新竹縣|苗栗縣|彰化縣|南投縣|雲林縣|嘉義縣|屏東縣|臺東縣|花蓮縣|澎湖縣|金門縣|連江縣|釣魚臺|南海島)([^市縣]+?[區鄉鎮市]).*$
        ///     - 範例：臺北市大安區忠孝東路12巷5弄8號
        ///
        /// 62. 【寄卡地址類型 SendCardAddressType】
        ///     - 若有填寫，將檢查【寄卡地址 SendCard_ 】是否與所選地址類型一致（如選擇同戶籍地址，則需相同）。
        ///
        /// 63. 【郵遞區號(帳單) Bill_ZipCode】
        ///     - 必填。
        ///     - 轉半形。
        ///
        /// 64. 【帳單地址_縣市 Bill_City】
        ///     - 當為【非卡友】時必填。
        ///
        /// 65. 【帳單地址_鄉鎮市區 Bill_District】
        ///     - 當為【非卡友】時必填。
        ///
        /// 66. 【帳單地址_路 Bill_Road】
        ///     - 當為【非卡友】時必填。
        ///
        /// 67. 【帳單地址_巷 Bill_Lane】
        ///     - 轉半形。
        ///
        /// 68. 【帳單地址_弄 Bill_Alley】
        ///     - 轉半形。
        ///
        /// 69. 【帳單地址_號 Bill_Number】
        ///     - 當為【非卡友】時必填。
        ///     - 轉半形。
        ///
        /// 70. 【帳單地址_之號 Bill_SubNumber】
        ///     - 轉半形。
        ///
        /// 71. 【帳單地址_樓 Bill_Floor】
        ///     - 轉半形。
        ///
        /// 72. 【帳單地址_其他 Bill_Other】
        ///     - 轉半形。
        ///
        /// 73. 【帳單地址_完整地址 Bill_FullAddr】
        ///     - 當為【卡友】時必填。
        ///     - 長度限制：100。
        ///     - 轉半形。
        ///     - 正則表達式：^(臺北市|新北市|桃園市|臺中市|臺南市|高雄市|新竹市|嘉義市|基隆市|宜蘭縣|新竹縣|苗栗縣|彰化縣|南投縣|雲林縣|嘉義縣|屏東縣|臺東縣|花蓮縣|澎湖縣|金門縣|連江縣|釣魚臺|南海島)([^市縣]+?[區鄉鎮市]).*$
        ///     - 範例：臺北市大安區忠孝東路12巷5弄8號
        ///
        /// 74. 【居住地址類型 LiveAddressType】
        ///     - 若有填寫，將檢查【居住地址 Live_ 】是否與所選地址類型一致（如選擇同戶籍地址，則需相同）。
        ///
        /// 75. 【郵遞區號(居住) Live_ZipCode】
        ///     - 轉半形。
        ///
        /// 76. 【居住地址_巷 Bill_Lane】
        ///     - 轉半形。
        ///
        /// 77. 【居住地址_弄 Live_Alley】
        ///     - 轉半形。
        ///
        /// 78. 【居住地址_號 Live_Number】
        ///     - 轉半形。
        ///
        /// 79. 【居住地址_之號 Live_SubNumber】
        ///     - 轉半形。
        ///
        /// 80. 【居住地址_樓 Live_Floor】
        ///     - 轉半形。
        ///
        /// 81. 【居住地址_其他 Live_Other】
        ///     - 轉半形。
        ///
        /// 82. 【居住地址_完整地址 Live_FullAddr】
        ///     - 長度限制：100。
        ///     - 轉半形。
        ///     - 正則表達式：^(臺北市|新北市|桃園市|臺中市|臺南市|高雄市|新竹市|嘉義市|基隆市|宜蘭縣|新竹縣|苗栗縣|彰化縣|南投縣|雲林縣|嘉義縣|屏東縣|臺東縣|花蓮縣|澎湖縣|金門縣|連江縣|釣魚臺|南海島)([^市縣]+?[區鄉鎮市]).*$
        ///     - 範例：臺北市大安區忠孝東路12巷5弄8號
        ///
        /// 83. 【子女人數 ChildrenCount】
        ///     - 數字。
        ///
        /// <para>📌【職業資料】</para>
        ///
        /// 1. 【公司名稱 CompName】
        ///     - 長度限制：25。
        ///     - 轉半形。
        ///
        /// 2. 【公司統一編號 CompID】
        ///     - 長度限制：8。
        ///     - 轉半形。
        ///     - 格式：只能輸入數字。
        ///
        /// 3. 【公司職稱 CompJobTitle】
        ///     - 長度限制：30。
        ///     - 轉半形。
        ///
        /// 4. 【公司電話 CompPhone】
        ///     - 長度限制：21。
        ///     - 轉半形。
        ///     - 格式：3碼-七八碼#5碼分機(範例020-28572463#55555)
        ///
        /// 5. 【AML職業別 AMLProfessionCode】
        ///     - 必填。
        ///
        ///     - **資料來源**：
        ///         - 選項來源為「取得徵審相關所有的下拉選單」API 中的【AML職業別】。
        ///
        /// 6. 【職業類別其他 AMLProfessionOther】
        ///     - 長度限制：50。
        ///     - 轉半形。
        ///     - 驗證規則：
        ///         - 根據【AML職業別_版本 AMLProfessionCode_Version】，從 `appsettings.json` 中讀取對應設定：`ValidationSetting:AMLProfessionOther_{AML職業別_版本}`
        ///         - 若查無該版本設定值 → 拋出錯誤訊息：「查無AML職業別其他的代碼，請確認設定檔是否有對應版本的ValidationSetting:AMLProfessionOther_{版本}」。
        ///
        /// 8. 【AML職級別 AMLJobLevelCode】
        ///     - 必填。
        ///
        /// 7. 【所得及資金來源 MainIncomeAndFundCodes】
        ///     - 必填。
        ///     - 長度限制：50。
        ///     - 格式：逗號分隔多選值（例如：`1,6`）。
        ///
        /// 8. 【主要收入_其他 MainIncomeAndFundOther】
        ///     - 長度限制：25。
        ///     - 轉半形。
        ///     - 當【所得及資金來源 MainIncomeAndFundCodes】中包含「其他」的選項時必填。
        ///     - 驗證規則：
        ///         - 從 `appsettings.json` 中讀取對應設定：`ValidationSetting:MainIncomeAndFundOther`
        ///
        /// 9. 【郵遞區號(公司) Comp_ZipCode】
        ///     - 轉半形。
        ///     - 長度限制：25。
        ///
        /// 10. 【公司地址_巷 Comp_Lane】
        ///     - 長度限制：25。
        ///     - 轉半形。
        ///
        /// 11. 【公司地址_弄 Comp_Alley】
        ///     - 長度限制：25。
        ///     - 轉半形。
        ///
        /// 12. 【公司地址_號 Comp_Number】
        ///     - 長度限制：25。
        ///     - 轉半形。
        ///
        /// 13. 【公司地址_之號 Comp_SubNumber】
        ///     - 長度限制：25。
        ///     - 轉半形。
        ///
        /// 14. 【公司地址_樓 Comp_Floor】
        ///     - 長度限制：25。
        ///     - 轉半形。
        ///
        /// 15. 【公司地址_完整地址 Comp_FullAddr】
        ///     - 長度限制：100。
        ///     - 轉半形。
        ///     - 正則表達式：^(臺北市|新北市|桃園市|臺中市|臺南市|高雄市|新竹市|嘉義市|基隆市|宜蘭縣|新竹縣|苗栗縣|彰化縣|南投縣|雲林縣|嘉義縣|屏東縣|臺東縣|花蓮縣|澎湖縣|金門縣|連江縣|釣魚臺|南海島)([^市縣]+?[區鄉鎮市]).*$
        ///     - 範例：臺北市大安區忠孝東路12巷5弄8號
        ///
        /// 16. 【公司地址_其他 Comp_Other】
        ///     - 長度限制：100。
        ///     - 轉半形。
        ///
        /// 17. 【部門名稱 DepartmentName】
        ///     - 長度限制：25。
        ///
        /// 18. 【到職日期 EmploymentDate】
        ///     - 長度限制：7。
        ///     - 格式：民國格式 `YYYMMDD`。
        ///
        /// <para>📌【學生身份】</para>
        ///
        /// 1.  【是否為學生 IsStudent】
        ///     - 必填。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///     - 若為 Y，以下欄位皆為必填：
        ///         - 【學生就讀學校 StudSchool】
        ///         - 【學生預定畢業日期 StudScheduledGraduationDate】
        ///         - 【家長姓名 ParentName】
        ///         - 【家長電話 ParentPhone】
        ///         - 【學生申請人與本人關係 StudentApplicantRelationship】
        ///         - 【家長居住地址 ParentAddress_】
        ///
        /// 2.  【學生就讀學校 StudSchool】
        ///     - 當 【是否為學生 IsStudent】 = Y 時必填。
        ///     - 長度限制：25。
        ///     - 轉半形。
        ///
        /// 3.  【學生預定畢業日期 StudScheduledGraduationDate】
        ///     - 當 【是否為學生 IsStudent】 = Y 時必填。
        ///     - 長度限制：7。
        ///     - 轉半形。
        ///     - 格式：民國格式 `YYYMMDD`。
        ///
        /// 4.  【家長姓名 ParentName】
        ///     - 當 【是否為學生 IsStudent】 = Y 時必填。
        ///     - 轉半形。
        ///     - 長度限制：25。
        ///
        /// 5. 【家長電話 ParentPhone】
        ///     - 當 【是否為學生 IsStudent】 = Y 時必填。
        ///     - 長度限制：20。
        ///     - 轉半形。
        ///     - 可以行動電話或者家電或公司電話，格式如下：
        ///     - 家電 範例：020-28572463，3碼-七八碼
        ///     - 公司電話 範例：020-28572463#55555，3碼-七八碼#5碼分機
        ///     - 行動電話 範例：0978822811
        ///
        /// 6. 【學生申請人與本人關係 StudentApplicantRelationship】
        ///     - 當 【是否為學生 IsStudent】 = Y 時必填。
        ///
        /// 7. 【家長居住地址類型 ParentLiveAddressType】
        ///    - 若有填寫，將檢查【家長居住地址 ParentLive_ 】是否與所選地址類型一致（如選擇同戶籍地址，則需相同）。
        ///
        /// 8. 【家長居住地址_郵遞區號 ParentLive_ZipCode】
        ///     - 當 【是否為學生 IsStudent】 = Y 時必填。
        ///     - 轉半形。
        ///
        /// 9. 【家長居住地址_縣市 ParentLive_City】
        ///     - 當 【是否為學生 IsStudent】 = Y ，且為【非卡友】時必填。
        ///
        /// 10. 【家長居住地址_鄉鎮市區 ParentLive_District】
        ///     - 當 【是否為學生 IsStudent】 = Y ，且為【非卡友】時必填。
        ///
        /// 11. 【家長居住地址_路 ParentLive_Road】
        ///     - 當 【是否為學生 IsStudent】 = Y ，且為【非卡友】時必填。
        ///
        /// 12. 【家長居住地址_巷 ParentLive_Lane】
        ///     - 轉半形。
        ///
        /// 13. 【家長居住地址_弄 ParentLive_Alley】
        ///     - 轉半形。
        ///
        /// 14. 【家長居住地址_號 ParentLive_Number】
        ///     - 當 【是否為學生 IsStudent】 = Y ，且為【非卡友】時必填。
        ///     - 轉半形。
        ///
        /// 15. 【家長居住地址_之號 ParentLive_SubNumber】
        ///     - 轉半形。
        ///
        /// 16. 【家長居住地址_樓 ParentLive_Floor】
        ///     - 轉半形。
        ///
        /// 17. 【家長居住地址_其他 ParentLive_Other】
        ///     - 轉半形。
        ///
        /// 18. 【家長居住地址_完整地址 ParentLive_FullAddr】
        ///     - 轉半形。
        ///     - 長度限制：100。
        ///     - 當 【是否為學生 IsStudent】 = Y ，且為【卡友】時必填。
        ///     - 正則表達式：^(臺北市|新北市|桃園市|臺中市|臺南市|高雄市|新竹市|嘉義市|基隆市|宜蘭縣|新竹縣|苗栗縣|彰化縣|南投縣|雲林縣|嘉義縣|屏東縣|臺東縣|花蓮縣|澎湖縣|金門縣|連江縣|釣魚臺|南海島)([^市縣]+?[區鄉鎮市]).*$
        ///     - 範例：臺北市大安區忠孝東路12巷5弄8號
        ///
        /// <para>📌【活動資料】</para>
        ///
        /// 1.  【本人是否同意提供資料予聯名認同集團 IsAgreeDataOpen】
        ///     - 必填。
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///
        /// 2. 【是否同意提供資料供本行進行行銷 IsAgreeMarketing】
        ///     - 必填。
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///
        /// 3. 【是否同意悠遊卡自動加值預設開啟 IsAcceptEasyCardDefaultBonus】
        ///     - 必填。
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///
        /// 4.  【是否綁定消費通知 IsPayNoticeBind】
        ///     - 必填。
        ///     - 長度限制：1。
        ///     - 格式：只能輸入 Ｙ 或 Ｎ。
        ///
        /// 5.  【首刷禮代碼 FirstBrushingGiftCode】
        ///     - 長度限制：10。
        ///     - 轉半形。
        ///
        /// 6.  【專案代號 ProjectCode】
        ///     - 必填。
        ///     - 長度限制：20。
        ///     - 轉半形。
        ///
        /// 7. 【電子化約定條款 ElecCodeId】
        ///     - 長度限制：10。
        ///     - 範例：202007
        ///
        /// 8. 【年費收取方式 AnnualFeePaymentType】
        ///     - 長度限制：2。
        ///     - **資料來源**：
        ///         - 選項來源為「取得徵審相關所有的下拉選單」API 中的【年費收取方式】。
        ///
        /// </remarks>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(typeof(修改申請書資料_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改申請書資料_2000_ResEx),
            typeof(修改申請書資料查無此資料_4001_ResEx),
            typeof(修改申請書資料路由與Req比對錯誤_4003_ResEx),
            typeof(修改申請書資料查無郵遞區號_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateApplicationInfoById")]
        public async Task<IResult> UpdateApplicationInfoById([FromRoute] string applyNo, [FromBody] UpdateApplicationInfoByIdRequest request)
        {
            var result = await _mediator.Send(new Command(applyNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateApplicationInfoById
{
    public record Command(string applyNo, UpdateApplicationInfoByIdRequest updateApplicationInfoByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext _context, IJWTProfilerHelper _jwthelper, ILogger<Handler> _logger)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateReq = ToHalfWidthRequest(request.updateApplicationInfoByIdRequest);

            if (updateReq.ApplyNo != request.applyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity_main = await _context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == request.applyNo);

            var entity_handles = await _context.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == request.applyNo).ToListAsync();

            if (entity_main is null || entity_handles.Count == 0)
                return ApiResponseHelper.NotFound<string>(null, request.applyNo);

            // 轉換郵遞區號
            var addressErrorBuilder = new StringBuilder();
            if (
                string.IsNullOrEmpty(updateReq.Reg_ZipCode)
                && !String.IsNullOrEmpty(updateReq.Reg_City)
                && !String.IsNullOrEmpty(updateReq.Reg_District)
                && !String.IsNullOrEmpty(updateReq.Reg_Road)
            )
            {
                updateReq.Reg_ZipCode = await SearchZipCode(
                    updateReq.Reg_City,
                    updateReq.Reg_District,
                    updateReq.Reg_Road,
                    updateReq.Reg_Number,
                    updateReq.Reg_SubNumber,
                    updateReq.Reg_Lane
                );

                if (string.IsNullOrEmpty(updateReq.Reg_ZipCode))
                    addressErrorBuilder.AppendFormat("正卡人戶籍地址、");
            }
            else if (string.IsNullOrEmpty(updateReq.Reg_ZipCode) && !String.IsNullOrEmpty(updateReq.Reg_FullAddr))
            {
                updateReq.Reg_ZipCode = await SearchZipCode(updateReq.Reg_FullAddr);

                if (string.IsNullOrEmpty(updateReq.Reg_ZipCode))
                    addressErrorBuilder.AppendFormat("正卡人戶籍地址、");
            }

            if (
                string.IsNullOrEmpty(updateReq.Live_ZipCode)
                && !String.IsNullOrEmpty(updateReq.Live_City)
                && !String.IsNullOrEmpty(updateReq.Live_District)
                && !String.IsNullOrEmpty(updateReq.Live_Road)
            )
            {
                updateReq.Live_ZipCode = await SearchZipCode(
                    updateReq.Live_City,
                    updateReq.Live_District,
                    updateReq.Live_Road,
                    updateReq.Live_Number,
                    updateReq.Live_SubNumber,
                    updateReq.Live_Lane
                );

                if (string.IsNullOrEmpty(updateReq.Live_ZipCode))
                    addressErrorBuilder.AppendFormat("正卡人居住地址、");
            }
            else if (string.IsNullOrEmpty(updateReq.Live_ZipCode) && !String.IsNullOrEmpty(updateReq.Live_FullAddr))
            {
                updateReq.Live_ZipCode = await SearchZipCode(updateReq.Live_FullAddr);

                if (string.IsNullOrEmpty(updateReq.Live_ZipCode))
                    addressErrorBuilder.AppendFormat("正卡人居住地址、");
            }

            if (
                string.IsNullOrEmpty(updateReq.Comp_ZipCode)
                && !String.IsNullOrEmpty(updateReq.Comp_City)
                && !String.IsNullOrEmpty(updateReq.Comp_District)
                && !String.IsNullOrEmpty(updateReq.Comp_Road)
            )
            {
                updateReq.Comp_ZipCode = await SearchZipCode(
                    updateReq.Comp_City,
                    updateReq.Comp_District,
                    updateReq.Comp_Road,
                    updateReq.Comp_Number,
                    updateReq.Comp_SubNumber,
                    updateReq.Comp_Lane
                );

                if (string.IsNullOrEmpty(updateReq.Comp_ZipCode))
                    addressErrorBuilder.AppendFormat("正卡人公司地址、");
            }
            else if (string.IsNullOrEmpty(updateReq.Comp_ZipCode) && !String.IsNullOrEmpty(updateReq.Comp_FullAddr))
            {
                updateReq.Comp_ZipCode = await SearchZipCode(updateReq.Comp_FullAddr);

                if (string.IsNullOrEmpty(updateReq.Comp_ZipCode))
                    addressErrorBuilder.AppendFormat("正卡人公司地址、");
            }

            if (
                string.IsNullOrEmpty(updateReq.SendCard_ZipCode)
                && !String.IsNullOrEmpty(updateReq.SendCard_City)
                && !String.IsNullOrEmpty(updateReq.SendCard_District)
                && !String.IsNullOrEmpty(updateReq.SendCard_Road)
            )
            {
                updateReq.SendCard_ZipCode = await SearchZipCode(
                    updateReq.SendCard_City,
                    updateReq.SendCard_District,
                    updateReq.SendCard_Road,
                    updateReq.SendCard_Number,
                    updateReq.SendCard_SubNumber,
                    updateReq.SendCard_Lane
                );

                if (string.IsNullOrEmpty(updateReq.SendCard_ZipCode))
                    addressErrorBuilder.AppendFormat("正卡人寄卡地址、");
            }
            else if (string.IsNullOrEmpty(updateReq.SendCard_ZipCode) && !String.IsNullOrEmpty(updateReq.SendCard_FullAddr))
            {
                updateReq.SendCard_ZipCode = await SearchZipCode(updateReq.SendCard_FullAddr);

                if (string.IsNullOrEmpty(updateReq.SendCard_ZipCode))
                    addressErrorBuilder.AppendFormat("正卡人寄卡地址、");
            }

            if (
                string.IsNullOrEmpty(updateReq.Bill_ZipCode)
                && !String.IsNullOrEmpty(updateReq.Bill_City)
                && !String.IsNullOrEmpty(updateReq.Bill_District)
                && !String.IsNullOrEmpty(updateReq.Bill_Road)
            )
            {
                updateReq.Bill_ZipCode = await SearchZipCode(
                    updateReq.Bill_City,
                    updateReq.Bill_District,
                    updateReq.Bill_Road,
                    updateReq.Bill_Number,
                    updateReq.Bill_SubNumber,
                    updateReq.Bill_Lane
                );

                if (string.IsNullOrEmpty(updateReq.Bill_ZipCode))
                    addressErrorBuilder.AppendFormat("正卡人帳單地址、");
            }
            else if (string.IsNullOrEmpty(updateReq.Bill_ZipCode) && !String.IsNullOrEmpty(updateReq.Bill_FullAddr))
            {
                updateReq.Bill_ZipCode = await SearchZipCode(updateReq.Bill_FullAddr);

                if (string.IsNullOrEmpty(updateReq.Bill_ZipCode))
                    addressErrorBuilder.AppendFormat("正卡人帳單地址、");
            }

            if (
                string.IsNullOrEmpty(updateReq.ParentLive_ZipCode)
                && updateReq.IsStudent == "Y"
                && !String.IsNullOrEmpty(updateReq.ParentLive_City)
                && !String.IsNullOrEmpty(updateReq.ParentLive_District)
                && !String.IsNullOrEmpty(updateReq.ParentLive_Road)
            )
            {
                updateReq.ParentLive_ZipCode = await SearchZipCode(
                    updateReq.ParentLive_City,
                    updateReq.ParentLive_District,
                    updateReq.ParentLive_Road,
                    updateReq.ParentLive_Number,
                    updateReq.ParentLive_SubNumber,
                    updateReq.ParentLive_Lane
                );

                if (string.IsNullOrEmpty(updateReq.ParentLive_ZipCode))
                    addressErrorBuilder.AppendFormat("家長居住地址、");
            }
            else if (
                string.IsNullOrEmpty(updateReq.ParentLive_ZipCode)
                && updateReq.IsStudent == "Y"
                && !String.IsNullOrEmpty(updateReq.ParentLive_FullAddr)
            )
            {
                updateReq.ParentLive_ZipCode = await SearchZipCode(updateReq.ParentLive_FullAddr);

                if (string.IsNullOrEmpty(updateReq.ParentLive_ZipCode))
                    addressErrorBuilder.AppendFormat("家長居住地址、");
            }

            // Tips: 一個案件只會有一個徵信代碼，所以所有卡片徵信代碼會一致
            foreach (var entity_handle in entity_handles)
            {
                entity_handle.CreditCheckCode = updateReq.CreditCheckCode;
            }

            // 更新主卡人資料
            entity_main.CHName = updateReq.CHName;
            entity_main.ID = updateReq.ID;
            entity_main.CitizenshipCode = updateReq.CitizenshipCode;
            entity_main.BirthCitizenshipCode = updateReq.BirthCitizenshipCode;
            entity_main.Mobile = updateReq.Mobile;
            entity_main.Sex = updateReq.Sex;
            entity_main.BirthDay = updateReq.BirthDay;
            entity_main.ENName = updateReq.ENName;
            entity_main.IDCardRenewalLocationCode = updateReq.IDCardRenewalLocationCode;
            entity_main.BirthCitizenshipCodeOther = updateReq.BirthCitizenshipCodeOther;
            entity_main.MarriageState = updateReq.MarriageState;
            entity_main.Education = updateReq.Education;
            entity_main.GraduatedElementarySchool = updateReq.GraduatedElementarySchool;
            entity_main.EMail = updateReq.EMail;
            entity_main.HouseRegPhone = updateReq.HouseRegPhone;
            entity_main.LivePhone = updateReq.LivePhone;
            entity_main.LiveOwner = updateReq.LiveOwner;
            entity_main.LiveYear = updateReq.LiveYear;
            entity_main.IDIssueDate = updateReq.IDIssueDate;
            entity_main.ResidencePermitIssueDate = updateReq.ResidencePermitIssueDate;
            entity_main.PassportNo = updateReq.PassportNo;
            entity_main.PassportDate = updateReq.PassportDate;
            entity_main.ExpatValidityPeriod = updateReq.ExpatValidityPeriod;
            entity_main.IsFATCAIdentity = updateReq.IsFATCAIdentity;
            entity_main.SocialSecurityCode = updateReq.SocialSecurityCode;
            // 姓名檢核相關
            entity_main.IsDunyangBlackList = updateReq.IsDunyangBlackList;
            entity_main.NameCheckedReasonCodes = updateReq.NameCheckedReasonCodes;
            entity_main.ISRCAForCurrentPEP = updateReq.ISRCAForCurrentPEP;
            entity_main.ResignPEPKind = updateReq.ResignPEPKind;
            entity_main.PEPRange = updateReq.PEPRange;
            entity_main.IsCurrentPositionRelatedPEPPosition = updateReq.IsCurrentPositionRelatedPEPPosition;

            entity_main.IsStudent = updateReq.IsStudent;
            entity_main.BillType = updateReq.BillType;
            entity_main.ResidencePermitBackendNum = updateReq.ResidencePermitBackendNum;
            entity_main.IsForeverResidencePermit = updateReq.IsForeverResidencePermit;
            entity_main.ResidencePermitDeadline = updateReq.ResidencePermitDeadline;
            entity_main.CompName = updateReq.CompName;
            entity_main.CompID = updateReq.CompID;
            entity_main.CompJobTitle = updateReq.CompJobTitle;
            entity_main.CompSeniority = updateReq.CompSeniority;
            entity_main.CompPhone = updateReq.CompPhone;
            entity_main.AMLProfessionCode = updateReq.AMLProfessionCode;
            entity_main.AMLProfessionOther = updateReq.AMLProfessionOther;
            entity_main.AMLJobLevelCode = updateReq.AMLJobLevelCode;
            entity_main.CompTrade = updateReq.CompTrade;
            entity_main.CompJobLevel = updateReq.CompJobLevel;
            entity_main.CurrentMonthIncome = updateReq.CurrentMonthIncome;
            entity_main.MainIncomeAndFundCodes = updateReq.MainIncomeAndFundCodes;
            entity_main.MainIncomeAndFundOther = updateReq.MainIncomeAndFundOther;
            entity_main.EmploymentDate = updateReq.EmploymentDate;
            entity_main.DepartmentName = updateReq.DepartmentName;
            entity_main.ParentName = updateReq.ParentName;
            entity_main.StudSchool = updateReq.StudSchool;
            entity_main.ParentPhone = updateReq.ParentPhone;
            entity_main.StudScheduledGraduationDate = updateReq.StudScheduledGraduationDate;
            entity_main.StudentApplicantRelationship = updateReq.StudentApplicantRelationship;
            entity_main.IsAgreeDataOpen = updateReq.IsAgreeDataOpen;
            entity_main.IsAgreeMarketing = updateReq.IsAgreeMarketing;
            entity_main.IsPayNoticeBind = updateReq.IsPayNoticeBind;
            entity_main.IsAcceptEasyCardDefaultBonus = updateReq.IsAcceptEasyCardDefaultBonus;
            entity_main.IsApplyDigtalCard = updateReq.IsApplyDigtalCard;

            entity_main.LiveAddressType = updateReq.LiveAddressType;
            entity_main.SendCardAddressType = updateReq.SendCardAddressType;
            entity_main.BillAddressType = updateReq.BillAddressType;
            entity_main.ParentLiveAddressType = updateReq.ParentLiveAddressType;
            entity_main.OldCertificateVerified = updateReq.OldCertificateVerified;
            entity_main.ChildrenCount = updateReq.ChildrenCount;
            entity_main.IDTakeStatus = updateReq.IDTakeStatus;
            entity_main.PromotionUnit = updateReq.PromotionUnit;
            entity_main.PromotionUser = updateReq.PromotionUser;
            entity_main.AcceptType = updateReq.AcceptType;
            entity_main.FirstBrushingGiftCode = updateReq.FirstBrushingGiftCode;
            entity_main.ProjectCode = updateReq.ProjectCode;
            entity_main.LastUpdateUserId = _jwthelper.UserId;
            entity_main.LastUpdateTime = DateTime.Now;
            entity_main.AnliNo = updateReq.AnliNo;
            entity_main.IsConvertCard = updateReq.IsConvertCard;
            entity_main.ElecCodeId = updateReq.ElecCodeId;
            entity_main.AnnualFeePaymentType = updateReq.AnnualFeePaymentType;

            // 戶籍地址
            entity_main.Reg_ZipCode = updateReq.Reg_ZipCode;
            entity_main.Reg_City = updateReq.Reg_City;
            entity_main.Reg_District = updateReq.Reg_District;
            entity_main.Reg_Road = updateReq.Reg_Road;
            entity_main.Reg_Number = updateReq.Reg_Number;
            entity_main.Reg_SubNumber = updateReq.Reg_SubNumber;
            entity_main.Reg_Floor = updateReq.Reg_Floor;
            entity_main.Reg_Lane = updateReq.Reg_Lane;
            entity_main.Reg_Alley = updateReq.Reg_Alley;
            entity_main.Reg_FullAddr = updateReq.Reg_FullAddr;
            entity_main.Reg_Other = updateReq.Reg_Other;
            // 居住地址
            entity_main.Live_ZipCode = updateReq.Live_ZipCode;
            entity_main.Live_City = updateReq.Live_City;
            entity_main.Live_District = updateReq.Live_District;
            entity_main.Live_Road = updateReq.Live_Road;
            entity_main.Live_Number = updateReq.Live_Number;
            entity_main.Live_SubNumber = updateReq.Live_SubNumber;
            entity_main.Live_Floor = updateReq.Live_Floor;
            entity_main.Live_Lane = updateReq.Live_Lane;
            entity_main.Live_Alley = updateReq.Live_Alley;
            entity_main.Live_FullAddr = updateReq.Live_FullAddr;
            entity_main.Live_Other = updateReq.Live_Other;
            // 公司地址
            entity_main.Comp_ZipCode = updateReq.Comp_ZipCode;
            entity_main.Comp_City = updateReq.Comp_City;
            entity_main.Comp_District = updateReq.Comp_District;
            entity_main.Comp_Road = updateReq.Comp_Road;
            entity_main.Comp_Number = updateReq.Comp_Number;
            entity_main.Comp_SubNumber = updateReq.Comp_SubNumber;
            entity_main.Comp_Floor = updateReq.Comp_Floor;
            entity_main.Comp_Lane = updateReq.Comp_Lane;
            entity_main.Comp_Alley = updateReq.Comp_Alley;
            entity_main.Comp_FullAddr = updateReq.Comp_FullAddr;
            entity_main.Comp_Other = updateReq.Comp_Other;
            // 寄卡地址
            entity_main.SendCard_ZipCode = updateReq.SendCard_ZipCode;
            entity_main.SendCard_City = updateReq.SendCard_City;
            entity_main.SendCard_District = updateReq.SendCard_District;
            entity_main.SendCard_Road = updateReq.SendCard_Road;
            entity_main.SendCard_Number = updateReq.SendCard_Number;
            entity_main.SendCard_SubNumber = updateReq.SendCard_SubNumber;
            entity_main.SendCard_Floor = updateReq.SendCard_Floor;
            entity_main.SendCard_Lane = updateReq.SendCard_Lane;
            entity_main.SendCard_Alley = updateReq.SendCard_Alley;
            entity_main.SendCard_FullAddr = updateReq.SendCard_FullAddr;
            entity_main.SendCard_Other = updateReq.SendCard_Other;
            // 帳單地址
            entity_main.Bill_ZipCode = updateReq.Bill_ZipCode;
            entity_main.Bill_City = updateReq.Bill_City;
            entity_main.Bill_District = updateReq.Bill_District;
            entity_main.Bill_Road = updateReq.Bill_Road;
            entity_main.Bill_Number = updateReq.Bill_Number;
            entity_main.Bill_SubNumber = updateReq.Bill_SubNumber;
            entity_main.Bill_Floor = updateReq.Bill_Floor;
            entity_main.Bill_Lane = updateReq.Bill_Lane;
            entity_main.Bill_Alley = updateReq.Bill_Alley;
            entity_main.Bill_FullAddr = updateReq.Bill_FullAddr;
            entity_main.Bill_Other = updateReq.Bill_Other;
            // 家長地址
            entity_main.ParentLive_ZipCode = updateReq.ParentLive_ZipCode;
            entity_main.ParentLive_City = updateReq.ParentLive_City;
            entity_main.ParentLive_District = updateReq.ParentLive_District;
            entity_main.ParentLive_Road = updateReq.ParentLive_Road;
            entity_main.ParentLive_Number = updateReq.ParentLive_Number;
            entity_main.ParentLive_SubNumber = updateReq.ParentLive_SubNumber;
            entity_main.ParentLive_Floor = updateReq.ParentLive_Floor;
            entity_main.ParentLive_Lane = updateReq.ParentLive_Lane;
            entity_main.ParentLive_Alley = updateReq.ParentLive_Alley;
            entity_main.ParentLive_FullAddr = updateReq.ParentLive_FullAddr;
            entity_main.ParentLive_Other = updateReq.ParentLive_Other;

            await _context.SaveChangesAsync();

            if (addressErrorBuilder.Length > 0)
            {
                var addressErrorMsg = String.Join(Environment.NewLine, addressErrorBuilder.ToString().TrimEnd('、').Split('、'));
                return ApiResponseHelper.BusinessLogicFailed<string>(
                    null,
                    $"請檢查以下地址之郵遞區號（查詢錯誤，請自行填寫）：{Environment.NewLine}{string.Join(Environment.NewLine, addressErrorMsg)}"
                );
            }

            return ApiResponseHelper.UpdateByIdSuccess(request.applyNo, request.applyNo);
        }

        private async Task<string> SearchZipCode(string city, string district, string road, string number, string subNumber, string lane)
        {
            var addressInfos = await _context
                .SetUp_AddressInfo.AsNoTracking()
                .Where(x => x.City == city && x.Area == district && x.Road == road)
                .ToListAsync();

            var addressInfoDtos = addressInfos
                .Select(x => new AddressInfoDto()
                {
                    City = x.City,
                    Area = x.Area,
                    Road = x.Road,
                    Scope = x.Scope,
                    ZipCode = x.ZIPCode,
                })
                .ToList();

            var searchAddressInfo = new SearchAddressInfoDto()
            {
                City = city,
                District = district,
                Road = road,
                Number = int.TryParse(number, out var numberInt) ? numberInt : 0,
                SubNumber = int.TryParse(subNumber, out var subNumberInt) ? subNumberInt : 0,
                Lane = int.TryParse(lane, out var laneInt) ? laneInt : 0,
            };

            var zipCode = AddressHelper.FindZipCode(addressInfoDtos, searchAddressInfo);

            if (string.IsNullOrEmpty(zipCode))
            {
                _logger.LogError(
                    "郵遞區號查詢錯誤，city: {@city}, district: {@district}, road: {@road}, number: {@number}, subNumber: {@subNumber}, lane: {@lane}",
                    city,
                    district,
                    road,
                    number,
                    subNumber,
                    lane
                );
            }

            return zipCode;
        }

        private async Task<string> SearchZipCode(string fullAddress)
        {
            var (city, area) = AddressHelper.GetCityAndDistrict(fullAddress);
            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(area))
                return string.Empty;

            var addressInfo = await _context.SetUp_AddressInfo.AsNoTracking().FirstOrDefaultAsync(x => x.City == city && x.Area == area);

            if (addressInfo == null)
                return string.Empty;

            var zipCode = AddressHelper.ZipCodeFormatZero(addressInfo.ZIPCode, 2);
            return zipCode;
        }

        /// <summary>
        /// 將 UpdateApplicationInfoByIdRequest 的全形字元轉換為半形字元
        /// </summary>
        /// <param name="request">原始請求物件</param>
        /// <returns>轉換後的新請求物件</returns>
        private UpdateApplicationInfoByIdRequest ToHalfWidthRequest(UpdateApplicationInfoByIdRequest update)
        {
            return new UpdateApplicationInfoByIdRequest()
            { // 個人基本資料
                ApplyNo = update.ApplyNo,
                CHName = update.CHName.ToHalfWidth(),
                ID = update.ID.ToHalfWidth(),
                CitizenshipCode = update.CitizenshipCode,
                BirthCitizenshipCode = update.BirthCitizenshipCode,
                Mobile = update.Mobile.ToHalfWidth(),
                Sex = update.Sex,
                BirthDay = update.BirthDay.ToHalfWidth(),
                ENName = update.ENName.ToHalfWidth(),
                IDCardRenewalLocationCode = update.IDCardRenewalLocationCode,
                IDTakeStatus = update.IDTakeStatus,
                BirthCitizenshipCodeOther = update.BirthCitizenshipCodeOther,
                MarriageState = update.MarriageState,
                Education = update.Education,
                GraduatedElementarySchool = update.GraduatedElementarySchool.ToHalfWidth(),
                EMail = update.EMail,
                HouseRegPhone = update.HouseRegPhone.ToHalfWidth(),
                LivePhone = update.LivePhone.ToHalfWidth(),
                LiveOwner = update.LiveOwner,
                LiveYear = update.LiveYear,
                PromotionUnit = update.PromotionUnit.ToHalfWidth(),
                IDIssueDate = update.IDIssueDate.ToHalfWidth(),
                PromotionUser = update.PromotionUser.ToHalfWidth(),
                AnliNo = update.AnliNo.ToHalfWidth(),
                ResidencePermitIssueDate = update.ResidencePermitIssueDate.ToHalfWidth(),
                PassportNo = update.PassportNo.ToHalfWidth(),
                PassportDate = update.PassportDate.ToHalfWidth(),
                ExpatValidityPeriod = update.ExpatValidityPeriod.ToHalfWidth(),
                IsApplyDigtalCard = update.IsApplyDigtalCard,
                IsFATCAIdentity = update.IsFATCAIdentity,
                SocialSecurityCode = update.SocialSecurityCode.ToHalfWidth(),
                IsDunyangBlackList = update.IsDunyangBlackList,
                NameCheckedReasonCodes = update.NameCheckedReasonCodes,
                ISRCAForCurrentPEP = update.ISRCAForCurrentPEP,
                ResignPEPKind = update.ResignPEPKind,
                PEPRange = update.PEPRange,
                IsCurrentPositionRelatedPEPPosition = update.IsCurrentPositionRelatedPEPPosition,
                BillType = update.BillType,
                AcceptType = update.AcceptType,
                IsConvertCard = update.IsConvertCard,
                ResidencePermitBackendNum = update.ResidencePermitBackendNum.ToHalfWidth(),
                IsForeverResidencePermit = update.IsForeverResidencePermit,
                ResidencePermitDeadline = update.ResidencePermitDeadline.ToHalfWidth(),
                OldCertificateVerified = update.OldCertificateVerified,
                ChildrenCount = update.ChildrenCount,
                ElecCodeId = update.ElecCodeId.ToHalfWidth(),
                AnnualFeePaymentType = update.AnnualFeePaymentType.ToHalfWidth(),
                EmploymentDate = update.EmploymentDate.ToHalfWidth(),
                DepartmentName = update.DepartmentName.ToHalfWidth(),

                // 戶籍地址
                Reg_ZipCode = update.Reg_ZipCode.ToHalfWidth(),
                Reg_City = update.Reg_City,
                Reg_District = update.Reg_District,
                Reg_Road = update.Reg_Road,
                Reg_Lane = update.Reg_Lane.ToHalfWidth(),
                Reg_Alley = update.Reg_Alley.ToHalfWidth(),
                Reg_Number = update.Reg_Number.ToHalfWidth(),
                Reg_SubNumber = update.Reg_SubNumber.ToHalfWidth(),
                Reg_Floor = update.Reg_Floor.ToHalfWidth(),
                Reg_FullAddr = update.Reg_FullAddr.ToHalfWidth(),
                Reg_Other = update.Reg_Other.ToHalfWidth(),

                // 居住地址
                Live_ZipCode = update.Live_ZipCode.ToHalfWidth(),
                Live_City = update.Live_City,
                Live_District = update.Live_District,
                Live_Road = update.Live_Road,
                Live_Lane = update.Live_Lane.ToHalfWidth(),
                Live_Alley = update.Live_Alley.ToHalfWidth(),
                Live_Number = update.Live_Number.ToHalfWidth(),
                Live_SubNumber = update.Live_SubNumber.ToHalfWidth(),
                Live_Floor = update.Live_Floor.ToHalfWidth(),
                Live_FullAddr = AddressHelper.將縣市台字轉換為臺字(update.Live_FullAddr.ToHalfWidth()),
                Live_Other = update.Live_Other.ToHalfWidth(),
                LiveAddressType = update.LiveAddressType,

                // 寄卡地址
                SendCard_ZipCode = update.SendCard_ZipCode.ToHalfWidth(),
                SendCard_City = update.SendCard_City,
                SendCard_District = update.SendCard_District,
                SendCard_Road = update.SendCard_Road,
                SendCard_Lane = update.SendCard_Lane.ToHalfWidth(),
                SendCard_Alley = update.SendCard_Alley.ToHalfWidth(),
                SendCard_Number = update.SendCard_Number.ToHalfWidth(),
                SendCard_SubNumber = update.SendCard_SubNumber.ToHalfWidth(),
                SendCard_Floor = update.SendCard_Floor.ToHalfWidth(),
                SendCard_FullAddr = AddressHelper.將縣市台字轉換為臺字(update.SendCard_FullAddr.ToHalfWidth()),
                SendCard_Other = update.SendCard_Other.ToHalfWidth(),
                SendCardAddressType = update.SendCardAddressType,

                // 帳單地址
                Bill_ZipCode = update.Bill_ZipCode.ToHalfWidth(),
                Bill_City = update.Bill_City,
                Bill_District = update.Bill_District,
                Bill_Road = update.Bill_Road,
                Bill_Lane = update.Bill_Lane.ToHalfWidth(),
                Bill_Alley = update.Bill_Alley.ToHalfWidth(),
                Bill_Number = update.Bill_Number.ToHalfWidth(),
                Bill_SubNumber = update.Bill_SubNumber.ToHalfWidth(),
                Bill_Floor = update.Bill_Floor.ToHalfWidth(),
                Bill_FullAddr = AddressHelper.將縣市台字轉換為臺字(update.Bill_FullAddr.ToHalfWidth()),
                Bill_Other = update.Bill_Other.ToHalfWidth(),
                BillAddressType = update.BillAddressType,

                // 職業資料
                CompName = update.CompName.ToHalfWidth(),
                CompID = update.CompID.ToHalfWidth(),
                CompJobTitle = update.CompJobTitle.ToHalfWidth(),
                CompSeniority = update.CompSeniority,
                CompPhone = update.CompPhone.ToHalfWidth(),
                AMLProfessionCode = update.AMLProfessionCode,
                AMLProfessionOther = update.AMLProfessionOther.ToHalfWidth(),
                AMLJobLevelCode = update.AMLJobLevelCode,
                CompTrade = update.CompTrade,
                CompJobLevel = update.CompJobLevel,
                CurrentMonthIncome = update.CurrentMonthIncome,
                MainIncomeAndFundCodes = update.MainIncomeAndFundCodes,
                MainIncomeAndFundOther = update.MainIncomeAndFundOther.ToHalfWidth(),

                // 公司地址
                Comp_ZipCode = update.Comp_ZipCode.ToHalfWidth(),
                Comp_City = update.Comp_City,
                Comp_District = update.Comp_District,
                Comp_Road = update.Comp_Road,
                Comp_Lane = update.Comp_Lane.ToHalfWidth(),
                Comp_Alley = update.Comp_Alley.ToHalfWidth(),
                Comp_Number = update.Comp_Number.ToHalfWidth(),
                Comp_SubNumber = update.Comp_SubNumber.ToHalfWidth(),
                Comp_Floor = update.Comp_Floor.ToHalfWidth(),
                Comp_FullAddr = AddressHelper.將縣市台字轉換為臺字(update.Comp_FullAddr.ToHalfWidth()),
                Comp_Other = update.Comp_Other.ToHalfWidth(),

                // 學生申請人
                IsStudent = update.IsStudent,
                ParentName = update.ParentName.ToHalfWidth(),
                StudSchool = update.StudSchool.ToHalfWidth(),
                ParentPhone = update.ParentPhone.ToHalfWidth(),
                StudScheduledGraduationDate = update.StudScheduledGraduationDate.ToHalfWidth(),
                StudentApplicantRelationship = update.StudentApplicantRelationship,

                // 家長居住地址
                ParentLive_ZipCode = update.ParentLive_ZipCode.ToHalfWidth(),
                ParentLive_City = update.ParentLive_City,
                ParentLive_District = update.ParentLive_District,
                ParentLive_Road = update.ParentLive_Road,
                ParentLive_Lane = update.ParentLive_Lane.ToHalfWidth(),
                ParentLive_Alley = update.ParentLive_Alley.ToHalfWidth(),
                ParentLive_Number = update.ParentLive_Number.ToHalfWidth(),
                ParentLive_SubNumber = update.ParentLive_SubNumber.ToHalfWidth(),
                ParentLive_Floor = update.ParentLive_Floor.ToHalfWidth(),
                ParentLive_FullAddr = AddressHelper.將縣市台字轉換為臺字(update.ParentLive_FullAddr.ToHalfWidth()),
                ParentLive_Other = update.ParentLive_Other.ToHalfWidth(),
                ParentLiveAddressType = update.ParentLiveAddressType,

                // 活動資料
                FirstBrushingGiftCode = update.FirstBrushingGiftCode.ToHalfWidth(),
                ProjectCode = update.ProjectCode,
                IsAgreeDataOpen = update.IsAgreeDataOpen,
                IsAgreeMarketing = update.IsAgreeMarketing,
                IsPayNoticeBind = update.IsPayNoticeBind,
                IsAcceptEasyCardDefaultBonus = update.IsAcceptEasyCardDefaultBonus,

                // 簽核
                CreditCheckCode = update.CreditCheckCode,
            };
        }
    }
}
