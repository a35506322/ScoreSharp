using ScoreSharp.API.Modules.SetUp.ApplicationCategory.GetApplicationCategoriesByQueryString;

namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory
{
    public partial class ApplicationCategoryController
    {
        /// <summary>
        /// 查詢多筆申請書類別
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;ApplicationCategoryName=&amp;IsOCRForm=&amp;ApplicationCategoryCode=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetApplicationCategoriesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得申請書類別_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplicationCategoriesByQueryString")]
        public async Task<IResult> GetApplicationCategoriesByQueryString([FromQuery] GetApplicationCategoriesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory.GetApplicationCategoriesByQueryString
{
    public record Query(GetApplicationCategoriesByQueryStringRequest getApplicationCategoriesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetApplicationCategoriesByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetApplicationCategoriesByQueryStringResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ScoreSharpContext _context;

        public Handler(IMapper mapper, ScoreSharpContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ResultResponse<List<GetApplicationCategoriesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getApplicationCategoriesByQueryStringRequest = request.getApplicationCategoriesByQueryStringRequest;

            List<string> sortBy = new List<string> { "J", "M", "V" };

            var all = _context.View_ApplicationCategoryJoinCard.AsNoTracking().ToList();

            var entites = all.Where(x =>
                    String.IsNullOrEmpty(getApplicationCategoriesByQueryStringRequest.IsActive)
                    || x.IsActive == getApplicationCategoriesByQueryStringRequest.IsActive
                )
                .Where(x =>
                    String.IsNullOrEmpty(getApplicationCategoriesByQueryStringRequest.ApplicationCategoryName)
                    || x.ApplicationCategoryName.Contains(getApplicationCategoriesByQueryStringRequest.ApplicationCategoryName)
                )
                .Where(x =>
                    String.IsNullOrEmpty(getApplicationCategoriesByQueryStringRequest.ApplicationCategoryCode)
                    || x.ApplicationCategoryCode.Contains(getApplicationCategoriesByQueryStringRequest.ApplicationCategoryCode)
                )
                .Where(x =>
                    String.IsNullOrEmpty(getApplicationCategoriesByQueryStringRequest.IsOCRForm)
                    || x.IsOCRForm == getApplicationCategoriesByQueryStringRequest.IsOCRForm
                )
                .ToList();

            var result = entites
                .GroupBy(x => new
                {
                    x.ApplicationCategoryCode,
                    x.ApplicationCategoryName,
                    x.IsOCRForm,
                    x.IsActive,
                    x.AddTime,
                    x.UpdateTime,
                    x.AddUserId,
                    x.UpdateUserId,
                })
                .Select(x => new GetApplicationCategoriesByQueryStringResponse
                {
                    ApplicationCategoryCode = x.Key.ApplicationCategoryCode,
                    ApplicationCategoryName = x.Key.ApplicationCategoryName,
                    IsOCRForm = x.Key.IsOCRForm,
                    IsActive = x.Key.IsActive,
                    AddTime = x.Key.AddTime,
                    UpdateTime = x.Key.UpdateTime,
                    AddUserId = x.Key.AddUserId,
                    UpdateUserId = x.Key.UpdateUserId,
                    CardInfo = x.Select(y => new CardInfoDto
                        {
                            BINCode = y.BINCode,
                            CardCode = y.CardCode,
                            CardName = y.CardName,
                        })
                        .OrderBy(sorted => sortBy.Contains(sorted.CardCode.Substring(0, 1)) ? 0 : 1)
                        .ThenBy(sorted => sortBy.IndexOf(sorted.CardCode.Substring(0, 1)))
                        .ToList(),
                })
                .ToList();

            return ApiResponseHelper.Success(result);
        }
    }
}
