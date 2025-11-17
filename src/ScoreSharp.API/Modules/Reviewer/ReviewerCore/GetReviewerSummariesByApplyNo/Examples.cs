namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetReviewerSummariesByApplyNo;

[ExampleAnnotation(Name = "[2000]取得徵審照會摘要", ExampleType = ExampleType.Response)]
public class 取得徵審照會摘要_2000_ResEx : IExampleProvider<ResultResponse<List<GetReviewerSummariesByApplyNoResponse>>>
{
    public ResultResponse<List<GetReviewerSummariesByApplyNoResponse>> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 2000,\"returnMessage\": \"\",\"returnData\": [{\"seqNo\": 2,\"applyNo\": \"20241126F5500\",\"record\": \"信用分數偏低，建議提供額外財力證明以利後續審核\",\"addUserId\": \"tinalien\",\"addTime\": \"2024-11-26T10:43:13.157\",\"updateUserId\": null,\"updateTime\": null},{\"seqNo\": 3,\"applyNo\": \"20241126F5500\",\"record\": \"因聯絡資訊無法確認，請補正電話或電郵地址。\",\"addUserId\": \"tinalien\",\"addTime\": \"2024-11-26T10:44:28.947\",\"updateUserId\": null,\"updateTime\": null},{\"seqNo\": 4,\"applyNo\": \"20241126F5500\",\"record\": \"身份核實未通過，建議要求重新提供證件影本\",\"addUserId\": \"tinalien\",\"addTime\": \"2024-11-26T10:47:45.06\",\"updateUserId\": null,\"updateTime\": null},{\"seqNo\": 5,\"applyNo\": \"20241126F5500\",\"record\": \"客戶聲明部分資料有誤，建議更新資料後重新提交審核\",\"addUserId\": \"tinalien\",\"addTime\": \"2024-11-26T10:48:49.48\",\"updateUserId\": null,\"updateTime\": null},{\"seqNo\": 6,\"applyNo\": \"20241126F5500\",\"record\": \"申請文件內容不完整，缺少收入證明，請盡快補件。\",\"addUserId\": \"tinalien\",\"addTime\": \"2024-11-26T10:52:38.857\",\"updateUserId\": null,\"updateTime\": null},{\"seqNo\": 7,\"applyNo\": \"20241126F5500\",\"record\": \"客戶近期信用查詢次數過高，可能存在潛在風險，需深入核查。\",\"addUserId\": \"tinalien\",\"addTime\": \"2024-11-26T10:53:29.573\",\"updateUserId\": null,\"updateTime\": null}]}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<List<GetReviewerSummariesByApplyNoResponse>>>(jsonString);
        return data;
    }
}


//[ExampleAnnotation(Name = "[4001]取得徵審照會摘要-查無此申請書編號", ExampleType = ExampleType.Response)]
//public class 取得徵審照會摘要查無此申請書編號_4001_ResEx : IExampleProvider<ResultResponse<GetReviewerSummariesResponse>>
//{
//    public ResultResponse<GetReviewerSummariesResponse> GetExample()
//    {
//        return ApiResponseHelper.NotFound<GetReviewerSummariesResponse>(null, "20240803B0001");
//    }
//}
