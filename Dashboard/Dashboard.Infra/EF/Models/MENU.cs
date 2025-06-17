namespace Dashboard.Infra.EF.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MENU")]
    public partial class MENU
    {
        [Key]
        [StringLength(36)]
        public string MID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MTYPE { get; set; }

        [StringLength(50)]
        public string MDESC { get; set; }

        [StringLength(50)]
        public string MPATH { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MGROUP { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MSEQ { get; set; }

        public decimal? TEST { get; set; }
    }
}
