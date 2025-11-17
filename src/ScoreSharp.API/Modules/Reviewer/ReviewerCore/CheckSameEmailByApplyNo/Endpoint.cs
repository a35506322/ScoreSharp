using ScoreSharp.API.Modules.Reviewer.ReviewerCore.CheckSameEmailByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 比對Email相同紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/CheckSameEmailByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpPost("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(比對相同電子郵件成功_2000_ResEx),
            typeof(比對相同電子郵件失敗查無此申請書編號_4001_ResEx),
            typeof(比對相同電子郵件失敗內部程式失敗_5000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("CheckSameEmailByApplyNo")]
        public async Task<IResult> CheckSameEmailByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Command(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.CheckSameEmailByApplyNo
{
    public record Command(string applyNo) : IRequest<ResultResponse<string>>;

    public class Handler(
        ScoreSharpContext context,
        IJWTProfilerHelper jwtProfilerHelper,
        IScoreSharpDapperContext dapperContext,
        ILogger<Handler> logger
    ) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var applyCase = await context.Reviewer_ApplyCreditCardInfoMain.AsNoTracking().FirstOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (applyCase is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);
            if (!(applyCase.Source != Source.ECARD || applyCase.Source != Source.APP))
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"申請書編號 {applyNo} 不是 Web 案件");
            if (string.IsNullOrEmpty(applyCase.EMail))
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"申請書編號 {applyNo} 沒有 EMail 紀錄");

            var sql = @"EXEC [ScoreSharp].[dbo].[Usp_CheckSameEmail] @ApplyNo = @ApplyNo";
            var multi = await context.Database.GetDbConnection().QueryMultipleAsync(sql, new { ApplyNo = applyNo });
            var result = await multi.ReadFirstOrDefaultAsync<CheckSameEmailByApplyNoResult>();
            var traces = await multi.ReadAsync<Reviewer_CheckTrace>();
            traces = traces.Select(trace =>
            {
                trace.CurrentUserType = UserType.正卡人;
                return trace;
            });

            var digistCheckInfo = await context.Reviewer_BankTrace.FirstOrDefaultAsync(x =>
                x.ApplyNo == applyNo && x.ID == applyCase.ID && x.UserType == applyCase.UserType
            );
            digistCheckInfo.SameEmail_Flag = result.CompareResult;
            digistCheckInfo.SameEmail_CheckRecord = null;
            digistCheckInfo.SameEmail_UpdateUserId = null;
            digistCheckInfo.SameEmail_UpdateTime = null;
            digistCheckInfo.SameEmail_IsError = null;

            await context
                .Reviewer_CheckTrace.Where(x => x.CurrentApplyNo == applyNo && x.CheckType == CheckTraceType.網路件EMAIL比對)
                .ExecuteDeleteAsync();
            if (traces.Any())
                await context.Reviewer_CheckTrace.AddRangeAsync(traces);
            await context.SaveChangesAsync();

            return ApiResponseHelper.Success(applyNo, $"申請書編號 {applyNo} 比對相同Email成功，結果為 {result.CompareResult}");
        }
    }
}
