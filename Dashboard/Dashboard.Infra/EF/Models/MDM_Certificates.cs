namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Certificates
    {
        public Guid ID { get; set; }

        [StringLength(50)]
        public string UDID { get; set; }

        [StringLength(50)]
        public string CertName { get; set; }

        public bool? IsIdentityCert { get; set; }
    }
}
