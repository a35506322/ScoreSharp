using Microsoft.Extensions.DependencyInjection;
using ScoreSharp.Common.Helpers;

namespace ScoreSharp.Common.Adapters.MW3;

public class MW3ProcInterceptorLogString : DelegatingHandler
{
    private readonly ILogger<MW3ProcInterceptorLogString> _logger;
    private readonly IMW3SecurityHelper _mw3SecurityHelper;

    public MW3ProcInterceptorLogString(ILogger<MW3ProcInterceptorLogString> logger, [FromKeyedServices("PROC")] IMW3SecurityHelper mw3SecurityHelper)
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

    public MW3ProcInterceptorLogString(HttpMessageHandler handler, ILogger<MW3ProcInterceptorLogString> logger, IMW3SecurityHelper mw3SecurityHelper)
        : base(handler)
    {
        this._logger = logger;
        this._mw3SecurityHelper = mw3SecurityHelper;
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

        if (request.Content != null)
        {
            // 讀取原始內容
            string originalPayload = await request.Content.ReadAsStringAsync();
            try
            {
                string formattedPayload = JsonHelper.格式化Json(originalPayload);
                LogMessage.AppendLine($"Request Payload: {formattedPayload}");
            }
            catch
            {
                LogMessage.AppendLine($"Request Payload: {originalPayload}");
            }

            // 加密處理
            string encryptedPayload = _mw3SecurityHelper.AESEncrypt(originalPayload);

            var dbProcRequest = new DBProcRequest { info = encryptedPayload };
            // 替換為加密後的內容
            request.Content = new StringContent(JsonSerializer.Serialize(dbProcRequest), Encoding.UTF8, "application/json");
        }

        // 發送請求
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        LogMessage.AppendLine($"Response: {response.StatusCode} ({((int)response.StatusCode)})");

        string responsePayload = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.OK)
        {
            if (response.Content != null)
            {
                string encryptedResponse = _mw3SecurityHelper.AESDecrypt(responsePayload);
                try
                {
                    string formattedResponse = JsonHelper.格式化Json(encryptedResponse);
                    LogMessage.AppendLine($"Response Payload: {formattedResponse}");
                }
                catch
                {
                    LogMessage.AppendLine($"Response Payload: {encryptedResponse}");
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
                    string formattedResponse = JsonHelper.格式化Json(responsePayload);
                    LogMessage.AppendLine($"Response Payload: {formattedResponse}");
                }
                catch
                {
                    LogMessage.AppendLine($"Response Payload: {responsePayload}");
                }
            }
        }

        _logger.LogInformation(LogMessage.ToString());

        return response;
    }
}
