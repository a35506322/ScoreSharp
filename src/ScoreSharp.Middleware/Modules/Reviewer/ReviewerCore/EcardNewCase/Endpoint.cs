using Microsoft.Identity.Client;
using ScoreSharp.Common.Extenstions._Enum;
using ScoreSharp.Common.Helpers;
using ScoreSharp.Middleware.Infrastructures.FileData;
using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase;
using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Helpers;
using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;
using ScoreSharp.Watermark;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// ECARD進件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EcardNewCaseResponse))]
        [EndpointSpecificExample(
            typeof(ECARD進件非卡友存戶_0000_ReqEx),
            typeof(ECARD進件非卡友非存戶_0000_ReqEx),
            typeof(ECARD進件卡友_0000_ReqEx),
            typeof(ECARD進件申請書編號長度不符_0001_ReqEx),
            typeof(ECARD進件申請書編號重複進件或申請書編號不對_0003_ReqEx),
            typeof(ECARD進件資料異常非定義值_0005_ReqEx),
            typeof(ECARD進件必要欄位不能為空值_0007_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(ECARD進件匯入成功_0000_ResEx),
            typeof(ECARD進件申請書編號長度不符_0001_ResEx),
            typeof(ECARD進件申請書編號重複進件或申請書編號不對_0003_ResEx),
            typeof(ECARD進件資料異常非定義值_0005_ResEx),
            typeof(ECARD進件必要欄位不能為空值_0007_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("EcardNewCase")]
        public async Task<IResult> EcardNewCase([FromBody] EcardNewCaseRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase
{
    public record Command(EcardNewCaseRequest ecardNewCaseRequest) : IRequest<EcardNewCaseResponse>;

    public class Handler : IRequestHandler<Command, EcardNewCaseResponse>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly ScoreSharpContext _scoreSharpContext;
        private readonly IScoreSharpDapperContext _scoreSharpDapperContext;
        private readonly IWatermarkHelper _watermarkHelper;
        private readonly IWebHostEnvironment _env;
        private readonly string BirthPlace其它固定值 = "其它";
        public readonly IConfiguration _configuration;
        private readonly IFusionCache _cache;
        private readonly ScoreSharpFileContext _scoreSharpFileContext;

        public Handler(
            ILogger<Handler> logger,
            IDiagnosticContext diagnosticContext,
            ScoreSharpContext scoreSharpContext,
            IScoreSharpDapperContext scoreSharpDapperContext,
            IWatermarkHelper watermarkHelper,
            IWebHostEnvironment env,
            IConfiguration configuration,
            IFusionCache cache,
            ScoreSharpFileContext scoreSharpFileContext
        )
        {
            _logger = logger;
            _diagnosticContext = diagnosticContext;
            _scoreSharpContext = scoreSharpContext;
            _scoreSharpDapperContext = scoreSharpDapperContext;
            _watermarkHelper = watermarkHelper;
            _env = env;
            _configuration = configuration;
            _cache = cache;
            _scoreSharpFileContext = scoreSharpFileContext;
        }

        public async Task<EcardNewCaseResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            EcardNewCaseRequest req = MapHelper.ToHalfWidthRequest(request.ecardNewCaseRequest);

            using var _ = _logger.PushProperties(("ApplyNo", req.ApplyNo));
            _diagnosticContext.SetProperties(("ApplyNo", req.ApplyNo, false));

            try
            {
                bool isCITSCard = await 檢查_是否為國旅卡(req.ApplyCardType);
                CaseContext caseContext = MapHelper.MapToCaseContext(req, isCITSCard);

                var originParams = await 查詢_參數();

                if (!VerifyHelper.檢查_申請書編號是否長度為13(req.ApplyNo))
                {
                    _logger.LogError("申請書編號: {@ApplyNo} 申請書編號長度不符 13", req.ApplyNo);
                    var errorNotice = MapHelper.MapToErrorNotice(
                        req.ApplyNo,
                        SystemErrorLogTypeConst.申請書資料驗證失敗,
                        $"申請書編號長度不符 13:{req.ApplyNo}",
                        req
                    );
                    await 發送錯誤通知_後續不在執行(errorNotice);
                    return new EcardNewCaseResponse(回覆代碼.申請書編號長度不符);
                }

                if (await 檢查_申請書編號是否重複進件(req.ApplyNo))
                {
                    _logger.LogError("申請書編號: {@ApplyNo} 申請書編號重複進件", req.ApplyNo);
                    var errorNotice = MapHelper.MapToErrorNotice(
                        req.ApplyNo,
                        SystemErrorLogTypeConst.申請書資料驗證失敗,
                        $"申請書編號重複進件:{req.ApplyNo}",
                        req
                    );
                    await 發送錯誤通知_後續不在執行(errorNotice);
                    return new EcardNewCaseResponse(回覆代碼.申請書編號重複進件或申請書編號不對);
                }

                if (!VerifyHelper.檢查_申請書編號格式是否正確(req.ApplyNo))
                {
                    _logger.LogError("申請書編號: {@ApplyNo} 申請書編號格式不對", req.ApplyNo);
                    var errorNotice = MapHelper.MapToErrorNotice(
                        req.ApplyNo,
                        SystemErrorLogTypeConst.申請書資料驗證失敗,
                        $"申請書編號格式不對:{req.ApplyNo}",
                        req
                    );

                    await 發送錯誤通知_後續不在執行(errorNotice);
                    return new EcardNewCaseResponse(回覆代碼.申請書編號重複進件或申請書編號不對);
                }

                var cardParams = originParams.Where(x => x.IsActive == "Y" && x.Type == 參數類別.卡片種類).ToList();
                if (!檢查_卡片代碼是否正確(req.ApplyCardType, cardParams))
                {
                    _logger.LogError("申請書編號: {@ApplyNo} 卡片代碼不在定義值內:{@ApplyCardType}", req.ApplyNo, req.ApplyCardType);
                    var errorNotice = MapHelper.MapToErrorNotice(
                        req.ApplyNo,
                        SystemErrorLogTypeConst.申請書資料驗證失敗,
                        $"卡片代碼不在定義值內:{req.ApplyCardType}",
                        req
                    );

                    await 發送錯誤通知_後續不在執行(errorNotice);
                    return new EcardNewCaseResponse(回覆代碼.無法對應卡片代碼);
                }

                if (await 檢查_LineBankUUID是否重複(req.LineBankUUID))
                {
                    _logger.LogError("申請書編號: {@ApplyNo} LineBankUUID重複:{@LineBankUUID}", req.ApplyNo, req.LineBankUUID);
                    var errorNotice = MapHelper.MapToErrorNotice(
                        req.ApplyNo,
                        SystemErrorLogTypeConst.申請書資料驗證失敗,
                        $"LineBankUUID重複:{req.LineBankUUID}",
                        req
                    );

                    await 發送錯誤通知_後續不在執行(errorNotice);
                    return new EcardNewCaseResponse(回覆代碼.UUID重複);
                }

                CheckRequiredResult checkRequiredResult = VerifyHelper.檢查_必要欄位不能為空值(
                    req.ApplyNo,
                    req.CardOwner,
                    req.ApplyCardType,
                    req.FormCode,
                    req.CHName,
                    req.Birthday,
                    req.ID,
                    req.Source
                );

                if (!checkRequiredResult.IsValid)
                {
                    _logger.LogError("申請書編號: {@ApplyNo} 必要欄位不能為空值：{@Error}", req.ApplyNo, checkRequiredResult.ErrorMessage);
                    var errorNotice = MapHelper.MapToErrorNotice(
                        req.ApplyNo,
                        SystemErrorLogTypeConst.申請書資料驗證失敗,
                        $"必要欄位不能為空值:{checkRequiredResult.ErrorMessage}",
                        req
                    );
                    await 發送錯誤通知_後續不在執行(errorNotice);

                    await Retry處理(MapHelper.MapRetryRequest(req, caseContext), 回覆代碼.必要欄位不能為空值, req, checkRequiredResult.ErrorMessage);

                    return new EcardNewCaseResponse(回覆代碼.必要欄位不能為空值);
                }

                if (!VerifyHelper.檢查_徵信代碼只能為A02或者空值(req.CreditCheckCode))
                {
                    _logger.LogError("申請書編號: {@ApplyNo} 資料異常非定義值：{@Error}", req.ApplyNo, $"徵信代碼不在定義值內:{req.CreditCheckCode}");
                    var errorNotice = MapHelper.MapToErrorNotice(
                        req.ApplyNo,
                        SystemErrorLogTypeConst.申請書資料驗證失敗,
                        $"徵信代碼不在定義值內:{req.CreditCheckCode}",
                        req
                    );

                    await 發送錯誤通知_後續不在執行(errorNotice);

                    await Retry處理(
                        MapHelper.MapRetryRequest(req, caseContext),
                        回覆代碼.資料異常非定義值,
                        req,
                        $"徵信代碼不在定義值內:{req.CreditCheckCode}"
                    );

                    return new EcardNewCaseResponse(回覆代碼.資料異常非定義值);
                }

                var systemParam = await 取得系統參數();

                var validationCodes = GetValidationCodes(systemParam.AMLProfessionCode_Version);

                // 查無版本號 error
                // 2025.07.23 根據對應版本從setting取出AML職業別其他的代碼
                if (validationCodes.AMLProfessionOtherCode is null)
                {
                    _logger.LogError(
                        $"查無AML職業別其他的代碼，請確認設定檔是否有對應版本的ValidationSetting:AMLProfessionOther_{systemParam.AMLProfessionCode_Version}"
                    );
                    throw new Exception("查無對應版本號的 AML 職業別「其他」代碼。");
                }

                DataValidationResult dataValidationResult;
                if (caseContext.CaseType == 進件類型.原卡友)
                {
                    dataValidationResult = 檢查_資料異常非定義值_卡友(req, originParams, validationCodes);
                }
                else
                {
                    dataValidationResult = 檢查_資料異常非定義值_非卡友(req, caseContext, originParams, validationCodes);
                }

                if (!dataValidationResult.IsValid)
                {
                    _logger.LogError("申請書編號: {@ApplyNo} 資料異常非定義值：{@Error}", req.ApplyNo, dataValidationResult.ErrorMessage);
                    var errorNotice = MapHelper.MapToErrorNotice(
                        req.ApplyNo,
                        SystemErrorLogTypeConst.申請書資料驗證失敗,
                        $"資料異常非定義值:{dataValidationResult.ErrorMessage}",
                        req
                    );
                    await 發送錯誤通知_後續不在執行(errorNotice);
                    await Retry處理(MapHelper.MapRetryRequest(req, caseContext), 回覆代碼.資料異常非定義值, req, dataValidationResult.ErrorMessage);
                    return new EcardNewCaseResponse(回覆代碼.資料異常非定義值);
                }

                List<ErrorNotice> errorNotices = new List<ErrorNotice>();
                ProcessApplyFileResult processApplyFileResult = new();

                var applyFileResult = await 查詢_申請書及附件(req.CardAppId);

                if (applyFileResult.IsException)
                {
                    caseContext.ResultCode = 回覆代碼.ECARD_FILE_DB_連線錯誤;

                    _logger.LogError("申請書編號: {@ApplyNo} 查詢_申請書及附件，發生例外錯誤: {@Error}", req.ApplyNo, applyFileResult.ErrorMessage);

                    var errorNotice = MapHelper.MapToErrorNotice(
                        applyNo: req.ApplyNo,
                        type: SystemErrorLogTypeConst.ECARD_FILE_DB_連線錯誤,
                        errorTitle: $"申請書編號:{req.ApplyNo} 查詢_申請書及附件，連線異常",
                        errorDetail: applyFileResult.ErrorMessage,
                        request: req
                    );

                    await 發送錯誤通知(errorNotice);
                }
                else if (applyFileResult.ApplyFile == null)
                {
                    caseContext.ResultCode = 回覆代碼.查無申請書附件檔案;
                    _logger.LogError("申請書編號: {@ApplyNo} 查詢_申請書及附件，查無申請書附件檔案", req.ApplyNo);
                    var errorNotice = MapHelper.MapToErrorNotice(
                        applyNo: req.ApplyNo,
                        type: SystemErrorLogTypeConst.ECARD_FILE_DB_查無申請書附件檔案,
                        errorTitle: $"申請書編號:{req.ApplyNo} 查無申請書附件檔案",
                        request: req
                    );

                    await 發送錯誤通知(errorNotice);
                }
                else
                {
                    轉換申請書(processApplyFileResult, applyFileResult, caseContext);
                    if (processApplyFileResult.ApplicationIsException)
                    {
                        caseContext.ResultCode = 回覆代碼.申請書異常;

                        _logger.LogError(
                            "申請書編號: {@ApplyNo} 轉換申請書，發生例外錯誤: {@Error}",
                            req.ApplyNo,
                            processApplyFileResult.ApplicationErrorMessage
                        );

                        errorNotices.Add(
                            MapHelper.MapToErrorNotice(
                                req.ApplyNo,
                                SystemErrorLogTypeConst.申請書異常,
                                processApplyFileResult.ApplicationErrorMessage,
                                req
                            )
                        );
                    }
                    else
                    {
                        if (caseContext.CaseType != 進件類型.原卡友 || caseContext.IsCITSCard)
                        {
                            壓印附件浮水印(processApplyFileResult, applyFileResult, req.ID);

                            if (processApplyFileResult.AppendixIsException)
                            {
                                caseContext.ResultCode = 回覆代碼.附件異常;
                                _logger.LogError(
                                    "申請書編號: {@ApplyNo} 壓印_附件浮水印，發生例外錯誤: {@Error}",
                                    req.ApplyNo,
                                    processApplyFileResult.AppendixErrorMessage
                                );

                                errorNotices.Add(
                                    MapHelper.MapToErrorNotice(
                                        req.ApplyNo,
                                        SystemErrorLogTypeConst.附件異常,
                                        processApplyFileResult.AppendixErrorMessage,
                                        req
                                    )
                                );
                            }
                        }
                    }
                }

                (List<Reviewer_ApplyFile> reviewerApplyFiles, List<Reviewer_ApplyCreditCardInfoFile> reviewerApplyCreditCardInfoFiles) =
                    MapHelper.MapToApplyFileAndCreditCardInfoFile(processApplyFileResult, caseContext);
                Reviewer_BankTrace bankTrace = MapHelper.MarpToBankTrace(req);
                Reviewer_InternalCommunicate communicate = MapHelper.MapToCommunicate(req);
                Reviewer_FinanceCheckInfo finance = MapHelper.MapToFinance(req, systemParam.KYC_StrongReVersion);
                Reviewer_ApplyCreditCardInfoHandle handle = MapHelper.MapToHandle(req, caseContext);
                Reviewer_ApplyCreditCardInfoMain main = MapHelper.MapToMain(req, caseContext, systemParam.AMLProfessionCode_Version);
                Reviewer_OutsideBankInfo outsideBankInfo = MapHelper.MapToOutsideBankInfo(req);
                Reviewer_ApplyCreditCardInfoProcess process = MapHelper.MapToProcess(caseContext);
                Reviewer_ApplyNote note = MapHelper.MapToNote(req);
                List<System_ErrorLog> systemLogs = errorNotices.Select(x => MapHelper.MapToSystemLog(x)).ToList();
                ReviewerPedding_WebApplyCardCheckJobForA02? checkjob_A02 = null;
                ReviewerPedding_WebApplyCardCheckJobForNotA02? checkjob_NotA02 = null;
                if (caseContext.CaseType == 進件類型.原卡友)
                {
                    checkjob_A02 = MapHelper.MapToCheckA02Job(caseContext, req);
                }
                else
                {
                    checkjob_NotA02 = MapHelper.MapToCheckNotA02Job(caseContext, req);
                }

                if (!String.IsNullOrWhiteSpace(req.BirthPlace))
                {
                    var isInTaiwan = originParams.Where(x => x.Type == 參數類別.縣市).Select(x => x.StringValue).Contains(req.BirthPlace);
                    if (isInTaiwan)
                    {
                        main.BirthCitizenshipCode = BirthCitizenshipCode.中華民國;
                        main.IsFATCAIdentity = null;
                    }
                    else
                    {
                        main.BirthCitizenshipCode = BirthCitizenshipCode.其他;
                        main.BirthCitizenshipCodeOther = req.BirthPlaceOther;
                        main.IsFATCAIdentity = null;
                    }
                }

                if (!String.IsNullOrWhiteSpace(req.IDCardRenewalLocationName))
                {
                    var idCardRenewalLocationParamsDic = originParams
                        .Where(x => x.Type == 參數類別.身分證換發地點)
                        .ToDictionary(x => x.Name, x => x.StringValue);
                    main.IDCardRenewalLocationCode = idCardRenewalLocationParamsDic[req.IDCardRenewalLocationName];
                }

                // 執行分散式交易
                await ExecuteDistributedTransaction(
                    reviewerApplyFiles,
                    reviewerApplyCreditCardInfoFiles,
                    bankTrace,
                    communicate,
                    finance,
                    handle,
                    main,
                    outsideBankInfo,
                    process,
                    note,
                    systemLogs,
                    caseContext,
                    req,
                    checkjob_A02,
                    checkjob_NotA02
                );

                // ! 此為小白件，主因數位峰之前談的規格問題..需請客人提供正本申請書再至紙本API流程
                if (caseContext.CaseType == 進件類型.非卡友 && caseContext.IDType == null)
                {
                    // TODO: 同步資料給紙本件
                }

                return new EcardNewCaseResponse(caseContext.ResultCode);
            }
            catch (Exception ex)
            {
                _logger.LogError("申請書編號: {@ApplyNo} 其它異常訊息: {@Error}", req.ApplyNo, ex.ToString());

                await 發送錯誤通知_後續不在執行(
                    MapHelper.MapToErrorNotice(req.ApplyNo, SystemErrorLogTypeConst.內部程式錯誤, "其它異常訊息", req, ex.ToString())
                );

                return new EcardNewCaseResponse(回覆代碼.其它異常訊息);
            }
        }

        private async Task ExecuteDistributedTransaction(
            List<Reviewer_ApplyFile> reviewerApplyFiles,
            List<Reviewer_ApplyCreditCardInfoFile> reviewerApplyCreditCardInfoFiles,
            Reviewer_BankTrace bankTrace,
            Reviewer_InternalCommunicate communicate,
            Reviewer_FinanceCheckInfo finance,
            Reviewer_ApplyCreditCardInfoHandle handle,
            Reviewer_ApplyCreditCardInfoMain main,
            Reviewer_OutsideBankInfo outsideBankInfo,
            Reviewer_ApplyCreditCardInfoProcess process,
            Reviewer_ApplyNote note,
            List<System_ErrorLog> systemLogs,
            CaseContext caseContext,
            EcardNewCaseRequest req,
            ReviewerPedding_WebApplyCardCheckJobForA02? checkjob_A02,
            ReviewerPedding_WebApplyCardCheckJobForNotA02? checkjob_NotA02
        )
        {
            using var fileTransaction = await _scoreSharpFileContext.Database.BeginTransactionAsync();
            using var mainTransaction = await _scoreSharpContext.Database.BeginTransactionAsync();
            try
            {
                // Phase 1: 準備階段 - 檔案 Context
                await PrepareFileContext(reviewerApplyFiles);

                // Phase 1: 準備階段 - 主要 Context
                await PrepareMainContext(
                    reviewerApplyCreditCardInfoFiles,
                    bankTrace,
                    communicate,
                    finance,
                    handle,
                    main,
                    outsideBankInfo,
                    process,
                    note,
                    systemLogs,
                    checkjob_A02,
                    checkjob_NotA02,
                    caseContext
                );

                // Phase 2: 提交階段
                await _scoreSharpFileContext.SaveChangesAsync();
                await _scoreSharpContext.SaveChangesAsync();
                await fileTransaction.CommitAsync();
                await mainTransaction.CommitAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2628)
            {
                try
                {
                    _logger.LogError("申請書編號: {@ApplyNo}，資料異常資料長度過長，開始回滾", req.ApplyNo);
                    await mainTransaction.RollbackAsync();
                    await fileTransaction.RollbackAsync();
                    _logger.LogError("申請書編號: {@ApplyNo}，資料異常資料長度過長，回滾成功", req.ApplyNo);
                }
                catch (Exception rollbackEx)
                {
                    _logger.LogError("申請書編號: {@ApplyNo}，資料異常資料長度過長，回滾失敗，{@Error}", req.ApplyNo, rollbackEx.ToString());
                }

                // SQL Server 錯誤代碼 2628 表示資料截斷錯誤
                _logger.LogError("申請書編號: {@ApplyNo} 資料異常資料長度過長: {@Error}", req.ApplyNo, ex.ToString());

                await 發送錯誤通知_後續不在執行(
                    MapHelper.MapToErrorNotice(req.ApplyNo, SystemErrorLogTypeConst.內部程式錯誤, "資料異常資料長度過長", req, ex.ToString())
                );

                await Retry處理(
                    MapHelper.MapRetryRequest(req, caseContext),
                    回覆代碼.資料異常資料長度過長,
                    req,
                    $"資料異常資料長度過長:{ex.InnerException.Message}"
                );

                caseContext.ResultCode = 回覆代碼.資料異常資料長度過長;
            }
            catch (Exception ex)
            {
                try
                {
                    _logger.LogError("申請書編號: {@ApplyNo} 分散式交易失敗，開始回滾: {@Error}", req.ApplyNo, ex.ToString());

                    await mainTransaction.RollbackAsync();
                    await fileTransaction.RollbackAsync();

                    _logger.LogError("申請書編號: {@ApplyNo} 回滾成功", req.ApplyNo);
                }
                catch (Exception rollbackEx)
                {
                    _logger.LogError("申請書編號: {@ApplyNo} 回滾失敗，{@Error}", req.ApplyNo, rollbackEx.ToString());
                }

                throw;
            }
            finally
            {
                await fileTransaction.DisposeAsync();
                await mainTransaction.DisposeAsync();
            }
        }

        private async Task PrepareFileContext(List<Reviewer_ApplyFile> reviewerApplyFiles)
        {
            await _scoreSharpFileContext.Reviewer_ApplyFile.AddRangeAsync(reviewerApplyFiles);
            // 不要在這裡 SaveChanges，等到 Commit 階段
        }

        private async Task PrepareMainContext(
            List<Reviewer_ApplyCreditCardInfoFile> reviewerApplyCreditCardInfoFiles,
            Reviewer_BankTrace bankTrace,
            Reviewer_InternalCommunicate communicate,
            Reviewer_FinanceCheckInfo finance,
            Reviewer_ApplyCreditCardInfoHandle handle,
            Reviewer_ApplyCreditCardInfoMain main,
            Reviewer_OutsideBankInfo outsideBankInfo,
            Reviewer_ApplyCreditCardInfoProcess process,
            Reviewer_ApplyNote note,
            List<System_ErrorLog> systemLogs,
            ReviewerPedding_WebApplyCardCheckJobForA02? checkjob_A02,
            ReviewerPedding_WebApplyCardCheckJobForNotA02? checkjob_NotA02,
            CaseContext caseContext
        )
        {
            if (caseContext.CaseType == 進件類型.原卡友)
            {
                await _scoreSharpContext.ReviewerPedding_WebApplyCardCheckJobForA02.AddAsync(checkjob_A02);
            }
            else
            {
                await _scoreSharpContext.ReviewerPedding_WebApplyCardCheckJobForNotA02.AddAsync(checkjob_NotA02);
            }

            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.AddRangeAsync(reviewerApplyCreditCardInfoFiles);
            await _scoreSharpContext.Reviewer_BankTrace.AddAsync(bankTrace);
            await _scoreSharpContext.Reviewer_InternalCommunicate.AddAsync(communicate);
            await _scoreSharpContext.Reviewer_FinanceCheckInfo.AddAsync(finance);
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.AddAsync(handle);
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.AddAsync(main);
            await _scoreSharpContext.Reviewer_OutsideBankInfo.AddAsync(outsideBankInfo);
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddAsync(process);
            await _scoreSharpContext.Reviewer_ApplyNote.AddAsync(note);
            if (systemLogs.Any())
                await _scoreSharpContext.System_ErrorLog.AddRangeAsync(systemLogs);
        }

        private async Task<bool> 檢查_申請書編號是否重複進件(string applyNo)
        {
            var isDuplicate = await _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.AsNoTracking().FirstOrDefaultAsync(x => x.ApplyNo == applyNo);
            return isDuplicate != null ? true : false;
        }

        private bool 檢查_卡片代碼是否正確(string applyCardType, List<Usp_GetApplyCreditCardInfoWithParamsResult> cardParams)
        {
            var cards = cardParams.Select(x => x.StringValue).ToList();
            var cardTypeParams = applyCardType.Split('/');
            var isExist = cardTypeParams.All(x => cards.Contains(x));
            return isExist;
        }

        private async Task<bool> 檢查_LineBankUUID是否重複(string lineBankUUID)
        {
            if (String.IsNullOrWhiteSpace(lineBankUUID))
                return false;

            var isDuplicate = await _scoreSharpContext
                .Reviewer_ApplyCreditCardInfoMain.AsNoTracking()
                .FirstOrDefaultAsync(x => x.LineBankUUID == lineBankUUID);
            return isDuplicate != null ? true : false;
        }

        private DataValidationResult 檢查_資料異常非定義值_非卡友(
            EcardNewCaseRequest request,
            CaseContext caseContext,
            List<Usp_GetApplyCreditCardInfoWithParamsResult> originParams,
            FieldValidationCodes validationCodes
        )
        {
            var result = new DataValidationResult();

            var filterParams = originParams.Where(x => x.IsActive == "Y");

            var errorBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(request.IDType))
            {
                if (!VerifyHelper.檢查是否存在EnumByName<IDType>(request.IDType!))
                    errorBuilder.AppendFormat("身分別不在定義值內:{0}、", request.IDType);
            }

            if (!VerifyHelper.檢查是否存在EnumByValue<CardOwner>(request.CardOwner))
                errorBuilder.AppendFormat("主附卡別不在定義值內:{0}、", request.CardOwner);

            if (!string.IsNullOrEmpty(request.Sex) && !VerifyHelper.檢查是否存在EnumByValue<Sex>(request.Sex!))
                errorBuilder.AppendFormat("性別不在定義值內:{0}、", request.Sex);

            if (!VerifyHelper.檢查是否民國年日期正確(request.Birthday))
                errorBuilder.AppendFormat("出生日期格式不正確或無法轉換為有效日期:{0}、", request.Birthday);

            var cityParamsDic = filterParams.Where(x => x.Type == 參數類別.縣市).ToDictionary(x => x.StringValue!, x => x.Name);
            if (
                !cityParamsDic.TryGetValue(request.BirthPlace ?? "", out var _)
                && request.BirthPlace != BirthPlace其它固定值
                && !string.IsNullOrEmpty(request.BirthPlace)
            )
                errorBuilder.AppendFormat("出生地不在定義值內:{0}、", request.BirthPlace);

            var citizenshipCodeParamsDic = filterParams.Where(x => x.Type == 參數類別.國籍).ToDictionary(x => x.StringValue!, x => x.Name);
            if (request.BirthPlace == BirthPlace其它固定值 && !citizenshipCodeParamsDic.TryGetValue(request.BirthPlaceOther ?? "", out var _))
                errorBuilder.AppendFormat("出生地其他不在定義值內且不能為空:{0}、", request.BirthPlaceOther);

            if (!string.IsNullOrEmpty(request.CitizenshipCode) && !citizenshipCodeParamsDic.TryGetValue(request.CitizenshipCode ?? "", out var _))
                errorBuilder.AppendFormat("國籍不在定義值內:{0}、", request.CitizenshipCode);

            if (!VerifyHelper.檢查是否身分證格式(request.ID))
                errorBuilder.AppendFormat("身分證格式不正確:{0}、", request.ID);

            if (!string.IsNullOrEmpty(request.IDIssueDate) && !VerifyHelper.檢查是否民國年日期正確(request.IDIssueDate!))
                errorBuilder.AppendFormat("身分證發證日期格式不正確或無法轉換為有效日期:{0}、", request.IDIssueDate);

            var idCardRenewalLocationParamsDic = filterParams
                .Where(x => x.Type == 參數類別.身分證換發地點)
                .ToDictionary(x => x.Name, x => x.StringValue);
            if (
                !string.IsNullOrEmpty(request.IDCardRenewalLocationName)
                && !idCardRenewalLocationParamsDic.ContainsKey(request.IDCardRenewalLocationName!)
            )
                errorBuilder.AppendFormat("身分證發證地點不在定義值內:{0}、", request.IDCardRenewalLocationName);

            if (!string.IsNullOrEmpty(request.IDTakeStatus) && !VerifyHelper.檢查是否存在EnumByValue<IDTakeStatus>(request.IDTakeStatus!))
                errorBuilder.AppendFormat("身分證請領狀態不在定義值內:{0}、", request.IDTakeStatus);

            if (!string.IsNullOrEmpty(request.BillAddress))
            {
                if (!VerifyHelper.檢查是否存在EnumByValue<EcardSendAddress>(request.BillAddress!))
                    errorBuilder.AppendFormat("帳單地址不在定義值內:{0}、", request.BillAddress);
                else
                {
                    EcardSendAddress sendAddress = EnumExtenstions.ConvertEnumByIntNotNull<EcardSendAddress>(int.Parse(request.BillAddress));
                    var (isFailed, errorMsg) = VerifyHelper.檢查對應地址填寫(sendAddress, request);
                    if (isFailed)
                        errorBuilder.AppendFormat("帳單地址對應地址 {0}、", errorMsg);
                }
            }

            if (!string.IsNullOrEmpty(request.SendCardAddress))
            {
                if (!VerifyHelper.檢查是否存在EnumByValue<EcardSendAddress>(request.SendCardAddress!))
                    errorBuilder.AppendFormat("寄卡地址不在定義值內:{0}、", request.SendCardAddress);
                else
                {
                    EcardSendAddress sendAddress = EnumExtenstions.ConvertEnumByIntNotNull<EcardSendAddress>(int.Parse(request.SendCardAddress));
                    var (isFailed, errorMsg) = VerifyHelper.檢查對應地址填寫(sendAddress, request);
                    if (isFailed)
                        errorBuilder.AppendFormat("寄卡地址對應地址 {0}、", errorMsg);
                }
            }

            if (!string.IsNullOrEmpty(request.Mobile) && !VerifyHelper.檢查手機號碼格式(request.Mobile))
                errorBuilder.AppendFormat("正卡行動電話手機格式不正確或無法轉換為台灣的手機號碼格式內:{0}、", request.Mobile);

            if (!string.IsNullOrEmpty(request.EMail) && !VerifyHelper.驗證Email(request.EMail))
                errorBuilder.AppendFormat("Email格式不正確:{0}、", request.EMail);

            var amlProfessionParamsDic = filterParams.Where(x => x.Type == 參數類別.AML職業別).ToDictionary(x => x.StringValue!, x => x.Name);
            if (!string.IsNullOrEmpty(request.AMLProfessionCode) && !amlProfessionParamsDic.TryGetValue(request.AMLProfessionCode ?? "", out var _))
                errorBuilder.AppendFormat("AML職業類別不在定義值內:{0}、", request.AMLProfessionCode);

            // 2025.09.04 Ecard 進件未驗證 AML職業類別其他不能為空
            //if (request.AMLProfessionCode == validationCodes.AMLProfessionOtherCode && string.IsNullOrEmpty(request.AMLProfessionOther))
            //    errorBuilder.AppendFormat("AML職業類別其他不能為空值:{0}、", request.AMLProfessionOther);

            var amlJobLevelParamsDic = filterParams.Where(x => x.Type == 參數類別.AML職級別).ToDictionary(x => x.StringValue!, x => x.Name);
            if (!string.IsNullOrEmpty(request.AMLJobLevelCode) && !amlJobLevelParamsDic.TryGetValue(request.AMLJobLevelCode ?? "", out var _))
                errorBuilder.AppendFormat("AML職級別不在定義值內:{0}、", request.AMLJobLevelCode);

            if (!string.IsNullOrEmpty(request.CurrentMonthIncome) && !Int32.TryParse(request.CurrentMonthIncome, out var _))
                errorBuilder.AppendFormat("現職月收入只能輸入數字:{0}、", request.CurrentMonthIncome);

            if (!string.IsNullOrEmpty(request.MainIncomeAndFundCodes))
            {
                var mainIncomeAndFundParamsDic = filterParams
                    .Where(x => x.Type == 參數類別.主要收入來源)
                    .ToDictionary(x => x.StringValue!, x => x.Name);
                var mainIncomeAndFundCodesArray = request.MainIncomeAndFundCodes!.Split(',');
                var invalidCodes = mainIncomeAndFundCodesArray
                    .Where(code => !mainIncomeAndFundParamsDic.TryGetValue(code ?? "", out var _))
                    .ToArray();
                if (invalidCodes.Any())
                    errorBuilder.AppendFormat("主要所得及資金來源不在定義值內:{0}、", request.MainIncomeAndFundCodes);

                if (
                    mainIncomeAndFundCodesArray.Contains(validationCodes.MainIncomeAndFundOtherCode)
                    && string.IsNullOrEmpty(request.MainIncomeAndFundOther)
                )
                    errorBuilder.AppendFormat("主要所得及資金來源其他不能為空值:{0}、", request.MainIncomeAndFundOther);
            }

            if (!string.IsNullOrEmpty(request.IsAgreeDataOpen) && !VerifyHelper.驗證只能輸入01(request.IsAgreeDataOpen!))
                errorBuilder.AppendFormat("本人同意提供資料予聯名(認同)集團不在定義值內:{0}、", request.IsAgreeDataOpen);

            if (!string.IsNullOrEmpty(request.IsAgreeMarketing) && !VerifyHelper.驗證只能輸入01(request.IsAgreeMarketing!))
                errorBuilder.AppendFormat("是否同意提供資料於第三人行銷不在定義值內:{0}、", request.IsAgreeMarketing);

            if (!string.IsNullOrEmpty(request.IsAcceptEasyCardDefaultBonus) && !VerifyHelper.驗證只能輸入01(request.IsAcceptEasyCardDefaultBonus!))
                errorBuilder.AppendFormat("是否同意悠遊卡自動加值預設開啟不在定義值內:{0}、", request.IsAcceptEasyCardDefaultBonus);

            if (!string.IsNullOrEmpty(request.BillType) && !VerifyHelper.檢查是否存在EnumByValue<BillType>(request.BillType!))
                errorBuilder.AppendFormat("帳單形式不在定義值內:{0}、", request.BillType);

            if (!string.IsNullOrEmpty(request.IsApplyAutoDeduction) && !VerifyHelper.驗證只能輸入YN(request.IsApplyAutoDeduction))
                errorBuilder.AppendFormat("是否申辦自動扣繳不在定義值內:{0}、", request.IsApplyAutoDeduction);

            if (
                !string.IsNullOrEmpty(request.AutoDeductionPayType)
                && !VerifyHelper.檢查是否存在EnumByValue<AutoDeductionPayType>(request.AutoDeductionPayType!)
            )
                errorBuilder.AppendFormat("繳款方式不在定義值內:{0}、", request.AutoDeductionPayType);

            if (!VerifyHelper.檢查是否存在EnumByName<EcardSource>(request.Source!))
                errorBuilder.AppendFormat("進件方式不在定義值內:{0}、", request.Source);

            if (!string.IsNullOrEmpty(request.UserSourceIP) && !VerifyHelper.驗證IPV4(request.UserSourceIP!))
                errorBuilder.AppendFormat("來源IP不是正確格式:{0}、", request.UserSourceIP);

            if (!string.IsNullOrEmpty(request.OTPMobile) && !VerifyHelper.檢查手機號碼格式(request.OTPMobile!))
                errorBuilder.AppendFormat("OTP手機格式不正確或無法轉換為台灣的手機號碼格式:{0}、", request.OTPMobile);

            if (!string.IsNullOrEmpty(request.OTPTime) && !DateTime.TryParse(request.OTPTime, out var _))
                errorBuilder.AppendFormat("OTP時間不是正確時間格式:{0}、", request.OTPTime);

            if (!string.IsNullOrEmpty(request.IsApplyDigtalCard) && !VerifyHelper.驗證只能輸入YN(request.IsApplyDigtalCard ?? string.Empty))
                errorBuilder.AppendFormat("是否申請數位卡不在定義值內:{0}、", request.IsApplyDigtalCard);

            if (!string.IsNullOrEmpty(request.ApplyCardKind) && !VerifyHelper.檢查是否存在EnumByValue<ApplyCardKind>(request.ApplyCardKind!))
                errorBuilder.AppendFormat("申請卡種不在定義值內:{0}、", request.ApplyCardKind);

            if (!string.IsNullOrEmpty(request.EcardAttachmentNotes))
            {
                if (!VerifyHelper.檢查是否存在EnumByValue<AttachmentNotes>(request.EcardAttachmentNotes ?? string.Empty))
                {
                    errorBuilder.AppendFormat("附件註記不在定義值內:{0}、", request.EcardAttachmentNotes);
                }
                else
                {
                    if (
                        Enum.Parse<AttachmentNotes>(request.EcardAttachmentNotes!) == AttachmentNotes.MYDATA後補
                        && string.IsNullOrEmpty(request.MyDataCaseNo)
                    )
                        errorBuilder.AppendFormat("MyData案件編號不能為空:{0}、", request.MyDataCaseNo);
                }
            }

            if (!string.IsNullOrEmpty(request.IsKYCChange) && !VerifyHelper.驗證只能輸入YN(request.IsKYCChange ?? string.Empty))
                errorBuilder.AppendFormat("是否變更KYC不在定義值內:{0}、", request.IsKYCChange);

            if (!string.IsNullOrEmpty(request.IsPayNoticeBind) && !VerifyHelper.驗證只能輸入YN(request.IsPayNoticeBind!))
                errorBuilder.AppendFormat("消費通知綁定不在定義值內:{0}、", request.IsPayNoticeBind);

            if (!string.IsNullOrEmpty(request.HUOCUN_Balance) && !Int32.TryParse(request.HUOCUN_Balance, out var _))
                errorBuilder.AppendFormat("活存目前餘額不是正確格式:{0}、", request.HUOCUN_Balance);

            if (!string.IsNullOrEmpty(request.DINGCUN_Balance) && !Int32.TryParse(request.DINGCUN_Balance, out var _))
                errorBuilder.AppendFormat("定存目前餘額不是正確格式:{0}、", request.DINGCUN_Balance);

            if (!string.IsNullOrEmpty(request.HUOCUN_Balance_90) && !Int32.TryParse(request.HUOCUN_Balance_90, out var _))
                errorBuilder.AppendFormat("活存90天平均餘額不是正確格式:{0}、", request.HUOCUN_Balance_90);

            if (!string.IsNullOrEmpty(request.DINGCUN_Balance_90) && !Int32.TryParse(request.DINGCUN_Balance_90, out var _))
                errorBuilder.AppendFormat("定存90天平均餘額不是正確格式:{0}、", request.DINGCUN_Balance_90);

            if (!string.IsNullOrEmpty(request.BalanceUpdateDate) && !DateTime.TryParse(request.BalanceUpdateDate, out var _))
                errorBuilder.AppendFormat("餘額更新日期不是正確時間格式:{0}、", request.BalanceUpdateDate);

            if (!string.IsNullOrEmpty(request.IsStudent) && !VerifyHelper.驗證只能輸入YN(request.IsStudent!))
                errorBuilder.AppendFormat("是否學生身分不在定義值內:{0}、", request.IsStudent);

            if (request.IsStudent == "Y")
            {
                if (string.IsNullOrEmpty(request.ParentName))
                    errorBuilder.AppendFormat("家長姓名不能為空:{0}、", request.ParentName);

                if (string.IsNullOrEmpty(request.ParentPhone))
                    errorBuilder.AppendFormat("家長電話不能為空:{0}、", request.ParentPhone);
                else if (request.ParentPhone.StartsWith("09") && !VerifyHelper.檢查手機號碼格式(request.ParentPhone))
                    errorBuilder.AppendFormat("家長電話格式不正確或無法轉換為台灣的手機號碼格式:{0}、", request.ParentPhone);
                else if (!VerifyHelper.檢查電話號碼格式(request.ParentPhone))
                    errorBuilder.AppendFormat("家長電話不是正確格式:{0}、", request.ParentPhone);

                if (VerifyHelper.檢查地址有效性(request.ParentLive_City, request.ParentLive_District, request.ParentLive_Road))
                {
                    errorBuilder.AppendFormat(
                        "家長居住地址 一定要有 縣市 / 區域 / 路名: {0} / {1} / {2}、",
                        request.ParentLive_City,
                        request.ParentLive_District,
                        request.ParentLive_Road
                    );
                }
            }

            if (!string.IsNullOrEmpty(request.Education) && !VerifyHelper.檢查是否存在EnumByValue<Education>(request.Education!))
                errorBuilder.AppendFormat("學歷不在定義值內:{0}、", request.Education);

            if (!string.IsNullOrEmpty(request.HouseRegPhone) && !VerifyHelper.檢查電話號碼格式(request.HouseRegPhone))
                errorBuilder.AppendFormat("戶籍電話不是正確格式:{0}、", request.HouseRegPhone);

            if (!string.IsNullOrEmpty(request.LivePhone) && !VerifyHelper.檢查電話號碼格式(request.LivePhone))
                errorBuilder.AppendFormat("居住電話不是正確格式:{0}、", request.LivePhone);

            if (!string.IsNullOrEmpty(request.LiveOwner) && !VerifyHelper.檢查是否存在EnumByValue<LiveOwner>(request.LiveOwner!))
                errorBuilder.AppendFormat("居住地所有權人(住所狀態)不在定義值內:{0}、", request.LiveOwner);

            if (!string.IsNullOrEmpty(request.CompID) && !VerifyHelper.驗證統一編號(request.CompID))
                errorBuilder.AppendFormat("統一編號不是正確格式:{0}、", request.CompID);

            if (!string.IsNullOrEmpty(request.CompSeniority) && !Int32.TryParse(request.CompSeniority, out var _))
                errorBuilder.AppendFormat("年資不是正確格式:{0}、", request.CompSeniority);

            if (VerifyHelper.檢查地址有效性(request.Reg_City, request.Reg_District, request.Reg_Road))
            {
                errorBuilder.AppendFormat(
                    "正卡人戶籍地址 一定要有 縣市 / 區域 / 路名: {0} / {1} / {2}、",
                    request.Reg_City,
                    request.Reg_District,
                    request.Reg_Road
                );
            }

            if (VerifyHelper.檢查地址有效性(request.Home_City, request.Home_District, request.Home_Road))
            {
                errorBuilder.AppendFormat(
                    "正卡人居住地址 一定要有 縣市 / 區域 / 路名: {0} / {1} / {2}、",
                    request.Home_City,
                    request.Home_District,
                    request.Home_Road
                );
            }

            if (errorBuilder.Length > 0)
            {
                string errorMessage = String.Join(Environment.NewLine, errorBuilder.ToString().TrimEnd('、').Split('、'));
                result.IsValid = false;
                result.ErrorMessage = errorMessage;
            }
            else
            {
                result.IsValid = true;
                result.ErrorMessage = string.Empty;
            }

            return result;
        }

        private DataValidationResult 檢查_資料異常非定義值_卡友(
            EcardNewCaseRequest request,
            List<Usp_GetApplyCreditCardInfoWithParamsResult> originParams,
            FieldValidationCodes validationCodes
        )
        {
            var result = new DataValidationResult();

            var filterParams = originParams.Where(x => x.IsActive == "Y");

            var errorBuilder = new StringBuilder();

            if (!VerifyHelper.檢查是否存在EnumByValue<CardOwner>(request.CardOwner))
                errorBuilder.AppendFormat("主附卡別不在定義值內:{0}、", request.CardOwner);

            if (!VerifyHelper.檢查是否存在EnumByValue<Sex>(request.Sex))
                errorBuilder.AppendFormat("性別不在定義值內:{0}、", request.Sex);

            if (!VerifyHelper.檢查是否民國年日期正確(request.Birthday))
                errorBuilder.AppendFormat("出生日期格式不正確或無法轉換為有效日期:{0}、", request.Birthday);

            var cityParamsDic = filterParams.Where(x => x.Type == 參數類別.縣市).ToDictionary(x => x.StringValue!, x => x.Name);
            if (
                !string.IsNullOrEmpty(request.BirthPlace)
                && !cityParamsDic.TryGetValue(request.BirthPlace ?? "", out var _)
                && request.BirthPlace != BirthPlace其它固定值
            )
                errorBuilder.AppendFormat("出生地不在定義值內:{0}、", request.BirthPlace);

            var citizenshipCodeParamsDic = filterParams.Where(x => x.Type == 參數類別.國籍).ToDictionary(x => x.StringValue!, x => x.Name);
            if (request.BirthPlace == BirthPlace其它固定值 && !citizenshipCodeParamsDic.TryGetValue(request.BirthPlaceOther ?? "", out var _))
                errorBuilder.AppendFormat("出生地其他不在定義值內且不能為空:{0}、", request.BirthPlaceOther);

            if (!string.IsNullOrEmpty(request.CitizenshipCode) && !citizenshipCodeParamsDic.TryGetValue(request.CitizenshipCode ?? "", out var _))
                errorBuilder.AppendFormat("國籍不在定義值內:{0}、", request.CitizenshipCode);

            if (!VerifyHelper.檢查是否身分證格式(request.ID))
                errorBuilder.AppendFormat("身分證格式不正確:{0}、", request.ID);

            if (!string.IsNullOrEmpty(request.Mobile) && !VerifyHelper.檢查手機號碼格式(request.Mobile))
                errorBuilder.AppendFormat("行動電話手機格式不正確或無法轉換為台灣的手機號碼格式內:{0}、", request.Mobile);

            if (!string.IsNullOrEmpty(request.EMail) && !VerifyHelper.驗證Email(request.EMail))
                errorBuilder.AppendFormat("Email格式不正確:{0}、", request.EMail);

            var amlProfessionParamsDic = filterParams.Where(x => x.Type == 參數類別.AML職業別).ToDictionary(x => x.StringValue!, x => x.Name);
            if (!string.IsNullOrEmpty(request.AMLProfessionCode) && !amlProfessionParamsDic.TryGetValue(request.AMLProfessionCode ?? "", out var _))
                errorBuilder.AppendFormat("AML職業類別不在定義值內:{0}、", request.AMLProfessionCode);

            // 2025.09.04 Ecard 進件未驗證 AML職業類別其他不能為空
            //if (request.AMLProfessionCode == validationCodes.AMLProfessionOtherCode && string.IsNullOrEmpty(request.AMLProfessionOther))
            //    errorBuilder.AppendFormat("AML職業類別其他不能為空值:{0}、", request.AMLProfessionOther);

            var amlJobLevelParamsDic = filterParams.Where(x => x.Type == 參數類別.AML職級別).ToDictionary(x => x.StringValue!, x => x.Name);
            if (!string.IsNullOrEmpty(request.AMLJobLevelCode) && !amlJobLevelParamsDic.TryGetValue(request.AMLJobLevelCode ?? "", out var _))
                errorBuilder.AppendFormat("AML職級別不在定義值內:{0}、", request.AMLJobLevelCode);

            if (!string.IsNullOrEmpty(request.MainIncomeAndFundCodes))
            {
                var mainIncomeAndFundParamsDic = filterParams
                    .Where(x => x.Type == 參數類別.主要收入來源)
                    .ToDictionary(x => x.StringValue!, x => x.Name);
                var mainIncomeAndFundCodesArray = request.MainIncomeAndFundCodes!.Split(',');
                var invalidCodes = mainIncomeAndFundCodesArray
                    .Where(code => !mainIncomeAndFundParamsDic.TryGetValue(code ?? "", out var _))
                    .ToArray();
                if (invalidCodes.Any())
                    errorBuilder.AppendFormat("主要所得及資金來源不在定義值內:{0}、", request.MainIncomeAndFundCodes);

                if (
                    mainIncomeAndFundCodesArray.Contains(validationCodes.MainIncomeAndFundOtherCode)
                    && string.IsNullOrEmpty(request.MainIncomeAndFundOther)
                )
                    errorBuilder.AppendFormat("主要所得及資金來源其他不能為空值:{0}、", request.MainIncomeAndFundOther);
            }

            if (!string.IsNullOrEmpty(request.IsAgreeDataOpen) && !VerifyHelper.驗證只能輸入01(request.IsAgreeDataOpen!))
                errorBuilder.AppendFormat("本人同意提供資料予聯名(認同)集團不在定義值內:{0}、", request.IsAgreeDataOpen);

            if (!string.IsNullOrEmpty(request.IsAgreeMarketing) && !VerifyHelper.驗證只能輸入01(request.IsAgreeMarketing!))
                errorBuilder.AppendFormat("是否同意提供資料於第三人行銷不在定義值內:{0}、", request.IsAgreeMarketing);

            if (!string.IsNullOrEmpty(request.IsAcceptEasyCardDefaultBonus) && !VerifyHelper.驗證只能輸入01(request.IsAcceptEasyCardDefaultBonus!))
                errorBuilder.AppendFormat("是否同意悠遊卡自動加值預設開啟不在定義值內:{0}、", request.IsAcceptEasyCardDefaultBonus);

            if (!string.IsNullOrEmpty(request.BillType) && !VerifyHelper.檢查是否存在EnumByValue<BillType>(request.BillType!))
                errorBuilder.AppendFormat("帳單形式不在定義值內:{0}、", request.BillType);

            if (!VerifyHelper.檢查是否存在EnumByName<EcardSource>(request.Source!))
                errorBuilder.AppendFormat("進件方式不在定義值內:{0}、", request.Source);

            if (!string.IsNullOrEmpty(request.UserSourceIP) && !VerifyHelper.驗證IPV4(request.UserSourceIP!))
                errorBuilder.AppendFormat("來源IP不是正確格式:{0}、", request.UserSourceIP);

            if (!string.IsNullOrEmpty(request.OTPMobile) && !VerifyHelper.檢查手機號碼格式(request.OTPMobile!))
                errorBuilder.AppendFormat("OTP手機格式不正確或無法轉換為台灣的手機號碼格式:{0}、", request.OTPMobile);

            if (!string.IsNullOrEmpty(request.OTPTime) && !DateTime.TryParse(request.OTPTime, out var _))
                errorBuilder.AppendFormat("OTP時間不是正確時間格式:{0}、", request.OTPTime);

            if (!string.IsNullOrEmpty(request.IsApplyDigtalCard) && !VerifyHelper.驗證只能輸入YN(request.IsApplyDigtalCard ?? string.Empty))
                errorBuilder.AppendFormat("是否申請數位卡不在定義值內:{0}、", request.IsApplyDigtalCard);

            if (!string.IsNullOrEmpty(request.ApplyCardKind) && !VerifyHelper.檢查是否存在EnumByValue<ApplyCardKind>(request.ApplyCardKind!))
                errorBuilder.AppendFormat("申請卡種不在定義值內:{0}、", request.ApplyCardKind);

            if (string.IsNullOrEmpty(request.CardAddr))
                errorBuilder.AppendFormat("寄卡地址需填寫:{0}、", request.CardAddr);

            if (!string.IsNullOrEmpty(request.IsKYCChange) && !VerifyHelper.驗證只能輸入YN(request.IsKYCChange ?? string.Empty))
                errorBuilder.AppendFormat("是否變更KYC不在定義值內:{0}、", request.IsKYCChange);

            if (!string.IsNullOrEmpty(request.IsPayNoticeBind) && !VerifyHelper.驗證只能輸入YN(request.IsPayNoticeBind!))
                errorBuilder.AppendFormat("消費通知綁定不在定義值內:{0}、", request.IsPayNoticeBind);

            if (!string.IsNullOrEmpty(request.IsStudent) && !VerifyHelper.驗證只能輸入YN(request.IsStudent!))
                errorBuilder.AppendFormat("是否學生身分不在定義值內:{0}、", request.IsStudent);

            if (errorBuilder.Length > 0)
            {
                string errorMessage = String.Join(Environment.NewLine, errorBuilder.ToString().TrimEnd('、').Split('、'));
                result.IsValid = false;
                result.ErrorMessage = errorMessage;
            }
            else
            {
                result.IsValid = true;
                result.ErrorMessage = string.Empty;
            }
            return result;
        }

        public async Task<bool> 檢查_是否為國旅卡(string applyCardType)
        {
            var citsCardList = await _cache.GetOrSetAsync(
                "CITSCard",
                async _ =>
                {
                    return await _scoreSharpContext
                        .SetUp_Card.Where(x => x.IsCITSCard == "Y" && x.IsActive == "Y")
                        .Select(x => x.CardCode)
                        .ToListAsync();
                }
            );

            if (applyCardType.Split('/').All(x => citsCardList.Contains(x)))
            {
                return true;
            }
            return false;
        }

        private async Task 發送錯誤通知_後續不在執行(ErrorNotice errorNotice)
        {
            _scoreSharpFileContext.ChangeTracker.Clear();
            _scoreSharpContext.ChangeTracker.Clear();
            // 新增錯誤紀錄
            await _scoreSharpContext.System_ErrorLog.AddAsync(MapHelper.MapToSystemLog(errorNotice));
            // 儲存資料
            await _scoreSharpContext.SaveChangesAsync();
        }

        private async Task 發送錯誤通知(ErrorNotice errorNotice)
        {
            // 新增錯誤紀錄
            await _scoreSharpContext.System_ErrorLog.AddAsync(MapHelper.MapToSystemLog(errorNotice));
        }

        private async Task Retry處理(RetryRequest req, string returnCode, EcardNewCaseRequest request, string log)
        {
            _scoreSharpContext.ChangeTracker.Clear();
            _scoreSharpFileContext.ChangeTracker.Clear();
            /*
                當回覆代碼為以下狀態時
                0005 (非定義值)
                0006 (資料長度過長)
                0007 (必要欄位不能為空值)
                0013 (ECARD_FILE_DB 例外錯誤)
                0014 (查無申請書附件檔案)

            1. 新增申請書編號,ID,姓名,卡別,案件狀態,進件日期,案件種類
            2. 新增 Process 紀錄
            3. 保留 Req + 於後台重新呼叫 Update 此筆資料
            */

            Reviewer_ApplyCreditCardInfoHandle handle = new()
            {
                SeqNo = Ulid.NewUlid().ToString(),
                ApplyNo = req.ApplyNo,
                ID = req.ID,
                UserType = req.UserType,
                ApplyCardType = req.ApplyCardType,
                CardStatus = ConvertHelper.ConvertRetryCardStatus(returnCode),
            };

            Reviewer_ApplyCreditCardInfoMain main = new()
            {
                ApplyNo = req.ApplyNo,
                ID = req.ID,
                CHName = req.CHName,
                UserType = req.UserType,
                Source = ConvertHelper.ConvertSource(req.Source).Value,
                ApplyDate = req.ApplyDate,
                CardOwner = ConvertHelper.ConvertCardOwner(req.CardOwner).Value,
                CaseType = CaseType.一般件,
            };

            ReviewerPedding_WebRetryCase retryCase = new()
            {
                ApplyNo = req.ApplyNo,
                Request = JsonHelper.序列化物件(request),
                ReturnCode = returnCode,
                CaseErrorLog = log,
                IsSend = "N",
            };

            Reviewer_ApplyCreditCardInfoProcess process = new()
            {
                ApplyNo = req.ApplyNo,
                StartTime = req.ApplyDate,
                EndTime = req.ApplyDate,
                ProcessUserId = "SYSTEM",
                Process = ConvertHelper.ConvertRetryCardStatus(returnCode).ToString(),
            };

            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.AddAsync(handle);
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.AddAsync(main);
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddAsync(process);
            await _scoreSharpContext.ReviewerPedding_WebRetryCase.AddAsync(retryCase);
            await _scoreSharpContext.SaveChangesAsync();
        }

        private async Task<List<Usp_GetApplyCreditCardInfoWithParamsResult>> 查詢_參數()
        {
            var result = await _cache.GetOrSetAsync(
                "ApplyCreditCardInfoWithParams",
                async _ =>
                {
                    return await _scoreSharpContext.Procedures.Usp_GetApplyCreditCardInfoWithParamsAsync();
                }
            );
            return result;
        }

        public async Task<GetApplyFileResult> 查詢_申請書及附件(string applyId)
        {
            var result = new GetApplyFileResult();
            try
            {
                string sql =
                    @" SELECT [idPic1],
                            [idPic2],
                            [upload1],
                            [upload2],
                            [upload3],
                            [upload4],
                            [upload5],
                            [upload6],
                            [uploadPDF]
                            FROM [eCard_file].[dbo].[ApplyFile]
                            WHERE [cCard_AppId] = @ApplyId ";

                using var conn = _scoreSharpDapperContext.CreateECardFileConnection();
                var file = await conn.QueryFirstOrDefaultAsync<ApplyFile>(sql, new { ApplyId = applyId });

                result.IsException = false;
                result.ApplyFile = file;
            }
            catch (Exception ex)
            {
                result.IsException = true;
                result.ApplyFile = null;
                result.ErrorMessage = ex.ToString();
            }
            return result;
        }

        public void 轉換申請書(ProcessApplyFileResult processApplyFileResult, GetApplyFileResult applyFileResult, CaseContext caseContext)
        {
            if (_env.IsDevelopment() && caseContext.ApplyNo == 測試案件編號.申請書異常)
            {
                processApplyFileResult.ApplicationIsException = true;
                processApplyFileResult.ApplicationErrorMessage = "申請書不存在";
                return;
            }

            if (applyFileResult.ApplyFile.UploadPDF is null)
            {
                processApplyFileResult.ApplicationIsException = true;
                processApplyFileResult.ApplicationErrorMessage = "申請書不存在";
                return;
            }

            processApplyFileResult.ApplyFiles.Add("uploadPDF", applyFileResult.ApplyFile.UploadPDF);
        }

        public void 壓印附件浮水印(ProcessApplyFileResult processApplyFileResult, GetApplyFileResult applyFileResult, string id)
        {
            string watermarkText = _configuration.GetValue<string>("WatermarkText") ?? "聯邦銀行股份授權";

            var 附件 = new Dictionary<string, byte[]>
            {
                { "idPic1", applyFileResult.ApplyFile.IdPic1 },
                { "idPic2", applyFileResult.ApplyFile.IdPic2 },
                { "upload1", applyFileResult.ApplyFile.Upload1 },
                { "upload2", applyFileResult.ApplyFile.Upload2 },
                { "upload3", applyFileResult.ApplyFile.Upload3 },
                { "upload4", applyFileResult.ApplyFile.Upload4 },
                { "upload5", applyFileResult.ApplyFile.Upload5 },
                { "upload6", applyFileResult.ApplyFile.Upload6 },
            };

            if (附件.Values.All(x => x == null))
            {
                processApplyFileResult.AppendixIsException = true;
                processApplyFileResult.AppendixErrorMessage = "所有附件皆無資料";
                return;
            }

            Dictionary<string, string> errorFiles = new();
            foreach (var image in 附件)
            {
                try
                {
                    if (image.Value != null)
                    {
                        var afetrWatermarkImage = _watermarkHelper.ImageWatermarkAndGetBytes(watermarkText, ".jpg", image.Value);
                        if (_env.EnvironmentName == "Testing" && id == 測試身分證字號.附件異常)
                        {
                            throw new Exception("附件檔案異常ForTesting");
                        }

                        processApplyFileResult.ApplyFiles.Add(image.Key, afetrWatermarkImage);
                    }
                }
                catch (Exception ex)
                {
                    errorFiles.Add(image.Key, ex.ToString());
                }
            }

            if (errorFiles.Any())
            {
                processApplyFileResult.AppendixIsException = true;
                processApplyFileResult.AppendixErrorMessage = string.Join("; ", errorFiles.Select(x => $"{x.Key}: {x.Value}"));
            }
        }

        private async Task<SysParamManage_SysParam> 取得系統參數() =>
            await _scoreSharpContext.SysParamManage_SysParam.AsNoTracking().FirstOrDefaultAsync();

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
