namespace Alexis.Dashboard.Models;

public class RoleModel
{
    public string rID { get; set; }

    public string RDESC { get; set; }

    public decimal RSTATUS { get; set; }

    public DateTime RCREATEDDATE { get; set; }

    public string CREATEDBYNAME { get; set; }

    public DateTime? RUPDATEDDATE { get; set; }

    public string UPDATEDBYNAME { get; set; }
    public string Status { get; set; }
    public string CREATEDBY { get; set; }
    public string UPDATEDBY { get; set; }
    public bool Visible { get; set; } = true;
    public bool ErrorVisible { get; set; } = false;
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }
}
