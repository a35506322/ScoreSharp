using ScoreSharp.API.Modules.Reviewer3rd.CheckInternalEmailByApplyNo;
using ScoreSharp.Common.Adapters.MW3.Models;

namespace ScoreSharp.API.Modules.Reviewer3rd
{
    public partial class Reviewer3rdController
    {
        /// <summary>
        /// 檢核_行內Email By 申請書編號
        /// Note: 無論成功或失敗都會清空紀錄及新增Process
        /// </summary>
        /// <param name="applyNo"></param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [OpenApiOperation("CheckInternalEmailByApplyNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(檢核行內Email業務狀況成功_2000_ResEx),
            typeof(檢核行內Email業務狀況_4001_ResEx),
            typeof(檢核行內Email業務狀況_帳單類型非電子帳單或正卡人Email為空故清空紀錄_4003_ResEx),
            typeof(檢核行內Email業務狀況失敗_5002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> CheckInternalEmailByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer3rd.CheckInternalEmailByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IMW3ProcAdapter mw3Adapter, IJWTProfilerHelper jwtProfilerHelper)
        : IRequestHandler<Query, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;
            DateTime startTime = DateTime.Now;

            // 查出正卡人Email
            var main = await context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x =>
                x.ApplyNo == applyNo && x.UserType == UserType.正卡人
            );

            var bankTrace = await context.Reviewer_BankTrace.SingleOrDefaultAsync(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人);

            if (main is null || bankTrace is null)
                return ApiResponseHelper.NotFound<string>(null, applyNo);

            if (main.BillType != BillType.電子帳單 || string.IsNullOrWhiteSpace(main.EMail))
            {
                await 例外處理(
                    main,
                    bankTrace,
                    applyNo,
                    startTime,
                    ProcessNoteConst.信用卡申請資料帳單類型非電子帳單或正卡人Email為空故清空紀錄,
                    cancellationToken
                );
                return ApiResponseHelper.Success<string>(applyNo, ProcessNoteConst.信用卡申請資料帳單類型非電子帳單或正卡人Email為空故清空紀錄);
            }

            // 利用原持卡人資料查詢Email
            var response = await mw3Adapter.QueryEBill(email: main.EMail);

            if (!response.IsSuccess)
            {
                await 例外處理(main, bankTrace, applyNo, startTime, ProcessNoteConst.行內Email檢核錯誤, cancellationToken);
                return ApiResponseHelper.CheckThirdPartyApiError<string>(applyNo, ProcessNoteConst.行內Email檢核錯誤);
            }

            List<Reviewer_BankInternalSameLog> logs = new();

            var queryEBillData = response.Data!;

            // 如果查詢結果有資料，則新增BankInternalSameLog
            if (queryEBillData.RtnCode == MW3RtnCodeConst.查詢電子帳單_該信箱已存在)
            {
                foreach (var data in queryEBillData.Info.Table.Where(x => x.ID != main.ID))
                {
                    var cardholderResponse = await mw3Adapter.QueryOriginalCardholderData(id: data.ID);
                    var billAddr = cardholderResponse.Data?.Info?.Table?.FirstOrDefault()?.BillAddr;

                    var originalCardholderData = new Reviewer_BankInternalSameLog()
                    {
                        ApplyNo = main.ApplyNo,
                        ID = main.ID,
                        UserType = main.UserType,
                        SameID = data.ID,
                        SameName = data.Name,
                        CheckType = BankInternalSameCheckType.Email,
                        SameBillAddr = billAddr,
                    };
                    logs.Add(originalCardholderData);
                }
            }

            bankTrace.InternalEmailSame_Flag = queryEBillData.RtnCode == MW3RtnCodeConst.查詢電子帳單_該信箱已存在 ? "Y" : "N";
            bankTrace.InternalEmailSame_CheckRecord = null;
            bankTrace.InternalEmailSame_UpdateUserId = null;
            bankTrace.InternalEmailSame_UpdateTime = null;
            bankTrace.InternalEmailSame_IsError = null;
            bankTrace.InternalEmailSame_Relation = null;

            main.LastUpdateTime = startTime;
            main.LastUpdateUserId = jwtProfilerHelper.UserId;

            await context
                .Reviewer_BankInternalSameLog.Where(x => x.ApplyNo == applyNo && x.CheckType == BankInternalSameCheckType.Email)
                .ExecuteDeleteAsync();

            if (logs.Count > 0)
                await context.Reviewer_BankInternalSameLog.AddRangeAsync(logs);

            await context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(MapToProcess(applyNo, startTime, note: $"({main.UserType}_{main.ID})"));
            await context.SaveChangesAsync();

            return ApiResponseHelper.Success<string>(request.applyNo, "此申請書編號查詢行內Email業務狀況完畢");
        }

        private Reviewer_ApplyCreditCardInfoProcess MapToProcess(string applyNo, DateTime startTime, string note)
        {
            return new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = applyNo,
                StartTime = startTime,
                EndTime = DateTime.Now,
                Notes = note,
                Process = ProcessConst.完成行內Email檢核,
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
            bankTrace.InternalEmailSame_Flag = null;
            bankTrace.InternalEmailSame_CheckRecord = null;
            bankTrace.InternalEmailSame_UpdateUserId = null;
            bankTrace.InternalEmailSame_UpdateTime = null;
            bankTrace.InternalEmailSame_IsError = null;
            bankTrace.InternalEmailSame_Relation = null;

            main.LastUpdateTime = startTime;
            main.LastUpdateUserId = jwtProfilerHelper.UserId;

            await context
                .Reviewer_BankInternalSameLog.Where(x => x.ApplyNo == applyNo && x.CheckType == BankInternalSameCheckType.Email)
                .ExecuteDeleteAsync(cancellationToken);

            context.Reviewer_ApplyCreditCardInfoProcess.Add(MapToProcess(applyNo, startTime, note));

            await context.SaveChangesAsync();
        }
    }
}
