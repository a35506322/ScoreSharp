using ScoreSharp.API.Modules.Manage.Stakeholder.GetStakeholderById;

namespace ScoreSharp.API.Modules.Manage.Stakeholder
{
    public partial class StakeholderController
    {
        /// <summary>
        /// 查詢單筆利害關係人
        /// </summary>
        /// <param name="seqNo">PK</param>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /Stakeholder/GetStakeholderById/3
        ///
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<查詢單筆利害關係人_2000_ResEx>))]
        [EndpointSpecificExample(
            typeof(查詢單筆利害關係人_2000_ResEx),
            typeof(查詢單筆利害關係人查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [HttpGet("{seqNo}")]
        [OpenApiOperation("GetStakeholderById")]
        public async Task<IResult> GetStakeholderById([FromRoute] long seqNo)
        {
            var response = await _mediator.Send(new Query(seqNo));
            return Results.Ok(response);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.Stakeholder.GetStakeholderById
{
    public record Query(long seqNo) : IRequest<ResultResponse<GetStakeholderByIdResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetStakeholderByIdResponse>>
    {
        public async Task<ResultResponse<GetStakeholderByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var seqNo = request.seqNo;

            var entity = await context.Reviewer_Stakeholder.AsNoTracking().SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (entity is null)
            {
                return ApiResponseHelper.NotFound<GetStakeholderByIdResponse>(null, seqNo.ToString());
            }

            var response = mapper.Map<GetStakeholderByIdResponse>(entity);

            return ApiResponseHelper.Success(response, seqNo.ToString());
        }
    }
}
