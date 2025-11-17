namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.GetApplyPermissionById;

[ExampleAnnotation(Name = "[2000]查詢單筆申請書權限-成功查詢_唯讀", ExampleType = ExampleType.Response)]
public class 查詢單筆申請書權限_成功查詢_唯讀_2000_ResEx : IExampleProvider<ResultResponse<GetApplyPermissionByIdResponse>>
{
    public ResultResponse<GetApplyPermissionByIdResponse> GetExample()
    {
        GetApplyPermissionByIdResponse response = new()
        {
            MonthlyIncome_IsShowChangeCaseType = "N",
            MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = "N",
            MonthlyIncome_IsShowInPermission = "N",
            MonthlyIncome_IsShowMonthlyIncome = "N",
            IsShowNameCheck = "N",
            IsShowUpdatePrimaryInfo = "N",
            IsShowQueryBranchInfo = "N",
            IsShowQuery929 = "N",
            IsShowInsertFileAttachment = "N",
            IsShowUpdateApplyNote = "N",

            InsertReviewerSummary = "N",
            IsShowFocus1 = "N",
            IsShowFocus2 = "N",
            IsShowWebMobileRequery = "N",
            IsShowWebEmailRequery = "N",
            IsShowUpdateReviewerSummary = "N",
            IsShowDeleteReviewerSummary = "N",
            IsShowDeleteApplyFileAttachment = "N",
            IsShowCommunicationNotes = "N",

            ManualReview_IsShowOutPermission = "N",
            ManualReview_IsShowChangeCaseType = "N",
            ManualReview_IsShowInPermission = "N",
            ManualReview_IsShowReturnReview = "N",

            IsShowUpdateSameIPCheckRecord = "N",
            IsShowUpdateWebEmailCheckRecord = "N",
            IsShowUpdateWebMobileCheckRecord = "N",
            IsShowUpdateInternalIPCheckRecord = "N",
            IsShowUpdateShortTimeIDCheckRecord = "N",

            IsShowInternalEmail = "N",
            IsShowInternalMobile = "N",
            IsShowUpdateInternalEmailCheckRecord = "N",
            IsShowUpdateInternalMobileCheckRecord = "N",
            IsShowUpdateSupplementaryInfo = "N",
            IsShowKYCSync = "N",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[2000]查詢單筆申請書權限-成功查詢_登入角色為當前經辦本人", ExampleType = ExampleType.Response)]
public class 查詢單筆申請書權限_成功查詢_登入角色為當前經辦本人_2000_ResEx : IExampleProvider<ResultResponse<GetApplyPermissionByIdResponse>>
{
    public ResultResponse<GetApplyPermissionByIdResponse> GetExample()
    {
        GetApplyPermissionByIdResponse response = new()
        {
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

            InsertReviewerSummary = "Y",
            IsShowFocus1 = "Y",
            IsShowFocus2 = "Y",
            IsShowWebMobileRequery = "Y",
            IsShowWebEmailRequery = "Y",
            IsShowUpdateReviewerSummary = "Y",
            IsShowDeleteReviewerSummary = "Y",
            IsShowDeleteApplyFileAttachment = "Y",
            IsShowCommunicationNotes = "Y",

            ManualReview_IsShowOutPermission = "N",
            ManualReview_IsShowChangeCaseType = "N",
            ManualReview_IsShowInPermission = "N",
            ManualReview_IsShowReturnReview = "N",

            IsShowUpdateSameIPCheckRecord = "Y",
            IsShowUpdateWebEmailCheckRecord = "Y",
            IsShowUpdateWebMobileCheckRecord = "Y",
            IsShowUpdateInternalIPCheckRecord = "Y",
            IsShowUpdateShortTimeIDCheckRecord = "Y",

            IsShowInternalEmail = "Y",
            IsShowInternalMobile = "Y",
            IsShowUpdateInternalEmailCheckRecord = "Y",
            IsShowUpdateInternalMobileCheckRecord = "Y",
            IsShowUpdateSupplementaryInfo = "Y",
            IsShowKYCSync = "N",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]查詢單筆申請書權限-查無此ID", ExampleType = ExampleType.Response)]
public class 查詢單筆申請書權限查無此ID_4001_ResEx : IExampleProvider<ResultResponse<GetApplyPermissionByIdResponse>>
{
    public ResultResponse<GetApplyPermissionByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetApplyPermissionByIdResponse>(null, "20250101X0001");
    }
}
