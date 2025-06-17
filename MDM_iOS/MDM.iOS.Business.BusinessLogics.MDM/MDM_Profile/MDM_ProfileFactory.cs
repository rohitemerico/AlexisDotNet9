namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Profile
{
    public class MDM_ProfileFactory
    {
        public static MDM_ProfileBase Create(string Provider)
        {
            switch (Provider)
            {
                default:
                    return new MDM_ProfileDefault();
            }
        }
    }
}
