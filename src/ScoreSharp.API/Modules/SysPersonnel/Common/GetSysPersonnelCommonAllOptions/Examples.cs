namespace ScoreSharp.API.Modules.SysPersonnel.Common.GetSysPersonnelCommonAllOptions;

[ExampleAnnotation(Name = "[2000]查詢系統人員作業設定下拉選單", ExampleType = ExampleType.Response)]
public class 查詢系統人員作業設定下拉選單_2000_ResEx : IExampleProvider<ResultResponse<GetSysPersonnelCommonAllOptionsResponse>>
{
    public ResultResponse<GetSysPersonnelCommonAllOptionsResponse> GetExample()
    {
        string jsonString = """

            {
                "returnCodeStatus": 2000,
                "returnMessage": "",
                "returnData": {
                    "caseCheckStatus": [
                        {
                            "name": "需檢核_未完成",
                            "value": 1,
                            "isActive": "Y"
                        },
                        {
                            "name": "需檢核_成功",
                            "value": 2,
                            "isActive": "Y"
                        },
                        {
                            "name": "需檢核_失敗",
                            "value": 3,
                            "isActive": "Y"
                        },
                        {
                            "name": "不需檢核",
                            "value": 4,
                            "isActive": "Y"
                        }
                    ]
                }
            }

            """;

        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<GetSysPersonnelCommonAllOptionsResponse>>(jsonString);
        return data;
    }
}
