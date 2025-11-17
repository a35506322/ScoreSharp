using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalMobileCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 查詢_行內手機重號紀錄
        /// </summary>
        /// <param name="applyNo"></param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [OpenApiOperation("GetInternalMobileCheckLogByApplyNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetInternalMobileCheckLogByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得相同行內手機檢核結果_2000_ResEx),
            typeof(取得相同行內手機檢核結果_查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> GetInternalMobileCheckLogByApplyNo([FromRoute] string applyNo) =>
            Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalMobileCheckLogByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetInternalMobileCheckLogByApplyNoResponse>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Query, ResultResponse<GetInternalMobileCheckLogByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetInternalMobileCheckLogByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            /*
             *  利用ApplyNo去查Handle、Main、BankTace、Reviewer_BankInternalSameLog
             */

            var applyNo = request.applyNo;
            var banktrace = await context
                .Reviewer_BankTrace.AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人);

            if (banktrace is null)
            {
                return ApiResponseHelper.NotFound<GetInternalMobileCheckLogByApplyNoResponse>(null, applyNo);
            }

            string sql = """
                SELECT
                    Trace.ApplyNo,
                    Trace.ID,
                    Trace.SameID ,
                    Trace.SameName ,
                    Trace.SameBillAddr ,
                    Main.CHName ,
                    Main.Mobile ,
                    Main.PromotionUnit ,
                    Main.PromotionUser ,
                    Handle.SeqNo ,
                    Handle.CardStatus
                FROM [ScoreSharp].[dbo].[Reviewer_BankInternalSameLog] Trace WITH (NOLOCK)
                JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] Main WITH (NOLOCK) ON Main.ApplyNo = Trace.ApplyNo AND Main.ID = Trace.ID AND Main.UserType = Main.UserType
                JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] Handle WITH (NOLOCK) ON Handle.ApplyNo = Main.ApplyNo AND Handle.ID = Main.ID  AND Handle.UserType = Main.UserType
                WHERE Trace.ApplyNo = @applyNo AND CheckType = @checkType
                """;

            var details = await context
                .Database.GetDbConnection()
                .QueryAsync<InternalMobileDto>(sql, new { applyNo = applyNo, checkType = BankInternalSameCheckType.手機 });

            var response = new GetInternalMobileCheckLogByApplyNoResponse();
            response.ApplyNo = banktrace.ApplyNo;
            response.SameInternalMobileChecked = banktrace.InternalMobileSame_Flag;
            response.CheckRecord = banktrace.InternalMobileSame_CheckRecord;
            response.UpdateUserId = banktrace.InternalMobileSame_UpdateUserId;
            response.UpdateTime = banktrace.InternalMobileSame_UpdateTime;
            response.IsError = banktrace.InternalMobileSame_IsError;
            response.Relation = banktrace.InternalMobileSame_Relation;
            response.SameInternalMobileCheckDetails = details
                .GroupBy(item => new
                {
                    item.ApplyNo,
                    item.ID,
                    item.CHName,
                    item.Mobile,
                    item.PromotionUnit,
                    item.PromotionUser,
                    item.SameID,
                    item.SameName,
                    item.SameBillAddr,
                })
                .Select(item =>
                {
                    var first = item.FirstOrDefault();
                    return new SameInternalMobileCheckDetailDto
                    {
                        CurrentApplyNo = first.ApplyNo,
                        CurrentID = first.ID,
                        CurrentName = first.CHName,
                        CurrentCardStatusList = item.Select(y => new CardStatusDto { SeqNo = y.SeqNo, CardStatus = y.CardStatus }).ToList(),
                        CurrentMobile = first.Mobile,
                        CurrentPromotionUnit = first.PromotionUnit,
                        CurrentPromotionUser = first.PromotionUser,
                        SameID = first.SameID,
                        SameName = first.SameName,
                        SameBillAddr = first.SameBillAddr,
                    };
                })
                .ToList();

            if (banktrace.InternalMobileSame_UpdateUserId is not null)
            {
                var user = await context.OrgSetUp_User.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == banktrace.InternalMobileSame_UpdateUserId);
                response.UpdateUserName = user is null ? null : user.UserName;
            }
            return ApiResponseHelper.Success(response);
        }
    }
}
