using ScoreSharp.API.Modules.Manage.ObligedAssignment.GetObligedAssignmentByQueryString;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.ObligedAssignment
{
    public partial class ObligedAssignmentController
    {
        /// <summary>
        /// 查詢強制派案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ObligedAssignment/GetObligedAssignmentByQueryString?ApplyNo=&amp;ID=L199517969&amp;CardStatus=
        ///
        /// </remarks>
        /// <response code="400">
        /// 1. 申請書編號、身份證字號、案件狀態只能輸入一個條件
        /// 2. 申請書編號、身份證字號、案件狀態至少需要一個
        /// </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetObligedAssignmentByQueryStringResponse>>))]
        [EndpointSpecificExample(typeof(查詢強制派案_2000_ResEx), ExampleType = ExampleType.Response, ResponseStatusCode = StatusCodes.Status200OK)]
        [OpenApiOperation("GetObligedAssignmentByQueryString")]
        public async Task<IResult> GetObligedAssignmentByQueryString([FromQuery] GetObligedAssignmentByQueryStringRequest request)
        {
            var result = await this._mediator.Send(new Query(request));

            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.GetObligedAssignmentByQueryString
{
    public record Query(GetObligedAssignmentByQueryStringRequest getObligedAssignmentByQueryStringRequest)
        : IRequest<ResultResponse<List<GetObligedAssignmentByQueryStringResponse>>>;

    public class Handler(IReviewerHelper _reviewerHelper) : IRequestHandler<Query, ResultResponse<List<GetObligedAssignmentByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetObligedAssignmentByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var req = request.getObligedAssignmentByQueryStringRequest;

            var baseData = await _reviewerHelper.GetApplyCreditCardBaseData(
                new GetApplyCreditCardBaseDataDto()
                {
                    ApplyNo = string.IsNullOrWhiteSpace(req.ApplyNo) ? null : req.ApplyNo,
                    ID = string.IsNullOrWhiteSpace(req.ID) ? null : req.ID,
                    CardStatus = req.CardStatus.HasValue ? [req.CardStatus.Value] : null,
                }
            );

            var response = baseData
                .Select(x =>
                {
                    var applyCardList = x.ApplyCardList;
                    return new GetObligedAssignmentByQueryStringResponse
                    {
                        ApplyNo = x.ApplyNo,
                        ID = x.M_ID,
                        CHName = x.M_CHName,
                        CardOwner = x.CardOwner,
                        ApplyCardList = applyCardList
                            .Select(y => new ApplyCardListDto
                            {
                                HandleSeqNo = y.HandleSeqNo,
                                ApplyCardType = y.ApplyCardType,
                                ApplyCardName = y.ApplyCardName,
                                CardStatus = y.CardStatus,
                                CardStep = y.CardStep,
                                UserType = y.UserType,
                                ID = y.ID,
                            })
                            .ToList(),
                        ApproveUserList = applyCardList
                            .Select(y => new ApproveUserDto
                            {
                                ApproveUserId = y.ApproveUserId,
                                ApproveUserName = y.ApproveUserName,
                                ApproveTime = y.ApproveTime.HasValue ? y.ApproveTime.Value : null,
                            })
                            .ToList(),
                    };
                })
                .ToList();

            return ApiResponseHelper.Success(response);
        }
    }
}
