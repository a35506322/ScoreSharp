using ScoreSharp.API.Modules.Manage.TransferCase.GetStatisticsTransferCasesByQueryString;
using ScoreSharp.API.Modules.Manage.UnassignedCasesList.BatchAssignCasesAutomatically;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.TransferCase
{
    public partial class TransferCaseController
    {
        /// <summary>
        /// 取得統計調撥案件
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /TransferCase/GetStatisticsTransferCasesByQueryString
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetStatisticsTransferCasesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得統計調撥案件_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetStatisticsTransferCasesByQueryString")]
        public async Task<IResult> GetStatisticsTransferCasesByQueryString()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.TransferCase.GetStatisticsTransferCasesByQueryString
{
    public record Query() : IRequest<ResultResponse<List<GetStatisticsTransferCasesByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper, IReviewerHelper reviewerHelper)
        : IRequestHandler<Query, ResultResponse<List<GetStatisticsTransferCasesByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetStatisticsTransferCasesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var 派案組織 = jwtProfilerHelper.CaseDispatchGroups;
            var queryAssignmentUsers = await GetAssignmentUsers(派案組織);

            var userIds = queryAssignmentUsers.Select(x => x.UserId).OrderBy(x => x).ToHashSet();
            var baseDatas = await reviewerHelper.GetApplyCreditCardBaseData(
                new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.網路件_待月收入預審, CardStatus.紙本件_待月收入預審, CardStatus.人工徵信中],
                    CurrentHandleUserIds = string.Join(",", userIds),
                }
            );

            var groupedBaseDatasDict = baseDatas.GroupBy(x => x.CurrentHandleUserId).ToDictionary(x => x.Key, x => x.ToList());
            List<GetStatisticsTransferCasesByQueryStringResponse> responses = new();
            foreach (var user in userIds)
            {
                var userName = queryAssignmentUsers.FirstOrDefault(x => x.UserId == user)?.UserName ?? string.Empty;
                var isExist = groupedBaseDatasDict.TryGetValue(user, out var baseDataList);

                if (!isExist)
                {
                    responses.Add(
                        new GetStatisticsTransferCasesByQueryStringResponse
                        {
                            UserId = user,
                            UserName = userName,
                            TransferCases =
                            [
                                new TransferCasesDto { TransferCaseType = TransferCaseType.網路件月收入預審, CaseCount = 0 },
                                new TransferCasesDto { TransferCaseType = TransferCaseType.網路件人工徵信中, CaseCount = 0 },
                                new TransferCasesDto { TransferCaseType = TransferCaseType.紙本件月收入預審, CaseCount = 0 },
                                new TransferCasesDto { TransferCaseType = TransferCaseType.紙本件人工徵信中, CaseCount = 0 },
                            ],
                            TransferCasesTotalCount = 0,
                        }
                    );
                    continue;
                }

                var caseByType = baseDataList.Select(caseGroup =>
                {
                    var cardStatuses = caseGroup.ApplyCardList.Select(x => x.CardStatus).ToHashSet();

                    var is紙本 = caseGroup.Source == Source.紙本;
                    var is月收入預審 = cardStatuses.Contains(is紙本 ? CardStatus.紙本件_待月收入預審 : CardStatus.網路件_待月收入預審);
                    var is人工徵信 = cardStatuses.Contains(CardStatus.人工徵信中);

                    return new
                    {
                        is紙本,
                        is月收入預審,
                        is人工徵信,
                    };
                });

                var 紙本件月收入預審 = caseByType.Count(c => c.is紙本 && c.is月收入預審);
                var 紙本件人工徵信中 = caseByType.Count(c => c.is紙本 && c.is人工徵信);
                var 網路件月收入預審 = caseByType.Count(c => !c.is紙本 && c.is月收入預審);
                var 網路件人工徵信中 = caseByType.Count(c => !c.is紙本 && c.is人工徵信);
                var 總件數 = caseByType.Count();

                responses.Add(
                    new GetStatisticsTransferCasesByQueryStringResponse
                    {
                        UserId = user,
                        UserName = userName,
                        TransferCases =
                        [
                            new TransferCasesDto { TransferCaseType = TransferCaseType.網路件月收入預審, CaseCount = 網路件月收入預審 },
                            new TransferCasesDto { TransferCaseType = TransferCaseType.網路件人工徵信中, CaseCount = 網路件人工徵信中 },
                            new TransferCasesDto { TransferCaseType = TransferCaseType.紙本件月收入預審, CaseCount = 紙本件月收入預審 },
                            new TransferCasesDto { TransferCaseType = TransferCaseType.紙本件人工徵信中, CaseCount = 紙本件人工徵信中 },
                        ],
                        TransferCasesTotalCount = 總件數,
                    }
                );
            }
            return ApiResponseHelper.Success(responses.ToList());
        }

        private async Task<List<QueryAssignmentUsersResult>> GetAssignmentUsers(List<CaseDispatchGroup> 派案組織)
        {
            var sql = @"EXEC [dbo].[Usp_GetAssignmentUsers] @CaseDispatchGroups";

            var caseDispatchGroupsString = string.Join(",", 派案組織.Select(x => (int)x));
            var queryAssignmentUsersResponse = await context
                .Database.GetDbConnection()
                .QueryAsync<QueryAssignmentUsersResult>(sql, new { CaseDispatchGroups = caseDispatchGroupsString });

            return queryAssignmentUsersResponse.ToList();
        }
    }
}
