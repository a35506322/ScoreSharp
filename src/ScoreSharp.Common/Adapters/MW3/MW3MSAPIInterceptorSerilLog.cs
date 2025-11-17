using ScoreSharp.Common.Helpers;

namespace ScoreSharp.Common.Adapters.MW3;

public class MW3MSAPIInterceptorSerilLog : DelegatingHandler
{
    private readonly ILogger<MW3MSAPIInterceptorSerilLog> _logger;

    public MW3MSAPIInterceptorSerilLog(ILogger<MW3MSAPIInterceptorSerilLog> logger)
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
    }

    public MW3MSAPIInterceptorSerilLog(HttpMessageHandler handler, ILogger<MW3MSAPIInterceptorSerilLog> logger)
        : base(handler)
    {
        this._logger = logger;
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
            string payload = await request.Content.ReadAsStringAsync();

            // 嘗試格式化 JSON 並確保中文字符正確顯示
            try
            {
                string formattedPayload = JsonHelper.格式化Json(payload);
                _logger.LogInformation("Request Payload: {Payload}", formattedPayload);
            }
            catch
            {
                // 如果不是有效的 JSON，直接記錄原始內容
                _logger.LogInformation("Request Payload: {Payload}", payload);
            }
        }

        // 發送請求
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        _logger.LogInformation("Response: {StatusCode} ({StatusCode})", response.StatusCode, ((int)response.StatusCode));

        if (response.Content != null)
        {
            string responsePayload = await response.Content.ReadAsStringAsync();

            // 嘗試格式化 JSON 並確保中文字符正確顯示
            try
            {
                string formattedResponse = JsonHelper.格式化Json(responsePayload);
                _logger.LogInformation("Response Payload: {ResponsePayload}", formattedResponse);
            }
            catch
            {
                // 如果不是有效的 JSON，直接記錄原始內容
                _logger.LogInformation("Response Payload: {ResponsePayload}", responsePayload);
            }
        }

        return response;
    }
}
