using System.Data;
using Azure;
using ScoreSharp.Common.Constant;
using ScoreSharp.Common.Helpers;
using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardSupplementMyDataFail;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// ECARD 網路補件MYDATA失敗
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EcardSupplementMyDataFailResponse))]
        [EndpointSpecificExample(
            typeof(ECARD_補件MYDATA失敗_匯入成功_0000_ReqEx),
            typeof(ECARD_補件MYDATA失敗_必要欄位為空值_0001_ReqEx),
            typeof(ECARD_補件MYDATA失敗_長度過長_0002_ReqEx),
            typeof(ECARD_補件MYDATA失敗_其它異常訊息_查無申請書資料_0003_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(ECARD_補件MYDATA失敗_匯入成功_0000_ResEx),
            typeof(ECARD_補件MYDATA失敗_必要欄位為空值_0001_ResEx),
            typeof(ECARD_補件MYDATA失敗_長度過長_0002_ResEx),
            typeof(ECARD_補件MYDATA失敗_其它異常訊息_查無申請書資料_0003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [HttpPost]
        [OpenApiOperation("EcardSupplementMyDataFail")]
        public async Task<IResult> EcardSupplementMyDataFail([FromBody] EcardSupplementMyDataFailRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardSupplementMyDataFail
{
    public record Command(EcardSupplementMyDataFailRequest ecardSupplementMyDataFailRequest) : IRequest<EcardSupplementMyDataFailResponse>;

    public class Handler : IRequestHandler<Command, EcardSupplementMyDataFailResponse>
    {
        private readonly string SOURCE = "EcardSupplementMyDataFail";
        private readonly ILogger<Handler> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly ScoreSharpContext _scoreSharpContext;
        private readonly IScoreSharpDapperContext _scoreSharpDapperContext;
        private DateTime _now;

        public Handler(
            ILogger<Handler> logger,
            IDiagnosticContext diagnosticContext,
            ScoreSharpContext scoreSharpContext,
            IScoreSharpDapperContext scoreSharpDapperContext
        )
        {
            _logger = logger;
            _scoreSharpContext = scoreSharpContext;
            _scoreSharpDapperContext = scoreSharpDapperContext;
            _diagnosticContext = diagnosticContext;
            _now = DateTime.Now;
        }

        public async Task<EcardSupplementMyDataFailResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var req = request.ecardSupplementMyDataFailRequest;

            using var _ = _logger.PushProperties(("ID", req.ID));
            _diagnosticContext.SetProperties(("ID", req.ID, false));

            try
            {
                if (string.IsNullOrEmpty(req.ID))
                {
                    _logger.LogError("必要欄位不能為空值 – ID:{@ID}", req.ID);
                    var response = new EcardSupplementMyDataFailResponse(回覆代碼.必要欄位為空值);
                    await 發送錯誤通知(
                        SystemErrorLogTypeConst.Ecard補件MyData失敗資料驗證失敗,
                        "必要欄位不能為空值",
                        request: JsonHelper.序列化物件(request),
                        response: JsonHelper.序列化物件(response)
                    );
                    return response;
                }

                if (req.ID.Length > 11)
                {
                    _logger.LogError("長度過長 – ID:{@ID}  ", req.ID);
                    var response = new EcardSupplementMyDataFailResponse(回覆代碼.長度過長);
                    await 發送錯誤通知(
                        SystemErrorLogTypeConst.Ecard補件MyData失敗資料驗證失敗,
                        "長度過長",
                        request: JsonHelper.序列化物件(request),
                        response: JsonHelper.序列化物件(response)
                    );
                    return response;
                }

                List<ECardSupplementInfo> 補件所需相關資訊;
                using (var conn = _scoreSharpDapperContext.CreateScoreSharpConnection())
                {
                    SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                        sql: "Usp_GetECardSupplementInfo",
                        param: new { ID = req.ID, MyDataNo = String.Empty },
                        commandType: CommandType.StoredProcedure
                    );
                    補件所需相關資訊 = results.Read<ECardSupplementInfo>().ToList();
                }
                var 補件所需相關資訊GroupByApplyNo = 補件所需相關資訊
                    .GroupBy(x => x.ApplyNo)
                    .Select(x => new ECardSupplementInfoGroupByApplyNo { ApplyNo = x.Key, ECardSupplementInfos = x.ToList() })
                    .ToList();

                if (補件所需相關資訊GroupByApplyNo.Count == 0)
                {
                    _logger.LogError("其它異常訊息: {@Error}", $"查無 {req.ID} 對應資料");
                    await 發送錯誤通知(
                        SystemErrorLogTypeConst.補件資料驗證失敗,
                        "其它異常訊息_查無ID對應資料",
                        request: JsonHelper.序列化物件(req),
                        response: JsonHelper.序列化物件(new EcardSupplementMyDataFailResponse(回覆代碼.其它異常訊息))
                    );
                    return new EcardSupplementMyDataFailResponse(回覆代碼.其它異常訊息);
                }

                // 產生補件歷程_卡別紀錄資訊_更改狀態
                List<ECardSupplementInfoContext> supplementContexts = new();

                foreach (var applyInfoGroupByApplyNo in 補件所需相關資訊GroupByApplyNo)
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
                    // 建立補件歷程
                    context.Processes.Add(
                        new Reviewer_ApplyCreditCardInfoProcess
                        {
                            ApplyNo = context.ApplyNo,
                            Process = CardStatus.MyData取回失敗.ToString(),
                            StartTime = _now,
                            EndTime = _now,
                            Notes = $"補件MyData取回失敗FROM ECard;",
                            ProcessUserId = UserIdConst.SYSTEM,
                        }
                    );

                    context.Processes.AddRange(
                        firstHandleInfos
                            .Select(x => new Reviewer_ApplyCreditCardInfoProcess
                            {
                                ApplyNo = context.ApplyNo,
                                Process = 計算補件後卡片狀態(x.Source, x.CardStep.Value).ToString(),
                                StartTime = _now,
                                EndTime = _now,
                                Notes = $"補件MyData取回失敗FROM ECard;({(x.UserType == UserType.正卡人 ? "正卡" : "附卡")}_{x.ApplyCardType})",
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
                            AddTime = _now,
                            HandleNote = $"完成附件補檔FROM ECard;({(x.UserType == UserType.正卡人 ? "正卡" : "附卡")}_{x.ApplyCardType})",
                            HandleSeqNo = x.HandleSeqNo,
                        })
                    );
                    supplementContexts.Add(context);
                }

                // 寫入歷程、卡別紀錄
                var updateProcess = supplementContexts.SelectMany(x => x.Processes).ToList();
                var updateCardRecords = supplementContexts.SelectMany(x => x.CardRecords).ToList();

                await _scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(updateProcess);
                await _scoreSharpContext.Reviewer_CardRecord.AddRangeAsync(updateCardRecords);

                // 更新 Handle 的 CardStatus 與補件資訊清空
                string updateSql =
                    @"
                UPDATE [dbo].[Reviewer_ApplyCreditCardInfoHandle]
                SET [CardStatus] = {1},
                    [ApproveUserId] = NULL,
                    [ApproveTime] = NULL
                WHERE [SeqNo] = {0}";

                var handleSeqAndCardStatus = supplementContexts
                    .SelectMany(x => x.HandleInfos)
                    .Select(x => new { x.HandleSeqNo, x.AfterCardStatus })
                    .Distinct()
                    .ToList();

                foreach (var item in handleSeqAndCardStatus)
                {
                    await _scoreSharpContext.Database.ExecuteSqlRawAsync(updateSql, item.HandleSeqNo, item.AfterCardStatus);
                }

                // 更新 Main 的 lastUpdateUserId、lastUpdateTime
                var applyNos = supplementContexts.Select(x => x.ApplyNo).Distinct().ToList();

                await _scoreSharpContext
                    .Reviewer_ApplyCreditCardInfoMain.Where(x => applyNos.Contains(x.ApplyNo))
                    .ExecuteUpdateAsync(setters =>
                        setters.SetProperty(x => x.LastUpdateUserId, UserIdConst.SYSTEM).SetProperty(x => x.LastUpdateTime, _now)
                    );

                await _scoreSharpContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ECARD補件MYDATA失敗API發生錯誤: {@Error}", ex.ToString());

                await 發送錯誤通知(
                    SystemErrorLogTypeConst.內部程式錯誤,
                    "其它異常訊息_ECARD補件API發生錯誤",
                    request: JsonHelper.序列化物件(request.ecardSupplementMyDataFailRequest),
                    response: JsonHelper.序列化物件(new EcardSupplementMyDataFailResponse(回覆代碼.其它異常訊息)),
                    errorDeatil: ex.ToString()
                );

                return new EcardSupplementMyDataFailResponse(回覆代碼.其它異常訊息);
            }

            return new EcardSupplementMyDataFailResponse(回覆代碼.匯入成功);
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
                    Source = SOURCE,
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
