namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models
{
    public class QueryCheckName : BaseResDto
    {
        /// <summary>
        /// 姓名檢核查詢結果
        /// </summary>
        public Reviewer3rd_NameCheckLog Reviewer3rd_NameCheckLog { get; set; } = new();
        public string 是否命中 => RtnCode == MW3RtnCodeConst.查詢姓名檢核_命中 ? "Y" : "N";
    }
}
