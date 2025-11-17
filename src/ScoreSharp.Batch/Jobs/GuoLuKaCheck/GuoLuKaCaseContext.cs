namespace ScoreSharp.Batch.Jobs.GuoLuKaCheck;

public class GuoLuKaCaseContext
{
    public string ApplyNo { get; set; } = null!;

    public string ID { get; set; } = null!;

    public CardStatus CardStatus { get; set; }

    public DateTime ApplyDate { get; set; }
}
