namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Profile_General_BranchID
    {
        [Key]
        public Guid IID { get; set; }

        public Guid? Profile_ID { get; set; }

        public Guid? cProfile_ID { get; set; }

        public Guid? Branch_ID { get; set; }
    }
}
