namespace ScoreSharp.API.Infrastructures.Security;

public interface IPermissionAuthorizationProvider
{
    /// <summary>
    /// 取得角色的權限政策
    /// 這邊角色只關心Action，不用管Rouer
    /// </summary>
    /// <param name="roleIds">token get</param>
    /// <returns></returns>
    Task<IEnumerable<string>> GetRoleWithActoinByRoleIds(string[] roleIds);

    /// <summary>
    /// 取得Action資料
    /// </summary>
    /// <param name="actionId"></param>
    /// <returns></returns>
    Task<ActionDto?> GetActionByActionId(string actionId);
}
