namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_OS_Updates
    {
        public Guid ID { get; set; }

        [StringLength(50)]
        public string UDID { get; set; }

        [StringLength(50)]
        public string HumanReadableName { get; set; }

        [StringLength(50)]
        public string ProductKey { get; set; }

        [StringLength(50)]
        public string ProductVersion { get; set; }

        public bool? RestartRequired { get; set; }
    }
}
