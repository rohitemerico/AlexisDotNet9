namespace Alexis.Dashboard.Models;

public class iOSMDMApplicationViewModel
{
    public Guid AppID { get; set; }
    public string Name { get; set; }

    public string Desc { get; set; }

    public string Version { get; set; }

    public string Identifier { get; set; }
    public DateTime CreatedDate { get; set; }
}
