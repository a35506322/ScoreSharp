namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class VerifyContext
{
    public bool 是否檢核IP相同 { get; set; }
    public bool 是否檢核行內IP { get; set; }
    public bool 是否檢核網路電子郵件 { get; set; }
    public bool 是否檢核網路手機號碼 { get; set; }
    public bool 是否檢核929 { get; set; }
    public bool 是否檢核分行資訊 { get; set; }
    public bool 是否檢核關注名單 { get; set; }
    public bool 是否檢核姓名檢查 { get; set; }
    public bool 是否檢查短時間ID相同 { get; set; }
    public bool 是否檢核行內Email { get; set; }
    public bool 是否檢核行內手機 { get; set; }
    public bool 是否檢查重覆進件 { get; set; }
}
