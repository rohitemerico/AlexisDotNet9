namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_App
{
    public class MDM_AppFactory
    {
        public static MDM_AppBase Create(string Provider)
        {
            switch (Provider)
            {
                default:
                    return new MDM_AppDefault();
            }
        }

    }
}
