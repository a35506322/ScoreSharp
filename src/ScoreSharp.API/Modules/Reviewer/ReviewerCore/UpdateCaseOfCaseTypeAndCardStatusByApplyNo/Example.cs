using ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateCommunicationNotesByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateCaseOfCaseTypeAndCardStatusByApplyNo;

[ExampleAnnotation(Name = "[2000]更改案件種類以及狀態-狀態轉成補回件", ExampleType = ExampleType.Request)]
public class 更改案件種類以及狀態成功_狀態轉成補回件_2000_ReqEx : IExampleProvider<UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest>
{
    public UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest GetExample() =>
        new() { ApplyNo = "20250321G7943", CaseOfAction = CaseOfAction.狀態_補回件 };
}

[ExampleAnnotation(Name = "[4003]更改案件種類以及狀態-狀態轉成補回件-查無補件作業中之案件", ExampleType = ExampleType.Response)]
public class 更改案件種類以及狀態_狀態轉成補回件_查無補件作業中之案件_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.BusinessLogicFailed<string>(null, "查無補件作業中之案件");
}

[ExampleAnnotation(Name = "[4003]更改案件種類以及狀態-狀態轉成補回件-成功", ExampleType = ExampleType.Response)]
public class 更改案件種類以及狀態_狀態轉成補回件_成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess<string>("20250321G7943", "20250321G7943");
}

[ExampleAnnotation(Name = "[4003]更改案件種類以及狀態-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 更改案件種類以及狀態路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.路由與Req比對錯誤<string>("2");
}

[ExampleAnnotation(Name = "[2000]更改案件種類以及狀態-變更案件種類變更為一般件-成功", ExampleType = ExampleType.Request)]
public class 更改案件種類以及狀態_變更案件種類變更為一般件_成功_2000_ReqEx
    : IExampleProvider<UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest>
{
    public UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest GetExample() =>
        new() { ApplyNo = "20250321G7943", CaseOfAction = CaseOfAction.案件種類_一般件 };
}

[ExampleAnnotation(Name = "[2000]更改案件種類以及狀態-變更案件種類變更為一般件-成功", ExampleType = ExampleType.Response)]
public class 更改案件種類以及狀態_變更案件種類變更為一般件_成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess<string>("20250321G7943", "20250321G7943");
}

[ExampleAnnotation(Name = "[2000]更改案件種類以及狀態-變更案件種類變更為急件-成功", ExampleType = ExampleType.Request)]
public class 更改案件種類以及狀態_變更案件種類變更為急件_成功_2000_ReqEx
    : IExampleProvider<UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest>
{
    public UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest GetExample() =>
        new() { ApplyNo = "20250321G7943", CaseOfAction = CaseOfAction.案件種類_一般件 };
}

[ExampleAnnotation(Name = "[2000]更改案件種類以及狀態-變更案件種類變更為急件-成功", ExampleType = ExampleType.Response)]
public class 更改案件種類以及狀態_變更案件種類變更為急件_成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess<string>("20250321G7943", "20250321G7943");
}

[ExampleAnnotation(Name = "[2000]更改案件種類以及狀態-變更案件種類變更為緊急製卡-成功", ExampleType = ExampleType.Request)]
public class 更改案件種類以及狀態_變更案件種類變更為緊急製卡_成功_2000_ReqEx
    : IExampleProvider<UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest>
{
    public UpdateCaseOfCaseTypeAndCardStatusByApplyNoRequest GetExample() =>
        new() { ApplyNo = "20250321G7943", CaseOfAction = CaseOfAction.案件種類_一般件 };
}

[ExampleAnnotation(Name = "[2000]更改案件種類以及狀態-變更案件種類變更為緊急製卡-成功", ExampleType = ExampleType.Response)]
public class 更改案件種類以及狀態_變更案件種類變更為緊急製卡_成功_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.UpdateByIdSuccess<string>("20250321G7943", "20250321G7943");
}

[ExampleAnnotation(Name = "[4001]更改案件種類以及狀態-變更案件種類-查無資料", ExampleType = ExampleType.Response)]
public class 更改案件種類以及狀態_變更案件種類_查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20250101X0111");
}
