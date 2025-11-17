using ScoreSharp.Common.Extenstions;
using ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyInfoPaper;

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 紙本建檔同步案件
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
        [OpenApiOperation("SyncApplyInfoPaper")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [EndpointSpecificExample(
            typeof(紙本同步案件資料_成功_200_2000_ReqEx),
            typeof(紙本同步案件資料_格式驗證失敗_400_4000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(紙本同步案件資料_成功_200_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(紙本同步案件資料_格式驗證失敗_400_4000_ResEx),
            typeof(紙本同步案件資料_商業邏輯有誤_400_4003_ResEx),
            typeof(紙本同步案件資料_資料庫定義值錯誤_400_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [EndpointSpecificExample(
            typeof(紙本同步案件資料_標頭驗證失敗_401_4004_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status401Unauthorized
        )]
        [EndpointSpecificExample(
            typeof(紙本同步案件資料_查無資料_404_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status404NotFound
        )]
        [EndpointSpecificExample(
            typeof(紙本同步案件資料_內部程式失敗_500_5000_ResEx),
            typeof(紙本同步案件資料_資料庫執行失敗_500_5002_ResEx),
            typeof(紙本同步案件資料_查無對應版本號_500_5000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status500InternalServerError
        )]
        public async Task<IResult> SyncApplyInfoPaper(
            [FromHeader(Name = "X-APPLYNO")] string applyNo,
            [FromHeader(Name = "X-SYNCUSERID")] string syncUserId,
            [FromBody] SyncApplyInfoPaperRequest request
        )
        {
            var result = await _mediator.Send(new Command(request, ModelState));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyInfoPaper
{
    public record Command(SyncApplyInfoPaperRequest syncApplyInfoPaperRequest, ModelStateDictionary modelState) : IRequest<ResultResponse<string>>;

    public class Handler(
        ILogger<Handler> logger,
        ScoreSharpContext scoreSharpContext,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration _configuration
    ) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            /*
             *  1. 格式檢查 ✅
             *      - 格式驗證
             *      - 資料庫定義值
             *  2. 驗證資料是否存在 ✅
             *      - Main 是否存在
             *      - Handle 是否存在
             *  3. 商業邏輯驗證 ✅
             *      - Handle 驗證狀態為約定狀態
             *  3. Map資料 ✅
             *      根據Request
             *      - Handle ⇒ 先全部刪除再新增，同步狀態為完成時，更改狀態為紙本件_待審核
             *      - Supplementary ⇒ 根據CardOwner檢查是否有附卡人+原先是否有附卡人資料來判斷新增或修改
             *  4. 增加同步歷程 ✅
             *      - 比對給的歷程去新增或修改歷程
             *      - 根據同步狀態 - 完成比修改多一條 : 更改狀態為紙本件_待審核
             *  5. 資料處理 ✅
             *      - 同步狀態為完成時，更改所有Handle的CardStatus為紙本件_待審核(20100)，並增加紙本件_待審核的Log
             *  6. 資料庫處理 ✅
             *      - Main 覆蓋正卡人資料
             *      - 清除舊資料並新增新資料
             */

            var req = MapHelper.ToHalfWidthRequest(request.syncApplyInfoPaperRequest);
            string traceId = Activity.Current?.Id ?? httpContextAccessor.HttpContext?.TraceIdentifier;

            /* ➀ 資料驗證 */
            ValidateModelState(request.modelState);

            var (handle, main, persistedProcesses) = await GetExistingData(req.ApplyNo);

            /* ➁ 驗證資料是否存在 */
            ValidateAMLNotFoundData(main, req.ApplyNo);

            string currentCaseVersion = main.AMLProfessionCode_Version;
            var validationCodes = GetValidationCodes(currentCaseVersion);
            if (validationCodes.AMLProfessionOtherCode is null)
            {
                throw new InternalServerException("查無對應版本號的 AML 職業別「其他」代碼。");
            }
            await ValidateDbDefinedValues(req, currentCaseVersion, validationCodes);

            /* ➂ 商業邏輯驗證 */
            ValidateBusinessLogic(req, handle);

            /* ➃ 處理資料 */
            var (newHandleDataList, updatedSupplementaryData) = MapData(req);
            if (req.SyncStatus == SyncStatus.完成)
            {
                foreach (var item in newHandleDataList)
                {
                    item.CardStatus = CardStatus.紙本件_待檢核;
                }
            }

            /* ➄ 增加同步歷程 */
            var pendingProcesses = GetProcessesForAdd(persistedProcesses, req);

            /* ➅ 處理 同步狀態 = 完成 */
            DateTime now = DateTime.Now;
            if (req.SyncStatus == SyncStatus.完成)
            {
                var process = new Reviewer_ApplyCreditCardInfoProcess()
                {
                    ApplyNo = req.ApplyNo,
                    Process = CardStatus.紙本件_待檢核.ToString(),
                    StartTime = now,
                    EndTime = now,
                    Notes = "紙本同步完成",
                    ProcessUserId = UserIdConst.SYSTEM,
                };
                pendingProcesses.Add(process);
            }

            /* ➆ 資料庫處理 */
            // 修改正卡人資料
            MapToMainForUpdate(main, req, now);

            // 同步狀態為完成時，準備待檢核相關資訊
            if (req.SyncStatus == SyncStatus.完成)
            {
                var systemParam = await scoreSharpContext.SysParamManage_SysParam.AsNoTracking().FirstOrDefaultAsync();
                var (outsideBankInfos, notes, communicate, bankTraces, financeCheckInfos, checkJob) = PrepareApplyCaseEntity(req, systemParam);
                await scoreSharpContext.Reviewer_OutsideBankInfo.AddRangeAsync(outsideBankInfos);
                await scoreSharpContext.Reviewer_ApplyNote.AddRangeAsync(notes);
                await scoreSharpContext.Reviewer_InternalCommunicate.AddAsync(communicate);
                await scoreSharpContext.Reviewer_BankTrace.AddRangeAsync(bankTraces);
                await scoreSharpContext.Reviewer_FinanceCheckInfo.AddRangeAsync(financeCheckInfos);
                await scoreSharpContext.ReviewerPedding_PaperApplyCardCheckJob.AddAsync(checkJob);
            }

            // 清除舊資料並新增新資料
            if (handle is not null)
                await scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == req.ApplyNo).ExecuteDeleteAsync();

            await scoreSharpContext.Reviewer_ApplyCreditCardInfoSupplementary.Where(x => x.ApplyNo == req.ApplyNo).ExecuteDeleteAsync();

            if (newHandleDataList.Any())
                await scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.AddRangeAsync(newHandleDataList);

            if (updatedSupplementaryData is not null)
                await scoreSharpContext.Reviewer_ApplyCreditCardInfoSupplementary.AddAsync(updatedSupplementaryData);

            if (pendingProcesses.Any())
                await scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(pendingProcesses);

            await scoreSharpContext.SaveChangesAsync();

            return ApiResponseHelper.Success(data: req.ApplyNo, message: $"同步成功: {req.ApplyNo}", traceId: traceId);
        }

        private void ValidateModelState(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                throw new BadRequestException(modelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()));
            }
        }

        private async Task ValidateDbDefinedValues(SyncApplyInfoPaperRequest request, string currentCaseVersion, FieldValidationCodes validationCodes)
        {
            var error = new Dictionary<string, string[]>();

            var paramsEntity = await scoreSharpContext.Procedures.Usp_GetApplyCreditCardInfoWithParamsAsync();
            var filterParams = paramsEntity.Where(x => x.IsActive == "Y");

            var cityParamsDic = filterParams.Where(x => x.Type == 參數類別.縣市).ToDictionary(x => x.StringValue!, x => x.Name);
            var citizenshipParamsDic = filterParams.Where(x => x.Type == 參數類別.國籍).ToDictionary(x => x.StringValue!, x => x.Name);

            if (!string.IsNullOrEmpty(request.M_BirthCitizenshipCodeOther) && request.M_BirthCitizenshipCode == BirthCitizenshipCode.其他)
            {
                var isValid = citizenshipParamsDic.TryGetValue(request.M_BirthCitizenshipCodeOther ?? "", out var _);
                if (!isValid)
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

            var amlProfessionParamsDic = await scoreSharpContext
                .SetUp_AMLProfession.AsNoTracking()
                .Where(x => x.Version == currentCaseVersion && x.IsActive == "Y")
                .ToDictionaryAsync(x => x.AMLProfessionCode!, x => x.AMLProfessionName);

            if (
                !string.IsNullOrEmpty(request.M_AMLProfessionCode)
                && !amlProfessionParamsDic.TryGetValue(request.M_AMLProfessionCode ?? "", out var _)
            )
            {
                AddOrAppendError(error, nameof(request.M_AMLProfessionCode), $"正卡_AML職業別 {request.M_AMLProfessionCode} 不存在。");
            }

            // ! 先拔掉不驗證 20251030 目前他們好像根本沒在驗AML職業別其他
            //if (
            //    string.IsNullOrEmpty(request.M_AMLProfessionOther)
            //    && amlProfessionParamsDic.TryGetValue(request.M_AMLProfessionCode ?? "", out var _)
            //    && request.M_AMLProfessionCode == validationCodes.AMLProfessionOtherCode
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
            if (!string.IsNullOrEmpty(request.M_MainIncomeAndFundCodes))
            {
                var mainIncomeAndFundCodes = request.M_MainIncomeAndFundCodes.Split(',');
                foreach (var code in mainIncomeAndFundCodes)
                {
                    if (!mainIncomeAndFundParamsDic.TryGetValue(code ?? "", out var _))
                    {
                        AddOrAppendError(error, nameof(request.M_MainIncomeAndFundCodes), $"正卡_所得及資金來源 {code} 不存在。");
                    }
                }
                if (
                    mainIncomeAndFundCodes.Contains(validationCodes.MainIncomeAndFundOtherCode)
                    && string.IsNullOrEmpty(request.M_MainIncomeAndFundOther)
                )
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

            if (
                !string.IsNullOrEmpty(request.S1_IDCardRenewalLocationCode)
                && !idCardRenewalLocationParamsDic.TryGetValue(request.S1_IDCardRenewalLocationCode ?? "", out var _)
            )
            {
                AddOrAppendError(
                    error,
                    nameof(request.S1_IDCardRenewalLocationCode),
                    $"附卡1_身分證發證地點 {request.S1_IDCardRenewalLocationCode} 不存在。"
                );
            }

            if (!string.IsNullOrEmpty(request.S1_BirthCitizenshipCodeOther) && request.S1_BirthCitizenshipCode == BirthCitizenshipCode.其他)
            {
                var isValid = citizenshipParamsDic.TryGetValue(request.S1_BirthCitizenshipCodeOther ?? "", out var _);
                if (!isValid)
                {
                    AddOrAppendError(
                        error,
                        nameof(request.S1_BirthCitizenshipCodeOther),
                        $"附卡1_出生地其他 {request.S1_BirthCitizenshipCodeOther} 不存在。"
                    );
                }
            }

            if (!citizenshipParamsDic.TryGetValue(request.S1_CitizenshipCode ?? "", out var _) && !string.IsNullOrEmpty(request.S1_CitizenshipCode))
            {
                AddOrAppendError(error, nameof(request.S1_CitizenshipCode), $"附卡1_國籍 {request.S1_CitizenshipCode} 不存在。");
            }

            var cardParamsDic = filterParams.Where(x => x.Type == 參數類別.卡片種類).ToDictionary(x => x.StringValue!, x => x.Name);
            if (request.CardInfo is not null && request.CardInfo.Count() > 0)
            {
                for (int i = 0; i < request.CardInfo.Count(); i++)
                {
                    var card = request.CardInfo[i];
                    bool isValidApplyCardType = cardParamsDic.TryGetValue(card.ApplyCardType ?? "", out var _);
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

            // 年費收取方式
            var annualFeeTypeParamsDic = filterParams.Where(x => x.Type == 參數類別.年費收取方式).ToDictionary(x => x.StringValue!, x => x.Name);
            if (
                !string.IsNullOrEmpty(request.AnnualFeePaymentType)
                && !annualFeeTypeParamsDic.TryGetValue(request.AnnualFeePaymentType ?? "", out var _)
            )
            {
                AddOrAppendError(error, nameof(request.AnnualFeePaymentType), $"年費收取方式 {request.AnnualFeePaymentType} 不存在。");
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

        private async Task<Reviewer_ApplyCreditCardInfoMain?> GetMainData(string applyNo) =>
            await scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == applyNo);

        /// <summary>
        /// 為確認目前紙本案件狀態，Handle案件狀態皆相同，所以取得一筆即可
        /// </summary>
        /// <param name="applyNo"></param>
        /// <returns></returns>
        private async Task<Reviewer_ApplyCreditCardInfoHandle?> GetHandleData(string applyNo) =>
            await scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.AsNoTracking().FirstOrDefaultAsync(x => x.ApplyNo == applyNo);

        private async Task<List<Reviewer_ApplyCreditCardInfoProcess>?> GetProcess(string applyNo) =>
            await scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AsNoTracking().Where(x => x.ApplyNo == applyNo).ToListAsync();

        private void MapToMainForUpdate(Reviewer_ApplyCreditCardInfoMain main, SyncApplyInfoPaperRequest request, DateTime now)
        {
            main.CardOwner = request.CardOwner;
            main.CHName = request.M_CHName;
            main.ID = request.M_ID;
            main.IDIssueDate = request.M_IDIssueDate;
            main.IDCardRenewalLocationCode = request.M_IDCardRenewalLocationCode;
            main.IDTakeStatus = request.M_IDTakeStatus;
            main.Sex = request.M_Sex;
            main.MarriageState = request.M_MarriageState;
            main.ChildrenCount = request.M_ChildrenCount;
            main.BirthDay = request.M_BirthDay;
            main.ENName = request.M_ENName;
            main.BirthCitizenshipCode = request.M_BirthCitizenshipCode;
            main.BirthCitizenshipCodeOther = request.M_BirthCitizenshipCodeOther;
            main.CitizenshipCode = request.M_CitizenshipCode;
            main.Education = request.M_Education;
            main.GraduatedElementarySchool = request.M_GraduatedElementarySchool;

            // 戶籍地址
            main.Reg_ZipCode = request.M_Reg_ZipCode;
            main.Reg_City = request.M_Reg_City;
            main.Reg_District = request.M_Reg_District;
            main.Reg_Road = request.M_Reg_Road;
            main.Reg_Lane = request.M_Reg_Lane;
            main.Reg_Alley = request.M_Reg_Alley;
            main.Reg_Number = request.M_Reg_Number;
            main.Reg_SubNumber = request.M_Reg_SubNumber;
            main.Reg_Floor = request.M_Reg_Floor;
            main.Reg_Other = request.M_Reg_Other;

            // 居住地址
            main.LiveAddressType = request.M_Live_AddressType;
            main.Live_ZipCode = request.M_Live_ZipCode;
            main.Live_City = request.M_Live_City;
            main.Live_District = request.M_Live_District;
            main.Live_Road = request.M_Live_Road;
            main.Live_Lane = request.M_Live_Lane;
            main.Live_Alley = request.M_Live_Alley;
            main.Live_Number = request.M_Live_Number;
            main.Live_SubNumber = request.M_Live_SubNumber;
            main.Live_Floor = request.M_Live_Floor;
            main.Live_Other = request.M_Live_Other;

            // 帳單地址
            main.BillAddressType = request.M_Bill_AddressType;
            main.Bill_ZipCode = request.M_Bill_ZipCode;
            main.Bill_City = request.M_Bill_City;
            main.Bill_District = request.M_Bill_District;
            main.Bill_Road = request.M_Bill_Road;
            main.Bill_Lane = request.M_Bill_Lane;
            main.Bill_Alley = request.M_Bill_Alley;
            main.Bill_Number = request.M_Bill_Number;
            main.Bill_SubNumber = request.M_Bill_SubNumber;
            main.Bill_Floor = request.M_Bill_Floor;
            main.Bill_Other = request.M_Bill_Other;

            // 寄卡地址
            main.SendCardAddressType = request.M_SendCard_AddressType;
            main.SendCard_ZipCode = request.M_SendCard_ZipCode;
            main.SendCard_City = request.M_SendCard_City;
            main.SendCard_District = request.M_SendCard_District;
            main.SendCard_Road = request.M_SendCard_Road;
            main.SendCard_Lane = request.M_SendCard_Lane;
            main.SendCard_Alley = request.M_SendCard_Alley;
            main.SendCard_Number = request.M_SendCard_Number;
            main.SendCard_SubNumber = request.M_SendCard_SubNumber;
            main.SendCard_Floor = request.M_SendCard_Floor;
            main.SendCard_Other = request.M_SendCard_Other;

            main.HouseRegPhone = request.M_HouseRegPhone;
            main.LivePhone = request.M_LivePhone;
            main.Mobile = request.M_Mobile;
            main.LiveOwner = request.M_LiveOwner;
            main.LiveYear = request.M_LiveYear;
            main.EMail = request.M_EMail;
            main.CompName = request.M_CompName;
            main.CompTrade = request.M_CompTrade;
            main.AMLProfessionCode = request.M_AMLProfessionCode;
            main.AMLProfessionOther = request.M_AMLProfessionOther;
            main.AMLJobLevelCode = request.M_AMLJobLevelCode;
            main.CompJobLevel = request.M_CompJobLevel;

            // 公司地址
            main.Comp_ZipCode = request.M_Comp_ZipCode;
            main.Comp_City = request.M_Comp_City;
            main.Comp_District = request.M_Comp_District;
            main.Comp_Road = request.M_Comp_Road;
            main.Comp_Lane = request.M_Comp_Lane;
            main.Comp_Alley = request.M_Comp_Alley;
            main.Comp_Number = request.M_Comp_Number;
            main.Comp_SubNumber = request.M_Comp_SubNumber;
            main.Comp_Floor = request.M_Comp_Floor;
            main.Comp_Other = request.M_Comp_Other;

            main.CompPhone = request.M_CompPhone;
            main.CompID = request.M_CompID;
            main.CompJobTitle = request.M_CompJobTitle;
            main.DepartmentName = request.M_DepartmentName;
            main.CurrentMonthIncome = request.M_CurrentMonthIncome;
            main.EmploymentDate = request.M_EmploymentDate;
            main.CompSeniority = request.M_CompSeniority;

            main.IsConvertCard = request.M_ReissuedCardType;
            main.IsAgreeDataOpen = request.M_IsAgreeDataOpen;
            main.MainIncomeAndFundCodes = request.M_MainIncomeAndFundCodes;
            main.MainIncomeAndFundOther = request.M_MainIncomeAndFundOther;

            main.ElecCodeId = request.ElecCodeId;
            main.IsHoldingBankCard = request.M_IsHoldingBankCard;

            main.FirstBrushingGiftCode = request.FirstBrushingGiftCode;
            main.AnnualFeePaymentType = request.AnnualFeePaymentType;
            main.IsAcceptEasyCardDefaultBonus = request.M_IsAcceptEasyCardDefaultBonus;
            main.IsAgreeMarketing = request.IsAgreeMarketing;
            main.BillType = request.BillType;
            main.ProjectCode = request.ProjectCode;
            main.PromotionUnit = request.PromotionUnit;
            main.PromotionUser = request.PromotionUser;
            main.AcceptType = request.AcceptType;
            main.AnliNo = request.AnliNo;

            main.LastUpdateUserId = UserIdConst.SYSTEM;
            main.LastUpdateTime = now;
        }

        private Reviewer_ApplyCreditCardInfoSupplementary MapToSupplementaryForAdd(SyncApplyInfoPaperRequest request)
        {
            var supplementary = new Reviewer_ApplyCreditCardInfoSupplementary();
            supplementary.ApplyNo = request.ApplyNo;
            supplementary.UserType = UserType.附卡人;

            supplementary.ApplicantRelationship = request.S1_ApplicantRelationship;
            supplementary.CHName = request.S1_CHName;
            supplementary.ID = request.S1_ID;
            supplementary.IDIssueDate = request.S1_IDIssueDate;
            supplementary.IDCardRenewalLocationCode = request.S1_IDCardRenewalLocationCode;
            supplementary.IDTakeStatus = request.S1_IDTakeStatus;
            supplementary.Sex = request.S1_Sex;
            supplementary.MarriageState = request.S1_MarriageState;
            supplementary.BirthDay = request.S1_BirthDay;
            supplementary.ENName = request.S1_ENName;
            supplementary.BirthCitizenshipCode = request.S1_BirthCitizenshipCode;
            supplementary.BirthCitizenshipCodeOther = request.S1_BirthCitizenshipCodeOther;
            supplementary.CitizenshipCode = request.S1_CitizenshipCode;

            // 居住地址
            supplementary.ResidenceType = request.S1_Live_AddressType;
            supplementary.Live_ZipCode = request.S1_Live_ZipCode;
            supplementary.Live_City = request.S1_Live_City;
            supplementary.Live_District = request.S1_Live_District;
            supplementary.Live_Road = request.S1_Live_Road;
            supplementary.Live_Lane = request.S1_Live_Lane;
            supplementary.Live_Alley = request.S1_Live_Alley;
            supplementary.Live_Number = request.S1_Live_Number;
            supplementary.Live_SubNumber = request.S1_Live_SubNumber;
            supplementary.Live_Floor = request.S1_Live_Floor;
            supplementary.Live_Other = request.S1_Live_Other;

            // 寄卡地址
            supplementary.ShippingCardAddressType = request.S1_SendCard_AddressType;
            supplementary.SendCard_ZipCode = request.S1_SendCard_ZipCode;
            supplementary.SendCard_City = request.S1_SendCard_City;
            supplementary.SendCard_District = request.S1_SendCard_District;
            supplementary.SendCard_Road = request.S1_SendCard_Road;
            supplementary.SendCard_Lane = request.S1_SendCard_Lane;
            supplementary.SendCard_Alley = request.S1_SendCard_Alley;
            supplementary.SendCard_Number = request.S1_SendCard_Number;
            supplementary.SendCard_SubNumber = request.S1_SendCard_SubNumber;
            supplementary.SendCard_Floor = request.S1_SendCard_Floor;
            supplementary.SendCard_Other = request.S1_SendCard_Other;

            supplementary.LivePhone = request.S1_LivePhone;
            supplementary.Mobile = request.S1_Mobile;
            supplementary.CompPhone = request.S1_CompPhone;
            supplementary.CompName = request.S1_CompName;
            supplementary.CompJobTitle = request.S1_CompJobTitle;

            return supplementary;
        }

        private List<Reviewer_ApplyCreditCardInfoHandle> MapToHandleForAdd(SyncApplyInfoPaperRequest request)
        {
            List<Reviewer_ApplyCreditCardInfoHandle> handles = new();

            var 卡片資訊 = request.CardInfo;

            foreach (var card in 卡片資訊)
            {
                var handle = new Reviewer_ApplyCreditCardInfoHandle
                {
                    SeqNo = Ulid.NewUlid().ToString(),
                    ApplyNo = request.ApplyNo,
                    ID = card.ID,
                    UserType = card.UserType,
                    CardStatus = card.CardStatus,
                    ApplyCardType = card.ApplyCardType,
                    ApplyCardKind = card.ApplyCardKind,
                };
                handles.Add(handle);
            }

            return handles;
        }

        private List<Reviewer_ApplyCreditCardInfoProcess> GetProcessesForAdd(
            List<Reviewer_ApplyCreditCardInfoProcess> systemProcess,
            SyncApplyInfoPaperRequest req
        )
        {
            var newProcesses = new List<Reviewer_ApplyCreditCardInfoProcess>();

            foreach (var applyProcess in req.ApplyProcess)
            {
                var process = applyProcess.Process;
                if (!systemProcess.Any(x => x.Process == process))
                {
                    var newProcess = new Reviewer_ApplyCreditCardInfoProcess
                    {
                        ApplyNo = req.ApplyNo,
                        Process = process,
                        StartTime = applyProcess.StartTime,
                        EndTime = applyProcess.EndTime,
                        Notes = $"{applyProcess.Notes}(紙本_{req.SyncStatus})",
                        ProcessUserId = applyProcess.ProcessUserId,
                    };
                    newProcesses.Add(newProcess);
                }
            }

            return newProcesses;
        }

        private async Task<(
            Reviewer_ApplyCreditCardInfoHandle?,
            Reviewer_ApplyCreditCardInfoMain?,
            List<Reviewer_ApplyCreditCardInfoProcess>?
        )> GetExistingData(string applyNo)
        {
            var handle = await GetHandleData(applyNo);
            var main = await GetMainData(applyNo);

            var process = await GetProcess(applyNo);

            return (handle, main, process);
        }

        private (List<Reviewer_ApplyCreditCardInfoHandle>?, Reviewer_ApplyCreditCardInfoSupplementary?) MapData(SyncApplyInfoPaperRequest req)
        {
            // 處理附卡人資料
            var updatedSupplementaryData = new Reviewer_ApplyCreditCardInfoSupplementary();
            if (req.CardOwner == CardOwner.正卡)
            {
                updatedSupplementaryData = null!;
            }
            else
            {
                updatedSupplementaryData = MapToSupplementaryForAdd(req);
            }

            // 處理處理檔資料
            var newHandleDataList = MapToHandleForAdd(req);

            return (newHandleDataList, updatedSupplementaryData);
        }

        private void ValidateBusinessLogic(SyncApplyInfoPaperRequest request, Reviewer_ApplyCreditCardInfoHandle handle)
        {
            // handle
            var allowedCardStatus = new[]
            {
                CardStatus.紙本件_初始,
                CardStatus.紙本件_一次件檔中,
                CardStatus.紙本件_二次件檔中,
                CardStatus.紙本件_建檔審核中,
            };

            if (handle is not null && !allowedCardStatus.Contains(handle.CardStatus))
                throw new BusinessBadRequestException($"申請書編號:{request.ApplyNo}卡片狀態不在範圍內請檢查。");
        }

        private (
            List<Reviewer_OutsideBankInfo>,
            List<Reviewer_ApplyNote>,
            Reviewer_InternalCommunicate,
            List<Reviewer_BankTrace>,
            List<Reviewer_FinanceCheckInfo>,
            ReviewerPedding_PaperApplyCardCheckJob
        ) PrepareApplyCaseEntity(SyncApplyInfoPaperRequest request, SysParamManage_SysParam systemParam)
        {
            var outsideBankInfos = MapHelper.MapToOutsideBankInfo(request);
            var notes = MapHelper.MapToNote(request);
            var communicate = MapHelper.MapToCommunicate(request);
            var bankTraces = MapHelper.MarpToBankTrace(request);
            var financeCheckInfos = MapHelper.MapToFinance(request, systemParam.KYC_StrongReVersion);
            var checkJob = MapHelper.MapToCheckJob(request);

            return (outsideBankInfos, notes, communicate, bankTraces, financeCheckInfos, checkJob);
        }

        private void ValidateAMLNotFoundData(Reviewer_ApplyCreditCardInfoMain main, string applyNo)
        {
            if (main is null)
                throw new NotFoundException($"查無申請書編號為{applyNo}的主檔資料。");
        }

        private FieldValidationCodes GetValidationCodes(string currentVersion)
        {
            return new FieldValidationCodes
            {
                AMLProfessionOtherCode = _configuration.GetSection($"ValidationSetting:AMLProfessionOther_{currentVersion}").Value,
                MainIncomeAndFundOtherCode = _configuration.GetSection("ValidationSetting:MainIncomeAndFundOther").Value,
            };
        }
    }
}
