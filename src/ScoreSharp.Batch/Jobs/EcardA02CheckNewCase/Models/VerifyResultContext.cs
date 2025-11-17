namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

public class VerifyResultContext
{
    public bool 是否查詢原持卡人成功 { get; set; }
    public bool 是否檢核929成功 { get; set; }
    public bool 是否檢核行內Email成功 { get; set; }
    public bool 是否檢核行內手機成功 { get; set; }
    public bool 是否檢核IP相同成功 { get; set; }
    public bool 是否檢核行內IP成功 { get; set; }
    public bool 是否檢核網路電子郵件成功 { get; set; }
    public bool 是否檢核網路手機成功 { get; set; }
    public bool 是否檢核關注名單成功 { get; set; }
    public bool 是否檢查短時間ID成功 { get; set; }
    public bool 是否檢查黑名單成功 { get; set; }
    public bool 是否檢查重覆進件成功 { get; set; }
    public bool 命中929 { get; set; }
    public bool 命中行內Email { get; set; }
    public bool 命中行內手機 { get; set; }
    public bool 命中關注名單1 { get; set; }
    public bool 命中關注名單2 { get; set; }
    public bool 命中IP相同 { get; set; }
    public bool 命中行內IP { get; set; }
    public bool 命中網路電子郵件相同 { get; set; }
    public bool 命中網路手機相同 { get; set; }
    public bool 命中短時間ID相同 { get; set; }
    public bool 命中黑名單 { get; set; }
    public bool 命中重覆進件 { get; set; }
    public bool 郵遞區號計算成功 { get; set; }
}
