using ScoreSharp.API.Modules.Manage.Common.GetManageCommonAllOptions;

namespace ScoreSharp.API.Modules.Manage.Common
{
    public partial class ManageCommonController
    {
        /// <summary>
        /// 取得全部管理作業相關下拉選單
        /// </summary>
        /// <param name="isActive"></param>
        /// <remarks>
        ///
        ///    Sample Router: /ManageCommon/GetManageCommonAllOptions?IsActive=Y
        ///
        ///     對照表
        ///     分案類型 = CaseAssignmentType
        ///     人員指派變更狀態 = AssignmentChangeStatus
        ///     派案類型 = CaseStatisticType
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetManageCommonAllOptionsResponse>))]
        [EndpointSpecificExample(
            typeof(取得全部管理作業相關下拉選單_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetManageCommonAllOptions")]
        public async Task<IResult> GetManageCommonAllOptions([FromQuery] string? isActive)
        {
            var result = await _mediator.Send(new Query(isActive));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.Common.GetManageCommonAllOptions
{
    public record Query(string? isActive) : IRequest<ResultResponse<GetManageCommonAllOptionsResponse>>;

    public class Handler() : IRequestHandler<Query, ResultResponse<GetManageCommonAllOptionsResponse>>
    {
        public async Task<ResultResponse<GetManageCommonAllOptionsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var caseAssignmentType = EnumExtenstions.GetEnumOptions<CaseAssignmentType>(request.isActive);
            var assignmentChangeStatus = EnumExtenstions.GetEnumOptions<AssignmentChangeStatus>(request.isActive);
            var transferCaseType = EnumExtenstions.GetEnumOptions<TransferCaseType>(request.isActive);
            var caseStatisticType = EnumExtenstions.GetEnumOptions<CaseStatisticType>(request.isActive);

            GetManageCommonAllOptionsResponse response = new()
            {
                CaseAssignmentType = caseAssignmentType,
                AssignmentChangeStatus = assignmentChangeStatus,
                TransferCaseType = transferCaseType,
                CaseStatisticType = caseStatisticType,
            };
            return ApiResponseHelper.Success(response);
        }
    }
}
