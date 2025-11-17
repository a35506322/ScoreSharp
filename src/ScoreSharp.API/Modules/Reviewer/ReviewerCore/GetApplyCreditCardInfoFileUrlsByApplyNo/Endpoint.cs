using System.Data;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoFileUrlsByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 取得申請書附件URL By申請書編號
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router:
        ///     /ReviewerCore/GetApplyCreditCardInfoFileUrlsByApplyNo/20241127X8342
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetApplyCreditCardInfoFileUrlsByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得申請書_附件URL_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplyCreditCardInfoFileUrlsByApplyNo")]
        public async Task<IResult> GetApplyCreditCardInfoFileUrlsByApplyNo([FromRoute] string applyNo, CancellationToken cancellationToken) =>
            Results.Ok(await _mediator.Send(new Query(applyNo), cancellationToken));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoFileUrlsByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetApplyCreditCardInfoFileUrlsByApplyNoResponse>>;

    public class Handler(ScoreSharpContext scoreSharpContext)
        : IRequestHandler<Query, ResultResponse<GetApplyCreditCardInfoFileUrlsByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetApplyCreditCardInfoFileUrlsByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var fileUrls = await scoreSharpContext
                .Reviewer_ApplyCreditCardInfoFile.AsNoTracking()
                .Where(t => t.ApplyNo == request.applyNo)
                .Select(t => "/ReviewerCore/GetApplyCreditCardInfoFileByApplyNoAndFileId/" + t.ApplyNo + "/" + t.FileId)
                .ToListAsync(cancellationToken);

            return ApiResponseHelper.Success(new GetApplyCreditCardInfoFileUrlsByApplyNoResponse { FileUrls = fileUrls }, "取得申請書附件URL成功");
        }
    }
}
