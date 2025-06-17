namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class TRANSACTION_LOG
    {
        [Key]
        [StringLength(36)]
        public string TRANSID { get; set; }

        [StringLength(36)]
        public string TTYPE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TSTATUS { get; set; }

        [StringLength(50)]
        public string MSERIAL { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? TRANSDATE { get; set; }

        [StringLength(50)]
        public string TDESC { get; set; }

        [StringLength(36)]
        public string USERID { get; set; }

        [StringLength(50)]
        public string MKIOSKID { get; set; }

        [StringLength(100)]
        public string SESSIONID { get; set; }

        [StringLength(100)]
        public string CUSTID { get; set; }

        [StringLength(100)]
        public string CUSTTYPE { get; set; }

        [StringLength(1000)]
        public string REMARKS { get; set; }
    }
}
