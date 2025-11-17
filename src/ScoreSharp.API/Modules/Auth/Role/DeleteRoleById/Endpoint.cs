using ScoreSharp.API.Modules.Auth.Role.DeleteRoleById;

namespace ScoreSharp.API.Modules.Auth.Role
{
    public partial class RoleController
    {
        /// <summary>
        /// 刪除單筆角色
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Role/DeleteRoleById/Consultant
        ///
        /// </remarks>
        /// <param name="roleId">PK</param>
        /// <returns></returns>
        [HttpDelete("{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除角色_2000_ResEx),
            typeof(刪除角色查無此資料_4001_ResEx),
            typeof(刪除角色此資源已被使用_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteRoleById")]
        public async Task<IResult> DeleteRoleById([FromRoute] string roleId)
        {
            var result = await _mediator.Send(new Command(roleId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.Role.DeleteRoleById
{
    public record Command(string roleId) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IFusionCache _fusionCache;

        public Handler(ScoreSharpContext context, IFusionCache fusionCache)
        {
            _context = context;
            _fusionCache = fusionCache;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.Auth_Role.SingleOrDefaultAsync(x => x.RoleId == request.roleId);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.roleId);

            var usedData = await _context.Auth_User_Role.AsNoTracking().Where(x => x.RoleId == request.roleId).ToListAsync();
            if (usedData.Count() > 0)
                return ApiResponseHelper.此資源已被使用<string>(null, request.roleId);

            _context.Remove(single);
            await _context.Auth_Role_Router_Action.Where(x => x.RoleId == request.roleId).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.roleId);
        }
    }
}
