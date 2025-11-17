using System.Data;
using Org.BouncyCastle.Ocsp;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Constant;
using ScoreSharp.Common.Helpers;
using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardMyDataFail;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// EcardMyData取件失敗API
        /// </summary>
        /// <remarks>
        /// 此 api 的 request content-type 是 application/x-www-form-urlencoded
        ///
        ///     範例:
        ///
        ///         P_ID=K12798732&amp;
        ///         APPLY_NO:20250508H5563&amp;
        ///         MYDATA_NO=e37b48ca-82da-49da-a605-bdc23b082186
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EcardMyDataFailResponse))]
        [EndpointSpecificExample(
            typeof(ECARD_MyData取件失敗_匯入成功_0000_ReqEx),
            typeof(ECARD_MyData取件失敗_必要欄位為空值_0001_ReqEx),
            typeof(ECARD_MyData取件失敗_長度過長_0002_ReqEx),
            typeof(ECARD_MyData取件失敗_其它異常訊息_查無申請書資料_0003_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(ECARD_MyData取件失敗_匯入成功_0000_ResEx),
            typeof(ECARD_MyData取件失敗_必要欄位為空值_0001_ResEx),
            typeof(ECARD_MyData取件失敗_長度過長_0002_ResEx),
            typeof(ECARD_MyData取件失敗_其它異常訊息_查無申請書資料_0003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("EcardMyDataFail")]
        public async Task<IResult> EcardMyDataFail([FromBody] EcardMyDataFailRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardMyDataFail
{
    public record Command(EcardMyDataFailRequest ecardMyDataFailRequest) : IRequest<EcardMyDataFailResponse>;

    public class Handler : IRequestHandler<Command, EcardMyDataFailResponse>
    {
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

        public async Task<EcardMyDataFailResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var _req = request.ecardMyDataFailRequest;
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
                        SystemErrorLogTypeConst.MyData進件失敗資料驗證失敗,
                        檢查Request結果.ErrorType,
                        request: JsonHelper.序列化物件(_req),
                        response: JsonHelper.序列化物件(檢查Request結果.Response),
                        applyNo: applyNo
                    );
                    return 檢查Request結果.Response;
                }

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

                if (附件所需相關資訊?.CardStatus is null || 附件所需相關資訊?.ApplyCardType is null || 附件所需相關資訊?.HandleSeqNo is null)
                {
                    _logger.LogError(
                        "查無等待MyData附件或書面申請等待MyData的申請書資料 – ID:{@ID} 申請書編號:{@ApplyNo} MyData案件編號:{@MyDataNo} ",
                        id,
                        applyNo,
                        mydataNo
                    );
                    await 發送錯誤通知(
                        SystemErrorLogTypeConst.MyData進件失敗資料驗證失敗,
                        "其它異常訊息_查無等待MyData附件或書面申請等待MyData的申請書資料",
                        request: JsonHelper.序列化物件(_req),
                        response: JsonHelper.序列化物件(new EcardMyDataFailResponse(回覆代碼.其它異常訊息)),
                        applyNo: _req.ApplyNo
                    );
                    return new EcardMyDataFailResponse(回覆代碼.其它異常訊息);
                }

                List<Reviewer_ApplyCreditCardInfoProcess> processes = new();

                // 建立取回MyData附件失敗進件資料歷程
                processes.Add(
                    new Reviewer_ApplyCreditCardInfoProcess
                    {
                        ApplyNo = _req.ApplyNo,
                        Process = CardStatus.網路件_MyData取回失敗.ToString(),
                        StartTime = _now,
                        EndTime = _now,
                        Notes = $"MyData取件失敗FROM ECard;MyData編號：{_req.MyDataNo}",
                        ProcessUserId = UserIdConst.SYSTEM,
                    }
                );

                // 狀態轉移與歷程新增
                processes.Add(
                    new Reviewer_ApplyCreditCardInfoProcess
                    {
                        ApplyNo = _req.ApplyNo,
                        Process = 傳換取件後卡片狀態(附件所需相關資訊.CardStatus).ToString(),
                        StartTime = _now.AddSeconds(1),
                        EndTime = _now.AddSeconds(1),
                        Notes = $"MyData取件失敗FROM ECard;(正卡_{附件所需相關資訊.ApplyCardType})",
                        ProcessUserId = UserIdConst.SYSTEM,
                    }
                );

                // Upate DB
                var handle = new Reviewer_ApplyCreditCardInfoHandle()
                {
                    SeqNo = 附件所需相關資訊.HandleSeqNo,
                    CardStatus = 傳換取件後卡片狀態(附件所需相關資訊.CardStatus),
                };
                _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.Attach(handle);
                _scoreSharpContext.Entry(handle).Property(u => u.CardStatus).IsModified = true;

                var main = new Reviewer_ApplyCreditCardInfoMain()
                {
                    ApplyNo = _req.ApplyNo,
                    LastUpdateUserId = UserIdConst.SYSTEM,
                    LastUpdateTime = _now,
                };
                _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.Attach(main);
                _scoreSharpContext.Entry(main).Property(u => u.LastUpdateUserId).IsModified = true;
                _scoreSharpContext.Entry(main).Property(u => u.LastUpdateTime).IsModified = true;

                await _scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);

                await _scoreSharpContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("EcardMyData取件失敗API發生錯誤: {@Error}", ex.ToString());

                await 發送錯誤通知(
                    SystemErrorLogTypeConst.內部程式錯誤,
                    "其它異常訊息_EcardMyData取件失敗發生錯誤",
                    request: JsonHelper.序列化物件(request.ecardMyDataFailRequest),
                    response: JsonHelper.序列化物件(new EcardMyDataFailResponse(回覆代碼.其它異常訊息)),
                    errorDeatil: ex.ToString(),
                    applyNo: request.ecardMyDataFailRequest.ApplyNo
                );

                return new EcardMyDataFailResponse(回覆代碼.其它異常訊息);
            }

            return new EcardMyDataFailResponse(回覆代碼.匯入成功);
        }

        private ValidateRequestResult 檢查Request(EcardMyDataFailRequest request)
        {
            bool 檢查必填 = string.IsNullOrEmpty(request.ID) || string.IsNullOrEmpty(request.ApplyNo) || string.IsNullOrEmpty(request.MyDataNo);

            if (檢查必填)
            {
                return new ValidateRequestResult
                {
                    IsValid = false,
                    Response = new EcardMyDataFailResponse(回覆代碼.必要欄位為空值),
                    ErrorType = "必要欄位為空值",
                };
            }

            var 長度過長檢查 = request.ID.Length > 11 || request.ApplyNo.Length > 13 || request.MyDataNo.Length > 36;
            if (長度過長檢查)
            {
                return new ValidateRequestResult
                {
                    IsValid = false,
                    Response = new EcardMyDataFailResponse(回覆代碼.長度過長),
                    ErrorType = "資料長度過長",
                };
            }
            return new ValidateRequestResult { IsValid = true };
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
                    Source = "EcardMyDataFail",
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

        private CardStatus 傳換取件後卡片狀態(CardStatus cardStatus) =>
            (cardStatus) switch
            {
                CardStatus.網路件_等待MyData附件 => CardStatus.網路件_非卡友_待檢核,
                CardStatus.網路件_書面申請等待MyData => CardStatus.網路件_書面申請等待列印申請書及回郵信封,
                _ => throw new ArgumentException("Invalid cardStatus"),
            };
    }
}
