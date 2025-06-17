namespace Dashboard.Common.Business.Component
{
    public enum ModuleLogAction
    {
        /*Auth*/
        User_Login,
        User_Logout,

        #region Reporting

        View_AuditTrailSystem,
        View_AuditTrailAndroid,
        View_AuditTrailiOS,
        View_AuditTrailWindows,

        View_DeviceListingAndroid,
        View_DeviceListingiOS,
        View_DeviceListingWindows,

        View_OutofServiceAndroid,
        View_OutofServiceiOS,
        View_OutofServiceWindows,

        View_DeviceComponentAndroid,
        View_DeviceComponentiOS,
        View_DeviceComponentWindows,

        View_DeviceUptimeAndroid,
        View_DeviceUptimeiOS,
        View_DeviceUptimeWindows,

        View_DeviceSLAAndroid,
        View_DeviceSLAiOS,
        View_DeviceSLAWindows,

        View_FirmwareKeyAndroid,
        View_FirmwareKeyiOS,
        View_FirmwareKeyWindows,

        View_TransDetailsMSF,
        View_TransSummaryMSF,
        View_FinancingProductMSF,

        View_DepositProductMOB,

        View_AppInstallationiOS,
        View_AppInstallationAndroid,
        View_AppInstallationWindows,

        View_AppDevicesiOS,
        View_AppDevicesAndroid,
        View_AppDevicesWindows,

        View_DataUsageiOS,
        View_DataUsageAndroid,
        View_DataUsageWindows,

        View_EnrollmentiOS,
        View_EnrollmentAndroid,
        View_EnrollmentWindows,

        View_HardwareiOS,
        View_HardwareAndroid,
        View_HardwareWindows,

        View_LocationSummaryiOS,
        View_LocationSummaryAndroid,
        View_LocationSummaryWindows,



        View_MachineMaintenanceCard,
        View_MachineMaintenanceAlert,
        View_MachineMaintenanceDocumentType,
        View_MachineMaintenanceDocument,
        View_MachineMaintenanceApplication,
        View_MachineMaintenanceGroup,
        View_MachineMaintenanceBusinessOperation,
        View_MachineMaintenanceKiosk,
        View_MachineMaintenanceHopper,

        View_VoiceVideoCallSummary,
        View_VoiceVideoCallDetails,

        View_AITokensDetails,
        View_AITokensSummary,
        View_DocumentAPIDetails,
        View_DocumentAPISummary,
        View_FaceAPIDetails,
        View_FaceAPISummary,
        View_OCRAPIDetails,
        View_OCRAPISummary,
        #endregion

        #region  Advertisement

        View_AdvertisementCheckerMaker,
        View_AdvertisementManagement,
        Create_AdvertisementManagement,
        Update_AdvertisementManagement,
        Approve_AdvertisementManagement,
        Decline_AdvertisementManagement,

        #endregion

        #region User Maintenance
        View_UserCheckerMaker,

        View_RoleMaintenance,
        Create_RoleMaintenance,
        Update_RoleMaintenance,
        Approve_RoleMaintenance,
        Decline_RoleMaintenance,

        View_UserMaintenance,
        Create_UserMaintenance,
        Update_UserMaintenance,
        Approve_UserMaintenance,
        Decline_UserMaintenance,

        View_BranchMaintenance,
        Create_BranchMaintenance,
        Update_BranchMaintenance,
        Approve_BranchMaintenance,
        Decline_BranchMaintenance,
        #endregion

        #region Download SDKs
        View_SDKs,
        #endregion

        #region Download Resources
        View_Resources,
        #endregion

        #region Settings
        View_SettingCheckerMaker,
        View_MasterSetting,
        Create_MasterSetting,
        Update_MasterSetting,
        Approve_MasterSetting,
        Decline_MasterSetting,
        #endregion

        #region Android MDM
        View_AndroidMDMDevices,

        View_AndroidMDMDeviceGroup,
        Create_AndroidMDMDeviceGroup,
        Update_AndroidMDMDeviceGroup,

        View_AndroidMDMProfile,
        Create_AndroidMDMProfile,
        Update_AndroidMDMProfile,

        Create_AndroidMDMAppUpload,
        View_AndroidMDMApp,

        Push_AndroidMDMPushProfile,

        Push_AndroidMDMPushApp,
        Push_AndroidMDMRemoveApp,
        Push_AndroidMDMEnableKioskMode,
        Push_AndroidMDMRemoveKioskMode,
        Push_AndroidMDMEnableGeoFenceMode,
        Push_AndroidMDMRemoveGeoFenceMode,
        Update_AndroidMDMGeoFenceConfig,
        #endregion

        #region iOS MDM

        View_iOSMDMCheckerMaker,
        View_iOSMDMEnrollmentManagement,
        Approve_iOSMDMEnrollment,
        Decline_iOSMDMEnrollment,



        View_iOSMDMDeviceManagement,
        Update_iOSDeviceManagement,

        View_iOSMDMProfileManagement,
        Approve_iOSMDMProfile,
        Decline_iOSMDMProfile,
        Create_iOSMDMProfile,
        Update_iOSMDMProfile,

        View_iOSMDMApplicationManagement,
        Create_iOSMDMAppUpload,
        Update_iOSMDMAppUpload,
        Approve_iOSMDMAppUpload,
        Decline_iOSMDMAppUpload,
        Enabled_iOSMDMAppUpload,

        Create_iOSMDMApp,
        Update_iOSMDMApp,

        View_iOSMDMPushProfile,
        View_iOSMDMPushApplication,








        Push_iOSMDMPushProfile,


        Send_iOSMDMClearPasscode,
        Send_iOSMDMRestartDevice,
        Send_iOSMDMShutdownDevice,
        Send_iOSMDMGetOSUpdate,
        Send_iOSMDMFactoryReset,

        Send_iOSMDMEnableLostMode,
        Send_iOSMDMDisableLostMode,
        Send_iOSMDMPlayLostModeSound,
        Send_iOSMDMDeviceLocationLostMode,

        Send_iOSMDMSingleAppMode,
        Send_iOSMDMInstallApps,
        Send_iOSMDMRemoveApps,
        #endregion

        #region Windows MDM

        /*---------View Only---------*/
        View_WinMDMEnrollmentManagement,
        View_WinMDMDeviceManagement,
        View_WinMDMProfileManagement,
        View_WinMDMApplicationManagement,
        View_WinMDMPushProfile,
        View_WinMDMPushApplication,

        /*---------Actions Below---------*/
        /*iOS MDM Device Management Page*/
        Send_WinMDMReboot,


        #endregion

        #region  Data Analytics
        View_DataAnalytics,
        #endregion

        #region  Marketing Automation
        View_MarketingAutomation,
        #endregion



        View_Application,
        Create_Application,
        Update_Application,
        Approve_Application,
        Decline_Application,

        View_Card_Maintenance,
        Create_Card_Maintenance,
        Update_Card_Maintenance,
        Approve_Card_Maintenance,
        Decline_Card_Maintenance,

        View_Kiosk,
        Create_Kiosk,
        Update_Kiosk,
        Approve_Kiosk,
        Decline_Kiosk,

        View_DocType_Template,
        Create_DocType_Template,
        Update_DocType_Template,
        Approve_DocType_Template,
        Decline_DocType_Template,

        View_Document_Template,
        Create_Document_Template,
        Update_Document_Template,
        Approve_Document_Template,
        Decline_Document_Template,

        View_Hopper_Template,
        Create_Hopper_Template,
        Update_Hopper_Template,
        Approve_Hopper_Template,
        Decline_Hopper_Template,

        View_BusinessHour_Template,
        Create_BusinessHour_Template,
        Update_BusinessHour_Template,
        Approve_BusinessHour_Template,
        Decline_BusinessHour_Template,

        View_Service_Template,
        Create_Service_Template,
        Update_Service_Template,
        Approve_Service_Template,
        Decline_Service_Template,

        View_Wrap_Template,
        Create_Wrap_Template,
        Update_Wrap_Template,
        Approve_Wrap_Template,
        Decline_Wrap_Template,

        View_Master_Setting,
        Create_Master_Setting,
        Update_Master_Setting,
        Approve_Master_Setting,
        Decline_Master_Setting,

        View_Security_Pay_Cheque,
        Create_Security_Pay_Cheque,
        Update_Security_Pay_Cheque,
        Approve_Security_Pay_Cheque,
        Decline_Security_Pay_Cheque,

        View_Alert_Template,
        Create_Alert_Template,
        Update_Alert_Template,
        Approve_Alert_Template,
        Decline_Alert_Template,

        View_Group_Template,
        Create_Group_Template,
        Update_Group_Template,
        Approve_Group_Template,
        Decline_Group_Template,

        View_KioskManagement,



        #region  Agent Video Conference
        View_AgentVideoConference,
        #endregion
    }


    public class Module
    {
        /// <summary>
        /// Return module id (Guid) if exists in db for the performed action. 
        /// Otherwise, return 00000000-0000-0000-0000-000000000000. 
        /// </summary>
        /// <param name="action">E.g. User_Logout (see enum ModuleLogAction)</param>
        /// <returns>E.g. 0fe0775e-5609-4185-a305-96f075e15c65 (for logout module)</returns>
        public static Guid GetModuleId(ModuleLogAction action)
        {
            Guid ret = Guid.Empty;
            string[] tmp = action.ToString().Split('_');

            switch (tmp[1].ToString())
            {
                #region Reporting
                /*Reporting*/
                case "AuditTrailSystem": ret = Guid.Parse("a00001cb-31e4-4b3c-95bc-dd357fda648e"); break;
                case "AuditTrailAndroid": ret = Guid.Parse("a0a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "AuditTrailiOS": ret = Guid.Parse("a0f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "AuditTrailWindows": ret = Guid.Parse("a09066d5-799e-4d41-af79-6c9c5ba7ce63"); break;

                case "DeviceListingAndroid": ret = Guid.Parse("b0a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "DeviceListingiOS": ret = Guid.Parse("b0f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "DeviceListingWindows": ret = Guid.Parse("b09066d5-799e-4d41-af79-6c9c5ba7ce63"); break;

                case "OutofServiceAndroid": ret = Guid.Parse("c0a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "OutofServiceiOS": ret = Guid.Parse("c0f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "OutofServiceWindows": ret = Guid.Parse("c09066d5-799e-4d41-af79-6c9c5ba7ce63"); break;

                case "DeviceComponentAndroid": ret = Guid.Parse("d0a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "DeviceComponentiOS": ret = Guid.Parse("d0f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "DeviceComponentWindows": ret = Guid.Parse("d09066d5-799e-4d41-af79-6c9c5ba7ce63"); break;

                case "DeviceUptimeAndroid": ret = Guid.Parse("e0a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "DeviceUptimeiOS": ret = Guid.Parse("e0f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "DeviceUptimeWindows": ret = Guid.Parse("e09066d5-799e-4d41-af79-6c9c5ba7ce63"); break;

                case "DeviceSLAAndroid": ret = Guid.Parse("f0a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "DeviceSLAiOS": ret = Guid.Parse("f0f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "DeviceSLAWindows": ret = Guid.Parse("f09066d5-799e-4d41-af79-6c9c5ba7ce63"); break;

                case "FirmwareKeyAndroid": ret = Guid.Parse("a1a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "FirmwareKeyiOS": ret = Guid.Parse("a1f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "FirmwareKeyWindows": ret = Guid.Parse("a19066d5-799e-4d41-af79-6c9c5ba7ce63"); break;

                case "TransDetailsMSF": ret = Guid.Parse("a0d2e4e7-f459-49f9-8334-ae228e64d380"); break;
                case "TransSummaryMSF": ret = Guid.Parse("a1d2e4e7-f459-49f9-8334-ae228e64d380"); break;
                case "FinancingProductMSF": ret = Guid.Parse("a2d2e4e7-f459-49f9-8334-ae228e64d380"); break;

                case "DepositProductMOB": ret = Guid.Parse("a1ca7fdc-23ff-4591-8a19-51516b8ae655"); break;

                case "AppInstallationiOS": ret = Guid.Parse("a2f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "AppInstallationAndroid": ret = Guid.Parse("a2a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "AppInstallationWindows": ret = Guid.Parse("a2f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;

                case "AppDevicesiOS": ret = Guid.Parse("a3f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "AppDevicesAndroid": ret = Guid.Parse("a3a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "AppDevicesWindows": ret = Guid.Parse("a3f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;

                case "DataUsageiOS": ret = Guid.Parse("a4f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "DataUsageAndroid": ret = Guid.Parse("a4a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "DataUsageWindows": ret = Guid.Parse("a4f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;

                case "EnrollmentiOS": ret = Guid.Parse("a5f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "EnrollmentAndroid": ret = Guid.Parse("a5a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "EnrollmentWindows": ret = Guid.Parse("a5f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;

                case "HardwareiOS": ret = Guid.Parse("a6f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "HardwareAndroid": ret = Guid.Parse("a6a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "HardwareWindows": ret = Guid.Parse("a6f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;

                case "LocationSummaryiOS": ret = Guid.Parse("a7f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;
                case "LocationSummaryAndroid": ret = Guid.Parse("a7a55bca-1fdc-4bd5-9679-be0e03fa4db1"); break;
                case "LocationSummaryWindows": ret = Guid.Parse("a7f9a964-96c3-4ea2-9ea0-64efdc90c30f"); break;

                case "MachineMaintenanceCard": ret = Guid.Parse("11d2e4e7-f459-49f9-8334-ae228e64d001"); break;
                case "MachineMaintenanceAlert": ret = Guid.Parse("11d2e4e7-f459-49f9-8334-ae228e64d002"); break;
                case "MachineMaintenanceDocumentType": ret = Guid.Parse("11d2e4e7-f459-49f9-8334-ae228e64d003"); break;
                case "MachineMaintenanceDocument": ret = Guid.Parse("11d2e4e7-f459-49f9-8334-ae228e64d004"); break;
                case "MachineMaintenanceApplication": ret = Guid.Parse("11d2e4e7-f459-49f9-8334-ae228e64d005"); break;
                case "MachineMaintenanceGroup": ret = Guid.Parse("11d2e4e7-f459-49f9-8334-ae228e64d006"); break;
                case "MachineMaintenanceBusinessOperation": ret = Guid.Parse("11d2e4e7-f459-49f9-8334-ae228e64d007"); break;
                case "MachineMaintenanceKiosk": ret = Guid.Parse("11d2e4e7-f459-49f9-8334-ae228e64d008"); break;
                case "MachineMaintenanceHopper": ret = Guid.Parse("11d2e4e7-f459-49f9-8334-ae228e64d009"); break;

                case "VoiceVideoCallSummary": ret = Guid.Parse("21d2e4e7-f459-49f9-8334-ae228e64d001"); break;
                case "VoiceVideoCallDetails": ret = Guid.Parse("21d2e4e7-f459-49f9-8334-ae228e64d002"); break;

                case "AITokensSummary": ret = Guid.Parse("31d2e4e7-f459-49f9-8334-ae228e64d001"); break;
                case "AITokensDetails": ret = Guid.Parse("31d2e4e7-f459-49f9-8334-ae228e64d002"); break;

                case "DocumentAPISummary": ret = Guid.Parse("41d2e4e7-f459-49f9-8334-ae228e64d001"); break;
                case "DocumentAPIDetails": ret = Guid.Parse("41d2e4e7-f459-49f9-8334-ae228e64d002"); break;

                case "FaceAPISummary": ret = Guid.Parse("51d2e4e7-f459-49f9-8334-ae228e64d001"); break;
                case "FaceAPIDetails": ret = Guid.Parse("51d2e4e7-f459-49f9-8334-ae228e64d002"); break;

                case "OCRAPISummary": ret = Guid.Parse("61d2e4e7-f459-49f9-8334-ae228e64d001"); break;
                case "OCRAPIDetails": ret = Guid.Parse("61d2e4e7-f459-49f9-8334-ae228e64d002"); break;

                #endregion

                #region Advertisement
                case "AdvertisementCheckerMaker": ret = Guid.Parse("ad222f6c-2bf1-4d9a-b367-8be3e4581a73"); break;
                case "AdvertisementManagement": ret = Guid.Parse("ad333f6c-2bf1-4d9a-b367-8be3e4581a73"); break;
                case "Advertisement": ret = Guid.Parse("ad444f6c-2bf1-4d9a-b367-8be3e4581a73"); break;
                #endregion

                #region User Maintenance
                case "UserCheckerMaker": ret = Guid.Parse("5aacb385-bdca-46c1-a31f-82489e737c0e"); break;
                case "RoleMaintenance": ret = Guid.Parse("3741A6B9-8D92-406A-8E6A-47CF2BC13585"); break;
                case "UserMaintenance": ret = Guid.Parse("B91FB678-2F48-4C56-9047-D2F906FCACCC"); break;
                case "BranchMaintenance": ret = Guid.Parse("bdd69a23-6dd0-4c90-ac9f-6ffb5045cb79"); break;
                #endregion

                #region Download SDKs
                case "SDKs": ret = Guid.Parse("d88b88e8-9950-47e9-85a5-ea74605def85"); break;
                #endregion

                #region Download Resources
                case "Resources": ret = Guid.Parse("d88b88e8-9950-47e9-85a5-ea74605def86"); break;
                #endregion

                #region Settings
                case "SettingCheckerMaker": ret = Guid.Parse("2dc935a2-6fb1-4b4d-ba2b-26472647d688"); break;
                case "MasterSetting": ret = Guid.Parse("a323f166-eb3b-4c76-9f3a-1c91bf455670"); break;
                #endregion

                #region Android MDM
                case "AndroidMDMDevices": ret = Guid.Parse("020305ce-857d-49b8-abf6-4eba61fd7fa1"); break;
                case "AndroidMDMDeviceGroup": ret = Guid.Parse("01052561-f071-454d-a10c-1053f8ddfb1d"); break;
                case "AndroidMDMProfile": ret = Guid.Parse("050a01bb-6886-437a-997d-f77ca8b79e11"); break;
                case "AndroidMDMAppUpload":
                case "AndroidMDMApp": ret = Guid.Parse("030d64c8-fdf0-49c1-adaf-cb92d9868f06"); break;
                case "AndroidMDMPushProfile": ret = Guid.Parse("060718e8-632f-4d63-b56a-6002d0dedad3"); break;

                case "Push_AndroidMDMRemoveApp":
                case "Push_AndroidMDMEnableKioskMode":
                case "Push_AndroidMDMRemoveKioskMode":
                case "Push_AndroidMDMEnableGeoFenceMode":
                case "Push_AndroidMDMRemoveGeoFenceMode":
                case "Update_AndroidMDMGeoFenceConfig":
                case "AndroidMDMPushApp": ret = Guid.Parse("0400985d-4d4b-4464-ae19-64a5afe4236c"); break;
                #endregion

                #region iOS MDM
                case "iOSMDMCheckerMaker": ret = Guid.Parse("cabd22ed-7612-42cd-9a4d-937db21a59d5"); break;
                case "iOSMDMEnrollmentManagement":
                case "iOSEnrollment": ret = Guid.Parse("cabd33ed-7612-42cd-9a4d-937db21a59d5"); break;
                case "iOSMDMApplicationManagement":
                case "iOSMDMAppUpload":
                case "iOSMDMApp": ret = Guid.Parse("cabd66ed-7612-42cd-9a4d-937db21a59d5"); break;
                case "iOSMDMProfileManagement":
                case "iOSMDMProfile": ret = Guid.Parse("cabd55ed-7612-42cd-9a4d-937db21a59d5"); break;
                case "iOSMDMPushProfile": ret = Guid.Parse("cabd77ed-7612-42cd-9a4d-937db21a59d5"); break;

                /*iOS MDM Device Management Page*/
                case "iOSMDMDeviceManagement":
                case "iOSMDMClearPasscode":
                case "iOSMDMRestartDevice":
                case "iOSMDMShutdownDevice":
                case "iOSMDMGetOSUpdate":
                case "iOSMDMFactoryReset":
                case "iOSMDMEnableLostMode":
                case "iOSMDMDisableLostMode":
                case "iOSMDMPlayLostModeSound":
                case "iOSMDMDeviceLocationLostMode": ret = Guid.Parse("cabd44ed-7612-42cd-9a4d-937db21a59d5"); break;

                /*iOS_Push_Application Page*/
                case "iOSMDMPushApplication":
                case "iOSMDMSingleAppMode":
                case "iOSMDMInstallApps":
                case "iOSMDMRemoveApps": ret = Guid.Parse("cabd88ed-7612-42cd-9a4d-937db21a59d5"); break;
                #endregion

                #region Windows MDM

                case "WinMDMEnrollmentManagement": ret = Guid.Parse("13c9e8a2-1399-48ed-8854-48ba2ee8fdff"); break;
                case "WinMDMDeviceManagement":
                case "WinMDMReboot": ret = Guid.Parse("13c9e8a3-1399-48ed-8854-48ba2ee8fdff"); break;
                case "WinMDMProfileManagement": ret = Guid.Parse("13c9e8a4-1399-48ed-8854-48ba2ee8fdff"); break;
                case "WinMDMApplicationManagement": ret = Guid.Parse("13c9e8a5-1399-48ed-8854-48ba2ee8fdff"); break;
                case "WinMDMPushProfile": ret = Guid.Parse("13c9e8a6-1399-48ed-8854-48ba2ee8fdff"); break;
                case "WinMDMPushApplication": ret = Guid.Parse("13c9e8a7-1399-48ed-8854-48ba2ee8fdff"); break;
                #endregion

                #region Kiosk
                case "KioskManagement": ret = Guid.Parse("e19ce568-2258-49ab-8811-973e6f9de6ef"); break;
                case "Card": ret = Guid.Parse("5367C88C-C596-4F5D-BB8C-F65B5104E924"); break;
                case "Alert": ret = Guid.Parse("53701E8A-8D28-4C7C-A7BC-C3C39FCC61B9"); break;
                case "DocType": ret = Guid.Parse("8082d926-7adc-4ce1-bf6f-4244318e26e1"); break;
                case "Document": ret = Guid.Parse("A17CC31E-16A9-4BF2-9EEC-45CECAB0C82C"); break;
                case "Application": ret = Guid.Parse("c3eb0893-687e-4227-aeab-27884ba15c85"); break;
                case "Group": ret = Guid.Parse("CD9FDEC0-01CE-4185-8320-CBB7145F0EF4"); break;
                case "BusinessHour": ret = Guid.Parse("F95B0C18-F639-4142-8D8F-A4B5773FA085"); break;
                case "Kiosk": ret = Guid.Parse("D2592074-78E9-4C82-BD2F-E9F25ED8D96C"); break;
                case "Hopper": ret = Guid.Parse("10A3F8F2-D7C8-4366-BC22-E09424A5F071"); break;
                #endregion

                #region Agent Video Conference
                case "AgentVideoConference": ret = Guid.Parse("2a27bce0-f99c-4db1-9c7e-cf48bfaac829"); break;
                #endregion

                #region Data Analytics
                case "DataAnalytics": ret = Guid.Parse("ebf8d8cc-9027-4502-81ec-ecc69ee7c83f"); break;
                #endregion

                #region Marketing Automation
                case "MarketingAutomation": ret = Guid.Parse("06ad3576-b642-4ece-b969-63e902b06777"); break;
                #endregion

                default: ret = new Guid(); break;

            }
            return ret;
        }

    }
}
