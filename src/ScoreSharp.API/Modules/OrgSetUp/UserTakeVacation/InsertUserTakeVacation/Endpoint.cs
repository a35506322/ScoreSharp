using ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.InsertUserTakeVacation;

namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation
{
    public partial class UserTakeVacationController
    {
        /// <summary>
        /// 新增單筆員工休假
        /// </summary>
        /// <remarks>
        ///
        ///     新增說明：
        ///
        ///     一次只能新增一天的假，
        ///     例如若需請假 12/27 至 12/30，
        ///     就要新增 12/27、12/28、12/29 和 12/30 的請假
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增單筆員工休假_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增單筆員工休假_2000_ResEx),
            typeof(新增單筆員工休假查無定義值_4001_ResEx),
            typeof(新增單筆員工休假時間有誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertUserTakeVacation")]
        public async Task<IResult> InsertUserTakeVacation([FromBody] InsertUserTakeVacationRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.InsertUserTakeVacation
{
    public record Command(InsertUserTakeVacationRequest insertUserTakeVacationRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertUserTakeVacationRequest;

            var activeUser = await context
                .OrgSetUp_User.AsNoTracking()
                .SingleOrDefaultAsync(x => x.UserId == dto.UserId && x.IsActive == "Y");

            if (activeUser is null)
                return ApiResponseHelper.NotFound<string>(null, dto.UserId);

            if (dto.StartTime >= dto.EndTime)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "請確認開始時間不晚於結束時間");

            var currentTime = DateTime.Now;

            if (currentTime > dto.StartTime)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "請假開始時間必須晚於目前時間，請重新設定。");

            if (dto.StartTime.Date != dto.EndTime.Date)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "請確認每次申請僅限單日請假，若需多日請假，請分別提交申請。");

            var duplicateLeaveRequest = await context
                .OrgSetUp_UserTakeVacation.AsNoTracking()
                .SingleOrDefaultAsync(x => x.UserId == dto.UserId && x.StartTime.Date == dto.StartTime.Date);

            if (duplicateLeaveRequest != null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "該帳號已重複提交相同日期的請假申請，請檢查後再試。");

            var entity = mapper.Map<OrgSetUp_UserTakeVacation>(dto);
            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess<string>(entity.SeqNo.ToString(), entity.SeqNo.ToString());
        }
    }
}
