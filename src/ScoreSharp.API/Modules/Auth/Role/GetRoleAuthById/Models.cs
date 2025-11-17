namespace ScoreSharp.API.Modules.Auth.Role.GetRoleAuthById;

public class GetRoleAuthByIdResponse
{
    public string RouterCategoryId { get; set; }
    public string RouterCategoryName { get; set; }
    public List<Router> Routers { get; set; }
}

public class Router
{
    public string RouterId { get; set; }
    public string RouterName { get; set; }
    public List<Action> Actions { get; set; }
}

public class Action
{
    public string ActionId { get; set; }
    public string ActionName { get; set; }
    public string HasPermission { get; set; }
}

public class RoleAuthDto
{
    public string RouterCategoryId { get; set; }
    public string RouterCategoryName { get; set; }
    public string RouterId { get; set; }
    public string RouterName { get; set; }
    public string ActionId { get; set; }
    public string ActionName { get; set; }
}
