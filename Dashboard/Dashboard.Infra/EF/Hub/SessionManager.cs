using System.Text.Json;
using Alexis.Common;
using Dashboard.Infra.EF.Models.UserRoleBranch;
using Microsoft.AspNetCore.Http;

namespace Dashboard.Infra.EF.Hub;

public class SessionManager
{
    private const string SessionKeyClient = "CLIENT_SESSION_DETAILS";
    private const string SessionKeyUser = "USER_SESSION_DETAILS";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public ClientIdentity ClientSession
    {
        get { return Get<ClientIdentity>(SessionKeyClient); }
        set { Set(SessionKeyClient, value); }
    }

    public USER_LOGIN UserSession
    {
        get { return Get<USER_LOGIN>(SessionKeyUser); }
        set { Set(SessionKeyUser, value); }
    }

    public async Task<ClientIdentity> GetClientInfoAsync(string tenantId)
    {
        var mutlitenancyAPI = ConfigHelper.MultiTenancyAPI;
        var result = new ClientIdentity();

        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync(mutlitenancyAPI + "/getTenantInfo?tid=" + tenantId))
            {
                if (!response.IsSuccessStatusCode) return null;

                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<ClientIdentity>(apiResponse);
                //result = JsonConvert.DeserializeObject<ClientIdentity>(apiResponse);
            }
        }
        return result;
    }

    /// <summary> Gets. </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /// <param name="key"> The key. </param>
    /// <returns> . </returns>
    private T Get<T>(string key)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null)
            return default;
        object o = session.GetString(key);
        if (o is T)
        {
            return (T)o;
        }

        return default;
    }

    /// <summary> Sets. </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    /// <param name="key">  The key. </param>
    /// <param name="item"> The item. </param>
    public void Set<T>(string key, T item)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return;

        var json = JsonSerializer.Serialize(item);
        session.SetString(key, json); // Don't assign this to a variable
    }
}

public class ClientIdentity
{
    public string Name { get; set; }
    public string TID { get; set; } //tenantId
    public string ConnectionString { get; set; }
    public string StoragePath { get; set; }
    public string DataFolder { get; set; }
}
