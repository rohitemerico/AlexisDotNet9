namespace MDM.Win.Business.BusinessLogics.MDM;

public class PostDMSessionCmd
{
    public string DeviceId { get; set; }

    public static string CommandType { get => "Exec"; }

    public string CommandNodePath { get; set; }
}
