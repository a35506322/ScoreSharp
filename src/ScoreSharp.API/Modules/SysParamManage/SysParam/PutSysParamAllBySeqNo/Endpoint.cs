using ScoreSharp.API.Modules.SysParamManage.SysParam.PutSysParamAllBySeqNo;

namespace ScoreSharp.API.Modules.SysParamManage.SysParam
{
    public partial class SysParamController
    {
        /// <summary>
        /// 修改全部系統參數設定
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /SysParam/PutSysParamAllBySeqNo/1
        ///
        /// Notes :
        ///
        ///     系統參數只有一筆
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(修改全部系統參數設定_2000_ReqEx),
            typeof(修改全部系統參數設定查無資料_4001_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(修改全部系統參數設定_2000_ResEx),
            typeof(修改全部系統參數設定查無資料_4001_ResEx),
            typeof(修改全部系統參數設定呼叫有誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(修改全部系統參數設定AML職業別版本為無效日期_4000_ResEx),
            typeof(修改全部系統參數設定KYC加強審核版本為無效日期_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("PutSysParamAllBySeqNo")]
        public async Task<IResult> PutSysParamAllBySeqNo([FromRoute] int seqNo, [FromBody] PutSysParamAllBySeqNoRequest request)
        {
            var result = await _mediator.Send(new Command(seqNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysParamManage.SysParam.PutSysParamAllBySeqNo
{
    public record Command(int seqNo, PutSysParamAllBySeqNoRequest PutSysParamAllBySeqNoRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            int seqNo = request.seqNo;
            var dto = request.PutSysParamAllBySeqNoRequest;

            if (seqNo != dto.SeqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await context.SysParamManage_SysParam.SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, seqNo.ToString());

            entity.IPCompareHour = dto.IPCompareHour;
            entity.IPMatchCount = dto.IPMatchCount;
            entity.QueryHisDataDayRange = dto.QueryHisDataDayRange;
            entity.WebCaseEmailCompareHour = dto.WebCaseEmailCompareHour;
            entity.WebCaseEmailMatchCount = dto.WebCaseEmailMatchCount;
            entity.WebCaseMobileCompareHour = dto.WebCaseMobileCompareHour;
            entity.WebCaseMobileMatchCount = dto.WebCaseMobileMatchCount;
            entity.ShortTimeIDCompareHour = dto.ShortTimeIDCompareHour;
            entity.ShortTimeIDMatchCount = dto.ShortTimeIDMatchCount;
            entity.AMLProfessionCode_Version = dto.AMLProfessionCode_Version;
            entity.KYCFixStartTime = dto.KYCFixStartTime;
            entity.KYCFixEndTime = dto.KYCFixEndTime;
            entity.KYC_StrongReVersion = dto.KYC_StrongReVersion;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(seqNo.ToString(), seqNo.ToString());
        }
    }
}
