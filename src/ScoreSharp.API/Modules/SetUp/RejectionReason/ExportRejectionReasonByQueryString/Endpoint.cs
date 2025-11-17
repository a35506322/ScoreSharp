using MiniExcelLibs;
using ScoreSharp.API.Modules.SetUp.RejectionReason.ExportRejectionReasonByQueryString;

namespace ScoreSharp.API.Modules.SetUp.RejectionReason
{
    public partial class RejectionReasonController
    {
        /// <summary>
        /// 匯出多筆補件原因
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString :
        ///
        ///     ?IsActive=Y&amp;RejectionReasonName=卡&amp;RejectionReasonCode
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<ExportRejectionReasonByQueryStringResponse>))]
        [EndpointSpecificExample(
            typeof(匯出退件原因Excel_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("ExportRejectionReasonByQueryString")]
        public async Task<IResult> ExportRejectionReasonByQueryString([FromQuery] RejectionReasonByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.RejectionReason.ExportRejectionReasonByQueryString
{
    public record Query(RejectionReasonByQueryStringRequest rejectionReasonByQueryStringRequest)
        : IRequest<ResultResponse<ExportRejectionReasonByQueryStringResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<ExportRejectionReasonByQueryStringResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMiniExcelHelper _miniExcelHelper;

        public Handler(ScoreSharpContext context, IMiniExcelHelper miniExcelHelper)
        {
            _context = context;
            _miniExcelHelper = miniExcelHelper;
        }

        public async Task<ResultResponse<ExportRejectionReasonByQueryStringResponse>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var filter = request.rejectionReasonByQueryStringRequest;

            var exportDto = await _context
                .SetUp_RejectionReason.Where(x => string.IsNullOrEmpty(filter.IsActive) || x.IsActive == filter.IsActive)
                .Where(x => string.IsNullOrEmpty(filter.RejectionReasonCode) || x.RejectionReasonCode == filter.RejectionReasonCode)
                .Where(x => string.IsNullOrEmpty(filter.RejectionReasonName) || x.RejectionReasonName.Contains(filter.RejectionReasonName))
                .Select(x => new RejectionReasonByQueryStringExportDto()
                {
                    IsActive = x.IsActive,
                    RejectionReasonCode = x.RejectionReasonCode,
                    RejectionReasonName = x.RejectionReasonName,
                })
                .ToListAsync();

            MemoryStream memoryStream = await _miniExcelHelper.基本匯出ExcelToStream(exportDto);

            var result = new ExportRejectionReasonByQueryStringResponse
            {
                FileContent = memoryStream.ToArray(),
                FileName = "退件原因.xlsx",
            };

            return ApiResponseHelper.Success(result);
        }
    }
}
