using System;
using ScoreSharp.API.Modules.SetUp.Card.UpdateCardById;

namespace ScoreSharp.API.Modules.SetUp.Card
{
    public partial class CardController
    {
        /// <summary>
        /// 修改單筆信用卡卡片種類
        /// </summary>
        /// <param name="code">BIN</param>
        /// <param name="request"></param>
        /// <response code="400">
        /// 檢查選優惠辦法
        /// 1. 可選優惠辦法至少為一個
        /// 2. 不在可選優惠辦法名單中
        /// </response>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(修改信用卡卡片種類_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [EndpointSpecificExample(
            typeof(修改信用卡卡片種類_2000_ResEx),
            typeof(修改信用卡卡片種類查無資料_4001_ResEx),
            typeof(修改信用卡卡片種類路由與Req比對錯誤_4003_ResEx),
            typeof(修改信用卡卡片種類查無帳單日資料_4003_ResEx),
            typeof(修改信用卡卡片種類查無優惠辦法資料_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(修改信用卡卡片種類預設優惠不在可選名單中_4000_ResEx),
            typeof(修改信用卡卡片種類可選優惠至少一個_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("UpdateCardById")]
        public async Task<IResult> UpdateCardById([FromRoute] string code, [FromBody] UpdateCardByIdRequest request)
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Card.UpdateCardById
{
    public record Command(string code, UpdateCardByIdRequest updateCardByIdRequest) : IRequest<ResultResponse<string>>;

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
            var updateCardByIdRequest = request.updateCardByIdRequest;

            if (request.code != updateCardByIdRequest.BINCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_Card.SingleOrDefaultAsync(x => x.BINCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            var isBillDay = _context
                .SetUp_BillDay.Where(x => x.IsActive == "Y")
                .Select(x => x.BillDay)
                .Contains(updateCardByIdRequest.DefaultBillDay);
            if (!isBillDay)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無帳單日資料，請檢查");

            var validCardPromotions = _context.SetUp_CardPromotion.Where(x => x.IsActive == "Y").Select(x => x.CardPromotionCode).ToList();

            if (
                !updateCardByIdRequest.OptionalCardPromotions.All(optionalCardPromotion =>
                    validCardPromotions.Contains(optionalCardPromotion)
                )
            )
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無優惠辦法資料，請檢查");
            }

            var entities = updateCardByIdRequest
                .OptionalCardPromotions.Select(cardPromotion => new SetUp_Card_CardPromotion
                {
                    BINCode = updateCardByIdRequest.BINCode,
                    CardPromotionCode = cardPromotion,
                })
                .ToList();

            entity.IsActive = updateCardByIdRequest.IsActive;
            entity.CardName = updateCardByIdRequest.CardName;
            entity.CardCategory = updateCardByIdRequest.CardCategory;
            entity.SampleRejectionLetter = updateCardByIdRequest.SampleRejectionLetter;
            entity.DefaultBillDay = updateCardByIdRequest.DefaultBillDay;
            entity.SaleLoanCategory = updateCardByIdRequest.SaleLoanCategory;
            entity.DefaultDiscount = updateCardByIdRequest.DefaultDiscount;
            entity.PrimaryCardQuotaUpperlimit = updateCardByIdRequest.PrimaryCardQuotaUpperlimit;
            entity.PrimaryCardQuotaLowerlimit = updateCardByIdRequest.PrimaryCardQuotaLowerlimit;
            entity.PrimaryCardYearUpperlimit = updateCardByIdRequest.PrimaryCardYearUpperlimit;
            entity.PrimaryCardYearLowerlimit = updateCardByIdRequest.PrimaryCardYearLowerlimit;
            entity.SupplementaryCardQuotaUpperlimit = updateCardByIdRequest.SupplementaryCardQuotaUpperlimit;
            entity.SupplementaryCardQuotaLowerlimit = updateCardByIdRequest.SupplementaryCardQuotaLowerlimit;
            entity.SupplementaryCardYearUpperlimit = updateCardByIdRequest.SupplementaryCardYearUpperlimit;
            entity.SupplementaryCardYearLowerlimit = updateCardByIdRequest.SupplementaryCardYearLowerlimit;
            entity.IsCARDPAUnderLimit = updateCardByIdRequest.IsCARDPAUnderLimit;
            entity.CARDPACQuotaLimit = updateCardByIdRequest.CARDPACQuotaLimit;
            entity.IsApplyAdditionalCard = updateCardByIdRequest.IsApplyAdditionalCard;
            entity.IsIndependentCard = updateCardByIdRequest.IsIndependentCard;
            entity.IsIVRvCTIQuery = updateCardByIdRequest.IsIVRvCTIQuery;
            entity.IsCITSCard = updateCardByIdRequest.IsCITSCard;
            entity.IsQuickCardIssuance = updateCardByIdRequest.IsQuickCardIssuance;
            entity.IsTicket = updateCardByIdRequest.IsTicket;
            entity.IsJointGroup = updateCardByIdRequest.IsJointGroup;
            entity.CardCode = updateCardByIdRequest.CardCode;
            // 會偵測不到副表為修改狀態，要自己加上更新資訊
            entity.UpdateUserId = _jwthelper.UserId;
            entity.UpdateTime = DateTime.Now;

            await _context.SetUp_Card_CardPromotion.Where(x => x.BINCode == updateCardByIdRequest.BINCode).ExecuteDeleteAsync();
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
