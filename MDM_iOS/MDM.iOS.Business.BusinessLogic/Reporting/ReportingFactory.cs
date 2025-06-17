namespace MDM.iOS.Business.BusinessLogic.Reporting
{
    public class ReportingFactory
    {
        /// <summary>
        /// Creates a monitoring class base on the provider.
        /// Currently its a defaulted class.
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public static ReportingBase Create(string Provider)
        {
            switch (Provider)
            {
                default:
                    return new ReportingDefault();
            }
        }
    }
}

