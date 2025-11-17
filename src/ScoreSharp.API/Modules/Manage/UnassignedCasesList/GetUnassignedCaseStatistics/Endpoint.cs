using ScoreSharp.API.Modules.Manage.UnassignedCasesList.GetUnassignedCaseStatistics;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList
{
    public partial class UnassignedCasesListController
    {
        ///<summary>
        /// 取得待派案案件統計
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /UnassignedCasesList/GetUnassignedCaseStatistics
        ///
        /// 統計各分案類型的未分派案件數量：
        ///
        ///     1. 網路件月收入預審_姓名檢核Y清單 - 案件中有任何一張卡（正卡或附卡）姓名檢核為Y
        ///     2. 網路件月收入預審_姓名檢核N清單 - 案件中所有卡（正卡和附卡）姓名檢核都不是Y
        ///     3. 網路件人工徵信中
        ///     4. 紙本件月收入預審_姓名檢核Y清單 - 案件中有任何一張卡（正卡或附卡）姓名檢核為Y
        ///     5. 紙本件月收入預審_姓名檢核N清單 - 案件中所有卡（正卡和附卡）姓名檢核都不是Y
        ///     6. 紙本件人工徵信中
        ///     7. 總件數
        ///
        /// </remarks>
        /// <response code="200">
        /// 成功取得統計資料
        /// </response>
        ///<returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetUnassignedCaseStatisticsResponse>))]
        [EndpointSpecificExample(
            typeof(取得未分派案件統計_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetUnassignedCaseStatistics")]
        public async Task<IResult> GetUnassignedCaseStatistics()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.GetUnassignedCaseStatistics
{
    public record Query() : IRequest<ResultResponse<GetUnassignedCaseStatisticsResponse>>;

    public class Handler(IReviewerHelper reviewerHelper) : IRequestHandler<Query, ResultResponse<GetUnassignedCaseStatisticsResponse>>
    {
        public async Task<ResultResponse<GetUnassignedCaseStatisticsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var baseData = await reviewerHelper.GetApplyCreditCardBaseData(
                new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.網路件_待月收入預審, CardStatus.紙本件_待月收入預審, CardStatus.人工徵信中],
                    CurrentHandleUserId = "<NULL>",
                }
            );
            var statistics = 統計未分派案件(baseData.ToList());
            return ApiResponseHelper.Success(statistics);
        }

        private GetUnassignedCaseStatisticsResponse 統計未分派案件(List<GetApplyCreditCardBaseDataResult> baseData)
        {
            if (!baseData.Any())
            {
                return new GetUnassignedCaseStatisticsResponse();
            }

            // 按案件編號分組，建立每個案件的完整資訊
            var caseInfos = baseData
                .GroupBy(x => x.ApplyNo)
                .Select(g => new
                {
                    ApplyNo = g.Key,
                    Source = g.First().Source,
                    // 有任何一張卡（正卡或附卡）姓名檢核為Y
                    HasNameCheckedY = g.First().HasNameCheckedY,
                    // 該案件所有待派案的卡片狀態
                    CardStatuses = g.SelectMany(x => x.ApplyCardList.Select(y => y.CardStatus)).ToList(),
                })
                .ToList();

            // 使用 LINQ 統計各分類
            var 網路件月收入預審_姓名檢核Y清單 = caseInfos.Count(c =>
                c.Source != Source.紙本 && c.CardStatuses.Contains(CardStatus.網路件_待月收入預審) && c.HasNameCheckedY
            );

            var 網路件月收入預審_姓名檢核N清單 = caseInfos.Count(c =>
                c.Source != Source.紙本 && c.CardStatuses.Contains(CardStatus.網路件_待月收入預審) && !c.HasNameCheckedY
            );

            var 網路件人工審查 = caseInfos.Count(c => c.Source != Source.紙本 && c.CardStatuses.Contains(CardStatus.人工徵信中));

            var 紙本件月收入預審_姓名檢核Y清單 = caseInfos.Count(c =>
                c.Source == Source.紙本 && c.CardStatuses.Contains(CardStatus.紙本件_待月收入預審) && c.HasNameCheckedY
            );

            var 紙本件月收入預審_姓名檢核N清單 = caseInfos.Count(c =>
                c.Source == Source.紙本 && c.CardStatuses.Contains(CardStatus.紙本件_待月收入預審) && !c.HasNameCheckedY
            );

            var 紙本件人工審查 = caseInfos.Count(c => c.Source == Source.紙本 && c.CardStatuses.Contains(CardStatus.人工徵信中));

            var 總件數 = caseInfos.Count;

            return new GetUnassignedCaseStatisticsResponse
            {
                new()
                {
                    Id = (int)CaseAssignmentType.網路件月收入預審_姓名檢核Y清單,
                    Name = CaseAssignmentType.網路件月收入預審_姓名檢核Y清單.ToName(),
                    Value = 網路件月收入預審_姓名檢核Y清單,
                },
                new()
                {
                    Id = (int)CaseAssignmentType.網路件月收入預審_姓名檢核N清單,
                    Name = CaseAssignmentType.網路件月收入預審_姓名檢核N清單.ToName(),
                    Value = 網路件月收入預審_姓名檢核N清單,
                },
                new()
                {
                    Id = (int)CaseAssignmentType.網路件人工徵信中,
                    Name = CaseAssignmentType.網路件人工徵信中.ToName(),
                    Value = 網路件人工審查,
                },
                new()
                {
                    Id = (int)CaseAssignmentType.紙本件月收入預審_姓名檢核Y清單,
                    Name = CaseAssignmentType.紙本件月收入預審_姓名檢核Y清單.ToName(),
                    Value = 紙本件月收入預審_姓名檢核Y清單,
                },
                new()
                {
                    Id = (int)CaseAssignmentType.紙本件月收入預審_姓名檢核N清單,
                    Name = CaseAssignmentType.紙本件月收入預審_姓名檢核N清單.ToName(),
                    Value = 紙本件月收入預審_姓名檢核N清單,
                },
                new()
                {
                    Id = (int)CaseAssignmentType.紙本件人工徵信中,
                    Name = CaseAssignmentType.紙本件人工徵信中.ToName(),
                    Value = 紙本件人工審查,
                },
                new()
                {
                    Id = 7,
                    Name = "總件數",
                    Value = 總件數,
                },
            };
        }
    }
}
