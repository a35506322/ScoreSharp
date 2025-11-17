using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyFileAttachmentsInfoByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得徵審行員附件 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetApplyFileAttachmentsInfoByApplyNo/20241014B0701
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetApplyFileAttachmentsInfoByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得徵審行員附件_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplyFileAttachmentsInfoByApplyNo")]
        public async Task<IResult> GetApplyFileAttachmentsInfoByApplyNo([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyFileAttachmentsInfoByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetApplyFileAttachmentsInfoByApplyNoResponse>>;

    public class Handler(ScoreSharpContext context, IScoreSharpDapperContext dapperContext)
        : IRequestHandler<Query, ResultResponse<GetApplyFileAttachmentsInfoByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetApplyFileAttachmentsInfoByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;

            var id_primary = await context.Reviewer_ApplyCreditCardInfoMain.AsNoTracking().SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            if (id_primary == null)
            {
                return ApiResponseHelper.NotFound<GetApplyFileAttachmentsInfoByApplyNoResponse>(null, applyNo);
            }

            var userDic = await context.OrgSetUp_User.ToDictionaryAsync(x => x.UserId, x => x.UserName);

            var id_supplementary = await context
                .Reviewer_ApplyCreditCardInfoSupplementary.AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            var attachments = await context
                .Reviewer_ApplyFileAttachment.AsNoTracking()
                .Where(x => x.ApplyNo == applyNo)
                .Where(x => x.DeleteTime == null && string.IsNullOrEmpty(x.DeleteUserId))
                .Select(x => new
                {
                    SeqNo = x.SeqNo,
                    AddTime = x.AddTime,
                    ID = x.ID,
                    AddUserId = x.AddUserId,
                    DBName = x.DBName,
                    IsHistory = x.IsHistory,
                    FileId = x.FileId,
                    ApplyNo = x.ApplyNo,
                })
                .ToListAsync();

            // 這邊DBName 和 IsHistory 一定會有值，因為在新增附件時，一定會有DBName和IsHistory
            // 刪除時，一定會有DeleteTime和DeleteUserId，但不會刪除掉DBName和IsHistory的值
            // 主要方便找取 DBName 和 IsHistory 的值
            Dictionary<Guid, string> files = new Dictionary<Guid, string>();
            if (attachments.Any())
            {
                string dbName = attachments.FirstOrDefault().DBName;
                string isHistory = attachments.FirstOrDefault().IsHistory;
                using var connection = GetSqlConnection(isHistory, dbName);
                var sql =
                    $@"SELECT [SeqNo] ,
                              [FileId] ,
                              [ApplyNo] ,
                              [FileName] ,
                              [FileType]
                    FROM [{dbName}].[dbo].[Reviewer_ApplyFile] WHERE ApplyNo = @ApplyNo AND FileType = 2";
                var query = await connection.QueryAsync<ReviewerApplyFileDto>(sql, new { ApplyNo = applyNo });
                files = query.ToDictionary(x => x.FileId, x => x.FileName);
            }

            var response = new GetApplyFileAttachmentsInfoByApplyNoResponse();

            response.IDOptions.Add(
                new IDOption()
                {
                    ID = id_primary.ID,
                    Name = id_primary.CHName,
                    UserType = UserType.正卡人,
                    UserTypeName = UserType.正卡人.ToString(),
                }
            );
            if (id_supplementary is not null)
            {
                response.IDOptions.Add(
                    new IDOption()
                    {
                        ID = id_supplementary.ID,
                        Name = id_supplementary.CHName,
                        UserType = UserType.附卡人,
                        UserTypeName = UserType.附卡人.ToString(),
                    }
                );
            }

            response.AttachmentsInfo = attachments
                .Select(x => new AttachmentsInfo()
                {
                    SeqNo = x.SeqNo,
                    FileName = files.GetValueOrDefault(x.FileId),
                    AddTime = x.AddTime,
                    ID = x.ID,
                    UserId = x.AddUserId,
                    UserName = userDic.GetValueOrDefault(x.AddUserId),
                    FileId = x.FileId,
                    ApplyNo = x.ApplyNo,
                })
                .ToList();

            return ApiResponseHelper.Success(response);
        }

        private SqlConnection GetSqlConnection(string isHistory, string dbName) =>
            isHistory switch
            {
                "Y" => dapperContext.CreateScoreSharpFileHisConnection(dbName),
                _ => dapperContext.CreateScoreSharpFileConnection(),
            };
    }
}
