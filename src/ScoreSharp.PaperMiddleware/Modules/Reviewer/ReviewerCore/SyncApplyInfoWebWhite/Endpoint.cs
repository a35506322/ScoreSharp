using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ScoreSharp.Common.Extenstions;
using ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyInfoWebWhite;

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 網路件小白同步案件資料 API
        /// </summary>
        /// <param name="request"></param>
        /// <param name="applyNo">申請書編號</param>
        /// <param name="syncUserId">同步員工編號</param>
        /// <returns></returns>
        /// <response code="200">Return Code = 2000 同步成功</response>
        /// <response code="400">
        /// Return Code = 4000 格式驗證失敗
        /// Return Code = 4003 商業邏輯有誤
        /// Return Code = 4001 資料庫定義值錯誤
        /// </response>
        /// <response code="401">Return Code = 4004 標頭驗證失敗</response>
        /// <response code="404">Return Code = 4002 查無此資料</response>
        /// <response code="500">
        /// Return Code = 5000 內部程式失敗
        /// Return Code = 5002 資料庫執行失敗
        /// </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [EndpointSpecificExample(
            typeof(網路件小白同步案件資料_成功_2000_ReqEx),
            typeof(網路件小白同步案件資料_格式驗證失敗_4000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(網路件小白同步案件資料_成功_200_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(網路件小白同步案件資料_格式驗證失敗_400_4000_ResEx),
            typeof(網路件小白同步案件資料_商業邏輯有誤_400_4003_ResEx),
            typeof(網路件小白同步案件資料_資料庫定義值錯誤_400_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [EndpointSpecificExample(
            typeof(網路件小白同步案件資料_標頭驗證失敗_401_4004_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status401Unauthorized
        )]
        [EndpointSpecificExample(
            typeof(網路件小白同步案件資料_查無資料_404_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status404NotFound
        )]
        [EndpointSpecificExample(
            typeof(網路件小白同步案件資料_內部程式失敗_500_5000_ResEx),
            typeof(網路件小白同步案件資料_資料庫執行失敗_500_5002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status500InternalServerError
        )]
        [OpenApiOperation("SyncApplyInfoWebWhite")]
        public async Task<IResult> SyncApplyInfoWebWhite(
            [FromHeader(Name = "X-APPLYNO")] string applyNo,
            [FromHeader(Name = "X-SYNCUSERID")] string syncUserId,
            [FromBody] SyncApplyInfoWebWhiteRequest request
        ) => Results.Ok(await _mediator.Send(new Command(request, ModelState)));
    }
}

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyInfoWebWhite
{
    public record Command(SyncApplyInfoWebWhiteRequest syncApplyInfoWebWhiteRequest, ModelStateDictionary modelState)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext scoreSharpContext, ScoreSharpFileContext scoreSharpFileContext, IHttpContextAccessor httpContextAccessor)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            /*
             * 1. 檢查申請書編號 案件編號為XYZ
             *    檢查 CardStatus是20012 (書面申請等待MyData)、20014 (書面申請等待列印申請書及回郵信封)
             * 2. 根據狀態 變更 下一步 卡片狀態
             *      - 20012 (書面申請等待MyData) => 20014 (書面申請等待列印申請書及回郵信封)
             *      - 20014 (書面申請等待列印申請書及回郵信封) => 30100 (網路件初始_非卡友待檢核)
             * 3. 修改主檔資料 + 修改處理檔資料
             * 4. 紀錄 process (包含對方傳送歷程)
             */

            var req = MapHelper.ToHalfWidthRequest(request.syncApplyInfoWebWhiteRequest);
            var now = DateTime.Now;
            string traceId = Activity.Current?.Id ?? httpContextAccessor.HttpContext?.TraceIdentifier;

            資料驗證格式(request.modelState);
            await 資料庫定義值驗證格式(req);

            var main = await 查詢主檔資料(req.ApplyNo);
            var handle = await 查詢處理檔資料(req.ApplyNo);

            驗證商業邏輯(req, main, handle);

            var cardStatusChangeResult = 轉換卡片狀態(handle);

            try
            {
                修改主檔資料(req, main, now);

                修改處理檔資料(req, handle, cardStatusChangeResult);

                await 產生歷程(req, cardStatusChangeResult, now);

                await scoreSharpContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseExecuteException("修改資料庫資料失敗", ex);
            }

            return ApiResponseHelper.Success(data: req.ApplyNo, message: $"同步成功: {req.ApplyNo}", traceId: traceId);
        }

        private void 資料驗證格式(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                throw new BadRequestException(modelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()));
            }
        }

        private async Task 資料庫定義值驗證格式(SyncApplyInfoWebWhiteRequest request)
        {
            var error = new Dictionary<string, string[]>();

            var paramsEntity = await scoreSharpContext.Procedures.Usp_GetApplyCreditCardInfoWithParamsAsync();
            var filterParams = paramsEntity.Where(x => x.IsActive == "Y");

            // 出生地
            var cityParamsDic = filterParams.Where(x => x.Type == 參數類別.縣市).ToDictionary(x => x.StringValue!, x => x.Name);
            var citizenshipParamsDic = filterParams.Where(x => x.Type == 參數類別.國籍).ToDictionary(x => x.StringValue!, x => x.Name);

            if (request.M_BirthCitizenshipCode == BirthCitizenshipCode.其他)
            {
                if (
                    !citizenshipParamsDic.TryGetValue(request.M_BirthCitizenshipCodeOther ?? "", out var _)
                    && !string.IsNullOrEmpty(request.M_BirthCitizenshipCodeOther)
                )
                {
                    AddOrAppendError(
                        error,
                        nameof(request.M_BirthCitizenshipCodeOther),
                        $"正卡_出生地其他 {request.M_BirthCitizenshipCodeOther} 不存在。"
                    );
                }
            }

            if (!citizenshipParamsDic.TryGetValue(request.M_CitizenshipCode ?? "", out var _) && !string.IsNullOrEmpty(request.M_CitizenshipCode))
            {
                AddOrAppendError(error, nameof(request.M_CitizenshipCode), $"正卡_國籍 {request.M_CitizenshipCode} 不存在。");
            }

            var idCardRenewalLocationParamsDic = filterParams
                .Where(x => x.Type == 參數類別.身分證換發地點)
                .ToDictionary(x => x.StringValue!, x => x.Name);
            if (
                !string.IsNullOrEmpty(request.M_IDCardRenewalLocationCode)
                && !idCardRenewalLocationParamsDic.TryGetValue(request.M_IDCardRenewalLocationCode ?? "", out var _)
            )
            {
                AddOrAppendError(
                    error,
                    nameof(request.M_IDCardRenewalLocationCode),
                    $"正卡_身分證發證地點 {request.M_IDCardRenewalLocationCode} 不存在。"
                );
            }

            var amlProfessionParamsDic = filterParams.Where(x => x.Type == 參數類別.AML職業別).ToDictionary(x => x.StringValue!, x => x.Name);
            if (
                !string.IsNullOrEmpty(request.M_AMLProfessionCode)
                && !amlProfessionParamsDic.TryGetValue(request.M_AMLProfessionCode ?? "", out var _)
            )
            {
                AddOrAppendError(error, nameof(request.M_AMLProfessionCode), $"正卡_AML職業別 {request.M_AMLProfessionCode} 不存在。");
            }

            // 2025.09.23 ECard 沒驗證AML職業別為其他，需必填
            //var AML職業別為其他_代碼 = amlProfessionParamsDic.Where(x => x.Value.Contains("其他")).Select(x => x.Key).FirstOrDefault();
            //if (
            //    string.IsNullOrEmpty(request.M_AMLProfessionOther)
            //    && amlProfessionParamsDic.TryGetValue(request.M_AMLProfessionCode ?? "", out var _)
            //    && request.M_AMLProfessionCode == AML職業別為其他_代碼
            //)
            //{
            //    AddOrAppendError(
            //        error,
            //        nameof(request.M_AMLProfessionOther),
            //        "當「正卡_AML職業別」選擇「其他」時，請填寫「正卡_AML職業別其他」欄位。"
            //    );
            //}

            var amlJobLevelParamsDic = filterParams.Where(x => x.Type == 參數類別.AML職級別).ToDictionary(x => x.StringValue!, x => x.Name);
            if (!string.IsNullOrEmpty(request.M_AMLJobLevelCode) && !amlJobLevelParamsDic.TryGetValue(request.M_AMLJobLevelCode ?? "", out var _))
            {
                AddOrAppendError(error, nameof(request.M_AMLJobLevelCode), $"正卡_AML職級別 {request.M_AMLJobLevelCode} 不存在。");
            }

            var mainIncomeAndFundParamsDic = filterParams.Where(x => x.Type == 參數類別.主要收入來源).ToDictionary(x => x.StringValue!, x => x.Name);
            var 主要收入來源為其他_代碼 = mainIncomeAndFundParamsDic.Where(x => x.Value.Contains("其他")).Select(x => x.Key).FirstOrDefault();
            if (!string.IsNullOrEmpty(request.M_MainIncomeAndFundCodes))
            {
                var mainIncomeAndFundCodes = request.M_MainIncomeAndFundCodes.Split(',');
                foreach (var code in mainIncomeAndFundCodes)
                {
                    if (!mainIncomeAndFundParamsDic.TryGetValue(code, out var _))
                    {
                        AddOrAppendError(error, nameof(request.M_MainIncomeAndFundCodes), $"正卡_所得及資金來源 {code} 不存在。");
                    }
                }
                if (mainIncomeAndFundCodes.Contains(主要收入來源為其他_代碼) && string.IsNullOrEmpty(request.M_MainIncomeAndFundOther))
                {
                    AddOrAppendError(
                        error,
                        nameof(request.M_MainIncomeAndFundOther),
                        "當「正卡_所得及資金來源」選擇「其他」時，請填寫「正卡_所得及資金來源其他」欄位。"
                    );
                }
            }

            // ! 先拔掉不驗證 20251009 目前他們好像根本沒在驗證專案代號的表
            // var projectCodeDic = filterParams.Where(x => x.Type == 參數類別.專案代號).ToDictionary(x => x.StringValue!, x => x.Name);
            // if (!string.IsNullOrEmpty(request.ProjectCode) && !projectCodeDic.TryGetValue(request.ProjectCode ?? "", out var _))
            // {
            //     AddOrAppendError(error, nameof(request.ProjectCode), $"專案代號 {request.ProjectCode} 不存在。");
            // }

            // ! 先拔掉不驗證 20251009 目前他們好像根本沒在驗證推廣單位的表
            // var promotionUnitParamsDic = filterParams.Where(x => x.Type == 參數類別.推廣單位).ToDictionary(x => x.StringValue!, x => x.Name);
            // if (!string.IsNullOrEmpty(request.PromotionUnit) && !promotionUnitParamsDic.TryGetValue(request.PromotionUnit ?? "", out var _))
            // {
            //     AddOrAppendError(error, nameof(request.PromotionUnit), $"推廣單位 {request.PromotionUnit} 不存在。");
            // }

            var cardParamsDic = filterParams.Where(x => x.Type == 參數類別.卡片種類).ToDictionary(x => x.StringValue!, x => x.Name);
            if (request.CardInfo is not null && request.CardInfo.Count() > 0)
            {
                for (int i = 0; i < request.CardInfo.Count(); i++)
                {
                    var card = request.CardInfo[i];
                    bool isValidApplyCardType = cardParamsDic.TryGetValue(card.ApplyCardType, out var _);
                    if (!isValidApplyCardType && !string.IsNullOrEmpty(card.ApplyCardType))
                    {
                        AddOrAppendError(
                            error,
                            $"{nameof(request.CardInfo)}[{i}].{nameof(card.ApplyCardType)}",
                            $"申請卡別 {card.ApplyCardType} 不存在。"
                        );
                    }
                }
            }

            if (error.Count > 0)
            {
                throw new DatabaseDefinitionException(error);
            }
        }

        private void AddOrAppendError(Dictionary<string, string[]> errorDict, string key, string message)
        {
            if (errorDict.ContainsKey(key))
            {
                errorDict[key] = errorDict[key].Concat(new[] { message }).ToArray();
            }
            else
            {
                errorDict[key] = new[] { message };
            }
        }

        private async Task<Reviewer_ApplyCreditCardInfoMain?> 查詢主檔資料(string applyNo) =>
            await scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == applyNo);

        private async Task<Reviewer_ApplyCreditCardInfoHandle?> 查詢處理檔資料(string applyNo) =>
            await scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.FirstOrDefaultAsync(x => x.ApplyNo == applyNo);

        private void 驗證商業邏輯(
            SyncApplyInfoWebWhiteRequest req,
            Reviewer_ApplyCreditCardInfoMain? main,
            Reviewer_ApplyCreditCardInfoHandle? handle
        )
        {
            // 檢查申請書編號 案件編號為XYZ
            var isValidApplyNo = Regex.IsMatch(req.ApplyNo, @"^\d{8}[X-Z]{1}\d{4}$");
            if (!isValidApplyNo)
            {
                throw new BusinessBadRequestException($"申請書編號格式有誤: {req.ApplyNo}");
            }

            if (main is null)
            {
                throw new NotFoundException($"查無申請書編號為{req.ApplyNo}的主檔資料。");
            }

            if (handle is null)
            {
                throw new NotFoundException($"查無申請書編號為{req.ApplyNo}的處理檔資料。");
            }

            // 檢查 CardStatus是20012 (書面申請等待MyData)、20014 (書面申請等待列印申請書及回郵信封)
            var isValidCardStatus =
                handle.CardStatus == CardStatus.網路件_書面申請等待MyData || handle.CardStatus == CardStatus.網路件_書面申請等待列印申請書及回郵信封;
            if (!isValidCardStatus)
            {
                throw new BusinessBadRequestException($"申請書編號 {req.ApplyNo} 的卡片狀態不符合要求: {handle?.CardStatus}");
            }
        }

        private async Task 產生歷程(SyncApplyInfoWebWhiteRequest req, CardStausChangeResult cardStausChangeResult, DateTime now)
        {
            var processes = new List<Reviewer_ApplyCreditCardInfoProcess>();
            processes.AddRange(
                req.ApplyProcess.Select(x => new Reviewer_ApplyCreditCardInfoProcess
                    {
                        ApplyNo = req.ApplyNo,
                        Process = x.Process,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime,
                        ProcessUserId = x.ProcessUserId,
                        Notes = x.Notes,
                    })
                    .ToList()
            );
            var process = new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = req.ApplyNo,
                Process = cardStausChangeResult.AfterCardStatus.ToString(),
                StartTime = now,
                EndTime = now,
                ProcessUserId = req.SyncUserId,
                Notes = "網路件小白同步案件資料",
            };
            processes.Add(process);

            await scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);
        }

        public CardStausChangeResult 轉換卡片狀態(Reviewer_ApplyCreditCardInfoHandle handle)
        {
            CardStausChangeResult result = new CardStausChangeResult();

            result.BeforeCardStatus = handle.CardStatus;

            if (result.BeforeCardStatus == CardStatus.網路件_書面申請等待MyData)
            {
                result.AfterCardStatus = CardStatus.網路件_等待MyData附件;
            }
            else
            {
                result.AfterCardStatus = CardStatus.網路件_非卡友_待檢核;
            }

            return result;
        }

        private void 修改處理檔資料(
            SyncApplyInfoWebWhiteRequest req,
            Reviewer_ApplyCreditCardInfoHandle handle,
            CardStausChangeResult cardStausChangeResult
        )
        {
            var cardInfo = req.CardInfo.FirstOrDefault(x => x.UserType == UserType.正卡人);
            handle.CardStatus = cardStausChangeResult.AfterCardStatus;
            handle.ID = cardInfo.ID;
            handle.ApplyCardType = cardInfo.ApplyCardType;
            handle.ApplyCardKind = cardInfo.ApplyCardKind; // TODO:要再確認申請卡種邏輯
        }

        private void 修改主檔資料(SyncApplyInfoWebWhiteRequest req, Reviewer_ApplyCreditCardInfoMain main, DateTime now)
        {
            // 基本資料
            main.UserType = UserType.正卡人; // 固定為正卡人
            main.CHName = req.M_CHName;
            main.ID = req.M_ID;
            main.Sex = req.M_Sex;
            main.BirthDay = req.M_BirthDay;
            main.ENName = req.M_ENName;
            main.BirthCitizenshipCode = req.M_BirthCitizenshipCode;
            main.BirthCitizenshipCodeOther = req.M_BirthCitizenshipCodeOther;
            main.CitizenshipCode = req.M_CitizenshipCode;
            main.IDIssueDate = req.M_IDIssueDate;
            main.IDCardRenewalLocationCode = req.M_IDCardRenewalLocationCode;
            main.IDTakeStatus = req.M_IDTakeStatus;
            main.Education = req.M_Education;
            main.GraduatedElementarySchool = req.M_GraduatedElementarySchool;

            // 聯絡資料
            main.Mobile = req.M_Mobile;
            main.EMail = req.M_EMail;
            main.HouseRegPhone = req.M_HouseRegPhone;
            main.LivePhone = req.M_LivePhone;

            // 戶籍地址
            main.Reg_ZipCode = req.M_Reg_ZipCode;
            main.Reg_City = req.M_Reg_City;
            main.Reg_District = req.M_Reg_District;
            main.Reg_Road = req.M_Reg_Road;
            main.Reg_Lane = req.M_Reg_Lane;
            main.Reg_Alley = req.M_Reg_Alley;
            main.Reg_Number = req.M_Reg_Number;
            main.Reg_SubNumber = req.M_Reg_SubNumber;
            main.Reg_Floor = req.M_Reg_Floor;
            main.Reg_Other = req.M_Reg_Other;

            // 居住地址
            main.LiveAddressType = req.M_Live_AddressType;
            main.Live_ZipCode = req.M_Live_ZipCode;
            main.Live_City = req.M_Live_City;
            main.Live_District = req.M_Live_District;
            main.Live_Road = req.M_Live_Road;
            main.Live_Lane = req.M_Live_Lane;
            main.Live_Alley = req.M_Live_Alley;
            main.Live_Number = req.M_Live_Number;
            main.Live_SubNumber = req.M_Live_SubNumber;
            main.Live_Floor = req.M_Live_Floor;
            main.Live_Other = req.M_Live_Other;

            // 帳單地址
            main.BillAddressType = req.M_Bill_AddressType;
            main.Bill_ZipCode = req.M_Bill_ZipCode;
            main.Bill_City = req.M_Bill_City;
            main.Bill_District = req.M_Bill_District;
            main.Bill_Road = req.M_Bill_Road;
            main.Bill_Lane = req.M_Bill_Lane;
            main.Bill_Alley = req.M_Bill_Alley;
            main.Bill_Number = req.M_Bill_Number;
            main.Bill_SubNumber = req.M_Bill_SubNumber;
            main.Bill_Floor = req.M_Bill_Floor;
            main.Bill_Other = req.M_Bill_Other;

            // 寄卡地址
            main.SendCardAddressType = req.M_SendCard_AddressType;
            main.SendCard_ZipCode = req.M_SendCard_ZipCode;
            main.SendCard_City = req.M_SendCard_City;
            main.SendCard_District = req.M_SendCard_District;
            main.SendCard_Road = req.M_SendCard_Road;
            main.SendCard_Lane = req.M_SendCard_Lane;
            main.SendCard_Alley = req.M_SendCard_Alley;
            main.SendCard_Number = req.M_SendCard_Number;
            main.SendCard_SubNumber = req.M_SendCard_SubNumber;
            main.SendCard_Floor = req.M_SendCard_Floor;
            main.SendCard_Other = req.M_SendCard_Other;

            // 工作資料
            main.CompName = req.M_CompName;
            main.CompPhone = req.M_CompPhone;
            main.CompID = req.M_CompID;
            main.CompJobTitle = req.M_CompJobTitle;
            main.CompSeniority = req.M_CompSeniority;
            main.CurrentMonthIncome = req.M_CurrentMonthIncome;
            main.AMLProfessionCode = req.M_AMLProfessionCode;
            main.AMLProfessionOther = req.M_AMLProfessionOther;
            main.AMLJobLevelCode = req.M_AMLJobLevelCode;
            main.MainIncomeAndFundCodes = req.M_MainIncomeAndFundCodes;
            main.MainIncomeAndFundOther = req.M_MainIncomeAndFundOther;

            // 公司地址
            main.Comp_ZipCode = req.M_Comp_ZipCode;
            main.Comp_City = req.M_Comp_City;
            main.Comp_District = req.M_Comp_District;
            main.Comp_Road = req.M_Comp_Road;
            main.Comp_Lane = req.M_Comp_Lane;
            main.Comp_Alley = req.M_Comp_Alley;
            main.Comp_Number = req.M_Comp_Number;
            main.Comp_SubNumber = req.M_Comp_SubNumber;
            main.Comp_Floor = req.M_Comp_Floor;
            main.Comp_Other = req.M_Comp_Other;

            // 同意條款相關
            main.IsAgreeDataOpen = req.M_IsAgreeDataOpen;
            main.IsAgreeMarketing = req.IsAgreeMarketing;
            main.IsAcceptEasyCardDefaultBonus = req.M_IsAcceptEasyCardDefaultBonus;

            // 其他設定
            main.CardOwner = req.CardOwner;
            main.PromotionUnit = req.PromotionUnit;
            main.PromotionUser = req.PromotionUser;
            main.BillType = req.BillType;
            main.LiveOwner = req.LiveOwner;
            main.AnliNo = req.AnliNo;
            main.FirstBrushingGiftCode = req.FirstBrushingGiftCode;
            main.ProjectCode = req.ProjectCode;

            // 更新時間和更新人員
            main.LastUpdateTime = now;
            main.LastUpdateUserId = UserIdConst.SYSTEM;
        }
    }
}
