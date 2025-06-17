namespace Alexis.Dashboard.Models;

public class iOSMDMDevicesViewModel
{

    public string MachineUDID { get; set; }
    public string MachineName { get; set; }

    public string MachineSerial { get; set; }

    public string bDesc { get; set; }
    public int iPadStatus { get; set; }
    public string IStatus { get; set; }
    public DateTime LastApprovedDateTime { get; set; }
    public Guid bID { get; set; }
}
