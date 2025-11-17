using ScoreSharp.API.Modules.Auth.Role.GetRoleById;

namespace ScoreSharp.API.Modules.Auth.Role
{
    public partial class RoleController
    {
        /// <summary>
        /// 查詢單筆角色
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /Role/GetRoleById/Admin
        ///
        /// </remarks>
        /// <param name="roleId">PK</param>
        /// <returns></returns>
        [HttpGet("{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetRoleByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得角色_2000_ResEx),
            typeof(取得角色查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetRoleById")]
        public async Task<IResult> GetRoleById([FromRoute] string roleId)
        {
            var result = await _mediator.Send(new Query(roleId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Role.GetRoleById
{
    public record Query(string roleId) : IRequest<ResultResponse<GetRoleByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetRoleByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetRoleByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.Auth_Role.AsNoTracking().SingleOrDefaultAsync(x => x.RoleId == request.roleId);

            if (single is null)
                return ApiResponseHelper.NotFound<GetRoleByIdResponse>(null, request.roleId);

            var result = _mapper.Map<GetRoleByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
