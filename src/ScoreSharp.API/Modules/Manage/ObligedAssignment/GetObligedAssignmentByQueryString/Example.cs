namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.GetObligedAssignmentByQueryString;

[ExampleAnnotation(Name = "[2000]查詢強制派案", ExampleType = ExampleType.Response)]
public class 查詢強制派案_2000_ResEx : IExampleProvider<ResultResponse<List<GetObligedAssignmentByQueryStringResponse>>>
{
    public ResultResponse<List<GetObligedAssignmentByQueryStringResponse>> GetExample()
    {
        var response = new List<GetObligedAssignmentByQueryStringResponse>()
        {
            new GetObligedAssignmentByQueryStringResponse
            {
                ApplyNo = "20251015B2131",
                ID = "L199517969",
                CHName = "龔燁華",
                CardOwner = CardOwner.正卡,
                ApplyCardList = new List<ApplyCardListDto>()
                {
                    new ApplyCardListDto { HandleSeqNo = "01K7JQHXQ7CE1BM34DETC7T2SW",ID = "A123456789",UserType = UserType.正卡人,ApplyCardType = "VS09", ApplyCardName = "聯邦一卡通吉鶴卡", CardStatus = CardStatus.人工徵信中, CardStep = CardStep.人工徵審 },
                    new ApplyCardListDto { HandleSeqNo = "01K4S3TREG34A9ABBGEXPVQN8W",ID = "B123456789",UserType = UserType.附卡人,ApplyCardType = "VS09", ApplyCardName = "聯邦一卡通吉鶴卡", CardStatus = CardStatus.人工徵信中, CardStep = CardStep.人工徵審 },
                },

                ApproveUserList = new List<ApproveUserDto>()
                {
                    new ApproveUserDto
                    {
                        ApproveUserId = "janehuang",
                        ApproveUserName = "黃佳玲",
                        ApproveTime = new DateTime(2025, 10, 15, 10, 0, 0),
                    },
                },
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
