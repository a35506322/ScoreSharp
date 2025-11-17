using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using ScoreSharp.Common.Adapters.EcardMiddleware.Model;
using ScoreSharp.Common.Adapters.MW3.Options;

namespace ScoreSharp.Common.Adapters.EcardMiddleware;

public class MiddlewareAdapter : IMiddlewareAdapter
{
    private readonly HttpClient _httpClient;
    private readonly MiddlewareAdapterOption _middlewareAdapterOption;

    public MiddlewareAdapter(HttpClient httpClient, IOptions<MiddlewareAdapterOption> middlewareAdapterOption)
    {
        _httpClient = httpClient;
        _middlewareAdapterOption = middlewareAdapterOption.Value;

        _httpClient.BaseAddress = new Uri(_middlewareAdapterOption.BaseUrl);
    }

    public async Task<PostEcardNewCaseResponse> PostEcardNewCaseAsync(EcardNewCaseRequest request)
    {
        PostEcardNewCaseResponse result = new();

        try
        {
            var response = await _httpClient.PostAsJsonAsync("ReviewerCore/EcardNewCase", request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var postEcardNewCaseResult = await response.Content.ReadFromJsonAsync<PostEcardNewCaseResult>();
                result.Result = postEcardNewCaseResult ?? new();
            }
            else
            {
                result.ErrorMessage = await response.Content.ReadAsStringAsync();
            }

            return result;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.ToString();
            return result;
        }
    }
}
