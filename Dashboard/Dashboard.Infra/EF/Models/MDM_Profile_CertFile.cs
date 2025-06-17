namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Profile_CertFile
    {
        [Key]
        public Guid IpadProfile_ID { get; set; }

        public string CertPath { get; set; }

        public string Extension { get; set; }

        public string FriendlyName { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string CertFormat { get; set; }

        public string NameInfo { get; set; }

        public string CertType { get; set; }

        public string SerialNumber { get; set; }

        public string IssuerName { get; set; }

        public string SubjectName { get; set; }

        public int? CertVersion { get; set; }
    }
}
