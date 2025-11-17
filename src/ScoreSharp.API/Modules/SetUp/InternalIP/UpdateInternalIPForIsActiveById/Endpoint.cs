using System;
using ScoreSharp.API.Modules.SetUp.InternalIP.UpdateInternalIPForIsActiveById;

namespace ScoreSharp.API.Modules.SetUp.InternalIP
{
    public partial class InternalIPController
    {
        /// <summary>
        /// 單筆修改行內IP狀態
        /// </summary>
        /// <param name="ip">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{ip}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(修改行內IP狀態_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改行內IP狀態_2000_ResEx),
            typeof(修改行內IP狀態查無此資料_4001_ResEx),
            typeof(修改行內IP狀態路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateInternalIPForIsActiveById")]
        public async Task<IResult> UpdateInternalIPForIsActiveById(
            [FromRoute] string ip,
            [FromBody] UpdateInternalIPForIsActiveByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(ip, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.InternalIP.UpdateInternalIPForIsActiveById
{
    public record Command(string ip, UpdateInternalIPForIsActiveByIdRequest updateInternalIPForIsActiveByIdRequest)
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
            var updateInternalIPForIsActiveByIdRequest = request.updateInternalIPForIsActiveByIdRequest;

            var entity = await _context.SetUp_InternalIP.SingleOrDefaultAsync(x => x.IP == request.ip);

            if (request.ip != request.updateInternalIPForIsActiveByIdRequest.IP)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.ip);

            entity.IsActive = updateInternalIPForIsActiveByIdRequest.IsActive;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(request.ip, request.ip);
        }
    }
}
