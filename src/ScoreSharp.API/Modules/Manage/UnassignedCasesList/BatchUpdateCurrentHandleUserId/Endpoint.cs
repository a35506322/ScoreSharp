using System.Transactions;
using ScoreSharp.API.Modules.Manage.UnassignedCasesList.BatchUpdateCurrentHandleUserId;

namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList
{
    public partial class UnassignedCasesListController
    {
        /// <summary>
        /// 批次修改當前經辦
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [OpenApiOperation("BatchUpdateCurrentHandleUserId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(批次修改當前經辦_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(批次修改當前經辦_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> BatchUpdateCurrentHandleUserId([FromBody] BatchUpdateCurrentHandleUserIdRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.BatchUpdateCurrentHandleUserId
{
    public record Command(BatchUpdateCurrentHandleUserIdRequest batchUpdateCurrentHandleUserIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly IScoreSharpDapperContext _dappercontext;
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwtHelper;
        private readonly ILogger<Handler> _logger;

        public Handler(IScoreSharpDapperContext dappercontext, IJWTProfilerHelper jwtHelper, ScoreSharpContext context, ILogger<Handler> logger)
        {
            _dappercontext = dappercontext;
            _jwtHelper = jwtHelper;
            _context = context;
            _logger = logger;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var tempReq = request.batchUpdateCurrentHandleUserIdRequest;
            var currentHandleUserId = tempReq.CurrentHandleUserId;
            var batchUpdateApplyNoList = tempReq.ApplyNo;
            var caseAssignmentType = tempReq.CaseAssignmentType;

            var assignHandleUser = await _context.OrgSetUp_User.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == currentHandleUserId);

            var (cardStatus, cardStep) = 分案類型轉換(caseAssignmentType);

            var batchUpdateList = batchUpdateApplyNoList
                .Select(x => new
                {
                    ApplyNo = x,
                    CurrentHandleUserId = currentHandleUserId,
                    PreviousHandleUserId = currentHandleUserId,
                })
                .ToList();

            DateTime now = DateTime.Now;
            int errorCount = 0;

            int pageSize = 1000;
            int pageCount = (int)Math.Ceiling((double)batchUpdateList.Count / pageSize);

            for (int page = 0; page < pageCount; page++)
            {
                var slice = batchUpdateList.Skip(page * pageSize).Take(pageSize).ToList();

                var updateParams = slice
                    .Select(x => new
                    {
                        ApplyNo = x.ApplyNo,
                        CurrentHandleUserId = x.CurrentHandleUserId,
                        PreviousHandleUserId = x.PreviousHandleUserId,
                        UpdateUserId = _jwtHelper.UserId,
                        Now = now,
                    })
                    .ToList();

                var processRecords = slice
                    .Select(info => new Reviewer_ApplyCreditCardInfoProcess
                    {
                        ApplyNo = info.ApplyNo,
                        Process = cardStatus.ToString(),
                        StartTime = now,
                        EndTime = now,
                        Notes = $"{_jwtHelper.UserName} 指派TO：{assignHandleUser.UserName}",
                        ProcessUserId = _jwtHelper.UserId,
                    })
                    .ToList();

                var updateHandleList = slice
                    .Select(x => new
                    {
                        ApplyNo = x.ApplyNo,
                        CardStep = cardStep,
                        ReviewerUserId = currentHandleUserId,
                        ReviewerTime = now,
                    })
                    .ToList();

                var statistics = slice.Select(x => new Reviewer_CaseStatistics
                {
                    ApplyNo = x.ApplyNo,
                    AddTime = now,
                    CaseType = CaseStatisticType.待派案_人工,
                    UserId = x.CurrentHandleUserId,
                });

                // TODO : 2025.07.15 確認是否DBName是否不加或自己控制
                string updateSql = """
                        UPDATE [dbo].[Reviewer_ApplyCreditCardInfoMain]
                        SET CurrentHandleUserId = @CurrentHandleUserId,
                            PreviousHandleUserId = @PreviousHandleUserId,
                            LastUpdateTime = @Now,
                            LastUpdateUserId = @UpdateUserId
                        WHERE ApplyNo = @ApplyNo;
                    """;

                string updateHandleSql = "";
                if (cardStatus == CardStatus.人工徵信中)
                {
                    updateHandleSql = """
                                UPDATE [dbo].[Reviewer_ApplyCreditCardInfoHandle]
                                SET CardStep = @CardStep,
                                    ReviewerUserId = @ReviewerUserId,
                                    ReviewerTime = @ReviewerTime
                                WHERE ApplyNo = @ApplyNo;
                        """;
                }
                else
                {
                    updateHandleSql = """
                                UPDATE [dbo].[Reviewer_ApplyCreditCardInfoHandle]
                                SET CardStep = @CardStep
                                WHERE ApplyNo = @ApplyNo;
                        """;
                }

                string insertSql = """
                        INSERT INTO [dbo].[Reviewer_ApplyCreditCardInfoProcess]
                        (ApplyNo, Process, StartTime, EndTime, Notes, ProcessUserId)
                        VALUES (@ApplyNo, @Process, @StartTime, @EndTime, @Notes, @ProcessUserId);
                    """;

                string statisticSql = """
                        INSERT INTO [ScoreSharp].[dbo].[Reviewer_CaseStatistics]
                        (UserId, AddTime, CaseType, ApplyNo)
                        VALUES
                        (@UserId, @AddTime, @CaseType, @ApplyNo)
                    """;

                try
                {
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        using var connection = _dappercontext.CreateScoreSharpConnection();
                        await connection.ExecuteAsync(updateSql, updateParams);
                        await connection.ExecuteAsync(updateHandleSql, updateHandleList);
                        await connection.ExecuteAsync(insertSql, processRecords);
                        await connection.ExecuteAsync(statisticSql, statistics);
                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    var failedApplyNos = string.Join(", ", slice.Select(x => x.ApplyNo));
                    _logger.LogError("批次修改當前經辦失敗，ApplyNo: {ApplyNos}，錯誤訊息: {@Exception}", failedApplyNos, ex);
                    errorCount++;
                }
            }

            if (errorCount > 0)
                return ApiResponseHelper.Success<string>(null, "部分資料未指派，請重新確認");

            return ApiResponseHelper.Success<string>(null, "更新成功");
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
