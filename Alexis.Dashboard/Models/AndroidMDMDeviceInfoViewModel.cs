namespace Alexis.Dashboard.Models;

public class AndroidMDMDeviceInfoViewModel
{
    public int ID { get; set; }
    public string deviceMACAdd { get; set; }
    public string DEVICENAME { get; set; }
    public bool DEVICESTATUS { get; set; }
    public int BATTERYLEVEL { get; set; }
    public string DEVICE_LOCATION { get; set; }
    public int CONNECTIONSTATUS { get; set; }
    public int TOUCHSCREENSTATUS { get; set; }
    public decimal CARDREADERSPERDAY { get; set; }
    public DateTime enrollDatetime { get; set; }
    public DateTime lastSyncDatetime { get; set; }
    public string GROUPNAME { get; set; }
    public string FirmwareVersion { get; set; }
    public string FirmwareBatteryStatus { get; set; }
    public string FirmwareBatteryLevel { get; set; }
    public string FirmwareSerial { get; set; }

    public string STATUS => DEVICESTATUS == true ? "Active" : "Inactive";
    public string CONNECTIONSTATUS_String => CONNECTIONSTATUS == 1 ? "Online" : "Offline";

    public string TOUCHSCREENSTATUS_String => TOUCHSCREENSTATUS == 1 ? "Active" : "Inactive";
}


public class AndroidMDMDeviceGroupViewModel
{
    public string GID { get; set; }
    public string GROUPNAME { get; set; }
    public string GROUPDESC { get; set; }
    public DateTime CREATEDDATE { get; set; }
    public DateTime? UPDATEDDATE { get; set; }
    public string CREATED_BY { get; set; }
    public string? UPDATED_BY { get; set; }
}

public class AndroidMDMApplicationInfoViewModel
{
    public string APPID { get; set; }
    public string APPLICATION_NAME { get; set; }
    public string FPATH { get; set; }
    public string VER { get; set; }
    public DateTime CREATED_ON { get; set; }
    public DateTime? UPDATED_ON { get; set; }
    public string? CREATED_BY { get; set; }
    public string? UPDATED_BY { get; set; }
}


public class AndroidMDMProfileInfoViewModel
{
    public string PRID { get; set; }
    public string PROFILE_NAME { get; set; }
    public string FPATH { get; set; }
    public decimal STATUS { get; set; }
    public DateTime CREATED_ON { get; set; }
    public DateTime? LAST_UPDATED_ON { get; set; }
    public string CREATED_BY { get; set; }
    public string? UPDATED_BY { get; set; }
}

