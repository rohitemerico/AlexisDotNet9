namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Profile_General
    {
        [Key]
        public Guid Profile_ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Identifier { get; set; }

        [StringLength(50)]
        public string Organization { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(50)]
        public string ConsentMessage { get; set; }

        [StringLength(50)]
        public string Security { get; set; }

        [StringLength(50)]
        public string AuthorizationPassword { get; set; }

        [StringLength(50)]
        public string AutomaticallyRemoveProfile { get; set; }

        public DateTime? AutomaticallyRemoveProfile_Date { get; set; }

        [StringLength(50)]
        public string AutomaticallyRemoveProfile_Days { get; set; }

        [StringLength(50)]
        public string AutomaticallyRemoveProfile_Hours { get; set; }

        public Guid? Branch_ID { get; set; }

        public string Profile_APNS_Path { get; set; }

        public string Profile_Enroll_Path { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public Guid? CProfileId { get; set; }

        public int? pStatus { get; set; }

        public Guid? LastUpdateBy { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
