using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.SetUp.Card.UpdateCardById;

public class UpdateCardByIdRequest : IValidatableObject
{
    /// <summary>
    /// BIN，長度8碼
    /// </summary>
    [Display(Name = "BIN")]
    [MaxLength(8)]
    [Required]
    public string BINCode { get; set; }

    /// <summary>
    /// 卡片代碼，範例：JST65
    /// </summary>
    [Display(Name = "卡片代碼")]
    [MaxLength(10)]
    [Required]
    public string CardCode { get; set; }

    /// <summary>
    /// 卡片名稱
    /// </summary>
    [Display(Name = "卡片名稱")]
    [Required]
    public string CardName { get; set; }

    /// <summary>
    /// 卡片類別，一般發卡、國民現金卡、消、現金卡代償
    /// </summary>
    [ValidEnumValue]
    [Display(Name = "卡片類別")]
    [Required]
    public CardCategory CardCategory { get; set; }

    /// <summary>
    /// 拒件函樣板，拒件函 (信用卡) 、拒件函 (消貸)、拒件函 (代償)
    /// </summary>
    [ValidEnumValue]
    [Display(Name = "拒件函樣板")]
    [Required]
    public SampleRejectionLetter SampleRejectionLetter { get; set; }

    /// <summary>
    /// 預設帳單日，關聯 SetUp_BillDay
    /// </summary>
    [Display(Name = "預設帳單日")]
    [RegularExpression("^(0[1-9]|1[0-9]|2[0-9]|3[01])$")]
    [Required]
    public string DefaultBillDay { get; set; }

    /// <summary>
    /// 銷貸類別，0:代償、1:銷貸、2:其他
    /// </summary>
    [ValidEnumValue]
    [Display(Name = "銷貸類別")]
    [Required]
    public SaleLoanCategory SaleLoanCategory { get; set; }

    /// <summary>
    /// 預設優惠辦法，關聯 SetUp_CardPromotion
    /// </summary>
    [Display(Name = "預設優惠辦法")]
    [RegularExpression("^(?!0000)\\d{4}$")]
    [Required]
    public string DefaultDiscount { get; set; }

    /// <summary>
    /// 是否啟用，Y | N
    /// </summary>
    [Display(Name = "是否啟用")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsActive { get; set; }

    /// <summary>
    /// 主卡額度上限，範例：3000000
    /// </summary>
    [Display(Name = "主卡額度上限")]
    [Required]
    public int PrimaryCardQuotaUpperlimit { get; set; }

    /// <summary>
    /// 主卡額度下限，範例：10000
    /// </summary>
    [Display(Name = "主卡額度下限")]
    [Required]
    public int PrimaryCardQuotaLowerlimit { get; set; }

    /// <summary>
    /// 主卡年齡上限，範例：20
    /// </summary>
    [Display(Name = "主卡年齡上限")]
    [Required]
    public int PrimaryCardYearUpperlimit { get; set; }

    /// <summary>
    /// 主卡年齡下限，範例：99
    /// </summary>
    [Display(Name = "主卡年齡下限")]
    [Required]
    public int PrimaryCardYearLowerlimit { get; set; }

    /// <summary>
    /// 附卡額度上限，範例：3000000
    /// </summary>
    [Display(Name = "附卡額度上限")]
    [Required]
    public int SupplementaryCardQuotaUpperlimit { get; set; }

    /// <summary>
    /// 附卡額度下限，範例：10000
    /// </summary>
    [Display(Name = "附卡額度下限")]
    [Required]
    public int SupplementaryCardQuotaLowerlimit { get; set; }

    /// <summary>
    /// 附卡年齡上限，範例：20
    /// </summary>
    [Display(Name = "附卡年齡上限")]
    [Required]
    public int SupplementaryCardYearUpperlimit { get; set; }

    /// <summary>
    /// 附卡年齡下限，範例：99
    /// </summary>
    [Display(Name = "附卡年齡下限")]
    [Required]
    public int SupplementaryCardYearLowerlimit { get; set; }

    /// <summary>
    /// 是否不大於CARDPAC額度限制，Y | N
    /// </summary>
    [Display(Name = "是否不大於CARDPAC額度限制")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsCARDPAUnderLimit { get; set; }

    /// <summary>
    /// CARDPAC額度限制，20
    /// </summary>
    [Display(Name = "CARDPAC額度限制")]
    [Required]
    public int CARDPACQuotaLimit { get; set; }

    /// <summary>
    /// 不得申請辦附卡，Y | N，Y :  是不能申請
    /// </summary>
    [Display(Name = "不得申請辦附卡")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsApplyAdditionalCard { get; set; }

    /// <summary>
    /// 是否獨立卡別，Y | N
    /// </summary>
    [Display(Name = "是否獨立卡別")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsIndependentCard { get; set; }

    /// <summary>
    /// 是否作IVR/CTI查詢，Y | N
    /// </summary>
    [Display(Name = "是否作IVR/CTI查詢")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsIVRvCTIQuery { get; set; }

    /// <summary>
    /// 國旅卡，Y | N
    /// </summary>
    [Display(Name = "國旅卡")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsCITSCard { get; set; }

    /// <summary>
    /// 快速發卡，Y | N
    /// </summary>
    [Display(Name = "快速發卡")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsQuickCardIssuance { get; set; }

    /// <summary>
    /// 票證功能，Y | N
    /// </summary>
    [Display(Name = "票證功能")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsTicket { get; set; }

    /// <summary>
    /// 聯名集團，Y | N
    /// </summary>
    [Display(Name = "聯名集團")]
    [RegularExpression("[YN]")]
    [Required]
    public string IsJointGroup { get; set; }

    /// <summary>
    /// 可選優惠辦法
    /// </summary>
    [Display(Name = "可選優惠辦法")]
    public List<string> OptionalCardPromotions { get; set; } = new List<string>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (OptionalCardPromotions.Count() == 0)
        {
            yield return new ValidationResult("可選優惠辦法至少為一個", new[] { this.GetDisplayName(nameof(OptionalCardPromotions)) });
        }
        else
        {
            if (!OptionalCardPromotions.Contains(DefaultDiscount))
            {
                yield return new ValidationResult("不在可選優惠辦法名單中", new[] { this.GetDisplayName(nameof(DefaultDiscount)) });
            }
        }
    }
}
