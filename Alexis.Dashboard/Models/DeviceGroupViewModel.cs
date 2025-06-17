namespace Alexis.Dashboard.Models;

public class DeviceGroupViewModel
{
    public string GID { get; set; }
    public string GroupName { get; set; }
    public string GroupDesc { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Created_By { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? Updated_By { get; set; }
}
