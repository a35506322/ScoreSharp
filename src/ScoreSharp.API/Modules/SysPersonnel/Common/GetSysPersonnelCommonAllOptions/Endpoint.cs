using ScoreSharp.API.Modules.SysPersonnel.Common.GetSysPersonnelCommonAllOptions;

namespace ScoreSharp.API.Modules.SysPersonnel.Common
{
    public partial class SysPersonnelCommonController
    {
        /// <summary>
        /// 查詢系統人員作業設定下拉選單
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router: /SysPersonnelCommon/GetSysPersonnelCommonAllOptions?IsActive=Y
        ///
        ///     對照表
        ///     檢核狀態 = CaseCheckStatus
        ///     檢核完畢狀態 = CaseCheckedStatus
        ///
        /// </remarks>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetSysPersonnelCommonAllOptionsResponse>))]
        [EndpointSpecificExample(
            typeof(查詢系統人員作業設定下拉選單_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetSysPersonnelCommonAllOptions")]
        public async Task<IResult> GetSysPersonnelCommonAllOptions([FromQuery] string? isActive)
        {
            var result = await _mediator.Send(new Query(isActive));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.Common.GetSysPersonnelCommonAllOptions
{
    public record Query(string? isActive) : IRequest<ResultResponse<GetSysPersonnelCommonAllOptionsResponse>>;

    public class Handler() : IRequestHandler<Query, ResultResponse<GetSysPersonnelCommonAllOptionsResponse>>
    {
        public async Task<ResultResponse<GetSysPersonnelCommonAllOptionsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var caseCheckStatusOptions = EnumExtenstions.GetEnumOptions<CaseCheckStatus>(request.isActive);

            var caseCheckedStatusOptions = EnumExtenstions.GetEnumOptions<CaseCheckedStatus>(request.isActive);

            GetSysPersonnelCommonAllOptionsResponse response = new()
            {
                CaseCheckStatus = caseCheckStatusOptions,
                CaseCheckedStatus = caseCheckedStatusOptions,
            };

            return ApiResponseHelper.Success(response);
        }
    }
}
