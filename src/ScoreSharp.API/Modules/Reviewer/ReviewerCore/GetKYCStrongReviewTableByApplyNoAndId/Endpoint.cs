using PuppeteerSharp;
using PuppeteerSharp.Media;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetKYCStrongReviewTableByApplyNoAndId;
using ScoreSharp.RazorTemplate.Kyc;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得KYC加強審核表格
        /// </summary>
        /// <param name="applyNo">申請書編號</param>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetKYCStrongReviewTableByApplyNoAndIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得KYC加強審核表格_查無此申請書編號_4001_ResEx),
            typeof(取得KYC加強審核表格_未產生KYC加強審核表格_4003_ResEx),
            typeof(取得KYC加強審核表格_成功_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetKYCStrongReviewTableByApplyNoAndId")]
        [HttpGet("{applyNo}/{id}")]
        public async Task<IResult> GetKYCStrongReviewTableByApplyNoAndId([FromRoute] string applyNo, [FromRoute] string id)
        {
            var result = await _mediator.Send(new Query(applyNo, id));
            // return Results.File(result.ReturnData.KYCStrongReviewTable, result.ReturnData.ContentType, result.ReturnData.FileName);
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetKYCStrongReviewTableByApplyNoAndId
{
    public record Query(string applyNo, string id) : IRequest<ResultResponse<GetKYCStrongReviewTableByApplyNoAndIdResponse>>;

    public class Handler(ScoreSharpContext context, IRazorTemplateEngine razorTemplate, IConfiguration configuration, ILogger<Handler> logger)
        : IRequestHandler<Query, ResultResponse<GetKYCStrongReviewTableByApplyNoAndIdResponse>>
    {
        public async Task<ResultResponse<GetKYCStrongReviewTableByApplyNoAndIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            var id = request.id;

            var financeCheckInfos = await context
                .Reviewer_FinanceCheckInfo.AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.ID == id);

            if (financeCheckInfos == null)
            {
                return ApiResponseHelper.NotFound<GetKYCStrongReviewTableByApplyNoAndIdResponse>(null, $"{applyNo}-{id}");
            }

            if (String.IsNullOrEmpty(financeCheckInfos.KYC_StrongReDetailJson))
            {
                return ApiResponseHelper.BusinessLogicFailed<GetKYCStrongReviewTableByApplyNoAndIdResponse>(
                    null,
                    $"此申請書編號 {applyNo}及ID {id}，未產生KYC加強審核表格"
                );
            }

            var renderedView = await GetKYCStrongReviewTableByKYCVersion(
                financeCheckInfos.KYC_StrongReVersion,
                financeCheckInfos.KYC_StrongReDetailJson
            );

            var kycStrongReviewTable = await GetKYCStrongReviewTableByKYCVersion(
                financeCheckInfos.KYC_StrongReVersion,
                financeCheckInfos.KYC_StrongReDetailJson
            );

            // 計時
            var stopwatch = Stopwatch.StartNew();
            // ! 開發環境的 dev chrome.exe 沒有問題，但發佈到正式機要改用別的 chrome.exe ，此處有說　https://stackoverflow.com/questions/79027560/puppeteer-sharp-times-out-when-running-via-iis
            var executablePath = configuration.GetValue<string>("Chrome:ExecutablePath");
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true, ExecutablePath = executablePath });

            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(kycStrongReviewTable);
            var pdfBytes = await page.PdfDataAsync(
                new PdfOptions
                {
                    Format = PaperFormat.A4,
                    PrintBackground = true,
                    Scale = 0.98M,
                }
            );
            stopwatch.Stop();
            logger.LogInformation($"生成PDF時間: {stopwatch.ElapsedMilliseconds / 1000}秒");

            return ApiResponseHelper.Success(
                new GetKYCStrongReviewTableByApplyNoAndIdResponse
                {
                    ApplyNo = applyNo,
                    ID = id,
                    KYCStrongReviewTable = pdfBytes,
                    FileName = $"KYC加強審核執行表_{applyNo}_{id}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf",
                    ContentType = "application/pdf",
                },
                $"此申請書編號 {applyNo}及ID {id}，查詢KYC加強審核表格成功"
            );
        }

        private async Task<string> GetKYCStrongReviewTableByKYCVersion(string version, string detailJson)
        {
            if (version == "20210501")
            {
                var KYCStrongReviewTable = JsonHelper.反序列化物件不分大小寫<KYCStrongReDetail_20210501>(detailJson);
                var renderedView = await razorTemplate.RenderAsync("Kyc/KYCStrongReDetailView_20210501.cshtml", KYCStrongReviewTable);
                return renderedView;
            }
            else
            {
                throw new Exception($"不支援的版本: {version}");
            }
        }
    }
}
