namespace MDM.iOS.Entities.MDM
{
    public enum Enum_CommandName
    {
        DeviceLock = 1,
        EraseDevice = 2,
        ClearPasscode = 3,
        SecurityInfo = 4,
        InstalledApplicationList = 5,
        DeviceInformation = 6,
        CertificateList = 7,
        ProvisioningProfileList = 8,
        ManagedApplicationList = 9,
        ProfileList = 10,

        InstallProfile_Restriction = 11,       //Under InstallProfile command in ipad_mobileconfig
        RemoveProfile_Restriction = 12,
        ProfileEnrollment = 13,
        InstallApplication = 14,
        RemoveApplication = 15,
        SingleAppMode = 16,                    //Under InstallProfile command in ipad_mobileconfig
        RemoveSingleAppMode = 17,
        InstallProfile = 18,                   //Under InstallProfile command in ipad_mobileconfig
        RestartDevice = 19,
        ShutDownDevice = 20,

        //Lost Mode functionalities. 
        EnableLostMode = 21,
        DisableLostMode = 22,
        PlayLostModeSound = 23,
        DeviceLocation = 24,

        // Install application and disable the backup. 
        InstallApplicationNoBackup = 25,

        // OS Scans 
        AvailableOSUpdates = 26,
        ScheduleOSUpdate = 27

    };

}
