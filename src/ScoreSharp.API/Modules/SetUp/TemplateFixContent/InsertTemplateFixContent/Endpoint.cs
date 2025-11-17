using ScoreSharp.API.Modules.SetUp.TemplateFixContent.InsertTemplateFixContent;

namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent
{
    public partial class TemplateFixContentController
    {
        ///<summary>
        /// 新增樣板固定值設定
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        ///
        /// 樣板的固定值設定(設定 word 套版上固定的值)：
        ///
        ///     －採用 Key-Value 配對方式
        ///
        ///     1. TemplateKey ：由程式相關人員設定，用於標識樣板的唯一鍵值。
        ///     2. TemplateValue ：由行員自行設定，用於定義樣板的具體內容。
        ///
        /// </remarks>
        /// <response code="200">
        ///
        ///  確認樣板 ID 是否存在並處於啟用狀態
        ///
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增樣板固定值_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增樣板固定值_2000_ResEx),
            typeof(新增樣板固定值資料已存在_4002_ResEx),
            typeof(新增樣板固定值無效的樣板ID_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertTemplateFixContent")]
        public async Task<IResult> InsertTemplateFixContent([FromBody] InsertTemplateFixContentRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent.InsertTemplateFixContent
{
    public record Command(InsertTemplateFixContentRequest insertTemplateFixContentRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertTemplateFixContentRequest;

            var single = context
                .SetUp_TemplateFixContent.AsNoTracking()
                .SingleOrDefault(x => x.TemplateId == dto.TemplateId && x.TemplateKey == dto.TemplateKey);

            var isValidTemplate = context
                .SetUp_Template.Where(x => x.IsActive == "Y" && x.TemplateId == dto.TemplateId)
                .Select(x => x.TemplateId)
                .ToList();

            if (isValidTemplate.Count() == 0)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "無效的樣板ID，請重新確認。");

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(
                    null,
                    $"TemplateId = {dto.TemplateId} & TemplateKey = {dto.TemplateKey}"
                );

            var entity = mapper.Map<SetUp_TemplateFixContent>(dto);

            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(entity.SeqNo.ToString(), entity.SeqNo.ToString());
        }
    }
}
