using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Helpers;
using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase;

public class EcardNotA02CheckNewCaseService(
    ScoreSharpContext efContext,
    IMW3ProcAdapter mw3Adapter,
    IMW3MSAPIAdapter mw3MSAdapter,
    IMW3APAPIAdapter mW3APAPIAdapter,
    IMW3ProcAdapter procAdapter,
    ILogger<EcardNotA02CheckNewCaseService> logger
) : IEcardNotA02CheckNewCaseService
{
    public async Task<CheckCaseRes<QueryBranchInfo>> 檢核_查詢分行資訊(CheckJobContext context)
    {
        CheckCaseRes<QueryBranchInfo> result = new();
        result.SetStartTime();

        try
        {
            var response = await mw3Adapter.QuerySearchCusData(context.ID);

            if (!response.IsSuccess)
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢分行資訊_回傳結果IsSuccess為N",
                    request: new { Id = context.ID, UserType = context.UserType },
                    response: response
                );

                result.SetError(error);

                return result;
            }

            var searchCusDataApiResponse = response.Data!;

            if (searchCusDataApiResponse.RtnCode == MW3RtnCodeConst.成功)
            {
                QueryBranchInfo queryBranchInfoResponse = new()
                {
                    ApplyNo = context.ApplyNo,
                    ID = context.ID,
                    UserType = context.UserType,
                    RtnCode = searchCusDataApiResponse.RtnCode,
                    RtnMsg = searchCusDataApiResponse.Response != null ? searchCusDataApiResponse.Response.Trim() : string.Empty,
                    QueryTime = result.StartTime,
                };

                queryBranchInfoResponse.BranchCusCusInfo.AddRange(
                    searchCusDataApiResponse.Info.客戶資訊.Select(item => new Reviewer3rd_BranchCusCusInfo
                    {
                        ApplyNo = context.ApplyNo,
                        ID = item.ID != null ? item.ID.Trim() : string.Empty,
                        SN = item.SN != null ? item.SN.Trim() : string.Empty,
                        Cate = item.Cate != null ? item.Cate.Trim() : string.Empty,
                        UserType = context.UserType,
                    })
                );

                queryBranchInfoResponse.BranchCusWMCust.AddRange(
                    searchCusDataApiResponse.Info.財富管理客戶.Select(item => new Reviewer3rd_BranchCusWMCust
                    {
                        ApplyNo = context.ApplyNo,
                        ID = context.ID != null ? context.ID.Trim() : string.Empty,
                        ICountFlag = item.ICountFlag != null ? item.ICountFlag.Trim() : string.Empty,
                        UserType = context.UserType,
                    })
                );

                queryBranchInfoResponse.BranchCusCD.AddRange(
                    searchCusDataApiResponse.Info.定存明細資訊.Select(item => new Reviewer3rd_BranchCusCD
                    {
                        ApplyNo = context.ApplyNo,
                        ID = item.ID != null ? item.ID.Trim() : string.Empty,
                        Currency = item.Currency != null ? item.Currency.Trim() : string.Empty,
                        InterestD = item.InterestD != null ? item.InterestD.Trim() : string.Empty,
                        ExpirationD = item.ExpirationD != null ? item.ExpirationD.Trim() : string.Empty,
                        Amount = item.Amount,
                        Cate = item.Cate != null ? item.Cate.Trim() : string.Empty,
                        UserType = context.UserType,
                    })
                );

                queryBranchInfoResponse.BranchCusDD.AddRange(
                    searchCusDataApiResponse.Info.活期存款明細資訊.Select(item => new Reviewer3rd_BranchCusDD
                    {
                        ApplyNo = context.ApplyNo,
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
                        UserType = context.UserType,
                    })
                );

                queryBranchInfoResponse.BranchCusCAD.AddRange(
                    searchCusDataApiResponse.Info.支票存款明細資訊.Select(item => new Reviewer3rd_BranchCusCAD
                    {
                        ApplyNo = context.ApplyNo,
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
                        UserType = context.UserType,
                    })
                );

                queryBranchInfoResponse.BranchCusCreditOver.AddRange(
                    searchCusDataApiResponse.Info.授信逾期狀況.Select(item => new Reviewer3rd_BranchCusCreditOver
                    {
                        ApplyNo = context.ApplyNo,
                        ID = item.ID != null ? item.ID.Trim() : string.Empty,
                        Account = item.Account != null ? item.Account.Trim() : string.Empty,
                        OverStatus = item.OverStatus != null ? item.OverStatus.Trim() : string.Empty,
                        UserType = context.UserType,
                    })
                );

                result.SetSuccess(queryBranchInfoResponse);
            }
            else if (searchCusDataApiResponse.RtnCode == MW3RtnCodeConst.查詢分行資訊_查無資料)
            {
                QueryBranchInfo queryBranchInfoResponse = new()
                {
                    ApplyNo = context.ApplyNo,
                    ID = context.ID,
                    UserType = context.UserType,
                    RtnCode = searchCusDataApiResponse.RtnCode,
                    RtnMsg = searchCusDataApiResponse.Response.Trim(),
                    QueryTime = result.StartTime,
                };

                result.SetSuccess(queryBranchInfoResponse);
            }
            else if (
                searchCusDataApiResponse.RtnCode == MW3RtnCodeConst.查詢分行資訊_聯絡系統管理員
                || searchCusDataApiResponse.RtnCode == MW3RtnCodeConst.查詢分行資訊_傳入規格不符合
                || searchCusDataApiResponse.RtnCode == MW3RtnCodeConst.查詢分行資訊_此服務已失效
            )
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢分行資訊_回傳結果RtnCode為Er01、Er02、Er03",
                    request: new { Id = context.ID, UserType = context.UserType },
                    response: response
                );

                result.SetError(error);
            }
            else
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢分行資訊_回傳結果RtnCode為其他",
                    request: new { Id = context.ID, UserType = context.UserType },
                    response: response
                );

                result.SetError(error);
            }
        }
        catch (Exception ex)
        {
            SystemErrorLog error = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "查詢分行資訊_發生例外",
                request: new { Id = context.ID, UserType = context.UserType },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );

            result.SetError(error);
        }

        return result;
    }

    public async Task<CheckCaseRes<ConcernDetailInfo>> 檢核_查詢關注名單(CheckJobContext context)
    {
        CheckCaseRes<ConcernDetailInfo> result = new();
        result.SetStartTime();
        try
        {
            var response = await mw3MSAdapter.QueryConcernDetail(context.ID);

            if (!response.IsSuccess)
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢關注名單_回傳結果IsSuccess為N",
                    request: new { Id = context.ID, UserType = context.UserType },
                    response: response
                );
                result.SetError(error);
                return result;
            }

            var queryConcernDetailResponse = response.Data!;
            var code = queryConcernDetailResponse.RtnCode;
            var msg = queryConcernDetailResponse.Msg != null ? queryConcernDetailResponse.Msg.Trim() : string.Empty;
            var traceId = queryConcernDetailResponse.TraceId;

            if (code == MW3RtnCodeConst.成功)
            {
                ConcernDetailInfo concernDetailInfo = new()
                {
                    ApplyNo = context.ApplyNo,
                    ID = context.ID,
                    UserType = context.UserType,
                    RtnCode = code,
                    RtnMsg = msg,
                    QueryTime = result.StartTime,
                    TraceId = traceId,
                };

                var info = queryConcernDetailResponse.Info;

                if (info.Restriction.Count > 0)
                {
                    // 告誡名單 (A)
                    concernDetailInfo.WarnLogs.AddRange(
                        info.Restriction.Select(item => new Reviewer3rd_WarnLog
                        {
                            ApplyNo = context.ApplyNo,
                            DataType = item.DataType != null ? item.DataType.Trim() : string.Empty,
                            ID = item.ID != null ? item.ID.Trim() : string.Empty,
                            WarningDate = item.WarningDate != null ? item.WarningDate.Trim() : string.Empty,
                            ExpireDate = item.ExpireDate != null ? item.ExpireDate.Trim() : string.Empty,
                            Issuer = item.Issuer != null ? item.Issuer.Trim() : string.Empty,
                            CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                            UserType = context.UserType,
                        })
                    );

                    concernDetailInfo.Focus2HitList.Add("A");
                }

                if (info.WarningCompany.Count > 0)
                {
                    // 受警示企業戶之負責人 (B)
                    concernDetailInfo.WarningCompanyLogs.AddRange(
                        info.WarningCompany.Select(item => new Reviewer3rd_WarnCompLog
                        {
                            ApplyNo = context.ApplyNo,
                            Account = item.Account != null ? item.Account.Trim() : string.Empty,
                            CorporateID = item.CorporateID != null ? item.CorporateID.Trim() : string.Empty,
                            PID = item.PID != null ? item.PID.Trim() : string.Empty,
                            AccountDate = item.AccountDate != null ? item.AccountDate.Trim() : string.Empty,
                            AccidentCode = item.AccidentCode != null ? item.AccidentCode.Trim() : string.Empty,
                            AccidentDate = item.AccidentDate != null ? item.AccidentDate.Trim() : string.Empty,
                            AccidentCancelDate = item.AccidentCancelDate != null ? item.AccidentCancelDate.Trim() : string.Empty,
                            CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                            UserType = context.UserType,
                            ID = context.ID,
                        })
                    );

                    concernDetailInfo.Focus1HitList.Add("B");
                }

                if (info.RiskAccount.Count > 0)
                {
                    // 風險帳戶 (C)
                    concernDetailInfo.RiskAccountLogs.AddRange(
                        info.RiskAccount.Select(item => new Reviewer3rd_RiskAccountLog
                        {
                            ApplyNo = context.ApplyNo,
                            Account = item.Account != null ? item.Account.Trim() : string.Empty,
                            PID = item.PID != null ? item.PID.Trim() : string.Empty,
                            AccountDate = item.AccountDate != null ? item.AccountDate.Trim() : string.Empty,
                            AccidentDate = item.AccidentDate != null ? item.AccidentDate.Trim() : string.Empty,
                            AccidentCancelDate = item.AccidentCancelDate != null ? item.AccidentCancelDate.Trim() : string.Empty,
                            CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                            UserType = context.UserType,
                            Memo = item.Memo != null ? item.Memo.Trim() : string.Empty,
                            ID = context.ID,
                        })
                    );

                    concernDetailInfo.Focus1HitList.Add("C");
                }

                if (info.FrdId.Count > 0)
                {
                    // 疑似涉詐境內帳戶 (H)
                    concernDetailInfo.FrdIdLogs.AddRange(
                        info.FrdId.Select(item => new Reviewer3rd_FrdIdLog
                        {
                            ApplyNo = context.ApplyNo,
                            UserType = context.UserType,
                            ID = item.ID != null ? item.ID.Trim() : string.Empty,
                            Account = item.Account != null ? item.Account.Trim() : string.Empty,
                            NotifyDate = item.NotifyDate != null ? item.NotifyDate.Trim() : string.Empty,
                            CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                        })
                    );

                    concernDetailInfo.Focus1HitList.Add("H");
                }

                if (info.Fled.Count > 0)
                {
                    // 聯徵資料─行方不明 (D)
                    concernDetailInfo.FledLogs.AddRange(
                        info.Fled.Select(item => new Reviewer3rd_FledLog
                        {
                            ApplyNo = context.ApplyNo,
                            ResidentIdNo = item.ResidentIdNo != null ? item.ResidentIdNo.Trim() : string.Empty,
                            ENName = item.ENName != null ? item.ENName.Trim() : string.Empty,
                            PassportNo = item.PassportNo != null ? item.PassportNo.Trim() : string.Empty,
                            Nationality = item.Nationality != null ? item.Nationality.Trim() : string.Empty,
                            BirthDate = item.BirthDate != null ? item.BirthDate.Trim() : string.Empty,
                            Gender = item.Gender != null ? item.Gender.Trim() : string.Empty,
                            FledDate = item.FledDate != null ? item.FledDate.Trim() : string.Empty,
                            CatchingDate = item.CatchingDate != null ? item.CatchingDate.Trim() : string.Empty,
                            CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                            UserType = context.UserType,
                            ID = context.ID,
                        })
                    );

                    concernDetailInfo.Focus2HitList.Add("D");
                }

                if (info.Punish.Count > 0)
                {
                    // 聯徵資料─收容遣返 (E)
                    concernDetailInfo.PunishLogs.AddRange(
                        info.Punish.Select(item => new Reviewer3rd_PunishLog
                        {
                            ApplyNo = context.ApplyNo,
                            ResidentIdNo = item.ResidentIdNo != null ? item.ResidentIdNo.Trim() : string.Empty,
                            ENName = item.ENName != null ? item.ENName.Trim() : string.Empty,
                            PassportNo = item.PassportNo != null ? item.PassportNo.Trim() : string.Empty,
                            Nationality = item.Nationality != null ? item.Nationality.Trim() : string.Empty,
                            BirthDate = item.BirthDate != null ? item.BirthDate.Trim() : string.Empty,
                            Gender = item.Gender != null ? item.Gender.Trim() : string.Empty,
                            CatchingDate = item.CatchingDate != null ? item.CatchingDate.Trim() : string.Empty,
                            ImmigrateDate = item.ImmigrateDate != null ? item.ImmigrateDate.Trim() : string.Empty,
                            CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                            UserType = context.UserType,
                            ID = context.ID,
                        })
                    );

                    concernDetailInfo.Focus2HitList.Add("E");
                }

                if (info.Immi.Count > 0)
                {
                    // 聯徵資料─出境 (F)
                    concernDetailInfo.ImmiLogs.AddRange(
                        info.Immi.Select(item => new Reviewer3rd_ImmiLog
                        {
                            ApplyNo = context.ApplyNo,
                            ResidentIdNo = item.ResidentIdNo != null ? item.ResidentIdNo.Trim() : string.Empty,
                            ENName = item.ENName != null ? item.ENName.Trim() : string.Empty,
                            PassportNo = item.PassportNo != null ? item.PassportNo.Trim() : string.Empty,
                            Nationality = item.Nationality != null ? item.Nationality.Trim() : string.Empty,
                            BirthDate = item.BirthDate != null ? item.BirthDate.Trim() : string.Empty,
                            Gender = item.Gender != null ? item.Gender.Trim() : string.Empty,
                            ImmigrateDate = item.ImmigrateDate != null ? item.ImmigrateDate.Trim() : string.Empty,
                            InTW = item.InTW != null ? item.InTW.Trim() : string.Empty,
                            CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                            UserType = context.UserType,
                            ID = context.ID,
                        })
                    );

                    concernDetailInfo.Focus2HitList.Add("F");
                }

                if (info.MissingPersons != null)
                {
                    // 失蹤人口 (G)
                    concernDetailInfo.MissingPersonsLogs = new Reviewer3rd_MissingPersonsLog
                    {
                        ApplyNo = context.ApplyNo,
                        ID = context.ID,
                        UserType = context.UserType,
                        YnmpInfo = info.MissingPersons.YnmpInfo != null ? info.MissingPersons.YnmpInfo.Trim() : string.Empty,
                        CreateDate = info.MissingPersons.CreateDate != null ? info.MissingPersons.CreateDate.Trim() : string.Empty,
                    };

                    if (!string.IsNullOrEmpty(concernDetailInfo.MissingPersonsLogs.YnmpInfo) && concernDetailInfo.MissingPersonsLogs.YnmpInfo == "Y")
                    {
                        concernDetailInfo.Focus2HitList.Add("G");
                    }
                }

                if (info.LayOff.Count > 0)
                {
                    // 聯徵資料─解聘 (I)
                    concernDetailInfo.LayOffLogs.AddRange(
                        info.LayOff.Select(item => new Reviewer3rd_LayOffLog
                        {
                            ApplyNo = context.ApplyNo,
                            UserType = context.UserType,
                            ID = context.ID,
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
                        })
                    );

                    concernDetailInfo.Focus2HitList.Add("I");
                }

                result.SetSuccess(concernDetailInfo);
            }
            else
            {
                logger.LogError("查詢關注名單失敗，{@Type}，{@Code}，{@Msg}，{@TraceId}", "查詢關注名單失敗", code, msg, traceId);
                SystemErrorLog dto = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: $"查詢關注名單_回傳結果RtnCode為{code},Msg:{queryConcernDetailResponse.Msg.Trim()}",
                    request: new { Id = context.ID, UserType = context.UserType },
                    response: response
                );
                result.SetError(dto);
            }
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "查詢關注名單_發生例外",
                request: new { Id = context.ID, UserType = context.UserType },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(dto);
        }

        return result;
    }

    public async Task<CheckCaseRes<Query929Info>> 檢核_發查929(CheckJobContext context)
    {
        CheckCaseRes<Query929Info> result = new();
        result.SetStartTime();

        try
        {
            var response = await mw3Adapter.QueryOCSI929(context.ID);

            if (!response.IsSuccess)
            {
                SystemErrorLog dto = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "發查929_回傳結果IsSuccess為N",
                    request: new { Id = context.ID, UserType = context.UserType },
                    response: response
                );

                result.SetError(dto);
                return result;
            }

            var oCSI929ApiResponse = response.Data!;
            var code = oCSI929ApiResponse.RtnCode;
            var msg = oCSI929ApiResponse.Response.Trim();

            if (code == MW3RtnCodeConst.成功)
            {
                Query929Info query929InfoResponse = new()
                {
                    ApplyNo = context.ApplyNo,
                    ID = context.ID,
                    UserType = context.UserType,
                    RtnCode = code,
                    RtnMsg = msg,
                    QueryTime = result.StartTime,
                };

                query929InfoResponse.Reviewer3rd_929Logs.AddRange(
                    oCSI929ApiResponse.Info.Table.Select(item => new Reviewer3rd_929Log
                    {
                        ApplyNo = context.ApplyNo,
                        TxnDate = item.TxnDate != null ? item.TxnDate.Trim() : string.Empty,
                        BrachCode = item.BrachCode != null ? item.BrachCode.Trim() : string.Empty,
                        BrachEmp = item.BrachEmp != null ? item.BrachEmp.Trim() : string.Empty,
                        BusinessCode = item.BusinessCode != null ? item.BusinessCode.Trim() : string.Empty,
                        ChName = item.ChName != null ? item.ChName.Trim() : string.Empty,
                        LoginDate = item.LoginDate != null ? item.LoginDate.Trim() : string.Empty,
                        ApplyCause = item.ApplyCause != null ? item.ApplyCause.Trim() : string.Empty,
                        ApplyReMark = item.ApplyReMark != null ? item.ApplyReMark.Trim() : string.Empty,
                        ID = context.ID,
                        UserType = context.UserType,
                    })
                );

                result.SetSuccess(query929InfoResponse);
            }
            else if (code == MW3RtnCodeConst.查詢929_查無資料)
            {
                Query929Info query929InfoResponse = new()
                {
                    ApplyNo = context.ApplyNo,
                    ID = context.ID,
                    UserType = context.UserType,
                    RtnCode = code,
                    RtnMsg = msg,
                    QueryTime = result.StartTime,
                };

                result.SetSuccess(query929InfoResponse);
            }
            else if (
                code == MW3RtnCodeConst.查詢929_交易有誤
                || code == MW3RtnCodeConst.查詢929_聯絡系統管理員
                || code == MW3RtnCodeConst.查詢929_傳入規格不符合
                || code == MW3RtnCodeConst.查詢929_此服務已失效
            )
            {
                SystemErrorLog dto = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "發查929_回傳結果RtnCode為9999、Er01、Er02、Er03",
                    request: new { Id = context.ID, UserType = context.UserType },
                    response: response
                );

                result.SetError(dto);
            }
            else
            {
                SystemErrorLog dto = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "發查929_回傳結果RtnCode為其他",
                    request: new { Id = context.ID, UserType = context.UserType },
                    response: response
                );

                result.SetError(dto);
            }
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "發查929_發生例外",
                request: new { Id = context.ID, UserType = context.UserType },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );

            result.SetError(dto);
        }

        return result;
    }

    public async Task<CheckCaseRes<QueryCheckName>> 檢核_查詢姓名檢核(CheckJobContext context)
    {
        CheckCaseRes<QueryCheckName> result = new();
        string ulid = Ulid.NewUlid().ToString();
        result.SetStartTime();

        try
        {
            var response = await mW3APAPIAdapter.QueryNameCheck(context.CHName, UserIdConst.ScoreSharpBatch, ulid);

            if (!response.IsSuccess)
            {
                SystemErrorLog dto = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢姓名檢核_回傳結果IsSuccess為N",
                    request: new
                    {
                        name = context.CHName,
                        callUser = UserIdConst.ScoreSharpBatch,
                        traceId = ulid,
                    },
                    response: response
                );

                result.SetError(dto);
                return result;
            }

            var nameCheckData = response.Data!.Info!.Result!.Data;
            var matchResult = nameCheckData.MatchedResult;
            var msg = response.Data!.Response.Trim();

            if (matchResult == MW3RtnCodeConst.查詢姓名檢核_命中 || matchResult == MW3RtnCodeConst.查詢姓名檢核_未命中)
            {
                QueryCheckName queryCheckName = new()
                {
                    ApplyNo = context.ApplyNo,
                    ID = context.ID,
                    UserType = context.UserType,
                    RtnCode = matchResult,
                    RtnMsg = msg,
                    QueryTime = result.StartTime,
                    Reviewer3rd_NameCheckLog = new()
                    {
                        ApplyNo = context.ApplyNo,
                        UserType = context.UserType,
                        ID = context.ID,
                        StartTime = result.StartTime,
                        EndTime = DateTime.Now,
                        ResponseResult = nameCheckData.MatchedResult,
                        RcPoint = int.Parse(nameCheckData.RCScore),
                        AMLId = nameCheckData.AMLReference,
                        TraceId = nameCheckData.ReferenceNumber,
                        Name = context.CHName,
                    },
                };

                result.SetSuccess(queryCheckName);
            }
            else
            {
                SystemErrorLog dto = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: $"查詢姓名檢核_回傳結果MatchResult為其他（{nameCheckData.MatchedResult}）",
                    request: new
                    {
                        name = context.CHName,
                        callUser = UserIdConst.ScoreSharpBatch,
                        traceId = ulid,
                    },
                    response: response
                );

                result.SetError(dto);
            }
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "查詢姓名檢核_發生例外",
                request: new
                {
                    name = context.CHName,
                    callUser = UserIdConst.ScoreSharpBatch,
                    traceId = ulid,
                },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );

            result.SetError(dto);
        }

        return result;
    }

    public async Task<CheckCaseRes<CheckSameIP>> 檢核_相同IP比對(CheckJobContext context, CommonDBDataDto commonDBDataDto)
    {
        /*
            驗證條件
            1. 不為自身案件
            2. 相同IP
            3. 申請時間在24小時內 (要看 SysParam 的設定 IPCompareHour(幾小時))
            4. 不為自身 ID
            5. 查出來的數量如大於等於 SysParam 的設定 IPMatchCount(命中幾次) 為命中
        */

        CheckCaseRes<CheckSameIP> result = new();
        result.SetStartTime();

        try
        {
            List<Reviewer_CheckTrace> traces = new();
            var sameIpEntities = commonDBDataDto
                .HistoryApplyCreditInfo.Where(x => x.ApplyNo != context.ApplyNo)
                .Where(x => x.UserSourceIP == context.UserSourceIP)
                .Where(x =>
                    x.ApplyDate >= context.ApplyDate.AddHours(-commonDBDataDto.SysParam.IPCompareHour)
                    && x.ApplyDate <= context.ApplyDate.AddSeconds(1)
                )
                .Where(x => x.ID != context.ID)
                .ToList();

            bool isSameIP = sameIpEntities.Count >= commonDBDataDto.SysParam.IPMatchCount;

            if (isSameIP)
            {
                var sameIPDetails = sameIpEntities
                    .Select(detail => new Reviewer_CheckTrace
                    {
                        CurrentApplyNo = context.ApplyNo,
                        SameApplyNo = detail.ApplyNo,
                        CurrentID = context.ID,
                        CurrentUserType = context.UserType,
                        CheckType = CheckTraceType.相同IP比對,
                    })
                    .ToList();

                traces = sameIPDetails;
            }

            result.SetSuccess(new() { Reviewer_CheckTraces = traces });
        }
        catch (Exception ex)
        {
            SystemErrorLog error = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "相同IP比對_發生例外",
                request: new { IP = context.UserSourceIP },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(error);
        }

        return result;
    }

    public async Task<CheckCaseRes<CheckSameWebCaseMobile>> 檢核_相同手機號碼比對(CheckJobContext context, CommonDBDataDto commonDBDataDto)
    {
        /*
            驗證條件
            1. 不為自身案件
            2. 相同手機號碼
            3. 申請時間在設定的小時內 (要看 SysParam 的設定 WebCaseMobileCompareHour(幾小時))
            4. 不為自身 ID
            5. 查出來的數量如大於等於 SysParam 的設定 WebCaseMobileMatchCount(命中幾次) 為命中
        */

        CheckCaseRes<CheckSameWebCaseMobile> result = new();
        result.SetStartTime();

        try
        {
            if (string.IsNullOrWhiteSpace(context.Mobile))
            {
                result.SetSuccess(new CheckSameWebCaseMobile() { Reviewer_CheckTraces = [] });
                return result;
            }

            List<Reviewer_CheckTrace> dto = new();
            var sameMobileEntities = commonDBDataDto
                .HistoryApplyCreditInfo.Where(x => x.ApplyNo != context.ApplyNo)
                .Where(x => x.Mobile == context.Mobile && !string.IsNullOrEmpty(x.Mobile))
                .Where(x =>
                    x.ApplyDate >= context.ApplyDate.AddHours(-commonDBDataDto.SysParam.WebCaseMobileCompareHour)
                    && x.ApplyDate <= context.ApplyDate.AddSeconds(1)
                )
                .Where(x => x.ID != context.ID)
                .ToList();

            bool isSameMobile = sameMobileEntities.Count >= commonDBDataDto.SysParam.WebCaseMobileMatchCount;

            if (isSameMobile)
            {
                var sameMobileDetails = sameMobileEntities
                    .Select(detail => new Reviewer_CheckTrace
                    {
                        CurrentApplyNo = context.ApplyNo,
                        SameApplyNo = detail.ApplyNo,
                        CurrentID = context.ID,
                        CurrentUserType = context.UserType,
                        CheckType = CheckTraceType.網路件手機號碼比對,
                    })
                    .ToList();

                dto = sameMobileDetails;
            }

            result.SetSuccess(new CheckSameWebCaseMobile() { Reviewer_CheckTraces = dto });
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "相同手機號碼比對_發生例外",
                request: new { Mobile = context.Mobile },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(dto);
        }

        return result;
    }

    public async Task<CheckCaseRes<CheckSameWebCaseEmail>> 檢核_相同電子郵件比對(CheckJobContext context, CommonDBDataDto commonDBDataDto)
    {
        /*
            驗證條件
            1. 不為自身案件
            2. 相同電子郵件
            3. 申請時間在設定的小時內 (要看 SysParam 的設定 WebCaseEmailCompareHour(幾小時))
            4. 不為自身 ID
            5. 查出來的數量如大於等於 SysParam 的設定 WebCaseEmailMatchCount(命中幾次) 為命中
        */

        CheckCaseRes<CheckSameWebCaseEmail> result = new();
        result.SetStartTime();

        try
        {
            if (string.IsNullOrWhiteSpace(context.EMail))
            {
                result.SetSuccess(new CheckSameWebCaseEmail() { Reviewer_CheckTraces = [] });
                return result;
            }

            List<Reviewer_CheckTrace> dto = new();
            var sameEmailEntities = commonDBDataDto
                .HistoryApplyCreditInfo.Where(x => x.ApplyNo != context.ApplyNo)
                .Where(x => x.EMail == context.EMail && !string.IsNullOrEmpty(x.EMail))
                .Where(x =>
                    x.ApplyDate >= context.ApplyDate.AddHours(-commonDBDataDto.SysParam.WebCaseEmailCompareHour)
                    && x.ApplyDate <= context.ApplyDate.AddSeconds(1)
                )
                .Where(x => x.ID != context.ID)
                .ToList();

            bool isSameEmail = sameEmailEntities.Count >= commonDBDataDto.SysParam.WebCaseEmailMatchCount;

            if (isSameEmail)
            {
                var sameEmailDetails = sameEmailEntities
                    .Select(detail => new Reviewer_CheckTrace
                    {
                        CurrentApplyNo = context.ApplyNo,
                        SameApplyNo = detail.ApplyNo,
                        CurrentID = context.ID,
                        CurrentUserType = context.UserType,
                        CheckType = CheckTraceType.網路件EMAIL比對,
                    })
                    .ToList();

                dto = sameEmailDetails;
            }

            result.SetSuccess(new CheckSameWebCaseEmail() { Reviewer_CheckTraces = dto });
        }
        catch (Exception ex)
        {
            SystemErrorLog error = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "相同電子郵件比對_發生例外",
                request: new { Email = context.EMail },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(error);
        }

        return result;
    }

    public async Task<CheckCaseRes<CheckShortTimeID>> 檢核_短時間ID相同比對(CheckJobContext context, CommonDBDataDto commonDBDataDto)
    {
        /*
            驗證條件：
            檢核申請時間前 「短時間ID比對時間(小時)」 小時內的案件，比對ID次數 >= 「短時間ID比對吻合次數」
        */

        CheckCaseRes<CheckShortTimeID> result = new();
        result.SetStartTime();

        try
        {
            List<Reviewer_CheckTrace> dto = new();
            var shortTimeIDEntities = commonDBDataDto
                .HistoryApplyCreditInfo.Where(x => x.ApplyNo != context.ApplyNo)
                .Where(x => x.ID == context.ID)
                .Where(x =>
                    x.ApplyDate >= context.ApplyDate.AddHours(-commonDBDataDto.SysParam.ShortTimeIDCompareHour) && x.ApplyDate <= context.ApplyDate
                )
                .ToList();

            bool isShortTimeID = shortTimeIDEntities.Count >= commonDBDataDto.SysParam.ShortTimeIDMatchCount;

            if (isShortTimeID)
            {
                var shortTimeIDDetails = shortTimeIDEntities
                    .Select(detail => new Reviewer_CheckTrace()
                    {
                        CurrentApplyNo = context.ApplyNo,
                        SameApplyNo = detail.ApplyNo,
                        CurrentID = detail.ID,
                        CurrentUserType = context.UserType,
                        CheckType = CheckTraceType.短時間頻繁ID比對,
                    })
                    .ToList();
                dto = shortTimeIDDetails;
            }

            result.SetSuccess(new() { Reviewer_CheckTraces = dto });
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "短時間ID相同比對_發生例外",
                request: new { ID = context.ID },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(dto);
        }

        return result;
    }

    public async Task<CheckCaseRes<bool>> 檢核_行內IP相同(CheckJobContext context, CommonDBDataDto commonDBDataDto)
    {
        CheckCaseRes<bool> result = new();
        result.SetStartTime();

        try
        {
            bool isContainInternalIP = commonDBDataDto.InternalIPs.Contains(context.UserSourceIP);
            result.SetSuccess(isContainInternalIP);
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "檢核_行內IP相同_發生例外",
                request: new { IP = context.UserSourceIP },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(dto);
        }

        return result;
    }

    public string 計算郵遞區號(AddressContext address, List<SetUp_AddressInfo> addressInfos)
    {
        var filterAddressInfos = addressInfos.Where(x => x.City == address.City && x.Area == address.District && x.Road == address.Road).ToList();

        if (filterAddressInfos.Count == 0 || filterAddressInfos is null)
        {
            logger.LogError("縣市：{city} 區域：{district} 街道：{road} 查無郵遞區號", address.City, address.District, address.Road);
            return "";
        }

        var convertAddressInfos = filterAddressInfos
            .Select(x => new AddressInfoDto()
            {
                City = address.City,
                Area = address.District,
                Road = address.Road,
                Scope = x.Scope,
                ZipCode = x.ZIPCode,
            })
            .ToList();

        if (!int.TryParse(address.Number, out int number))
        {
            logger.LogError("門牌號碼：{number} 無法轉換為數字", address.Number);
            return "";
        }

        var searchAddressInfo = new SearchAddressInfoDto
        {
            City = address.City,
            District = address.District,
            Road = address.Road,
            Number = number,
            SubNumber = int.TryParse(address.SubNumber, out int subNumber) ? subNumber : 0,
            Lane = int.TryParse(address.Lane, out int lane) ? lane : 0,
        };

        var zipCode = AddressHelper.FindZipCode(convertAddressInfos, searchAddressInfo);

        if (string.IsNullOrEmpty(zipCode))
        {
            logger.LogError(
                "縣市：{city} 區域：{district} 街道：{road} 門牌號碼：{number} 無法轉換郵遞區號",
                address.City,
                address.District,
                address.Road,
                address.Number
            );
        }

        return zipCode;
    }

    public async Task 通知_檢核異常信件(string applyNo, string type, string errorMessage, string errorDetail = "")
    {
        efContext.ChangeTracker.Clear();

        await efContext.System_ErrorLog.AddAsync(
            new System_ErrorLog()
            {
                ApplyNo = applyNo,
                Project = SystemErrorLogProjectConst.BATCH,
                Source = "EcardNotA02CheckNewCase",
                ErrorMessage = errorMessage,
                ErrorDetail = errorDetail,
                AddTime = DateTime.Now,
                Type = type,
                SendStatus = SendStatus.等待,
            }
        );

        await efContext.SaveChangesAsync();
    }

    private SystemErrorLog CreateErrorLog(
        string applyNo,
        string title,
        string type = SystemErrorLogTypeConst.第三方API執行錯誤,
        object? request = null,
        object? response = null,
        Exception? exception = null
    )
    {
        return new()
        {
            ApplyNo = applyNo,
            Project = SystemErrorLogProjectConst.BATCH,
            Source = "EcardNotA02CheckNewCase",
            Type = type,
            ErrorMessage = exception is null ? title : $"{title}：{exception?.Message}",
            ErrorDetail = exception is null ? string.Empty : exception.ToString(),
            Request = request != null ? JsonHelper.序列化物件(request) : string.Empty,
            Response = response != null ? JsonHelper.序列化物件(response) : string.Empty,
        };
    }

    public async Task<int> 案件異常處理(string applyNo, string type, string errorTitle, Exception ex)
    {
        efContext.ChangeTracker.Clear();
        DateTime now = DateTime.Now;

        var handleInfo = await efContext.Reviewer_ApplyCreditCardInfoHandle.FirstOrDefaultAsync(x => x.ApplyNo == applyNo);
        handleInfo.CardStatus = CardStatus.網路件_待月收入預審_檢核異常;

        // 單筆案件解鎖並且錯誤次數加1
        var pedding = await efContext.ReviewerPedding_WebApplyCardCheckJobForA02.FirstOrDefaultAsync(x => x.ApplyNo == applyNo);
        pedding.ErrorCount++;

        await efContext.Reviewer_ApplyCreditCardInfoProcess.AddAsync(
            MapHelper.MapToProcess(applyNo: applyNo, action: CardStatus.網路件_待月收入預審_檢核異常.ToString(), startTime: now, endTime: now)
        );

        await efContext.System_ErrorLog.AddAsync(
            new System_ErrorLog()
            {
                ApplyNo = applyNo,
                Project = SystemErrorLogProjectConst.BATCH,
                Source = "EcardNotA02CheckNewCase",
                Type = type,
                ErrorMessage = errorTitle,
                ErrorDetail = ex.ToString(),
                AddTime = now,
                SendStatus = SendStatus.等待,
            }
        );

        await efContext.SaveChangesAsync();

        return pedding.ErrorCount;
    }

    public async Task 寄信給達錯誤2次案件(List<string> applyNo)
    {
        efContext.ChangeTracker.Clear();

        var errorLogs = applyNo.Select(x => new System_ErrorLog()
        {
            ApplyNo = x,
            Project = SystemErrorLogProjectConst.BATCH,
            Source = "EcardA02CheckNewCase",
            Type = SystemErrorLogTypeConst.達錯誤2次案件需寄信,
            ErrorMessage = "達錯誤2次案件需寄信",
            ErrorDetail = "達錯誤2次案件需寄信",
            AddTime = DateTime.Now,
            SendStatus = SendStatus.等待,
        });

        await efContext.System_ErrorLog.AddRangeAsync(errorLogs);

        await efContext.SaveChangesAsync();
    }

    public async Task<CheckCaseRes<CheckInternalEmailSameResult>> 檢核_行內Email資料(CheckJobContext context)
    {
        CheckCaseRes<CheckInternalEmailSameResult> result = new();
        result.SetStartTime();

        if (string.IsNullOrWhiteSpace(context.EMail))
        {
            result.SetSuccess(new CheckInternalEmailSameResult() { BankInternalSameLogs = [] });
            return result;
        }

        try
        {
            var response = await procAdapter.QueryEBill(email: context.EMail);

            if (!response.IsSuccess)
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢電子帳單For行內Email_回傳結果IsSuccess為N",
                    request: new { Email = context.EMail },
                    response: response
                );

                result.SetError(error);

                return result;
            }

            var queryEBillData = response.Data!;

            if (queryEBillData.RtnCode == MW3RtnCodeConst.查詢電子帳單_該信箱已存在)
            {
                var ebillDataList = new List<Reviewer_BankInternalSameLog>();
                foreach (var data in queryEBillData.Info.Table.Where(x => x.ID != context.ID))
                {
                    var cardholderResponse = await procAdapter.QueryOriginalCardholderData(id: data.ID);
                    var billAddr = cardholderResponse.Data?.Info?.Table?.FirstOrDefault()?.BillAddr;

                    var ebillData = new Reviewer_BankInternalSameLog()
                    {
                        ApplyNo = context.ApplyNo,
                        ID = context.ID,
                        UserType = context.UserType,
                        SameID = data.ID,
                        SameName = data.Name,
                        CheckType = BankInternalSameCheckType.Email,
                        SameBillAddr = billAddr,
                    };

                    ebillDataList.Add(ebillData);
                }

                result.SetSuccess(new CheckInternalEmailSameResult() { BankInternalSameLogs = ebillDataList });
            }
            else if (queryEBillData.RtnCode == MW3RtnCodeConst.查詢電子帳單_無相同信箱會員)
            {
                result.SetSuccess(new CheckInternalEmailSameResult() { BankInternalSameLogs = [] });
            }
            else
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢電子帳單For行內Email_回傳結果RtnCode為其他",
                    request: new { Email = context.EMail },
                    response: response
                );
                result.SetError(error);
            }
        }
        catch (Exception ex)
        {
            SystemErrorLog error = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "查詢電子帳單For行內Email_發生例外",
                request: new { Email = context.EMail },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );

            result.SetError(error);
        }
        return result;
    }

    public async Task<CheckCaseRes<CheckInternalMobileSameResult>> 檢核_行內Mobile資料(CheckJobContext context)
    {
        CheckCaseRes<CheckInternalMobileSameResult> result = new();
        result.SetStartTime();

        if (string.IsNullOrWhiteSpace(context.Mobile))
        {
            result.SetSuccess(new CheckInternalMobileSameResult() { BankInternalSameLogs = [] });
            return result;
        }

        try
        {
            var response = await procAdapter.QueryOriginalCardholderData(id: "", email: "", mobile: context.Mobile);

            if (!response.IsSuccess)
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢原持卡人資料For行內Mobile_回傳結果IsSuccess為N",
                    request: new { Mobile = context.Mobile },
                    response: response
                );

                result.SetError(error);

                return result;
            }

            var queryOriginalCardholderData = response.Data!;

            if (queryOriginalCardholderData.RtnCode == MW3RtnCodeConst.成功)
            {
                var originalCardholderData = queryOriginalCardholderData
                    .Info.Table.Select(x => MapHelper.MapToQueryOriginalCardholderData(x))
                    .Where(x => x.ID != context.ID)
                    .Select(x => new Reviewer_BankInternalSameLog()
                    {
                        ApplyNo = context.ApplyNo,
                        ID = context.ID,
                        UserType = context.UserType,
                        SameID = x.ID,
                        SameName = x.ChineseName,
                        CheckType = BankInternalSameCheckType.手機,
                        SameBillAddr = x.BillAddr,
                    })
                    .ToList();
                result.SetSuccess(new CheckInternalMobileSameResult() { BankInternalSameLogs = originalCardholderData });
            }
            else if (queryOriginalCardholderData.RtnCode == MW3RtnCodeConst.查詢原持卡人資料_查無資料)
            {
                result.SetSuccess(new CheckInternalMobileSameResult() { BankInternalSameLogs = [] });
            }
            else
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢原持卡人資料For行內Mobile_回傳結果RtnCode為其他",
                    request: new { Mobile = context.Mobile },
                    response: response
                );
                result.SetError(error);
            }
        }
        catch (Exception ex)
        {
            SystemErrorLog error = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "查詢原持卡人資料For行內Mobile_發生例外",
                request: new { Mobile = context.Mobile },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(error);
        }

        return result;
    }

    public async Task<CheckCaseRes<bool>> 檢查_是否為重覆進件(CheckJobContext context)
    {
        CheckCaseRes<bool> result = new();
        result.SetStartTime();

        try
        {
            bool isContainID = await efContext.Reviewer_ApplyCreditCardInfoHandle.AnyAsync(x => x.ID == context.ID && x.ApplyNo != context.ApplyNo);
            result.SetSuccess(isContainID);
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "檢核_是否為重覆進件_發生例外",
                request: new { ApplyNo = context.ApplyNo },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(dto);
        }

        return result;
    }
}
