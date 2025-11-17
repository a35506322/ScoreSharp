using ScoreSharp.API.Modules.OrgSetUp.User.GetUserById;

namespace ScoreSharp.API.Modules.OrgSetUp.User
{
    public partial class UserController
    {
        /// <summary>
        /// 取得單筆使用者
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /User/GetUserById/SuperAdmin
        ///
        /// </remarks>
        /// <param name="userId">PK</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetUserByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得使用者_2000_ResEx),
            typeof(取得使用者查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetUserById")]
        public async Task<IResult> GetUserById([FromRoute] string userId)
        {
            var result = await _mediator.Send(new Query(userId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.User.GetUserById
{
    public record Query(string userId) : IRequest<ResultResponse<GetUserByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetUserByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetUserByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.OrgSetUp_User.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == request.userId);

            if (single is null)
                return ApiResponseHelper.NotFound<GetUserByIdResponse>(null, request.userId);

            var roles = await _context.Auth_User_Role.AsNoTracking().Where(x => x.UserId == request.userId).Select(x => x.RoleId).ToArrayAsync();

            var organize = await _context.OrgSetUp_Organize.AsNoTracking().SingleOrDefaultAsync(x => x.OrganizeCode == single.OrganizeCode);

            var response = _mapper.Map<GetUserByIdResponse>(single);

            response.RoleId = roles;
            response.OrganizeName = organize is null ? null : organize.OrganizeName;
            response.CaseDispatchGroups = string.IsNullOrWhiteSpace(single.CaseDispatchGroup)
                ? []
                : single
                    .CaseDispatchGroup.Split(",")
                    .Select(y => new CaseDispatchGroupModel { CaseDispatchGroup = Enum.Parse<CaseDispatchGroup>(y) })
                    .ToList();

            return ApiResponseHelper.Success(response);
        }
    }
}
