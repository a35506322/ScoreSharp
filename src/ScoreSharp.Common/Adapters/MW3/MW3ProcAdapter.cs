using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace ScoreSharp.Common.Adapters.MW3;

public class MW3ProcAdapter : IMW3ProcAdapter
{
    private readonly HttpClient _httpClient;
    private readonly MW3AdapterConfigurationOptions _mw3Configuration;

    public MW3ProcAdapter(HttpClient httpClient, IOptions<MW3AdapterConfigurationOptions> mw3Configuration)
    {
        _httpClient = httpClient;
        _mw3Configuration = mw3Configuration.Value;

        _httpClient.BaseAddress = new Uri(_mw3Configuration.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("api-version", _mw3Configuration.APIVersion);
    }

    public async Task<BaseMW3Response<QueryOCSI929Response>> QueryOCSI929(string id, string rtnCode = "")
    {
        try
        {
            QueryOCSI929Request request = new()
            {
                SPName = "OCSI929API",
                Info = new OCSI929Info { ID = id, RtnCode = rtnCode },
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/DB/PROC", request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var queryOCSI929Response = await response.Content.ReadFromJsonAsync<QueryOCSI929Response>();
                return new BaseMW3Response<QueryOCSI929Response> { IsSuccess = true, Data = queryOCSI929Response };
            }

            return new BaseMW3Response<QueryOCSI929Response> { IsSuccess = false, ErrorMessage = await response.Content.ReadAsStringAsync() };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<QueryOCSI929Response> { IsSuccess = false, ErrorMessage = ex.ToString() };
        }
    }

    public async Task<BaseMW3Response<QuerySearchCusDataResponse>> QuerySearchCusData(string id, string rtnCode = "")
    {
        try
        {
            QuerySearchCusDataRequest request = new()
            {
                SPName = "SearchCusDataAPI",
                Info = new SearchCusDataInfo { ID = id, RtnCode = rtnCode },
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/DB/PROC", request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var querySearchCusDataResponse = await response.Content.ReadFromJsonAsync<QuerySearchCusDataResponse>();
                return new BaseMW3Response<QuerySearchCusDataResponse> { IsSuccess = true, Data = querySearchCusDataResponse };
            }

            return new BaseMW3Response<QuerySearchCusDataResponse> { IsSuccess = false, ErrorMessage = await response.Content.ReadAsStringAsync() };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<QuerySearchCusDataResponse> { IsSuccess = false, ErrorMessage = ex.ToString() };
        }
    }

    public async Task<BaseMW3Response<QueryTravelCardCustomerResponse>> QueryTravelCardCustomer(string id, string rtnCode = "")
    {
        try
        {
            QueryTravelCardCustomerRequest request = new()
            {
                SPName = "TravelCardCustomerAPI",
                Info = new TravelCardCustomerInfo { ID = id, RtnCode = rtnCode },
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/DB/PROC", request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var queryTravelCardCustomerResponse = await response.Content.ReadFromJsonAsync<QueryTravelCardCustomerResponse>();
                return new BaseMW3Response<QueryTravelCardCustomerResponse> { IsSuccess = true, Data = queryTravelCardCustomerResponse };
            }

            return new BaseMW3Response<QueryTravelCardCustomerResponse>
            {
                IsSuccess = false,
                ErrorMessage = await response.Content.ReadAsStringAsync(),
            };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<QueryTravelCardCustomerResponse> { IsSuccess = false, ErrorMessage = ex.ToString() };
        }
    }

    public async Task<BaseMW3Response<QueryIBM7020Response>> QueryIBM7020(string id)
    {
        try
        {
            QueryIBM7020Request request = new()
            {
                Code = "7020",
                TermID = "0000",
                ID = id,
                Filler = string.Empty,
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/IBM/7020", request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var queryIBM7020Response = await response.Content.ReadFromJsonAsync<QueryIBM7020Response>();
                return new BaseMW3Response<QueryIBM7020Response> { IsSuccess = true, Data = queryIBM7020Response };
            }

            return new BaseMW3Response<QueryIBM7020Response> { IsSuccess = false, ErrorMessage = await response.Content.ReadAsStringAsync() };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<QueryIBM7020Response> { IsSuccess = false, ErrorMessage = ex.ToString() };
        }
    }

    public async Task<BaseMW3Response<QueryOriginalCardholderDataResponse>> QueryOriginalCardholderData(
        string id = "",
        string email = "",
        string mobile = ""
    )
    {
        try
        {
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(mobile))
            {
                throw new ArgumentException("id, email, mobile 至少需要一個");
            }

            QueryOriginalCardholderDataRequest request = new()
            {
                SPName = "usp_OriginalMember_WithPCICAndAI00",
                Info = new QueryOriginalCardholderDataInfo
                {
                    ID = id,
                    Phone = mobile,
                    Email = email,
                    RtnCode = string.Empty,
                },
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/DB/PROC", request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var queryOriginalCardholderDataResponse = await response.Content.ReadFromJsonAsync<QueryOriginalCardholderDataResponse>();
                return new BaseMW3Response<QueryOriginalCardholderDataResponse> { IsSuccess = true, Data = queryOriginalCardholderDataResponse };
            }

            return new BaseMW3Response<QueryOriginalCardholderDataResponse>
            {
                IsSuccess = false,
                ErrorMessage = await response.Content.ReadAsStringAsync(),
            };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<QueryOriginalCardholderDataResponse> { IsSuccess = false, ErrorMessage = ex.ToString() };
        }
    }

    public async Task<BaseMW3Response<QueryEBillResponse>> QueryEBill(string email)
    {
        try
        {
            QueryEBillRequest request = new()
            {
                SPName = "EBill_202509300007_Query",
                Info = new QueryEBillRequestInfo { Email = email, RtnCode = string.Empty },
            };

            var response = await _httpClient.PostAsJsonAsync("/mw3/DB/PROC", request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var queryEBillDataResponse = await response.Content.ReadFromJsonAsync<QueryEBillResponse>();
                return new BaseMW3Response<QueryEBillResponse> { IsSuccess = true, Data = queryEBillDataResponse };
            }

            return new BaseMW3Response<QueryEBillResponse> { IsSuccess = false, ErrorMessage = await response.Content.ReadAsStringAsync() };
        }
        catch (Exception ex)
        {
            return new BaseMW3Response<QueryEBillResponse> { IsSuccess = false, ErrorMessage = ex.ToString() };
        }
    }
}
