namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.UpdateHighFinancialSecrecyCountryById
{
    public class UpdateHighFinancialSecrecyCountryByIdRequest
    {
        /// <summary>
        /// 高金融保密國家代碼，舉例：TW
        /// </summary>
        [Display(Name = "高金融保密國家代碼")]
        [RegularExpression(@"^[A-Z]+$")]
        [MaxLength(5)]
        [Required]
        public string HighFinancialSecrecyCountryCode { get; set; }

        /// <summary>
        /// 高金融保密國家名稱
        /// </summary>
        [Display(Name = "高金融保密國家名稱")]
        [MaxLength(50)]
        [Required]
        public string HighFinancialSecrecyCountryName { get; set; }

        /// <summary>
        /// 是否啟用，範例：Y | N
        /// </summary>
        [Display(Name = "是否啟用")]
        [RegularExpression("[YN]")]
        [Required]
        public string IsActive { get; set; }
    }
}
