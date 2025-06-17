namespace Alexis.Dashboard.Models;

public class KioskMaintenanceViewModel
{
    public string mid { get; set; }
    public string description { get; set; }
    public string mKioskID { get; set; }
    public string serial { get; set; }
    public string address { get; set; }
    public string hopper { get; set; }
    public string latitude { get; set; }
    public string longitude { get; set; }
    public string document { get; set; }
    public string alert { get; set; }
    public string groupdesc { get; set; }
    public string businesshour { get; set; }
    public string mstatus { get; set; }
    public DateTime createddate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? lastupdateddate { get; set; }
    public string? UpdatedBy { get; set; }
    public bool Visible { get; set; } = true;
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }
}
