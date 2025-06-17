using System.Data;
using MDM.iOS.Entities.MDM;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Restriction
{
    /// <summary>
    /// Restriction abstract class and implementation 
    /// </summary>
    public abstract class MDM_RestrictionBase
    {
        public abstract DataTable GetRestrictionMenu();
        public abstract DataTable GetRestrictionMenuGroup();
        public abstract En_MDM_Profile Get_IpadProfile_Detail_DT(En_MDM_Profile My_En_MDM_Profile);
        public abstract En_MDM_Profile Get_IpadProfile_Detail_Restriction(En_MDM_Profile My_En_MDM_Profile);
        public abstract En_MDM_Profile Get_IpadProfile_Detail_Restriction_Group(En_MDM_Profile My_En_MDM_Profile);
        public abstract En_MDM_Profile Get_IpadProfile_Detail_Restriction_MDM(En_MDM_Profile My_En_MDM_Profile);
    }
}
