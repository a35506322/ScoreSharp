using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetNameCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 取得姓名檢核結果 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetNameCheckLogByApployNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetNameCheckLogResponse>>))]
        [EndpointSpecificExample(
            typeof(取得姓名檢核紀錄_2000_ResEx),
            typeof(取得姓名檢核紀錄查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetNameCheckLogByApplyNo")]
        public async Task<IResult> GetNameCheckLogByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetNameCheckLogByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<List<GetNameCheckLogResponse>>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Query, ResultResponse<List<GetNameCheckLogResponse>>>
    {
        public async Task<ResultResponse<List<GetNameCheckLogResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;

            var nameCheckLogs = await context.Reviewer3rd_NameCheckLog.AsNoTracking().Where(x => x.ApplyNo == applyNo).ToListAsync();

            if (nameCheckLogs is null)
                return ApiResponseHelper.NotFound<List<GetNameCheckLogResponse>>(null, applyNo);

            List<GetNameCheckLogResponse> response = nameCheckLogs
                .Select(item => new GetNameCheckLogResponse
                {
                    SeqNo = item.SeqNo,
                    ApplyNo = item.ApplyNo,
                    ID = item.ID,
                    Name = item.Name,
                    UserType = item.UserType,
                    UserTypeName = item.UserType.ToName(),
                    AMLId = item.AMLId,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    ResponseResult = item.ResponseResult,
                    RcPoint = item.RcPoint,
                })
                .OrderByDescending(x => x.StartTime)
                .ToList();

            return ApiResponseHelper.Success(response);
        }
    }
}
