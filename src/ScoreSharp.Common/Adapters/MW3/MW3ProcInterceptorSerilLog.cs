using Microsoft.Extensions.DependencyInjection;

namespace ScoreSharp.Common.Adapters.MW3;

public class MW3ProcInterceptorSerilLog : DelegatingHandler
{
    private readonly ILogger<MW3ProcInterceptorSerilLog> _logger;
    private readonly IMW3SecurityHelper _mw3SecurityHelper;

    public MW3ProcInterceptorSerilLog(ILogger<MW3ProcInterceptorSerilLog> logger, [FromKeyedServices("PROC")] IMW3SecurityHelper mw3SecurityHelper)
        : base(
            new HttpClientHandler()
            {
                // The SSL connection could not be established, see inner exception
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                },
            }
        )
    {
        this._logger = logger;
        this._mw3SecurityHelper = mw3SecurityHelper;
    }

    public MW3ProcInterceptorSerilLog(HttpMessageHandler handler, ILogger<MW3ProcInterceptorSerilLog> logger, IMW3SecurityHelper mw3SecurityHelper)
        : base(handler)
    {
        this._logger = logger;
        this._mw3SecurityHelper = mw3SecurityHelper;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {Method} / {RequestUri}", request.Method, request.RequestUri);

        foreach (var header in request.Headers)
        {
            string key = header.Key;
            string value = string.Join(", ", header.Value);
            _logger.LogInformation("Request Headers: {Key}: {Value}", key, value);
        }

        if (request.Content != null)
        {
            // 讀取原始內容
            string originalPayload = await request.Content.ReadAsStringAsync();
            try
            {
                string formattedPayload = FormatJson(originalPayload);
                _logger.LogInformation("Request Payload: {Payload}", formattedPayload);
            }
            catch
            {
                // 如果不是有效的 JSON，直接記錄原始內容
                _logger.LogInformation("Request Payload: {Payload}", originalPayload);
            }

            // 加密處理
            string encryptedPayload = _mw3SecurityHelper.AESEncrypt(originalPayload);

            var dbProcRequest = new DBProcRequest { info = encryptedPayload };
            // 替換為加密後的內容
            request.Content = new StringContent(JsonSerializer.Serialize(dbProcRequest), Encoding.UTF8, "application/json");
        }

        // 發送請求
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        _logger.LogInformation("Response: {StatusCode} ({StatusCode})", response.StatusCode, ((int)response.StatusCode));

        string responsePayload = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.OK)
        {
            if (response.Content != null)
            {
                string encryptedResponse = _mw3SecurityHelper.AESDecrypt(responsePayload);
                try
                {
                    string formattedPayload = FormatJson(encryptedResponse);
                    _logger.LogInformation("Response Payload: {EncryptedResponse}", formattedPayload);
                }
                catch
                {
                    // 如果不是有效的 JSON，直接記錄原始內容
                    _logger.LogInformation("Response Payload: {EncryptedResponse}", encryptedResponse);
                }
                response.Content = new StringContent(encryptedResponse, Encoding.UTF8, "application/json");
            }
        }
        else
        {
            if (response.Content != null)
            {
                try
                {
                    string formattedPayload = FormatJson(responsePayload);
                    _logger.LogInformation("Response Payload: {ResponsePayload}", formattedPayload);
                }
                catch
                {
                    // 如果不是有效的 JSON，直接記錄原始內容
                    _logger.LogInformation("Response Payload: {ResponsePayload}", responsePayload);
                }
            }
        }

        return response;
    }

    private string FormatJson(string json)
    {
        var jsonDoc = System.Text.Json.JsonDocument.Parse(json);
        var options = new System.Text.Json.JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        return System.Text.Json.JsonSerializer.Serialize(jsonDoc, options);
    }
}
