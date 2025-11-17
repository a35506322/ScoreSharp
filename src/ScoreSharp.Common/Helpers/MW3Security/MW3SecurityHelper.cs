namespace ScoreSharp.Common.Helpers.MW3Security;

public class MW3SecurityHelper : IMW3SecurityHelper
{
    private readonly string _key;
    private readonly string _iv;

    public MW3SecurityHelper(string key, string iv)
    {
        this._key = key;
        this._iv = iv;
    }

    /// <summary>
    /// 驗證key和iv的長度
    /// </summary>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    private static void Validate_KeyIV_Length(string key, string iv)
    {
        //驗證key和iv都必須為128bits或192bits或256bits
        List<int> LegalSizes = new List<int>() { 128, 192, 256 };
        int keyBitSize = Encoding.UTF8.GetBytes(key).Length * 8;
        int ivBitSize = Encoding.UTF8.GetBytes(iv).Length * 8;
        if (!LegalSizes.Contains(keyBitSize) || !LegalSizes.Contains(ivBitSize))
        {
            throw new Exception($@"key或iv的長度不在128bits、192bits、256bits其中一個，輸入的key bits:{keyBitSize},iv bits:{ivBitSize}");
        }
    }

    /// <summary>
    /// 加密後回傳base64String，相同明碼文字編碼後的base64String結果會相同(類似雜湊)，除非變更key或iv
    /// </summary>
    /// <param name="plain_text"></param>
    /// <returns></returns>
    public string AESEncrypt(string plain_text)
    {
        Validate_KeyIV_Length(_key, _iv);
        Aes aes = Aes.Create();
        aes.Mode = CipherMode.CBC; //非必須，但加了較安全
        aes.Padding = PaddingMode.PKCS7; //非必須，但加了較安全

        ICryptoTransform transform = aes.CreateEncryptor(Encoding.UTF8.GetBytes(_key), Encoding.UTF8.GetBytes(_iv));

        byte[] bPlainText = Encoding.UTF8.GetBytes(plain_text); //明碼文字轉byte[]
        byte[] outputData = transform.TransformFinalBlock(bPlainText, 0, bPlainText.Length); //加密
        return Convert.ToBase64String(outputData);
    }

    /// <summary>
    /// 解密後，回傳明碼文字
    /// </summary>
    /// <param name="base64String"></param>
    /// <returns></returns>
    public string AESDecrypt(string base64String)
    {
        byte[] bEnBase64String = null;
        byte[] outputData = null;
        try
        {
            Validate_KeyIV_Length(_key, _iv);
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC; //非必須，但加了較安全
            aes.Padding = PaddingMode.PKCS7; //非必須，但加了較安全
            ICryptoTransform transform = aes.CreateDecryptor(Encoding.UTF8.GetBytes(_key), Encoding.UTF8.GetBytes(_iv));
            bEnBase64String = Convert.FromBase64String(base64String); //有可能base64String格式錯誤
            outputData = transform.TransformFinalBlock(bEnBase64String, 0, bEnBase64String.Length); //有可能解密出錯
        }
        catch (Exception)
        {
            throw new Exception($@"解密出錯，請確認加密資料是否正確。");
        }

        //解密成功
        return Encoding.UTF8.GetString(outputData);
    }
}
