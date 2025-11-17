using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.Common.GetReviewerCaseStatusCount;

public class GetReviewerCaseStatusCountResponse
{
    public string StatusName { get; set; }
    public string StatusId { get; set; }
    public int Count { get; set; }
}
