using ScoreSharp.API.Modules.OrgSetUp.User.ChangeNotADMimaById;

namespace ScoreSharp.API.Modules.OrgSetUp.User
{
    public partial class UserController
    {
        /// <summary>
        /// 修改非AD User密碼
        /// </summary>
        /// <returns></returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<ResultResponse<string>>))]
        [EndpointSpecificExample(
            typeof(修改非ADUser密碼_2000_ResEx),
            typeof(修改非ADUser密碼查無此資料_4001_ResEx),
            typeof(修改非ADUser密碼路由與Req比對錯誤_4003_ResEx),
            typeof(修改非ADUser密碼AD帳號無法修改密碼_4004_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("ChangeNotADMimaById")]
        public async Task<IResult> ChangeNotADMimaById([FromRoute] string userId, [FromBody] ChangeNotADMimaByIdRequest request)
        {
            var result = await _mediator.Send(new Command(userId, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.User.ChangeNotADMimaById
{
    public record Command(string userId, ChangeNotADMimaByIdRequest changeNotADMimaByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var changeNotADMimaByIdRequest = request.changeNotADMimaByIdRequest;

            if (changeNotADMimaByIdRequest.UserId != request.userId)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var user = await context.OrgSetUp_User.SingleOrDefaultAsync(x => x.UserId == request.userId);

            if (user is null)
                return ApiResponseHelper.NotFound<string>(null, request.userId);

            bool isAD = user.IsAD == "Y";
            if (isAD)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "AD 帳號無法修改密碼");

            user.Mima = changeNotADMimaByIdRequest.NewMima;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.userId, request.userId);
        }
    }
}
