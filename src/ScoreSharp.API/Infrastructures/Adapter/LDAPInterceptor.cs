namespace ScoreSharp.API.Infrastructures.Adapter;

public class LDAPInterceptor : DelegatingHandler
{
    private readonly ILogger<LDAPInterceptor> _logger;

    public LDAPInterceptor(ILogger<LDAPInterceptor> logger)
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

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request: {Method} {RequestUri}", request.Method, request.RequestUri);

        LogHeaders(request);

        if (request.Content != null)
        {
            string payload = await request.Content.ReadAsStringAsync();
            LogPayload("Request", payload);
        }

        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        _logger.LogInformation("Response: {StatusCode} ({INTStatusCode})", response.StatusCode, ((int)response.StatusCode));

        LogPayload("Response", await response.Content.ReadAsStringAsync());

        return response;
    }

    private void LogHeaders(HttpRequestMessage request)
    {
        foreach (var header in request.Headers)
        {
            _logger.LogInformation("Request Headers: {0}: {1}", header.Key, string.Join(", ", header.Value));
        }

        if (request.Content?.Headers != null)
        {
            foreach (var header in request.Content.Headers)
            {
                _logger.LogInformation("Request Content Headers: {0}: {1}", header.Key, string.Join(", ", header.Value));
            }
        }
    }

    private void LogPayload(string stage, string payload)
    {
        if (string.IsNullOrEmpty(payload))
            return;

        if (payload.Length <= 1000)
            _logger.LogInformation("{Stage} Payload: {Payload}", stage, payload);
        else
            _logger.LogInformation("{Stage} Payload is too large to log", stage);
    }
}
