namespace Dashboard.Infra.EF.Models.Ad
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MACHINE_ADVERTISEMENT
    {
        [Key]
        public Guid AID { get; set; }

        [StringLength(50)]
        public string ADESC { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ACREATEDDATE { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AAPPROVEDDATE { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ADECLINEDDATE { get; set; }

        [StringLength(36)]
        public string ACREATEDBY { get; set; }

        [StringLength(36)]
        public string AAPPROVEDBY { get; set; }

        [StringLength(36)]
        public string ADECLINEBY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ASTATUS { get; set; }

        public string AABSOLUTEPATH { get; set; } //absolute physical path
        public string ARELATIVEPATHURL { get; set; } //relative URI

        [StringLength(100)]
        public string ANAME { get; set; } //filename

        public string AEXTENSIONTYPE { get; set; } //file extension type

        public double? ADURATION { get; set; }

        [StringLength(36)]
        public string AUPDATEDBY { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? AUPDATEDDATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ATOTAL { get; set; }

        public bool AISBACKGROUNDIMG { get; set; }

        [StringLength(500)]
        public string AREMARKS { get; set; }
    }
}
