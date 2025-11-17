using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreSharp.API.Modules.OrgSetUp.User.ChangeNotADMimaById;

[ExampleAnnotation(Name = "[2000]修改非AD User密碼", ExampleType = ExampleType.Response)]
public class 修改非ADUser密碼_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("Agent", "Agent");
    }
}

[ExampleAnnotation(Name = "[4001]修改非AD User密碼-查無此資料", ExampleType = ExampleType.Response)]
public class 修改非ADUser密碼查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "Agent");
    }
}

[ExampleAnnotation(Name = "[4003]修改非AD User密碼-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改非ADUser密碼路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[4003]修改非AD User密碼-AD 帳號無法修改密碼", ExampleType = ExampleType.Response)]
public class 修改非ADUser密碼AD帳號無法修改密碼_4004_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "AD 帳號無法修改密碼");
    }
}
