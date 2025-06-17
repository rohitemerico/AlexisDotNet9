using System.Data;
using MDM.iOS.Entities.MDM;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_App
{
    public abstract class MDM_AppBase
    {
        public abstract DataTable GetMDM_APP(string paramsfilter);

        public abstract DataTable GetMDM_APP_APPID(Guid My_AppID);

        public abstract DataTable GetMDM_APP();

        public abstract bool CheckNameExists(En_MDMApp My_En_MDMApp);

        public abstract bool AppInsertIntoDb(En_MDMApp My_En_MDMApp);

        public abstract bool ApproveApplication(Guid appID, string identifier);

        public abstract DataTable checkIdentifierNotPending(string Identifier);

        public abstract void DisabledPreviousApplication(DataTable dt);

        public abstract bool DeclineApplication(Guid Appid);

        public abstract bool AppInstallSummaryInsertIntoDb(En_MDMAppInstallationSummary summary);

    }
}
