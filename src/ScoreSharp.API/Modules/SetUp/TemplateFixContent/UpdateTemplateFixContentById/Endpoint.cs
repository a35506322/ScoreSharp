using ScoreSharp.API.Modules.SetUp.TemplateFixContent.UpdateTemplateFixContentById;

namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent
{
    public partial class TemplateFixContentController
    {
        /// <summary>
        ///  單筆修改樣板固定值
        /// </summary>
        /// <param name="seqNo">PK</param>
        /// <param name="request"></param>
        /// <response code="200">
        ///
        ///  確認樣板 ID 是否存在並處於啟用狀態
        ///
        /// </response>
        /// <returns></returns>
        [HttpPut("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(修改單筆樣板固定值_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改單筆樣板固定值_2000_ResEx),
            typeof(修改單筆樣板固定值查無此資料_4001_ResEx),
            typeof(修改單筆樣板固定值路由與Req比對錯誤_4003_ResEx),
            typeof(修改單筆樣板固定值無效的樣板ID_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateTemplateFixContentById")]
        public async Task<IResult> UpdateTemplateFixContentById(
            [FromRoute] long seqNo,
            [FromBody] UpdateTemplateFixContentByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(seqNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.UpdateTemplateFixContentById
{
    public record Command(long seqNo, UpdateTemplateFixContentByIdRequest updateTemplateFixContentByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.updateTemplateFixContentByIdRequest;

            if (dto.SeqNo != request.seqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var isValidTemplate = context
                .SetUp_Template.Where(x => x.IsActive == "Y" && x.TemplateId == dto.TemplateId)
                .Select(x => x.TemplateId)
                .ToList();

            if (isValidTemplate.Count() == 0)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "無效的樣板ID，請重新確認。");

            var entity = await context.SetUp_TemplateFixContent.SingleOrDefaultAsync(x => x.SeqNo == dto.SeqNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.seqNo.ToString());

            entity.TemplateId = dto.TemplateId;
            entity.TemplateValue = dto.TemplateValue;
            entity.TemplateKey = dto.TemplateKey;
            entity.IsActive = dto.IsActive;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.seqNo.ToString(), request.seqNo.ToString());
        }
    }
}
