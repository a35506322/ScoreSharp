using ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.InsertUserOrgCaseSetUp;

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp
{
    public partial class UserOrgCaseSetUpController
    {
        ///<summary>
        /// 新增單筆人員組織分案群設定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">新增成功返回 Key 值</response>
        /// <remarks>
        ///  檢查與資料庫定義值相同
        ///     UserId、DesignatedSupervisor1、DesignatedSupervisor2 => 關聯 OrgSetUp_User
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(
            typeof(新增單筆人員組織分案群設定_2000_ReqEx),
            typeof(新增單筆人員組織分案群設定使用者帳號不在使用者名單中_4003_ReqEx),
            typeof(新增單筆人員組織分案群設定指定主管不在使用者名單中_4003_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(新增單筆人員組織分案群設定_2000_ResEx),
            typeof(新增單筆人員組織分案群設定使用者帳號不在使用者名單中_4003_ResEx),
            typeof(新增單筆人員組織分案群設定指定主管不在使用者名單中_4003_ResEx),
            typeof(新增單筆人員組織分案群設定資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertUserOrgCaseSetUp")]
        public async Task<IResult> InsertUserOrgCaseSetUp([FromBody] InsertUserOrgCaseSetUpRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.InsertUserOrgCaseSetUp
{
    public record Command(InsertUserOrgCaseSetUpRequest insertserOrgCaseSetUpRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertserOrgCaseSetUpRequest;

            var validUsers = await context.OrgSetUp_User.AsNoTracking().AnyAsync(x => x.IsActive == "Y" && x.UserId == dto.UserId, cancellationToken);

            if (!validUsers)
                return ApiResponseHelper.BusinessLogicFailed<string>(dto.UserId, "該使用者帳號 不在使用者名單中");

            var single = await context.OrgSetUp_UserOrgCaseSetUp.AsNoTracking().SingleOrDefaultAsync(user => user.UserId == dto.UserId);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, dto.UserId);

            var entity = mapper.Map<OrgSetUp_UserOrgCaseSetUp>(dto);

            await context.OrgSetUp_UserOrgCaseSetUp.AddAsync(entity);
            await context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(entity.UserId, entity.UserId);
        }
    }
}
