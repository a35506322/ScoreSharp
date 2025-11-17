using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Helpers;
using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase;

public class EcardA02CheckNewCaseService(
    ScoreSharpContext efContext,
    ILogger<EcardA02CheckNewCaseService> logger,
    IEcardA02CheckNewCaseRepository repository,
    IMW3ProcAdapter procAdapter,
    IMW3MSAPIAdapter mw3MSAdapter
) : IEcardA02CheckNewCaseService
{
    public async Task<CheckCaseRes<QueryOriginalCardholderData>> 檢核_原持卡人資料(CheckA02JobContext context)
    {
        CheckCaseRes<QueryOriginalCardholderData> result = new();
        result.SetStartTime();

        try
        {
            var response = await procAdapter.QueryOriginalCardholderData(id: context.ID, email: "", mobile: "");

            if (!response.IsSuccess)
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢原持卡人資料_回傳結果IsSuccess為N",
                    request: new { Id = context.ID, UserType = context.UserType },
                    response: response
                );

                result.SetError(error);

                return result;
            }

            var queryOriginalCardholderData = response.Data!;

            if (queryOriginalCardholderData.RtnCode == MW3RtnCodeConst.成功)
            {
                var originalCardholderData = queryOriginalCardholderData.Info.Table.FirstOrDefault();
                QueryOriginalCardholderData data = MapHelper.MapToQueryOriginalCardholderData(originalCardholderData);
                result.SetSuccess(data);
            }
            else if (queryOriginalCardholderData.RtnCode == MW3RtnCodeConst.查詢原持卡人資料_查無資料)
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢原持卡人資料_回傳結果RtnCode為查無資料",
                    request: new { Id = context.ID, UserType = context.UserType },
                    response: response
                );

                result.SetError(error);
            }
            else
            {
                SystemErrorLog error = CreateErrorLog(
                    applyNo: context.ApplyNo,
                    title: "查詢原持卡人資料_回傳結果RtnCode為其他",
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
                title: "查詢原持卡人資料_發生例外",
                request: new { Id = context.ID, UserType = context.UserType },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );

            result.SetError(error);
        }

        return result;
    }

    public async Task<CheckCaseRes<Check929Info>> 檢核_發查929(CheckA02JobContext context)
    {
        CheckCaseRes<Check929Info> result = new();
        result.SetStartTime();

        try
        {
            var response = await procAdapter.QueryOCSI929(context.ID);

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
                Check929Info query929InfoResponse = new()
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
                Check929Info query929InfoResponse = new()
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

    public async Task<CheckCaseRes<CheckInternalEmailSameResult>> 檢核_行內Email資料(CheckA02JobContext context)
    {
        CheckCaseRes<CheckInternalEmailSameResult> result = new();
        result.SetStartTime();

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

    public async Task<CheckCaseRes<CheckInternalMobileSameResult>> 檢核_行內Mobile資料(CheckA02JobContext context)
    {
        CheckCaseRes<CheckInternalMobileSameResult> result = new();
        result.SetStartTime();

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

    public async Task 通知_檢核異常信件(string applyNo, string type, string errorMessage, string errorDetail = "")
    {
        efContext.ChangeTracker.Clear();

        await efContext.System_ErrorLog.AddAsync(
            new System_ErrorLog()
            {
                ApplyNo = applyNo,
                Project = SystemErrorLogProjectConst.BATCH,
                Source = "EcardA02CheckNewCase",
                ErrorMessage = errorMessage,
                ErrorDetail = errorDetail,
                AddTime = DateTime.Now,
                Type = type,
                SendStatus = SendStatus.等待,
            }
        );

        await efContext.SaveChangesAsync();
    }

    public async Task<CheckCaseRes<bool>> 檢核_行內IP相同(CheckA02JobContext context, CommonDBDataDto commonDBDataDto)
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
            Source = "EcardA02CheckNewCase",
            Type = type,
            ErrorMessage = exception is null ? title : $"{title}：{exception?.Message}",
            ErrorDetail = exception is null ? string.Empty : exception.ToString(),
            Request = request != null ? JsonHelper.序列化物件(request) : string.Empty,
            Response = response != null ? JsonHelper.序列化物件(response) : string.Empty,
        };
    }

    public async Task<CheckCaseRes<CheckSameIP>> 檢核_IP比對相同(CheckA02JobContext context)
    {
        CheckCaseRes<CheckSameIP> result = new();
        result.SetStartTime();

        try
        {
            var response = await repository.查詢_IP比對相同(context.ApplyNo);
            result.SetSuccess(
                new CheckSameIP()
                {
                    Reviewer_CheckTraces = response
                        .HitApplyNoInfos.Select(x => new Reviewer_CheckTrace()
                        {
                            SameApplyNo = x.SameApplyNo,
                            CurrentID = x.CurrentID,
                            CurrentUserType = x.CurrentUserType,
                            CheckType = x.CheckType,
                            CurrentApplyNo = x.CurrentApplyNo,
                        })
                        .ToList(),
                }
            );
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "檢核_IP比對相同_發生例外",
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(dto);
        }

        return result;
    }

    public async Task<CheckCaseRes<CheckSameWebCaseEmail>> 檢核_網路電子郵件相同(CheckA02JobContext context)
    {
        CheckCaseRes<CheckSameWebCaseEmail> result = new();
        result.SetStartTime();

        try
        {
            var response = await repository.查詢_網路電子郵件相同(context.ApplyNo);
            result.SetSuccess(
                new CheckSameWebCaseEmail()
                {
                    Reviewer_CheckTraces = response
                        .HitApplyNoInfos.Select(x => new Reviewer_CheckTrace()
                        {
                            SameApplyNo = x.SameApplyNo,
                            CurrentID = x.CurrentID,
                            CurrentUserType = x.CurrentUserType,
                            CheckType = x.CheckType,
                            CurrentApplyNo = x.CurrentApplyNo,
                        })
                        .ToList(),
                }
            );
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "檢核_網路電子郵件相同_發生例外",
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(dto);
        }

        return result;
    }

    public async Task<CheckCaseRes<CheckSameWebMobile>> 檢核_網路手機相同(CheckA02JobContext context)
    {
        CheckCaseRes<CheckSameWebMobile> result = new();
        result.SetStartTime();

        try
        {
            var response = await repository.查詢_網路手機相同(context.ApplyNo);
            result.SetSuccess(
                new CheckSameWebMobile()
                {
                    Reviewer_CheckTraces = response
                        .HitApplyNoInfos.Select(x => new Reviewer_CheckTrace()
                        {
                            SameApplyNo = x.SameApplyNo,
                            CurrentID = x.CurrentID,
                            CurrentUserType = x.CurrentUserType,
                            CheckType = x.CheckType,
                            CurrentApplyNo = x.CurrentApplyNo,
                        })
                        .ToList(),
                }
            );
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "檢核_網路手機相同_發生例外",
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(dto);
        }

        return result;
    }

    public async Task<CheckCaseRes<CheckShortTimeID>> 檢核_頻繁ID(CheckA02JobContext context)
    {
        CheckCaseRes<CheckShortTimeID> result = new();
        result.SetStartTime();

        try
        {
            var response = await repository.查詢_頻繁ID(context.ApplyNo);
            result.SetSuccess(
                new CheckShortTimeID()
                {
                    Reviewer_CheckTraces = response
                        .HitApplyNoInfos.Select(x => new Reviewer_CheckTrace()
                        {
                            SameApplyNo = x.SameApplyNo,
                            CurrentID = x.CurrentID,
                            CurrentUserType = x.CurrentUserType,
                            CheckType = x.CheckType,
                            CurrentApplyNo = x.CurrentApplyNo,
                        })
                        .ToList(),
                }
            );
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: context.ApplyNo,
                title: "檢核_頻繁ID_發生例外",
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(dto);
        }

        return result;
    }

    public async Task<CheckCaseRes<ConcernDetailInfo>> 檢核_查詢關注名單(CheckA02JobContext context)
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
            var msg = queryConcernDetailResponse.Msg.Trim();
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

    public async Task<int> 案件異常處理(string applyNo, string type, string errorTitle, Exception ex)
    {
        logger.LogInformation("Service Context HashCode: {hashCode}", efContext.GetHashCode());

        efContext.ChangeTracker.Clear();
        DateTime now = DateTime.Now;

        var handleInfo = await efContext.Reviewer_ApplyCreditCardInfoHandle.FirstOrDefaultAsync(x => x.ApplyNo == applyNo);
        handleInfo.CardStatus = CardStatus.網路件_卡友_檢核異常;

        // 單筆案件解鎖並且錯誤次數加1
        var pedding = await efContext.ReviewerPedding_WebApplyCardCheckJobForA02.FirstOrDefaultAsync(x => x.ApplyNo == applyNo);
        pedding.ErrorCount++;

        await efContext.Reviewer_ApplyCreditCardInfoProcess.AddAsync(
            MapHelper.MapToProcess(applyNo: applyNo, action: CardStatus.網路件_卡友_檢核異常.ToString(), startTime: now, endTime: now)
        );

        await efContext.System_ErrorLog.AddAsync(
            new System_ErrorLog()
            {
                ApplyNo = applyNo,
                Project = SystemErrorLogProjectConst.BATCH,
                Source = "EcardA02CheckNewCase",
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
        logger.LogInformation("Service Context HashCode: {hashCode}", efContext.GetHashCode());

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

    public async Task<CheckCaseRes<bool>> 檢查_是否為重覆進件(CheckA02JobContext context)
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

    public CheckCaseRes<string> 計算_郵遞區號(string fullAddress, CommonDBDataDto commonDBDataDto, string applyNo)
    {
        CheckCaseRes<string> result = new();
        result.SetStartTime();

        try
        {
            var addressInfos = commonDBDataDto.AddressInfos;
            var (city, area) = AddressHelper.GetCityAndDistrict(fullAddress);
            var addressInfo = addressInfos.FirstOrDefault(x => x.City == city && x.Area == area);

            if (addressInfo == null)
                throw new Exception($"郵遞區號計算失敗，找不到地址資料，{fullAddress}");

            var zipCode = AddressHelper.ZipCodeFormatZero(addressInfo.ZIPCode, 2);

            result.SetSuccess(zipCode);
        }
        catch (Exception ex)
        {
            SystemErrorLog dto = CreateErrorLog(
                applyNo: applyNo,
                title: "計算_郵遞區號_發生例外",
                request: new { ApplyNo = applyNo, FullAddress = fullAddress },
                exception: ex,
                type: SystemErrorLogTypeConst.內部程式錯誤
            );
            result.SetError(dto);
        }

        return result;
    }
}
