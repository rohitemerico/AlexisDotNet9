namespace Alexis.Dashboard.Models;

public class ApplicationMaintenanceViewModel
{
    public string SysID { get; set; }
    public string VER { get; set; }
    public DateTime CREATEDDATETIME { get; set; }
    public string FPATH { get; set; }
    public string Pilot { get; set; }
    public string FStatus { get; set; }
    public int COUNTDL { get; set; }
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }
}
