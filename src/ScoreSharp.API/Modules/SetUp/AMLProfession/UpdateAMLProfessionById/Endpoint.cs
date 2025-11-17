using ScoreSharp.API.Modules.SetUp.AMLProfession.UpdateAMLProfessionById;

namespace ScoreSharp.API.Modules.SetUp.AMLProfession
{
    public partial class AMLProfessionController
    {
        /// <summary>
        /// 單筆修改AML職業別
        /// </summary>
        /// <param name="code">PK</param>
        /// <param name="request"></param>
        /// <response code="400">
        /// 檢查 Version (版本) 格式是否為 yyyyMMdd
        /// </response>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改AML職業別_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改AML職業別_2000_ResEx),
            typeof(修改AML職業別查無此資料_4001_ResEx),
            typeof(修改AML職業別路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(修改AML職業別版本不符合格式_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("UpdateAMLProfessionById")]
        public async Task<IResult> UpdateAMLProfessionById([FromRoute] string code, [FromBody] UpdateAMLProfessionByIdRequest request)
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AMLProfession.UpdateAMLProfessionById
{
    public record Command(string code, UpdateAMLProfessionByIdRequest updateAMLProfessionByIdRequest) : IRequest<ResultResponse<string>>;

    public class Hanlder : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Hanlder(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateAMLProfessionByIdRequest = request.updateAMLProfessionByIdRequest;

            if (request.code != updateAMLProfessionByIdRequest.SeqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(request.code);

            var all = await _context.SetUp_AMLProfession.AsNoTracking().ToListAsync();

            var origin = all.SingleOrDefault(x => x.SeqNo == request.code);

            if (origin is null)
                return ApiResponseHelper.NotFound<string>(request.code, request.code.ToString());

            bool 驗證是否為原始版本和代碼 =
                origin.AMLProfessionCode == updateAMLProfessionByIdRequest.AMLProfessionCode
                && origin.Version == updateAMLProfessionByIdRequest.Version;

            if (!(驗證是否為原始版本和代碼))
            {
                var isDuplicate = all.Where(x => x.SeqNo != origin.SeqNo)
                    .SingleOrDefault(x =>
                        (
                            x.AMLProfessionCode == updateAMLProfessionByIdRequest.AMLProfessionCode
                            && x.Version == updateAMLProfessionByIdRequest.Version
                        )
                    );

                if (isDuplicate != null)
                    return ApiResponseHelper.DataAlreadyExists<string>(null, origin.SeqNo);
            }

            var entity = await _context.SetUp_AMLProfession.SingleOrDefaultAsync(x => x.SeqNo == request.code);

            entity.AMLProfessionCode = updateAMLProfessionByIdRequest.AMLProfessionCode;
            entity.Version = updateAMLProfessionByIdRequest.Version;
            entity.IsActive = updateAMLProfessionByIdRequest.IsActive;
            entity.AMLProfessionName = updateAMLProfessionByIdRequest.AMLProfessionName;
            entity.AMLProfessionCompareResult = updateAMLProfessionByIdRequest.AMLProfessionCompareResult;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
