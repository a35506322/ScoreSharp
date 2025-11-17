using ScoreSharp.API.Modules.Manage.Stakeholder.UpdateStakeholderById;

namespace ScoreSharp.API.Modules.Manage.Stakeholder
{
    public partial class StakeholderController
    {
        /// <summary>
        /// 更新單筆利害關係人
        /// </summary>
        /// <param name="seqNo">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新單筆利害關係人_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新單筆利害關係人_2000_ResEx),
            typeof(更新單筆利害關係人查無此資料_4001_ResEx),
            typeof(更新單筆利害關係人路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [HttpPut("{seqNo}")]
        [OpenApiOperation("UpdateStakeholderById")]
        public async Task<IResult> UpdateStakeholderById([FromRoute] long seqNo, [FromBody] UpdateStakeholderByIdRequest request)
        {
            var result = await _mediator.Send(new Command(seqNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.Stakeholder.UpdateStakeholderById
{
    public record Command(long seqNo, UpdateStakeholderByIdRequest updateStakeholderByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext _context, IJWTProfilerHelper _jwthelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var seqNo = request.seqNo;
            var dto = request.updateStakeholderByIdRequest;

            if (seqNo != dto.SeqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.Reviewer_Stakeholder.SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (entity == null)
                return ApiResponseHelper.NotFound<string>(null, seqNo.ToString());

            entity.ID = dto.ID;
            entity.UserId = dto.UserId;
            entity.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(seqNo.ToString(), seqNo.ToString());
        }
    }
}
