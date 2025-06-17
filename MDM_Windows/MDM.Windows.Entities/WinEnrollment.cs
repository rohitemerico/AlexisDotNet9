//====================================================
// .net core dll not compatible with .net framework
// Nevertheless, dll included for reference (to tally)
//====================================================
using System.ComponentModel.DataAnnotations.Schema;

namespace MDM.Windows.Entities;

//windows configuration service provider (CSP)
[Table("WinEnrollments")]
public class WinEnrollment //: WinMdm.Library.DB.WinEnrollment
{
    public Guid ID { get; set; }
    public string Source_DevID { get; set; }
    public string Source_DevSysMan { get; set; }
    public string Source_DevModel { get; set; }
    public string Source_DevMgmtVer { get; set; }
    public string Source_Lang { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime LastEnrolled { get; set; }
    public DateTime LastSync { get; set; }
    public int SyncCount { get; set; }
    public bool EnrollStatus { get; set; }
}
