using ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.ExecWebRetryCaseBySeqNo.Models;

namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.ExecWebRetryCaseBySeqNo;

[ExampleAnnotation(Name = "[2000]查詢網路件重試-匯入成功", ExampleType = ExampleType.Response)]
public class 查詢網路件重試匯入成功_2000_ResEx : IExampleProvider<ResultResponse<ExecWebRetryCaseBySeqNoResponse>>
{
    public ResultResponse<ExecWebRetryCaseBySeqNoResponse> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess(
            new ExecWebRetryCaseBySeqNoResponse
            {
                SeqNo = 1000,
                ReturnCode = 回覆代碼.匯入成功,
                ReturnCodeMessage = nameof(回覆代碼.匯入成功),
            },
            "1000"
        );
    }
}

[ExampleAnnotation(Name = "[2000]查詢網路件重試-申請書異常", ExampleType = ExampleType.Response)]
public class 查詢網路件重試申請書異常_2000_ResEx : IExampleProvider<ResultResponse<ExecWebRetryCaseBySeqNoResponse>>
{
    public ResultResponse<ExecWebRetryCaseBySeqNoResponse> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess(
            new ExecWebRetryCaseBySeqNoResponse
            {
                SeqNo = 1000,
                ReturnCode = 回覆代碼.申請書異常,
                ReturnCodeMessage = nameof(回覆代碼.申請書異常),
            },
            "1000"
        );
    }
}

[ExampleAnnotation(Name = "[2000]查詢網路件重試-附件異常", ExampleType = ExampleType.Response)]
public class 查詢網路件重試附件異常_2000_ResEx : IExampleProvider<ResultResponse<ExecWebRetryCaseBySeqNoResponse>>
{
    public ResultResponse<ExecWebRetryCaseBySeqNoResponse> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess(
            new ExecWebRetryCaseBySeqNoResponse
            {
                SeqNo = 1000,
                ReturnCode = 回覆代碼.附件異常,
                ReturnCodeMessage = nameof(回覆代碼.附件異常),
            },
            "1000"
        );
    }
}

[ExampleAnnotation(Name = "[4001]查詢網路件重試-查無資料", ExampleType = ExampleType.Response)]
public class 查詢網路件重試查無資料_4001_ResEx : IExampleProvider<ResultResponse<ExecWebRetryCaseBySeqNoResponse>>
{
    public ResultResponse<ExecWebRetryCaseBySeqNoResponse> GetExample()
    {
        return ApiResponseHelper.NotFound(new ExecWebRetryCaseBySeqNoResponse { SeqNo = 10001 }, "10001");
    }
}

[ExampleAnnotation(Name = "[4003]查詢網路件重試-案件已重新寄送", ExampleType = ExampleType.Response)]
public class 查詢網路件重試案件已重新寄送_4003_ResEx : IExampleProvider<ResultResponse<ExecWebRetryCaseBySeqNoResponse>>
{
    public ResultResponse<ExecWebRetryCaseBySeqNoResponse> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed(
            new ExecWebRetryCaseBySeqNoResponse { SeqNo = 10000 },
            "此案件已經重新寄送，請確認後再試。"
        );
    }
}

[ExampleAnnotation(Name = "[5001]查詢網路件重試-Request必要欄位不能為空值", ExampleType = ExampleType.Response)]
public class 查詢網路件重試案件Request必要欄位不能為空值_5001_ResEx : IExampleProvider<ResultResponse<ExecWebRetryCaseBySeqNoResponse>>
{
    public ResultResponse<ExecWebRetryCaseBySeqNoResponse> GetExample()
    {
        return ApiResponseHelper.UpdateByIdError(
            new ExecWebRetryCaseBySeqNoResponse
            {
                SeqNo = 10000,
                ReturnCode = 回覆代碼.必要欄位不能為空值,
                ReturnCodeMessage = nameof(回覆代碼.必要欄位不能為空值),
            },
            "10000"
        );
    }
}

[ExampleAnnotation(Name = "[5001]查詢網路件重試-Request資料異常非定義值", ExampleType = ExampleType.Response)]
public class 查詢網路件重試案件Request資料異常非定義值_5001_ResEx : IExampleProvider<ResultResponse<ExecWebRetryCaseBySeqNoResponse>>
{
    public ResultResponse<ExecWebRetryCaseBySeqNoResponse> GetExample()
    {
        return ApiResponseHelper.UpdateByIdError(
            new ExecWebRetryCaseBySeqNoResponse
            {
                SeqNo = 10000,
                ReturnCode = 回覆代碼.資料異常非定義值,
                ReturnCodeMessage = nameof(回覆代碼.資料異常非定義值),
            },
            "10000"
        );
    }
}

[ExampleAnnotation(Name = "[5001]查詢網路件重試-Request資料異常資料長度過長", ExampleType = ExampleType.Response)]
public class 查詢網路件重試案件Request資料異常資料長度過長_5001_ResEx : IExampleProvider<ResultResponse<ExecWebRetryCaseBySeqNoResponse>>
{
    public ResultResponse<ExecWebRetryCaseBySeqNoResponse> GetExample()
    {
        return ApiResponseHelper.UpdateByIdError(
            new ExecWebRetryCaseBySeqNoResponse
            {
                SeqNo = 10000,
                ReturnCode = 回覆代碼.資料異常資料長度過長,
                ReturnCodeMessage = nameof(回覆代碼.資料異常資料長度過長),
            },
            "10000"
        );
    }
}

[ExampleAnnotation(Name = "[5001]查詢網路件重試-申請書編號重複進件或申請書編號不對", ExampleType = ExampleType.Response)]
public class 查詢網路件重試案件申請書編號重複進件或申請書編號不對_5001_ResEx
    : IExampleProvider<ResultResponse<ExecWebRetryCaseBySeqNoResponse>>
{
    public ResultResponse<ExecWebRetryCaseBySeqNoResponse> GetExample()
    {
        return ApiResponseHelper.UpdateByIdError(
            new ExecWebRetryCaseBySeqNoResponse
            {
                SeqNo = 10000,
                ReturnCode = 回覆代碼.申請書編號重複進件或申請書編號不對,
                ReturnCodeMessage = nameof(回覆代碼.申請書編號重複進件或申請書編號不對),
            },
            "10000"
        );
    }
}
