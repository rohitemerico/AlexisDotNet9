namespace Alexis.Dashboard.Models;

public class AndroidDeviceDataUsageViewModel
{
    public string DEVICENAME { get; set; }
    public string deviceMACAdd { get; set; }
    public string? GroupName { get; set; }
    public decimal? MOBILE_DATA_USED { get; set; } = 0;  // In MB
    public decimal? WIFI_DATA_USED { get; set; } = 0;  // In MB
    public decimal? TOTAL_DATA_USED { get; set; } = 0;  // In MB
    public DateTime? LAST_UPDATED { get; set; }
    public bool DEVICESTATUS { get; set; }
}

public class iOSDeviceDataUsageViewModel
{
    public string MachineName { get; set; }
    public string MachineSerial { get; set; }
    public decimal? MOBILE_DATA_USED { get; set; } = 0;  // In MB
    public decimal? WIFI_DATA_USED { get; set; } = 0;  // In MB
    public decimal? TOTAL_DATA_USED { get; set; } = 0;  // In MB
    public DateTime? LAST_UPDATED { get; set; }
}

public class WindowsDeviceDataUsageViewModel
{
    public string DNSComputerName { get; set; }
    public string SMBIOSSerialNumber { get; set; }
    public decimal? MOBILE_DATA_USED { get; set; } = 0;  // In MB
    public decimal? WIFI_DATA_USED { get; set; } = 0;  // In MB
    public decimal? TOTAL_DATA_USED { get; set; } = 0;  // In MB
    public DateTime? LAST_UPDATED { get; set; }
}