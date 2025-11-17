using ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.DeleteUserOrgCaseSetUpById;

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp
{
    public partial class UserOrgCaseSetUpController
    {
        ///<summary>
        /// 刪除單筆人員組織分案群組設定 By userId
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /UserOrgCaseSetUp/DeleteUserOrgCaseSetUpById/raya00
        ///
        /// </remarks>
        /// <param name="userId">PK</param>
        ///<returns></returns>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除單筆人員組織分案群組設定_2000_ResEx),
            typeof(刪除單筆人員組織分案群組設定查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteUserOrgCaseSetUpById")]
        public async Task<IResult> DeleteUserOrgCaseSetUpById([FromRoute] string userId)
        {
            var result = await _mediator.Send(new Command(userId));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp.DeleteUserOrgCaseSetUpById
{
    public record Command(string userId) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            string userId = request.userId;

            var single = await context.OrgSetUp_UserOrgCaseSetUp.SingleOrDefaultAsync(x => x.UserId == userId);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, userId);

            context.OrgSetUp_UserOrgCaseSetUp.Remove(single);
            await context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(userId);
        }
    }
}
