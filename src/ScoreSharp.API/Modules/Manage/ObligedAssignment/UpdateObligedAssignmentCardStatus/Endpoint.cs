using ScoreSharp.API.Modules.Manage.ObligedAssignment.UpdateObligedAssignmentCardStatus;

namespace ScoreSharp.API.Modules.Manage.ObligedAssignment
{
    public partial class ObligedAssignmentController
    {
        /// <summary>
        /// 強制派案狀態權限設定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(強制派案狀態權限設定_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(強制派案狀態權限設定_2000_ResEx),
            typeof(強制派案狀態權限設定查無資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateObligedAssignmentCardStatus")]
        public async Task<IResult> UpdateObligedAssignmentCardStatus([FromBody] UpdateObligedAssignmentCardStatusRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.UpdateObligedAssignmentCardStatus
{
    public record Command(UpdateObligedAssignmentCardStatusRequest updateObligedAssignmentCardStatusRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext _context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var req = request.updateObligedAssignmentCardStatusRequest;

            var roleIdList = string.Join(',', req.RoleIds);

            var raw = await _context.Auth_ObligedAssignmentCardStatus_Role.SingleOrDefaultAsync(x => x.CardStatus == req.CardStatus);

            if (raw is null)
            {
                return ApiResponseHelper.NotFound<string>(null, req.CardStatus.ToString());
            }

            raw.RoleId = roleIdList;
            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(null, req.CardStatus.ToString());
        }
    }
}
