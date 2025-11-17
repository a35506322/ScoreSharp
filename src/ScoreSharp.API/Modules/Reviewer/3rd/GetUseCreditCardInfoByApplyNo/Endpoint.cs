using ScoreSharp.API.Modules.Reviewer3rd.GetUseCreditCardInfoByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer3rd
{
    public partial class Reviewer3rdController
    {
        /// <summary>
        /// 查詢卡人信用使用狀況 By 申請書編號 API (申請書編號 => 正卡+附卡人 ID)
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Reviewer3rd/GetUseCreditCardInfoByApplyNo/20241223X6503
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [OpenApiOperation("GetUseCreditCardInfoByApplyNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetUseCreditCardInfoByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(查詢卡人信用使用狀況_2000_ResEx),
            typeof(查詢卡人信用使用狀況_4003_ResEx),
            typeof(查詢卡人信用使用狀況_5002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> GetUseCreditCardInfoByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer3rd.GetUseCreditCardInfoByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetUseCreditCardInfoByApplyNoResponse>>;

    public class Handler(ScoreSharpContext context, IMW3ProcAdapter mw3Adapter)
        : IRequestHandler<Query, ResultResponse<GetUseCreditCardInfoByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetUseCreditCardInfoByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var checkApplyNo = request.applyNo;
            var checkIDList = await 取得申請信用卡身分證(checkApplyNo);

            if (checkIDList.Count == 0)
            {
                return ApiResponseHelper.BusinessLogicFailed<GetUseCreditCardInfoByApplyNoResponse>(null, "信用卡申請資料查無正附卡人ID");
            }

            DateTime startTime = DateTime.Now;
            var checkUseCreditCardInfoTask = checkIDList.Select(async item =>
            {
                var response = await mw3Adapter.QueryIBM7020(item.ID);
                return new
                {
                    id = item.ID,
                    userType = item.UserType,
                    userTypeName = item.UserType.ToString(),
                    response,
                };
            });

            var checkUseCreditCardInfoResults = await Task.WhenAll(checkUseCreditCardInfoTask);

            if (checkUseCreditCardInfoResults.Any(x => !x.response.IsSuccess))
            {
                return ApiResponseHelper.CheckThirdPartyApiError<GetUseCreditCardInfoByApplyNoResponse>(null, request.applyNo);
            }

            GetUseCreditCardInfoByApplyNoResponse result = new();

            result.QueryDate = startTime;
            result.UseCreditCardInfos = [];
            foreach (var checkUseCreditCardInfoResult in checkUseCreditCardInfoResults)
            {
                var ibm7020ApiResponse = checkUseCreditCardInfoResult.response.Data!;
                var code = ibm7020ApiResponse.RetnCode;

                var userType = checkUseCreditCardInfoResult.userType;
                var userTypeName = checkUseCreditCardInfoResult.userTypeName;
                var id = checkUseCreditCardInfoResult.id;

                if (code == MW3RtnCodeConst.成功)
                {
                    List<CreditCardDetailsAndRecords> creditCardDetailsAndRecords = ibm7020ApiResponse
                        .Rec.Where(x => x.RetnCode == MW3RtnCodeConst.成功)
                        .Select(x => new CreditCardDetailsAndRecords
                        {
                            CreditCardNumber = x.CardNmbr,
                            CardName = $"{x.Card1}{x.Card2}",
                            CardType = $"{x.Card3}{x.Card4}",
                            CardCrlimit = int.Parse(x.CardCrlimit),
                            AccountOpeningDate = TransformToROCDate(x.OpendYYYY, x.OpendMM, x.OpendDD),
                            CardExpiryDate = TransformToCardExpiryDate(x.ExpDate),
                            OutstandingBalance = int.TryParse(x.CurrBal, out int outstandingBalance) ? outstandingBalance : 0,
                            BlockCode = x.BlockCode,
                            Last12MonthsRepaymentRecord =
                                $"{x.DelqHist1}{x.DelqHist2}{x.DelqHist3}{x.DelqHist4}{x.DelqHist5}{x.DelqHist6}{x.DelqHist7}{x.DelqHist8}{x.DelqHist9}{x.DelqHist10}{x.DelqHist11}{x.DelqHist12}",
                            LastCreditLimitChangeDate = TransformToROCDate(x.LstCrlimitYYYY, x.LstCrlimitMM, x.LstCrlimitDD),
                            LastCreditLimit_AccountData = int.TryParse(x.LstCrlimit, out int lastCreditLimit) ? lastCreditLimit : 0,
                            LastPaymentDate = TransformToROCDate(x.LstPymtYYYY, x.LstPymtMM, x.LstPymtDD),
                            LastPayment_AccountData = int.TryParse(x.LstPymtAmnt, out int lastPayment) ? lastPayment : 0,
                        })
                        .ToList();

                    CardBalanceAndSummary cardBalanceAndSummary = new CardBalanceAndSummary()
                    {
                        CashAdvanceBalance = int.TryParse(ibm7020ApiResponse.AvailCredit, out int cashAdvanceBalance) ? cashAdvanceBalance : 0,
                        AvailableBalance = int.TryParse(ibm7020ApiResponse.AvailCash, out int availableBalance) ? availableBalance : 0,
                        CreditLimitAdjustmentDate = TransformToROCDate(
                            ibm7020ApiResponse.CoBirthYYYY,
                            ibm7020ApiResponse.CoBirthMM,
                            ibm7020ApiResponse.CoBirthDD
                        ),
                        Memo = ibm7020ApiResponse.Memo,
                    };

                    var info = new UseCreditCardInfo()
                    {
                        UserType = userType,
                        UserTypeName = userTypeName,
                        ID = id,
                        IsSuccess = true,
                        CardBalanceAndSummary = cardBalanceAndSummary,
                        CreditCardDetailsAndRecords = creditCardDetailsAndRecords,
                    };

                    result.UseCreditCardInfos.Add(info);
                }
                else if (code == MW3RtnCodeConst.查詢信用卡資訊_查無資料)
                {
                    var info = new UseCreditCardInfo()
                    {
                        UserType = userType,
                        UserTypeName = userTypeName,
                        ID = id,
                        IsSuccess = false,
                    };

                    result.UseCreditCardInfos.Add(info);
                }
                else
                {
                    return ApiResponseHelper.CheckThirdPartyApiError<GetUseCreditCardInfoByApplyNoResponse>(null, request.applyNo);
                }
            }

            return ApiResponseHelper.Success<GetUseCreditCardInfoByApplyNoResponse>(result, "");
        }

        private async Task<List<QueryIBM7020IDDto>> 取得申請信用卡身分證(string applyNo)
        {
            return await context
                .Reviewer_FinanceCheckInfo.AsNoTracking()
                .Where(x => x.ApplyNo == applyNo)
                .Select(x => new QueryIBM7020IDDto { UserType = x.UserType, ID = x.ID })
                .OrderBy(x => x.UserType)
                .ToListAsync();
        }

        private const int ROC_YEAR_OFFSET = 1911;

        private string TransformToROCDate(string year, string month, string day)
        {
            if (year == "0000" || month == "00" || day == "00")
            {
                return string.Empty;
            }
            int westYear = int.TryParse(year, out int result) ? result : 0;
            string date = $"{westYear - ROC_YEAR_OFFSET}/{month}/{day}";
            return date;
        }

        private string TransformToCardExpiryDate(string monthyeardate)
        {
            int year = int.TryParse($"20{monthyeardate.Substring(2)}", out int result) ? result : 0;
            int month = int.TryParse(monthyeardate.Substring(0, 2), out int result2) ? result2 : 0;
            int day = DateTime.DaysInMonth(year, month);

            return $"{year - ROC_YEAR_OFFSET}/{month}/{day}";
        }
    }
}
