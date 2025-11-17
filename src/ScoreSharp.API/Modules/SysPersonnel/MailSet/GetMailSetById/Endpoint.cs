using ScoreSharp.API.Modules.SysPersonnel.MailSet.GetMailSetById;

namespace ScoreSharp.API.Modules.SysPersonnel.MailSet
{
    public partial class MailSetController
    {
        /// <summary>
        /// 查詢單筆郵件設定
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /MailSet/UpdateMailSetById/1
        ///
        /// Notes :
        ///
        ///     系統參數只有一筆
        /// </remarks>
        /// <param name="seqNo"></param>
        /// <returns></returns>
        [HttpGet("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetMailSetByIdResponse>))]
        [EndpointSpecificExample(
            typeof(查詢單筆郵件設定_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetMailSetById")]
        public async Task<IResult> GetMailSetById([FromRoute] int seqNo)
        {
            var result = await _mediator.Send(new Query(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.MailSet.GetMailSetById
{
    public record Query(int seqNo) : IRequest<ResultResponse<GetMailSetByIdResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetMailSetByIdResponse>>
    {
        public async Task<ResultResponse<GetMailSetByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await context.SysParamManage_MailSet.AsNoTracking().SingleOrDefaultAsync(x => x.SeqNo == request.seqNo);

            var response = mapper.Map<GetMailSetByIdResponse>(entity);

            return ApiResponseHelper.Success(response);
        }
    }
}
