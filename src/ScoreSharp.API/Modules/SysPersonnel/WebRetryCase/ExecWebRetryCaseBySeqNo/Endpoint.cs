using System.Text.Json.Serialization;
using ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.ExecWebRetryCaseBySeqNo;
using ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.ExecWebRetryCaseBySeqNo.Models;
using ScoreSharp.Common.Adapters.EcardMiddleware;
using ScoreSharp.Common.Adapters.MW3.Models;

namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase
{
    public partial class WebRetryCaseController
    {
        /// <summary>
        /// 執行重試 By SeqNo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<ExecWebRetryCaseBySeqNoResponse>))]
        [EndpointSpecificExample(
            typeof(查詢網路件重試匯入成功_2000_ResEx),
            typeof(查詢網路件重試申請書異常_2000_ResEx),
            typeof(查詢網路件重試附件異常_2000_ResEx),
            typeof(查詢網路件重試查無資料_4001_ResEx),
            typeof(查詢網路件重試案件已重新寄送_4003_ResEx),
            typeof(查詢網路件重試案件Request必要欄位不能為空值_5001_ResEx),
            typeof(查詢網路件重試案件Request資料異常非定義值_5001_ResEx),
            typeof(查詢網路件重試案件Request資料異常資料長度過長_5001_ResEx),
            typeof(查詢網路件重試案件申請書編號重複進件或申請書編號不對_5001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("ExecWebRetryCaseBySeqNo")]
        public async Task<IResult> ExecWebRetryCaseBySeqNo([FromRoute] long seqNo)
        {
            var result = await _mediator.Send(new Command(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.ExecWebRetryCaseBySeqNo
{
    public record Command(long seqNo) : IRequest<ResultResponse<ExecWebRetryCaseBySeqNoResponse>>;

    public class Handler : IRequestHandler<Command, ResultResponse<ExecWebRetryCaseBySeqNoResponse>>
    {
        private readonly ScoreSharpContext _scoreSharpContext;
        public readonly IConfiguration _configuration;
        private readonly IMiddlewareAdapter _middlewareAdapter;
        private readonly IJWTProfilerHelper _jwtHelper;
        private readonly ILogger<Handler> _logger;

        public Handler(
            ScoreSharpContext scoreSharpContext,
            IConfiguration configuration,
            IMiddlewareAdapter middlewareAdapter,
            IJWTProfilerHelper jwtHelper,
            ILogger<Handler> logger
        )
        {
            _scoreSharpContext = scoreSharpContext;
            _configuration = configuration;
            _middlewareAdapter = middlewareAdapter;
            _jwtHelper = jwtHelper;
            _logger = logger;
        }

        public async Task<ResultResponse<ExecWebRetryCaseBySeqNoResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            long seqNo = request.seqNo;

            // 檢查 ISSend 防呆
            var retryCase = await _scoreSharpContext.ReviewerPedding_WebRetryCase.AsNoTracking().SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (retryCase == null)
                return ApiResponseHelper.NotFound(new ExecWebRetryCaseBySeqNoResponse { SeqNo = seqNo }, seqNo.ToString());

            if (retryCase.IsSend == "Y")
                return ApiResponseHelper.BusinessLogicFailed(
                    new ExecWebRetryCaseBySeqNoResponse { SeqNo = seqNo },
                    $"此 RetryCase 編號:{retryCase.SeqNo} 案件已經重新寄送，請確認後再試。"
                );

            EcardNewCaseRequest retryCaseRequest;

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                };

                retryCaseRequest = JsonSerializer.Deserialize<EcardNewCaseRequest>(retryCase.Request, options);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.BusinessLogicFailed(
                    new ExecWebRetryCaseBySeqNoResponse { SeqNo = seqNo },
                    $"Request格式有誤，無法順利轉成Json格式{Environment.NewLine}{ex.ToString()}"
                );
            }

            // 刪除原資料handle、main
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == retryCaseRequest.ApplyNo).ExecuteDeleteAsync();
            await _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.Where(x => x.ApplyNo == retryCaseRequest.ApplyNo).ExecuteDeleteAsync();

            var result = await _middlewareAdapter.PostEcardNewCaseAsync(retryCaseRequest);

            if (!result.IsSuccess)
            {
                _logger.LogError("呼叫EcardMiddleware失敗，錯誤訊息{@errorMessage}", result.ErrorMessage);
                return ApiResponseHelper.CheckThirdPartyApiError<ExecWebRetryCaseBySeqNoResponse>(null, seqNo.ToString());
            }

            var webRetryCase = await _scoreSharpContext.ReviewerPedding_WebRetryCase.SingleOrDefaultAsync(x => x.SeqNo == seqNo);
            webRetryCase.IsSend = "Y";
            webRetryCase.LastSendTtime = DateTime.Now;
            webRetryCase.LastSendUserId = _jwtHelper.UserId;

            // 2025.07.16 找出申請日期，第一筆新增的時間即是申請日期
            var original = await _scoreSharpContext
                .ReviewerPedding_WebRetryCase.AsNoTracking()
                .Where(x => x.ApplyNo == retryCaseRequest.ApplyNo)
                .OrderBy(x => x.AddTime)
                .FirstOrDefaultAsync();

            DateTime originalApplyDate = original.AddTime;

            var main = await _scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == retryCaseRequest.ApplyNo);
            var processes = await _scoreSharpContext
                .Reviewer_ApplyCreditCardInfoProcess.Where(x => x.ApplyNo == retryCaseRequest.ApplyNo)
                .ToListAsync();

            main.ApplyDate = originalApplyDate;
            foreach (var process in processes)
            {
                process.StartTime = originalApplyDate;
                process.EndTime = originalApplyDate;
            }

            await _scoreSharpContext.SaveChangesAsync();
            return ApiResponseHelper.UpdateByIdSuccess(
                new ExecWebRetryCaseBySeqNoResponse
                {
                    SeqNo = seqNo,
                    ReturnCode = result.Result.ID,
                    ReturnCodeMessage = result.Result.Result,
                },
                seqNo.ToString()
            );
        }
    }
}
