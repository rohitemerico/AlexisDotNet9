namespace Dashboard.Infra.EF.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class MDM_Profile_Passcode
    {
        [Key]
        public Guid Profile_ID { get; set; }

        [StringLength(50)]
        public string AllowSimpleValue { get; set; }

        [StringLength(50)]
        public string Requirealphanumericvalue { get; set; }

        [StringLength(50)]
        public string MinimumPasscodeLength { get; set; }

        [StringLength(50)]
        public string MinimumNumberOfComplexCharacters { get; set; }

        [StringLength(50)]
        public string MaximumPasscodeAge { get; set; }

        [StringLength(50)]
        public string MaximumAutoLock { get; set; }

        [StringLength(50)]
        public string PasscodeHistory { get; set; }

        [StringLength(50)]
        public string MaximumGracePeriod { get; set; }

        [StringLength(50)]
        public string MaximumNumberOfFailedAttempts { get; set; }
    }
}
