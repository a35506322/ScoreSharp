namespace ScoreSharp.API.Modules.OrgSetUp.User.GetADUserById;

[ExampleAnnotation(Name = "[2000]取得單筆AD Server User", ExampleType = ExampleType.Response)]
public class 取得單筆ADServerUser_2000_ResEx : IExampleProvider<ResultResponse<GetADUserByIdResponse>>
{
    public ResultResponse<GetADUserByIdResponse> GetExample()
    {
        GetADUserByIdResponse response = new GetADUserByIdResponse()
        {
            DisplayName = "陳曉明",
            MemberOf = { "CN=資訊服務部,OU=資訊服務部,OU=UITC,DC=uitctech,DC=com,DC=tw" },
            SAMAccountName = "chenming",
            UserPrincipalName = "chenbigmin@uitctech.com.tw",
        };

        return ApiResponseHelper.Success(response);
    }
}
