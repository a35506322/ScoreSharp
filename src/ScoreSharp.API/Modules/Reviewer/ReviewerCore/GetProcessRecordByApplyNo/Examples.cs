namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetProcessRecordByApplyNo;

[ExampleAnnotation(Name = "[2000]取得申請紀錄", ExampleType = ExampleType.Response)]
public class 取得申請紀錄_2000_ResEx : IExampleProvider<ResultResponse<GetProcessRecordByApplyNoResponse>>
{
    public ResultResponse<GetProcessRecordByApplyNoResponse> GetExample()
    {
        string jsonString =
            "{ \"returnCodeStatus\": 2000, \"returnMessage\": \"\", \"returnData\": { \"applyCreditCardInfoProcess\": [ { \"seqNo\": 175, \"applyNo\": \"20241127X8342\", \"process\": \"待月收入預審\", \"startTime\": \"2024-11-27T11:24:52.653\", \"endTime\": \"2024-11-27T11:24:52.653\", \"notes\": null, \"processUserId\": \"SYSTEM\" }, { \"seqNo\": 172, \"applyNo\": \"20241127X8342\", \"process\": \"完成分行資訊查詢\", \"startTime\": \"2024-11-27T11:24:52.627\", \"endTime\": \"2024-11-27T11:24:52.64\", \"notes\": null, \"processUserId\": \"SYSTEM\" }, { \"seqNo\": 173, \"applyNo\": \"20241127X8342\", \"process\": \"完成929業務狀況查詢\", \"startTime\": \"2024-11-27T11:24:52.627\", \"endTime\": \"2024-11-27T11:24:52.633\", \"notes\": null, \"processUserId\": \"SYSTEM\" }, { \"seqNo\": 174, \"applyNo\": \"20241127X8342\", \"process\": \"完成告誡名單查詢\", \"startTime\": \"2024-11-27T11:24:52.627\", \"endTime\": \"2024-11-27T11:24:52.647\", \"notes\": null, \"processUserId\": \"SYSTEM\" }, { \"seqNo\": 171, \"applyNo\": \"20241127X8342\", \"process\": \"初始\", \"startTime\": \"2024-11-27T11:24:31.323\", \"endTime\": \"2024-11-27T11:24:31.323\", \"notes\": null, \"processUserId\": \"SYSTEM\" } ], \"applyFileLog\": [ { \"seqNo\": 106, \"applyNo\": \"20241127X8342\", \"process\": \"初始\", \"page\": 1, \"addTime\": \"2024-11-27T11:24:31.323\", \"addUserId\": \"SYSTEM\", \"note\": \"20241127X8342_uploadPDF.pdf\" }, { \"seqNo\": 107, \"applyNo\": \"20241127X8342\", \"process\": \"初始\", \"page\": 2, \"addTime\": \"2024-11-27T11:24:31.323\", \"addUserId\": \"SYSTEM\", \"note\": \"20241127X8342_idPic1.jpg\" }, { \"seqNo\": 108, \"applyNo\": \"20241127X8342\", \"process\": \"初始\", \"page\": 3, \"addTime\": \"2024-11-27T11:24:31.323\", \"addUserId\": \"SYSTEM\", \"note\": \"20241127X8342_idPic2.jpg\" }, { \"seqNo\": 109, \"applyNo\": \"20241127X8342\", \"process\": \"初始\", \"page\": 4, \"addTime\": \"2024-11-27T11:24:31.323\", \"addUserId\": \"SYSTEM\", \"note\": \"20241127X8342_upload1.jpg\" } ] } }";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<GetProcessRecordByApplyNoResponse>>(jsonString);
        return data;
    }
}
