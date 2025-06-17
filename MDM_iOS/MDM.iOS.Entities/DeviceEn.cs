using System.Xml.Serialization;

namespace MDM.iOS.Entities
{
    /// <summary>
    /// Device entity class which contains all device details 
    /// such as serial number, passcode status, etc 
    /// </summary>

    [Serializable]
    public class DeviceEn
    {
        public DeviceEn()
        {
            this.LogAlert = false;
            this.IsResolved = false;
        }

        private string deviceSerial;
        [XmlElement]
        public string DeviceSerial
        {
            get { return deviceSerial; }
            set { deviceSerial = value; }
        }


        private string deviceImei;
        [XmlElement]
        public string DeviceImei
        {
            get { return deviceImei; }
            set { deviceImei = value; }
        }

        private string deviceUdid;
        [XmlAnyAttribute]
        public string DeviceUDID
        {
            get { return deviceUdid; }
            set { deviceUdid = value; }
        }

        private string deviceName;
        [XmlAnyAttribute]
        public string DeviceName
        {
            get { return deviceName; }
            set { deviceName = value; }
        }

        private string buildVersion;
        [XmlAnyAttribute]
        public string BuildVersion
        {
            get { return buildVersion; }
            set { buildVersion = value; }
        }

        private bool passcodePresent;
        [XmlAnyAttribute]
        public bool PasscodePresent
        {
            get { return passcodePresent; }
            set { passcodePresent = value; }
        }

        private bool passcodeCompliant;
        [XmlAnyAttribute]
        public bool PasscodeCompliant
        {
            get { return passcodeCompliant; }
            set { passcodeCompliant = value; }
        }

        private bool passcodeCompliantWithProfiles;
        [XmlAnyAttribute]
        public bool PasscodeCompliantWithProfiles
        {
            get { return passcodeCompliantWithProfiles; }
            set { passcodeCompliantWithProfiles = value; }
        }

        private string wifiMAC;
        [XmlAnyAttribute]
        public string WifiMAC
        {
            get { return wifiMAC; }
            set { wifiMAC = value; }
        }

        private string bluetoothMAC;
        [XmlAnyAttribute]
        public string BluetoothMAC
        {
            get { return bluetoothMAC; }
            set { bluetoothMAC = value; }
        }

        private bool isRoaming;
        [XmlAnyAttribute]
        public bool IsRoaming
        {
            get { return isRoaming; }
            set { isRoaming = value; }
        }


        private Guid branchId;
        [XmlAnyAttribute]
        public Guid BranchId
        {
            get { return branchId; }
            set { branchId = value; }
        }

        private bool isSupervised;
        [XmlAnyAttribute]
        public bool IsSupervised
        {
            get { return isSupervised; }
            set { isSupervised = value; }
        }

        private bool singleAppModeEnabled;
        [XmlAnyAttribute]
        public bool SingleAppModeEnabled
        {
            get { return singleAppModeEnabled; }
            set { singleAppModeEnabled = value; }
        }

        // this will be initialised to false upon enrollment. 
        private bool lostModeEnabled;
        [XmlAnyAttribute]
        public bool LostModeEnabled
        {
            get { return lostModeEnabled; }
            set { lostModeEnabled = value; }
        }

        // can be left empty as it is 
        private string lostLatitude;
        [XmlAnyAttribute]
        public string LostLatitude
        {
            get { return lostLatitude; }
            set { lostLatitude = value; }
        }

        // can be left empty as it is 
        private string lostLongitude;
        [XmlAnyAttribute]
        public string LostLongitude
        {
            get { return lostLongitude; }
            set { lostLongitude = value; }
        }

        private int deviceStatus;
        [XmlAnyAttribute]
        public int DeviceStatus
        {
            get { return deviceStatus; }
            set { deviceStatus = value; }
        }

        private int ipadStatus;
        [XmlAnyAttribute]
        public int iPadStatus
        {
            get { return ipadStatus; }
            set { ipadStatus = value; }
        }

        private int appStat;
        [XmlAnyAttribute]
        public int appStatus
        {
            get { return appStat; }
            set { appStat = value; }
        }

        private int componentStatus;
        [XmlElement]
        public int ComponentStatus
        {
            get { return componentStatus; }
            set { componentStatus = value; }
        }

        private DateTime? monitoringPushDatetime;
        [XmlElement]
        public DateTime? MonitoringPushDatetime
        {
            get { return monitoringPushDatetime; }
            set { monitoringPushDatetime = value; }
        }

        private DateTime? monitoringRecDatetime;
        [XmlElement]
        public DateTime? MonitoringRecDatetime
        {
            get { return monitoringRecDatetime; }
            set { monitoringRecDatetime = value; }
        }

        private DateTime? apnPushDatetime;
        [XmlElement]
        public DateTime? ApnPushDatetime
        {
            get { return apnPushDatetime; }
            set { apnPushDatetime = value; }
        }

        private DateTime? apnRecDatetime;
        [XmlElement]
        public DateTime? ApnRecDatetime
        {
            get { return apnRecDatetime; }
            set { apnRecDatetime = value; }
        }

        private DateTime? resolvedTime;
        [XmlAnyAttribute]
        public DateTime? ResolvedTime
        {
            get { return resolvedTime; }
            set { resolvedTime = value; }
        }

        private DateTime? componentAlertTime;
        [XmlElement]
        public DateTime? ComponentAlertTime
        {
            get { return componentAlertTime; }
            set { componentAlertTime = value; }
        }

        private DateTime? componentLastAlertTime;
        [XmlAnyAttribute]
        public DateTime? ComponentLastAlertTime
        {
            get { return componentLastAlertTime; }
            set { componentLastAlertTime = value; }
        }

        private DateTime? lastInitializeTime;
        [XmlAnyAttribute]
        public DateTime? LastInitializeTime
        {
            get { return lastInitializeTime; }
            set { lastInitializeTime = value; }
        }

        private string deviceDataSignal;
        [XmlAnyAttribute]
        public string DeviceDataSignal
        {
            get { return deviceDataSignal; }
            set { deviceDataSignal = value; }
        }

        private string iPadBattLevel;
        [XmlElement]
        public string IPadBattLevel
        {
            get { return iPadBattLevel; }
            set { iPadBattLevel = value; }
        }

        private int componentBatteryLevel;
        [XmlAnyAttribute]
        public int ComponentBattLvl
        {
            get { return componentBatteryLevel; }
            set { componentBatteryLevel = value; }
        }

        private bool componentCardReaderStatus;
        [XmlAnyAttribute]
        public bool ComponentCardReaderStatus
        {
            get { return componentCardReaderStatus; }
            set { componentCardReaderStatus = value; }
        }

        private bool componentThumbStatus;
        [XmlAnyAttribute]
        public bool ComponentThumbStatus
        {
            get { return componentThumbStatus; }
            set { componentThumbStatus = value; }
        }

        private string msfAppID;
        [XmlAnyAttribute]
        public string MsfAppID
        {
            get { return msfAppID; }
            set { msfAppID = value; }
        }

        /***/
        private string latitude;
        [XmlElement]
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private string longitude;
        [XmlElement]
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        private bool logAlert;
        [XmlElement]
        public bool LogAlert
        {
            get { return logAlert; }
            set { logAlert = value; }
        }

        private bool isResolved;
        [XmlElement]
        public bool IsResolved
        {
            get { return isResolved; }
            set { isResolved = value; }
        }

        private string osVersion;
        [XmlElement]
        public string OsVersion
        {
            get { return osVersion; }
            set { osVersion = value; }
        }

        private string deviceCapacity;
        [XmlElement]
        public string DeviceCapacity
        {
            get { return deviceCapacity; }
            set { deviceCapacity = value; }
        }


        private string availableDevice_Capacity;
        [XmlElement]
        public string AvailableDevice_Capacity
        {
            get { return availableDevice_Capacity; }
            set { availableDevice_Capacity = value; }
        }


        private DateTime eraseDateTime;
        [XmlElement]
        public DateTime EraseDateTime
        {
            get { return eraseDateTime; }
            set { eraseDateTime = value; }
        }

        private string firmwareVersion;
        [XmlAnyAttribute]
        public string FirmwareVersion
        {
            get { return firmwareVersion; }
            set { firmwareVersion = value; }
        }

        private string firmwareBatteryStatus;
        [XmlAnyAttribute]
        public string FirmwareBatteryStatus
        {
            get { return firmwareBatteryStatus; }
            set { firmwareBatteryStatus = value; }
        }

        private string firmwareBatteryLevel;
        [XmlAnyAttribute]
        public string FirmwareBatteryLevel
        {
            get { return firmwareBatteryLevel; }
            set { firmwareBatteryLevel = value; }
        }


        private string firmwareSerial;
        [XmlAnyAttribute]
        public string FirmwareSerial
        {
            get { return firmwareSerial; }
            set { firmwareSerial = value; }
        }

        private DateTime firmwareExpiredDate;
        [XmlAnyAttribute]
        public DateTime FirmwareExpiredDate
        {
            get { return firmwareExpiredDate; }
            set { firmwareExpiredDate = value; }
        }

        private string appDeviceToken;
        [XmlAnyAttribute]
        public string AppDeviceToken
        {
            get { return appDeviceToken; }
            set { appDeviceToken = value; }
        }
    }
}
