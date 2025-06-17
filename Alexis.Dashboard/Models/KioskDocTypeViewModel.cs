namespace Alexis.Dashboard.Models;

public class KioskDocTypeViewModel
{
    public string dID { get; set; }
    public string dName { get; set; }
    public string dDesc { get; set; }
    public string CComponentID { get; set; }
    public DateTime dCreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? dUpdatedDate { get; set; }
    public string UpdatedBy { get; set; }
    public string txtStatus { get; set; }
    public string docStatus { get; set; }
    public bool Visible { get; set; } = true;
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }
    public List<DocTypeViewModel> Components { get; set; }
}

public class DocTypeViewModel
{
    public string dID { get; set; }
    public string REF_DID { get; set; }
    public string DocTypeId { get; set; }
    public string dName { get; set; }
    public decimal SWALLOW { get; set; }
    public decimal PRINT { get; set; }
}
public class DocTypeModel
{
    public string dID { get; set; }
    public string REF_DID { get; set; }
    public string dName { get; set; }
    public string cComponentID { get; set; }
    public bool SWALLOW { get; set; }
    public bool PRINT { get; set; }
}