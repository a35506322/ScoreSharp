using System;
using ScoreSharp.API.Modules.SetUp.ApplicationCategory.UpdateApplicationCategoryById;

namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory
{
    public partial class ApplicationCategoryController
    {
        /// <summary>
        /// 修改單筆申請書類別
        /// </summary>
        /// <param name="code">申請書類別代碼</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(修改申請書類別_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(修改申請書類別_2000_ResEx),
            typeof(修改申請書類別查無資料_4001_ResEx),
            typeof(修改申請書類別路由與Req比對錯誤_4003_ResEx),
            typeof(修改申請書類別查無信用卡卡片種類資料_4003_ResEx),
            typeof(修改申請書類別查出未啟用之信用卡卡片種類_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateApplicationCategoryById")]
        public async Task<IResult> UpdateApplicationCategoryById(
            [FromRoute] string code,
            [FromBody] UpdateApplicationCategoryByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.UpdateApplicationCategoryById
{
    public record Command(string code, UpdateApplicationCategoryByIdRequest updateApplicationCategoryByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwthelper;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwthelper)
        {
            _context = context;
            _jwthelper = jwthelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateApplicationCategoryByIdRequest = request.updateApplicationCategoryByIdRequest;

            if (request.code != updateApplicationCategoryByIdRequest.ApplicationCategoryCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_ApplicationCategory.SingleOrDefaultAsync(x => x.ApplicationCategoryCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            var allCard = _context.SetUp_Card.AsNoTracking().ToList();
            var isCard = allCard.Select(x => x.BINCode);

            if (!updateApplicationCategoryByIdRequest.BINCodes.All(cardInfo => isCard.Contains(cardInfo)))
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無信用卡卡片種類資料，請檢查");
            }

            var inactiveBINCodes = allCard.Where(x => x.IsActive == "N").Select(x => x.BINCode);

            var isNotActiveCard = updateApplicationCategoryByIdRequest.BINCodes.Where(cardInfo => inactiveBINCodes.Contains(cardInfo));

            if (isNotActiveCard.Any())
            {
                var notActiveBINCodes = allCard
                    .Where(x => isNotActiveCard.Contains(x.BINCode))
                    .Select(x => $"（{x.CardCode}）{x.CardName}")
                    .ToList();
                string notActiveBINCode = string.Join("、", notActiveBINCodes);
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"以下為未啟用之信用卡卡片種類，請檢查 :　{notActiveBINCode}");
            }

            var entities = updateApplicationCategoryByIdRequest
                .BINCodes.Select(BINCode => new SetUp_ApplicationCategory_Card
                {
                    BINCode = BINCode,
                    ApplicationCategoryCode = request.code,
                })
                .ToList();

            entity.IsActive = updateApplicationCategoryByIdRequest.IsActive;
            entity.IsOCRForm = updateApplicationCategoryByIdRequest.IsOCRForm;
            entity.ApplicationCategoryName = updateApplicationCategoryByIdRequest.ApplicationCategoryName;
            // 會偵測不到副表為修改狀態，要自己加上更新資訊
            entity.UpdateUserId = _jwthelper.UserId;
            entity.UpdateTime = DateTime.Now;

            await _context
                .SetUp_ApplicationCategory_Card.Where(x =>
                    x.ApplicationCategoryCode == updateApplicationCategoryByIdRequest.ApplicationCategoryCode
                )
                .ExecuteDeleteAsync();
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
