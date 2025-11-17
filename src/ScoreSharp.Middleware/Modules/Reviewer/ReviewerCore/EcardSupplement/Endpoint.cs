using System.Data;
using ScoreSharp.Common.Helpers;
using ScoreSharp.Middleware.Infrastructures.FileData;
using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardSupplement;
using ScoreSharp.Watermark;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// ECARD補件API
        /// </summary>
        /// <remarks>
        /// 此 api 的 request content-type 是 application/x-www-form-urlencoded
        ///
        ///     範例:
        ///
        ///         P_ID=K12798732&amp;
        ///         SUP_NO=20250409175522259&amp;
        ///         APPENDIX_FILE_NAME_01=20250409_17552282159__3.jpg&amp;
        ///         APPENDIX_FILE_NAME_02=&amp;
        ///         APPENDIX_FILE_NAME_03=&amp;
        ///         APPENDIX_FILE_NAME_04=&amp;
        ///         APPENDIX_FILE_NAME_05=&amp;
        ///         APPENDIX_FILE_NAME_06=&amp;
        ///         MYDATA_NO=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EcardSupplementResponse))]
        [EndpointSpecificExample(
            typeof(ECARD補件匯入成功_0000_ReqEx),
            typeof(ECARD補件必要欄位為空值_0001_ReqEx),
            typeof(ECARD補件長度過長_0002_ReqEx),
            typeof(ECARD補件其它異常訊息_查無ID對應資料_0003_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(ECARD補件匯入成功_0000_ResEx),
            typeof(ECARD補件必要欄位為空值_0001_ResEx),
            typeof(ECARD補件長度過長_0002_ResEx),
            typeof(ECARD補件其它異常訊息_查無ID對應資料_0003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("EcardSupplement")]
        public async Task<IResult> EcardSupplement([FromForm] EcardSupplementRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardSupplement
{
    public record Command(EcardSupplementRequest ecardSupplementRequest) : IRequest<EcardSupplementResponse>;

    public class Handler : IRequestHandler<Command, EcardSupplementResponse>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly ScoreSharpContext _scoreSharpContext;
        private readonly IScoreSharpDapperContext _scoreSharpDapperContext;
        private readonly IWatermarkHelper _watermarkHelper;
        private readonly string _watermarkText;
        private readonly IFTPHelper _ftpHelper;
        private readonly FTPOption _ftpOption;
        private readonly ScoreSharpFileContext _scoreSharpFileContext;
        private DateTime _now;

        public Handler(
            ILogger<Handler> logger,
            IDiagnosticContext diagnosticContext,
            ScoreSharpContext scoreSharpContext,
            IScoreSharpDapperContext scoreSharpDapperContext,
            IWatermarkHelper watermarkHelper,
            IFTPHelper ftpHelper,
            IConfiguration configuration,
            IOptions<FTPOption> ftpOption,
            ScoreSharpFileContext scoreSharpFileContext
        )
        {
            _logger = logger;
            _scoreSharpContext = scoreSharpContext;
            _scoreSharpDapperContext = scoreSharpDapperContext;
            _watermarkHelper = watermarkHelper;
            _diagnosticContext = diagnosticContext;
            _ftpHelper = ftpHelper;
            _ftpOption = ftpOption.Value;
            _watermarkText = configuration.GetValue<string>("WatermarkText");
            _scoreSharpFileContext = scoreSharpFileContext;
            _now = DateTime.Now;
        }

        public async Task<EcardSupplementResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var req = request.ecardSupplementRequest;

            using var _ = _logger.PushProperties(("SupplementNo", req.SupplementNo), ("ID", req.ID), ("MyDataNo", req.MyDataNo ?? String.Empty));
            _diagnosticContext.SetProperties(
                ("SupplementNo", req.SupplementNo, false),
                ("ID", req.ID, false),
                ("MyDataNo", req.MyDataNo ?? String.Empty, false)
            );

            try
            {
                var checkValidateResult = 檢查_EcardSupplementRequest(req);
                if (!checkValidateResult.IsValid)
                {
                    _logger.LogError("{@ErrorType}： ID:{@ID} 補件編號:{@SupplementNo} ", checkValidateResult.ErrorType, req.ID, req.MyDataNo);

                    await 發送錯誤通知(
                        SystemErrorLogTypeConst.補件資料驗證失敗,
                        checkValidateResult.ErrorType,
                        request: JsonHelper.序列化物件(req),
                        response: JsonHelper.序列化物件(checkValidateResult.Response)
                    );

                    return checkValidateResult.Response;
                }

                var 補件所需相關資訊 = await 取出需補件案件資訊(req.ID);
                if (補件所需相關資訊.Count == 0)
                {
                    _logger.LogError("其它異常訊息: {@Error}", $"查無 {req.ID} 對應資料");
                    await 發送錯誤通知(
                        SystemErrorLogTypeConst.補件資料驗證失敗,
                        "其它異常訊息_查無ID對應資料",
                        request: JsonHelper.序列化物件(req),
                        response: JsonHelper.序列化物件(new EcardSupplementResponse(回覆代碼.其它異常訊息))
                    );
                    return new EcardSupplementResponse(回覆代碼.其它異常訊息);
                }

                var fileNames = 取出檔案名稱(req);
                var ftpResult = await GetFtpResult(fileNames);
                if (ftpResult.Results.Any(x => !x.IsSuccess) || !ftpResult.IsSuccess)
                {
                    string errorMsg = string.IsNullOrEmpty(ftpResult.ErrorMessage)
                        ? String.Join('、', ftpResult.Results.Select(x => x.ErrorMessage))
                        : ftpResult.ErrorMessage;
                    _logger.LogError("FTP 操作過程中發生錯誤");
                    await 發送錯誤通知(
                        SystemErrorLogTypeConst.補件FTP下載失敗,
                        "其它異常訊息_FTP操作過程中發生錯誤",
                        request: JsonHelper.序列化物件(req),
                        response: JsonHelper.序列化物件(new EcardSupplementResponse(回覆代碼.其它異常訊息)),
                        errorDeatil: errorMsg
                    );
                    return new EcardSupplementResponse(回覆代碼.其它異常訊息);
                }

                var 浮水印加工後檔案 = 浮水印加工(ftpResult.Results);

                var eCardSupplementInfoContext = GenerateECardSupplementInfoContext(
                    群組補件所需相關資訊: 補件所需相關資訊,
                    浮水印加工後檔案: 浮水印加工後檔案,
                    req: req,
                    now: _now
                );

                await ExecuteDistributedTransaction(eCardSupplementInfoContext, req);
            }
            catch (Exception ex)
            {
                _logger.LogError("ECARD補件API發生錯誤: {@Error}", ex.ToString());

                await 發送錯誤通知(
                    SystemErrorLogTypeConst.內部程式錯誤,
                    "其它異常訊息_ECARD補件API發生錯誤",
                    request: JsonHelper.序列化物件(request.ecardSupplementRequest),
                    response: JsonHelper.序列化物件(new EcardSupplementResponse(回覆代碼.其它異常訊息)),
                    errorDeatil: ex.ToString()
                );

                return new EcardSupplementResponse(回覆代碼.其它異常訊息);
            }

            return new EcardSupplementResponse(回覆代碼.匯入成功);
        }

        private async Task PrepareScoreSharpFilesContext(List<ECardSupplementInfoContext> context)
        {
            var applyFiles = context.SelectMany(x => x.ApplyFiles).ToList();
            await _scoreSharpFileContext.Reviewer_ApplyFile.AddRangeAsync(applyFiles);
        }

        private async Task PrepareScoreSharpLogContext(List<ECardSupplementInfoContext> context)
        {
            var updateProcess = context.SelectMany(x => x.Processes).ToList();
            var updateFiles = context.SelectMany(x => x.FilesLog).ToList();
            var updateCardRecords = context.SelectMany(x => x.CardRecords).ToList();
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.AddRangeAsync(updateFiles);
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(updateProcess);
            await _scoreSharpContext.Reviewer_CardRecord.AddRangeAsync(updateCardRecords);
        }

        private async Task PrepareScoreSharpHandleContext(List<ECardSupplementInfoContext> context)
        {
            string updateSql =
                @"
                UPDATE [dbo].[Reviewer_ApplyCreditCardInfoHandle]
                SET [CardStatus] = {1},
                    [ApproveUserId] = NULL,
                    [ApproveTime] = NULL
                WHERE [SeqNo] = {0}";

            var handleSeqAndCardStatus = context
                .SelectMany(x => x.HandleInfos)
                .Select(x => new { x.HandleSeqNo, x.AfterCardStatus })
                .Distinct()
                .ToList();

            foreach (var item in handleSeqAndCardStatus)
            {
                await _scoreSharpContext.Database.ExecuteSqlRawAsync(updateSql, item.HandleSeqNo, item.AfterCardStatus);
            }
        }

        private async Task ExecuteDistributedTransaction(List<ECardSupplementInfoContext> context, EcardSupplementRequest req)
        {
            using var fileTransaction = await _scoreSharpFileContext.Database.BeginTransactionAsync();
            using var mainTransaction = await _scoreSharpContext.Database.BeginTransactionAsync();

            try
            {
                // Phase 1: 準備階段 - Context
                await PrepareScoreSharpFilesContext(context);
                await PrepareScoreSharpLogContext(context);
                await PrepareScoreSharpHandleContext(context);
                await PrepareScoreSharpMainContext(context, _now);

                // Phase 2: 提交階段
                await _scoreSharpFileContext.SaveChangesAsync();
                await _scoreSharpContext.SaveChangesAsync();
                await fileTransaction.CommitAsync();
                await mainTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                try
                {
                    _logger.LogError(
                        "補件編號: {@SupplementNo} ID:{@ID} 分散式交易失敗，開始回滾: {@Error}",
                        req.SupplementNo,
                        req.ID,
                        ex.ToString()
                    );

                    await mainTransaction.RollbackAsync();
                    await fileTransaction.RollbackAsync();

                    _logger.LogError("補件編號: {@SupplementNo} ID:{@ID} 回滾成功", req.SupplementNo, req.ID);
                }
                catch (Exception rollbackEx)
                {
                    _logger.LogError("補件編號: {@SupplementNo} ID:{@ID} 回滾失敗，{@Error}", req.SupplementNo, req.ID, rollbackEx.ToString());
                }

                throw;
            }
            finally
            {
                await fileTransaction.DisposeAsync();
                await mainTransaction.DisposeAsync();
            }
        }

        private async Task 發送錯誤通知(
            string type,
            string errorMessage,
            string? request = null,
            string? response = null,
            string? applyNo = null,
            string? errorDeatil = null
        )
        {
            _scoreSharpContext.ChangeTracker.Clear();
            _scoreSharpFileContext.ChangeTracker.Clear();

            // 新增錯誤紀錄
            await _scoreSharpContext.System_ErrorLog.AddAsync(
                new System_ErrorLog()
                {
                    ApplyNo = applyNo,
                    Project = SystemErrorLogProjectConst.MIDDLEWARE,
                    Source = "EcardSupplement",
                    Type = type,
                    ErrorMessage = errorMessage,
                    ErrorDetail = errorDeatil,
                    Request = request,
                    Response = response,
                    AddTime = DateTime.Now,
                    SendStatus = SendStatus.等待,
                }
            );

            // 儲存資料
            await _scoreSharpContext.SaveChangesAsync();
        }

        private List<GetMultipleFilesBytesAsyncItemResult> 浮水印加工(List<GetMultipleFilesBytesAsyncItemResult> originfiles)
        {
            using var activity = Log.Logger.StartActivity("AddWatermark");

            var processedFiles = new List<GetMultipleFilesBytesAsyncItemResult>();

            foreach (var originfile in originfiles)
            {
                string fileExtension = Path.GetExtension(originfile.FileName);
                byte[] processedBytes;

                // 根據副檔名選擇浮水印處理方式
                if (!string.IsNullOrEmpty(fileExtension) && fileExtension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    processedBytes = _watermarkHelper.PdfWatermarkAndGetBytes(_watermarkText, originfile.FileBytes);
                }
                else
                {
                    processedBytes = _watermarkHelper.ImageWatermarkAndGetBytes(_watermarkText, fileExtension, originfile.FileBytes);
                }

                processedFiles.Add(new GetMultipleFilesBytesAsyncItemResult { FileName = originfile.FileName, FileBytes = processedBytes });
            }

            return processedFiles;
        }

        private async Task<GetMultipleFilesBytesAsyncResult> GetFtpResult(string[] fileNamesWithAttachmentsList)
        {
            var ftpResult = await _ftpHelper.GetMultipleFilesBytesAsync(fileNamesWithAttachmentsList, _ftpOption.FixedEcardSupplementFolderPath);
            return ftpResult;
        }

        private ValidateRequestResult 檢查_EcardSupplementRequest(EcardSupplementRequest req)
        {
            var id = req.ID;
            var suppNo = req.SupplementNo;
            var result = new ValidateRequestResult();

            bool 必要欄位不能為空值 = string.IsNullOrEmpty(req.ID) || string.IsNullOrEmpty(req.SupplementNo);

            if (必要欄位不能為空值)
            {
                return new ValidateRequestResult
                {
                    IsValid = false,
                    Response = new EcardSupplementResponse(回覆代碼.必要欄位為空值),
                    ErrorType = "必要欄位為空值",
                };
            }

            var fileNames = new[]
            {
                req.AppendixFileName_01,
                req.AppendixFileName_02,
                req.AppendixFileName_03,
                req.AppendixFileName_04,
                req.AppendixFileName_05,
                req.AppendixFileName_06,
            };

            bool 欄位值長度是否過長 = req.ID.Length > 11 || req.SupplementNo.Length > 20 || req.MyDataNo?.Length > 36;
            bool 檔案名稱是否過長 = fileNames.Any(f => f?.Length > 100);

            if (檔案名稱是否過長 || 欄位值長度是否過長)
            {
                return new ValidateRequestResult
                {
                    IsValid = false,
                    Response = new EcardSupplementResponse(回覆代碼.長度過長),
                    ErrorType = "資料長度過長",
                };
            }

            return new ValidateRequestResult { IsValid = true };
        }

        private string[] 取出檔案名稱(EcardSupplementRequest req)
        {
            var fileNames = typeof(EcardSupplementRequest)
                .GetProperties()
                .Where(p => p.Name.StartsWith("AppendixFileName_") && p.GetValue(req) is string value && !string.IsNullOrEmpty(value))
                .Select(p => (string)p.GetValue(req)!)
                .ToArray();
            return fileNames;
        }

        private List<ECardSupplementInfoContext> GenerateECardSupplementInfoContext(
            List<ECardSupplementInfoGroupByApplyNo> 群組補件所需相關資訊,
            List<GetMultipleFilesBytesAsyncItemResult> 浮水印加工後檔案,
            EcardSupplementRequest req,
            DateTime now
        )
        {
            var eCardSupplementInfoContexts = new List<ECardSupplementInfoContext>();

            foreach (var applyInfoGroupByApplyNo in 群組補件所需相關資訊)
            {
                var context = new ECardSupplementInfoContext();

                context.ApplyNo = applyInfoGroupByApplyNo.ApplyNo;
                var firstHandleInfos = applyInfoGroupByApplyNo.ECardSupplementInfos.Where(x => x.CardStatus == CardStatus.補件作業中).ToList();

                foreach (var handleInfo in firstHandleInfos)
                {
                    var handle = new HandleInfo();
                    handle.HandleSeqNo = handleInfo.HandleSeqNo;
                    handle.ApplyNo = handleInfo.ApplyNo;
                    handle.OriginCardStatus = handleInfo.CardStatus;
                    handle.AfterCardStatus = 計算補件後卡片狀態(handleInfo.Source, handleInfo.CardStep.Value);
                    context.HandleInfos.Add(handle);
                }

                // 新增 Process
                context.Processes.Add(
                    new Reviewer_ApplyCreditCardInfoProcess
                    {
                        ApplyNo = context.ApplyNo,
                        Process = CardStatus.補回件.ToString(),
                        StartTime = now,
                        EndTime = now,
                        Notes = $"完成附件補檔FROM ECard;補件編號:{req.SupplementNo};MyData編號:{req.MyDataNo}",
                        ProcessUserId = UserIdConst.SYSTEM,
                    }
                );

                context.Processes.AddRange(
                    firstHandleInfos
                        .Select(x => new Reviewer_ApplyCreditCardInfoProcess
                        {
                            ApplyNo = context.ApplyNo,
                            Process = 計算補件後卡片狀態(x.Source, x.CardStep.Value).ToString(),
                            StartTime = now,
                            EndTime = now,
                            Notes = $"完成附件補檔FROM ECard;({(x.UserType == UserType.正卡人 ? "正卡" : "附卡")}_{x.ApplyCardType})",
                            ProcessUserId = UserIdConst.SYSTEM,
                        })
                        .ToList()
                );

                // 新增 CardRecord
                context.CardRecords.AddRange(
                    firstHandleInfos.Select(x => new Reviewer_CardRecord
                    {
                        ApplyNo = context.ApplyNo,
                        CardStatus = 計算補件後卡片狀態(x.Source, x.CardStep.Value),
                        ApproveUserId = UserIdConst.SYSTEM,
                        AddTime = now,
                        HandleNote = $"完成附件補檔FROM ECard;({(x.UserType == UserType.正卡人 ? "正卡" : "附卡")}_{x.ApplyCardType})",
                        HandleSeqNo = x.HandleSeqNo,
                    })
                );
                var prePageNo = applyInfoGroupByApplyNo.ECardSupplementInfos.First().PrePageNo;

                foreach (var item in 浮水印加工後檔案)
                {
                    var fileId = Guid.NewGuid();

                    // 建立檔案紀錄
                    var cardInfoFile = new Reviewer_ApplyCreditCardInfoFile
                    {
                        ApplyNo = context.ApplyNo,
                        FileId = fileId,
                        AddTime = now,
                        Page = ++prePageNo,
                        Process = CardStatus.補回件.ToString(),
                        AddUserId = UserIdConst.SYSTEM,
                        IsHistory = "N",
                        DBName = "ScoreSharp_File",
                        Note = $"完成附件補檔FROM ECard;補件編號:{req.SupplementNo};MyData編號:{req.MyDataNo}",
                    };
                    context.FilesLog.Add(cardInfoFile);

                    // 建立檔案
                    var applyFile = new Reviewer_ApplyFile
                    {
                        ApplyNo = context.ApplyNo,
                        FileId = fileId,
                        FileName = $"{context.ApplyNo}_{item.FileName}",
                        FileContent = item.FileBytes,
                        FileType = FileType.申請書相關,
                    };

                    context.ApplyFiles.Add(applyFile);
                }

                eCardSupplementInfoContexts.Add(context);
            }

            return eCardSupplementInfoContexts;
        }

        private async Task PrepareScoreSharpMainContext(List<ECardSupplementInfoContext> context, DateTime now)
        {
            var applyNos = context.Select(x => x.ApplyNo).Distinct().ToList();

            await _scoreSharpContext
                .Reviewer_ApplyCreditCardInfoMain.Where(x => applyNos.Contains(x.ApplyNo))
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(x => x.LastUpdateUserId, UserIdConst.SYSTEM).SetProperty(x => x.LastUpdateTime, now)
                );
        }

        private async Task<List<ECardSupplementInfoGroupByApplyNo>> 取出需補件案件資訊(string id)
        {
            List<ECardSupplementInfo> 補件所需相關資訊 = new();
            using (var conn = _scoreSharpDapperContext.CreateScoreSharpConnection())
            {
                SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                    sql: "Usp_GetECardSupplementInfo",
                    param: new { ID = id },
                    commandType: CommandType.StoredProcedure
                );
                補件所需相關資訊 = results.Read<ECardSupplementInfo>().ToList();
            }

            var 補件所需相關資訊GroupByApplyNo = 補件所需相關資訊
                .GroupBy(x => x.ApplyNo)
                .Select(x => new ECardSupplementInfoGroupByApplyNo { ApplyNo = x.Key, ECardSupplementInfos = x.ToList() })
                .ToList();

            return 補件所需相關資訊GroupByApplyNo;
        }

        private CardStatus 計算補件後卡片狀態(Source source, CardStep cardStep) =>
            (source, cardStep) switch
            {
                (Source.紙本, CardStep.月收入確認) => CardStatus.紙本件_待月收入預審,
                (Source.紙本, CardStep.人工徵審) => CardStatus.補回件,
                (Source.APP, CardStep.月收入確認) => CardStatus.網路件_待月收入預審,
                (Source.APP, CardStep.人工徵審) => CardStatus.補回件,
                (Source.ECARD, CardStep.月收入確認) => CardStatus.網路件_待月收入預審,
                (Source.ECARD, CardStep.人工徵審) => CardStatus.補回件,
                _ => throw new ArgumentException("Invalid source or card step"),
            };
    }
}
