namespace ScoreSharp.Common.Helpers.UITCSecurity;

public interface IUITCSecurityHelper
{
    public string EncryptConn(string data);
    public string DecryptConn(string data);
    public string EncryptData(string data);
    public string DecryptData(string data);
}
