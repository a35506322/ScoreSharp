namespace ScoreSharp.API.Modules.Reviewer.Common.GetAddressInfoOptions;

[ExampleAnnotation(Name = "[2000]取得地址相關下拉選單", ExampleType = ExampleType.Response)]
public class 取得地址相關下拉選單_2000_ResEx : IExampleProvider<ResultResponse<GetAddressInfoOptionsResponse>>
{
    public ResultResponse<GetAddressInfoOptionsResponse> GetExample()
    {
        string jsonString =
            @"{
                                  ""returnCodeStatus"": 2000,
                                  ""returnMessage"": """",
                                  ""returnData"": {
                                    ""city"": [
                                      {
                                        ""name"": ""臺北市"",
                                        ""value"": ""臺北市"",
                                        ""isActive"": ""Y""
                                      }
                                    ],
                                    ""area"": [
                                      {
                                        ""city"": ""新北市"",
                                        ""areas"": [
                                          {
                                            ""name"": ""三重區"",
                                            ""value"": ""三重區"",
                                            ""isActive"": ""Y""
                                          }
                                        ]
                                      }
                                    ],
                                    ""road"": [
                                      {
                                        ""city"": ""桃園市"",
                                        ""area"": ""復興區"",
                                        ""roads"": [
                                          {
                                            ""name"": ""三民路一段"",
                                            ""value"": ""三民路一段"",
                                            ""isActive"": ""Y""
                                          },
                                          {
                                            ""name"": ""三民路１段"",
                                            ""value"": ""三民路１段"",
                                            ""isActive"": ""Y""
                                          }
                                        ]
                                      }
                                    ]
                                  }
                                }";

        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<GetAddressInfoOptionsResponse>>(jsonString);
        return data;
    }
}
