using ScoreSharp.API.Modules.Reviewer.ReviewerCore.CheckInternalIPByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 比對行內IP By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/CheckInternalIPByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [Obsolete]
        [HttpPost("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(比對行內IP成功_2000_ResEx),
            typeof(比對行內IP失敗查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("CheckInternalIPByApplyNo")]
        public async Task<IResult> CheckInternalIPByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Command(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.CheckInternalIPByApplyNo
{
    public record Command(string applyNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;

            var main = await context.Reviewer_ApplyCreditCardInfoMain.AsNoTracking().FirstOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (main is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            var internalIPs = await context.SetUp_InternalIP.AsNoTracking().Where(x => x.IsActive == "Y").AnyAsync(x => x.IP == main.UserSourceIP);

            string compareResult = internalIPs ? "Y" : "N";

            var digistCheckInfo = await context.Reviewer_BankTrace.FirstOrDefaultAsync(x =>
                x.ApplyNo == applyNo && x.ID == main.ID && x.UserType == main.UserType
            );
            digistCheckInfo.EqualInternalIP_Flag = compareResult;
            digistCheckInfo.EqualInternalIP_CheckRecord = null;
            digistCheckInfo.EqualInternalIP_UpdateUserId = null;
            digistCheckInfo.EqualInternalIP_UpdateTime = null;
            digistCheckInfo.EqualInternalIP_IsError = null;

            await context.SaveChangesAsync();

            return ApiResponseHelper.Success(applyNo, $"申請書編號 {applyNo} 比對行內IP成功，結果為 {compareResult}");
        }
    }
}
