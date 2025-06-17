namespace Alexis.Dashboard.Models;

public class iOSDeviceComponentViewModel
{
    public string MSERIAL { get; set; }
    public string MachineName { get; set; }
    public string Component { get; set; }
    public DateTime ALERTTIME { get; set; }
    public DateTime UPTIME { get; set; }
    public decimal TOTALALERT { get; set; }
}
public class AndroidDeviceComponentViewModel
{
    public string deviceMACAdd { get; set; }
    public string deviceName { get; set; }
    public string Component { get; set; }
    public DateTime ALERTTIME { get; set; }
    public DateTime UPTIME { get; set; }
    public decimal TOTALALERT { get; set; }
}

public class WindowsDeviceComponentViewModel
{
    public string SMBIOSSerialNumber { get; set; }
    public string DNSComputerName { get; set; }
    public string Component { get; set; }
    public DateTime ALERTTIME { get; set; }
    public DateTime UPTIME { get; set; }
    public decimal TOTALALERT { get; set; }
}
