using System.Collections.Generic;

namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.GetUserTakeVacationByQueryString;

[ExampleAnnotation(Name = "[2000]查詢多筆員工休假", ExampleType = ExampleType.Response)]
public class 查詢多筆員工休假_2000_ResEx : IExampleProvider<ResultResponse<List<GetUserTakeVacationByQueryStringResponse>>>
{
    public ResultResponse<List<GetUserTakeVacationByQueryStringResponse>> GetExample()
    {
        List<GetUserTakeVacationByQueryStringResponse> response = new()
        {
            new GetUserTakeVacationByQueryStringResponse
            {
                Date = "2024/12/28",
                SeqNo = 2,
                UserId = "rayyeh",
                UserName = "葉清宏",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                AddUserId = "tinalien",
                AddTime = DateTime.Now,
            },
            new GetUserTakeVacationByQueryStringResponse
            {
                Date = "2024/12/28",
                SeqNo = 11,
                UserId = "abbylin",
                UserName = "林廷芳",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                AddUserId = "",
                AddTime = DateTime.Now,
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
