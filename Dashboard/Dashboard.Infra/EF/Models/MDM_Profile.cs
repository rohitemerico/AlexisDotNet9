namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MDM_Profile
    {
        [Key]
        public Guid Profile_ID { get; set; }

        [StringLength(50)]
        public string Profile_Name { get; set; }

        public string Profile_Desc { get; set; }

        public int? Profile_GroupID { get; set; }

        public int? Profile_Status { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        [Column(TypeName = "text")]
        public string PlistContent { get; set; }

        public string Profile_APNS_Path { get; set; }

        public string Profile_Enroll_Path { get; set; }
    }
}
