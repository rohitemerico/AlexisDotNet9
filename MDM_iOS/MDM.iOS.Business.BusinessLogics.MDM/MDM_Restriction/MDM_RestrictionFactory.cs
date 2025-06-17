namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Restriction
{
    public class MDM_RestrictionFactory
    {
        public static MDM_RestrictionBase Create(string Provider)
        {
            switch (Provider)
            {
                default:
                    return new MDM_RestrictionDefault();
            }
        }
    }
}
