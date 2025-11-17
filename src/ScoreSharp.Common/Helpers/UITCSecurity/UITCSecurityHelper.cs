using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace ScoreSharp.Common.Helpers.UITCSecurity;

public class UITCSecurityHelper : IUITCSecurityHelper
{
    private readonly byte[] _keyConn = Encoding.Unicode.GetBytes("聯邦網通加密鍵值Key     ");
    private readonly byte[] _ivConn = Encoding.Unicode.GetBytes("聯邦網通加密鍵值IV      ");
    private readonly byte[] _keyData = Encoding.Unicode.GetBytes("聯邦網通KeyData     ");
    private readonly byte[] _ivData = Encoding.Unicode.GetBytes("聯邦網通IVData      ");

    /// <summary>
    /// 加密連線字串
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public string EncryptConn(string data) => Encrypt(data, _keyConn, _ivConn);

    /// <summary>
    /// 解密連線字串
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public string DecryptConn(string data) => Decrypt(data, _keyConn, _ivConn);

    /// <summary>
    /// 加密資料
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public string EncryptData(string data) => Encrypt(data, _keyData, _ivData);

    /// <summary>
    /// 解密資料
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public string DecryptData(string data) => Decrypt(data, _keyData, _ivData);

    private static string Encrypt(string data, byte[] key, byte[] iv)
    {
        var inputBytes = Encoding.UTF8.GetBytes(data);
        var engine = new RijndaelEngine(256);
        var blockCipher = new CbcBlockCipher(engine);
        var cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
        var keyParam = new KeyParameter(key);
        var keyParamWithIv = new ParametersWithIV(keyParam, iv, 0, 32);
        cipher.Init(true, keyParamWithIv);
        var outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
        var length = cipher.ProcessBytes(inputBytes, outputBytes, 0);
        cipher.DoFinal(outputBytes, length);
        return Convert.ToBase64String(outputBytes);
    }

    private static string Decrypt(string data, byte[] key, byte[] iv)
    {
        try
        {
            var inputBytes = Convert.FromBase64String(data);
            var engine = new RijndaelEngine(256);
            var blockCipher = new CbcBlockCipher(engine);
            CipherUtilities.GetCipher("AES/CTR/NoPadding");
            IBufferedCipher cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());
            var keyParam = new KeyParameter(key);
            var keyParamWithIv = new ParametersWithIV(keyParam, iv, 0, 32);
            cipher.Init(false, keyParamWithIv);
            var outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
            var length = cipher.ProcessBytes(inputBytes, outputBytes, 0);
            cipher.DoFinal(outputBytes, length);
            return Encoding.UTF8.GetString(outputBytes).Split('\0')[0];
        }
        catch (Exception ex)
        {
            return data;
        }
    }
}
