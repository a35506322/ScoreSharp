using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSupplementaryInfoByApplyNo;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 修改申請書附卡人資料 By申請書編號
        /// </summary>
        /// <param name="applyNo"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        ///<remarks>
        ///
        /// <para> ✅ 欄位驗證說明：</para>
        ///
        /// 1.【申請書編號 ApplyNo】
        /// - 必填。
        /// - 長度限制：14。
        /// - 格式：只能輸入英數等。
        ///
        /// 2.【中文姓名 CHName】
        /// - 必填。
        /// - 長度限制：25。
        /// - 轉半形。
        ///
        /// 3.【身分證字號 ID】
        /// - 必填。
        /// - 長度限制：10。
        /// - 轉半形。
        /// - 符合下列格式之一：
        ///     - ① 台灣國民身分證號（現行）
        ///         - 格式：1 碼英文字母 + 9 碼數字（共 10 碼）
        ///         - 第 2 碼數字為性別：1（男）、2（女）
        ///     - ② 舊制外籍人士統一證號
        ///         - 格式：2 碼英文字母 + 8 碼數字（共 10 碼）
        ///     - ③ 新制外籍人士統一證號（2021 年起）
        ///         - 格式：1 碼英文字母 + 第 2 碼為 8 或 9 + 8 碼數字（共 10 碼）
        ///
        /// 4.【出生年月日 BirthDay】
        /// - 必填。
        /// - 長度限制：7。
        /// - 轉半形。
        /// - 格式：民國格式 `YYYMMDD`。
        ///
        /// 5.【英文姓名 ENName】
        /// - 必填。
        /// - 長度限制：100 。
        /// - 轉半形。
        ///
        /// 6.【與正卡人關係 ApplicantRelationship】
        /// - 必填。
        ///
        /// 7.【國籍 CitizenshipCode】
        /// - 必填。
        /// - 長度限制：2。
        ///
        /// 8.【身分證發證日期 IDIssueDate】
        /// - 長度限制：7。
        /// - 轉半形。
        /// - 格式：民國格式 `YYYMMDD`。
        /// - 當【國籍 CitizenshipCode】為 TW（台灣）且為【非卡友】時必填。
        ///
        /// 9.【身分證換發地點 IDCardRenewalLocationCode】
        /// - 長度限制：8。
        /// - 當【國籍 CitizenshipCode】為 TW（台灣）且為【非卡友】時必填。
        ///
        /// 10.【身分證領取狀態 IDTakeStatus】
        /// - 當【國籍 CitizenshipCode】為 TW（台灣）且為【非卡友】時必填。
        ///
        /// 11.【出生地 BirthCitizenshipCode】
        /// - 必填。
        /// - 當選擇「其他（代碼為 2）」時，需同時填寫「出生地國籍_其他」。
        ///
        /// 12.【出生地_其他 BirthCitizenshipCodeOther】
        /// - 當「出生地國籍」為「其他（代碼為 2）」時必填。
        /// - 長度限制：16。
        ///
        /// 13.【FATCA身份 IsFATCAIdentity】
        /// - 必填。
        /// - 長度限制：1。
        /// - 格式：只能輸入 Ｙ 或 Ｎ 或 null。
        /// - 當【出生地國籍_其他 BirthCitizenshipCodeOther】為 US（美國）時必填，預設值為 Y，也可選 N。
        /// - 當【出生地國籍_其他 BirthCitizenshipCodeOther】不為 US 時，應為 null。
        ///
        /// 14.【社會安全號碼 SocialSecurityCode】
        /// - 長度限制：30。
        /// - 轉半形。
        /// - 當【是否為 FATCA 身份 IsFATCAIdentity】為 Y 時必填。
        ///
        /// 15.【是否為永久居留證 IsForeverResidencePermit】
        /// - 長度限制：1。
        /// - 格式：只能輸入 Ｙ 或 Ｎ。
        /// - 當選擇 N 時，則需填寫「外籍人士指定效期」。
        /// - 當【國籍 CitizenshipCode】不為 TW（台灣）時必填。
        ///
        /// 16.【居留證發證日期 ResidencePermitIssueDate】
        /// - 長度限制：8。
        /// - 轉半形。
        /// - 格式：西元格式 `YYYYMMDD`。
        /// - 當【國籍 CitizenshipCode】不為 TW（台灣）時必填。
        ///
        /// 17.【居留證期限 ResidencePermitDeadline】
        /// - 長度限制：8。
        /// - 轉半形。
        /// - 格式：西元格式 `YYYYMMDD`。
        /// - 當【國籍 CitizenshipCode】不為 TW（台灣）時必填。
        ///
        /// 18.【居留證背面號碼 ResidencePermitBackendNum】
        /// - 長度限制：10。
        /// - 轉半形。
        /// - 格式：前兩碼大寫英文 + 8 碼數字，範例ＹＺ80000001。
        /// - 當【國籍 CitizenshipCode】不為 TW（台灣）時必填。
        ///
        /// 19.【護照號碼 PassportNo】
        /// - 長度限制：20。
        /// - 轉半形。
        ///
        /// 20.【護照日期 PassportDate】
        /// - 長度限制：8。
        /// - 轉半形。
        /// - 格式：西元格式 `YYYYMMDD`。
        ///
        /// 21.【外籍人士指定效期 ExpatValidityPeriod】
        /// - 長度限制：6。
        /// - 轉半形。
        /// - 格式：西元格式 `YYYYMM`。
        /// - 當「是否為永久居留證」選擇 N 時必填。
        ///
        /// 22.【舊照查驗 OldCertificateVerified】
        /// - 長度限制：1。
        /// - 格式：只能輸入 Ｙ 或 Ｎ。
        ///
        /// 23.【公司名稱 CompName】
        /// - 長度限制：25。
        /// - 轉半形。
        ///
        /// 24.【職稱 CompJobTitle】
        /// - 長度限制：30。
        /// - 轉半形。
        ///
        /// 25.【公司電話 CompPhone】
        /// - 長度限制：21。
        /// - 轉半形。
        /// - 格式：3碼-七八碼#5碼分機(範例020-28572463#55555)
        ///
        /// 26.【居住電話 LivePhone】
        /// - 長度限制：18。
        /// - 轉半形。
        /// - 格式：3碼-七八碼(範例020-28572463)
        ///
        /// 27.【行動電話 Mobile】
        /// - 必填。
        /// - 長度限制：10。
        /// - 轉半形。
        /// - 格式：09 開頭、共 10 碼數字。
        ///
        /// 28.【居住地址類型 ResidenceType】
        /// - 若有填寫：將檢查【居住地址 Live_ 】是否與所選地址類型一致（如選擇同戶籍地址，則需相同）。
        ///
        /// 29.【居住_巷 Live_Lane】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 30.【居住_弄 Live_Alley】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 31.【居住_號 Live_Number】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 32.【居住_之號 Live_SubNumber】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 33.【居住_樓層 Live_Floor】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 34.【居住_其他 Live_Other】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 35.【居住_完整地址 Live_FullAddr】
        /// - 轉半形。
        /// - 長度限制：100。
        /// - 正則表達式：^(臺北市|新北市|桃園市|臺中市|臺南市|高雄市|新竹市|嘉義市|基隆市|宜蘭縣|新竹縣|苗栗縣|彰化縣|南投縣|雲林縣|嘉義縣|屏東縣|臺東縣|花蓮縣|澎湖縣|金門縣|連江縣|釣魚臺|南海島)([^市縣]+?[區鄉鎮市]).*$
        /// - 範例：臺北市大安區忠孝東路12巷5弄8號
        ///
        /// 36.【寄卡地址類型 ShippingCardAddressType】
        /// - 若有填寫：將檢查【寄卡地址 SendCard_ 】是否與所選地址類型一致（如選擇同戶籍地址，則需相同）。
        ///
        /// 37.【寄卡_郵遞區號 SendCard_ZipCode】
        /// - 必填。
        /// - 轉半形。
        ///
        /// 38.【寄卡_縣市 SendCard_City】
        /// - 當為【非卡友】時必填。
        ///
        /// 39.【寄卡_區域 SendCard_District】
        /// - 當為【非卡友】時必填。
        ///
        /// 40.【寄卡_街道 SendCard_Road】
        /// - 當為【非卡友】時必填。
        ///
        /// 41.【寄卡_巷 SendCard_Lane】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 42.【寄卡_弄 SendCard_Alley】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 43.【寄卡_號 SendCard_Number】
        /// - 轉半形。
        /// - 當為【非卡友】時必填。
        /// - 長度限制：25。
        ///
        /// 44.【寄卡_之號 SendCard_SubNumber】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 45.【寄卡_樓層 SendCard_Floor】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 46.【寄卡_其他 SendCard_Other】
        /// - 轉半形。
        /// - 長度限制：25。
        ///
        /// 47.【寄卡_完整地址 SendCard_FullAddr】
        /// - 轉半形。
        /// - 長度限制：100。
        /// - 當為【卡友】時，必填。
        /// - 正則表達式：^(臺北市|新北市|桃園市|臺中市|臺南市|高雄市|新竹市|嘉義市|基隆市|宜蘭縣|新竹縣|苗栗縣|彰化縣|南投縣|雲林縣|嘉義縣|屏東縣|臺東縣|花蓮縣|澎湖縣|金門縣|連江縣|釣魚臺|南海島)([^市縣]+?[區鄉鎮市]).*$
        /// - 範例：臺北市大安區忠孝東路12巷5弄8號
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
        /// 48.【敦陽系統黑名單 IsDunyangBlackList】
        /// - 長度限制：1。
        /// - 格式：只能輸入 Ｙ 或 Ｎ。
        /// - 當正卡人【姓名檢核結果 NameChecked】= Y 時，此欄位必填。
        /// - `當此欄位為 "Y" 時，【姓名檢核理由碼 NameCheckedReasonCodes】需必填且不得勾選無：5；反之，只能勾選無：5`。
        ///
        /// 49.【姓名檢核理由代碼 NameCheckedReasonCodes】
        /// - 長度限制：20。
        /// - 格式：逗號分隔多選值（例如：`1,6`）
        /// - 驗證規則：52.【當前或曾為PEP身分 ISRCAForCurrentPEP】
        ///     - 勾選 6：RCA 時，**必須同時勾選** 1：PEP，例如 `1,6`
        ///     - RCA (6) **不可與除 1 以外的項目並存**（如 `1,3,6` 無效）53.【卸任PEP種類 ResignPEPKind】
        ///     - 勾選 7~10 時，**必須同時勾選**  1：PEP
        ///     - 勾選 5：無 時，**不可與其他任一項目混選**（需單選）54.【擔任PEP範圍 PEPRange】
        ///
        /// - 提示建議：【現任職位是否與PEP職位相關 IsCurrentPositionRelatedPEPPosition】
        ///     - 勾選 RCA 未勾 PEP → 顯示「勾選 RCA 時，需同時勾選 PEP」
        ///
        /// 50.【是否為現任 PEP ISRCAForCurrentPEP】
        /// - 長度限制：1。
        /// - 格式：只能輸入 Ｙ 或 Ｎ。
        /// - 預設為 N。
        /// - 當【姓名檢核理由碼 NameCheckedReasonCodes】中包含 PEP（1）+卸任 PEP（10）時必填。
        ///
        /// 51.【現任職位是否與 PEP 職務相關 IsCurrentPositionRelatedPEPPosition】
        /// - 長度限制：1。
        /// - 格式：只能輸入 Ｙ 或 Ｎ。
        /// - 當【姓名檢核理由碼 NameCheckedReasonCodes】中包含 PEP（1）+卸任 PEP（10）時必填。
        ///
        /// 52.【擔任 PEP 範圍 PEPRange】
        /// - 當【姓名檢核理由碼 NameCheckedReasonCodes】中包含 PEP（1）+卸任 PEP（10）時必填。
        ///
        /// 53.【是否已辭去 PEP 職位 ResignPEPKind】
        /// - 當【姓名檢核理由碼 NameCheckedReasonCodes】中包含 PEP（1）+卸任 PEP（10）時必填。
        ///
        /// <para> ✂️ 刪除欄位 </para>
        ///
        /// 1. 【洗防風險等級 AMLRiskLevel】
        ///
        /// 2. 【AML職業別 AMLProfessionCode】
        ///
        /// 3. 【AML職業別_其他 AMLProfessionOther】
        ///
        /// 4. 【AML職級別 AMLJobLevelCode】
        ///
        /// 5. 【主要收入來源 MainIncomeAndFundCodes】
        ///
        /// 6. 【主要收入來源_其他 MainIncomeAndFundOther】
        ///
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(修改申請書附卡人資料_2000_ReqEx),
            typeof(修改申請書附卡人資料查無定義值_4000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(修改申請書附卡人資料_2000_ResEx),
            typeof(修改申請書附卡人資料查無郵遞區號_4003_ResEx),
            typeof(修改申請書附卡人資料國籍與FATCA身分不符_4003_ResEx),
            typeof(修改申請書附卡人資料路由與Req比對錯誤_4003_ResEx),
            typeof(修改申請書附卡人資料查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(修改申請書附卡人資料查無定義值_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [HttpPut("{applyNo}")]
        [OpenApiOperation("UpdateSupplementaryInfoByApplyNo")]
        public async Task<IResult> UpdateSupplementaryInfoByApplyNo(
            [FromRoute] string applyNo,
            [FromBody] UpdateSupplementaryInfoByApplyNoRequest request
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSupplementaryInfoByApplyNo
{
    public record Command(string applyNo, UpdateSupplementaryInfoByApplyNoRequest updateSupplementaryInfoByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext _context, IJWTProfilerHelper _jwthelper, ILogger<Handler> _logger)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            /*
             * 1. 檢查定義值
             * 2. 邏輯檢查 -> 有誤先回傳錯誤訊息
             * 3. 轉換郵遞區號
             * 4. 有問題的儲存後再給提示訊息
             */

            var req = ToHalfWidthRequest(request.updateSupplementaryInfoByApplyNoRequest);
            var applyNo = request.applyNo;

            if (applyNo != request.applyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var supplementary = await _context.Reviewer_ApplyCreditCardInfoSupplementary.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (supplementary is null)
                return ApiResponseHelper.NotFound<string>(null, request.applyNo);

            // 轉換郵遞區號
            var addressErrorBuilder = new StringBuilder();
            if (
                string.IsNullOrEmpty(req.Live_ZipCode)
                && !String.IsNullOrEmpty(req.Live_City)
                && !String.IsNullOrEmpty(req.Live_District)
                && !String.IsNullOrEmpty(req.Live_Road)
            )
            {
                req.Live_ZipCode = await SearchZipCode(
                    req.Live_City,
                    req.Live_District,
                    req.Live_Road,
                    req.Live_Number,
                    req.Live_SubNumber,
                    req.Live_Lane
                );

                if (string.IsNullOrEmpty(req.Live_ZipCode))
                    addressErrorBuilder.AppendFormat("附卡人居住地址、");
            }
            else if (string.IsNullOrEmpty(req.Live_ZipCode) && !String.IsNullOrEmpty(req.Live_FullAddr))
            {
                req.Live_ZipCode = await SearchZipCode(req.Live_FullAddr);

                if (string.IsNullOrEmpty(req.Live_ZipCode))
                    addressErrorBuilder.AppendFormat("附卡人居住地址、");
            }

            if (
                string.IsNullOrEmpty(req.SendCard_ZipCode)
                && !String.IsNullOrEmpty(req.SendCard_City)
                && !String.IsNullOrEmpty(req.SendCard_District)
                && !String.IsNullOrEmpty(req.SendCard_Road)
            )
            {
                req.SendCard_ZipCode = await SearchZipCode(
                    req.SendCard_City,
                    req.SendCard_District,
                    req.SendCard_Road,
                    req.SendCard_Number,
                    req.SendCard_SubNumber,
                    req.SendCard_Lane
                );

                if (string.IsNullOrEmpty(req.SendCard_ZipCode))
                    addressErrorBuilder.AppendFormat("附卡人寄卡地址、");
            }
            else if (string.IsNullOrEmpty(req.SendCard_ZipCode) && !String.IsNullOrEmpty(req.SendCard_FullAddr))
            {
                req.SendCard_ZipCode = await SearchZipCode(req.SendCard_FullAddr);

                if (string.IsNullOrEmpty(req.SendCard_ZipCode))
                    addressErrorBuilder.AppendFormat("附卡人寄卡地址、");
            }

            // 更新附卡人資料
            supplementary.CHName = req.CHName;
            supplementary.ID = req.ID;
            supplementary.Sex = req.Sex;
            supplementary.BirthDay = req.BirthDay;
            supplementary.ENName = req.ENName;
            supplementary.MarriageState = req.MarriageState;
            supplementary.ApplicantRelationship = req.ApplicantRelationship;
            supplementary.CitizenshipCode = req.CitizenshipCode;
            supplementary.IDIssueDate = req.IDIssueDate;
            supplementary.IDCardRenewalLocationCode = req.IDCardRenewalLocationCode;
            supplementary.IDTakeStatus = req.IDTakeStatus;
            supplementary.BirthCitizenshipCode = req.BirthCitizenshipCode;
            supplementary.BirthCitizenshipCodeOther = req.BirthCitizenshipCodeOther;
            supplementary.IsFATCAIdentity = req.IsFATCAIdentity;
            supplementary.SocialSecurityCode = req.SocialSecurityCode;
            supplementary.IsForeverResidencePermit = req.IsForeverResidencePermit;
            supplementary.ResidencePermitIssueDate = req.ResidencePermitIssueDate;
            supplementary.ResidencePermitDeadline = req.ResidencePermitDeadline;
            supplementary.ResidencePermitBackendNum = req.ResidencePermitBackendNum;
            supplementary.PassportNo = req.PassportNo;
            supplementary.PassportDate = req.PassportDate;
            supplementary.ExpatValidityPeriod = req.ExpatValidityPeriod;
            supplementary.OldCertificateVerified = req.OldCertificateVerified;
            supplementary.CompName = req.CompName;
            supplementary.CompJobTitle = req.CompJobTitle;
            supplementary.CompPhone = req.CompPhone;
            supplementary.LivePhone = req.LivePhone;
            supplementary.Mobile = req.Mobile;

            supplementary.ResidenceType = req.ResidenceType;
            // 居住地址
            supplementary.Live_ZipCode = req.Live_ZipCode;
            supplementary.Live_City = req.Live_City;
            supplementary.Live_District = req.Live_District;
            supplementary.Live_Road = req.Live_Road;
            supplementary.Live_Number = req.Live_Number;
            supplementary.Live_SubNumber = req.Live_SubNumber;
            supplementary.Live_Floor = req.Live_Floor;
            supplementary.Live_Lane = req.Live_Lane;
            supplementary.Live_Alley = req.Live_Alley;
            supplementary.Live_FullAddr = req.Live_FullAddr;
            supplementary.Live_Other = req.Live_Other;
            // 寄卡地址
            supplementary.ShippingCardAddressType = req.ShippingCardAddressType;
            supplementary.SendCard_ZipCode = req.SendCard_ZipCode;
            supplementary.SendCard_City = req.SendCard_City;
            supplementary.SendCard_District = req.SendCard_District;
            supplementary.SendCard_Road = req.SendCard_Road;
            supplementary.SendCard_Number = req.SendCard_Number;
            supplementary.SendCard_SubNumber = req.SendCard_SubNumber;
            supplementary.SendCard_Floor = req.SendCard_Floor;
            supplementary.SendCard_Lane = req.SendCard_Lane;
            supplementary.SendCard_Alley = req.SendCard_Alley;
            supplementary.SendCard_FullAddr = req.SendCard_FullAddr;
            supplementary.SendCard_Other = req.SendCard_Other;

            supplementary.IsDunyangBlackList = req.IsDunyangBlackList;
            supplementary.NameCheckedReasonCodes = req.NameCheckedReasonCodes;
            supplementary.ISRCAForCurrentPEP = req.ISRCAForCurrentPEP;
            supplementary.ResignPEPKind = req.ResignPEPKind;
            supplementary.PEPRange = req.PEPRange;
            supplementary.IsCurrentPositionRelatedPEPPosition = req.IsCurrentPositionRelatedPEPPosition;

            await _context
                .Reviewer_ApplyCreditCardInfoMain.Where(x => x.ApplyNo == applyNo)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(x => x.LastUpdateTime, DateTime.Now).SetProperty(x => x.LastUpdateUserId, _jwthelper.UserId)
                );

            await _context.SaveChangesAsync();

            if (addressErrorBuilder.Length > 0)
            {
                var addressErrorMsg = String.Join(Environment.NewLine, addressErrorBuilder.ToString().TrimEnd('、').Split('、'));
                return ApiResponseHelper.BusinessLogicFailed<string>(
                    null,
                    $"請檢查以下地址之郵遞區號（查詢錯誤，請自行填寫）：{Environment.NewLine}{string.Join(Environment.NewLine, addressErrorMsg)}"
                );
            }
            return ApiResponseHelper.UpdateByIdSuccess(applyNo, applyNo);
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

        private UpdateSupplementaryInfoByApplyNoRequest ToHalfWidthRequest(UpdateSupplementaryInfoByApplyNoRequest req)
        {
            return new UpdateSupplementaryInfoByApplyNoRequest()
            {
                ApplyNo = req.ApplyNo,
                CHName = req.CHName.ToHalfWidth(),
                ID = req.ID.ToHalfWidth(),
                Sex = req.Sex,
                BirthDay = req.BirthDay.ToHalfWidth(),
                ENName = req.ENName.ToHalfWidth(),
                MarriageState = req.MarriageState,
                ApplicantRelationship = req.ApplicantRelationship,
                CitizenshipCode = req.CitizenshipCode,
                IDIssueDate = req.IDIssueDate.ToHalfWidth(),
                IDCardRenewalLocationCode = req.IDCardRenewalLocationCode,
                IDTakeStatus = req.IDTakeStatus,
                BirthCitizenshipCode = req.BirthCitizenshipCode,
                BirthCitizenshipCodeOther = req.BirthCitizenshipCodeOther,
                IsFATCAIdentity = req.IsFATCAIdentity,
                SocialSecurityCode = req.SocialSecurityCode.ToHalfWidth(),
                IsForeverResidencePermit = req.IsForeverResidencePermit,
                ResidencePermitIssueDate = req.ResidencePermitIssueDate.ToHalfWidth(),
                ResidencePermitDeadline = req.ResidencePermitDeadline.ToHalfWidth(),
                ResidencePermitBackendNum = req.ResidencePermitBackendNum.ToHalfWidth(),
                PassportNo = req.PassportNo.ToHalfWidth(),
                PassportDate = req.PassportDate.ToHalfWidth(),
                ExpatValidityPeriod = req.ExpatValidityPeriod.ToHalfWidth(),
                OldCertificateVerified = req.OldCertificateVerified,
                CompName = req.CompName.ToHalfWidth(),
                CompJobTitle = req.CompJobTitle.ToHalfWidth(),
                CompPhone = req.CompPhone.ToHalfWidth(),
                LivePhone = req.LivePhone.ToHalfWidth(),
                Mobile = req.Mobile.ToHalfWidth(),

                ResidenceType = req.ResidenceType,
                // 居住地址
                Live_ZipCode = req.Live_ZipCode.ToHalfWidth(),
                Live_City = req.Live_City,
                Live_District = req.Live_District,
                Live_Road = req.Live_Road,
                Live_Number = req.Live_Number.ToHalfWidth(),
                Live_SubNumber = req.Live_SubNumber.ToHalfWidth(),
                Live_Floor = req.Live_Floor.ToHalfWidth(),
                Live_Lane = req.Live_Lane.ToHalfWidth(),
                Live_Alley = req.Live_Alley.ToHalfWidth(),
                Live_FullAddr = AddressHelper.將縣市台字轉換為臺字(req.Live_FullAddr.ToHalfWidth()),
                Live_Other = req.Live_Other.ToHalfWidth(),
                // 寄卡地址
                ShippingCardAddressType = req.ShippingCardAddressType,
                SendCard_ZipCode = req.SendCard_ZipCode.ToHalfWidth(),
                SendCard_City = req.SendCard_City,
                SendCard_District = req.SendCard_District,
                SendCard_Road = req.SendCard_Road,
                SendCard_Number = req.SendCard_Number.ToHalfWidth(),
                SendCard_SubNumber = req.SendCard_SubNumber.ToHalfWidth(),
                SendCard_Floor = req.SendCard_Floor.ToHalfWidth(),
                SendCard_Lane = req.SendCard_Lane.ToHalfWidth(),
                SendCard_Alley = req.SendCard_Alley.ToHalfWidth(),
                SendCard_FullAddr = AddressHelper.將縣市台字轉換為臺字(req.SendCard_FullAddr.ToHalfWidth()),
                SendCard_Other = req.SendCard_Other.ToHalfWidth(),

                IsDunyangBlackList = req.IsDunyangBlackList,
                NameCheckedReasonCodes = req.NameCheckedReasonCodes,
                ISRCAForCurrentPEP = req.ISRCAForCurrentPEP,
                ResignPEPKind = req.ResignPEPKind,
                PEPRange = req.PEPRange,
                IsCurrentPositionRelatedPEPPosition = req.IsCurrentPositionRelatedPEPPosition,
            };
        }
    }
}
