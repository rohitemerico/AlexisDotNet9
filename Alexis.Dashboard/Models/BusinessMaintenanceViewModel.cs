namespace Alexis.Dashboard.Models;

public class BusinessMaintenanceViewModel
{
    public string bId { get; set; }
    public string bDesc { get; set; }
    public string bStartTime { get; set; }
    public string bEndTime { get; set; }
    public decimal bMonday { get; set; }
    public decimal bTuesday { get; set; }
    public decimal bWednesday { get; set; }
    public decimal bThursday { get; set; }
    public decimal bFriday { get; set; }
    public decimal bSaturday { get; set; }
    public decimal bSunday { get; set; }
    public string bStatus { get; set; }
    public DateTime BCREATEDDATE { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? BUPDATEDDATE { get; set; }
    public string? UpdatedBy { get; set; }
    public bool Visible { get; set; } = true;
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }
}
