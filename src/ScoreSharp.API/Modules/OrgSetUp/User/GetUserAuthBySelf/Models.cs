namespace ScoreSharp.API.Modules.OrgSetUp.User.GetUserAuthBySelf;

public class GetUserAuthByIdResponse
{
    public string routerCategoryId { get; set; }
    public string routerCategoryName { get; set; }
    public string? icon { get; set; }
    public List<Router> routers { get; set; }
}

public class Router
{
    public string routerId { get; set; }
    public string routerName { get; set; }
    public string? icon { get; set; }
    public List<Action> actions { get; set; }
}

public class Action
{
    public string actionId { get; set; }
    public string actionName { get; set; }
}

public class UserAuthDto
{
    public string RouterCategoryId { get; set; }
    public string RouterCategoryName { get; set; }
    public string CategoryIcon { get; set; }
    public string RouterId { get; set; }
    public string RouterName { get; set; }
    public string RouterIcon { get; set; }
    public string ActionId { get; set; }
    public string ActionName { get; set; }
}
