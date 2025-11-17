namespace ScoreSharp.PaperMiddleware.Modules.Test.PostTest;

public class PostTestRequest
{
    [Required]
    [Display(Name = "姓名")]
    [MaxLength(3)]
    public string Name { get; set; }

    [Required]
    [Display(Name = "年紀")]
    public int? Age { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "電子信箱")]
    public string Email { get; set; }
}

public class PostTestResponse
{
    public string Name { get; set; }
    public int? Age { get; set; }
    public string Email { get; set; }
}
