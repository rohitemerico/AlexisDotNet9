using System.Data;

namespace MDM.iOS.Business.BusinessLogic.Reporting
{
    public abstract class ReportingBase : IDisposable
    {
        public abstract void Dispose();
        public abstract DataTable GetAllDevices_Report();
        public abstract DataTable GetDeviceCert_Report(string deviceUDID);
        public abstract DataTable GetAllDeviceGroups_Report();
        public abstract DataTable GetAllApp_Report();
        public abstract DataTable GetAllProfiles_Report();

    }
}
