namespace Alexis.Dashboard.Models;

public class iOSMDMDeviceInfoViewModel
{
    public string MachineUDID { get; set; }
    public string MachineName { get; set; }
    public string MachineImei { get; set; }
    public string MachineSerial { get; set; }
    public string BuildVersion { get; set; }
    public string OSVersion { get; set; }
    public int MachineStatus { get; set; }
    public string AvailableDevice_Capacity { get; set; }
    public string DeviceCapacity { get; set; }
    public bool? PasscodePresent { get; set; }
    public bool? PasscodeCompliant { get; set; }
    public bool? PasscodeCompliantWithProfiles { get; set; }
    public string WifiMAC { get; set; }
    public string BluetoothMAC { get; set; }
    public bool IsRoaming { get; set; }
    public bool isSupervised { get; set; }
    public string FirmwareVersion { get; set; }
    public string FirmwareBatteryStatus { get; set; }
    public string FirmwareBatteryLevel { get; set; }
    public string FirmwareSerial { get; set; }
    public string bDesc { get; set; }
    public string Location { get; set; }
    public string CertListing { get; set; }
    public string Status => MachineStatus == 1 ? "Active" : "Inactive";
    public string AvailableSpace => string.Format("{0}/{1}", AvailableDevice_Capacity, DeviceCapacity);
}


public class iOSMDMDeviceGroupViewModel
{
    public Guid bID { get; set; }
    public string bDesc { get; set; }
    public TimeSpan bOpenTime { get; set; }
    public TimeSpan bCloseTime { get; set; }
    public int bStatus { get; set; }
    public bool bMonday { get; set; }
    public bool bTuesday { get; set; }
    public bool bWednesday { get; set; }
    public bool bThursday { get; set; }
    public bool bFriday { get; set; }
    public bool bSaturday { get; set; }
    public bool bSunday { get; set; }
    public DateTime bCreatedDate { get; set; }
    public DateTime? bApprovedDate { get; set; }
    public DateTime? bDeclinedDate { get; set; }
    public DateTime? bUpdatedDate { get; set; }
    public Guid bCreatedBy { get; set; }
    public Guid bApprovedBy { get; set; }
    public Guid? bDeclinedBy { get; set; }
    public Guid? bUpdatedBy { get; set; }
    public string Status { get; set; }
    public string CreatedBy { get; set; }
}

public class iOSMDMApplicationInfoViewModel
{
    public Guid AppID { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public string Version { get; set; }
    public string Path { get; set; }
    public string Identifier { get; set; }
    public string Icon_FilePath { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? DeclineDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid ApprovedBy { get; set; }
    public Guid DeclinedBy { get; set; }
    public Guid UpdatedBy { get; set; }
    public string status { get; set; }
    public int rstatus { get; set; }
}


public class iOSMDMProfileInfoViewModel
{
    public Guid Profile_ID { get; set; }
    public string Name { get; set; }
    public string Identifier { get; set; }
    public string Organization { get; set; }
    public string Description { get; set; }
    public string ConsentMessage { get; set; }
    public string Security { get; set; }
    public string AuthorizationPassword { get; set; }
    public bool? AutomaticallyRemoveProfile { get; set; }
    public DateTime? AutomaticallyRemoveProfile_Date { get; set; }
    public int? AutomaticallyRemoveProfile_Days { get; set; }
    public int? AutomaticallyRemoveProfile_Hours { get; set; }
    public string Branch_ID { get; set; }
    public string Profile_APNS_Path { get; set; }
    public string Profile_Enroll_Path { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public Guid CProfileId { get; set; }
    public int pStatus { get; set; }
    public Guid LastUpdateBy { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Created_By { get; set; }
    public string Updated_By { get; set; }
    public string Status { get; set; }
}

