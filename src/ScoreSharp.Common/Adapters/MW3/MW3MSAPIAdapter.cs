using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace ScoreSharp.Common.Adapters.MW3;

public class MW3MSAPIAdapter : IMW3MSAPIAdapter
{
    private readonly HttpClient _httpClient;
    private readonly MW3AdapterConfigurationOptions _mw3Configuration;

    public MW3MSAPIAdapter(HttpClient httpClient, IOptions<MW3AdapterConfigurationOptions> mw3Configuration)
    {
        _httpClient = httpClient;
        _mw3Configuration = mw3Configuration.Value;

        _httpClient.BaseAddress = new Uri(_mw3Configuration.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("api-version", _mw3Configuration.APIVersion);
    }

    public async Task<BaseMW3Response<QueryConcernDetailResponse>> QueryConcernDetail(string id)
    {
        try
        {
            QueryConcernDetailRequest request = new()
            {
                Method = "post",
                Headers = new(),
                Provider = "ccs-uwl",
                APIRouter = "ConcernDetail",
                Info = new ConcernDetailInfo
                {
                    ID = id,
                    Source = "Paperless",
                    Agent = "",
                },
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/MS/API", request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var queryConcernDetailResponse = await response.Content.ReadFromJsonAsync<QueryConcernDetailResponse>();
                return new BaseMW3Response<QueryConcernDetailResponse> { IsSuccess = true, Data = queryConcernDetailResponse };
            }

            return new BaseMW3Response<QueryConcernDetailResponse>
            {
                IsSuccess = false,
                ErrorMessage = await response.Content.ReadAsStringAsync(),
            };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<QueryConcernDetailResponse> { IsSuccess = false, ErrorMessage = ex.ToString() };
        }
    }
}
