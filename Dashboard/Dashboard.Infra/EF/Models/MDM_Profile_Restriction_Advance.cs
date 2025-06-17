namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Profile_Restriction_Advance
    {
        [Key]
        public Guid Profile_ID { get; set; }

        [StringLength(50)]
        public string AcceptCookies { get; set; }

        [StringLength(50)]
        public string RestrictAppUsage { get; set; }

        public string App_Identify { get; set; }
    }
}
