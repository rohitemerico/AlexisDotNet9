using Newtonsoft.Json;

namespace Alexis.Dashboard.Helper;

public static class SessionHelper
{
    public static void SetObject<T>(ISession session, string key, T value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObject<T>(ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }
}