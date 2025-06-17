namespace Dashboard.Infra.EF.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TBLAGENTVIDEO")]
    public partial class TBLAGENTVIDEO
    {
        [StringLength(5)]
        public string ID { get; set; }

        [StringLength(50)]
        public string Provider { get; set; }

        public string JsonSetting { get; set; }
    }
}
