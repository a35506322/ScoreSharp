using System.Data;
using ScoreSharp.Common.Helpers;
using ScoreSharp.Middleware.Infrastructures.FileData;
using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardMyDataSuccess;
using ScoreSharp.Watermark;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// EcardMyData取件成功API
        /// </summary>
        /// <remarks>
        /// 此 api 的 request content-type 是 application/x-www-form-urlencoded
        ///
        ///     範例:
        ///
        ///         P_ID=K12798732&amp;
        ///         APPLY_NO:20250508H5563&amp;
        ///         APPENDIX_FILE_NAME_01=20250409_17552282159__3.jpg&amp;
        ///         APPENDIX_FILE_NAME_02=&amp;
        ///         APPENDIX_FILE_NAME_03=&amp;
        ///         APPENDIX_FILE_NAME_04=&amp;
        ///         APPENDIX_FILE_NAME_05=&amp;
        ///         APPENDIX_FILE_NAME_06=&amp;
        ///         MYDATA_NO=e37b48ca-82da-49da-a605-bdc23b082186
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EcardMyDataSuccessResponse))]
        [EndpointSpecificExample(
            typeof(ECARD_MyData取件成功_匯入成功_0000_ReqEx),
            typeof(ECARD_MyData取件成功_必要欄位為空值_0001_ReqEx),
            typeof(ECARD_MyData取件成功_長度過長_0002_ReqEx),
            typeof(ECARD_MyData取件成功_其它異常訊息_查無申請書資料_0003_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(ECARD_MyData取件成功_匯入成功_0000_ResEx),
            typeof(ECARD_MyData取件成功_必要欄位為空值_0001_ResEx),
            typeof(ECARD_MyData取件成功_長度過長_0002_ResEx),
            typeof(ECARD_MyData取件成功_其它異常訊息_查無申請書資料_0003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("EcardMyDataSuccess")]
        public async Task<IResult> EcardMyDataSuccess([FromBody] EcardMyDataSuccessRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardMyDataSuccess
{
    public record Command(EcardMyDataSuccessRequest ecardMyDataSuccessRequest) : IRequest<EcardMyDataSuccessResponse>;

    public class Handler : IRequestHandler<Command, EcardMyDataSuccessResponse>
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

        public async Task<EcardMyDataSuccessResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var _req = request.ecardMyDataSuccessRequest;
            var id = _req.ID;
            var applyNo = _req.ApplyNo;
            var mydataNo = _req.MyDataNo;

            using var _ = _logger.PushProperties(("ApplyNo", applyNo), ("ID", id), ("MyDataNo", mydataNo));
            _diagnosticContext.SetProperties(("ApplyNo", applyNo, false), ("ID", id, false), ("MyDataNo", mydataNo, false));

            try
            {
                var 檢查Request結果 = 檢查Request(_req);
                if (!檢查Request結果.IsValid)
                {
                    _logger.LogError(
                        "{@ErrorType} – ID:{@ID} 申請書編號:{@ApplyNo} MyData案件編號:{@MydataNo} ",
                        檢查Request結果.ErrorType,
                        id,
                        applyNo,
                        mydataNo
                    );
                    await 發送錯誤通知(
                        SystemErrorLogTypeConst.MyData進件成功資料驗證失敗,
                        檢查Request結果.ErrorType,
                        request: JsonHelper.序列化物件(_req),
                        response: JsonHelper.序列化物件(檢查Request結果.Response),
                        applyNo: applyNo
                    );
                    return 檢查Request結果.Response;
                }

                var 附件所需相關資訊 = await 取得附件所需相關資訊(id, applyNo, mydataNo);
                if (附件所需相關資訊?.CardStatus is null || 附件所需相關資訊?.ApplyCardType is null || 附件所需相關資訊?.HandleSeqNo is null)
                {
                    _logger.LogError(
                        "查無等待MyData附件或書面申請等待MyData的申請書資料 – ID:{@ID} 申請書編號:{@ApplyNo} MyData案件編號:{@MyDataNo} ",
                        id,
                        applyNo,
                        mydataNo
                    );
                    await 發送錯誤通知(
                        SystemErrorLogTypeConst.MyData進件成功資料驗證失敗,
                        "其它異常訊息_查無等待MyData附件或書面申請等待MyData的申請書資料",
                        request: JsonHelper.序列化物件(_req),
                        response: JsonHelper.序列化物件(new EcardMyDataSuccessResponse(回覆代碼.其它異常訊息)),
                        applyNo: _req.ApplyNo
                    );
                    return new EcardMyDataSuccessResponse(回覆代碼.其它異常訊息);
                }

                var 檔案名稱 = 取出檔案名稱(_req, cancellationToken);
                var ftpResult = await GetFtpResult(檔案名稱);
                if (ftpResult.Results.Any(x => !x.IsSuccess) || !ftpResult.IsSuccess)
                {
                    string errorMsg = string.IsNullOrEmpty(ftpResult.ErrorMessage)
                        ? String.Join('、', ftpResult.Results.Select(x => x.ErrorMessage))
                        : ftpResult.ErrorMessage;
                    _logger.LogError("FTP 操作過程中發生錯誤");
                    await 發送錯誤通知(
                        SystemErrorLogTypeConst.MyData進件成功資料驗證失敗,
                        "其它異常訊息_FTP 操作過程中發生錯誤",
                        request: JsonHelper.序列化物件(_req),
                        response: JsonHelper.序列化物件(new EcardMyDataSuccessResponse(回覆代碼.其它異常訊息)),
                        errorDeatil: errorMsg,
                        applyNo: _req.ApplyNo
                    );
                    return new EcardMyDataSuccessResponse(回覆代碼.其它異常訊息);
                }

                var 加工後檔案 = 浮水印加工(ftpResult.Results);
                await ExecuteDistributedTransaction(附件所需相關資訊, 加工後檔案, _req);
            }
            catch (Exception ex)
            {
                _logger.LogError("EcardMyData取件成功API發生錯誤: {@Error}", ex.ToString());

                await 發送錯誤通知(
                    SystemErrorLogTypeConst.內部程式錯誤,
                    "其它異常訊息_EcardMyDataSuccess發生錯誤",
                    request: JsonHelper.序列化物件(request.ecardMyDataSuccessRequest),
                    response: JsonHelper.序列化物件(new EcardMyDataSuccessResponse(回覆代碼.其它異常訊息)),
                    errorDeatil: ex.ToString(),
                    applyNo: request.ecardMyDataSuccessRequest.ApplyNo
                );

                return new EcardMyDataSuccessResponse(回覆代碼.其它異常訊息);
            }

            return new EcardMyDataSuccessResponse(回覆代碼.匯入成功);
        }

        private async Task PrepareScoreSharpContext(EcardMyDataSuccessContext context)
        {
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(context.Processes);
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.AddRangeAsync(context.FileLogs);
        }

        private async Task PrepareScoreSharpFileContext(EcardMyDataSuccessContext context)
        {
            await _scoreSharpFileContext.Reviewer_ApplyFile.AddRangeAsync(context.ApplyFiles);
        }

        private async Task PrepareScoreSharpHandleContext(EcardMyDataSuccessContext context)
        {
            var handle = new Reviewer_ApplyCreditCardInfoHandle() { SeqNo = context.HandleSeqNo, CardStatus = context.AfterCardStatus };
            _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.Attach(handle);
            _scoreSharpContext.Entry(handle).Property(u => u.CardStatus).IsModified = true;
        }

        private async Task ExecuteDistributedTransaction(
            EcardMyDataInfoDto 附件相關資訊,
            List<GetMultipleFilesBytesAsyncItemResult> 加工後的檔案,
            EcardMyDataSuccessRequest req
        )
        {
            EcardMyDataSuccessContext ecardMyDataSuccessContext = new();
            ecardMyDataSuccessContext.ApplyNo = req.ApplyNo;
            ecardMyDataSuccessContext.OriginCardStatus = 附件相關資訊.CardStatus;
            ecardMyDataSuccessContext.AfterCardStatus = 轉換取件後卡片狀態(附件相關資訊.CardStatus);
            ecardMyDataSuccessContext.PrePageNo = 附件相關資訊.PrePageNo;
            ecardMyDataSuccessContext.HandleSeqNo = 附件相關資訊.HandleSeqNo;
            ecardMyDataSuccessContext.ApplyCardType = 附件相關資訊.ApplyCardType;

            // 建立成功取回MyData附件晉見資料歷程
            ecardMyDataSuccessContext.Processes.Add(
                new Reviewer_ApplyCreditCardInfoProcess
                {
                    ApplyNo = req.ApplyNo,
                    Process = CardStatus.網路件_MyData取回成功.ToString(),
                    StartTime = _now,
                    EndTime = _now,
                    Notes = $"完成MyData取回FROM ECard;MyData編號:{req.MyDataNo}",
                    ProcessUserId = UserIdConst.SYSTEM,
                }
            );

            // 轉移後的狀態
            ecardMyDataSuccessContext.Processes.Add(
                new Reviewer_ApplyCreditCardInfoProcess
                {
                    ApplyNo = req.ApplyNo,
                    Process = 轉換取件後卡片狀態(附件相關資訊.CardStatus).ToString(),
                    StartTime = _now.AddSeconds(1),
                    EndTime = _now.AddSeconds(1),
                    Notes = $"完成MyData取回FROM ECard;(正卡_{附件相關資訊.ApplyCardType})",
                    ProcessUserId = UserIdConst.SYSTEM,
                }
            );

            int pageCount = 1;
            foreach (var item in 加工後的檔案)
            {
                var fileId = Guid.NewGuid();

                // 建立檔案紀錄
                var cardInfoFile = new Reviewer_ApplyCreditCardInfoFile
                {
                    ApplyNo = req.ApplyNo,
                    Page = (附件相關資訊.PrePageNo ?? 0) + pageCount,
                    Process = CardStatus.網路件_MyData取回成功.ToString(),
                    AddTime = _now,
                    AddUserId = UserIdConst.SYSTEM,
                    FileId = fileId,
                    DBName = "ScoreSharp_File",
                    IsHistory = "N",
                };
                pageCount++;
                ecardMyDataSuccessContext.FileLogs.Add(cardInfoFile);

                // 儲存檔案
                var applyFile = new Reviewer_ApplyFile
                {
                    FileId = fileId,
                    ApplyNo = ecardMyDataSuccessContext.ApplyNo,
                    FileName = $"{ecardMyDataSuccessContext.ApplyNo}_{item.FileName}",
                    FileContent = item.FileBytes,
                    FileType = FileType.申請書相關,
                };
                ecardMyDataSuccessContext.ApplyFiles.Add(applyFile);
            }

            using var fileTransaction = await _scoreSharpFileContext.Database.BeginTransactionAsync();
            using var mainTransaction = await _scoreSharpContext.Database.BeginTransactionAsync();

            try
            {
                // Phase 1: 準備階段 - Context
                await PrepareScoreSharpFileContext(ecardMyDataSuccessContext);
                await PrepareScoreSharpContext(ecardMyDataSuccessContext);
                await PrepareScoreSharpHandleContext(ecardMyDataSuccessContext);
                await PrepareScoreSharpMainContext(ecardMyDataSuccessContext);

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
                        "申請書編號: {@SupplementNo} ID:{@ID} MyDataNo:{@MyDataNo} 分散式交易失敗，開始回滾: {@Error}",
                        req.ApplyNo,
                        req.ID,
                        req.MyDataNo,
                        ex.ToString()
                    );

                    await mainTransaction.RollbackAsync();
                    await fileTransaction.RollbackAsync();

                    _logger.LogError("申請書編號: {@SupplementNo} ID:{@ID} MyDataNo:{@MyDataNo} 回滾成功", req.ApplyNo, req.ID, req.MyDataNo);
                }
                catch (Exception rollbackEx)
                {
                    _logger.LogError(
                        "申請書編號: {@SupplementNo} ID:{@ID} MyDataNo:{@MyDataNo} 回滾失敗，{@Error}",
                        req.ApplyNo,
                        req.ID,
                        req.MyDataNo,
                        rollbackEx.ToString()
                    );
                }

                throw;
            }
            finally
            {
                await fileTransaction.DisposeAsync();
                await mainTransaction.DisposeAsync();
            }
        }

        private ValidateRequestResult 檢查Request(EcardMyDataSuccessRequest request)
        {
            bool 檢查必填 = string.IsNullOrEmpty(request.ID) || string.IsNullOrEmpty(request.ApplyNo) || string.IsNullOrEmpty(request.MyDataNo);

            if (檢查必填)
            {
                return new ValidateRequestResult
                {
                    IsValid = false,
                    Response = new EcardMyDataSuccessResponse(回覆代碼.必要欄位為空值),
                    ErrorType = "必要欄位為空值",
                };
            }

            var fileNames = new[]
            {
                request.AppendixFileName_01,
                request.AppendixFileName_02,
                request.AppendixFileName_03,
                request.AppendixFileName_04,
                request.AppendixFileName_05,
                request.AppendixFileName_06,
            };
            var 長度過長檢查 = request.ID.Length > 11 || request.ApplyNo.Length > 13 || request.MyDataNo.Length > 36;
            if (fileNames.Any(f => f?.Length > 100) || 長度過長檢查)
            {
                return new ValidateRequestResult
                {
                    IsValid = false,
                    Response = new EcardMyDataSuccessResponse(回覆代碼.長度過長),
                    ErrorType = "資料長度過長",
                };
            }
            return new ValidateRequestResult { IsValid = true };
        }

        private string[] 取出檔案名稱(EcardMyDataSuccessRequest request, CancellationToken cancellationToken)
        {
            var fileNames = typeof(EcardMyDataSuccessRequest)
                .GetProperties()
                .Where(p => p.Name.StartsWith("AppendixFileName_") && p.GetValue(request) is string value && !string.IsNullOrEmpty(value))
                .Select(p => (string)p.GetValue(request)!)
                .ToArray();
            return fileNames;
        }

        private async Task<GetMultipleFilesBytesAsyncResult> GetFtpResult(string[] fileNamesWithAttachmentsList)
        {
            var ftpResult = await _ftpHelper.GetMultipleFilesBytesAsync(fileNamesWithAttachmentsList, _ftpOption.FixedEcardSupplementFolderPath);

            return ftpResult;
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

            // 新增錯誤紀錄
            await _scoreSharpContext.System_ErrorLog.AddAsync(
                new System_ErrorLog()
                {
                    ApplyNo = applyNo,
                    Project = SystemErrorLogProjectConst.MIDDLEWARE,
                    Source = "EcardMyDataSuccess",
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

        private async Task<EcardMyDataInfoDto> 取得附件所需相關資訊(string id, string applyNo, string mydataNo)
        {
            EcardMyDataInfoDto 附件所需相關資訊;
            using (var conn = _scoreSharpDapperContext.CreateScoreSharpConnection())
            {
                SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                    sql: "Usp_GetECardMyDataInfo",
                    param: new
                    {
                        ID = id,
                        ApplyNo = applyNo,
                        MyDataNo = mydataNo,
                    },
                    commandType: CommandType.StoredProcedure
                );
                附件所需相關資訊 = results.Read<EcardMyDataInfoDto>().FirstOrDefault();
            }
            return 附件所需相關資訊;
        }

        private async Task PrepareScoreSharpMainContext(EcardMyDataSuccessContext context)
        {
            var main = new Reviewer_ApplyCreditCardInfoMain()
            {
                ApplyNo = context.ApplyNo,
                LastUpdateUserId = UserIdConst.SYSTEM,
                LastUpdateTime = _now,
            };
            _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.Attach(main);
            _scoreSharpContext.Entry(main).Property(u => u.LastUpdateUserId).IsModified = true;
            _scoreSharpContext.Entry(main).Property(u => u.LastUpdateTime).IsModified = true;
        }

        private CardStatus 轉換取件後卡片狀態(CardStatus cardStatus) =>
            (cardStatus) switch
            {
                CardStatus.網路件_等待MyData附件 => CardStatus.網路件_非卡友_待檢核,
                CardStatus.網路件_書面申請等待MyData => CardStatus.網路件_書面申請等待列印申請書及回郵信封,
                _ => throw new ArgumentException("Invalid cardStatus"),
            };
    }
}
