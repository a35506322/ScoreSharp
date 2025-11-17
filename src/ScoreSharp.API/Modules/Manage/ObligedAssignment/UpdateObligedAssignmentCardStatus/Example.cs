namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.UpdateObligedAssignmentCardStatus;

[ExampleAnnotation(Name = "[2000]強制派案狀態權限設定", ExampleType = ExampleType.Request)]
public class 強制派案狀態權限設定_2000_ReqEx : IExampleProvider<UpdateObligedAssignmentCardStatusRequest>
{
    public UpdateObligedAssignmentCardStatusRequest GetExample()
    {
        var req = new UpdateObligedAssignmentCardStatusRequest
        {
            CardStatus = CardStatus.製卡失敗,
            RoleIds = new List<string> { "Admin", "M-WorkManager" },
        };

        return req;
    }
}

[ExampleAnnotation(Name = "[2000]強制派案狀態權限設定", ExampleType = ExampleType.Response)]
public class 強制派案狀態權限設定_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>(null, CardStatus.製卡失敗.ToString());
    }
}

[ExampleAnnotation(Name = "[4001]強制派案狀態權限設定-查無資料", ExampleType = ExampleType.Response)]
public class 強制派案狀態權限設定查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, CardStatus.製卡失敗.ToString());
    }
}
