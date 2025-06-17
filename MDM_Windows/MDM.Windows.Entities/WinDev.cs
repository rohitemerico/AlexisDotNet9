//====================================================
// .net core dll not compatible with .net framework
// Nevertheless, dll included for reference (to tally)
//====================================================
using System.ComponentModel.DataAnnotations.Schema;

namespace MDM.Windows.Entities;

//client devices
[Table("WinDevs")]
public class WinDev //: WinMdm.Library.DB.WinDev
{
    public string ID { get; set; }
    public WinDevDetail WinDevDetail { get; set; }
    public WinDevStatus WinDevStatus { get; set; }
    public WinDevLocation WinDevLocation { get; set; }
    public WinDevProfileAssignment WinDevProfileAssignment { get; set; }
    public string Name { get; set; }
}
