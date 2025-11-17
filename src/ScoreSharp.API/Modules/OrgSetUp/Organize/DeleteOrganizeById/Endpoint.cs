using ScoreSharp.API.Modules.OrgSetUp.Organize.DeleteOrganizeById;

namespace ScoreSharp.API.Modules.OrgSetUp.Organize
{
    public partial class OrganizeController
    {
        ///<summary>
        /// 刪除單筆組織 By organizeCode
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Organize/DeleteOrganizeById/ORG001
        ///
        /// </remarks>
        /// <param name="organizeCode">PK</param>
        ///<returns></returns>
        [HttpDelete("{organizeCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除組織_2000_ResEx),
            typeof(刪除組織查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteOrganizeById")]
        public async Task<IResult> DeleteOrganizeById([FromRoute] string organizeCode) =>
            Results.Ok(await _mediator.Send(new Command(organizeCode)));
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.Organize.DeleteOrganizeById
{
    public record Command(string organizeCode) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            string organizeCode = request.organizeCode;

            var single = await context.OrgSetUp_Organize.SingleOrDefaultAsync(x => x.OrganizeCode == organizeCode);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, organizeCode);

            context.OrgSetUp_Organize.Remove(single);
            await context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(organizeCode);
        }
    }
}
