namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ANDROIDMDM_APPLICATION
    {
        [Key]
        [StringLength(36)]
        public string APPID { get; set; }

        [StringLength(4000)]
        public string APPLICATION_NAME { get; set; }

        [StringLength(4000)]
        public string FPATH { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? STATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? VER { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CREATED_ON { get; set; }

        [StringLength(255)]
        public string CREATED_BY { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? UPDATED_ON { get; set; }

        [StringLength(255)]
        public string UPDATED_BY { get; set; }
    }
}
