namespace Alexis.Dashboard.Models;

public class CardViewModel
{
    public string cID { get; set; }
    public string cDesc { get; set; }
    public string txtContactless { get; set; }
    public string cType { get; set; }
    public string cBin { get; set; }
    public string cMask { get; set; }
    public string txtStatus { get; set; }
    public DateTime? cCreatedDate { get; set; }
    public string CreatedName { get; set; }
    public DateTime? cUpdatedDate { get; set; }
    public string UpdatedName { get; set; }
    public bool Visible { get; set; } = true;
    public bool ErrorVisible { get; set; } = false;
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }
}