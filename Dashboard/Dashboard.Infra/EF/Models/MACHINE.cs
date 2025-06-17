namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MACHINE")]
    public partial class MACHINE
    {
        [Key]
        [StringLength(36)]
        public string MID { get; set; }

        [StringLength(50)]
        public string MDESC { get; set; }

        [StringLength(50)]
        public string MSERIAL { get; set; }

        [StringLength(500)]
        public string MADDRESS { get; set; }

        [StringLength(50)]
        public string MLATITUDE { get; set; }

        [StringLength(50)]
        public string MLONGITUDE { get; set; }

        [StringLength(36)]
        public string MGROUPID { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? MCREATEDDATE { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? MAPPROVEDDATE { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? MDECLINEDATE { get; set; }

        [StringLength(36)]
        public string MAPPROVEDBY { get; set; }

        [StringLength(36)]
        public string MCREATEDBY { get; set; }

        [StringLength(36)]
        public string MDECLINEBY { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? MACTIVATEDDATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MSTATUS { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LASTUPDATEDDATE { get; set; }

        [StringLength(500)]
        public string SKMESSAGE { get; set; }

        [StringLength(500)]
        public string SKPIN { get; set; }

        [StringLength(50)]
        public string MKIOSKID { get; set; }

        [StringLength(100)]
        public string SESSIONKEY { get; set; }

        [StringLength(100)]
        public string MSTATIONID { get; set; }

        [StringLength(100)]
        public string IPADDRESS { get; set; }

        [StringLength(100)]
        public string PORTNUMBER { get; set; }

        [StringLength(500)]
        public string MREMARKS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MONITORINGSTATUS { get; set; }

        [StringLength(50)]
        public string VTMVERSION { get; set; }

        public double? MPILOT { get; set; }

        [StringLength(40)]
        public string MARKERSHAPE { get; set; }
    }
}
