using ScoreSharp.API.Infrastructures.Adapter;
using ScoreSharp.API.Modules.OrgSetUp.User.GetADUserById;

namespace ScoreSharp.API.Modules.OrgSetUp.User
{
    public partial class UserController
    {
        /// <summary>
        /// 取得單筆AD Server User
        /// </summary>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<ResultResponse<GetADUserByIdResponse>>))]
        [EndpointSpecificExample(
            typeof(取得單筆ADServerUser_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetADUserById")]
        public async Task<IResult> GetADUserById([FromRoute] string userId)
        {
            var result = await _mediator.Send(new Query(userId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.User.GetADUserById
{
    public record Query(string userId) : IRequest<ResultResponse<GetADUserByIdResponse>>;

    public class Handler(ILDAPHelper ldapHelper, ILDAPAdapter ldapAdapter, IWebHostEnvironment env)
        : IRequestHandler<Query, ResultResponse<GetADUserByIdResponse>>
    {
        public async Task<ResultResponse<GetADUserByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            /*
             * Development、Production 環境
             * => 透過 AD Server

             * Testing 環境
             * => 透過 192.168.233.40 => LDAP Server
             */

            if (env.IsDevelopment() || env.IsProduction())
            {
                var ldapInfo = ldapHelper.SearchBySAMAccountName(request.userId);

                if (ldapInfo == null)
                {
                    return ApiResponseHelper.NotFound<GetADUserByIdResponse>(null, request.userId);
                }

                GetADUserByIdResponse response = new()
                {
                    DisplayName = ldapInfo.DisplayName,
                    MemberOf = ldapInfo.MemberOf,
                    SAMAccountName = ldapInfo.SAMAccountName,
                    UserPrincipalName = ldapInfo.UserPrincipalName,
                };

                return ApiResponseHelper.Success(response);
            }
            else
            {
                var ldapInfo = await ldapAdapter.SearchBySAMAccountName(request.userId);

                if (ldapInfo.IsSuccess && ldapInfo.Data != null)
                {
                    GetADUserByIdResponse response = new()
                    {
                        DisplayName = ldapInfo.Data.DisplayName,
                        MemberOf = ldapInfo.Data.MemberOf,
                        SAMAccountName = ldapInfo.Data.SAMAccountName,
                        UserPrincipalName = ldapInfo.Data.UserPrincipalName,
                    };

                    return ApiResponseHelper.Success(response);
                }
                else if (ldapInfo.IsSuccess && ldapInfo.Data == null)
                {
                    return ApiResponseHelper.NotFound<GetADUserByIdResponse>(null, request.userId);
                }
                else
                {
                    return ApiResponseHelper.CheckThirdPartyApiError<GetADUserByIdResponse>(null, request.userId);
                }
            }
        }
    }
}
