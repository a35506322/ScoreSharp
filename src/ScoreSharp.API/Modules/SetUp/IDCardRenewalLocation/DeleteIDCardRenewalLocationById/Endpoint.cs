using ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.DeleteIDCardRenewalLocationById;

namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation
{
    public partial class IDCardRenewalLocationController
    {
        /// <summary>
        /// 刪除單筆身分證換發地點
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /IDCardRenewalLocation/DeleteIDCardRenewalLocationById/09007000
        ///
        /// </remarks>
        /// <param name="idCardRenewalLocationCode">PK</param>
        /// <returns></returns>
        [HttpDelete("{idCardRenewalLocationCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(刪除身分證換發地點_2000_ResEx),
            typeof(刪除身分證換發地點查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("DeleteIDCardRenewalLocationById")]
        public async Task<IResult> DeleteIDCardRenewalLocationById([FromRoute] string idCardRenewalLocationCode)
        {
            var result = await _mediator.Send(new Command(idCardRenewalLocationCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.DeleteIDCardRenewalLocationById
{
    public record Command(string idCardRenewalLocationCode) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_IDCardRenewalLocation.SingleOrDefaultAsync(x =>
                x.IDCardRenewalLocationCode == request.idCardRenewalLocationCode
            );

            if (single is null)
                return ApiResponseHelper.NotFound<string>(null, request.idCardRenewalLocationCode);

            _context.Remove(single);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.DeleteByIdSuccess(request.idCardRenewalLocationCode);
        }
    }
}
