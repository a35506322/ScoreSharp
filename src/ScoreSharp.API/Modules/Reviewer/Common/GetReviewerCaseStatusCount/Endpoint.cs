using System.Linq;
using ScoreSharp.API.Modules.Reviewer.Common.GetReviewerCaseStatusCount;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.Common
{
    public partial class ReviewerCommonController
    {
        /// <summary>
        /// 取得徵審案件狀態彙總
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetReviewerCaseStatusCountResponse>>))]
        [EndpointSpecificExample(
            typeof(取得徵審案件狀態彙總_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetReviewerCaseStatusCount")]
        [HttpGet]
        public async Task<IResult> GetReviewerCaseStatusCount()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.Common.GetReviewerCaseStatusCount
{
    public record Query() : IRequest<ResultResponse<List<GetReviewerCaseStatusCountResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetReviewerCaseStatusCountResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IJWTProfilerHelper _jwtHelper;
        private readonly IReviewerHelper _reviewerHelper;

        public Handler(ScoreSharpContext context, IJWTProfilerHelper jwtHelper, IReviewerHelper reviewerHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _reviewerHelper = reviewerHelper;
        }

        public async Task<ResultResponse<List<GetReviewerCaseStatusCountResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUserId = _jwtHelper.UserId;

            var dto = new GetApplyCreditCardBaseDataDto() { CurrentHandleUserId = currentUserId };
            var response = await _reviewerHelper.GetApplyCreditCardBaseData(dto);
            var responses = response.GroupBy(x => x.ApplyNo).Select(x => new { Cases = x.ToList() }).ToList();

            var manualReviewValidStatus = new HashSet<CardStatus>
            {
                CardStatus.人工徵信中,
                CardStatus.退件_等待完成本案徵審,
                CardStatus.補件_等待完成本案徵審,
                CardStatus.撤件_等待完成本案徵審,
                CardStatus.核卡_等待完成本案徵審,
                CardStatus.申請核卡_等待完成本案徵審,
                CardStatus.申請退件_等待完成本案徵審,
                CardStatus.申請補件_等待完成本案徵審,
                CardStatus.申請撤件_等待完成本案徵審,
                CardStatus.申請核卡中,
                CardStatus.申請退件中,
                CardStatus.申請補件中,
                CardStatus.申請撤件中,
            };

            List<GetReviewerCaseStatusCountResponse> results = new List<GetReviewerCaseStatusCountResponse>();
            foreach (CaseStatus caseStatus in Enum.GetValues(typeof(CaseStatus)))
            {
                var result = new GetReviewerCaseStatusCountResponse
                {
                    StatusName = caseStatus.ToString(),
                    StatusId = ((int)caseStatus).ToString(),
                    Count = 0,
                };

                switch (caseStatus)
                {
                    case CaseStatus.急件:
                        result.Count = responses.Count(x => x.Cases.Any(y => y.CaseType == CaseType.急件));
                        break;
                    case CaseStatus.緊急製卡:
                        result.Count = responses.Count(x => x.Cases.Any(y => y.CaseType == CaseType.緊急製卡));
                        break;
                    case CaseStatus.網路件月收入確認:
                        result.Count = responses.Count(x =>
                            x.Cases.Any(y =>
                            {
                                var hasMatchingCardStatus = y.ApplyCardList.Any(g =>
                                    (
                                        g.CardStatus == CardStatus.網路件_待月收入預審
                                        || g.CardStatus == CardStatus.退件_等待完成本案徵審
                                        || g.CardStatus == CardStatus.補件_等待完成本案徵審
                                        || g.CardStatus == CardStatus.撤件_等待完成本案徵審
                                    )
                                    && g.CardStep == CardStep.月收入確認
                                );

                                return hasMatchingCardStatus && y.Source != Source.紙本;
                            })
                        );
                        break;
                    case CaseStatus.拒件_撤件重審:
                        result.Count = responses.Count(x => x.Cases.Any(y => y.ApplyCardList.Any(s => s.CardStatus == CardStatus.拒撤退重審)));
                        break;
                    case CaseStatus.紙本件製卡失敗:
                        result.Count = responses.Count(x =>
                            x.Cases.Any(y => y.ApplyCardList.Any(s => s.CardStatus == CardStatus.製卡失敗 && y.Source == Source.紙本))
                        );
                        break;
                    case CaseStatus.網路件製卡失敗:
                        result.Count = responses.Count(x =>
                            x.Cases.Any(y => y.ApplyCardList.Any(s => s.CardStatus == CardStatus.製卡失敗 && y.Source != Source.紙本))
                        );
                        break;
                    case CaseStatus.補回件:
                        result.Count = responses.Count(x => x.Cases.Any(y => y.ApplyCardList.Any(s => s.CardStatus == CardStatus.補回件)));
                        break;
                    case CaseStatus.退回重審:
                        result.Count = responses.Count(x => x.Cases.Any(y => y.ApplyCardList.Any(s => s.CardStatus == CardStatus.退回重審)));
                        break;
                    case CaseStatus.紙本件月收入確認:
                        result.Count = responses.Count(x =>
                            x.Cases.Any(y =>
                            {
                                var hasMatchingCardStatus = y.ApplyCardList.Any(s =>
                                    (
                                        s.CardStatus == CardStatus.紙本件_待月收入預審
                                        || s.CardStatus == CardStatus.退件_等待完成本案徵審
                                        || s.CardStatus == CardStatus.補件_等待完成本案徵審
                                        || s.CardStatus == CardStatus.撤件_等待完成本案徵審
                                    )
                                    && s.CardStep == CardStep.月收入確認
                                );

                                return hasMatchingCardStatus && y.Source == Source.紙本;
                            })
                        );
                        break;
                    case CaseStatus.未補回:
                        // TODO: 2025.07.11 未補回 條件未確定
                        result.Count = 0; // 未補回的案件不計數
                        break;
                    case CaseStatus.網路件人工審查:

                        result.Count = responses.Count(x =>
                            x.Cases.Any(y =>
                                y.ApplyCardList.Any(s => manualReviewValidStatus.Contains(s.CardStatus) && s.CardStep == CardStep.人工徵審)
                                && y.Source != Source.紙本
                            )
                        );
                        break;
                    case CaseStatus.紙本件人工審查:

                        result.Count = responses.Count(x =>
                            x.Cases.Any(y =>
                                y.ApplyCardList.Any(s => manualReviewValidStatus.Contains(s.CardStatus) && s.CardStep == CardStep.人工徵審)
                                && y.Source == Source.紙本
                            )
                        );
                        break;
                    default:
                        continue; // Skip unsupported statuses
                }

                results.Add(result);
            }

            return ApiResponseHelper.Success(results);
        }
    }
}
