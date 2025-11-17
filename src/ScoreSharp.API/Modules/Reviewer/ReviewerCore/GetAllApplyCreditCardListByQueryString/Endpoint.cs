using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetAllApplyCreditCardListByQueryString;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 查詢全域案件 ByQueryString
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetAllApplyCreditCardListByQueryString?caseStatus=1
        ///
        /// 可以點擊查詢詳細資料規則主要看 IsQuery 是否為 Y，
        /// </remarks>
        /// <response code="400">
        /// 1. 申請書編號、中文姓名、身份證字號只能輸入一個條件
        /// 2. 申請書編號、中文姓名、身份證字號至少需要一個
        /// </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetAllApplyCreditCardListByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(查詢全域案件成功_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [OpenApiOperation("GetAllApplyCreditCardListByQueryString")]
        public async Task<IResult> GetAllApplyCreditCardListByQueryString(
            [FromQuery] GetAllApplyCreditCardListByQueryStringRequest request,
            CancellationToken cancellationToken
        )
        {
            var result = await _mediator.Send(new Query(request), cancellationToken);
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetAllApplyCreditCardListByQueryString
{
    public record Query(GetAllApplyCreditCardListByQueryStringRequest getAllApplyCreditCardListByQueryStringRequest)
        : IRequest<ResultResponse<List<GetAllApplyCreditCardListByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IReviewerHelper reviewerHelper)
        : IRequestHandler<Query, ResultResponse<List<GetAllApplyCreditCardListByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetAllApplyCreditCardListByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var _request = request.getAllApplyCreditCardListByQueryStringRequest;

            string id = string.IsNullOrEmpty(_request.ID) ? null : _request.ID;
            string chName = string.IsNullOrEmpty(_request.CHName) ? null : _request.CHName;
            string applyNo = string.IsNullOrEmpty(_request.ApplyNo) ? null : _request.ApplyNo;

            var dto = new GetApplyCreditCardBaseDataDto()
            {
                ApplyNo = applyNo,
                ID = id,
                CHName = chName,
            };

            var baseData = await reviewerHelper.GetApplyCreditCardBaseData(dto);

            var response = baseData
                .Select(data =>
                {
                    var response = new GetAllApplyCreditCardListByQueryStringResponse();
                    response.ApplyNo = data.ApplyNo;
                    response.CHName = data.M_CHName;
                    response.ID = data.M_ID;
                    response.CaseType = data.CaseType is not null ? data.CaseType : null;
                    response.ApplyDate = data.ApplyDate;

                    var applyCardTypeList = data
                        .ApplyCardList.Where(x => !string.IsNullOrWhiteSpace(x.ApplyCardType))
                        .Select(x => new ApplyCardTypeDto { ApplyCardType = x.ApplyCardType, ApplyCardTypeName = x.ApplyCardName })
                        .ToList();
                    response.ApplyCardTypeList = applyCardTypeList;
                    response.ApplyCardTypeName = String.Join("/", applyCardTypeList.Select(x => $"({x.ApplyCardType}){x.ApplyCardTypeName}"));

                    var cardStatusList = data
                        .ApplyCardList.Select(x => new CardStatusDto { CardStatus = x.CardStatus, CardStatusName = x.CardStatus.ToString() })
                        .ToList();
                    response.CardStatusList = cardStatusList;
                    response.CardStatusName = String.Join("/", cardStatusList.Select(x => x.CardStatusName));

                    response.CurrentHandleUserId = data.CurrentHandleUserId;
                    response.CurrentHandleUserName = data.CurrentHandleUserName;

                    response.IsQuery = cardStatusList.Any(x =>
                        x.CardStatus == CardStatus.網路件初始_待重新發送_必要欄位不能為空值
                        || x.CardStatus == CardStatus.網路件初始_待重新發送_資料長度過長
                        || x.CardStatus == CardStatus.網路件初始_待重新發送_非定義值
                    )
                        ? "N"
                        : "Y";

                    return response;
                })
                .ToList();

            return ApiResponseHelper.Success(response);
        }
    }
}
