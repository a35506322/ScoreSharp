using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetShortTimeIDCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 取得頻繁申請ID比對紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetShortTimeIDCheckLogByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetShortTimeIDCheckLogByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得短時間ID檢核結果_2000_ResEx),
            typeof(取得短時間ID檢核結果查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetShortTimeIDCheckLogByApplyNo")]
        public async Task<IResult> GetShortTimeIDCheckLogByApplyNo([FromRoute] string applyNo) =>
            Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetShortTimeIDCheckLogByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetShortTimeIDCheckLogByApplyNoResponse>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Query, ResultResponse<GetShortTimeIDCheckLogByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetShortTimeIDCheckLogByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;

            string sql = """

                SELECT Main.ApplyNo,
                       BankTrace.ShortTimeID_Flag,
                       BankTrace.ShortTimeID_CheckRecord,
                       BankTrace.ShortTimeID_UpdateUserId,
                       BankTrace.ShortTimeID_UpdateTime,
                       BankTrace.ShortTimeID_IsError,
                       TraceMain.ApplyNo AS Trace_ApplyNo,
                       TraceMain.ApplyDate AS Trace_ApplyDate,
                       TraceHandle.ApplyCardType AS Trace_ApplyCardType,
                       TraceHandle.CardStatus AS Trace_CardStatus
                FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] Main
                JOIN [ScoreSharp].[dbo].[Reviewer_BankTrace] BankTrace ON BankTrace.ApplyNo = Main.ApplyNo
                JOIN [ScoreSharp].[dbo].[Reviewer_CheckTrace] Trace ON Trace.CurrentApplyNo = Main.ApplyNo
                JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] TraceMain ON TraceMain.ApplyNo = Trace.SameApplyNo
                JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] TraceHandle ON TraceHandle.ApplyNo = TraceMain.ApplyNo AND TraceHandle.ID = TraceMain.ID AND TraceHandle.UserType = TraceMain.UserType
                WHERE Main.ApplyNo = @ApplyNo AND Trace.CheckType = @CheckType;

                SELECT UserId , UserName FROM [ScoreSharp].[dbo].[OrgSetUp_User];

                SELECT CardCode,CardName FROM [ScoreSharp].[dbo].[SetUp_Card];

                """;

            var multi = await context
                .Database.GetDbConnection()
                .QueryMultipleAsync(sql, new { ApplyNo = applyNo, CheckType = CheckTraceType.短時間頻繁ID比對 });
            var dtos = await multi.ReadAsync<ShortTimeIDCheckLogDto>();
            var userDtos = await multi.ReadAsync<UserDto>();
            var cardDtos = await multi.ReadAsync<CardDto>();

            if (dtos.Count() == 0)
            {
                var defaultResponse = new GetShortTimeIDCheckLogByApplyNoResponse();
                defaultResponse.ApplyNo = applyNo;
                defaultResponse.ShortTimeID_Flag = "N";
                return ApiResponseHelper.Success(defaultResponse);
            }

            var mainGroups = dtos.GroupBy(item => new
            {
                item.ApplyNo,
                item.ShortTimeID_Flag,
                item.ShortTimeID_CheckRecord,
                item.ShortTimeID_UpdateUserId,
                item.ShortTimeID_UpdateTime,
                item.ShortTimeID_IsError,
            });

            var response = mainGroups
                .Select(mainGroup => new GetShortTimeIDCheckLogByApplyNoResponse()
                {
                    ApplyNo = mainGroup.Key.ApplyNo,
                    ShortTimeID_Flag = mainGroup.Key.ShortTimeID_Flag,
                    ShortTimeID_CheckRecord = mainGroup.Key.ShortTimeID_CheckRecord,
                    ShortTimeID_UpdateUserId = mainGroup.Key.ShortTimeID_UpdateUserId,
                    ShortTimeID_UpdateUserName = userDtos.SingleOrDefault(x => x.UserId == mainGroup.Key.ShortTimeID_UpdateUserId)?.UserName,
                    ShortTimeID_UpdateTime = mainGroup.Key.ShortTimeID_UpdateTime,
                    ShortTimeID_IsError = mainGroup.Key.ShortTimeID_IsError,
                    CheckTraceDtos = mainGroup
                        .GroupBy(item => new { item.Trace_ApplyNo, item.Trace_ApplyDate })
                        .Select(handleGroup => new CheckTraceDto()
                        {
                            ApplyNo = handleGroup.Key.Trace_ApplyNo,
                            ApplyDate = handleGroup.Key.Trace_ApplyDate,
                            ApplyCardType = string.Join("/", handleGroup.Select(z => z.Trace_ApplyCardType)),
                            ApplyCardTypeName = string.Join(
                                "/",
                                handleGroup.Select(z => cardDtos.SingleOrDefault(x => x.CardCode == z.Trace_ApplyCardType)?.CardName)
                            ),
                            CardStatus = string.Join("/", handleGroup.Select(z => (int)z.Trace_CardStatus)),
                            CardStatusName = string.Join("/", handleGroup.Select(z => z.Trace_CardStatusName)),
                        })
                        .ToList(),
                })
                .FirstOrDefault();

            if (response is null)
                return ApiResponseHelper.NotFound<GetShortTimeIDCheckLogByApplyNoResponse>(null, applyNo);

            return ApiResponseHelper.Success(response);
        }
    }
}
