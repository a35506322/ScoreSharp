using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameEmailCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 取得電子信箱相同紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetSameEmailCheckLogByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetSameEmailCheckLogByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得相同電子信箱檢核結果_2000_ResEx),
            typeof(取得相同電子信箱檢核結果查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetSameEmailCheckLogByApplyNo")]
        public async Task<IResult> GetSameEmailCheckLogByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameEmailCheckLogByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetSameEmailCheckLogByApplyNoResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetSameEmailCheckLogByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetSameEmailCheckLogByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;

            var log = await context.View_WebDigist.AsNoTracking().SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            string sql = """

                SELECT
                    Trace.SeqNo,
                    Trace.CurrentApplyNo,
                    Main.ApplyNo AS SameApplyNo,
                    Main.ID AS SameID,
                    Main.CHName AS SameName,
                    Main.CompName AS SameCompName,
                    Main.UserSourceIP AS SameUserSourceIP,
                    Main.OTPMobile AS SameOTPMobile,
                    Main.OTPTime AS SameOTPTime,
                    Handle.CardStatus AS SameCardStatus
                FROM [ScoreSharp].[dbo].[Reviewer_CheckTrace] Trace
                    JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] Main ON Main.ApplyNo = Trace.SameApplyNo
                    JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] Handle ON Handle.ApplyNo = Main.ApplyNo AND Handle.ID = Main.ID AND Handle.UserType = Main.UserType
                WHERE Trace.CurrentApplyNo = @ApplyNo AND Trace.CheckType = @CheckType

                """;

            var multi = await context
                .Database.GetDbConnection()
                .QueryMultipleAsync(sql, new { ApplyNo = applyNo, CheckType = CheckTraceType.網路件EMAIL比對 });
            var details = await multi.ReadAsync<SameEmailCheckLogDto>();

            if (log is null)
                return ApiResponseHelper.NotFound<GetSameEmailCheckLogByApplyNoResponse>(null, applyNo);

            GetSameEmailCheckLogByApplyNoResponse response = new();
            response.ApplyNo = log.ApplyNo;
            response.SameWebCaseEmailChecked = log.SameEmail_Flag;
            response.CheckRecord = log.SameEmail_CheckRecord;
            response.UpdateUserId = log.SameEmail_UpdateUserId;
            response.UpdateTime = log.SameEmail_UpdateTime;
            response.IsError = log.SameEmail_IsError;

            response.SameEmailCheckDetails = details
                .Select(item => new SameEmailCheckDetailDto
                {
                    SeqNo = item.SeqNo,
                    CurrentApplyNo = item.CurrentApplyNo,
                    CurrentCardStatus = log.CardStatus,
                    CurrentCompName = log.CompName,
                    CurrentID = log.ID,
                    CurrentName = log.CHName,
                    CurrentOTPMobile = log.OTPMobile,
                    CurrentEmail = log.EMail,
                    CurrentPromotionUnit = log.PromotionUnit,
                    CurrentPromotionUser = log.PromotionUser,
                    SameApplyNo = item.SameApplyNo,
                    SameCardStatus = item.SameCardStatus,
                    SameCompName = item.SameCompName,
                    SameID = item.SameID,
                    SameName = item.SameName,
                    SameOTPMobile = item.SameOTPMobile,
                })
                .ToList();

            if (log.SameEmail_UpdateUserId is not null)
            {
                var user = await context.OrgSetUp_User.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == log.SameEmail_UpdateUserId);
                response.UpdateUserName = user is null ? null : user.UserName;
            }

            return ApiResponseHelper.Success(response);
        }
    }
}
