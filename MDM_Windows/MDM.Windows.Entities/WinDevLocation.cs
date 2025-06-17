//====================================================
// .net core dll not compatible with .net framework
// Nevertheless, dll included for reference (to tally)
//====================================================
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDM.Windows.Entities;

[Table("WinDevLocations")]
public class WinDevLocation //: WinMdm.Library.DB.WinDevLocation
{
    [Key] public string WinDevID { get; set; }
    public WinDev WinDev { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Accuracy { get; set; }
    public double Altitude { get; set; }
    public int AltitudeAccuracy { get; set; }
    public int Age { get; set; }
}
