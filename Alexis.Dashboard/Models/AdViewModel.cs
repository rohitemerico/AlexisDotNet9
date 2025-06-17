namespace Alexis.Dashboard.Models;

public class AdViewModel
{
    public Guid aID { get; set; }
    public string aDesc { get; set; }
    public string AdvertStatus { get; set; }
    public double aDuration { get; set; }
    public string aRelativePathUrl { get; set; }
    public DateTime aCreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? aUpdatedDate { get; set; }
    public string? Updatedby { get; set; }
    public bool Visible { get; set; } = true;
    public bool ErrorVisible { get; set; } = false;
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowDecline { get; set; }
    public bool AllowView { get; set; }
}
