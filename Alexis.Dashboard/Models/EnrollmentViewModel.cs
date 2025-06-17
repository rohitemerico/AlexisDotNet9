namespace Alexis.Dashboard.Models;

public class EnrollmentViewModel
{
    public Guid MDMId { get; set; }
    public string MachineName { get; set; }
    public string SerialNo { get; set; }
    public string IMEI { get; set; }
    public string StrStatus { get; set; }
    public Guid? BranchId { get; set; }
    public DateTime LastModifiedDateTime { get; set; }
}