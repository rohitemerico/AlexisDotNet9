using System.Data;
using Dashboard.Entities.ADCB;
using MDM.iOS.Entities;
using MDM.iOS.Entities.Generic;
using MDM.iOS.Entities.MDM;
using MDM.iOS.Entities.Monitoring;

public abstract class MonitoringBase : IDisposable
{
    public abstract void Dispose();

    public abstract bool IsAuthenticated(GenericRequestEn login);

    public abstract string GetLdapInfo();

    public abstract DeviceEn GetDeviceDetails(string imei);

    public abstract DataTable GetDeviceDetails_dt(DeviceEn device);

    public abstract DataTable GetDeviceDetails_UDID_dt(DeviceEn device);

    public abstract DataTable GetDeviceDetails_All_Active(List<Params> myParams);

    public abstract DataTable GetDeviceDetailForDisplay(List<Params> Params);

    public abstract DataTable GetDeviceDetailCerts(DeviceCertificateEn cert);

    public abstract DataTable GetDeviceDetailInstalledApps(InstalledAppEn app);

    public abstract DataTable GetDeviceDetailOSUpdates(OSUpdateEn update);

    public abstract bool UpdateDeviceDetailSecurityInfo(DeviceEn device);

    public abstract bool InsertDeviceCerts(DeviceCertificateEn cert);

    public abstract bool InsertDeviceApps(InstalledAppEn app);

    public abstract bool InsertDeviceOSUpdates(OSUpdateEn update);

    public abstract bool DeleteDeviceCerts(DeviceCertificateEn cert);

    public abstract bool DeleteDeviceApps(InstalledAppEn app);

    public abstract bool DeleteDeviceOSUpdates(OSUpdateEn update);

    public abstract DataTable GetDeviceApps(InstalledAppEn app);
    public abstract DataTable GetDeviceCerts(DeviceCertificateEn cert);

    public abstract DataTable GetDevicePendingUpdates(OSUpdateEn update);


    // public abstract DataTable GetDeviceDetails_All_Active_ByBranch(DeviceEn my_DeviceEn);

    public abstract bool UpdateLostMode(string deviceSerial, bool lostModeEnabled);

    public abstract bool UpdateSingleAppMode(string MachineSerial, bool singleAppModeEnabled);

    public abstract bool UpdateLostLatLng(string lat, string lng, string UDID);

    public abstract DataTable GetLostLatLng(string UDID);

    public abstract DataTable GetLocationHistory(List<Params> myParams);

    public abstract DataTable GetipadOnlineOfflineCount(string onOroff);


    public abstract bool InsertDevice(DeviceEn device);

    public abstract bool UpdateDevice_ws(DeviceEn device);

    public abstract bool UpdateDevice_ws_byUDID(DeviceEn device);

    public abstract bool UpdateDeviceComponentStatus(DeviceEn device);


    public abstract bool UpdateOSInstallStatusbyMachine(OSUpdateEn update);

    public abstract bool CheckEndTime(string imei);

    public abstract bool InsertMonitoringActivity(string imei);

    public abstract MonitoringActivityEn GetLastMonitoringActivity(string imei);

    public abstract bool UpdateMonitoringActivity(string activityID, string imei);

    public abstract DataTable GetBranchesWithProfiles(List<Params> Params);

    public abstract bool UpdateFirmwareAppDeviceToken(DeviceEn device);

    public abstract string GetFirmwareAppDeviceToken(DeviceEn device);

    public abstract List<string> GetAllFirmwareAppDeviceToken();


}
