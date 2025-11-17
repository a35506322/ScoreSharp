using System;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetCardRecordsByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得卡別紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetCardRecordsByApplyNo/20250331B3106
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetCardRecordsByApplyNoResponse>>))]
        [EndpointSpecificExample(
            typeof(取得卡別紀錄_2000_ResEx),
            typeof(取得卡別紀錄查無此ID_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCardRecordsByApplyNo")]
        public async Task<IResult> GetCardRecordsByApplyNo([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));

            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetCardRecordsByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<List<GetCardRecordsByApplyNoResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<List<GetCardRecordsByApplyNoResponse>>>
    {
        public async Task<ResultResponse<List<GetCardRecordsByApplyNoResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var entities = await context
                .Reviewer_CardRecord.AsNoTracking()
                .Where(x => x.ApplyNo == applyNo)
                .OrderByDescending(x => x.AddTime)
                .ToListAsync();

            if (entities.Count == 0)
            {
                return ApiResponseHelper.NotFound<List<GetCardRecordsByApplyNoResponse>>(null, applyNo);
            }

            var userDic = await context.OrgSetUp_User.AsNoTracking().ToDictionaryAsync(x => x.UserId, x => x.UserName);

            var result = mapper.Map<List<GetCardRecordsByApplyNoResponse>>(entities);

            result.ForEach(x =>
            {
                x.ApproveUserName = userDic.GetValueOrDefault(x.ApproveUserId);
            });

            return ApiResponseHelper.Success(result);
        }
    }
}
