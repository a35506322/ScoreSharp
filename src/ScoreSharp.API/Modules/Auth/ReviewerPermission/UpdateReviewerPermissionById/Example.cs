namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.UpdateReviewerPermissionById;

[ExampleAnnotation(Name = "[2000]更新單筆徵審權限", ExampleType = ExampleType.Request)]
public class 更新單筆徵審權限_2000_ReqEx : IExampleProvider<UpdateReviewerPermissionByIdRequest>
{
    public UpdateReviewerPermissionByIdRequest GetExample()
    {
        UpdateReviewerPermissionByIdRequest request = new()
        {
            SeqNo = 123,
            CardStep = CardStep.月收入確認,
            MonthlyIncome_IsShowChangeCaseType = "N",
            MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = "N",
            MonthlyIncome_IsShowInPermission = "N",
            MonthlyIncome_IsShowMonthlyIncome = "N",
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
            IsShowCommunicationNotes = "Y",
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
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]更新單筆徵審權限", ExampleType = ExampleType.Response)]
public class 更新單筆徵審權限_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.Success("30010");
    }
}

[ExampleAnnotation(Name = "[4001]更新單筆徵審權限-查無此ID", ExampleType = ExampleType.Response)]
public class 更新單筆徵審權限查無此ID_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "30011");
    }
}

[ExampleAnnotation(Name = "[4003]更新單筆徵審權限-呼叫有誤", ExampleType = ExampleType.Response)]
public class 更新單筆徵審權限呼叫有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
