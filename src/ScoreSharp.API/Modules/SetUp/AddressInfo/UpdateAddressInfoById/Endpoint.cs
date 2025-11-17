using ScoreSharp.API.Modules.SetUp.AddressInfo.UpdateAddressInfoById;

namespace ScoreSharp.API.Modules.SetUp.AddressInfo
{
    public partial class AddressInfoController
    {
        /// <summary>
        /// 單筆修改地址資訊
        /// </summary>
        /// <param name="seqNo">流水號</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改地址資訊_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改地址資訊_2000_ResEx),
            typeof(修改地址資訊查無此資料_4001_ResEx),
            typeof(修改地址資訊路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateAddressInfoById")]
        public async Task<IResult> UpdateAddressInfoById([FromRoute] string seqNo, [FromBody] UpdateAddressInfoByIdRequest request)
        {
            var result = await _mediator.Send(new Command(seqNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AddressInfo.UpdateAddressInfoById
{
    public record Command(string seqNo, UpdateAddressInfoByIdRequest updateAddressInfoByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.seqNo != request.updateAddressInfoByIdRequest.SeqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await context.SetUp_AddressInfo.SingleOrDefaultAsync(x => x.SeqNo == request.seqNo);
            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.seqNo);

            var dto = request.updateAddressInfoByIdRequest;

            entity.ZIPCode = dto.ZIPCode;
            entity.City = dto.City;
            entity.Area = dto.Area;
            entity.Road = dto.Road;
            entity.Scope = dto.Scope;
            entity.IsActive = dto.IsActive;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(request.seqNo, request.seqNo);
        }
    }
}
