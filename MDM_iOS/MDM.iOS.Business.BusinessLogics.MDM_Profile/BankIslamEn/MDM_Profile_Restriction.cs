namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Profile_Restriction
    {
        [Key]
        public Guid ID { get; set; }

        public Guid Profile_ID { get; set; }

        public Guid RID { get; set; }

        [StringLength(100)]
        public string RestrictionName { get; set; }

        public bool? IsCheck { get; set; }

        public int Queue { get; set; }
    }
}
