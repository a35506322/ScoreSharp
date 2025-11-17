using ScoreSharp.API.Modules.SetUp.CreditCheckCode.ExportCreditCheckCodeByQueryString;

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode
{
    public partial class CreditCheckCodeController
    {
        /// <summary>
        /// 匯出多筆徵信代碼
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString :
        ///
        ///     ?IsActive=Y&amp;CreditCheckCodeName=&amp;CreditCheckCodeCode&amp;IsManualReview&amp;IsSystemReview
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<ExportCreditCheckCodeByQueryStringResponse>))]
        [EndpointSpecificExample(
            typeof(匯出徵信代碼Excel_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("ExportCreditCheckCodeByQueryString")]
        public async Task<IResult> ExportCreditCheckCodeByQueryString([FromQuery] CreditCheckCodeByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.ExportCreditCheckCodeByQueryString
{
    public record Query(CreditCheckCodeByQueryStringRequest creditCheckCodeByQueryStringRequest)
        : IRequest<ResultResponse<ExportCreditCheckCodeByQueryStringResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<ExportCreditCheckCodeByQueryStringResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMiniExcelHelper _miniExcelHelper;

        public Handler(ScoreSharpContext context, IMiniExcelHelper miniExcelHelper)
        {
            _context = context;
            _miniExcelHelper = miniExcelHelper;
        }

        public async Task<ResultResponse<ExportCreditCheckCodeByQueryStringResponse>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var filter = request.creditCheckCodeByQueryStringRequest;

            var exportDto = await _context
                .SetUp_CreditCheckCode.Where(x => string.IsNullOrEmpty(filter.IsActive) || x.IsActive == filter.IsActive)
                .Where(x => string.IsNullOrEmpty(filter.CreditCheckCode) || x.CreditCheckCode == filter.CreditCheckCode)
                .Where(x => string.IsNullOrEmpty(filter.CreditCheckCodeName) || x.CreditCheckCodeName.Contains(filter.CreditCheckCodeName))
                .Select(x => new CreditCheckCodeByQueryStringExportDto()
                {
                    IsActive = x.IsActive,
                    CreditCheckCode = x.CreditCheckCode,
                    CreditCheckCodeName = x.CreditCheckCodeName,
                })
                .ToListAsync();

            MemoryStream memoryStream = await _miniExcelHelper.基本匯出ExcelToStream(exportDto);

            var result = new ExportCreditCheckCodeByQueryStringResponse
            {
                FileContent = memoryStream.ToArray(),
                FileName = "徵信代碼.xlsx",
            };

            return ApiResponseHelper.Success(result);
        }
    }
}
