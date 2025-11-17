using System.Transactions;
using ScoreSharp.API.Modules.Manage.Common.Helpers;
using ScoreSharp.API.Modules.Manage.UnassignedCasesList.BatchAssignCasesAutomatically;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList
{
    public partial class UnassignedCasesListController
    {
        /// <summary>
        /// 整批派案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<BatchAssignCasesAutomaticallyResponse>))]
        [EndpointSpecificExample(typeof(整批派案_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(整批派案_2000_ResEx),
            typeof(商業邏輯驗證失敗_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("BatchAssignCasesAutomatically")]
        public async Task<IResult> BatchAssignCasesAutomatically([FromBody] BatchAssignCasesAutomaticallyRequest request)
        {
            var result = await _mediator.Send(new Command(request));

            if (result.ReturnCodeStatus != ReturnCodeStatus.成功)
                return Results.Ok(result);

            return Results.File(result.ReturnData.ResultFile, result.ReturnData.ContentType, result.ReturnData.FileName);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.BatchAssignCasesAutomatically
{
    public record Command(BatchAssignCasesAutomaticallyRequest batchAssignCasesAutomaticallyRequest)
        : IRequest<ResultResponse<BatchAssignCasesAutomaticallyResponse>>;

    public class Handler(
        ScoreSharpContext context,
        IScoreSharpDapperContext dapperContext,
        IReviewerHelper reviewerHelper,
        IJWTProfilerHelper jwtProfilerHelper,
        IManageHelper manageHelper,
        IMiniExcelHelper miniExcelHelper,
        ILogger<Handler> logger
    ) : IRequestHandler<Command, ResultResponse<BatchAssignCasesAutomaticallyResponse>>
    {
        public async Task<ResultResponse<BatchAssignCasesAutomaticallyResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var assignCaseUserInfos = request.batchAssignCasesAutomaticallyRequest.AssignCaseUserInfos;
            var caseAssignmentType = request.batchAssignCasesAutomaticallyRequest.CaseAssignmentType;
            var caseTotalCount = assignCaseUserInfos.Sum(x => x.CaseCount);

            // 查詢派案案件
            var getApplyCreditCardBaseDataDto = CreateGetApplyCreditCardBaseDataDto(caseAssignmentType);
            var applyCreditCardBaseDataList = await reviewerHelper.GetApplyCreditCardBaseData(getApplyCreditCardBaseDataDto);
            string isNameCheckedFilter = IsNameCheckedFilter(caseAssignmentType);
            var assignCasesInfo = applyCreditCardBaseDataList
                .Select(x => new AssignCasesInfo
                {
                    ApplyNo = x.ApplyNo,
                    PromotionUserId = x.PromotionUser,
                    MonthlyIncomeCheckUserId = x.ApplyCardList.FirstOrDefault()?.MonthlyIncomeCheckUserId ?? string.Empty,
                    M_ID = x.M_ID,
                    S1_ID = x.S1_ID,
                    AssignedUserId = string.Empty,
                    M_NameChecked = x.M_NameChecked,
                    S1_NameChecked = x.S1_NameChecked,
                })
                .Where(x =>
                    isNameCheckedFilter == string.Empty ? true
                    : isNameCheckedFilter == "Y" ? x.HasNameCheckedY
                    : !x.HasNameCheckedY
                )
                .OrderBy(x => Guid.NewGuid())
                .Take(caseTotalCount)
                .ToList();

            if (assignCasesInfo.Count == 0)
            {
                return ApiResponseHelper.BusinessLogicFailed<BatchAssignCasesAutomaticallyResponse>(null, $"{caseAssignmentType} 派案案件數量為 0");
            }

            // 查詢派案人員
            var caseDispatchGroups = jwtProfilerHelper.CaseDispatchGroups;
            var queryAssignmentUsersResult = await QueryAssignmentUsers(caseDispatchGroups);

            // 查詢使用者員編
            var userIdToEmployeeNoDic = await context
                .OrgSetUp_User.AsNoTracking()
                .Where(x => assignCaseUserInfos.Select(y => y.AssignedUserId).Contains(x.UserId))
                .ToDictionaryAsync(x => x.UserId, y => y.EmployeeNo);

            var assignedUserDtos = 合併派案人員資料(caseAssignmentType, queryAssignmentUsersResult, assignCaseUserInfos, userIdToEmployeeNoDic);

            // 派案
            var assignCaseResult = await AssignCases(caseAssignmentType, assignCasesInfo, assignedUserDtos);

            // 更新派案結果至資料庫
            var assignedSuccessCase = assignCaseResult.Where(x => x.IsAssigned).ToList();
            var (updateSuccess, failedApplyNos, successResults) = await 更新派案結果(caseAssignmentType, assignedSuccessCase);

            if (!updateSuccess)
            {
                logger.LogWarning(
                    "部分案件資料庫更新失敗。分案類型: {CaseAssignmentType}, 失敗數量: {FailedCount}, 失敗案件: {FailedApplyNos}",
                    caseAssignmentType,
                    failedApplyNos.Count,
                    string.Join(", ", failedApplyNos)
                );

                // 將資料庫更新失敗的案件標記為「資料庫更新失敗」
                var failedApplyNoSet = failedApplyNos.ToHashSet(StringComparer.OrdinalIgnoreCase);
                foreach (var result in assignCaseResult.Where(x => failedApplyNoSet.Contains(x.ApplyNo)))
                {
                    // 新增 Exception 訊息
                    result.ExceptionMessage = "資料庫更新失敗";
                }
            }

            // 回傳Excel
            var excelDtos = assignCaseResult
                .GroupBy(k => k.UserId)
                .ToDictionary(
                    g => $"{g.Key}_{g.First().UserName}_派案結果",
                    g =>
                        g.Select(c => new BatchAssignCasesAutomaticallyExcelDto
                            {
                                ApplyNo = c.ApplyNo,
                                IsAssigned = c.IsAssigned ? "Y" : "N",
                                FilterReason = c.FilterReason,
                                ExceptionMessage = c.ExceptionMessage,
                            })
                            .ToList()
                );
            var stream = await miniExcelHelper.匯出多個工作表ExcelToStream(excelDtos);
            var response = new BatchAssignCasesAutomaticallyResponse
            {
                ResultFile = stream.ToArray(),
                FileName = $"整批派案結果_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            };

            // 根據資料庫更新結果決定回傳狀態
            if (updateSuccess)
            {
                return ApiResponseHelper.Success(response);
            }
            else
            {
                return ApiResponseHelper.Success(response, $"部分案件派案失敗 ({failedApplyNos.Count} 筆),詳情請見 Excel 檔案");
            }
        }

        private async Task<(bool isSuccess, List<string> failedApplyNos, List<AssignCaseResult> successResults)> 更新派案結果(
            CaseAssignmentType caseAssignmentType,
            List<AssignCaseResult> assignedSuccessCase
        )
        {
            var now = DateTime.Now;

            var (cardStatus, cardStep) = 分案類型轉換(caseAssignmentType);

            List<string> failedApplyNos = new();
            List<AssignCaseResult> successResults = new();
            int pageSize = 1000;
            int pageIndex = (int)Math.Ceiling((double)assignedSuccessCase.Count() / pageSize);

            for (int index = 0; index < pageIndex; index++)
            {
                var slice = assignedSuccessCase.Skip(index * pageSize).Take(pageSize).ToList();

                string mainSql = """
                        UPDATE T
                        SET CurrentHandleUserId = @CurrentHandleUserId,
                            PreviousHandleUserId = @PreviousHandleUserId,
                            LastUpdateUserId = @LastUpdateUserId,
                            LastUpdateTime = @LastUpdateTime
                        FROM (
                            SELECT *
                            FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain]
                            WHERE ApplyNo = @ApplyNo
                        ) T
                    """;

                var mainParams = slice
                    .Select(x => new
                    {
                        ApplyNo = x.ApplyNo,
                        CurrentHandleUserId = x.UserId,
                        PreviousHandleUserId = x.UserId,
                        LastUpdateUserId = jwtProfilerHelper.UserId,
                        LastUpdateTime = now,
                    })
                    .ToList();

                string handleSql =
                    cardStatus == CardStatus.人工徵信中
                        ? """
                                UPDATE [dbo].[Reviewer_ApplyCreditCardInfoHandle]
                                SET CardStep = @CardStep,
                                    ReviewerUserId = @ReviewerUserId,
                                    ReviewerTime = @ReviewerTime
                                WHERE ApplyNo = @ApplyNo;
                            """
                        : """
                                UPDATE [dbo].[Reviewer_ApplyCreditCardInfoHandle]
                                SET CardStep = @CardStep
                                WHERE ApplyNo = @ApplyNo;
                            """;

                var handleParams = slice
                    .Select(x => new
                    {
                        ApplyNo = x.ApplyNo,
                        CardStep = cardStep,
                        ReviewerUserId = x.UserId,
                        ReviewerTime = now,
                    })
                    .ToList();

                string processSql = """
                        INSERT INTO [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoProcess]
                        (ApplyNo, Process, StartTime, EndTime, Notes, ProcessUserId)
                        VALUES
                        (@ApplyNo, @Process, @StartTime, @EndTime, @Notes, @ProcessUserId)
                    """;

                var processes = slice
                    .Select(x => new Reviewer_ApplyCreditCardInfoProcess
                    {
                        ApplyNo = x.ApplyNo,
                        Process = cardStatus.ToString(),
                        StartTime = now,
                        EndTime = now,
                        Notes = $"【整批派案】{jwtProfilerHelper.UserName} 指派TO：{x.UserName}",
                        ProcessUserId = jwtProfilerHelper.UserId,
                    })
                    .ToList();

                string statisticSql = """
                        INSERT INTO [ScoreSharp].[dbo].[Reviewer_CaseStatistics]
                        (UserId, AddTime, CaseType, ApplyNo)
                        VALUES
                        (@UserId, @AddTime, @CaseType, @ApplyNo)
                    """;

                var statistics = slice.Select(x => new Reviewer_CaseStatistics
                {
                    UserId = x.UserId,
                    AddTime = now,
                    CaseType = CaseStatisticType.整批派案,
                    ApplyNo = x.ApplyNo,
                });

                try
                {
                    using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                    {
                        using var connection = dapperContext.CreateScoreSharpConnection();
                        await connection.ExecuteAsync(mainSql, mainParams);
                        await connection.ExecuteAsync(handleSql, handleParams);
                        await connection.ExecuteAsync(processSql, processes);
                        await connection.ExecuteAsync(statisticSql, statistics);
                        scope.Complete();
                    }

                    successResults.AddRange(slice);
                }
                catch (Exception ex)
                {
                    var batchFailedApplyNos = slice.Select(x => x.ApplyNo).ToList();
                    failedApplyNos.AddRange(batchFailedApplyNos);

                    logger.LogError("批次修改當前經辦失敗，ApplyNo: {ApplyNos}，錯誤訊息: {@Exception}", string.Join(", ", batchFailedApplyNos), ex);
                }
            }

            bool 是否執行成功 = failedApplyNos.Count == 0;
            return (是否執行成功, failedApplyNos, successResults);
        }

        private async Task<List<AssignCaseResult>> AssignCases(
            CaseAssignmentType caseAssignmentType,
            List<AssignCasesInfo> assignCasesInfos,
            List<AssignedUserDto> assignUserDtos
        )
        {
            List<AssignCaseResult> assignCaseResult = [];
            List<GetApplyCreditCardBaseDataResult> remainingApplyCreditCardBaseDataList = [];
            Dictionary<string, int> 已分派案件數量紀錄 = assignUserDtos.ToDictionary(x => x.UserId, x => 0);

            // 查詢利害關係人
            var stakeholders = await context.Reviewer_Stakeholder.AsNoTracking().Where(x => x.IsActive == "Y").ToListAsync();
            var stakeholderLookup = stakeholders.ToLookup(x => x.ID, StringComparer.OrdinalIgnoreCase);

            var 是否為人工徵信中案件 = caseAssignmentType is CaseAssignmentType.網路件人工徵信中 or CaseAssignmentType.紙本件人工徵信中;

            foreach (var caseInfo in assignCasesInfos)
            {
                foreach (var user in assignUserDtos)
                {
                    if (已分派案件數量紀錄[user.UserId] >= user.CaseCount)
                    {
                        continue;
                    }

                    (bool isCheckCaseCanAssign, string filterReason) = CheckCaseCanAssign(
                        assignedUserId: user.UserId,
                        m_ID: caseInfo.M_ID,
                        s1_ID: caseInfo.S1_ID,
                        promotionUserId: caseInfo.PromotionUserId,
                        monthlyIncomeCheckUserId: caseInfo.MonthlyIncomeCheckUserId,
                        isManualCase: 是否為人工徵信中案件,
                        employeeNo: user.EmployeeNo,
                        stakeholderLookup: stakeholderLookup
                    );

                    if (!isCheckCaseCanAssign)
                    {
                        assignCaseResult.Add(
                            new AssignCaseResult
                            {
                                ApplyNo = caseInfo.ApplyNo,
                                UserId = user.UserId,
                                UserName = user.UserName,
                                FilterReason = filterReason,
                            }
                        );
                        continue;
                    }
                    else
                    {
                        assignCaseResult.Add(
                            new AssignCaseResult
                            {
                                ApplyNo = caseInfo.ApplyNo,
                                UserId = user.UserId,
                                UserName = user.UserName,
                            }
                        );
                        已分派案件數量紀錄[user.UserId] += 1;
                        break;
                    }
                }
            }

            return assignCaseResult;
        }

        private (bool isCheckCaseCanAssign, string filterReason) CheckCaseCanAssign(
            string assignedUserId,
            string m_ID,
            string s1_ID,
            string promotionUserId,
            string monthlyIncomeCheckUserId,
            bool isManualCase,
            string? employeeNo,
            ILookup<string, Reviewer_Stakeholder> stakeholderLookup
        )
        {
            List<string> 篩選原因 = [];

            if (manageHelper.與推廣人員是否相同(promotionUserId, assignedUserId, employeeNo))
                篩選原因.Add(FilterReasonConst.推廣員編相同);

            var idSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { m_ID };
            if (!string.IsNullOrEmpty(s1_ID))
                idSet.Add(s1_ID);

            if (manageHelper.檢核利害關係人(stakeholderLookup, idSet, assignedUserId, employeeNo))
                篩選原因.Add(FilterReasonConst.利害關係人);

            if (isManualCase)
            {
                if (manageHelper.與月收入確認人員是否相同(new HashSet<string> { monthlyIncomeCheckUserId }, assignedUserId))
                    篩選原因.Add(FilterReasonConst.月收入確認人員相同);
            }

            bool 是否派案 = !(篩選原因.Count > 0);
            string returnReason = string.Join("、", 篩選原因);

            return (是否派案, returnReason);
        }

        private List<AssignedUserDto> 合併派案人員資料(
            CaseAssignmentType caseAssignmentType,
            List<QueryAssignmentUsersResult> queryAssignmentUsersResult,
            List<AssignUserInfo> assignCaseUserInfos,
            Dictionary<string, string?> userIdToEmployeeNoDic
        )
        {
            List<AssignedUserDto> dtos = assignCaseUserInfos
                .Select(x =>
                {
                    var userInfo = queryAssignmentUsersResult.Single(q => q.UserId == x.AssignedUserId);
                    var employeeNo = userIdToEmployeeNoDic.GetValueOrDefault(x.AssignedUserId);
                    return new AssignedUserDto
                    {
                        UserId = userInfo.UserId,
                        UserName = userInfo.UserName,
                        CaseDispatchGroup = userInfo.CaseDispatchGroup,
                        IsPaperCase = userInfo.IsPaperCase,
                        PaperCaseSort = userInfo.PaperCaseSort,
                        IsWebCase = userInfo.IsWebCase,
                        WebCaseSort = userInfo.WebCaseSort,
                        IsManualCase = userInfo.IsManualCase,
                        ManualCaseSort = userInfo.ManualCaseSort,
                        EmployeeNo = employeeNo,
                        CaseCount = x.CaseCount,
                    };
                })
                .ToList();

            return caseAssignmentType switch
            {
                CaseAssignmentType.紙本件月收入預審_姓名檢核N清單 or CaseAssignmentType.紙本件月收入預審_姓名檢核Y清單 => dtos.Where(x =>
                        x.IsPaperCase == "Y"
                    )
                    .OrderBy(x => x.PaperCaseSort)
                    .ToList(),
                CaseAssignmentType.網路件月收入預審_姓名檢核N清單 or CaseAssignmentType.網路件月收入預審_姓名檢核Y清單 => dtos.Where(x =>
                        x.IsWebCase == "Y"
                    )
                    .OrderBy(x => x.WebCaseSort)
                    .ToList(),
                CaseAssignmentType.紙本件人工徵信中 or CaseAssignmentType.網路件人工徵信中 => dtos.Where(x => x.IsManualCase == "Y")
                    .OrderBy(x => x.ManualCaseSort)
                    .ToList(),
                _ => throw new ArgumentException("Invalid case assignment type"),
            };
        }

        private async Task<List<QueryAssignmentUsersResult>> QueryAssignmentUsers(List<CaseDispatchGroup> caseDispatchGroups)
        {
            // Tips: 查詢這個是為了剩餘案件時候,可以用比重排序進行派案
            var sql = @"EXEC [dbo].[Usp_GetAssignmentUsers] @CaseDispatchGroups";

            var caseDispatchGroupsString = string.Join(",", caseDispatchGroups.Select(x => (int)x));
            var queryAssignmentUsersResponse = await context
                .Database.GetDbConnection()
                .QueryAsync<QueryAssignmentUsersResult>(sql, new { CaseDispatchGroups = caseDispatchGroupsString });

            return queryAssignmentUsersResponse.ToList();
        }

        private readonly string 未派案 = "<NULL>";

        private GetApplyCreditCardBaseDataDto CreateGetApplyCreditCardBaseDataDto(CaseAssignmentType caseAssignmentType)
        {
            return caseAssignmentType switch
            {
                CaseAssignmentType.紙本件人工徵信中 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.人工徵信中],
                    Source = [Source.紙本],
                    CurrentHandleUserId = 未派案,
                },
                CaseAssignmentType.網路件人工徵信中 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.人工徵信中],
                    Source = [Source.APP, Source.ECARD],
                    CurrentHandleUserId = 未派案,
                },
                CaseAssignmentType.紙本件月收入預審_姓名檢核N清單 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.紙本件_待月收入預審],
                    CurrentHandleUserId = 未派案,
                },
                CaseAssignmentType.紙本件月收入預審_姓名檢核Y清單 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.紙本件_待月收入預審],
                    CurrentHandleUserId = 未派案,
                },
                CaseAssignmentType.網路件月收入預審_姓名檢核N清單 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.網路件_待月收入預審],
                    CurrentHandleUserId = 未派案,
                },
                CaseAssignmentType.網路件月收入預審_姓名檢核Y清單 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.網路件_待月收入預審],
                    CurrentHandleUserId = 未派案,
                },
                _ => throw new ArgumentException("Invalid case assignment type"),
            };
        }

        private string IsNameCheckedFilter(CaseAssignmentType caseAssignmentType)
        {
            return caseAssignmentType switch
            {
                CaseAssignmentType.紙本件人工徵信中 => string.Empty,
                CaseAssignmentType.網路件人工徵信中 => string.Empty,
                CaseAssignmentType.紙本件月收入預審_姓名檢核N清單 => "N",
                CaseAssignmentType.紙本件月收入預審_姓名檢核Y清單 => "Y",
                CaseAssignmentType.網路件月收入預審_姓名檢核N清單 => "N",
                CaseAssignmentType.網路件月收入預審_姓名檢核Y清單 => "Y",
                _ => null,
            };
        }

        private (CardStatus, CardStep) 分案類型轉換(CaseAssignmentType caseAssignmentType) =>
            caseAssignmentType switch
            {
                CaseAssignmentType.網路件月收入預審_姓名檢核N清單 => (CardStatus.網路件_待月收入預審, CardStep.月收入確認),
                CaseAssignmentType.網路件月收入預審_姓名檢核Y清單 => (CardStatus.網路件_待月收入預審, CardStep.月收入確認),
                CaseAssignmentType.網路件人工徵信中 => (CardStatus.人工徵信中, CardStep.人工徵審),
                CaseAssignmentType.紙本件人工徵信中 => (CardStatus.人工徵信中, CardStep.人工徵審),
                CaseAssignmentType.紙本件月收入預審_姓名檢核N清單 => (CardStatus.紙本件_待月收入預審, CardStep.月收入確認),
                CaseAssignmentType.紙本件月收入預審_姓名檢核Y清單 => (CardStatus.紙本件_待月收入預審, CardStep.月收入確認),
                _ => throw new ArgumentException("Invalid case assignment type"),
            };
    }
}
