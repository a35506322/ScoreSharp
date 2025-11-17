using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer3rd.GetFocus2ByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer3rd
{
    public partial class Reviewer3rdController
    {
        /// <summary>
        /// 查詢關注名單2 API (查詢案件正附卡人)
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Reviewer3rd/GetFocus2ByApplyNo/20240803B0001
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [OpenApiOperation("GetFocus2ByApplyNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(查詢關注名單2成功_2000_ResEx),
            typeof(查詢關注名單2失敗_4003_ResEx),
            typeof(查詢關注名單2失敗_5002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> GetFocus2ByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer3rd.GetFocus2ByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<string>>;

    public class Handler(
        ScoreSharpContext context,
        IMW3MSAPIAdapter mw3Adapter,
        IJWTProfilerHelper jwtProfilerHelper,
        IReviewerHelper reviewerHelper,
        ILogger<Handler> logger
    ) : IRequestHandler<Query, ResultResponse<string>>
    {
        // A122911398 有資料
        public async Task<ResultResponse<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            var checkApplyNo = request.applyNo;
            var checkIDList = await 取得申請信用卡身分證(checkApplyNo);

            if (checkIDList.Count == 0)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "信用卡申請資料查無正附卡人ID");
            }

            DateTime now = DateTime.Now;
            bool hasApiError = false;
            string? finalErrorMessage = null;

            var checkConcernDeailTask = checkIDList.Select(async item =>
            {
                var response = await mw3Adapter.QueryConcernDetail(item.ID);
                return new
                {
                    id = item.ID,
                    userType = item.UserType,
                    response,
                };
            });

            var checkConcernDeailResults = await Task.WhenAll(checkConcernDeailTask);

            var concernDetailInfoDtos = new List<ConcernDetailInfoDto>();
            List<Reviewer3rd_WarnLog> warningLogs = new();
            List<Reviewer3rd_FledLog> fledLogs = new();
            List<Reviewer3rd_PunishLog> punishLogs = new();
            List<Reviewer3rd_ImmiLog> immiLogs = new();
            List<Reviewer3rd_MissingPersonsLog> missingPersonsLogs = new();
            List<Reviewer3rd_LayOffLog> layOffLogs = new();
            List<Reviewer_ApplyCreditCardInfoProcess> processes = new();

            foreach (var result in checkConcernDeailResults)
            {
                var id = result.id != null ? result.id.Trim() : string.Empty;
                var userType = result.userType;

                // HTTP 失敗處理
                if (!result.response.IsSuccess)
                {
                    logger.LogError(
                        "查詢關注名單2失敗 - ApplyNo: {ApplyNo}, ID: {ID}, UserType: {UserType}, ErrorMessage: {ErrorMessage}",
                        checkApplyNo,
                        id,
                        userType,
                        result.response.ErrorMessage
                    );

                    hasApiError = true;
                    finalErrorMessage = "此申請書編號查詢關注名單2失敗";

                    // 使用 DTO 統一處理（失敗情況）
                    ConcernDetailInfoDto concernDetailInfoDto = new()
                    {
                        ApplyNo = checkApplyNo,
                        ID = id,
                        UserType = userType,
                        RtnCode = null, // HTTP 失敗設為 null
                        RtnMsg = null,
                        QueryTime = now, // 仍然記錄查詢時間
                        TraceId = null,
                        // Focus2HitList 保持空的，Focus2Checked 會自動變成 "N"
                    };

                    concernDetailInfoDtos.Add(concernDetailInfoDto);
                    processes.Add(MapToProcess(checkApplyNo, now, note: $"關注名單2查詢失敗({userType}_{id})"));
                    continue; // 跳過此人，繼續處理下一個
                }

                var queryConcernDetailResponse = result.response.Data!;
                var code = queryConcernDetailResponse.RtnCode != null ? queryConcernDetailResponse.RtnCode.Trim() : string.Empty;
                var msg = queryConcernDetailResponse.Msg != null ? queryConcernDetailResponse.Msg.Trim() : string.Empty;
                var traceId = queryConcernDetailResponse.TraceId != null ? queryConcernDetailResponse.TraceId.Trim() : string.Empty;

                if (code == MW3RtnCodeConst.成功)
                {
                    ConcernDetailInfoDto concernDetailInfoDto = new()
                    {
                        ApplyNo = checkApplyNo,
                        ID = id,
                        UserType = userType,
                        RtnCode = code,
                        RtnMsg = msg,
                        QueryTime = now,
                        TraceId = traceId,
                    };

                    var info = queryConcernDetailResponse.Info;

                    if (info.Restriction.Count > 0)
                    {
                        warningLogs.AddRange(
                            info.Restriction.Select(item => new Reviewer3rd_WarnLog
                                {
                                    ApplyNo = checkApplyNo,
                                    DataType = item.DataType != null ? item.DataType.Trim() : string.Empty,
                                    ID = item.ID != null ? item.ID.Trim() : string.Empty,
                                    WarningDate = item.WarningDate != null ? item.WarningDate.Trim() : string.Empty,
                                    ExpireDate = item.ExpireDate != null ? item.ExpireDate.Trim() : string.Empty,
                                    Issuer = item.Issuer != null ? item.Issuer.Trim() : string.Empty,
                                    CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                })
                                .ToList()
                        );

                        concernDetailInfoDto.Focus2HitList.Add("A");
                    }
                    if (info.Fled.Count > 0)
                    {
                        // 聯徵資料─行方不明 (D)
                        fledLogs.AddRange(
                            info.Fled.Select(item => new Reviewer3rd_FledLog
                                {
                                    ApplyNo = checkApplyNo,
                                    ResidentIdNo = item.ResidentIdNo != null ? item.ResidentIdNo.Trim() : string.Empty,
                                    ENName = item.ENName != null ? item.ENName.Trim() : string.Empty,
                                    PassportNo = item.PassportNo != null ? item.PassportNo.Trim() : string.Empty,
                                    Nationality = item.Nationality != null ? item.Nationality.Trim() : string.Empty,
                                    BirthDate = item.BirthDate != null ? item.BirthDate.Trim() : string.Empty,
                                    Gender = item.Gender != null ? item.Gender.Trim() : string.Empty,
                                    FledDate = item.FledDate != null ? item.FledDate.Trim() : string.Empty,
                                    CatchingDate = item.CatchingDate != null ? item.CatchingDate.Trim() : string.Empty,
                                    CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                    UserType = userType,
                                    ID = id,
                                })
                                .ToList()
                        );

                        concernDetailInfoDto.Focus2HitList.Add("D");
                    }

                    if (info.Punish.Count > 0)
                    {
                        // 聯徵資料─收容遣返 (E)
                        punishLogs.AddRange(
                            info.Punish.Select(item => new Reviewer3rd_PunishLog
                                {
                                    ApplyNo = checkApplyNo,
                                    ResidentIdNo = item.ResidentIdNo != null ? item.ResidentIdNo.Trim() : string.Empty,
                                    ENName = item.ENName != null ? item.ENName.Trim() : string.Empty,
                                    PassportNo = item.PassportNo != null ? item.PassportNo.Trim() : string.Empty,
                                    Nationality = item.Nationality != null ? item.Nationality.Trim() : string.Empty,
                                    BirthDate = item.BirthDate != null ? item.BirthDate.Trim() : string.Empty,
                                    Gender = item.Gender != null ? item.Gender.Trim() : string.Empty,
                                    CatchingDate = item.CatchingDate != null ? item.CatchingDate.Trim() : string.Empty,
                                    ImmigrateDate = item.ImmigrateDate != null ? item.ImmigrateDate.Trim() : string.Empty,
                                    CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                    UserType = userType,
                                    ID = id,
                                })
                                .ToList()
                        );

                        concernDetailInfoDto.Focus2HitList.Add("E");
                    }

                    if (info.Immi.Count > 0)
                    {
                        // 聯徵資料─出境 (F)
                        immiLogs.AddRange(
                            info.Immi.Select(item => new Reviewer3rd_ImmiLog
                                {
                                    ApplyNo = checkApplyNo,
                                    ResidentIdNo = item.ResidentIdNo != null ? item.ResidentIdNo.Trim() : string.Empty,
                                    ENName = item.ENName != null ? item.ENName.Trim() : string.Empty,
                                    PassportNo = item.PassportNo != null ? item.PassportNo.Trim() : string.Empty,
                                    Nationality = item.Nationality != null ? item.Nationality.Trim() : string.Empty,
                                    BirthDate = item.BirthDate != null ? item.BirthDate.Trim() : string.Empty,
                                    Gender = item.Gender != null ? item.Gender.Trim() : string.Empty,
                                    ImmigrateDate = item.ImmigrateDate != null ? item.ImmigrateDate.Trim() : string.Empty,
                                    InTW = item.InTW != null ? item.InTW.Trim() : string.Empty,
                                    CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                    UserType = userType,
                                    ID = id,
                                })
                                .ToList()
                        );

                        concernDetailInfoDto.Focus2HitList.Add("F");
                    }

                    if (info.MissingPersons != null)
                    {
                        var missingPersons = new Reviewer3rd_MissingPersonsLog
                        {
                            ApplyNo = checkApplyNo,
                            ID = id,
                            UserType = userType,
                            YnmpInfo = info.MissingPersons.YnmpInfo != null ? info.MissingPersons.YnmpInfo.Trim() : string.Empty,
                            CreateDate = info.MissingPersons.CreateDate != null ? info.MissingPersons.CreateDate.Trim() : string.Empty,
                        };
                        // 失蹤人口 (G)
                        missingPersonsLogs.Add(missingPersons);

                        if (!string.IsNullOrEmpty(missingPersons.YnmpInfo) && missingPersons.YnmpInfo == "Y")
                        {
                            concernDetailInfoDto.Focus2HitList.Add("G");
                        }
                    }

                    if (info.LayOff.Count > 0)
                    {
                        // 聯徵資料─解聘 (I)
                        layOffLogs.AddRange(
                            info.LayOff.Select(item => new Reviewer3rd_LayOffLog
                                {
                                    ApplyNo = checkApplyNo,
                                    TransDate = item.TransDate != null ? item.TransDate.Trim() : string.Empty,
                                    ChngId = item.ChngId != null ? item.ChngId.Trim() : string.Empty,
                                    NatCode = item.NatCode != null ? item.NatCode.Trim() : string.Empty,
                                    PassNo = item.PassNo != null ? item.PassNo.Trim() : string.Empty,
                                    ExpirWkNo = item.ExpirWkNo != null ? item.ExpirWkNo.Trim() : string.Empty,
                                    KnowDate = item.KnowDate != null ? item.KnowDate.Trim() : string.Empty,
                                    DynaDate = item.DynaDate != null ? item.DynaDate.Trim() : string.Empty,
                                    HappCode = item.HappCode != null ? item.HappCode.Trim() : string.Empty,
                                    VendCode = item.VendCode != null ? item.VendCode.Trim() : string.Empty,
                                    LaborCode = item.LaborCode != null ? item.LaborCode.Trim() : string.Empty,
                                    ExpireDate = item.ExpireDate != null ? item.ExpireDate.Trim() : string.Empty,
                                    ImmiType = item.ImmiType != null ? item.ImmiType.Trim() : string.Empty,
                                    ImmiTypeDesc = item.ImmiTypeDesc != null ? item.ImmiTypeDesc.Trim() : string.Empty,
                                    HappCodeDesc = item.HappCodeDesc != null ? item.HappCodeDesc.Trim() : string.Empty,
                                    VendCodeDesc = item.VendCodeDesc != null ? item.VendCodeDesc.Trim() : string.Empty,
                                    LaborCodeDesc = item.LaborCodeDesc != null ? item.LaborCodeDesc.Trim() : string.Empty,
                                    WpCode = item.WpCode != null ? item.WpCode.Trim() : string.Empty,
                                    WpCodeDesc = item.WpCodeDesc != null ? item.WpCodeDesc.Trim() : string.Empty,
                                    Resnum = item.Resnum != null ? item.Resnum.Trim() : string.Empty,
                                    ImmigartionDate = item.ImmigartionDate != null ? item.ImmigartionDate.Trim() : string.Empty,
                                    SystemDate = item.SystemDate != null ? item.SystemDate.Trim() : string.Empty,
                                    NiaResidenceNo = item.NiaResidenceNo != null ? item.NiaResidenceNo.Trim() : string.Empty,
                                    CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                    UserType = userType,
                                    ID = id,
                                })
                                .ToList()
                        );
                        concernDetailInfoDto.Focus2HitList.Add("I");
                    }

                    concernDetailInfoDtos.Add(concernDetailInfoDto);

                    processes.Add(MapToProcess(checkApplyNo, now, note: $"({userType}_{id})"));
                }
                else
                {
                    // 業務錯誤情況
                    logger.LogWarning(
                        "查詢關注名單2業務錯誤 - ApplyNo: {ApplyNo}, ID: {ID}, UserType: {UserType}, RtnCode: {RtnCode}, Msg: {Msg}",
                        checkApplyNo,
                        id,
                        userType,
                        code,
                        msg
                    );

                    hasApiError = true;
                    finalErrorMessage = "此申請書編號查詢關注名單2失敗";

                    // 使用 DTO 統一處理（業務錯誤情況）
                    ConcernDetailInfoDto concernDetailInfoDto = new()
                    {
                        ApplyNo = checkApplyNo,
                        ID = id,
                        UserType = userType,
                        RtnCode = code, // 記錄實際的業務錯誤碼
                        RtnMsg = msg,
                        QueryTime = now,
                        TraceId = traceId,
                        // Focus2HitList 保持空的，Focus2Checked 會自動變成 "N"
                    };

                    concernDetailInfoDtos.Add(concernDetailInfoDto);
                    processes.Add(MapToProcess(checkApplyNo, now, note: $"關注名單2查詢失敗({userType}_{id})"));
                }
            }

            // 刪除原有紀錄
            await context.Reviewer3rd_MissingPersonsLog.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
            await context.Reviewer3rd_ImmiLog.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
            await context.Reviewer3rd_PunishLog.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
            await context.Reviewer3rd_FledLog.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
            await context.Reviewer3rd_WarnLog.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
            await context.Reviewer3rd_LayOffLog.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();

            // 新增detail
            if (missingPersonsLogs.Count > 0)
            {
                context.Reviewer3rd_MissingPersonsLog.AddRange(missingPersonsLogs);
            }
            if (immiLogs.Count > 0)
            {
                context.Reviewer3rd_ImmiLog.AddRange(immiLogs);
            }
            if (punishLogs.Count > 0)
            {
                context.Reviewer3rd_PunishLog.AddRange(punishLogs);
            }
            if (fledLogs.Count > 0)
            {
                context.Reviewer3rd_FledLog.AddRange(fledLogs);
            }
            if (warningLogs.Count > 0)
            {
                context.Reviewer3rd_WarnLog.AddRange(warningLogs);
            }
            if (layOffLogs.Count > 0)
            {
                context.Reviewer3rd_LayOffLog.AddRange(layOffLogs);
            }
            context.Reviewer_ApplyCreditCardInfoProcess.AddRange(processes);

            // 更新是否命中及命中代號（包含成功和失敗的項目）
            foreach (var concernDetailInfoDto in concernDetailInfoDtos)
            {
                var checkInfo = await context.Reviewer_FinanceCheckInfo.SingleOrDefaultAsync(x =>
                    x.ApplyNo == checkApplyNo && x.UserType == concernDetailInfoDto.UserType && x.ID == concernDetailInfoDto.ID
                );

                if (checkInfo != null)
                {
                    // 判斷是否為成功情況
                    bool isSuccess = concernDetailInfoDto.RtnCode == MW3RtnCodeConst.成功;

                    // 成功時設定實際值，失敗時設定 null（可重試）
                    checkInfo.Focus2Check = isSuccess ? concernDetailInfoDto.Focus2Checked : null;
                    checkInfo.Focus2_RtnCode = concernDetailInfoDto.RtnCode;
                    checkInfo.Focus2_RtnMsg = concernDetailInfoDto.RtnMsg;
                    checkInfo.Focus2_QueryTime = concernDetailInfoDto.QueryTime;
                    checkInfo.Focus2_TraceId = concernDetailInfoDto.TraceId;
                    checkInfo.Focus2Hit = isSuccess ? String.Join("、", concernDetailInfoDto.Focus2HitList) : null;
                }
            }

            await context.SaveChangesAsync();

            var isUpdateSuccessful = await reviewerHelper.UpdateMainLastModified(checkApplyNo, jwtProfilerHelper.UserId, now) == 1;
            if (!isUpdateSuccessful)
            {
                return ApiResponseHelper.InternalServerError<string>(null, "更新Main最後異動資訊失敗");
            }

            // 根據是否有錯誤決定最終回傳
            if (hasApiError)
            {
                return ApiResponseHelper.CheckThirdPartyApiError<string>(checkApplyNo, finalErrorMessage ?? "此申請書編號查詢關注名單2失敗");
            }

            return ApiResponseHelper.Success<string>(checkApplyNo, "此申請書編號查詢關注名單2完畢");
        }

        private async Task<List<QueryConernDetailIDDto>> 取得申請信用卡身分證(string applyNo)
        {
            return await context
                .Reviewer_FinanceCheckInfo.AsNoTracking()
                .Where(x => x.ApplyNo == applyNo)
                .Select(x => new QueryConernDetailIDDto { UserType = x.UserType, ID = x.ID })
                .ToListAsync();
        }

        private Reviewer_ApplyCreditCardInfoProcess MapToProcess(string applyNo, DateTime startTime, string? note = null) =>
            new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = applyNo,
                ProcessUserId = jwtProfilerHelper.UserId,
                Process = ProcessConst.完成關注名單2查詢,
                StartTime = startTime,
                EndTime = DateTime.Now,
                Notes = note,
            };
    }
}
