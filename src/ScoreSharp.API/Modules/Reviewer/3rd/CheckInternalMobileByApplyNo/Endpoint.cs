using ScoreSharp.API.Modules.Reviewer3rd.CheckInternalMobileByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer3rd
{
    public partial class Reviewer3rdController
    {
        /// <summary>
        /// 檢核_行內手機 By 申請書編號
        /// Note: 無論成功或失敗都會清空紀錄及新增Process
        /// </summary>
        /// <param name="applyNo"></param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [OpenApiOperation("CheckInternalMobileByApplyNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(檢核行內手機業務狀況成功_2000_ResEx),
            typeof(檢核行內手機業務狀況_4001_ResEx),
            typeof(檢核行內手機業務狀況_不符合檢核條件故清空紀錄_2000_ResEx),
            typeof(檢核行內手機業務狀況失敗_5002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> CheckInternalMobileByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer3rd.CheckInternalMobileByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMW3ProcAdapter mw3Adapter, IJWTProfilerHelper jwtProfilerHelper)
        : IRequestHandler<Query, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            /*
             * 利用ApplyNo去查詢Mobile
             * 利用該比ApplyNo的Mobile放到QueryOriginalCardholderData()去查詢
             * 更新結果(BankTrace、BankInternalSameLog)
             * 新增Process
             */

            var applyNo = request.applyNo;
            DateTime startTime = DateTime.Now;

            var main = await context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x =>
                x.ApplyNo == applyNo && x.UserType == UserType.正卡人
            );

            var bankTrace = await context.Reviewer_BankTrace.SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人);

            if (main is null || bankTrace is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            var mobile = main.Mobile;

            if (string.IsNullOrWhiteSpace(mobile))
            {
                await 例外處理(main, bankTrace, applyNo, startTime, ProcessNoteConst.信用卡申請資料查無正卡人手機故清空紀錄, cancellationToken);
                return ApiResponseHelper.Success<string>(applyNo, ProcessNoteConst.信用卡申請資料查無正卡人手機故清空紀錄);
            }

            // 利用原持卡人資料查詢Mobile
            var response = await mw3Adapter.QueryOriginalCardholderData(id: "", email: "", mobile: mobile);

            if (!response.IsSuccess)
            {
                await 例外處理(main, bankTrace, applyNo, startTime, ProcessNoteConst.行內手機檢核錯誤, cancellationToken);
                return ApiResponseHelper.CheckThirdPartyApiError<string>(applyNo, ProcessNoteConst.行內手機檢核錯誤);
            }

            List<Reviewer_BankInternalSameLog> logs = new();

            var queryOriginalCardholderData = response.Data!;

            // 如果查詢結果有資料，則新增BankInternalSameLog
            if (queryOriginalCardholderData.RtnCode == MW3RtnCodeConst.成功)
            {
                var originalCardholderData = queryOriginalCardholderData
                    .Info.Table.Select(x => MapHelper.MapToQueryOriginalCardholderData(x))
                    .Where(x => x.ID != main.ID)
                    .Select(x => new Reviewer_BankInternalSameLog()
                    {
                        ApplyNo = main.ApplyNo,
                        ID = main.ID,
                        UserType = main.UserType,
                        SameID = x.ID,
                        SameName = x.ChineseName,
                        CheckType = BankInternalSameCheckType.手機,
                        SameBillAddr = x.BillAddr,
                    })
                    .ToList();
                logs.AddRange(originalCardholderData);
            }

            // 更新BankTrace
            bankTrace.InternalMobileSame_Flag = queryOriginalCardholderData.RtnCode == MW3RtnCodeConst.成功 ? "Y" : "N";
            bankTrace.InternalMobileSame_CheckRecord = null;
            bankTrace.InternalMobileSame_UpdateUserId = null;
            bankTrace.InternalMobileSame_UpdateTime = null;
            bankTrace.InternalMobileSame_IsError = null;
            bankTrace.InternalMobileSame_Relation = null;

            // 更新UpdateUserId、UpdateTime
            main.LastUpdateTime = startTime;
            main.LastUpdateUserId = jwtProfilerHelper.UserId;

            // 新增Process
            var process = MapToProcess(applyNo, startTime, note: $"({main.UserType}_{main.ID})");

            // 移除檢核紀錄
            await context
                .Reviewer_BankInternalSameLog.Where(x => x.ApplyNo == applyNo && x.CheckType == BankInternalSameCheckType.手機)
                .ExecuteDeleteAsync();

            if (logs.Count > 0)
                await context.Reviewer_BankInternalSameLog.AddRangeAsync(logs);

            await context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(process);
            await context.SaveChangesAsync();

            return ApiResponseHelper.Success<string>(request.applyNo, "此申請書編號查詢行內手機業務狀況完畢");
        }

        private Reviewer_ApplyCreditCardInfoProcess MapToProcess(string applyNo, DateTime startTime, string note)
        {
            return new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = applyNo,
                StartTime = startTime,
                EndTime = DateTime.Now,
                Notes = note,
                Process = ProcessConst.完成行內手機檢核,
                ProcessUserId = jwtProfilerHelper.UserId,
            };
        }

        private async Task 例外處理(
            Reviewer_ApplyCreditCardInfoMain main,
            Reviewer_BankTrace bankTrace,
            string applyNo,
            DateTime startTime,
            string note,
            CancellationToken cancellationToken
        )
        {
            // 清空所有相關檢核欄位
            bankTrace.InternalMobileSame_Flag = null;
            bankTrace.InternalMobileSame_CheckRecord = null;
            bankTrace.InternalMobileSame_UpdateUserId = null;
            bankTrace.InternalMobileSame_UpdateTime = null;
            bankTrace.InternalMobileSame_IsError = null;
            bankTrace.InternalMobileSame_Relation = null;

            // 更新主檔最後異動資訊
            main.LastUpdateTime = startTime;
            main.LastUpdateUserId = jwtProfilerHelper.UserId;

            // 刪除舊的檢核紀錄
            await context
                .Reviewer_BankInternalSameLog.Where(x => x.ApplyNo == applyNo && x.CheckType == BankInternalSameCheckType.手機)
                .ExecuteDeleteAsync(cancellationToken);

            // 新增 Process 追蹤
            context.Reviewer_ApplyCreditCardInfoProcess.Add(MapToProcess(applyNo, startTime, note));

            // 儲存變更
            await context.SaveChangesAsync();
        }
    }
}
