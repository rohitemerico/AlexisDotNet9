//====================================================
// .net core dll not compatible with .net framework
// Nevertheless, dll included for reference (to tally)
//====================================================
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MDM.Windows.Entities;

[Table("WinDevDetails")]
public class WinDevDetail //: WinMdm.Library.DB.WinDevDetail
{
    [Key]
    public string WinDevID { get; set; }
    public WinDev WinDev { get; set; }

    public string WlanIPv4Address { get; set; }
    public bool VoLTEServiceSetting { get; set; }
    public string WLANMACAddress { get; set; }
    public int TotalRAM { get; set; }
    public int TotalStorage { get; set; }
    public string SMBIOSSerialNumber { get; set; }
    public string DNSComputerName { get; set; }
    public int ProcessorArchitecture { get; set; }
    public string CommercializationOperator { get; set; }
    public string Resolution { get; set; }
    public string RadioSwV { get; set; }
    public string WlanIPv6Address { get; set; }
    public int ProcessorType { get; set; }
    public string LocalTime { get; set; }
    public string MobileID { get; set; }
    public string MaxDepth { get; set; }
    public bool LrgObj { get; set; }
    public string HwV { get; set; }
    public string SwV { get; set; }
    public string FwV { get; set; }
    public string OEM { get; set; }
    public string DevType { get; set; }
    public string OSPlatform { get; set; }
    public string WlanSubnetMask { get; set; }
}
