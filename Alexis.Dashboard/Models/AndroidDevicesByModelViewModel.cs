namespace Alexis.Dashboard.Models;

public class AndroidDevicesByModelViewModel
{
    public string ModelName { get; set; }
    public int DeviceCount { get; set; }
    public int ActiveDevices { get; set; }
    public int InactiveDevices { get; set; }
    public decimal AverageBatteryLevel { get; set; }
    public DateTime LastEnrolledDevice { get; set; }
    public DateTime LastSyncedDevice { get; set; }
}


public class iOSDevicesByModelViewModel
{
    public string ModelName { get; set; }
    public int DeviceCount { get; set; }
    public int ActiveDevices { get; set; }
    public int InactiveDevices { get; set; }
}

public class WindowsDevicesByModelViewModel
{
    public string OEM { get; set; }
    public int DeviceCount { get; set; }
    public int ActiveDevices { get; set; }
    public int InactiveDevices { get; set; }
}