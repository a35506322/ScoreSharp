namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyNoteByApplyNo;

[ExampleAnnotation(Name = "[2000]取得徵審行員備註資料", ExampleType = ExampleType.Response)]
public class 取得徵審行員備註資料_2000_ResEx : IExampleProvider<ResultResponse<List<GetApplyNoteByApplyNoResponse>>>
{
    public ResultResponse<List<GetApplyNoteByApplyNoResponse>> GetExample()
    {
        string jsonString =
            "{ \"returnCodeStatus\": 2000, \"returnMessage\": \"\", \"returnData\": [ { \"seqNo\": 1, \"applyNo\": \"20241128M4480\", \"type\": 1, \"typeName\": \"正卡人\", \"id\": \"A110035356\", \"note\": \"\", \"updateUserId\": \"SYSTEM\", \"updateTime\": \"2024-11-28T11:20:08.05\" } ] }";

        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<List<GetApplyNoteByApplyNoResponse>>>(jsonString);
        return data;
    }
}
