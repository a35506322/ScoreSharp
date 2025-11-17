namespace ScoreSharp.API.Modules.Reviewer3rd.GetNameCheck;

public class GetNameCheckRequest
{
    /// <summary>
    /// 中文姓名
    /// 對於附卡人，此姓名將用於識別特定的附卡人資料
    /// </summary>
    public string CHName { get; set; } = null!;

    /// <summary>
    /// 正附卡類型
    /// 1 = 正卡人, 2 = 附卡人
    /// </summary>
    public UserType UserType { get; set; }
}

public class GetUserTypeIDDto
{
    public bool IsError => ID == null;

    public string? ID { get; set; }

    public string? ErrorMessage { get; set; }
}
