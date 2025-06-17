namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Restriction_Menu
    {
        [Key]
        public Guid RID { get; set; }

        [StringLength(100)]
        public string RestrictionName { get; set; }

        [StringLength(100)]
        public string RestrictionDesc { get; set; }

        public int? Queue { get; set; }

        public int? Active { get; set; }

        public int? RGroup { get; set; }

        public int? GroupHeader { get; set; }

        public int? NumberType { get; set; }

        public int? Partition { get; set; }
    }
}
