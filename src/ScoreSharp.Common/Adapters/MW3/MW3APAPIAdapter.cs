using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ScoreSharp.Common.Adapters.MW3;

public class MW3APAPIAdapter : IMW3APAPIAdapter
{
    private readonly HttpClient _httpClient;
    private readonly MW3AdapterConfigurationOptions _mw3Configuration;
    private readonly IMW3SecurityHelper _mw3SecurityHelper;

    public MW3APAPIAdapter(
        HttpClient httpClient,
        IOptions<MW3AdapterConfigurationOptions> mw3Configuration,
        [FromKeyedServices("APAPI")] IMW3SecurityHelper mw3SecurityHelper
    )
    {
        _httpClient = httpClient;
        _mw3Configuration = mw3Configuration.Value;

        _httpClient.BaseAddress = new Uri(_mw3Configuration.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("api-version", _mw3Configuration.APIVersion);
        _mw3SecurityHelper = mw3SecurityHelper;
    }

    public async Task<BaseMW3Response<EcardNewCaseData>> QueryEcardNewCase(string applyNo)
    {
        try
        {
            QueryEcardNewCaseRequest request = new() { Info = new QueryEcardNewCaseInfo { ApplyNo = applyNo } };

            var response = await _httpClient.PostAsJsonAsync("/mw3/AP/API", request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var queryEcardNewCaseResponse = await response.Content.ReadFromJsonAsync<QueryEcardNewCaseResponse>();

                if (queryEcardNewCaseResponse.StatusCode == "OK")
                {
                    var decryptData = _mw3SecurityHelper.AESDecrypt(queryEcardNewCaseResponse.Info.Data);

                    return new BaseMW3Response<EcardNewCaseData>
                    {
                        IsSuccess = true,
                        Data = JsonSerializer.Deserialize<EcardNewCaseData>(decryptData),
                    };
                }
            }

            return new BaseMW3Response<EcardNewCaseData>
            {
                IsSuccess = false,
                ErrorMessage = await response.Content.ReadAsStringAsync(),
                Data = new EcardNewCaseData(),
            };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<EcardNewCaseData>
            {
                IsSuccess = false,
                ErrorMessage = ex.ToString(),
                Data = new EcardNewCaseData(),
            };
        }
    }

    public async Task<BaseMW3Response<QueryNameCheckResponse>> QueryNameCheck(string name, string callUser, string traceId)
    {
        try
        {
            QueryNameCheckRequest request = new()
            {
                ApiName = "AML000001",
                Headers = new QueryNameCheckHeaders { Authorization = $"Basic {_mw3Configuration.EAIHubKey}" },
                Info = new QueryNameCheckInfo
                {
                    RestType = "AML000001",
                    BankNo = "TW",
                    BranchNo = "911",
                    Channel = string.Empty,
                    DOB = string.Empty,
                    EnglishName = name,
                    ID = string.Empty,
                    Nationality = string.Empty,
                    NonEnglishName = string.Empty,
                    ReferenceNumber = traceId,
                    TellerName = callUser,
                },
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/AP/API", request);
            response.EnsureSuccessStatusCode();

            // 先讀取原始 JSON 字串
            var responseContent = await response.Content.ReadAsStringAsync();

            // 使用 JsonDocument 來檢查 statusCode
            using var jsonDocument = JsonDocument.Parse(responseContent);
            var statusCode = jsonDocument.RootElement.GetProperty("statusCode").GetString();

            if (statusCode == "OK")
            {
                try
                {
                    // 只有當 statusCode 為 "OK" 時才進行完整的序列化
                    var queryNameCheckResponse = JsonSerializer.Deserialize<QueryNameCheckResponse>(responseContent);
                    return new BaseMW3Response<QueryNameCheckResponse> { IsSuccess = true, Data = queryNameCheckResponse };
                }
                catch (JsonException)
                {
                    return new BaseMW3Response<QueryNameCheckResponse>
                    {
                        IsSuccess = false,
                        ErrorMessage = responseContent,
                        Data = new QueryNameCheckResponse(),
                    };
                }
            }

            return new BaseMW3Response<QueryNameCheckResponse>
            {
                IsSuccess = false,
                ErrorMessage = responseContent,
                Data = new QueryNameCheckResponse(),
            };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<QueryNameCheckResponse>
            {
                IsSuccess = false,
                ErrorMessage = ex.ToString(),
                Data = new QueryNameCheckResponse(),
            };
        }
    }

    public async Task<BaseMW3Response<SuggestKycResponse>> SuggestKYC(SuggestKycMW3Info request)
    {
        try
        {
            SuggestKycRequest suggestKycRequest = new()
            {
                Headers = new SuggestKycMW3Headers { Authorization = $"Basic {_mw3Configuration.EAIHubKey}" },
                Info = request,
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/AP/API", suggestKycRequest);
            response.EnsureSuccessStatusCode();

            // 先讀取原始 JSON 字串
            var responseContent = await response.Content.ReadAsStringAsync();

            // 使用 JsonDocument 來檢查 statusCode
            using var jsonDocument = JsonDocument.Parse(responseContent);
            var statusCode = jsonDocument.RootElement.GetProperty("statusCode").GetString();

            if (statusCode == "OK")
            {
                try
                {
                    // 只有當 statusCode 為 "OK" 時才進行完整的序列化
                    var suggestKycResponse = JsonSerializer.Deserialize<SuggestKycResponse>(responseContent);
                    return new BaseMW3Response<SuggestKycResponse> { IsSuccess = true, Data = suggestKycResponse };
                }
                catch (JsonException)
                {
                    return new BaseMW3Response<SuggestKycResponse>
                    {
                        IsSuccess = false,
                        ErrorMessage = responseContent,
                        Data = new SuggestKycResponse(),
                    };
                }
            }

            return new BaseMW3Response<SuggestKycResponse>
            {
                IsSuccess = false,
                ErrorMessage = responseContent,
                Data = new SuggestKycResponse(),
            };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<SuggestKycResponse>
            {
                IsSuccess = false,
                ErrorMessage = ex.ToString(),
                Data = new SuggestKycResponse(),
            };
        }
    }

    public async Task<BaseMW3Response<SyncKycResponse>> SyncKYC(SyncKycMW3Info request)
    {
        try
        {
            SyncKycRequest syncKycRequest = new()
            {
                Headers = new SyncKycMW3Headers { Authorization = $"Basic {_mw3Configuration.EAIHubKey}" },
                Info = request,
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/AP/API", syncKycRequest);
            response.EnsureSuccessStatusCode();

            // 先讀取原始 JSON 字串
            var responseContent = await response.Content.ReadAsStringAsync();

            // 使用 JsonDocument 來檢查 statusCode
            using var jsonDocument = JsonDocument.Parse(responseContent);
            var statusCode = jsonDocument.RootElement.GetProperty("statusCode").GetString();

            if (statusCode == "OK")
            {
                try
                {
                    // 只有當 statusCode 為 "OK" 時才進行完整的序列化
                    var syncKycResponse = JsonSerializer.Deserialize<SyncKycResponse>(responseContent);
                    return new BaseMW3Response<SyncKycResponse> { IsSuccess = true, Data = syncKycResponse };
                }
                catch (JsonException)
                {
                    return new BaseMW3Response<SyncKycResponse>
                    {
                        IsSuccess = false,
                        ErrorMessage = responseContent,
                        Data = new SyncKycResponse(),
                    };
                }
            }

            return new BaseMW3Response<SyncKycResponse>
            {
                IsSuccess = false,
                ErrorMessage = responseContent,
                Data = new SyncKycResponse(),
            };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<SyncKycResponse>
            {
                IsSuccess = false,
                ErrorMessage = ex.ToString(),
                Data = new SyncKycResponse(),
            };
        }
    }

    public async Task<BaseMW3Response<QueryCustKYCRiskLevelResponse>> QueryCustKYCRiskLevel(QueryCustKYCRiskLevelRequestInfo request)
    {
        try
        {
            QueryCustKYCRiskLevelRequest queryCustKYCRiskLevelRequest = new()
            {
                Headers = new() { Authorization = $"Basic {_mw3Configuration.EAIHubKey}" },
                Info = request,
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/AP/API", queryCustKYCRiskLevelRequest);
            response.EnsureSuccessStatusCode();

            // 先讀取原始 JSON 字串
            var responseContent = await response.Content.ReadAsStringAsync();

            // 使用 JsonDocument 來檢查 statusCode
            using var jsonDocument = JsonDocument.Parse(responseContent);
            var statusCode = jsonDocument.RootElement.GetProperty("statusCode").GetString();

            if (statusCode == "OK")
            {
                try
                {
                    // 只有當 statusCode 為 "OK" 時才進行完整的序列化
                    var syncKycResponse = JsonSerializer.Deserialize<QueryCustKYCRiskLevelResponse>(responseContent);
                    return new BaseMW3Response<QueryCustKYCRiskLevelResponse> { IsSuccess = true, Data = syncKycResponse };
                }
                catch (JsonException)
                {
                    return new BaseMW3Response<QueryCustKYCRiskLevelResponse>
                    {
                        IsSuccess = false,
                        ErrorMessage = responseContent,
                        Data = new QueryCustKYCRiskLevelResponse(),
                    };
                }
            }

            return new BaseMW3Response<QueryCustKYCRiskLevelResponse>
            {
                IsSuccess = false,
                ErrorMessage = responseContent,
                Data = new QueryCustKYCRiskLevelResponse(),
            };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<QueryCustKYCRiskLevelResponse>
            {
                IsSuccess = false,
                ErrorMessage = ex.ToString(),
                Data = new QueryCustKYCRiskLevelResponse(),
            };
        }
    }
}
