using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer3rd.GetBranchInfoByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer3rd
{
    public partial class Reviewer3rdController
    {
        /// <summary>
        /// 查詢分行資訊 API (查詢案件正卡人)
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Reviewer3rd/GetBranchInfoByApplyNo/20241203E2074
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(查詢分行資訊業務狀況_2000_ResEx),
            typeof(查詢分行資訊業務狀況_5002_ResEx),
            typeof(查詢分行資訊業務狀況_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [HttpGet("{applyNo}")]
        [OpenApiOperation("GetBranchInfoByApplyNo")]
        public async Task<IResult> GetBranchInfoByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer3rd.GetBranchInfoByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMW3ProcAdapter mw3Adapter, IJWTProfilerHelper jwtProfilerHelper, IReviewerHelper reviewerHelper)
        : IRequestHandler<Query, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            // R120667319 查詢到資料

            var checkApplyNo = request.applyNo;
            var checkID = await 取得申請信用卡正卡人身分證(checkApplyNo);

            if (checkID is null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "信用卡申請資料查無正卡人ID");

            DateTime now = DateTime.Now;
            var response = await mw3Adapter.QuerySearchCusData(checkID);

            if (!response.IsSuccess)
            {
                context.Reviewer_ApplyCreditCardInfoProcess.Add(MapToProcess(checkApplyNo, now, ProcessNoteConst.分行資訊查詢錯誤));
                await context.SaveChangesAsync();
                return ApiResponseHelper.CheckThirdPartyApiError<string>(checkApplyNo, "此申請書編號查詢分行資訊失敗");
            }

            var searchCusDataAPIResponse = response.Data!;
            var code = searchCusDataAPIResponse.RtnCode;
            var res = searchCusDataAPIResponse.Response;

            if (code == MW3RtnCodeConst.成功)
            {
                QueryBranchInfoDto searchCusDataAPIData = new();

                searchCusDataAPIData.BranchCusCusInfo.AddRange(
                    searchCusDataAPIResponse.Info.客戶資訊.Select(item => new Reviewer3rd_BranchCusCusInfo
                    {
                        ApplyNo = checkApplyNo,
                        ID = item.ID != null ? item.ID.Trim() : string.Empty,
                        SN = item.SN != null ? item.SN.Trim() : string.Empty,
                        Cate = item.Cate != null ? item.Cate.Trim() : string.Empty,
                        UserType = UserType.正卡人,
                    })
                );

                searchCusDataAPIData.BranchCusWMCust.AddRange(
                    searchCusDataAPIResponse.Info.財富管理客戶.Select(item => new Reviewer3rd_BranchCusWMCust
                    {
                        ApplyNo = checkApplyNo,
                        ICountFlag = item.ICountFlag != null ? item.ICountFlag.Trim() : string.Empty,
                        UserType = UserType.正卡人,
                        ID = checkID,
                    })
                );

                searchCusDataAPIData.BranchCusCD.AddRange(
                    searchCusDataAPIResponse.Info.定存明細資訊.Select(item => new Reviewer3rd_BranchCusCD
                    {
                        ApplyNo = checkApplyNo,
                        ID = item.ID != null ? item.ID.Trim() : string.Empty,
                        Currency = item.Currency != null ? item.Currency.Trim() : string.Empty,
                        InterestD = item.InterestD != null ? item.InterestD.Trim() : string.Empty,
                        ExpirationD = item.ExpirationD != null ? item.ExpirationD.Trim() : string.Empty,
                        Amount = item.Amount,
                        Cate = item.Cate != null ? item.Cate.Trim() : string.Empty,
                        UserType = UserType.正卡人,
                    })
                );

                searchCusDataAPIData.BranchCusDD.AddRange(
                    searchCusDataAPIResponse.Info.活期存款明細資訊.Select(item => new Reviewer3rd_BranchCusDD
                    {
                        ApplyNo = checkApplyNo,
                        ID = item.ID != null ? item.ID.Trim() : string.Empty,
                        Cate = item.Cate != null ? item.Cate.Trim() : string.Empty,
                        Currency = item.Currency != null ? item.Currency.Trim() : string.Empty,
                        Account = item.Account != null ? item.Account.Trim() : string.Empty,
                        OpenAccountD = item.OpenAcountD != null ? item.OpenAcountD.Trim() : string.Empty,
                        CreditD = item.CreditD != null ? item.CreditD.Trim() : string.Empty,
                        Last3MavgCredit = item.Last3MavgCredit,
                        ThreeMavgCredit = item.ThreeMavgCredit,
                        TwoMavgCredit = item.TwoMavgCredit,
                        OneMavgCredit = item.OneMavgCredit,
                        Credit = item.Credit,
                        UserType = UserType.正卡人,
                    })
                );

                searchCusDataAPIData.BranchCusCAD.AddRange(
                    searchCusDataAPIResponse.Info.支票存款明細資訊.Select(item => new Reviewer3rd_BranchCusCAD
                    {
                        ApplyNo = checkApplyNo,
                        ID = item.ID != null ? item.ID.Trim() : string.Empty,
                        Cate = item.Cate != null ? item.Cate.Trim() : string.Empty,
                        Account = item.Account != null ? item.Account.Trim() : string.Empty,
                        OpenAccountD = item.OpenAcountD != null ? item.OpenAcountD.Trim() : string.Empty,
                        CreditD = item.CreditD != null ? item.CreditD.Trim() : string.Empty,
                        Last3MavgCredit = item.Last3MavgCredit,
                        ThreeMavgCredit = item.ThreeMavgCredit,
                        TwoMavgCredit = item.TwoMavgCredit,
                        OneMavgCredit = item.OneMavgCredit,
                        Credit = item.Credit,
                        UserType = UserType.正卡人,
                    })
                );

                searchCusDataAPIData.BranchCusCreditOver.AddRange(
                    searchCusDataAPIResponse.Info.授信逾期狀況.Select(item => new Reviewer3rd_BranchCusCreditOver
                    {
                        ApplyNo = checkApplyNo,
                        ID = item.ID != null ? item.ID.Trim() : string.Empty,
                        Account = item.Account != null ? item.Account.Trim() : string.Empty,
                        OverStatus = item.OverStatus != null ? item.OverStatus.Trim() : string.Empty,
                        UserType = UserType.正卡人,
                    })
                );

                var check = await context.Reviewer_FinanceCheckInfo.FirstOrDefaultAsync(x =>
                    x.ApplyNo == checkApplyNo && x.ID == checkID && x.UserType == UserType.正卡人
                );

                check.IsBranchCustomer = "Y";
                check.BranchCus_RtnCode = code;
                check.BranchCus_RtnMsg = res;
                check.BranchCus_QueryTime = now;

                var main = await context.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == checkApplyNo);

                main.IsBranchCustomer = "Y";

                await context.Reviewer3rd_BranchCusCusInfo.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
                await context.Reviewer3rd_BranchCusWMCust.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
                await context.Reviewer3rd_BranchCusCD.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
                await context.Reviewer3rd_BranchCusDD.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
                await context.Reviewer3rd_BranchCusCAD.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
                await context.Reviewer3rd_BranchCusCreditOver.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();

                await context.Reviewer3rd_BranchCusCusInfo.AddRangeAsync(searchCusDataAPIData.BranchCusCusInfo);
                await context.Reviewer3rd_BranchCusWMCust.AddRangeAsync(searchCusDataAPIData.BranchCusWMCust);
                await context.Reviewer3rd_BranchCusCD.AddRangeAsync(searchCusDataAPIData.BranchCusCD);
                await context.Reviewer3rd_BranchCusDD.AddRangeAsync(searchCusDataAPIData.BranchCusDD);
                await context.Reviewer3rd_BranchCusCAD.AddRangeAsync(searchCusDataAPIData.BranchCusCAD);
                await context.Reviewer3rd_BranchCusCreditOver.AddRangeAsync(searchCusDataAPIData.BranchCusCreditOver);

                await context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(MapToProcess(checkApplyNo, now, $"(正卡人_{checkID})"));

                await context.SaveChangesAsync();

                return ApiResponseHelper.Success<string>(checkApplyNo, "此申請書編號查詢分行資訊業務狀況完畢");
            }
            else if (code == MW3RtnCodeConst.查詢分行資訊_查無資料)
            {
                QueryBranchInfoDto searchCusDataAPIData = new();

                var check = await context.Reviewer_FinanceCheckInfo.FirstOrDefaultAsync(x =>
                    x.ApplyNo == checkApplyNo && x.ID == checkID && x.UserType == UserType.正卡人
                );

                check.IsBranchCustomer = "N";
                check.BranchCus_RtnCode = code;
                check.BranchCus_RtnMsg = res;
                check.BranchCus_QueryTime = now;

                var main = await context.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == checkApplyNo);

                main.IsBranchCustomer = "N";

                await context.Reviewer3rd_BranchCusCusInfo.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
                await context.Reviewer3rd_BranchCusWMCust.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
                await context.Reviewer3rd_BranchCusCD.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
                await context.Reviewer3rd_BranchCusDD.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
                await context.Reviewer3rd_BranchCusCAD.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
                await context.Reviewer3rd_BranchCusCreditOver.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();

                await context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(MapToProcess(checkApplyNo, now, $"(正卡人_{checkID})"));

                await context.SaveChangesAsync();

                var isUpdateSuccessful = await reviewerHelper.UpdateMainLastModified(checkApplyNo, jwtProfilerHelper.UserId, now) == 1;
                if (!isUpdateSuccessful)
                {
                    return ApiResponseHelper.InternalServerError<string>(null, "更新Main最後異動資訊失敗");
                }

                return ApiResponseHelper.Success<string>(checkApplyNo, "此申請書編號查詢分行資訊業務狀況完畢");
            }
            else if (
                code == MW3RtnCodeConst.查詢分行資訊_傳入規格不符合
                || code == MW3RtnCodeConst.查詢分行資訊_此服務已失效
                || code == MW3RtnCodeConst.查詢分行資訊_聯絡系統管理員
            )
            {
                string note = $"{ProcessNoteConst.分行資訊查詢錯誤}(正卡人_{checkID})";
                await context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(MapToProcess(checkApplyNo, now, note));
                await context.SaveChangesAsync();
                return ApiResponseHelper.CheckThirdPartyApiError<string>(checkApplyNo, "此申請書編號查詢分行資訊失敗");
            }
            else
            {
                string note = $"{ProcessNoteConst.分行資訊查詢錯誤}(正卡人_{checkID})";
                await context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(MapToProcess(checkApplyNo, now, note));
                await context.SaveChangesAsync();
                return ApiResponseHelper.CheckThirdPartyApiError<string>(checkApplyNo, "此申請書編號查詢分行資訊失敗");
            }
        }

        private async Task<string?> 取得申請信用卡正卡人身分證(string applyNo)
        {
            return (
                await context
                    .Reviewer_FinanceCheckInfo.AsNoTracking()
                    .SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人)
            )?.ID;
        }

        private Reviewer_ApplyCreditCardInfoProcess MapToProcess(string applyNo, DateTime startTime, string? note = null)
        {
            return new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = applyNo,
                ProcessUserId = jwtProfilerHelper.UserId,
                Process = ProcessConst.完成分行資訊查詢,
                StartTime = startTime,
                EndTime = DateTime.Now,
                Notes = note,
            };
        }
    }
}
