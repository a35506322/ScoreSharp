using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalEmailCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 查詢_行內Email重號紀錄
        /// </summary>
        /// <param name="applyNo"></param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [OpenApiOperation("GetInternalEmailCheckLogByApplyNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetInternalEmailCheckLogByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得相同行內Email檢核結果_2000_ResEx),
            typeof(取得相同行內Email檢核結果_查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> GetInternalEmailCheckLogByApplyNo([FromRoute] string applyNo) =>
            Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalEmailCheckLogByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetInternalEmailCheckLogByApplyNoResponse>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Query, ResultResponse<GetInternalEmailCheckLogByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetInternalEmailCheckLogByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
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
                return ApiResponseHelper.NotFound<GetInternalEmailCheckLogByApplyNoResponse>(null, applyNo);
            }

            string sql = """
                  SELECT
                       Trace.ApplyNo,
                       Trace.ID,
                       Trace.SameID ,
                       Trace.SameName ,
                       Trace.SameBillAddr ,
                       Main.CHName ,
                       Main.EMail ,
                       Main.PromotionUnit ,
                       Main.PromotionUser,
                       Handle.SeqNo,
                       Handle.CardStatus
                FROM [ScoreSharp].[dbo].[Reviewer_BankInternalSameLog] Trace WITH (NOLOCK)
                JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] Main WITH (NOLOCK) ON Main.ApplyNo = Trace.ApplyNo  And Main.ID = Trace.ID AND Main.UserType = Trace.UserType
                JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] Handle WITH (NOLOCK) ON Handle.ApplyNo = Main.ApplyNo AND Handle.ID = Main.ID AND Handle.UserType = Main.UserType
                WHERE Trace.ApplyNo = @applyNo AND Trace.CheckType = @checkType
                """;

            var details = await context
                .Database.GetDbConnection()
                .QueryAsync<InternalEmailDetailDto>(sql, new { applyNo = applyNo, checkType = BankInternalSameCheckType.Email });

            GetInternalEmailCheckLogByApplyNoResponse response = new();
            response.ApplyNo = applyNo;
            response.SameInternalEmailChecked = banktrace.InternalEmailSame_Flag;
            response.CheckRecord = banktrace.InternalEmailSame_CheckRecord;
            response.UpdateUserId = banktrace.InternalEmailSame_UpdateUserId;
            response.UpdateTime = banktrace.InternalEmailSame_UpdateTime;
            response.IsError = banktrace.InternalEmailSame_IsError;
            response.Relation = banktrace.InternalEmailSame_Relation;

            response.SameInternalEmailCheckDetails = details
                .GroupBy(item => new
                {
                    item.ApplyNo,
                    item.ID,
                    item.CHName,
                    item.Email,
                    item.PromotionUnit,
                    item.PromotionUser,
                    item.SameID,
                    item.SameName,
                    item.SameBillAddr,
                })
                .Select(x =>
                {
                    var first = x.FirstOrDefault();
                    return new SameInternalEmailCheckDetailDto
                    {
                        CurrentApplyNo = first.ApplyNo,
                        CurrentID = first.ID,
                        CurrentName = first.CHName,
                        CurrentCardStatusList = x.Select(y => new CardStatusDto { SeqNo = y.SeqNo, CardStatus = y.CardStatus }).ToList(),
                        CurrentEmail = first.Email,
                        CurrentPromotionUnit = first.PromotionUnit,
                        CurrentPromotionUser = first.PromotionUser,
                        SameID = first.SameID,
                        SameName = first.SameName,
                        SameBillAddr = first.SameBillAddr,
                    };
                })
                .ToList();

            if (banktrace.InternalEmailSame_UpdateUserId is not null)
            {
                var user = await context.OrgSetUp_User.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == banktrace.InternalEmailSame_UpdateUserId);
                response.UpdateUserName = user is null ? null : user.UserName;
            }
            return ApiResponseHelper.Success(response);
        }
    }
}
