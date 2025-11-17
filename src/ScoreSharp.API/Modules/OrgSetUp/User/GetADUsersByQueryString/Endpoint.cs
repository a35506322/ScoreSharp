using ScoreSharp.API.Infrastructures.Adapter;
using ScoreSharp.API.Modules.OrgSetUp.User.GetADUsersByQueryString;

namespace ScoreSharp.API.Modules.OrgSetUp.User
{
    public partial class UserController
    {
        /// <summary>
        /// 取得多筆AD Server User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetADUsersByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得ADServerUser_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetADUsersByQueryString")]
        public async Task<IResult> GetADUsersByQueryString()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.User.GetADUsersByQueryString
{
    public record Query() : IRequest<ResultResponse<List<GetADUsersByQueryStringResponse>>>;

    public class Handler(ILDAPHelper ldapHelper, ILDAPAdapter ldapAdapter, IWebHostEnvironment env)
        : IRequestHandler<Query, ResultResponse<List<GetADUsersByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetADUsersByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            /*
             * Development、Production 環境
             * => 透過 AD Server

             * Testing 環境
             * => 透過 192.168.233.40 => LDAP Server
             */

            if (env.IsDevelopment() || env.IsProduction())
            {
                var ldapInfos = ldapHelper.SearchUsersAll();
                var result = ldapInfos
                    .Where(x => x.DisplayName != null && x.MemberOf.Count > 0)
                    .Select(x => new GetADUsersByQueryStringResponse
                    {
                        DisplayName = x.DisplayName,
                        MemberOf = x.MemberOf,
                        SAMAccountName = x.SAMAccountName,
                        UserPrincipalName = x.UserPrincipalName,
                    })
                    .ToList();

                return ApiResponseHelper.Success(result);
            }
            else
            {
                var ldapInfos = await ldapAdapter.SearchUsersAll();
                if (!ldapInfos.IsSuccess)
                {
                    return ApiResponseHelper.CheckThirdPartyApiError<List<GetADUsersByQueryStringResponse>>(null, ldapInfos.ErrorMessage);
                }

                var result = ldapInfos
                    .Data.Where(x => x.DisplayName != null && x.MemberOf.Count > 0)
                    .Select(x => new GetADUsersByQueryStringResponse
                    {
                        DisplayName = x.DisplayName,
                        MemberOf = x.MemberOf,
                        SAMAccountName = x.SAMAccountName,
                        UserPrincipalName = x.UserPrincipalName,
                    })
                    .ToList();

                return ApiResponseHelper.Success(result);
            }
        }
    }
}
