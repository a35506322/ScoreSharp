namespace ScoreSharp.Middleware.Common.Helpers.FTP;

public class FTPOption
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Mima { get; set; }
    public int Port { get; set; }

    /// <summary>
    /// 是否使用SSL
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// 連線逾時時間
    /// /// </summary>
    public int ConnectTimeout { get; set; }

    /// <summary>
    /// 資料連線逾時時間
    /// </summary>
    /// <value></value>
    public int DataConnectionTimeout { get; set; }

    /// <summary>
    /// 固定ECARD補件資料夾路徑
    /// </summary>
    public string FixedEcardSupplementFolderPath { get; set; }
}
