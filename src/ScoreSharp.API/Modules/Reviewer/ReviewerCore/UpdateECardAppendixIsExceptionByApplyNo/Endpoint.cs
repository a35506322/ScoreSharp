using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateECardAppendixIsExceptionByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 修改ECard附件異常註記 By申請書編號
        /// </summary>
        /// <param name="applyNo">PK</param>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/UpdateECardAppendixIsExceptionByApplyNo/20250918B0001
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(修改ECard附件異常註記成功_2000_ResEx),
            typeof(修改ECard附件異常註記查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateECardAppendixIsExceptionByApplyNo")]
        public async Task<IResult> UpdateECardAppendixIsExceptionByApplyNo([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Command(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateECardAppendixIsExceptionByApplyNo
{
    public record Command(string applyNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            string applyNo = request.applyNo;
            DateTime now = DateTime.Now;

            var main = await context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (main is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            main.ECard_AppendixIsException = "N";
            main.LastUpdateUserId = jwtHelper.UserId;
            main.LastUpdateTime = now;

            var log = new Reviewer_ApplyCreditCardInfoProcess();
            log.ApplyNo = applyNo;
            log.Process = ProcessConst.已確認附件異常;
            log.ProcessUserId = jwtHelper.UserId;
            log.StartTime = now;
            log.EndTime = now;

            await context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(log);
            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(applyNo, applyNo);
        }
    }
}
