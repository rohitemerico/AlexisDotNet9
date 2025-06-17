//====================================================
// .net core dll not compatible with .net framework
// Nevertheless, dll included for reference (to tally)
//====================================================

using System.ComponentModel.DataAnnotations.Schema;

namespace MDM.Windows.Entities;

[Table("WinProfiles")]
public class WinProfile //: WinMdm.Library.DB.WinProfile
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Version { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime EditedDate { get; set; }
    public bool AllowManualMDMUnenrollment { get; set; }
    public ICollection<WinDevProfileAssignment> WinDevProfileAssignments { get; set; }
}
