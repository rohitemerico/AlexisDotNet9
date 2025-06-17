//====================================================
// .net core dll not compatible with .net framework
// Nevertheless, dll included for reference (to tally)
//====================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDM.Windows.Entities;

[Table("WinDevProfileAssignments")]
public class WinDevProfileAssignment //: WinMdm.Library.DB.WinDevProfileAssignment
{
    [Key] public string WinDevID { get; set; }
    public WinDev WinDev { get; set; }
    public Guid WinProfileID { get; set; }
    public WinProfile WinProfile { get; set; }
}
