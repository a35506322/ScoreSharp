using System;
using ScoreSharp.API.Modules.Auth.Role.UpdateRoleById;

namespace ScoreSharp.API.Modules.Auth.Role
{
    public partial class RoleController
    {
        /// <summary>
        /// 更新單筆角色
        /// </summary>
        /// <param name="roleId">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改角色_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改角色_2000_ResEx),
            typeof(修改角色查無此資料_4001_ResEx),
            typeof(修改角色路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateRoleById")]
        public async Task<IResult> UpdateRoleById([FromRoute] string roleId, [FromBody] UpdateRoleByIdRequest request)
        {
            var result = await _mediator.Send(new Command(roleId, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Role.UpdateRoleById
{
    public record Command(string roleId, UpdateRoleByIdRequest updateRoleByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;
        private readonly IFusionCache _fusionCache;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwthelper, IFusionCache fusionCache)
        {
            _context = context;
            _jwthelper = jwthelper;
            _fusionCache = fusionCache;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateRoleByIdRequest = request.updateRoleByIdRequest;

            if (request.roleId != updateRoleByIdRequest.RoleId)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.Auth_Role.SingleOrDefaultAsync(x => x.RoleId == request.roleId);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.roleId);

            entity.IsActive = updateRoleByIdRequest.IsActive;
            entity.RoleName = updateRoleByIdRequest.RoleName;
            entity.UpdateTime = DateTime.Now;
            entity.UpdateUserId = _jwthelper.UserId;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.roleId, request.roleId);
        }
    }
}
