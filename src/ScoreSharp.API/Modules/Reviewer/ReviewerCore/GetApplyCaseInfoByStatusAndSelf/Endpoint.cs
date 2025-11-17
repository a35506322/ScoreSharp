using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCaseInfoByStatusAndSelf;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        ///<summary>
        /// 查詢行員自身案件 / 主要用途為渲染畫面多筆查詢
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetApplyCaseInfoByStatusAndSelf/1
        ///
        ///     1.網路件月收入確認
        ///     2.網路件人工審查
        ///     3.緊急製卡
        ///     4.補回件
        ///     5.拒件/撤件重審
        ///     6.網路件製卡失敗
        ///     7.紙本件月收入確認
        ///     8.紙本件人工審查
        ///     9.急件
        ///     10.退回重審
        ///     11.未補回
        ///     12.紙本件製卡失敗
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{caseStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetApplyCaseInfoByStatusAndSelfResponse>))]
        [EndpointSpecificExample(
            typeof(查詢行員自身案件_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [EndpointSpecificExample(
            typeof(查詢行員自身案件查詢值無效_4000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status400BadRequest
        )]
        [OpenApiOperation("GetApplyCaseInfoByStatusAndSelf")]
        public async Task<IResult> GetApplyCaseInfoByStatusAndSelf([FromRoute] CaseStatus caseStatus)
        {
            var result = await _mediator.Send(new Query(caseStatus));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCaseInfoByStatusAndSelf
{
    public record Query(CaseStatus caseStatus) : IRequest<ResultResponse<GetApplyCaseInfoByStatusAndSelfResponse>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtHelper, IReviewerHelper reviewerHelper)
        : IRequestHandler<Query, ResultResponse<GetApplyCaseInfoByStatusAndSelfResponse>>
    {
        public async Task<ResultResponse<GetApplyCaseInfoByStatusAndSelfResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUserId = jwtHelper.UserId;
            var dto = new GetApplyCreditCardBaseDataDto() { CurrentHandleUserId = currentUserId };
            var baseData = await reviewerHelper.GetApplyCreditCardBaseData(dto);
            var allCases = baseData
                .Select(g =>
                {
                    var response = new BaseData();
                    response.ApplyNo = g.ApplyNo;
                    response.CHName = g.M_CHName;
                    response.NameCheckList.Add(new NameCheckDto { NameChecked = g.M_NameChecked, UserType = UserType.正卡人 });
                    if (g.CardOwner != CardOwner.正卡)
                    {
                        response.NameCheckList.Add(new NameCheckDto { NameChecked = g.S1_NameChecked, UserType = UserType.附卡人 });
                    }
                    response.ID = g.M_ID;
                    response.ApplyDate = g.ApplyDate;
                    response.CaseType = g.CaseType;
                    response.LastUpdateTime = g.LastUpdateTime;
                    response.PromotionUnit = g.PromotionUnit;
                    response.PromotionUser = g.PromotionUser;
                    response.ApplyCardTypeList = g
                        .ApplyCardList.Select(x => new ApplyCardListDto
                        {
                            HandleSeqNo = x.HandleSeqNo,
                            CardStep = x.CardStep,
                            UserType = x.UserType,
                            ApplyCardType = x.ApplyCardType,
                            ApplyCardName = x.ApplyCardName,
                            CardStatus = x.CardStatus,
                            ID = x.ID,
                        })
                        .ToList();
                    response.Source = g.Source;

                    return response;
                })
                .ToList();

            var response = new GetApplyCaseInfoByStatusAndSelfResponse();

            foreach (CaseStatus caseStatus in Enum.GetValues(typeof(CaseStatus)))
            {
                var predicate = GetFilterCondition(caseStatus);
                int count = allCases.Count(predicate);

                response.CaseCountStatistic.Add(new CaseCountStatisticDto { CaseStatus = caseStatus, Count = count });

                if (request.caseStatus == caseStatus)
                    response.BaseDataList = allCases.Where(predicate).ToList();
            }

            return ApiResponseHelper.Success(response);
        }

        private Func<BaseData, bool> GetFilterCondition(CaseStatus caseStatus)
        {
            return caseStatus switch
            {
                CaseStatus.急件 => x => x.CaseType == CaseType.急件,
                CaseStatus.緊急製卡 => x => x.CaseType == CaseType.緊急製卡,
                CaseStatus.網路件月收入確認 => x =>
                    x.ApplyCardTypeList.Any(y =>
                        (
                            y.CardStatus == CardStatus.網路件_待月收入預審
                            || y.CardStatus == CardStatus.退件_等待完成本案徵審
                            || y.CardStatus == CardStatus.補件_等待完成本案徵審
                            || y.CardStatus == CardStatus.撤件_等待完成本案徵審
                        )
                        && y.CardStep == CardStep.月收入確認
                    )
                    && x.Source != Source.紙本,
                CaseStatus.拒件_撤件重審 => x => x.ApplyCardTypeList.Any(y => y.CardStatus == CardStatus.拒撤退重審),
                CaseStatus.紙本件製卡失敗 => x => x.ApplyCardTypeList.Any(y => y.CardStatus == CardStatus.製卡失敗) && x.Source == Source.紙本,
                CaseStatus.網路件製卡失敗 => x => x.ApplyCardTypeList.Any(y => y.CardStatus == CardStatus.製卡失敗) && x.Source != Source.紙本,
                CaseStatus.補回件 => x => x.ApplyCardTypeList.Any(y => y.CardStatus == CardStatus.補回件),
                CaseStatus.退回重審 => x => x.ApplyCardTypeList.Any(y => y.CardStatus == CardStatus.退回重審),
                CaseStatus.紙本件月收入確認 => x =>
                    x.ApplyCardTypeList.Any(y =>
                        (
                            y.CardStatus == CardStatus.紙本件_待月收入預審
                            || y.CardStatus == CardStatus.退件_等待完成本案徵審
                            || y.CardStatus == CardStatus.補件_等待完成本案徵審
                            || y.CardStatus == CardStatus.撤件_等待完成本案徵審
                        )
                        && y.CardStep == CardStep.月收入確認
                    )
                    && x.Source == Source.紙本,
                CaseStatus.網路件人工審查 => x =>
                    x.ApplyCardTypeList.Any(y =>
                        (
                            y.CardStatus == CardStatus.人工徵信中
                            || y.CardStatus == CardStatus.退件_等待完成本案徵審
                            || y.CardStatus == CardStatus.補件_等待完成本案徵審
                            || y.CardStatus == CardStatus.撤件_等待完成本案徵審
                            || y.CardStatus == CardStatus.核卡_等待完成本案徵審
                            || y.CardStatus == CardStatus.申請核卡_等待完成本案徵審
                            || y.CardStatus == CardStatus.申請退件_等待完成本案徵審
                            || y.CardStatus == CardStatus.申請補件_等待完成本案徵審
                            || y.CardStatus == CardStatus.申請撤件_等待完成本案徵審
                            || y.CardStatus == CardStatus.申請核卡中
                            || y.CardStatus == CardStatus.申請退件中
                            || y.CardStatus == CardStatus.申請補件中
                            || y.CardStatus == CardStatus.申請撤件中
                        )
                        && y.CardStep == CardStep.人工徵審
                    )
                    && x.Source != Source.紙本,
                CaseStatus.紙本件人工審查 => x =>
                    x.ApplyCardTypeList.Any(y =>
                        (
                            y.CardStatus == CardStatus.人工徵信中
                            || y.CardStatus == CardStatus.退件_等待完成本案徵審
                            || y.CardStatus == CardStatus.補件_等待完成本案徵審
                            || y.CardStatus == CardStatus.撤件_等待完成本案徵審
                            || y.CardStatus == CardStatus.核卡_等待完成本案徵審
                            || y.CardStatus == CardStatus.申請核卡_等待完成本案徵審
                            || y.CardStatus == CardStatus.申請退件_等待完成本案徵審
                            || y.CardStatus == CardStatus.申請補件_等待完成本案徵審
                            || y.CardStatus == CardStatus.申請撤件_等待完成本案徵審
                            || y.CardStatus == CardStatus.申請核卡中
                            || y.CardStatus == CardStatus.申請退件中
                            || y.CardStatus == CardStatus.申請補件中
                            || y.CardStatus == CardStatus.申請撤件中
                        )
                        && y.CardStep == CardStep.人工徵審
                    )
                    && x.Source == Source.紙本,
                _ => _ => false,
            };
        }
    }
}
