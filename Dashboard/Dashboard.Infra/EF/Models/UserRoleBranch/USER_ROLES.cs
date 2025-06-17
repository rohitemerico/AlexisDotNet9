namespace Dashboard.Infra.EF.Models.UserRoleBranch
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class USER_ROLES
    {
        [Key]
        [StringLength(36)]
        public string RID { get; set; }

        [StringLength(50)]
        public string RDESC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RSTATUS { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RCREATEDDATE { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RAPPROVEDDATE { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RDECLINEDATE { get; set; }

        [StringLength(36)]
        public string RCREATEDBY { get; set; }

        [StringLength(36)]
        public string RAPPROVEDBY { get; set; }

        [StringLength(36)]
        public string RDECLINEBY { get; set; }

        [StringLength(36)]
        public string RUPDATEDBY { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RUPDATEDDATE { get; set; }

        [StringLength(500)]
        public string RREMARKS { get; set; }
    }
}
