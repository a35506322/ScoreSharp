using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameMobileCheckLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 取得手機號碼相同紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetSameMobileCheckLogByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetSameMobileCheckLogByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得相同手機號碼檢核結果_2000_ResEx),
            typeof(取得相同手機號碼檢核結果查無此申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetSameMobileCheckLogByApplyNo")]
        public async Task<IResult> GetSameMobileCheckLogByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameMobileCheckLogByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetSameMobileCheckLogByApplyNoResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetSameMobileCheckLogByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetSameMobileCheckLogByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
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
                .QueryMultipleAsync(sql, new { ApplyNo = applyNo, CheckType = CheckTraceType.網路件手機號碼比對 });
            var details = await multi.ReadAsync<SameMobileCheckLogDto>();

            if (log is null)
            {
                return ApiResponseHelper.NotFound<GetSameMobileCheckLogByApplyNoResponse>(null, applyNo);
            }

            GetSameMobileCheckLogByApplyNoResponse response = new();
            response.ApplyNo = log.ApplyNo;
            response.SameWebCaseMobileChecked = log.SameMobile_Flag;
            response.CheckRecord = log.SameMobile_CheckRecord;
            response.UpdateUserId = log.SameMobile_UpdateUserId;
            response.UpdateTime = log.SameMobile_UpdateTime;
            response.IsError = log.SameMobile_IsError;
            response.SameMobileCheckDetails = details
                .Select(item => new SameMobileCheckDetailDto
                {
                    SeqNo = item.SeqNo,
                    CurrentApplyNo = item.CurrentApplyNo,
                    CurrentCardStatus = log.CardStatus,
                    CurrentCompName = log.CompName,
                    CurrentID = log.ID,
                    CurrentName = log.CHName,
                    CurrentOTPMobile = log.OTPMobile,
                    CurrentMobile = log.Mobile,
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

            if (log.SameMobile_UpdateUserId is not null)
            {
                var user = await context.OrgSetUp_User.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == log.SameMobile_UpdateUserId);
                response.UpdateUserName = user is null ? null : user.UserName;
            }

            return ApiResponseHelper.Success(response);
        }
    }
}
