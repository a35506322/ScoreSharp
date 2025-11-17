namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyFileAttachmentsInfoByApplyNo;

[ExampleAnnotation(Name = "[2000]取得徵審行員附件", ExampleType = ExampleType.Response)]
public class 取得徵審行員附件_2000_ResEx : IExampleProvider<ResultResponse<List<GetApplyFileAttachmentsInfoByApplyNoResponse>>>
{
    public ResultResponse<List<GetApplyFileAttachmentsInfoByApplyNoResponse>> GetExample()
    {
        return new ResultResponse<List<GetApplyFileAttachmentsInfoByApplyNoResponse>>()
        {
            ReturnCodeStatus = ReturnCodeStatus.成功,
            ReturnMessage = "",
            ReturnData = new List<GetApplyFileAttachmentsInfoByApplyNoResponse>()
            {
                new GetApplyFileAttachmentsInfoByApplyNoResponse()
                {
                    IDOptions = new List<IDOption>()
                    {
                        new IDOption()
                        {
                            ID = "A110035356",
                            Name = "連翊妏",
                            UserType = UserType.正卡人,
                            UserTypeName = UserType.正卡人.ToString(),
                        },
                    },
                    AttachmentsInfo = new List<AttachmentsInfo>()
                    {
                        new AttachmentsInfo()
                        {
                            SeqNo = 1,
                            FileName = "test1.jpg",
                            AddTime = DateTime.Now,
                            ID = "A110035356",
                            UserId = "tinalien",
                            UserName = "連翊妏",
                            FileId = Guid.NewGuid(),
                            ApplyNo = "A110035356",
                        },
                    },
                },
            },
        };
    }
}
