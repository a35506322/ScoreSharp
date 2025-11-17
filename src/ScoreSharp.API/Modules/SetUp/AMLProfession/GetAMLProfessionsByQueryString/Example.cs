namespace ScoreSharp.API.Modules.SetUp.AMLProfession.GetAMLProfessionsByQueryString;

[ExampleAnnotation(Name = "[2000]取得AML職業別", ExampleType = ExampleType.Response)]
public class 取得AML職業別_2000_ResEx : IExampleProvider<ResultResponse<List<GetAMLProfessionsByQueryStringResponse>>>
{
    public ResultResponse<List<GetAMLProfessionsByQueryStringResponse>> GetExample()
    {
        var data = new List<GetAMLProfessionsByQueryStringResponse>
        {
            new GetAMLProfessionsByQueryStringResponse
            {
                SeqNo = "01J57D7NVXFHHD4QSZQDABTH3A",
                AMLProfessionCode = "20",
                Version = "20240101",
                AMLProfessionName = "金融/保險/當鋪/電子支付機構/第三方支付/非金融機構之外幣收匯兌業",
                AMLProfessionCompareResult = "Y",
                IsActive = "Y",
                AddTime = DateTime.Now,
                AddUserId = "ADMIN",
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
