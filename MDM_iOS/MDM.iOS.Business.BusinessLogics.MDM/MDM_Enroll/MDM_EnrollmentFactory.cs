namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Enroll
{
    public class MDM_EnrollmentFactory
    {
        public static MDM_EnrollmentBase Create(string Provider)
        {
            switch (Provider)
            {
                default:
                    return new MDM_EnrollmentDefault();
            }
        }
    }
}
