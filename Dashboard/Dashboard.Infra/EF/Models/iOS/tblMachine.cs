namespace Dashboard.Infra.EF.Models.iOS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dashboard.Infra.EF.Models.Reporting;
    using Dashboard.Infra.EF.Models.UserRoleBranch;

    [Table("tblMachine")]
    public partial class tblMachine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblMachine()
        {
            TblMonitoringDevices = new HashSet<TblMonitoringDevices>();
        }

        [StringLength(50)]
        public string MachineImei { get; set; }

        [Key] //Primary
        [StringLength(50)]
        public string MachineSerial { get; set; }

        [StringLength(50)]
        public string MachineUDID { get; set; }

        [StringLength(50)]
        public string MachineName { get; set; }

        public bool? IsSupervised { get; set; }

        public bool? SingleAppModeEnabled { get; set; }

        public bool? LostModeEnabled { get; set; }

        [StringLength(50)]
        public string LostLatitude { get; set; }

        [StringLength(50)]
        public string LostLongitude { get; set; }

        public int? MachineStatus { get; set; }

        public int? iPadStatus { get; set; }

        public int? appStatus { get; set; }

        public int? componentStatus { get; set; }

        public DateTime? MonitorPushDatetime { get; set; }

        public DateTime? MonitorRecDatetime { get; set; }

        public DateTime? ApnPushDatetime { get; set; }

        public DateTime? ApnRecDatetime { get; set; }

        public DateTime? ResolvedTime { get; set; }

        public DateTime? ComponentAlertTime { get; set; }

        public DateTime? ComponentLastAlertTime { get; set; }

        public DateTime? LastInitializeTime { get; set; }

        [StringLength(50)]
        public string MachineDataSignal { get; set; }

        [StringLength(50)]
        public string iPadBattLevel { get; set; }

        public int? ComponentBattLevel { get; set; }

        public bool? ComponentCardReaderStatus { get; set; }

        public bool? ComponentThumbStatus { get; set; }

        [StringLength(100)]
        public string MsfAppID { get; set; }

        [StringLength(50)]
        public string OSVersion { get; set; }

        [StringLength(50)]
        public string DeviceCapacity { get; set; }

        [StringLength(50)]
        public string AvailableDevice_Capacity { get; set; }

        [StringLength(50)]
        public string BuildVersion { get; set; }

        public bool? PasscodePresent { get; set; }

        public bool? PasscodeCompliant { get; set; }

        public bool? PasscodeCompliantWithProfiles { get; set; }

        [StringLength(50)]
        public string WifiMAC { get; set; }

        [StringLength(50)]
        public string BluetoothMAC { get; set; }

        public bool? IsRoaming { get; set; }

        public DateTime? EraseDateTime { get; set; }

        [StringLength(50)]
        public string FirmwareVersion { get; set; }

        [StringLength(50)]
        public string FirmwareBatteryStatus { get; set; }

        [StringLength(50)]
        public string FirmwareBatteryLevel { get; set; }

        [StringLength(50)]
        public string FirmwareSerial { get; set; }

        public DateTime? FirmwareExpiredDate { get; set; }

        public string AppDeviceToken { get; set; }


        //foreign
        [Column("BranchId")]
        public Guid? UserBranchbID { get; set; }
        public virtual User_Branch UserBranch { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblMonitoringDevices> TblMonitoringDevices { get; set; }
        public virtual ICollection<tbl_app_msf_translog> TblMSFTransLogs { get; set; }
        public virtual ICollection<tbl_app_mob_translog> TblMOBTransLogs { get; set; }

    }
}
