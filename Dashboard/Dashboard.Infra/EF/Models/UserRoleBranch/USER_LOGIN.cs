namespace Dashboard.Infra.EF.Models.UserRoleBranch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class USER_LOGIN
    {
        [Key]
        [StringLength(36)]
        public string AID { get; set; }

        [StringLength(50)]
        public string UNAME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? USTATUS { get; set; }

        [StringLength(36)]
        public string RID { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UCREATEDDATE { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UAPPROVEDDATE { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UDECLINEDATE { get; set; }

        [StringLength(36)]
        public string UCREATEDBY { get; set; }

        [StringLength(36)]
        public string UAPPROVEDBY { get; set; }

        [StringLength(36)]
        public string UDECLINEBY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AGENTFLAG { get; set; }

        [StringLength(100)]
        public string UFULLNAME { get; set; }

        [StringLength(36)]
        public string UCMID { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UUPDATEDDATE { get; set; }

        [StringLength(36)]
        public string UUPDATEDBY { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ULASTLOGINDATE { get; set; }

        [StringLength(100)]
        public string USESSIONID { get; set; }

        [StringLength(100)]
        public string SESSIONKEY { get; set; }

        [StringLength(500)]
        public string UREMARKS { get; set; }

        public double? LOGINFLAG { get; set; }

        public double? MCARDREPLENISHMENT { get; set; }

        public double? MCHEQUEREPLENISHMENT { get; set; }

        public double? MCONSUMABLE { get; set; }

        public double? MSECURITY { get; set; }

        public double? MTROUBLESHOOT { get; set; }

        public double? LOCALUSER { get; set; }
    }
}
