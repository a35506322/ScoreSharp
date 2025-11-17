using ScoreSharp.API.Modules.Auth.ReviewerPermission.DeleteReviewerPermissionById;

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission
{
    public partial class ReviewerPermissionController
    {
        ///<summary>
        /// 刪除單筆徵審權限 By seqNo
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerPermission/DeleteReviewerPermissionById/5
        ///
        /// </remarks>
        /// <param name="seqNo">PK</param>
        ///<returns></returns>
        [HttpDelete("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除單筆徵審權限_2000_ResEx),
            typeof(刪除單筆徵審權限查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteReviewerPermissionById")]
        public async Task<IResult> DeleteReviewerPermissionById([FromRoute] int seqNo) =>
            Results.Ok(await _mediator.Send(new Command(seqNo)));
    }
}

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.DeleteReviewerPermissionById
{
    public record Command(int seqNo) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var seqNo = request.seqNo;

            var single = await context.Auth_ReviewerPermission.SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, seqNo.ToString());

            context.Auth_ReviewerPermission.Remove(single);
            await context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(seqNo.ToString());
        }
    }
}
