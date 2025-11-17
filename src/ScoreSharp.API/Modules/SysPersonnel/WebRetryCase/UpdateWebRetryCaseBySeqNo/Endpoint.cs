using System.Text.Json.Serialization;
using ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.UpdateWebRetryCaseBySeqNo;

namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase
{
    public partial class WebRetryCaseController
    {
        /// <summary>
        /// 修改單筆網路件 By SeqNo
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(修改單筆網路件_2000_ReqEx),
            typeof(修改單筆網路件格式有誤_4003_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(修改單筆網路件_2000_ResEx),
            typeof(修改單筆網路件格式有誤_4003_ResEx),
            typeof(修改單筆網路件查無資料_4001_ResEx),
            typeof(修改單筆網路件呼叫有誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateWebRetryCaseBySeqNo")]
        public async Task<IResult> UpdateWebRetryCaseBySeqNo([FromRoute] int seqNo, [FromBody] UpdateWebRetryCaseBySeqNoRequest request)
        {
            var result = await _mediator.Send(new Command(seqNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.UpdateWebRetryCaseBySeqNo
{
    public record Command(int seqNo, UpdateWebRetryCaseBySeqNoRequest updateWebRetryCaseBySeqNoRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            long seqNo = request.seqNo;

            if (seqNo != request.updateWebRetryCaseBySeqNoRequest.SeqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            string retryreq = request.updateWebRetryCaseBySeqNoRequest.Request;
            EcardNewCaseRequest newreq;

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
                };

                newreq = JsonSerializer.Deserialize<EcardNewCaseRequest>(retryreq, options);
            }
            catch (Exception ex)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, ex.ToString());
            }

            var entity = await context.ReviewerPedding_WebRetryCase.SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, seqNo.ToString());

            entity.Request = retryreq;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(seqNo.ToString(), seqNo.ToString());
        }
    }
}
