namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Machine
{
    public class MDM_MachineFactory
    {
        public static MDM_MachineBase Create(string Provider)
        {
            switch (Provider)
            {
                default:
                    return new MDM_MachineDefault();
            }
        }
    }
}
