using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.Infra.EF.Models.iOS;

namespace Dashboard.Infra.EF.Models.Reporting;
public partial class TblMonitoringDevices
{
    [StringLength(50)] //foreign
    public string TblMachineMachineSerial { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? Uptime { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? Downtime { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? LastUptime { get; set; }

    [Column(TypeName = "datetime2")]
    public DateTime? LastCaptured { get; set; }

    public decimal? Uptime_Seconds { get; set; }

    public decimal? Downtime_Seconds { get; set; }

    [StringLength(450)] //primary
    public string ID { get; set; }
    public virtual tblMachine tblMachine { get; set; }
}
