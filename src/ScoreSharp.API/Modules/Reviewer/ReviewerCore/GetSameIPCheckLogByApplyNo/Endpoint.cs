using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameIPCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 取得相同IP檢核結果 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetSameIPCheckLogByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetSameIPCheckLogByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得相同IP檢核結果_2000_ResEx),
            typeof(取得相同IP檢核結果查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetSameIPCheckLogByApplyNo")]
        public async Task<IResult> GetSameIPCheckLogByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameIPCheckLogByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetSameIPCheckLogByApplyNoResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetSameIPCheckLogByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetSameIPCheckLogByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
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
                .QueryMultipleAsync(sql, new { ApplyNo = applyNo, CheckType = CheckTraceType.相同IP比對 });
            var details = await multi.ReadAsync<SameIPCheckLogDto>();

            if (log is null)
                return ApiResponseHelper.NotFound<GetSameIPCheckLogByApplyNoResponse>(null, applyNo);

            GetSameIPCheckLogByApplyNoResponse response = new();
            response.ApplyNo = log.ApplyNo;
            response.SameIPChecked = log.SameIP_Flag;
            response.CheckRecord = log.SameIP_CheckRecord;
            response.UpdateUserId = log.SameIP_UpdateUserId;
            response.UpdateTime = log.SameIP_UpdateTime;
            response.IsError = log.SameIP_IsError;
            response.SameIPCheckDetails = details
                .Select(item => new SameIPCheckDetailDto
                {
                    SeqNo = item.SeqNo,
                    CurrentApplyNo = item.CurrentApplyNo,
                    CurrentCardStatus = log.CardStatus,
                    CurrentCompName = log.CompName,
                    CurrentID = log.ID,
                    CurrentName = log.CHName,
                    CurrentOTPMobile = log.OTPMobile,
                    CurrentOTPTime = log.OTPTime,
                    CurrentPromotionUnit = log.PromotionUnit,
                    CurrentPromotionUser = log.PromotionUser,
                    CurrentUserSourceIP = log.UserSourceIP,
                    SameApplyNo = item.SameApplyNo,
                    SameCardStatus = item.SameCardStatus,
                    SameCompName = item.SameCompName,
                    SameID = item.SameID,
                    SameName = item.SameName,
                    SameOTPMobile = item.SameOTPMobile,
                    SameOTPTime = item.SameOTPTime,
                    SameUserSourceIP = item.SameUserSourceIP,
                })
                .ToList();

            if (log.SameIP_UpdateUserId is not null)
            {
                var user = await context.OrgSetUp_User.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == log.SameIP_UpdateUserId);
                response.UpdateUserName = user is null ? null : user.UserName;
            }

            return ApiResponseHelper.Success(response);
        }
    }
}
