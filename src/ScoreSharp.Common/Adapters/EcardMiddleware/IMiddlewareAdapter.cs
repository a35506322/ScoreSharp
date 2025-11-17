using ScoreSharp.Common.Adapters.EcardMiddleware.Model;

namespace ScoreSharp.Common.Adapters.EcardMiddleware;

public interface IMiddlewareAdapter
{
    Task<PostEcardNewCaseResponse> PostEcardNewCaseAsync(EcardNewCaseRequest request);
}
