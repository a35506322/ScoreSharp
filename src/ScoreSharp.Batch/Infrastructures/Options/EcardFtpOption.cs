namespace ScoreSharp.Batch.Infrastructures.Options;

public class EcardFtpOption
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Mima { get; set; }
    public int Port { get; set; }
    public bool UseSsl { get; set; }
    public int ConnectTimeout { get; set; }
    public int DataConnectionTimeout { get; set; }
    public string FixedEcardSupplementFolderPath { get; set; }
    public string CompareMissingCasesRemoteFolderPath { get; set; }
    public string CompareMissingCasesLocalFolderPath { get; set; }
}
