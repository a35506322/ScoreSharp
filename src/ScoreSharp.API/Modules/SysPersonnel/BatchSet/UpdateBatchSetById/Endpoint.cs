using ScoreSharp.API.Modules.SysPersonnel.BatchSet.UpdateBatchSetById;

namespace ScoreSharp.API.Modules.SysPersonnel.BatchSet
{
    public partial class BatchSetController
    {
        /// <summary>
        /// 更新單筆排程設定
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /BatchSet/UpdateBatchSetById/1
        ///
        /// Notes :
        ///
        ///     系統參數只有一筆
        /// </remarks>
        /// <param name="seqNo">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(更新單筆排程設定_2000_ReqEx),
            typeof(更新單筆排程設定查無資料_4001_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(更新單筆排程設定_2000_ResEx),
            typeof(更新單筆排程設定查無資料_4001_ResEx),
            typeof(更新單筆排程設定呼叫有誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateBatchSetById")]
        public async Task<IActionResult> UpdateBatchSetById([FromRoute] int seqNo, [FromBody] UpdateBatchSetByIdRequest request)
        {
            var response = await _mediator.Send(new Command(seqNo, request));
            return Ok(response);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.BatchSet.UpdateBatchSetById
{
    public record Command(int seqNo, UpdateBatchSetByIdRequest updateBatchSetByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            int seqNo = request.seqNo;
            var dto = request.updateBatchSetByIdRequest;
            if (seqNo != dto.SeqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await context.SysParamManage_BatchSet.SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, seqNo.ToString());

            entity.RetryWebCaseFileErrorJob_IsEnabled = dto.RetryWebCaseFileErrorJob_IsEnabled;
            entity.RetryWebCaseFileErrorJob_BatchSize = dto.RetryWebCaseFileErrorJob_BatchSize;
            entity.A02KYCSyncJob_IsEnabled = dto.A02KYCSyncJob_IsEnabled;
            entity.A02KYCSyncJob_BatchSize = dto.A02KYCSyncJob_BatchSize;
            entity.CompareMissingCases_IsEnabled = dto.CompareMissingCases_IsEnabled;
            entity.EcardA02CheckNewCase_IsEnabled = dto.EcardA02CheckNewCase_IsEnabled;
            entity.EcardA02CheckNewCase_BatchSize = dto.EcardA02CheckNewCase_BatchSize;
            entity.EcardNotA02CheckNewCase_IsEnabled = dto.EcardNotA02CheckNewCase_IsEnabled;
            entity.EcardNotA02CheckNewCase_BatchSize = dto.EcardNotA02CheckNewCase_BatchSize;
            entity.GuoLuKaCheck_IsEnabled = dto.GuoLuKaCheck_IsEnabled;
            entity.GuoLuKaCheck_BatchSize = dto.GuoLuKaCheck_BatchSize;
            entity.GuoLuKaCaseWithdrawDays = dto.GuoLuKaCaseWithdrawDays;
            entity.PaperCheckNewCase_IsEnabled = dto.PaperCheckNewCase_IsEnabled;
            entity.PaperCheckNewCase_BatchSize = dto.PaperCheckNewCase_BatchSize;
            entity.RetryKYCSync_IsEnabled = dto.RetryKYCSync_IsEnabled;
            entity.RetryKYCSync_BatchSize = dto.RetryKYCSync_BatchSize;
            entity.SendKYCErrorLog_IsEnabled = dto.SendKYCErrorLog_IsEnabled;
            entity.SendKYCErrorLog_BatchSize = dto.SendKYCErrorLog_BatchSize;
            entity.SendSystemErrorLog_IsEnabled = dto.SendSystemErrorLog_IsEnabled;
            entity.SendSystemErrorLog_BatchSize = dto.SendSystemErrorLog_BatchSize;
            entity.SupplementTemplateReport_IsEnabled = dto.SupplementTemplateReport_IsEnabled;
            entity.SystemAssignment_WebCase_IsEnabled = dto.SystemAssignment_WebCase_IsEnabled;
            entity.SystemAssignment_Paper_IsEnabled = dto.SystemAssignment_Paper_IsEnabled;
            entity.SystemAssignment_ReviewManual_IsEnabled = dto.SystemAssignment_ReviewManual_IsEnabled;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(seqNo.ToString(), seqNo.ToString());
        }
    }
}
