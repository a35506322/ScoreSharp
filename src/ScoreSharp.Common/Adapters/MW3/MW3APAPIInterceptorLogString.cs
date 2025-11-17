using ScoreSharp.Common.Helpers;

namespace ScoreSharp.Common.Adapters.MW3;

public class MW3APAPIInterceptorLogString : DelegatingHandler
{
    private readonly ILogger<MW3APAPIInterceptorLogString> _logger;

    public MW3APAPIInterceptorLogString(ILogger<MW3APAPIInterceptorLogString> logger)
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

    public MW3APAPIInterceptorLogString(HttpMessageHandler handler, ILogger<MW3APAPIInterceptorLogString> logger)
        : base(handler)
    {
        this._logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        StringBuilder LogMessage = new StringBuilder();
        LogMessage.AppendLine($"Request: {request.Method} / {request.RequestUri}");

        foreach (var header in request.Headers)
        {
            string key = header.Key;
            string value = string.Join(", ", header.Value);
            LogMessage.AppendLine($"Request Headers: {key}: {value}");
        }

        if (request.Content is not null)
        {
            // 讀取原始內容
            string payload = await request.Content.ReadAsStringAsync();
            try
            {
                string formattedPayload = JsonHelper.格式化Json(payload);
                LogMessage.AppendLine($"Request Payload: {formattedPayload}");
            }
            catch
            {
                LogMessage.AppendLine($"Request Payload: {payload}");
            }
        }

        // 發送請求
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        LogMessage.AppendLine($"Response: {response.StatusCode} ({((int)response.StatusCode)})");

        // 讀取原始內容
        string responsePayload = await response.Content.ReadAsStringAsync();
        try
        {
            string formattedResponse = JsonHelper.格式化Json(responsePayload);
            LogMessage.AppendLine($"Response Payload: {formattedResponse}");
        }
        catch
        {
            LogMessage.AppendLine($"Response Payload: {responsePayload}");
        }

        _logger.LogInformation(LogMessage.ToString());

        return response;
    }
}
