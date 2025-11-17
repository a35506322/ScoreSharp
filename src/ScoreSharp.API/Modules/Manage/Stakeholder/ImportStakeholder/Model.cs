namespace ScoreSharp.API.Modules.Manage.Stakeholder.ImportStakeholder;

public class ImportStakeholderRequest
{
    public IFormFile File { get; set; }
}

public class StackeholderCsv
{
    public string ID { get; set; }
    public string UserId { get; set; }
    public string AddUserId { get; set; }
    public DateTime AddTime { get; set; }
    public string IsActive { get; set; }
}
