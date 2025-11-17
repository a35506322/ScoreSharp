using ScoreSharp.API.Modules.Manage.Common.Helpers;
using ScoreSharp.API.Modules.Manage.ObligedAssignment.UpdateObligedAssignment;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.ObligedAssignment
{
    public partial class ObligedAssignmentController
    {
        /// <summary>
        /// 單筆強制派案
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// 說明 :
        ///
        ///     指派徵信人員 (AssignToUserId)
        ///
        ///         - 可從 /ManageCommon/GetAssignmentUsers 取得
        ///         - 此人員可以過濾請假人員以及只能為同組人員(前端自行判斷)
        ///
        ///     人員指派變更狀態 (AssignmentChangeStatus)
        ///         - 可從 /ManageCommon/GetManageCommonAllOptions?IsActive=Y 的 assignmentChangeStatus 取得
        ///         - 從 卡片狀態 到 對應的 人員指派變更狀態(前端自行判斷)
        ///
        ///     Request AssignCaseList 請務必只傳送要更改的圖片狀態
        ///
        /// </remarks>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(單筆強制派案_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(單筆強制派案_2000_ResEx),
            typeof(單筆強制派案指派人員不符合指派規則_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateObligedAssignment")]
        public async Task<IResult> UpdateObligedAssignment([FromBody] UpdateObligedAssignmentRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.ObligedAssignment.UpdateObligedAssignment
{
    public record Command(UpdateObligedAssignmentRequest updateObligedAssignmentRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext _context, IJWTProfilerHelper _jwthelper, IManageHelper _manageHelper, IReviewerHelper _reviewerHelper)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var req = request.updateObligedAssignmentRequest;
            var currentUserId = _jwthelper.UserId;
            var currentUserName = _jwthelper.UserName;
            var applyNo = req.ApplyNo;

            var baseData = await GetGroupedBaseDataAsync(applyNo);
            if (baseData is null)
            {
                return ApiResponseHelper.NotFound<string>(null, applyNo);
            }

            var assignedToUserId = req.AssignToUserId;
            var assignedUserInfo = await _context.OrgSetUp_User.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == assignedToUserId);
            var assignedToUserName = assignedUserInfo?.UserName;

            var stakeholders = await _context.Reviewer_Stakeholder.AsNoTracking().Where(x => x.IsActive == "Y").ToListAsync();
            var stakeholderLookup = stakeholders.ToLookup(x => x.ID, StringComparer.OrdinalIgnoreCase);
            var m_ID = baseData.M_ID;
            var s1_ID = baseData.S1_ID;
            var idSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { m_ID };
            if (!string.IsNullOrEmpty(s1_ID))
                idSet.Add(s1_ID);

            var promotionUserId = baseData?.PromotionUser;
            var monthlyIncomeCheckUserIdList = baseData.ApplyCardList.Select(x => x.MonthlyIncomeCheckUserId).ToHashSet();

            if (assignedToUserId is not null)
            {
                var checkDto = new CheckCaseCanAssignDto
                {
                    AssignedUserId = assignedToUserId,
                    IdSet = idSet,
                    MonthlyIncomeCheckUserId = monthlyIncomeCheckUserIdList,
                    PromotionUserId = promotionUserId,
                    StakeholderLookup = stakeholderLookup,
                    AssignedEmployeeNo = assignedUserInfo?.EmployeeNo ?? null,
                };

                var (isSuccess, errorMessage) = ValidateAssignUser(checkDto);
                if (!isSuccess)
                {
                    return ApiResponseHelper.BusinessLogicFailed<string>(null, errorMessage);
                }
            }

            var handles = await _context.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == applyNo).ToListAsync();
            var main = await _context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            // change CardStatus
            foreach (var item in req.AssignCaseList)
            {
                var handle = handles.Single(x => x.SeqNo == item.SeqNo);
                handle.CardStatus = (CardStatus)item.AssignmentChangeStatus;
            }
            if (assignedToUserId is not null)
            {
                main.CurrentHandleUserId = assignedToUserId;
            }

            var processNote = assignedToUserId is null ? "【強制派案】" : $"【強制派案】{currentUserName} 指派TO：{assignedToUserName}";
            var process = MapToProcess(applyNo, (CardStatus)req.AssignCaseList.FirstOrDefault().AssignmentChangeStatus, processNote, currentUserId);

            // 派案統計
            Reviewer_CaseStatistics? caseStatistics = null;

            // 派案統計
            if (!string.IsNullOrEmpty(assignedToUserId))
            {
                caseStatistics = new Reviewer_CaseStatistics
                {
                    UserId = assignedToUserId,
                    CaseType = CaseStatisticType.強制派案,
                    ApplyNo = applyNo,
                };
            }

            await _context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(process);
            if (caseStatistics is not null)
            {
                await _context.Reviewer_CaseStatistics.AddAsync(caseStatistics);
            }
            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(null, applyNo);
        }

        private Reviewer_ApplyCreditCardInfoProcess MapToProcess(string applyNo, CardStatus afterCardStatus, string note, string userId)
        {
            return new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = applyNo,
                Process = afterCardStatus.ToString(),
                Notes = note,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                ProcessUserId = userId,
            };
        }

        private (bool isSuccess, string errorMssage) ValidateAssignUser(CheckCaseCanAssignDto checkDto)
        {
            var assignedToUserId = checkDto.AssignedUserId;
            var promotionUserId = checkDto.PromotionUserId;
            var monthlyIncomeCheckUserIdList = checkDto.MonthlyIncomeCheckUserId;
            var idSet = checkDto.IdSet;
            var stakeholderLookup = checkDto.StakeholderLookup;
            var assignedEmployeeNo = checkDto.AssignedEmployeeNo;

            List<string> errorMessages = [];

            // 1. 利害關係人
            var stakeholderCheck = _manageHelper.檢核利害關係人(
                stakeholderLookup: stakeholderLookup,
                idSet: idSet,
                assignedUserId: assignedToUserId,
                employeeNo: assignedEmployeeNo
            );
            if (stakeholderCheck)
            {
                errorMessages.Add($"被指派人員：{assignedToUserId}為利害關係人");
            }

            // 2. 與推廣人員是否相同
            var haveToCheckPromotionUserId = !string.IsNullOrEmpty(promotionUserId);
            if (haveToCheckPromotionUserId)
            {
                var promotionUserCheck = _manageHelper.與推廣人員是否相同(
                    promotionUserId: promotionUserId!,
                    assignedUserId: assignedToUserId,
                    employeeNo: assignedEmployeeNo
                );
                if (promotionUserCheck)
                {
                    errorMessages.Add($"被指派人員：{assignedToUserId}與推廣人員相同");
                }
            }

            // 3. 與月收入確認人員是否相同
            var haveToCheckMonthlyIncomeUserId =
                monthlyIncomeCheckUserIdList is not null && monthlyIncomeCheckUserIdList.Any(x => !string.IsNullOrEmpty(x));
            if (haveToCheckMonthlyIncomeUserId)
            {
                var monthlyIncomeUserCheck = _manageHelper.與月收入確認人員是否相同(
                    monthlyIncomeCheckUserId: monthlyIncomeCheckUserIdList!,
                    assignedUserId: assignedToUserId
                );
                if (monthlyIncomeUserCheck)
                {
                    errorMessages.Add($"被指派人員：{assignedToUserId}與月收入確認人員相同");
                }
            }

            if (errorMessages.Any())
            {
                return (false, string.Join("、", errorMessages));
            }

            return (true, string.Empty);
        }

        private async Task<GetApplyCreditCardBaseDataResult?> GetGroupedBaseDataAsync(string applyNo)
        {
            var baseData = await _reviewerHelper.GetApplyCreditCardBaseData(new GetApplyCreditCardBaseDataDto { ApplyNo = applyNo });
            return baseData.FirstOrDefault();
        }
    }
}
