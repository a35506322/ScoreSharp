using ScoreSharp.API.Modules.SysPersonnel.MailSet.UpdateMailSetById;

namespace ScoreSharp.API.Modules.SysPersonnel.MailSet
{
    public partial class MailSetController
    {
        /// <summary>
        /// 更新單筆郵件設定
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /MailSet/UpdateMailSetById/1
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
            typeof(更新單筆郵件設定_2000_ReqEx),
            typeof(更新單筆郵件設定查無資料_4001_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(更新單筆郵件設定_2000_ResEx),
            typeof(更新單筆郵件設定查無資料_4001_ResEx),
            typeof(更新單筆郵件設定呼叫有誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateMailSetById")]
        public async Task<IResult> UpdateMailSetById([FromRoute] int seqNo, [FromBody] UpdateMailSetByIdRequest request)
        {
            var result = await _mediator.Send(new Command(seqNo, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.MailSet.UpdateMailSetById
{
    public record Command(int seqNo, UpdateMailSetByIdRequest UpdateMailSetByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            int seqNo = request.seqNo;
            var dto = request.UpdateMailSetByIdRequest;

            if (seqNo != dto.SeqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await context.SysParamManage_MailSet.SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, seqNo.ToString());

            var regex = @"^([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})(,[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})*$";

            if (
                !Regex.IsMatch(dto.SystemErrorLog_To, regex)
                || !Regex.IsMatch(dto.GuoLuKaCheckFailLog_To, regex)
                || !Regex.IsMatch(dto.KYCErrorLog_To, regex)
            )
                return ApiResponseHelper.UpdateByIdError<string>("郵件收件人請以逗點分割", seqNo.ToString());

            entity.SystemErrorLog_Template = dto.SystemErrorLog_Template;
            entity.SystemErrorLog_To = dto.SystemErrorLog_To;
            entity.SystemErrorLog_Title = dto.SystemErrorLog_Title;
            entity.GuoLuKaCheckFailLog_Template = dto.GuoLuKaCheckFailLog_Template;
            entity.GuoLuKaCheckFailLog_To = dto.GuoLuKaCheckFailLog_To;
            entity.GuoLuKaCheckFailLog_Title = dto.GuoLuKaCheckFailLog_Title;
            entity.KYCErrorLog_Template = dto.KYCErrorLog_Template;
            entity.KYCErrorLog_To = dto.KYCErrorLog_To;
            entity.KYCErrorLog_Title = dto.KYCErrorLog_Title;

            await context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(seqNo.ToString(), seqNo.ToString());
        }
    }
}
