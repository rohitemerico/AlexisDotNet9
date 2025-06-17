namespace Alexis.Dashboard.Models;

public class AuditRecordViewModel
{
    public string Module { get; set; }
    public string Description { get; set; }
    public string Action { get; set; }
    public string Status2 { get; set; }
    public string SourceIP { get; set; }
    public string DestinationIP { get; set; }
    public string Executed { get; set; }
    public DateTime Audit_Date { get; set; }
}
