namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_InstalledApps
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        public string UDID { get; set; }

        [StringLength(50)]
        public string AppName { get; set; }

        [StringLength(50)]
        public string Version { get; set; }

        [StringLength(50)]
        public string Identifier { get; set; }
    }
}
