using System.Text.RegularExpressions;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.Batch.Jobs.A02KYCSync.Helpers;

public static class MapHelper
{
    public static Reviewer3rd_KYCQueryLog MapKYCQueryLog(
        string applyNo,
        string cardStatus,
        string id,
        object request,
        object response,
        string kycCode,
        string kycRank,
        string kycMsg,
        DateTime queryTime,
        bool querySuccess,
        string apiName
    ) =>
        new()
        {
            ApplyNo = applyNo,
            CardStatus = cardStatus,
            ID = id,
            Request = JsonHelper.序列化物件(request),
            Response = JsonHelper.序列化物件(response),
            KYCCode = kycCode,
            KYCRank = kycRank,
            KYCMsg = kycMsg,
            AddTime = queryTime,
            KYCLastSendStatus = querySuccess && kycCode == MW3RtnCodeConst.入檔KYC_成功 ? KYCLastSendStatus.不需發送 : KYCLastSendStatus.等待,
            KYCLastSendTime = null,
            CurrentHandler = "KYCSyncJob",
            APIName = apiName,
            Source = "KYCSyncJob",
        };

    public static Reviewer_ApplyCreditCardInfoProcess MapKYCProcess(
        string applyNo,
        string process,
        DateTime startTime,
        DateTime endTime,
        bool isSuccess,
        string kycCode,
        string kycRank,
        UserType userType,
        string id,
        string keyMsg
    )
    {
        var isSuccessStr = isSuccess ? "成功" : "失敗";
        var userTypeStr = userType == UserType.正卡人 ? "正卡人" : "附卡人";
        var notes =
            $"KYC串接狀態: {isSuccessStr}; KYC Api: KYC入檔; KYC 代碼: {kycCode}; KYC Level: {kycRank}; KYC Message: {keyMsg}; ({userTypeStr}_{id})";

        return new()
        {
            ApplyNo = applyNo,
            Process = process,
            StartTime = startTime,
            EndTime = endTime,
            Notes = notes,
            ProcessUserId = "SYSTEM",
        };
    }

    public static SyncKycMW3Info Map原卡友SyncKycMW3Info(Reviewer_ApplyCreditCardInfoMain main)
    {
        return new SyncKycMW3Info()
        {
            BRRFlag = string.Empty,
            ID = 轉換ID符合KYC格式(main.ID),
            CHName = main.CHName.ToFullWidth(),
            ENName = main.ENName.ToFullWidth(),
            BirthDay = main.BirthDay.ToWesternDate(outputFormat: "yyyy-MM-dd"),
            NameCheckedReasonCodes = string.Empty,
            ISRCAForCurrentPEP = string.Empty,
            ResignPEPKind = string.Empty,
            PEPRange = string.Empty,
            IsCurrentPositionRelatedPEPPosition = string.Empty,
            NameCheckStartTime = string.Empty,
            NameCheckResponseResult = string.Empty,
            NameCheckRcPoint = string.Empty,
            NameCheckAMLId = string.Empty,
            Nation = main.CitizenshipCode == "TW" ? KYCRequestConst.國籍_中華民國 : KYCRequestConst.國籍_其他,
            NationO = main.CitizenshipCode != "TW" ? main.CitizenshipCode : string.Empty,
            BirthCitizenshipCode =
                main.BirthCitizenshipCode == BirthCitizenshipCode.中華民國 ? KYCRequestConst.出生地_中華民國 : KYCRequestConst.出生地_其他,
            BirthCitizenshipCodeOther = main.BirthCitizenshipCode == BirthCitizenshipCode.其他 ? main.BirthCitizenshipCodeOther : string.Empty,
            AMLProfessionCode = string.IsNullOrWhiteSpace(main.AMLProfessionCode) ? string.Empty : main.AMLProfessionCode.PadLeft(2, '0'),
            AMLProfessionOther = main.AMLProfessionOther,
            AMLJobLevelCode = main.AMLJobLevelCode,
            PosO = string.Empty, // TODO:20250729 確認新系統是否開啟此欄位
            HomeAddFlag = KYCRequestConst.戶籍地址_其他,
            Reg_ZipCode = string.Empty,
            Reg_City = string.Empty,
            Reg_District = string.Empty,
            Reg_Road = string.Empty,
            Reg_Lane = string.Empty,
            Reg_Alley = string.Empty,
            Reg_Number = string.Empty,
            Reg_SubNumber = string.Empty,
            Reg_Floor = string.Empty,
            Reg_Other = string.Empty,
            HomeO = main.CitizenshipCode,
            HomeAddZip = string.Empty,
            HomeAdd = main.Reg_FullAddr,
            MainIncomeAndFundCodes = string.Join(",", main.MainIncomeAndFundCodes.Split(',').Select(x => x.PadLeft(2, '0'))),
            MainIncomeAndFundOther = main.MainIncomeAndFundOther,
            RAreaFlag = KYCRequestConst.客戶風險評估_卡友固定值,
            UOpnion = KYCRequestConst.經辦審查意見_建議核准,
            EditTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Mobile = main.Mobile,
            Source = KYCRequestConst.資料來源_原卡友加辦卡,
        };
    }

    private static string 轉換ID符合KYC格式(string id)
    {
        Regex TaiwanIdRegex = new(@"^[A-Z][12]\d{8}$", RegexOptions.Compiled);
        Regex OldForeignIdRegex = new(@"^[A-Z]{2}\d{8}$", RegexOptions.Compiled);
        Regex NewForeignIdRegex = new(@"^[A-Z]{1}[89]{1}\d{8}$", RegexOptions.Compiled);

        if (TaiwanIdRegex.IsMatch(id) || OldForeignIdRegex.IsMatch(id) || NewForeignIdRegex.IsMatch(id))
            return $"{id}A";
        else
            return $"{id}C";
    }
}
