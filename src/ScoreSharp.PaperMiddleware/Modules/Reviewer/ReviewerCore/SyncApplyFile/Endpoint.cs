using System;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyFile;

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 圖檔傳送 API
        /// </summary>
        /// <param name="request"></param>
        /// <param name="applyNo">申請書編號</param>
        /// <param name="syncUserId">同步員工編號</param>
        /// <response code="200">
        /// Return Code = 2000 同步成功
        /// </response>
        /// <response code="401">
        /// Return Code = 4004 標頭驗證失敗
        /// </response>
        /// <response code="400">
        /// Return Code = 4000 格式驗證失敗
        /// Return Code = 4003 商業邏輯有誤
        /// - 紙本初始 = 1
        ///     檢查是否已存在這筆申請書編號
        /// - 網路小白件 = 3
        ///     檢查CardStatus是20012(書面申請等待MyData)、20014 (書面申請等待列印申請書及回郵信封)
        /// </response>
        /// <response code="404">
        /// Return Code = 4002 查無此資料
        /// - 補件 = 2
        ///     檢查是否存在這筆申請書編號
        /// - 網路小白件 = 3
        ///     檢查是否存在這筆申請書編號
        /// </response>
        /// <response code="500">
        /// Return Code = 5000 內部程式失敗
        /// Return Code = 5002 資料庫執行失敗
        /// </response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [EndpointSpecificExample(
            typeof(圖檔傳送_紙本初始_200_2000_ReqEx),
            typeof(圖檔傳送_紙本補件_200_2000_ReqEx),
            typeof(圖檔傳送_網路小白_200_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(圖檔傳送_同步成功_200_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [EndpointSpecificExample(typeof(圖檔傳送_格式驗證失敗_400_4000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(圖檔傳送_格式驗證失敗_400_4000_ResEx),
            typeof(圖檔傳送_紙本初始_資料已存在_400_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [EndpointSpecificExample(
            typeof(圖檔傳送_表頭驗證失敗_401_4004_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status401Unauthorized
        )]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [EndpointSpecificExample(
            typeof(圖檔傳送_紙本補件_查無資料_404_4002_ResEx),
            typeof(圖檔傳送_網路小白_查無資料_404_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status404NotFound
        )]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [EndpointSpecificExample(
            typeof(圖檔傳送_內部程式失敗_500_5000_ResEx),
            typeof(圖檔傳送_資料庫執行失敗_502_5002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status500InternalServerError
        )]
        [OpenApiOperation("SyncApplyFile")]
        public async Task<IResult> SyncApplyFile(
            [FromHeader(Name = "X-APPLYNO")] string applyNo,
            [FromHeader(Name = "X-SYNCUSERID")] string syncUserId,
            [FromBody] SyncApplyFileRequest request
        ) => Results.Ok(await _mediator.Send(new Command(request, ModelState)));
    }
}

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyFile
{
    public record Command(SyncApplyFileRequest syncApplyFileRequest, ModelStateDictionary modelState) : IRequest<ResultResponse<string>>;

    public class Handler(ILogger<Handler> logger, ScoreSharpContext scoreSharpContext, ScoreSharpFileContext scoreSharpFileContext)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            /*
             * 1. 針對各同步狀態檢查
             *  - 紙本初始 = 1
             *      檢查Main是否存在這筆申請書編號
             *  - 補件 = 2
             *      檢查Main有沒有這筆申請書編號
             *  - 網路小白件 = 3
             *      檢查Handle 、Main  有沒有這筆申請書編號
             *      檢查Handle 的 CardStatus是20012 (書面申請等待MyData)、20014 ( 書面申請等待列印申請書及回郵信封)
             * 2. 新增 Process、File、ApplyFile
             */

            var req = request.syncApplyFileRequest;

            資料驗證格式(request.modelState);

            var main = await 查詢主檔資料(req.ApplyNo);

            var handles = await 查詢處理檔資料(req.ApplyNo);

            驗證商業邏輯(req, main, handles);

            await 資料庫分散式交易(req, main, handles);

            return ApiResponseHelper.Success(data: req.ApplyNo, message: $"同步成功: {req.ApplyNo}");
        }

        private void 資料驗證格式(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                throw new BadRequestException(modelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()));
            }
        }

        private async Task<Reviewer_ApplyCreditCardInfoMain?> 查詢主檔資料(string applyNo) =>
            await scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.AsNoTracking().FirstOrDefaultAsync(x => x.ApplyNo == applyNo);

        private async Task<List<Reviewer_ApplyCreditCardInfoHandle>?> 查詢處理檔資料(string applyNo) =>
            await scoreSharpContext.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == applyNo).ToListAsync();

        private void 驗證商業邏輯(SyncApplyFileRequest req, Reviewer_ApplyCreditCardInfoMain? main, List<Reviewer_ApplyCreditCardInfoHandle>? handle)
        {
            if (req.SyncStatus == SyncFileStatus.紙本初始)
            {
                if (main is not null)
                {
                    throw new BusinessBadRequestException($"申請書編號 {req.ApplyNo} 已存在於主檔中。");
                }
            }
            else if (req.SyncStatus == SyncFileStatus.補件)
            {
                if (main is null)
                {
                    throw new NotFoundException($"申請書編號 {req.ApplyNo} 不存在於主檔中。");
                }
            }
            else if (req.SyncStatus == SyncFileStatus.網路小白件)
            {
                if (handle is null || main is null)
                {
                    throw new NotFoundException($"申請書編號 {req.ApplyNo} 不存在於主檔或處理檔中。");
                }
                else if (
                    handle.FirstOrDefault().CardStatus != CardStatus.網路件_書面申請等待MyData
                    && handle.FirstOrDefault().CardStatus != CardStatus.網路件_書面申請等待列印申請書及回郵信封
                )
                {
                    throw new BusinessBadRequestException($"申請書編號 {req.ApplyNo} 的處理狀態不符合要求。");
                }
            }
        }

        private async Task<string> 取得AMLProfessionCode_Version()
        {
            var systemParams = await scoreSharpContext.SysParamManage_SysParam.AsNoTracking().FirstOrDefaultAsync();
            return systemParams.AMLProfessionCode_Version;
        }

        private string 產生ProcessString(SyncApplyFileRequest req, List<Reviewer_ApplyCreditCardInfoHandle> handles)
        {
            if (req.SyncStatus == SyncFileStatus.紙本初始)
            {
                return CardStatus.紙本件_初始.ToString();
            }
            else if (req.SyncStatus == SyncFileStatus.補件)
            {
                return CardStatus.補回件.ToString();
            }
            else if (req.SyncStatus == SyncFileStatus.網路小白件)
            {
                return handles.FirstOrDefault().CardStatus.ToString();
            }
            return string.Empty;
        }

        private async Task 資料庫分散式交易(
            SyncApplyFileRequest req,
            Reviewer_ApplyCreditCardInfoMain main,
            List<Reviewer_ApplyCreditCardInfoHandle> handles
        )
        {
            var now = DateTime.Now;
            List<Reviewer_CardRecord> cardRecords = new();
            List<Reviewer_ApplyCreditCardInfoProcess> processes = new();
            List<Reviewer_ApplyFile> reviewerApplyFiles = new();
            List<Reviewer_ApplyCreditCardInfoFile> reviewerApplyCreditCardInfoFiles = new();
            Reviewer_ApplyCreditCardInfoMain? entityMain = null;

            // 新增檔案跟檔案歷程
            int maxPage = await scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.Where(x => x.ApplyNo == req.ApplyNo).CountAsync();
            int pageCount = 1;

            // TODO: 2025.09004 浮水印加工待確認
            foreach (var file in req.ApplyFiles)
            {
                var guid = Guid.NewGuid();
                // TODO: 2025.09.04 檔案命名規則待確認，暫定為 {ApplyNo}_{檔名}
                var fileName = $"{req.ApplyNo}_{file.FileName}";

                reviewerApplyFiles.Add(
                    new Reviewer_ApplyFile
                    {
                        ApplyNo = req.ApplyNo,
                        FileId = guid,
                        FileName = fileName,
                        FileContent = file.FileContent,
                        FileType = FileType.申請書相關,
                    }
                );

                reviewerApplyCreditCardInfoFiles.Add(
                    new Reviewer_ApplyCreditCardInfoFile
                    {
                        ApplyNo = req.ApplyNo,
                        Page = maxPage + pageCount,
                        Process = 產生ProcessString(req, handles),
                        AddTime = now,
                        AddUserId = req.SyncUserId,
                        Note = $"完成附件補檔FROM Paper;紙本FileId:{file.FileId};",
                        FileId = guid,
                        DBName = "ScoreSharp_File",
                    }
                );

                pageCount++;
            }

            // 如果紙本初始，則需要新增主檔資料
            if (req.SyncStatus == SyncFileStatus.紙本初始)
            {
                var currentVersion = await 取得AMLProfessionCode_Version();
                entityMain = new Reviewer_ApplyCreditCardInfoMain
                {
                    ApplyNo = req.ApplyNo,
                    UserType = UserType.正卡人,
                    Source = Source.紙本,
                    ApplyDate = now,
                    CaseType = CaseType.一般件,
                    IsHistory = "N",
                    LastUpdateUserId = UserIdConst.SYSTEM,
                    LastUpdateTime = now,

                    // 2025.07.22 以當前系統參數之 AMLProfessionCode_Version 為主
                    AMLProfessionCode_Version = currentVersion,
                };
            }

            // 產生 Process
            processes.Add(
                new Reviewer_ApplyCreditCardInfoProcess
                {
                    ApplyNo = req.ApplyNo,
                    Process = 產生ProcessString(req, handles),
                    ProcessUserId = req.SyncUserId,
                    StartTime = now,
                    EndTime = now,
                    Notes = $"完成附件補檔FROM Paper;紙本FileId:{string.Join(',', req.ApplyFiles.Select(x => x.FileId).ToList())};",
                }
            );

            if (req.SyncStatus == SyncFileStatus.補件 && main.Source == Source.紙本)
            {
                var filterHandles = handles.Where(x => x.CardStatus == CardStatus.補件作業中).ToList();
                if (filterHandles.Count > 0)
                {
                    var afterCardStatus = filterHandles.Any(x => x.CardStep == CardStep.人工徵審)
                        ? CardStatus.補回件
                        : CardStatus.紙本件_待月收入預審;

                    processes.AddRange(
                        filterHandles
                            .Select(handle => new Reviewer_ApplyCreditCardInfoProcess
                            {
                                ApplyNo = req.ApplyNo,
                                Process = afterCardStatus.ToString(),
                                ProcessUserId = req.SyncUserId,
                                StartTime = now.AddSeconds(1),
                                EndTime = now.AddSeconds(1),
                                Notes = $"完成附件補檔FROM Paper;({(handle.UserType == UserType.正卡人 ? "正卡" : "附卡")}_{handle.ApplyCardType})",
                            })
                            .ToList()
                    );
                }
            }
            else if (req.SyncStatus == SyncFileStatus.補件 && main.Source != Source.紙本)
            {
                // Tips: 網路件目前只有一筆
                var handle = handles.FirstOrDefault();
                if (handle.CardStatus == CardStatus.補件作業中)
                {
                    var afterCardStatus = handle.CardStep == CardStep.人工徵審 ? CardStatus.補回件 : CardStatus.網路件_待月收入預審;
                    processes.Add(
                        new Reviewer_ApplyCreditCardInfoProcess
                        {
                            ApplyNo = req.ApplyNo,
                            Process = afterCardStatus.ToString(),
                            ProcessUserId = req.SyncUserId,
                            StartTime = now.AddSeconds(1),
                            EndTime = now.AddSeconds(1),
                            Notes = $"完成附件補檔FROM Paper;({(handle.UserType == UserType.正卡人 ? "正卡" : "附卡")}_{handle.ApplyCardType})",
                        }
                    );
                }
            }

            // 產生 CardRecord
            if (req.SyncStatus == SyncFileStatus.補件 && main.Source == Source.紙本)
            {
                var filterHandles = handles.Where(x => x.CardStatus == CardStatus.補件作業中).ToList();
                if (filterHandles.Count > 0)
                {
                    var afterCardStatus = filterHandles.Any(x => x.CardStep == CardStep.人工徵審)
                        ? CardStatus.補回件
                        : CardStatus.紙本件_待月收入預審;

                    cardRecords.AddRange(
                        filterHandles
                            .Select(handle => new Reviewer_CardRecord
                            {
                                ApplyNo = req.ApplyNo,
                                CardStatus = afterCardStatus,
                                ApproveUserId = req.SyncUserId,
                                AddTime = now.AddSeconds(1),
                                HandleNote =
                                    $"完成附件補檔FROM Paper;({(handle.UserType == UserType.正卡人 ? "正卡" : "附卡")}_{handle.ApplyCardType})",
                                HandleSeqNo = handle.SeqNo,
                            })
                            .ToList()
                    );
                }
            }
            else if (req.SyncStatus == SyncFileStatus.補件 && main.Source != Source.紙本)
            {
                // Tips: 網路件目前只有一筆
                var handle = handles.FirstOrDefault();
                if (handle.CardStatus == CardStatus.補件作業中)
                {
                    var afterCardStatus = handle.CardStep == CardStep.人工徵審 ? CardStatus.補回件 : CardStatus.網路件_待月收入預審;
                    cardRecords.Add(
                        new Reviewer_CardRecord
                        {
                            ApplyNo = req.ApplyNo,
                            CardStatus = afterCardStatus,
                            ApproveUserId = req.SyncUserId,
                            AddTime = now.AddSeconds(1),
                            HandleNote = $"完成附件補檔FROM Paper;({(handle.UserType == UserType.正卡人 ? "正卡" : "附卡")}_{handle.ApplyCardType})",
                            HandleSeqNo = handle.SeqNo,
                        }
                    );
                }
            }

            using var fileTransaction = await scoreSharpFileContext.Database.BeginTransactionAsync();
            using var mainTransaction = await scoreSharpContext.Database.BeginTransactionAsync();
            try
            {
                await scoreSharpFileContext.Reviewer_ApplyFile.AddRangeAsync(reviewerApplyFiles);

                if (req.SyncStatus == SyncFileStatus.紙本初始)
                {
                    await scoreSharpContext.Reviewer_ApplyCreditCardInfoMain.AddAsync(entityMain);
                }

                if (req.SyncStatus == SyncFileStatus.補件 && main.Source == Source.紙本)
                {
                    var filterHandles = handles.Where(x => x.CardStatus == CardStatus.補件作業中).ToList();
                    if (filterHandles.Count > 0)
                    {
                        var afterCardStatus = filterHandles.Any(x => x.CardStep == CardStep.人工徵審)
                            ? CardStatus.補回件
                            : CardStatus.紙本件_待月收入預審;

                        foreach (var handle in filterHandles)
                        {
                            handle.CardStatus = afterCardStatus;
                        }
                    }
                    main.LastUpdateTime = now;
                    main.LastUpdateUserId = UserIdConst.SYSTEM;
                }
                else if (req.SyncStatus == SyncFileStatus.補件 && main.Source != Source.紙本)
                {
                    // Tips: 網路件目前只有一筆
                    var handle = handles.FirstOrDefault();
                    if (handle.CardStatus == CardStatus.補件作業中)
                    {
                        var afterCardStatus = handle.CardStep == CardStep.人工徵審 ? CardStatus.補回件 : CardStatus.網路件_待月收入預審;
                        handle.CardStatus = afterCardStatus;
                    }
                    main.LastUpdateTime = now;
                    main.LastUpdateUserId = UserIdConst.SYSTEM;
                }
                else if (req.SyncStatus == SyncFileStatus.網路小白件)
                {
                    main.LastUpdateTime = now;
                    main.LastUpdateUserId = UserIdConst.SYSTEM;
                }

                await scoreSharpContext.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);
                if (cardRecords.Count > 0)
                {
                    await scoreSharpContext.Reviewer_CardRecord.AddRangeAsync(cardRecords);
                }
                await scoreSharpContext.Reviewer_ApplyCreditCardInfoFile.AddRangeAsync(reviewerApplyCreditCardInfoFiles);

                await scoreSharpFileContext.SaveChangesAsync();
                await scoreSharpContext.SaveChangesAsync();
                await fileTransaction.CommitAsync();
                await mainTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                try
                {
                    logger.LogError("申請書編號: {@ApplyNo} 分散式交易失敗，開始回滾: {@Error}", req.ApplyNo, ex.ToString());

                    await mainTransaction.RollbackAsync();
                    await fileTransaction.RollbackAsync();

                    logger.LogError("申請書編號: {@ApplyNo} 回滾成功", req.ApplyNo);
                }
                catch (Exception rollbackEx)
                {
                    logger.LogError("申請書編號: {@ApplyNo} 回滾失敗，{@Error}", req.ApplyNo, rollbackEx.ToString());
                    throw new DatabaseExecuteException("分散式交易回滾失敗", rollbackEx);
                }

                throw new DatabaseExecuteException("分散式交易失敗", ex);
            }
            finally
            {
                await fileTransaction.DisposeAsync();
                await mainTransaction.DisposeAsync();
            }
        }
    }
}
