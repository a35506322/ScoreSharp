using ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.DeleteUserTakeVacationById;

namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation
{
    public partial class UserTakeVacationController
    {
        ///<summary>
        /// 刪除單筆員工休假 By seqNo
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /UserTakeVacation/DeleteUserTakeVacationById/1
        ///
        /// </remarks>
        /// <param name="seqNo">PK</param>
        ///<returns></returns>
        [HttpDelete("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除單筆員工休假_2000_ResEx),
            typeof(刪除單筆員工休假查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteUserTakeVacationById")]
        public async Task<IResult> DeleteUserTakeVacationById([FromRoute] long seqNo)
        {
            var result = await _mediator.Send(new Command(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.DeleteUserTakeVacationById
{
    public record Command(long seqNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var seqNo = request.seqNo;

            var single = await context.OrgSetUp_UserTakeVacation.SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, seqNo.ToString());

            context.OrgSetUp_UserTakeVacation.Remove(single);
            await context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(seqNo.ToString());
        }
    }
}
