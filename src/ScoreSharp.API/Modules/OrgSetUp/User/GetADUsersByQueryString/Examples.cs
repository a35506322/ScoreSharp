namespace ScoreSharp.API.Modules.OrgSetUp.User.GetADUsersByQueryString;

[ExampleAnnotation(Name = "[2000]取得AD Server User", ExampleType = ExampleType.Response)]
public class 取得ADServerUser_2000_ResEx : IExampleProvider<ResultResponse<List<GetADUsersByQueryStringResponse>>>
{
    public ResultResponse<List<GetADUsersByQueryStringResponse>> GetExample()
    {
        List<GetADUsersByQueryStringResponse> response = new List<GetADUsersByQueryStringResponse>()
        {
            new GetADUsersByQueryStringResponse()
            {
                DisplayName = "沉大名",
                MemberOf = { "CN=主機系統部,OU=主機系統部,OU=UITC,DC=uitctech,DC=com,DC=tw" },
                SAMAccountName = "chenbigmin",
                UserPrincipalName = "chenbigmin@uitctech.com.tw",
            },
            new GetADUsersByQueryStringResponse()
            {
                DisplayName = "張小英",
                MemberOf = { "CN=UPCM,OU=系統網路組,OU=資訊系統部,OU=UITC,DC=uitctech,DC=com,DC=tw", "CN=Users,DC=uitctech,DC=com,DC=tw" },
                SAMAccountName = "jhuangsmallen",
                UserPrincipalName = "jhuangsmallen00@uitctech.com.tw",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
