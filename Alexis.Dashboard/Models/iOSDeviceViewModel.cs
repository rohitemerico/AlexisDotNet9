namespace Alexis.Dashboard.Models;

public class iOSDeviceViewModel
{
    public string MachineUDID { get; set; }
    public string MachineName { get; set; }
    public string MachineSerial { get; set; }
    public string Bdesc { get; set; }
    public string OSVersion { get; set; }
    public bool IsSupervised { get; set; }
    public bool LostModeEnabled { get; set; }
    public decimal LostLatitude { get; set; }
    public decimal LostLongitude { get; set; }
}
