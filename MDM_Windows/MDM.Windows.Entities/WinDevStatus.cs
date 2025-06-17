//====================================================
// .net core dll not compatible with .net framework
// Nevertheless, dll included for reference (to tally)
//====================================================
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDM.Windows.Entities;

[Table("WinDevStatuses")]
public class WinDevStatus //: WinMdm.Library.DB.WinDevStatus
{
    [Key] public string WinDevID { get; set; }
    public WinDev WinDev { get; set; }

    public bool IsOnline { get; set; }
}
