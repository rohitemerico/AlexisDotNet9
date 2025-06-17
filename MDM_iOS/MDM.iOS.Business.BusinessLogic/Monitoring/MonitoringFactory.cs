public class MonitoringFactory
{
    /// <summary>
    /// Creates a monitoring class base on the provider.
    /// Currently its a defaulted class.
    /// </summary>
    /// <param name="Provider"></param>
    /// <returns></returns>
    public static MonitoringBase Create(string Provider)
    {
        switch (Provider)
        {
            default:
                return new MonitoringDefault();
        }
    }
}
