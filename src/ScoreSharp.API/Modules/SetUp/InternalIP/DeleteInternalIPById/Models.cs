namespace ScoreSharp.API.Modules.SetUp.InternalIP.DeleteInternalIPById
{
    public class DeleteInternalIPByIdRequest
    {
        /// <summary>
        /// PK，範例 : 172.28.234.10
        /// </summary>
        [Display(Name = "IP")]
        [Required]
        [MaxLength(20)]
        public string IP { get; set; }
    }
}
