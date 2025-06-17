namespace Alexis.Dashboard.Models;

public class UserViewModel
{
    public string aID { get; set; }
    public string uName { get; set; }
    public string uFullName { get; set; }
    public string rDesc { get; set; }
    public decimal uStatus { get; set; }
    public string Status { get; set; }
    public DateTime uCreatedDate { get; set; }
    public string CreatedByName { get; set; }
    public DateTime? uUpdatedDate { get; set; }
    public string UpdatedByName { get; set; }
    public DateTime? uLastLoginDate { get; set; }
    public bool Visible { get; set; } = true;
    public bool ErrorVisible { get; set; } = false;
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }
}
