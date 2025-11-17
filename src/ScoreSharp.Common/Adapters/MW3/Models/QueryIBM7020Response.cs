namespace ScoreSharp.Common.Adapters.MW3.Models;

public class QueryIBM7020Response
{
    [JsonPropertyName("senD_C_CODE")]
    public string Code { get; set; }

    [JsonPropertyName("senD_C_TERM_ID")]
    public string TermID { get; set; }

    [JsonPropertyName("senD_C_RETN_CODE")]
    public string RetnCode { get; set; }

    [JsonPropertyName("senD_C_AVAIL_CASH_SIGN")]
    public string AvailCashSign { get; set; }

    [JsonPropertyName("senD_C_AVAIL_CASH")]
    public string AvailCash { get; set; }

    [JsonPropertyName("senD_C_AVAIL_CREDIT_SIGN")]
    public string AvailCreditSign { get; set; }

    [JsonPropertyName("senD_C_AVAIL_CREDIT")]
    public string AvailCredit { get; set; }

    [JsonPropertyName("senD_C_CO_BIRTH_YYYY")]
    public string CoBirthYYYY { get; set; }

    [JsonPropertyName("senD_C_CO_BIRTH_MM")]
    public string CoBirthMM { get; set; }

    [JsonPropertyName("senD_C_CO_BIRTH_DD")]
    public string CoBirthDD { get; set; }

    [JsonPropertyName("senD_C_MEMO")]
    public string Memo { get; set; }

    [JsonPropertyName("filler")]
    public string Filler { get; set; }

    [JsonPropertyName("senD_H_REC")]
    public List<Rec> Rec { get; set; }
}

public class Rec
{
    [JsonPropertyName("senD_H_CODE")]
    public string Code { get; set; }

    [JsonPropertyName("senD_H_TERM_ID")]
    public string TermID { get; set; }

    [JsonPropertyName("senD_H_RETN_CODE")]
    public string RetnCode { get; set; }

    [JsonPropertyName("senD_H_CARD_NMBR")]
    public string CardNmbr { get; set; }

    [JsonPropertyName("senD_H_CARD_1")]
    public string Card1 { get; set; }

    [JsonPropertyName("senD_H_CARD_2")]
    public string Card2 { get; set; }

    [JsonPropertyName("senD_H_CARD_3")]
    public string Card3 { get; set; }

    [JsonPropertyName("senD_H_CARD_4")]
    public string Card4 { get; set; }

    [JsonPropertyName("senD_H_CARD_CRLIMIT")]
    public string CardCrlimit { get; set; }

    [JsonPropertyName("senD_H_OPENED_YYYY")]
    public string OpendYYYY { get; set; }

    [JsonPropertyName("senD_H_OPENED_MM")]
    public string OpendMM { get; set; }

    [JsonPropertyName("senD_H_OPENED_DD")]
    public string OpendDD { get; set; }

    [JsonPropertyName("senD_H_EXP_DATE")]
    public string ExpDate { get; set; }

    [JsonPropertyName("senD_H_CURR_BAL_SIGN")]
    public string CurrBalSign { get; set; }

    [JsonPropertyName("senD_H_CURR_BAL")]
    public string CurrBal { get; set; }

    [JsonPropertyName("senD_H_BLOCK_CODE")]
    public string BlockCode { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST1")]
    public string DelqHist1 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST2")]
    public string DelqHist2 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST3")]
    public string DelqHist3 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST4")]
    public string DelqHist4 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST5")]
    public string DelqHist5 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST6")]
    public string DelqHist6 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST7")]
    public string DelqHist7 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST8")]
    public string DelqHist8 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST9")]
    public string DelqHist9 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST10")]
    public string DelqHist10 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST11")]
    public string DelqHist11 { get; set; }

    [JsonPropertyName("senD_H_DELQ_HIST12")]
    public string DelqHist12 { get; set; }

    [JsonPropertyName("senD_H_LST_CRLIMIT_YYYY")]
    public string LstCrlimitYYYY { get; set; }

    [JsonPropertyName("senD_H_LST_CRLIMIT_MM")]
    public string LstCrlimitMM { get; set; }

    [JsonPropertyName("senD_H_LST_CRLIMIT_DD")]
    public string LstCrlimitDD { get; set; }

    [JsonPropertyName("senD_H_LST_CRLIMIT")]
    public string LstCrlimit { get; set; }

    [JsonPropertyName("senD_H_LST_PYMT_YYYY")]
    public string LstPymtYYYY { get; set; }

    [JsonPropertyName("senD_H_LST_PYMT_MM")]
    public string LstPymtMM { get; set; }

    [JsonPropertyName("senD_H_LST_PYMTT_DD")]
    public string LstPymtDD { get; set; }

    [JsonPropertyName("senD_H_LST_PYMT_AMNT")]
    public string LstPymtAmnt { get; set; }

    [JsonPropertyName("filler")]
    public string Filler { get; set; }
}
