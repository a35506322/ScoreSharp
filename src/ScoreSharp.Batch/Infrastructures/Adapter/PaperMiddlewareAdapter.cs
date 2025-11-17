using ScoreSharp.Batch.Infrastructures.Adapter.Models;
using ScoreSharp.Batch.Infrastructures.Adapter.Options;

namespace ScoreSharp.Batch.Infrastructures.Adapter.PaperMiddleware;

public class PaperMiddlewareAdapter : IPaperMiddlewareAdapter
{
    private readonly HttpClient _httpClient;
    private readonly PaperMiddlewareAdapterOption _paperMiddlewareAdapterOption;

    public PaperMiddlewareAdapter(HttpClient httpClient, IOptions<PaperMiddlewareAdapterOption> paperMiddlewareAdapterOption)
    {
        _httpClient = httpClient;
        _paperMiddlewareAdapterOption = paperMiddlewareAdapterOption.Value;
        _httpClient.BaseAddress = new Uri(_paperMiddlewareAdapterOption.BaseUrl);
    }

    public async Task<SyncApplyInfoWebWhiteRequest> CreateSyncApplyInfoWebWhiteReq(CreateSyncApplyInfoWebWhiteReqRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("Test/CreateSyncApplyInfoWebWhiteReq", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SyncApplyInfoWebWhiteRequest>();
        return result;
    }

    public async Task<SyncApplyInfoWebWhiteResponse> SyncApplyInfoWebWhite(SyncApplyInfoWebWhiteRequest request)
    {
        /*
            🔔 這邊的 Header 要先移除，再重新加入，因為 HttpClient 的 Header 是共用的，如果不清除，會導致 Header 累積
            _httpClient.DefaultRequestHeaders.Remove("X-APPLYNO");
            _httpClient.DefaultRequestHeaders.Remove("X-SYNCUSERID");
            _httpClient.DefaultRequestHeaders.Add("X-APPLYNO", request.ApplyNo);
            _httpClient.DefaultRequestHeaders.Add("X-SYNCUSERID", request.SyncUserId);
        */

        // 🔔 可以每次呼叫創建新的 HttpRequestMessage，避免 Header 累積，不過缺點是　SendAsync　不能用 PostAsJsonAsync
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "ReviewerCore/SyncApplyInfoWebWhite");
        httpRequest.Headers.Add("X-APPLYNO", request.ApplyNo);
        httpRequest.Headers.Add("X-SYNCUSERID", request.SyncUserId);
        httpRequest.Content = JsonContent.Create(request);

        var response = await _httpClient.SendAsync(httpRequest);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<SyncApplyInfoWebWhiteResponse>();
        return result;
    }
}
