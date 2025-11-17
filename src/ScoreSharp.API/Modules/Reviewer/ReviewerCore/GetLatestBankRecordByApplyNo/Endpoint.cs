using System.Data;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetLatestBankRecordByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 取得行內外紀錄 By申請書編號
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router: /ReviewerCore/GetLatestBankRecordByApplyNo/20241127X8342
        ///
        ///     Content:
        ///     1. 行外紀錄 (正卡人)
        ///     2. 分行資訊 (正卡人)
        ///     3. 929 (正/附卡人)
        ///     4. 關注名單 1 (正/附卡人)
        ///     	- WarningCompany B.受警示企業戶之負責人
        ///     	- RiskAccount C.風險帳戶
        ///     	- FrdIdLog H.疑似涉詐境內帳戶
        ///     5. 關注名單 2 (正/附卡人)
        ///     	- WarnLog A.告誡名單
        ///     	- FledLog D.聯徵資料─行方不明
        ///     	- PunishLog E.聯徵資料─收容遣返
        ///     	- ImmiLog F.聯徵資料─出境
        ///     	- MissingPersonsLog G.失蹤人口
        ///     	- LayOffLog I.聯徵資料─解聘
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetLatestBankRecordByApplyNoResponse>))]
        [EndpointSpecificExample(typeof(取得行內外資訊_2000_ResEx), ExampleType = ExampleType.Response, ResponseStatusCode = StatusCodes.Status200OK)]
        [OpenApiOperation("GetLatestBankRecordByApplyNo")]
        public async Task<IResult> GetLatestBankRecordByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetLatestBankRecordByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetLatestBankRecordByApplyNoResponse>>;

    public class Handler(IScoreSharpDapperContext scoreSharpDapperContext, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<GetLatestBankRecordByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetLatestBankRecordByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken) =>
            ApiResponseHelper.Success(await GetLatestBankRecordByApplyNo(request.applyNo));

        private async Task<GetLatestBankRecordByApplyNoResponse> GetLatestBankRecordByApplyNo(string applyNo)
        {
            var (
                outsideBankInfos,
                financeCheckInfos,
                branchCusCAD,
                branchCusCD,
                branchCusCreditOver,
                branchCusCusInfo,
                branchCusDD,
                branchCusWMCust,
                query929Logs,
                warnCompLogs,
                riskAccountLogs,
                frdIdLogs,
                warnLogs,
                fledLogs,
                punishLogs,
                immiLogs,
                missingPersonsLogs,
                layOffLogs
            ) = await GetLatestBankRecord(applyNo);

            GetLatestBankRecordByApplyNoResponse response = new();

            var primaryFinanceCheck = financeCheckInfos.SingleOrDefault(x => x.ApplyNo == applyNo && x.UserType == UserType.正卡人);

            // 分行資訊 (正卡人)
            if (primaryFinanceCheck is not null)
            {
                response.BranchCusInfo.BranchCusRes = new TxnResponse()
                {
                    ApplyNo = primaryFinanceCheck.ApplyNo,
                    Response = primaryFinanceCheck.BranchCus_RtnMsg,
                    RtnCode = primaryFinanceCheck.BranchCus_RtnCode,
                    AddTime = primaryFinanceCheck.BranchCus_QueryTime ?? DateTime.MinValue,
                };
            }
            response.BranchCusInfo.BranchCusCusInfo = branchCusCusInfo;
            response.BranchCusInfo.BranchCusCD = branchCusCD;
            response.BranchCusInfo.BranchCusDD = branchCusDD;
            response.BranchCusInfo.BranchCusCAD = branchCusCAD;
            response.BranchCusInfo.BranchCusWMCust = branchCusWMCust;
            response.BranchCusInfo.BranchCusCreditOver = branchCusCreditOver;

            // summary
            response.BranchCusInfo.BranchCusSummary.BranchCusCDTotal = branchCusCD.Sum(x => x.Amount);
            response.BranchCusInfo.BranchCusSummary.BranchCusDDTotal = branchCusDD.Sum(x => x.Last3MavgCredit);
            response.BranchCusInfo.BranchCusSummary.BranchCusCADTotal = branchCusCAD.Sum(x => x.Last3MavgCredit);
            response.BranchCusInfo.BranchCusSummary.SummaryTotal =
                response.BranchCusInfo.BranchCusSummary.BranchCusCDTotal
                + response.BranchCusInfo.BranchCusSummary.BranchCusDDTotal
                + response.BranchCusInfo.BranchCusSummary.BranchCusCADTotal;

            string note = "";
            if (branchCusCusInfo.Count > 0)
            {
                note += "客戶資訊、";
            }
            if (branchCusCD.Count > 0)
            {
                note += "定存明細資訊、";
            }
            if (branchCusDD.Count > 0)
            {
                note += "活期存款明細資訊、";
            }
            if (branchCusCAD.Count > 0)
            {
                note += "支票存款明細資訊、";
            }
            if (branchCusWMCust.Count > 0)
            {
                note += "財富管理客戶、";
            }
            if (branchCusCreditOver.Count > 0)
            {
                note += "授信逾期狀況、";
            }
            note = note.TrimEnd('、');

            if (note == "")
            {
                note = "查無資料";
            }
            else
            {
                note = "以下表格有資料：" + note;
            }
            response.BranchCusInfo.BranchCusSummary.Note = note;

            // 929查詢紀錄
            if (primaryFinanceCheck is not null)
            {
                response.Query929Info.Query929Res = new TxnResponse()
                {
                    ApplyNo = primaryFinanceCheck.ApplyNo,
                    Response = primaryFinanceCheck.Q929_RtnMsg,
                    RtnCode = primaryFinanceCheck.Q929_RtnCode,
                    AddTime = primaryFinanceCheck.Q929_QueryTime ?? DateTime.MinValue,
                };
            }
            response.Query929Info.Query929Logs = query929Logs;
            response.Query929Info.Note = "";
            if (!query929Logs.Any())
            {
                response.Query929Info.Note += "查無資料";
            }
            else
            {
                response.Query929Info.Note += string.Join(
                    "／",
                    query929Logs.Select(x => new { x.UserType, x.ID }).Distinct().Select(x => $"{x.UserType.ToString()}:{x.ID}:有資料")
                );
            }

            // 外部銀行資訊
            response.OutsideBankInfo = outsideBankInfos.FirstOrDefault();

            /*
                關注名單 1
                B. 受警示企業戶之負責人
                C. 風險帳戶
                H. 疑似涉詐境內帳戶
            */

            if (primaryFinanceCheck is not null)
            {
                response.Focus1Info.QueryFocus1Res = new TxnResponse()
                {
                    ApplyNo = primaryFinanceCheck.ApplyNo,
                    Response = primaryFinanceCheck.Focus1_RtnMsg,
                    RtnCode = primaryFinanceCheck.Focus1_RtnCode,
                    AddTime = primaryFinanceCheck.Focus1_QueryTime ?? DateTime.MinValue,
                    TraceId = primaryFinanceCheck.Focus1_TraceId,
                };
            }

            response.Focus1Info.WarnCompLogs = warnCompLogs;
            response.Focus1Info.RiskAccountLogs = riskAccountLogs;
            response.Focus1Info.FrdIdLogs = frdIdLogs;

            response.Focus1Info.Note = "";
            if (!warnCompLogs.Any() && !riskAccountLogs.Any() && !frdIdLogs.Any())
            {
                response.Focus1Info.Note += "查無資料";
            }
            else
            {
                if (warnCompLogs.Any())
                {
                    response.Focus1Info.Note += "受警示企業戶之負責人、";
                }
                if (riskAccountLogs.Any())
                {
                    response.Focus1Info.Note += "風險帳戶、";
                }
                if (frdIdLogs.Any())
                {
                    response.Focus1Info.Note += "疑似涉詐境內帳戶、";
                }
                response.Focus1Info.Note = response.Focus1Info.Note.Trim('、');
                response.Focus1Info.Note += "有資料";
            }

            /*
                關注名單 2
                A. 告誡名單
                D. 聯徵資料─行方不明
                E. 聯徵資料─收容遣返
                F. 聯徵資料─出境
                G. 失蹤人口
                I. 聯徵資料─解聘
            */

            if (primaryFinanceCheck is not null)
            {
                response.Focus2Info.QueryFocus2Res = new TxnResponse()
                {
                    ApplyNo = primaryFinanceCheck.ApplyNo,
                    Response = primaryFinanceCheck.Focus2_RtnMsg,
                    RtnCode = primaryFinanceCheck.Focus2_RtnCode,
                    AddTime = primaryFinanceCheck.Focus2_QueryTime ?? DateTime.MinValue,
                    TraceId = primaryFinanceCheck.Focus2_TraceId,
                };
            }

            response.Focus2Info.WarnLogs = warnLogs;
            response.Focus2Info.FledLogs = fledLogs;
            response.Focus2Info.PunishLogs = punishLogs;
            response.Focus2Info.ImmiLogs = immiLogs;
            response.Focus2Info.MissingPersonsLogs = missingPersonsLogs;
            response.Focus2Info.LayOffLogs = layOffLogs;

            response.Focus2Info.Note = "";
            if (
                !warnLogs.Any()
                && !fledLogs.Any()
                && !punishLogs.Any()
                && !immiLogs.Any()
                && !missingPersonsLogs.Any(x => x.YnmpInfo == "Y")
                && !layOffLogs.Any()
            )
            {
                response.Focus2Info.Note += "查無資料";
            }
            else
            {
                if (warnLogs.Any())
                {
                    response.Focus2Info.Note += "告誡名單、";
                }
                if (fledLogs.Any())
                {
                    response.Focus2Info.Note += "行方不明、";
                }
                if (punishLogs.Any())
                {
                    response.Focus2Info.Note += "收容遣返、";
                }
                if (immiLogs.Any())
                {
                    response.Focus2Info.Note += "出境、";
                }
                if (missingPersonsLogs.Any(x => x.YnmpInfo == "Y"))
                {
                    response.Focus2Info.Note += "失蹤人口、";
                }
                if (layOffLogs.Any())
                {
                    response.Focus2Info.Note += "聯徵資料─解聘、";
                }

                response.Focus2Info.Note = response.Focus2Info.Note.Trim('、');
                response.Focus2Info.Note += "有資料";
            }

            return response;
        }

        private async Task<(
            List<OutsideBankInfoResponse> outsideBankInfos,
            List<Reviewer_FinanceCheckInfo> financeCheckInfos,
            List<BranchCusCADResponse> branchCusCAD,
            List<BranchCusCDResponse> branchCusCD,
            List<BranchCusCreditOverResponse> branchCusCreditOver,
            List<BranchCusCusInfoResponse> branchCusCusInfo,
            List<BranchCusDDResponse> branchCusDD,
            List<BranchCusWMCustResponse> branchCusWMCust,
            List<Query929LogResponse> query929Logs,
            List<WarnCompLogResponse> warnCompLogs,
            List<RiskAccountLogResponse> riskAccountLogs,
            List<FrdIdLogResponse> frdIdLogs,
            List<WarnLogResponse> warnLogs,
            List<FledLogResponse> fledLogs,
            List<PunishLogResponse> punishLogs,
            List<ImmiLogResponse> immiLogs,
            List<MissingPersonsLogResponse> missingPersonsLogs,
            List<LayOffLogResponse> layOffLogs
        )> GetLatestBankRecord(string applyNo)
        {
            using var conn = scoreSharpDapperContext.CreateScoreSharpConnection();
            var results = await conn.QueryMultipleAsync(
                sql: "Usp_GetLatestBankRecord",
                param: new { applyNo = applyNo },
                commandType: CommandType.StoredProcedure
            );

            // 順序需與資料庫查詢結果相同
            var outsideBankInfos = results.Read<OutsideBankInfoResponse>().ToList();
            var financeCheckInfos = results.Read<Reviewer_FinanceCheckInfo>().ToList();
            var branchCusCAD = results.Read<BranchCusCADResponse>().ToList();
            var branchCusCD = results.Read<BranchCusCDResponse>().ToList();
            var branchCusCreditOver = results.Read<BranchCusCreditOverResponse>().ToList();
            var branchCusCusInfo = results.Read<BranchCusCusInfoResponse>().ToList();
            var branchCusDD = results.Read<BranchCusDDResponse>().ToList();
            var branchCusWMCust = results.Read<BranchCusWMCustResponse>().ToList();
            var query929Logs = results.Read<Query929LogResponse>().ToList();
            var warnCompLogs = results.Read<WarnCompLogResponse>().ToList();
            var riskAccountLogs = results.Read<RiskAccountLogResponse>().ToList();
            var frdIdLogs = results.Read<FrdIdLogResponse>().ToList();
            var warnLogs = results.Read<WarnLogResponse>().ToList();
            var fledLogs = results.Read<FledLogResponse>().ToList();
            var punishLogs = results.Read<PunishLogResponse>().ToList();
            var immiLogs = results.Read<ImmiLogResponse>().ToList();
            var missingPersonsLogs = results.Read<MissingPersonsLogResponse>().ToList();
            var layOffLogs = results.Read<LayOffLogResponse>().ToList();

            return (
                outsideBankInfos,
                financeCheckInfos,
                branchCusCAD,
                branchCusCD,
                branchCusCreditOver,
                branchCusCusInfo,
                branchCusDD,
                branchCusWMCust,
                query929Logs,
                warnCompLogs,
                riskAccountLogs,
                frdIdLogs,
                warnLogs,
                fledLogs,
                punishLogs,
                immiLogs,
                missingPersonsLogs,
                layOffLogs
            );
        }
    }
}
