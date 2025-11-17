using System.Net;
using Microsoft.Extensions.Options;
using ScoreSharp.API.Infrastructures.Adapter.Models;
using ScoreSharp.API.Infrastructures.Adapter.Options;
using LDAPUserInfo = ScoreSharp.API.Infrastructures.Adapter.Models.LDAPUserInfo;

namespace ScoreSharp.API.Infrastructures.Adapter;

public class LDAPAdapter : ILDAPAdapter
{
    private readonly HttpClient _httpClient;
    private readonly LDAPAdapterConfigurationOptions _ldapConfiguration;

    public LDAPAdapter(HttpClient httpClient, IOptions<LDAPAdapterConfigurationOptions> ldapConfiguration)
    {
        _httpClient = httpClient;
        _ldapConfiguration = ldapConfiguration.Value;

        _httpClient.BaseAddress = new Uri(_ldapConfiguration.BaseUrl);
    }

    public async Task<LDAPAdapterResponse<LDAPUserInfo>> SearchBySAMAccountName(string samAccountName)
    {
        try
        {
            var response = await _httpClient.GetAsync($"ldap/users/{samAccountName}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var searchBySAMAccountNameResponse = await response.Content.ReadFromJsonAsync<LDAPUserInfo>();
                return new LDAPAdapterResponse<LDAPUserInfo> { IsSuccess = true, Data = searchBySAMAccountNameResponse };
            }

            return new LDAPAdapterResponse<LDAPUserInfo> { IsSuccess = false, ErrorMessage = await response.Content.ReadAsStringAsync() };
        }
        catch (Exception ex)
        {
            return new LDAPAdapterResponse<LDAPUserInfo> { IsSuccess = false, ErrorMessage = ex.ToString() };
        }
    }

    public async Task<LDAPAdapterResponse<IEnumerable<LDAPUserInfo>>> SearchUsersAll()
    {
        try
        {
            var response = await _httpClient.GetAsync("ldap/users");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var searchUsersAllResponse = await response.Content.ReadFromJsonAsync<IEnumerable<LDAPUserInfo>>();
                return new LDAPAdapterResponse<IEnumerable<LDAPUserInfo>> { IsSuccess = true, Data = searchUsersAllResponse };
            }

            return new LDAPAdapterResponse<IEnumerable<LDAPUserInfo>>
            {
                IsSuccess = false,
                ErrorMessage = await response.Content.ReadAsStringAsync(),
            };
        }
        catch (Exception ex)
        {
            return new LDAPAdapterResponse<IEnumerable<LDAPUserInfo>> { IsSuccess = false, ErrorMessage = ex.ToString() };
        }
    }

    public async Task<LDAPAdapterResponse<bool>> ValidateLDAPAuth(string username, string mima)
    {
        try
        {
            ValidateLDAPAuthRequest request = new() { Username = username, Password = mima };

            var response = await _httpClient.PostAsJsonAsync("ldap/auth", request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var validateLDAPAuthResponse = await response.Content.ReadFromJsonAsync<bool>();
                return new LDAPAdapterResponse<bool> { IsSuccess = true, Data = validateLDAPAuthResponse };
            }

            return new LDAPAdapterResponse<bool> { IsSuccess = false, ErrorMessage = await response.Content.ReadAsStringAsync() };
        }
        catch (Exception ex)
        {
            return new LDAPAdapterResponse<bool> { IsSuccess = false, ErrorMessage = ex.ToString() };
        }
    }
}
