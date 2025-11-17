using System.Diagnostics.Metrics;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalCommunicateByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得內部溝通紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetInternalCommunicateByApplyNo/20241128M4480
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetInternalCommunicateByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得內部溝通紀錄_2000_ResEx),
            typeof(取得內部溝通紀錄查無此ID_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetInternalCommunicateByApplyNo")]
        public async Task<IResult> GetInternalCommunicateByApplyNo([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalCommunicateByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetInternalCommunicateByApplyNoResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<GetInternalCommunicateByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetInternalCommunicateByApplyNoResponse>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var applyNo = request.applyNo;

            var single = await context.Reviewer_InternalCommunicate.AsNoTracking().SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (single is null)
                return ApiResponseHelper.NotFound<GetInternalCommunicateByApplyNoResponse>(null, applyNo);

            string userName = string.Empty;
            if (!string.IsNullOrEmpty(single.SupplementContactRecords_UserId))
            {
                var userInfo = await context
                    .OrgSetUp_User.AsNoTracking()
                    .SingleOrDefaultAsync(x => x.UserId == single.SupplementContactRecords_UserId);
                userName = userInfo?.UserName ?? string.Empty;
            }

            var response = mapper.Map<GetInternalCommunicateByApplyNoResponse>(single);
            response.SupplementContactRecords_UserName = userName;

            return ApiResponseHelper.Success(response);
        }
    }
}
