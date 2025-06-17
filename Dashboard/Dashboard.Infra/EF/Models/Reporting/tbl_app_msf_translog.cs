using System.ComponentModel.DataAnnotations;
using Dashboard.Infra.EF.Models.iOS;

namespace Dashboard.Infra.EF.Models.Reporting;
public partial class tbl_app_msf_translog
{
    [Key] //primary
    public Guid TransID { get; set; }

    public DateTime? TransDate { get; set; }

    public int TransStatus { get; set; }

    public string TransDesc { get; set; }

    [StringLength(50)]
    public string TransType { get; set; }

    [StringLength(50)]
    public string UserName { get; set; }

    [StringLength(50)]
    public string StaffID { get; set; }

    [StringLength(50)]
    public string NRIC { get; set; }

    [StringLength(19)]
    public string AccountNo { get; set; }

    [StringLength(2)]
    public string AccountType { get; set; }

    [StringLength(250)]
    public string AccountName { get; set; }

    [StringLength(16)]
    public string CardNumber { get; set; }

    public bool? PinIssuance { get; set; }


    [StringLength(50)] //foreign
    public string TblMachineMachineSerial { get; set; }
    public virtual tblMachine tblMachine { get; set; }
}
