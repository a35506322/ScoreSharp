using ScoreSharp.API.Modules.SetUp.Template.UpdateTemplateById;

namespace ScoreSharp.API.Modules.SetUp.Template
{
    public partial class TemplateController
    {
        /// <summary>
        ///  單筆修改樣板
        /// </summary>
        /// <param name="templateId">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{templateId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(修改單筆樣板_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改單筆樣板_2000_ResEx),
            typeof(修改單筆樣板查無此資料_4001_ResEx),
            typeof(修改單筆樣板路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateTemplateById")]
        public async Task<IResult> UpdateTemplateById([FromRoute] string templateId, [FromBody] UpdateTemplateByIdRequest request)
        {
            var result = await _mediator.Send(new Command(templateId, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Template.UpdateTemplateById
{
    public record Command(string templateId, UpdateTemplateByIdRequest updateTemplateByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.updateTemplateByIdRequest;

            if (dto.TemplateId != request.templateId)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await context.SetUp_Template.SingleOrDefaultAsync(x => x.TemplateId == request.templateId);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.templateId);

            entity.TemplateName = dto.TemplateName;
            entity.IsActive = dto.IsActive;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.templateId, request.templateId);
        }
    }
}
