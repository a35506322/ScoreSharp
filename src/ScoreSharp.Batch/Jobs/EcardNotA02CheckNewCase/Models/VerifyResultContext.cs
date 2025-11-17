namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class VerifyResultContext
{
    public bool 檢核929成功 { get; set; }
    public bool 檢核分行資訊成功 { get; set; }
    public bool 檢核關注名單成功 { get; set; }
    public bool 檢核IP相同成功 { get; set; }
    public bool 檢核行內IP成功 { get; set; }
    public bool 檢核電子郵件成功 { get; set; }
    public bool 檢核手機號碼成功 { get; set; }
    public bool 檢核短時間ID相同成功 { get; set; }
    public bool 檢核姓名檢核成功 { get; set; }
    public bool 命中929 { get; set; }
    public bool 命中分行資訊 { get; set; }
    public bool 命中關注名單1 { get; set; }
    public bool 命中關注名單2 { get; set; }
    public bool 命中IP相同 { get; set; }
    public bool 命中行內IP { get; set; }
    public bool 命中電子郵件 { get; set; }
    public bool 命中手機號碼 { get; set; }
    public bool 命中短時間ID相同 { get; set; }
    public bool 命中姓名 { get; set; }

    public bool 是否檢核行內Email成功 { get; set; }
    public bool 是否檢核行內手機成功 { get; set; }
    public bool 命中行內Email { get; set; }
    public bool 命中行內手機 { get; set; }

    public bool 檢核重複進件成功 { get; set; }
    public bool 命中重複進件 { get; set; }
}
