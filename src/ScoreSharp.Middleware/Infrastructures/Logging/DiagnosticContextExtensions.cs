namespace ScoreSharp.Middleware.Infrastructures.Logging;

public static class DiagnosticContextExtensions
{
    public static void SetProperties(this IDiagnosticContext diagnosticContext, params (string key, object value, bool destructure)[] properties)
    {
        foreach (var (key, value, destructure) in properties)
        {
            diagnosticContext.Set(key, value, destructure);
        }
    }
}
