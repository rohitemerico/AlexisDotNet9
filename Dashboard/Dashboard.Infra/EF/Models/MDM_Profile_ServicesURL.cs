namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MDM_Profile_ServicesURL
    {
        [Key]
        [Column(Order = 0)]
        public Guid Profile_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GroupID { get; set; }

        public string CheckInURL { get; set; }

        public string CheckOutURL { get; set; }

        public string EnrollmentURL { get; set; }

        public string CertPath { get; set; }
    }
}
