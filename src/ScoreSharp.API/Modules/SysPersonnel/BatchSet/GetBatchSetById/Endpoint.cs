using ScoreSharp.API.Modules.SysPersonnel.BatchSet.GetBatchSetById;

namespace ScoreSharp.API.Modules.SysPersonnel.BatchSet
{
    public partial class BatchSetController
    {
        /// <summary>
        /// 查詢單筆排程設定
        /// </summary>
        /// /// <remarks>
        /// Sample Router:
        ///
        ///     /BatchSet/GetBatchSetById/1
        ///
        /// Notes :
        ///
        ///     系統參數只有一筆
        /// </remarks>
        /// <param name="seqNo">PK</param>
        /// <returns></returns>
        [HttpGet("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetBatchSetByIdResponse>))]
        [EndpointSpecificExample(
            typeof(查詢單筆排程設定_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetBatchSetById")]
        public async Task<IResult> GetBatchSetById([FromRoute] int seqNo)
        {
            var result = await _mediator.Send(new Query(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.BatchSet.GetBatchSetById
{
    public record Query(int seqNo) : IRequest<ResultResponse<GetBatchSetByIdResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetBatchSetByIdResponse>>
    {
        public async Task<ResultResponse<GetBatchSetByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await context.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync(x => x.SeqNo == request.seqNo);

            var response = mapper.Map<GetBatchSetByIdResponse>(entity);

            return ApiResponseHelper.Success(response);
        }
    }
}
