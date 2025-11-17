using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalIPCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 取得行內IP比對紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetInternalIPCheckLogByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetInternalIPCheckLogByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得行內IP比對紀錄_2000_ResEx),
            typeof(取得行內IP比對紀錄查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetInternalIPCheckLogByApplyNo")]
        public async Task<IResult> GetInternalIPCheckLogByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalIPCheckLogByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetInternalIPCheckLogByApplyNoResponse>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Query, ResultResponse<GetInternalIPCheckLogByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetInternalIPCheckLogByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;

            var log = await context.Reviewer_BankTrace.SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人);

            if (log is null)
                return ApiResponseHelper.NotFound<GetInternalIPCheckLogByApplyNoResponse>(null, applyNo);

            var response = new GetInternalIPCheckLogByApplyNoResponse();

            response.ApplyNo = log.ApplyNo;
            response.IsEqualInternalIP = log.EqualInternalIP_Flag;
            response.CheckRecord = log.EqualInternalIP_CheckRecord;
            response.UpdateUserId = log.EqualInternalIP_UpdateUserId;
            response.UpdateTime = log.EqualInternalIP_UpdateTime;
            response.IsError = log.EqualInternalIP_IsError;

            if (response.UpdateUserId is not null)
            {
                var user = await context.OrgSetUp_User.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == response.UpdateUserId);
                response.UpdateUserName = user is null ? null : user.UserName;
            }

            return ApiResponseHelper.Success(response);
        }
    }
}
