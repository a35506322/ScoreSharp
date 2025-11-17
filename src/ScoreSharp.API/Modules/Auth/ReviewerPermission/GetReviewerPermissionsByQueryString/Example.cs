namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.GetReviewerPermissionsByQueryString;

[ExampleAnnotation(Name = "[2000]查詢多筆申請書權限", ExampleType = ExampleType.Response)]
public class 查詢多筆申請書權限_2000_ResEx : IExampleProvider<ResultResponse<List<GetReviewerPermissionsByQueryStringResponse>>>
{
    public ResultResponse<List<GetReviewerPermissionsByQueryStringResponse>> GetExample()
    {
        List<GetReviewerPermissionsByQueryStringResponse> responses = new()
        {
            new GetReviewerPermissionsByQueryStringResponse
            {
                SeqNo = 4,
                CardStatus = CardStatus.網路件_待月收入預審,
                CardStatusName = CardStatus.網路件_待月收入預審.ToString(),
                MonthlyIncome_IsShowChangeCaseType = "N",
                MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = "N",
                MonthlyIncome_IsShowInPermission = "N",
                MonthlyIncome_IsShowMonthlyIncome = "N",
                IsShowNameCheck = "N",
                IsShowUpdatePrimaryInfo = "N",
                IsShowQueryBranchInfo = "Y",
                IsShowQuery929 = "Y",
                IsShowInsertFileAttachment = "Y",
                IsShowUpdateApplyNote = "Y",
                IsCurrentHandleUserId = "N",
                InsertReviewerSummary = "Y",
                IsShowFocus1 = "N",
                IsShowFocus2 = "N",
                IsShowWebMobileRequery = "N",
                IsShowWebEmailRequery = "N",
                IsShowUpdateReviewerSummary = "Y",
                IsShowDeleteReviewerSummary = "Y",
                IsShowDeleteApplyFileAttachment = "Y",
                IsShowCommunicationNotes = "N",
                CardStep = null,
                ManualReview_IsShowInPermission = "N",
                ManualReview_IsShowOutPermission = "N",
                ManualReview_IsShowReturnReview = "N",
                ManualReview_IsShowChangeCaseType = "N",

                IsShowUpdateSameIPCheckRecord = "Y",
                IsShowUpdateWebEmailCheckRecord = "Y",
                IsShowUpdateWebMobileCheckRecord = "Y",
                IsShowUpdateInternalIPCheckRecord = "Y",
                IsShowUpdateShortTimeIDCheckRecord = "Y",
                IsShowInternalEmail = "N",
                IsShowInternalMobile = "N",
                IsShowUpdateInternalEmailCheckRecord = "Y",
                IsShowUpdateInternalMobileCheckRecord = "Y",
                IsShowUpdateSupplementaryInfo = "N",
                IsShowKYCSync = "N",
            },
            new GetReviewerPermissionsByQueryStringResponse
            {
                SeqNo = 2,
                CardStatus = CardStatus.補件_等待完成本案徵審,
                CardStatusName = CardStatus.補件_等待完成本案徵審.ToString(),
                MonthlyIncome_IsShowChangeCaseType = "N",
                MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = "N",
                MonthlyIncome_IsShowInPermission = "N",
                MonthlyIncome_IsShowMonthlyIncome = "Y",
                IsShowNameCheck = "Y",
                IsShowUpdatePrimaryInfo = "Y",
                IsShowQueryBranchInfo = "Y",
                IsShowQuery929 = "Y",
                IsShowInsertFileAttachment = "Y",
                IsShowUpdateApplyNote = "Y",
                IsCurrentHandleUserId = "Y",
                InsertReviewerSummary = "Y",
                IsShowFocus1 = "N",
                IsShowFocus2 = "N",
                IsShowWebMobileRequery = "N",
                IsShowWebEmailRequery = "N",
                IsShowUpdateReviewerSummary = "Y",
                IsShowDeleteReviewerSummary = "Y",
                IsShowDeleteApplyFileAttachment = "Y",
                IsShowCommunicationNotes = "N",
                CardStep = null,
                ManualReview_IsShowInPermission = "N",
                ManualReview_IsShowOutPermission = "N",
                ManualReview_IsShowReturnReview = "N",
                ManualReview_IsShowChangeCaseType = "N",

                IsShowUpdateSameIPCheckRecord = "Y",
                IsShowUpdateWebEmailCheckRecord = "Y",
                IsShowUpdateWebMobileCheckRecord = "Y",
                IsShowUpdateInternalIPCheckRecord = "Y",
                IsShowUpdateShortTimeIDCheckRecord = "Y",
                IsShowInternalEmail = "N",
                IsShowInternalMobile = "N",
                IsShowUpdateInternalEmailCheckRecord = "Y",
                IsShowUpdateInternalMobileCheckRecord = "Y",
                IsShowUpdateSupplementaryInfo = "N",
                IsShowKYCSync = "N",
            },
        };
        return ApiResponseHelper.Success(responses);
    }
}
