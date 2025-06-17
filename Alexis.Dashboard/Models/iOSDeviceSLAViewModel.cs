namespace Alexis.Dashboard.Models;

public class iOSDeviceSLAViewModel
{
    public string TblMachineMachineSerial { get; set; }
    public string MachineName { get; set; }
    public DateTime Uptime { get; set; }
    public DateTime Downtime { get; set; }
    public DateTime LastUptime { get; set; }
    public decimal Uptime_Seconds { get; set; }
    public decimal Downtime_Seconds { get; set; }
}

public class AndroidDeviceSLAViewModel
{
    public string ID { get; set; }
    public string deviceMACAdd { get; set; }
    public string DeviceName { get; set; }
    public DateTime Uptime { get; set; }
    public DateTime Downtime { get; set; }
    public DateTime LastUptime { get; set; }
    public decimal Uptime_Seconds { get; set; }
    public decimal Downtime_Seconds { get; set; }
}

public class WindowsDeviceSLAViewModel
{
    public string DeviceID { get; set; }
    public string SMBIOSSerialNumber { get; set; }
    public string DNSComputerName { get; set; }
    public DateTime Uptime { get; set; }
    public DateTime Downtime { get; set; }
    public DateTime LastUptime { get; set; }
    public decimal Uptime_Seconds { get; set; }
    public decimal Downtime_Seconds { get; set; }
}

public class AndroidDeviceLocationSummaryViewModel
{
    public string ID { get; set; }
    public string deviceMACAdd { get; set; }
    public string DeviceName { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime SyncTime { get; set; }
}
public class WindowsDeviceLocationSummaryViewModel
{
    public string ID { get; set; }
    public string SMBIOSSERIALNUMBER { get; set; }
    public string DNSCOMPUTERNAME { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime SyncTime { get; set; }
}

public class AndroidAppInstallationSummaryViewModel
{
    public string ID { get; set; }
    public string deviceMACAdd { get; set; }
    public string DeviceName { get; set; }
    public string Application { get; set; }
    public string Action { get; set; }
    public DateTime Created_On { get; set; }
}


public class iOSAppInstallationSummaryViewModel
{

    public string MachineSerial { get; set; }
    public string MachineName { get; set; }
    public string Application { get; set; }
    public string Action { get; set; }
    public DateTime CREATEDDATE { get; set; }
}

public class WindowsAppInstallationSummaryViewModel
{

    public string SMBIOSSERIALNUMBER { get; set; }
    public string DNSComputerName { get; set; }
    public string Application { get; set; }
    public string Action { get; set; }
    public DateTime CREATEDDATE { get; set; }
}


public class iOSDeviceLocationSummaryViewModel
{
    public string MachineSerial { get; set; }
    public string MachineName { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime SyncTime { get; set; }
}