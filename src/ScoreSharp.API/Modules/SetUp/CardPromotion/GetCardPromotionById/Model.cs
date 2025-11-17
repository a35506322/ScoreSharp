namespace ScoreSharp.API.Modules.SetUp.CardPromotion.GetCardPromotionById
{
    public class GetCardPromotionByIdResponse
    {
        /// <summary>
        /// 優惠辦法代碼，範例 : 0001
        /// </summary>
        public string CardPromotionCode { get; set; } = null!;

        /// <summary>
        /// 優惠辦法名稱
        /// </summary>
        public string CardPromotionName { get; set; } = null!;

        /// <summary>
        /// 是否啟用，Y | N
        /// </summary>
        public string IsActive { get; set; } = null!;

        /// <summary>
        /// 正卡使用POT，範例 : 01
        /// </summary>
        public string PrimaryCardUsedPOT { get; set; } = null!;

        /// <summary>
        /// 附卡使用POT，範例 : 01
        /// </summary>
        public string SupplementaryCardUsedPOT { get; set; } = null!;

        /// <summary>
        /// 使用POT截止月份，範例 : 01
        /// </summary>
        public string UsedPOTExpiryMonth { get; set; } = null!;

        /// <summary>
        /// 正卡預留POT，範例 : 01
        /// </summary>
        public string? PrimaryCardReservedPOT { get; set; }

        /// <summary>
        /// 附卡預留POT，範例 : 01
        /// </summary>
        public string? SupplementaryCardReservedPOT { get; set; }

        /// <summary>
        /// 預留優惠期限(月)，範例 : 01
        /// </summary>
        public string? ReservePromotionPeriod { get; set; }

        /// <summary>
        /// 利率，範例 : 12.22
        /// </summary>
        public decimal? InterestRate { get; set; }

        /// <summary>
        /// 新增員工
        /// </summary>
        public string AddUserId { get; set; } = null!;

        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 修正員工
        /// </summary>
        public string? UpdateUserId { get; set; }

        /// <summary>
        /// 修正時間
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}
