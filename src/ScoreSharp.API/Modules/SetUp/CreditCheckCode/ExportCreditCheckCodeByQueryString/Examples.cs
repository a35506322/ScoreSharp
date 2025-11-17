namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.ExportCreditCheckCodeByQueryString;

[ExampleAnnotation(Name = "[2000]匯出徵信代碼Excel", ExampleType = ExampleType.Response)]
public class 匯出徵信代碼Excel_2000_ResEx : IExampleProvider<ResultResponse<ExportCreditCheckCodeByQueryStringResponse>>
{
    public ResultResponse<ExportCreditCheckCodeByQueryStringResponse> GetExample()
    {
        return ApiResponseHelper.Success<ExportCreditCheckCodeByQueryStringResponse>(
            new ExportCreditCheckCodeByQueryStringResponse
            {
                FileName = "徵信代碼.xlsx",
                FileContent = Encoding.UTF8.GetBytes("徵信代碼,徵信代碼名稱,是否啟用\n01,地址證明,Y\n02,收入證明,Y\n03,其他,N"),
            }
        );
    }
}
