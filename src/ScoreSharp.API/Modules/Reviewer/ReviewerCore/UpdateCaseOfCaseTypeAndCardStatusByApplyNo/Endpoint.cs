using ScoreSharp.API.Infrastructures.JWTToken;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateCaseOfCaseTypeAndCardStatusByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 更改案件種類以及狀態 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/UpdateCaseOfCaseTypeAndCardStatusByApplyNo/20250409X0001
        ///
        ///
        /// CaseOfAction:
        ///
        ///     1 = 案件種類_一般件
        ///     2 = 案件種類_急件
        ///     3 = 案件種類_緊急製卡
        ///     4 = 狀態_補回件
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpPut("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(更改案件種類以及狀態成功_狀態轉成補回件_2000_ReqEx),
            typeof(更改案件種類以及狀態_變更案件種類變更為一般件_成功_2000_ReqEx),
            typeof(更改案件種類以及狀態_變更案件種類變更為急件_成功_2000_ReqEx),
            typeof(更改案件種類以及狀態_變更案件種類變更為緊急製卡_成功_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(更改案件種類以及狀態_狀態轉成補回件_成功_2000_ResEx),
            typeof(更改案件種類以及狀態_狀態轉成補回件_查無補件作業中之案件_4003_ResEx),
            typeof(更改案件種類以及狀態路由與Req比對錯誤_4003_ResEx),
            typeof(更改案件種類以及狀態_變更案件種類變更為一般件_成功_2000_ResEx),
            typeof(更改案件種類以及狀態_變更案件種類變更為急件_成功_2000_ResEx),
            typeof(更改案件種類以及狀態_變更案件種類變更為緊急製卡_成功_2000_ResEx),
            typeof(更改案件種類以及狀態_變更案件種類_查無資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateCaseOfCaseTypeAndCardStatusByApplyNo")]
        public async Task<IResult> UpdateCaseOfCaseTypeAndCardStatusByApplyNo(
            [FromRoute] string applyNo,
            UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest request
        )
        {
            var result = await _mediator.Send(new Command(applyNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateCaseOfCaseTypeAndCardStatusByApplyNo
{
    public record Command(string applyNo, UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest updateCaseOfCaseTypeAndCardStatusByApplyNoRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtHelper, IReviewerHelper reviewerHelper)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var dto = request.updateCaseOfCaseTypeAndCardStatusByApplyNoRequest;

            if (applyNo != dto.ApplyNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            List<Reviewer_ApplyCreditCardInfoProcess> processes = new();
            DateTime now = DateTime.Now;

            if (dto.CaseOfAction == CaseOfAction.狀態_補回件)
            {
                var handles = await context
                    .Reviewer_ApplyCreditCardInfoHandle.AsNoTracking()
                    .Where(x => x.ApplyNo == applyNo && x.CardStatus == CardStatus.補件作業中)
                    .ToListAsync();

                if (handles.Count() == 0)
                    return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無補件作業中之案件");

                await context
                    .Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == applyNo && x.CardStatus == CardStatus.補件作業中)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(h => h.CardStatus, CardStatus.補回件), cancellationToken);

                foreach (var handle in handles)
                {
                    Reviewer_ApplyCreditCardInfoProcess process = new();
                    process.ApplyNo = applyNo;
                    process.Process = CardStatus.補回件.ToString();
                    process.StartTime = now;
                    process.EndTime = now;
                    process.Notes = $"({handle.UserType}_{handle.ID})";
                    process.ProcessUserId = jwtHelper.UserId;

                    processes.Add(process);
                }
            }
            else
            {
                var (caseType, processConst) = ConvertCaseActionToCaseTypeAndProcessConst(dto.CaseOfAction);

                // TODO: 附卡人變更案件種類
                var main = await context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

                if (main is null)
                    return ApiResponseHelper.NotFound<string>(null, applyNo);

                main.CaseType = caseType;

                Reviewer_ApplyCreditCardInfoProcess process = new();
                process.ApplyNo = applyNo;
                process.Process = processConst;
                process.StartTime = now;
                process.EndTime = now;
                process.Notes = null;
                process.ProcessUserId = jwtHelper.UserId;

                processes.Add(process);
            }
            await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes, cancellationToken);

            await context.SaveChangesAsync();

            var isUpdateSuccessful = await reviewerHelper.UpdateMainLastModified(applyNo, jwtHelper.UserId, now) == 1;
            if (!isUpdateSuccessful)
            {
                return ApiResponseHelper.InternalServerError<string>(null, "更新Main最後異動資訊失敗");
            }

            return ApiResponseHelper.UpdateByIdSuccess(applyNo, applyNo);
        }

        private static (CaseType, string) ConvertCaseActionToCaseTypeAndProcessConst(CaseOfAction action)
        {
            switch (action)
            {
                case CaseOfAction.案件種類_一般件:
                    return (CaseType.一般件, ProcessConst.變更案件種類變更為一般件);
                case CaseOfAction.案件種類_急件:
                    return (CaseType.急件, ProcessConst.變更案件種類變更為急件);
                case CaseOfAction.案件種類_緊急製卡:
                    return (CaseType.緊急製卡, ProcessConst.變更案件種類變更為緊急製卡);
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, "無效的案件種類");
            }
        }
    }
}
