using ScoreSharp.Batch.Infrastructures.Adapter.Models;

namespace ScoreSharp.Batch.Infrastructures.Adapter;

public interface IEmailAdapter
{
    Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request);
}
