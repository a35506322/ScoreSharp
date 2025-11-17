namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.InsertReviewerPermission;

[ExampleAnnotation(Name = "[2000]新增單筆徵審權限", ExampleType = ExampleType.Request)]
public class 新增單筆徵審權限_2000_ReqEx : IExampleProvider<InsertReviewerPermissionRequest>
{
    public InsertReviewerPermissionRequest GetExample()
    {
        InsertReviewerPermissionRequest request = new()
        {
            CardStatus = CardStatus.退件作業中_終止狀態,
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
            IsCurrentHandleUserId = "N",
            InsertReviewerSummary = "Y",
            IsShowFocus1 = "Y",
            IsShowFocus2 = "Y",
            IsShowWebMobileRequery = "Y",
            IsShowWebEmailRequery = "Y",
            IsShowUpdateReviewerSummary = "Y",
            IsShowDeleteReviewerSummary = "Y",
            IsShowDeleteApplyFileAttachment = "Y",
            IsShowCommunicationNotes = "Y",
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
            IsShowInternalEmail = "Y",
            IsShowInternalMobile = "Y",
            IsShowUpdateInternalEmailCheckRecord = "Y",
            IsShowUpdateInternalMobileCheckRecord = "Y",
            IsShowUpdateSupplementaryInfo = "N",
            IsShowKYCSync = "N",
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增單筆徵審權限", ExampleType = ExampleType.Response)]
public class 新增單筆徵審權限_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess(((int)CardStatus.退件作業中_終止狀態).ToString(), ((int)CardStatus.退件作業中_終止狀態).ToString());
    }
}

[ExampleAnnotation(Name = "[4002]新增單筆徵審權限-資料已存在", ExampleType = ExampleType.Response)]
public class 新增單筆徵審權限資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, ((int)CardStatus.網路件_待月收入預審).ToString());
    }
}
