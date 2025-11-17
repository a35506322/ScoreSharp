using ScoreSharp.Batch.Infrastructures.Adapter.Models;
using ScoreSharp.Batch.Infrastructures.Adapter.Options;

namespace ScoreSharp.Batch.Infrastructures.Adapter;

public class EmailAdapter : IEmailAdapter
{
    private readonly HttpClient _httpClient;
    private readonly EmailAdapterOption _emailAdapterOption;

    public EmailAdapter(HttpClient httpClient, IOptions<EmailAdapterOption> emailAdapterOption)
    {
        _httpClient = httpClient;
        _emailAdapterOption = emailAdapterOption.Value;

        _httpClient.BaseAddress = new Uri(_emailAdapterOption.BaseUrl);
    }

    public async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request)
    {
        var result = new SendEmailResponse();
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"sendEmail", request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var emailResult = await response.Content.ReadFromJsonAsync<SendEmailResult>();
                result.Result = emailResult ?? new();
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
