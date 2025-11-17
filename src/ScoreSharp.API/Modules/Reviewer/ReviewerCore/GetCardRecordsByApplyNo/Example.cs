namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetCardRecordsByApplyNo;

[ExampleAnnotation(Name = "[2000]取得卡別紀錄", ExampleType = ExampleType.Response)]
public class 取得卡別紀錄_2000_ResEx : IExampleProvider<ResultResponse<List<GetCardRecordsByApplyNoResponse>>>
{
    public ResultResponse<List<GetCardRecordsByApplyNoResponse>> GetExample()
    {
        var response = new List<GetCardRecordsByApplyNoResponse>()
        {
            new GetCardRecordsByApplyNoResponse()
            {
                SeqNo = 1,
                ApplyNo = "20250331B3106",
                CardLimit = null,
                CardStatus = CardStatus.補件_等待完成本案徵審,
                HandleNote = "補件原因:08,09(TO:戶籍地址)(正卡_JS59)",
                AddTime = DateTime.Now,
                ApproveUserId = "arthurlin",
                ApproveUserName = "林芃均",
                HandleSeqNo = "01JQMZDYYDN6VSHY1BF6RT3N55",
            },
            new GetCardRecordsByApplyNoResponse()
            {
                SeqNo = 2,
                ApplyNo = "20250331B3106",
                CardLimit = null,
                CardStatus = CardStatus.補件作業中,
                HandleNote = "完成本案;(正卡_JS59)",
                AddTime = DateTime.Now,
                ApproveUserId = "arthurlin",
                ApproveUserName = "林芃均",
                HandleSeqNo = "01JQMZDYYDN6VSHY1BF6RT3N55",
            },
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得卡別紀錄-查無此ID", ExampleType = ExampleType.Response)]
public class 取得卡別紀錄查無此ID_4001_ResEx : IExampleProvider<ResultResponse<List<GetCardRecordsByApplyNoResponse>>>
{
    public ResultResponse<List<GetCardRecordsByApplyNoResponse>> GetExample()
    {
        return ApiResponseHelper.NotFound<List<GetCardRecordsByApplyNoResponse>>(null, "20241128M4480");
    }
}
