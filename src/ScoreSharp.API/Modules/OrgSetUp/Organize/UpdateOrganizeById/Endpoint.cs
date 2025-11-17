using ScoreSharp.API.Modules.OrgSetUp.Organize.UpdateOrganizeById;

namespace ScoreSharp.API.Modules.OrgSetUp.Organize
{
    public partial class OrganizeController
    {
        ///<summary>
        /// 更新單筆組織 By organizeCode
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Organize/UpdateOrganizeById/ORG001
        ///
        /// </remarks>
        /// <param name="organizeCode">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{organizeCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(修改組織_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改組織_2000_ResEx),
            typeof(修改組織查無此資料_4001_ResEx),
            typeof(修改組織路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateOrganizeById")]
        public async Task<IResult> UpdateOrganizeById([FromRoute] string organizeCode, [FromBody] UpdateOrganizeByIdRequest request) =>
            Results.Ok(await _mediator.Send(new Command(organizeCode, request)));
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.Organize.UpdateOrganizeById
{
    public record Command(string organizeCode, UpdateOrganizeByIdRequest updateOrganizeByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.organizeCode != request.updateOrganizeByIdRequest.OrganizeCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await context.OrgSetUp_Organize.SingleOrDefaultAsync(x => x.OrganizeCode == request.organizeCode);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.organizeCode);

            var dto = request.updateOrganizeByIdRequest;

            entity.OrganizeName = dto.OrganizeName;
            entity.LegalRepresentative = dto.LegalRepresentative;
            entity.ZIPCode = dto.ZIPCode;
            entity.FullAddress = dto.FullAddress;

            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.UpdateByIdSuccess(request.organizeCode, request.organizeCode);
        }
    }
}
