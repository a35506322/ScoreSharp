using Microsoft.Extensions.Options;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.InsertApplyFileAttachment;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 新增審行員附件
        /// </summary>
        /// <remarks>
        ///
        ///
        ///     目前上傳圖檔資料僅接受JPG和PNG
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [OpenApiOperation("InsertApplyFileAttachment")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(新增審行員附件_2000_ResEx),
            typeof(新增審行員附件檔案格式有誤_4003_ResEx),
            typeof(新增審行員附件_4003_ResEx),
            typeof(新增審行員附件查無申請書編號_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> InsertApplyFileAttachment([FromForm] InsertApplyFileAttachmentRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.InsertApplyFileAttachment
{
    public record Command(InsertApplyFileAttachmentRequest insertApplyFileAttachmentRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, ScoreSharpFileContext fileContext, ILogger<Handler> logger)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertApplyFileAttachmentRequest = request.insertApplyFileAttachmentRequest;
            string applyNo = insertApplyFileAttachmentRequest.ApplyNo;
            var uploadFile = insertApplyFileAttachmentRequest.AttachmentFile;

            var main = await context.Reviewer_ApplyCreditCardInfoMain.AsNoTracking().SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (main is null)
                return ApiResponseHelper.NotFound<string>(applyNo, applyNo);

            // check檔案格式
            if (!IsValidFileFormat(uploadFile))
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "檔案格式不符合要求，請上傳符合 JPG 或 PNG 格式的檔案");

            // check檔名
            Guid fileId = Guid.NewGuid();
            string uniqueFileName = GenerateUniqueFileName(applyNo, uploadFile.FileName, fileId);

            // 儲存檔案
            using var memoryStream = new MemoryStream((int)uploadFile.Length);
            await uploadFile.CopyToAsync(memoryStream, cancellationToken);
            var fileContent = memoryStream.ToArray();

            Reviewer_ApplyFile file = new()
            {
                ApplyNo = applyNo,
                FileName = uniqueFileName,
                FileType = FileType.行員附件,
                FileContent = fileContent,
                FileId = fileId,
            };

            Reviewer_ApplyFileAttachment entity = new()
            {
                ApplyNo = applyNo,
                ID = insertApplyFileAttachmentRequest.ID,
                UserType = insertApplyFileAttachmentRequest.Type,
                FileId = fileId,
                DBName = "ScoreSharp_File",
                IsHistory = "N",
            };

            var result = await context.AddAsync(entity);
            await context.SaveChangesAsync();

            var result2 = await fileContext.AddAsync(file);
            await fileContext.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(fileId.ToString(), fileId.ToString());
        }

        private bool IsValidFileFormat(IFormFile file)
        {
            string[] permittedExtensions = { ".png", ".jpg", ".jpeg" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            return !string.IsNullOrEmpty(extension) && permittedExtensions.Contains(extension);
        }

        private string GenerateUniqueFileName(string applyNo, string fileName, Guid fileId)
        {
            string sanitizedFileName = Path.GetFileName(fileName);
            return $"{applyNo}_{fileId}_{sanitizedFileName}";
        }
    }
}
