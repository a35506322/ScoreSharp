using ScoreSharp.API.Infrastructures.Adapter.Models;
using LDAPUserInfo = ScoreSharp.API.Infrastructures.Adapter.Models.LDAPUserInfo;

namespace ScoreSharp.API.Infrastructures.Adapter;

public interface ILDAPAdapter
{
    public Task<LDAPAdapterResponse<bool>> ValidateLDAPAuth(string username, string mima);
    public Task<LDAPAdapterResponse<LDAPUserInfo>> SearchBySAMAccountName(string samAccountName);
    public Task<LDAPAdapterResponse<IEnumerable<LDAPUserInfo>>> SearchUsersAll();
}
