using ScoreSharp.Common.Extenstions;
using ScoreSharp.Common.Helpers.Address;

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyInfoWebWhite;

public static class MapHelper
{
    /// <summary>
    /// 將 SyncApplyInfoWebWhiteRequest 的全形字元轉換為半形字元
    /// </summary>
    /// <param name="source">原始請求物件</param>
    /// <returns>轉換後的新請求物件</returns>
    public static SyncApplyInfoWebWhiteRequest ToHalfWidthRequest(SyncApplyInfoWebWhiteRequest source)
    {
        return new SyncApplyInfoWebWhiteRequest
        {
            // 基本資訊，不需要轉換
            ApplyNo = source.ApplyNo,
            SyncUserId = source.SyncUserId,
            CardOwner = source.CardOwner,

            // 正卡人基本資料
            M_CHName = source.M_CHName.ToHalfWidth(),
            M_ID = source.M_ID,
            M_Sex = source.M_Sex,
            M_BirthDay = source.M_BirthDay,
            M_ENName = source.M_ENName.ToHalfWidth(),
            M_BirthCitizenshipCode = source.M_BirthCitizenshipCode,
            M_BirthCitizenshipCodeOther = source.M_BirthCitizenshipCodeOther,
            M_CitizenshipCode = source.M_CitizenshipCode,
            M_IDIssueDate = source.M_IDIssueDate,
            M_IDCardRenewalLocationCode = source.M_IDCardRenewalLocationCode,
            M_IDTakeStatus = source.M_IDTakeStatus,
            M_Education = source.M_Education,
            M_GraduatedElementarySchool = source.M_GraduatedElementarySchool,

            // 聯絡資料
            M_Mobile = source.M_Mobile.ToHalfWidth(),
            M_EMail = source.M_EMail,
            M_HouseRegPhone = source.M_HouseRegPhone.ToHalfWidth(),
            M_LivePhone = source.M_LivePhone.ToHalfWidth(),

            // 戶籍地址
            M_Reg_ZipCode = source.M_Reg_ZipCode.ToHalfWidth(),
            M_Reg_City = AddressHelper.將縣市台字轉換為臺字(source.M_Reg_City.ToHalfWidth()),
            M_Reg_District = source.M_Reg_District.ToHalfWidth(),
            M_Reg_Road = source.M_Reg_Road.ToHalfWidth(),
            M_Reg_Lane = source.M_Reg_Lane.ToHalfWidth(),
            M_Reg_Alley = source.M_Reg_Alley.ToHalfWidth(),
            M_Reg_Number = source.M_Reg_Number.ToHalfWidth(),
            M_Reg_SubNumber = source.M_Reg_SubNumber.ToHalfWidth(),
            M_Reg_Floor = source.M_Reg_Floor.ToHalfWidth(),
            M_Reg_Other = source.M_Reg_Other.ToHalfWidth(),

            // 居住地址
            M_Live_AddressType = source.M_Live_AddressType,
            M_Live_ZipCode = source.M_Live_ZipCode.ToHalfWidth(),
            M_Live_City = AddressHelper.將縣市台字轉換為臺字(source.M_Live_City.ToHalfWidth()),
            M_Live_District = source.M_Live_District.ToHalfWidth(),
            M_Live_Road = source.M_Live_Road.ToHalfWidth(),
            M_Live_Lane = source.M_Live_Lane.ToHalfWidth(),
            M_Live_Alley = source.M_Live_Alley.ToHalfWidth(),
            M_Live_Number = source.M_Live_Number.ToHalfWidth(),
            M_Live_SubNumber = source.M_Live_SubNumber.ToHalfWidth(),
            M_Live_Floor = source.M_Live_Floor.ToHalfWidth(),
            M_Live_Other = source.M_Live_Other.ToHalfWidth(),

            // 帳單地址
            M_Bill_AddressType = source.M_Bill_AddressType,
            M_Bill_ZipCode = source.M_Bill_ZipCode.ToHalfWidth(),
            M_Bill_City = AddressHelper.將縣市台字轉換為臺字(source.M_Bill_City.ToHalfWidth()),
            M_Bill_District = source.M_Bill_District.ToHalfWidth(),
            M_Bill_Road = source.M_Bill_Road.ToHalfWidth(),
            M_Bill_Lane = source.M_Bill_Lane.ToHalfWidth(),
            M_Bill_Alley = source.M_Bill_Alley.ToHalfWidth(),
            M_Bill_Number = source.M_Bill_Number.ToHalfWidth(),
            M_Bill_SubNumber = source.M_Bill_SubNumber.ToHalfWidth(),
            M_Bill_Floor = source.M_Bill_Floor.ToHalfWidth(),
            M_Bill_Other = source.M_Bill_Other.ToHalfWidth(),

            // 寄卡地址
            M_SendCard_AddressType = source.M_SendCard_AddressType,
            M_SendCard_ZipCode = source.M_SendCard_ZipCode.ToHalfWidth(),
            M_SendCard_City = AddressHelper.將縣市台字轉換為臺字(source.M_SendCard_City.ToHalfWidth()),
            M_SendCard_District = source.M_SendCard_District.ToHalfWidth(),
            M_SendCard_Road = source.M_SendCard_Road.ToHalfWidth(),
            M_SendCard_Lane = source.M_SendCard_Lane.ToHalfWidth(),
            M_SendCard_Alley = source.M_SendCard_Alley.ToHalfWidth(),
            M_SendCard_Number = source.M_SendCard_Number.ToHalfWidth(),
            M_SendCard_SubNumber = source.M_SendCard_SubNumber.ToHalfWidth(),
            M_SendCard_Floor = source.M_SendCard_Floor.ToHalfWidth(),
            M_SendCard_Other = source.M_SendCard_Other.ToHalfWidth(),

            // 工作資料
            M_CompName = source.M_CompName.ToHalfWidth(),
            M_CompPhone = source.M_CompPhone.ToHalfWidth(),
            M_CompID = source.M_CompID.ToHalfWidth(),
            M_CompJobTitle = source.M_CompJobTitle,
            M_CompSeniority = source.M_CompSeniority,
            M_CurrentMonthIncome = source.M_CurrentMonthIncome,
            M_AMLProfessionCode = source.M_AMLProfessionCode,
            M_AMLProfessionOther = source.M_AMLProfessionOther,
            M_AMLJobLevelCode = source.M_AMLJobLevelCode,
            M_MainIncomeAndFundCodes = source.M_MainIncomeAndFundCodes,
            M_MainIncomeAndFundOther = source.M_MainIncomeAndFundOther,

            // 公司地址
            M_Comp_ZipCode = source.M_Comp_ZipCode.ToHalfWidth(),
            M_Comp_City = AddressHelper.將縣市台字轉換為臺字(source.M_Comp_City.ToHalfWidth()),
            M_Comp_District = source.M_Comp_District.ToHalfWidth(),
            M_Comp_Road = source.M_Comp_Road.ToHalfWidth(),
            M_Comp_Lane = source.M_Comp_Lane.ToHalfWidth(),
            M_Comp_Alley = source.M_Comp_Alley.ToHalfWidth(),
            M_Comp_Number = source.M_Comp_Number.ToHalfWidth(),
            M_Comp_SubNumber = source.M_Comp_SubNumber.ToHalfWidth(),
            M_Comp_Floor = source.M_Comp_Floor.ToHalfWidth(),
            M_Comp_Other = source.M_Comp_Other.ToHalfWidth(),

            // 同意條款相關
            M_IsAgreeDataOpen = source.M_IsAgreeDataOpen,
            IsAgreeMarketing = source.IsAgreeMarketing,
            M_IsAcceptEasyCardDefaultBonus = source.M_IsAcceptEasyCardDefaultBonus,

            // 其他設定
            PromotionUnit = source.PromotionUnit,
            PromotionUser = source.PromotionUser,
            BillType = source.BillType,
            LiveOwner = source.LiveOwner,
            AnliNo = source.AnliNo,
            FirstBrushingGiftCode = source.FirstBrushingGiftCode,
            ProjectCode = source.ProjectCode,

            // 卡片資訊
            CardInfo = source.CardInfo,

            // 申請處理歷程
            ApplyProcess = source.ApplyProcess,
        };
    }
}
