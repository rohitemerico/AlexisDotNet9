namespace Dashboard.Infra.EF.Models.UserRoleBranch
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Dashboard.Infra.EF.Models.iOS;

    public partial class User_Branch
    {
        [Key] //primary
        public Guid bID { get; set; }

        [StringLength(100)]
        public string bDesc { get; set; }

        public int? bStatus { get; set; }

        public bool? bMonday { get; set; }

        public bool? bTuesday { get; set; }

        public bool? bWednesday { get; set; }

        public bool? bThursday { get; set; }

        public bool? bFriday { get; set; }

        public bool? bSaturday { get; set; }

        public bool? bSunday { get; set; }

        public TimeSpan? bOpenTime { get; set; }

        public TimeSpan? bCloseTime { get; set; }

        public DateTime? bCreatedDate { get; set; }

        public DateTime? bApprovedDate { get; set; }

        public DateTime? bDeclinedDate { get; set; }

        public DateTime? bUpdatedDate { get; set; }

        public Guid? bCreatedBy { get; set; }

        public Guid? bApprovedBy { get; set; }

        public Guid? bDeclinedBy { get; set; }

        public Guid? bUpdatedBy { get; set; }

        public string FTPpath { get; set; }

        [StringLength(100)]
        public string FTPUsername { get; set; }

        [StringLength(200)]
        public string FTPpassword { get; set; }

        [StringLength(10)]
        public string bBimbBranchCode { get; set; }

        [StringLength(10)]
        public string bBimbTellerID { get; set; }

        [StringLength(10)]
        public string bBimbControlUnit { get; set; }

        [StringLength(10)]
        public string bBimbBranchRCSFolderName { get; set; }

        [StringLength(50)]
        public string bRemarks { get; set; }


        //Navigation
        public virtual ICollection<tblMachine> TblMachines { get; set; }

    }
}
