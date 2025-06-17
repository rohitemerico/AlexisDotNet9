namespace Alexis.Dashboard.Models;

public class GroupMaintenanceViewModel
{
    public string kId { get; set; }
    public string Description { get; set; }
    public string kdesc { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? kUpdatedDate { get; set; }

    public string hopper { get; set; }
    public string document { get; set; }
    public string alert { get; set; }
    public string businesshour { get; set; }
    public string? UpdatedBy { get; set; }
    public bool Visible { get; set; } = true;
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }
}
