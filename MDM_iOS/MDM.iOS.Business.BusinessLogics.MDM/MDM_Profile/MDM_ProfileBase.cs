using System.Data;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Entities.MDM;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Profile
{
    public abstract class MDM_ProfileBase
    {
        public abstract DataTable GetAllProfile();
        public abstract DataTable GetProfiles();
        public abstract DataTable GetProfile(En_MDM_Profile My_En_MDM_Profile);

        public abstract DataTable GetProfileByBranch(Guid BranchId);

        public abstract DataTable GetBranchByProfileId(Guid ProfileId);

        public abstract bool UpdateProfileGeneralByUpdateType(MDM_Profile_General MY_MDM_Profile_General, string UpdateType);

        public abstract bool UpdateProfileGeneralBranchByID(MDM_Profile_General_BranchID MY_MDM_Profile_General_BranchID);
    }
}
