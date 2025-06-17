namespace Alexis.Dashboard.Models;

public class iOSMDMProfileViewModel
{
    public Guid Profile_ID { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public int pStatus { get; set; }
    public string Status { get; set; }
    public string Created_By { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public string Updated_by { get; set; }
    public Guid CProfileID { get; set; }
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool Visible { get; set; } = true;
}
