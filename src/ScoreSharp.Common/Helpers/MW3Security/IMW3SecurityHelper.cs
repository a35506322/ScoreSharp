namespace ScoreSharp.Common.Helpers.MW3Security;

public interface IMW3SecurityHelper
{
    string AESDecrypt(string base64String);
    string AESEncrypt(string plain_text);
}
