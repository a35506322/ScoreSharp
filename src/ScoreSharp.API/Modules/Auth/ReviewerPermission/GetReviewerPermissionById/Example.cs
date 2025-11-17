namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.GetReviewerPermissionById;

[ExampleAnnotation(Name = "[2000]查詢單筆徵審權限", ExampleType = ExampleType.Response)]
public class 查詢單筆徵審權限_2000_ResEx : IExampleProvider<ResultResponse<GetReviewerPermissionByIdResponse>>
{
    public ResultResponse<GetReviewerPermissionByIdResponse> GetExample()
    {
        GetReviewerPermissionByIdResponse response = new()
        {
            SeqNo = 4,
            CardStatus = CardStatus.網路件_待月收入預審,
            CardStatusName = CardStatus.網路件_待月收入預審.ToString(),
            MonthlyIncome_IsShowChangeCaseType = "Y",
            MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = "Y",
            MonthlyIncome_IsShowInPermission = "Y",
            MonthlyIncome_IsShowMonthlyIncome = "Y",
            IsShowNameCheck = "Y",
            IsShowUpdatePrimaryInfo = "Y",
            IsShowQueryBranchInfo = "Y",
            IsShowQuery929 = "Y",
            IsShowInsertFileAttachment = "Y",
            IsShowUpdateApplyNote = "Y",
            IsCurrentHandleUserId = "Y",
            InsertReviewerSummary = "Y",
            AddUserId = "superadmin",
            AddTime = DateTime.Now,
            IsShowFocus1 = "N",
            IsShowFocus2 = "N",
            IsShowWebMobileRequery = "N",
            IsShowWebEmailRequery = "N",
            IsShowUpdateReviewerSummary = "Y",
            IsShowDeleteReviewerSummary = "Y",
            IsShowDeleteApplyFileAttachment = "Y",
            IsShowCommunicationNotes = "Y",
            CardStep = null,
            ManualReview_IsShowInPermission = "N",
            ManualReview_IsShowOutPermission = "N",
            ManualReview_IsShowReturnReview = "N",
            ManualReview_IsShowChangeCaseType = "N",
            IsShowInternalEmail = "N",
            IsShowInternalMobile = "N",
            IsShowUpdateInternalEmailCheckRecord = "Y",
            IsShowUpdateInternalMobileCheckRecord = "Y",
            IsShowUpdateSupplementaryInfo = "N",
            IsShowKYCSync = "N",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]查詢單筆徵審權限-查無此ID", ExampleType = ExampleType.Response)]
public class 查詢單筆徵審權限查無此ID_4001_ResEx : IExampleProvider<ResultResponse<GetReviewerPermissionByIdResponse>>
{
    public ResultResponse<GetReviewerPermissionByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetReviewerPermissionByIdResponse>(null, "100");
    }
}
