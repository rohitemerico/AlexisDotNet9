namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_InstalledApplicationList
    {
        public long ID { get; set; }

        [StringLength(50)]
        public string UDID { get; set; }

        [StringLength(50)]
        public string BundleSize { get; set; }

        [StringLength(50)]
        public string DynamicSize { get; set; }

        [StringLength(50)]
        public string Identifier { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Version { get; set; }

        [StringLength(50)]
        public string ShortVersion { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
