using System.Transactions;
using ScoreSharp.API.Modules.Manage.Common.Helpers;
using ScoreSharp.API.Modules.Manage.TransferCase.ExecTransferCaseManual;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.TransferCase
{
    public partial class TransferCaseController
    {
        /// <summary>
        /// 手動調撥案件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<ExecTransferCaseManualResponse>))]
        [EndpointSpecificExample(typeof(手動調撥案件_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(手動調撥案件_2000_ResEx),
            typeof(商業邏輯驗證失敗_調撥指定案件數量與系統查找的筆數不符_4003_ResEx),
            typeof(商業邏輯驗證失敗_與指派人員的派案組織不同_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("ExecTransferCaseManual")]
        public async Task<IResult> ExecTransferCaseManual([FromBody] ExecTransferCaseManualRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            if (result.ReturnCodeStatus != ReturnCodeStatus.成功)
                return Results.Ok(result);

            return Results.File(result.ReturnData.ResultFile, result.ReturnData.ContentType, result.ReturnData.FileName);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.TransferCase.ExecTransferCaseManual
{
    public record Command(ExecTransferCaseManualRequest ExecTransferCaseManualRequest) : IRequest<ResultResponse<ExecTransferCaseManualResponse>>;

    public class Handler(
        ScoreSharpContext context,
        IScoreSharpDapperContext dapperContext,
        IReviewerHelper reviewerHelper,
        IManageHelper manageHelper,
        IJWTProfilerHelper jwtProfilerHelper,
        IMiniExcelHelper miniExcelHelper,
        ILogger<Handler> logger
    ) : IRequestHandler<Command, ResultResponse<ExecTransferCaseManualResponse>>
    {
        public async Task<ResultResponse<ExecTransferCaseManualResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var tempRequest = request.ExecTransferCaseManualRequest;
            var 處理經辦 = tempRequest.HandleUserId;
            var 指派經辦UserId = tempRequest.TransferredUserId;
            var 申請書編號 = tempRequest.ApplyNos.ToHashSet();

            var baseDatas = await reviewerHelper.GetApplyCreditCardBaseData(new GetApplyCreditCardBaseDataDto { CurrentHandleUserId = 處理經辦 });
            var transferCaseInfos = baseDatas
                .Select(x => new TransferCaseInfo
                {
                    ApplyNo = x.ApplyNo,
                    PromotionUserId = x.PromotionUser,
                    MonthlyIncomeCheckUserId = x.ApplyCardList.FirstOrDefault()?.MonthlyIncomeCheckUserId ?? string.Empty,
                    M_ID = x.M_ID,
                    S1_ID = x.S1_ID,
                    ApplyCardList = x
                        .ApplyCardList.Select(c => new ApplyCardDto { ApplyCardType = c.ApplyCardType, CardStatus = c.CardStatus })
                        .ToList(),
                    Source = x.Source ?? default,
                })
                .Where(x => 申請書編號.Contains(x.ApplyNo))
                .ToList();

            if (申請書編號.Count != transferCaseInfos.Count)
                return ApiResponseHelper.BusinessLogicFailed<ExecTransferCaseManualResponse>(null, $"調撥指定案件數量與系統查找的筆數不符");

            // 查詢派案人員
            var 派案組織 = jwtProfilerHelper.CaseDispatchGroups;
            var queryAssignmentUsers = await QueryAssignmentUsers(派案組織);

            var 指派經辦AssignmentInfo = queryAssignmentUsers.FirstOrDefault(x => x.UserId == 指派經辦UserId);

            if (指派經辦AssignmentInfo is null)
                return ApiResponseHelper.BusinessLogicFailed<ExecTransferCaseManualResponse>(null, "與指派人員的派案組織不同");

            var 指派經辦資訊 = await context.OrgSetUp_User.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == 指派經辦UserId);
            var transferUserDto = new TransferUserDto
            {
                UserId = 指派經辦資訊.UserId,
                UserName = 指派經辦資訊.UserName,
                EmployeeNo = 指派經辦資訊.EmployeeNo,
            };

            // 查詢利害關係人
            var stakeholders = await context.Reviewer_Stakeholder.AsNoTracking().Where(x => x.IsActive == "Y").ToListAsync();
            var stakeholderLookup = stakeholders.ToLookup(x => x.ID, StringComparer.OrdinalIgnoreCase);

            var transferCaseResult = 調撥檢核(transferUserDto, transferCaseInfos, stakeholderLookup);

            // 更新資料庫
            var canTransferredCases = transferCaseResult.Where(x => x.IsTransferred).ToList();
            bool isSuccess = await 更新調撥結果(canTransferredCases);

            if (!isSuccess)
            {
                // 資料庫更新失敗的案件 => 更新例外訊息
                var updateFailedApplyNos = canTransferredCases.Select(x => x.ApplyNo).ToHashSet(StringComparer.OrdinalIgnoreCase);
                foreach (var result in transferCaseResult.Where(x => updateFailedApplyNos.Contains(x.ApplyNo)))
                {
                    result.ExceptionMessage = "資料庫更新失敗";
                }
            }

            // 回傳Excel
            var excelDtos = transferCaseResult
                .GroupBy(k => k.UserId)
                .ToDictionary(
                    g => $"{g.Key}_{g.First().UserName}_調撥結果",
                    g =>
                        g.Select(c => new TransferCaseExcelDto
                            {
                                ApplyNo = c.ApplyNo,
                                IsTransferred = c.IsTransferred ? "Y" : "N",
                                FilterReason = c.FilterReason,
                                ExceptionMessage = c.ExceptionMessage,
                            })
                            .ToList()
                );
            var stream = await miniExcelHelper.匯出多個工作表ExcelToStream(excelDtos);
            var response = new ExecTransferCaseManualResponse
            {
                ResultFile = stream.ToArray(),
                FileName = $"調撥案件人工_統計_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            };

            return ApiResponseHelper.Success(response);
        }

        private async Task<bool> 更新調撥結果(List<TransferCaseResult> transferCaseResult)
        {
            bool isSuccess = false;
            var now = DateTime.Now;

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

            var mainParams = transferCaseResult
                .Select(x => new
                {
                    ApplyNo = x.ApplyNo,
                    CurrentHandleUserId = x.UserId,
                    PreviousHandleUserId = x.UserId,
                    LastUpdateUserId = jwtProfilerHelper.UserId,
                    LastUpdateTime = now,
                })
                .ToList();

            string processSql = """
                    INSERT INTO [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoProcess]
                    (ApplyNo, Process, StartTime, EndTime, Notes, ProcessUserId)
                    VALUES
                    (@ApplyNo, @Process, @StartTime, @EndTime, @Notes, @ProcessUserId)
                """;

            var processes = transferCaseResult
                .Select(x => new Reviewer_ApplyCreditCardInfoProcess
                {
                    ApplyNo = x.ApplyNo,
                    Process = String.Join("/", x.ApplyCardList.Select(c => c.CardStatus.ToString())),
                    StartTime = now,
                    EndTime = now,
                    Notes = $"【調撥案件】{jwtProfilerHelper.UserName} 指派TO：{x.UserName}",
                    ProcessUserId = jwtProfilerHelper.UserId,
                })
                .ToList();

            string statisticSql = """
                    INSERT INTO [ScoreSharp].[dbo].[Reviewer_CaseStatistics]
                    (UserId, AddTime, CaseType, ApplyNo)
                    VALUES
                    (@UserId, @AddTime, @CaseType, @ApplyNo)
                """;

            var statistics = transferCaseResult.Select(x => new Reviewer_CaseStatistics
            {
                UserId = x.UserId,
                AddTime = now,
                CaseType = CaseStatisticType.調撥_人工,
                ApplyNo = x.ApplyNo,
            });

            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using var connection = dapperContext.CreateScoreSharpConnection();
                await connection.ExecuteAsync(mainSql, mainParams);
                await connection.ExecuteAsync(processSql, processes);
                await connection.ExecuteAsync(statisticSql, statistics);
                scope.Complete();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                logger.LogError("調撥案件修改當前經辦失敗，錯誤訊息: {@Exception}", ex);
            }

            return isSuccess;
        }

        private List<TransferCaseResult> 調撥檢核(
            TransferUserDto user,
            List<TransferCaseInfo> transferCaseInfos,
            ILookup<string, Reviewer_Stakeholder> stakeholderLookup
        )
        {
            List<TransferCaseResult> transferCaseResults = [];

            foreach (var caseInfo in transferCaseInfos)
            {
                var (isCheckCaseCanAssign, filterReason) = CheckCaseCanAssign(
                    assignedUserId: user.UserId,
                    m_ID: caseInfo.M_ID,
                    s1_ID: caseInfo.S1_ID,
                    promotionUserId: caseInfo.PromotionUserId,
                    monthlyIncomeCheckUserId: caseInfo.MonthlyIncomeCheckUserId,
                    isManualCase: caseInfo.ApplyCardList.Any(c => c.CardStatus == CardStatus.人工徵信中),
                    employeeNo: user.EmployeeNo,
                    stakeholderLookup: stakeholderLookup
                );

                if (!isCheckCaseCanAssign)
                {
                    transferCaseResults.Add(
                        new TransferCaseResult
                        {
                            ApplyNo = caseInfo.ApplyNo,
                            UserId = user.UserId,
                            UserName = user.UserName,
                            ApplyCardList = caseInfo.ApplyCardList,
                            FilterReason = filterReason,
                        }
                    );
                }
                else
                {
                    transferCaseResults.Add(
                        new TransferCaseResult
                        {
                            ApplyNo = caseInfo.ApplyNo,
                            UserId = user.UserId,
                            UserName = user.UserName,
                            ApplyCardList = caseInfo.ApplyCardList,
                            FilterReason = string.Empty,
                        }
                    );
                }
            }

            return transferCaseResults;
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
    }
}
