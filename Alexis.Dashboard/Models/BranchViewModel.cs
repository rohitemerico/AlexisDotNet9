namespace Alexis.Dashboard.Models;

public class BranchViewModel
{
    public Guid BID { get; set; }
    public string BDESC { get; set; }
    public TimeSpan BOpenTime { get; set; }
    public bool bMonday { get; set; }
    public bool bTuesday { get; set; }
    public bool bWednesday { get; set; }
    public bool bThursday { get; set; }
    public bool bFriday { get; set; }
    public bool bSaturday { get; set; }
    public bool bSunday { get; set; }
    public int Monday { get; set; }
    public int Tuesday { get; set; }
    public int Wednesday { get; set; }
    public int Thursday { get; set; }
    public int Friday { get; set; }
    public int Saturday { get; set; }
    public int Sunday { get; set; }
    public int BSTATUS { get; set; }
    public string Status { get; set; }
    public DateTime BCREATEDDATE { get; set; }
    public string CREATEDBY { get; set; }
    public DateTime? BUPDATEDDATE { get; set; }
    public string? UPDATEDBY { get; set; }

    public bool Visible { get; set; } = true;
    public bool ErrorVisible { get; set; } = false;
    public bool AllowApprove { get; set; }
    public bool AllowReject { get; set; }
    public bool AllowView { get; set; }
    public bool AllowDecline { get; set; }
    public bool chk { get; set; }
}
