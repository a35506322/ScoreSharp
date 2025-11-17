using ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.UpdateUserOrgCaseSetUpById;

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp
{
    public partial class UserOrgCaseSetUpController
    {
        /// <summary>
        /// 單筆修改人員組織分案群組設定 By userId
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /UserOrgCaseSetUp/UpdateUserOrgCaseSetUpById/raya00
        ///
        /// </remarks>
        /// <param name="userId">PK</param>
        /// <param name="request"></param>
        /// <response code="400">
        /// 檢查與資料庫定義值相同
        /// Card (申請卡別) => 關聯 SetUp_Card
        /// </response>
        /// <returns></returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(
            typeof(修改人員組織分案群組設定_2000_ReqEx),
            typeof(修改人員組織分案群設定指定主管不在使用者名單中_4003_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(修改人員組織分案群組設定_2000_ResEx),
            typeof(修改人員組織分案群組設定查無此資料_4001_ResEx),
            typeof(修改人員組織分案群組設定路由與Req比對錯誤_4003_ResEx),
            typeof(修改人員組織分案群設定指定主管不在使用者名單中_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateUserOrgCaseSetUpById")]
        public async Task<IResult> UpdateUserOrgCaseSetUpById([FromRoute] string userId, [FromBody] UpdateUserOrgCaseSetUpByIdRequest request)
        {
            var result = await _mediator.Send(new Command(userId, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.UpdateUserOrgCaseSetUpById
{
    public record Command(string userId, UpdateUserOrgCaseSetUpByIdRequest updateUserOrgCaseSetUpByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = request.userId;
            var dto = request.updateUserOrgCaseSetUpByIdRequest;

            if (userId != dto.UserId)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var validUsers = await context.OrgSetUp_User.AsNoTracking().AnyAsync(x => x.IsActive == "Y" && x.UserId == dto.UserId, cancellationToken);

            if (!validUsers)
                return ApiResponseHelper.BusinessLogicFailed<string>(dto.UserId, "該使用者帳號 不在使用者名單中");

            var entity = await context.OrgSetUp_UserOrgCaseSetUp.SingleOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(userId, userId);

            entity.CardLimit = dto.CardLimit;
            entity.DesignatedSupervisor1 = dto.DesignatedSupervisor1;
            entity.DesignatedSupervisor2 = dto.DesignatedSupervisor2;
            entity.IsPaperCase = dto.IsPaperCase;
            entity.IsWebCase = dto.IsWebCase;
            entity.CheckWeight = dto.CheckWeight;
            entity.PaperCaseSort = dto.PaperCaseSort;
            entity.WebCaseSort = dto.WebCaseSort;
            entity.IsManualCase = dto.IsManualCase;
            entity.ManualCaseSort = dto.ManualCaseSort;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(userId, userId);
        }
    }
}
