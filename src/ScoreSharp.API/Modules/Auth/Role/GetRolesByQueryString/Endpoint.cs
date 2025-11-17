using ScoreSharp.API.Modules.Auth.Role.GetRolesByQueryString;

namespace ScoreSharp.API.Modules.Auth.Role
{
    public partial class RoleController
    {
        /// <summary>
        /// 查詢多筆角色
        /// </summary>
        /// <remarks>
        ///
        ///     Sample QueryString:
        ///
        ///     ?IsActive=Y
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetRolesByQueryStringResponse>))]
        [EndpointSpecificExample(
            typeof(取得角色_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetRolesByQueryString")]
        public async Task<IResult> GetRolesByQueryString([FromQuery] GetRolesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Role.GetRolesByQueryString
{
    public record Query(GetRolesByQueryStringRequest getRolesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetRolesByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetRolesByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetRolesByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var dto = request.getRolesByQueryStringRequest;

            var entities = await _context
                .Auth_Role.Where(x => string.IsNullOrEmpty(dto.IsActive) || x.IsActive == dto.IsActive)
                .ToListAsync();

            var result = _mapper.Map<List<GetRolesByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success<List<GetRolesByQueryStringResponse>>(result);
        }
    }
}
