using ScoreSharp.API.Modules.Manage.TransferCase.GetTransferCasesDetailByQueryString;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.TransferCase
{
    public partial class TransferCaseController
    {
        /// <summary>
        /// 取得調撥案件詳細資料
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /TransferCase/GetTransferCasesDetailByQueryString
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetTransferCasesDetailByQueryStringResponse>>))]
        [EndpointSpecificExample(typeof(取得調撥案件詳細資料_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(取得待派案案件清單_2000_ReqEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetTransferCasesDetailByQueryString")]
        public async Task<IResult> GetTransferCasesDetailByQueryString([FromQuery] GetTransferCasesDetailByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.TransferCase.GetTransferCasesDetailByQueryString
{
    public record Query(GetTransferCasesDetailByQueryStringRequest GetTransferCasesDetailByQueryStringRequest)
        : IRequest<ResultResponse<List<GetTransferCasesDetailByQueryStringResponse>>>;

    public class Handler(IReviewerHelper reviewerHelper) : IRequestHandler<Query, ResultResponse<List<GetTransferCasesDetailByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetTransferCasesDetailByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var tempRequest = request.GetTransferCasesDetailByQueryStringRequest;
            var 徵審人員 = tempRequest.TransferredUserId;
            var 案件類型 = tempRequest.TransferCaseType;

            GetApplyCreditCardBaseDataDto dto = CreateGetApplyCreditCardBaseDataDto(案件類型, 徵審人員);
            var baseDatas = await reviewerHelper.GetApplyCreditCardBaseData(dto);

            var response = baseDatas
                .Select(x => new GetTransferCasesDetailByQueryStringResponse
                {
                    ApplyNo = x.ApplyNo,
                    CHName = x.M_CHName,
                    ID = x.M_ID,
                    ApplyCardTypeList = x
                        .ApplyCardList.Select(c => new ApplyCardTypeDto
                        {
                            CardStatus = c.CardStatus,
                            CardStatusName = c.CardStatusName,
                            ApplyCardType = c.ApplyCardType,
                            ApplyCardTypeName = c.ApplyCardName,
                        })
                        .ToList(),
                    CaseType = x.CaseType,
                    ApplyDate = x.ApplyDate,
                    LastUpdateTime = x.LastUpdateTime,
                    Notes = string.Empty,
                })
                .ToList();

            return ApiResponseHelper.Success(response);
        }

        private GetApplyCreditCardBaseDataDto CreateGetApplyCreditCardBaseDataDto(TransferCaseType 調撥案件類型, string handleUser) =>
            調撥案件類型 switch
            {
                TransferCaseType.網路件月收入預審 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.網路件_待月收入預審],
                    CurrentHandleUserId = handleUser,
                },
                TransferCaseType.網路件人工徵信中 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.人工徵信中],
                    Source = [Source.APP, Source.ECARD],
                    CurrentHandleUserId = handleUser,
                },
                TransferCaseType.紙本件月收入預審 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.紙本件_待月收入預審],
                    CurrentHandleUserId = handleUser,
                },
                TransferCaseType.紙本件人工徵信中 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.人工徵信中],
                    Source = [Source.紙本],
                    CurrentHandleUserId = handleUser,
                },
                _ => throw new ArgumentException("Invalid transfer case type"),
            };
    }
}
