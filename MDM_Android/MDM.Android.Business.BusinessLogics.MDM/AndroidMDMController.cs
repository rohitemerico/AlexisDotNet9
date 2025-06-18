using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.Android.Common.Data.Component;

namespace MDM.Android.Business.BusinessLogics.MDM;

public class AndroidMDMController
{
    /// <summary>
    /// Get all android mdm devices info for reporting module. I.e.
    /// <para>DID, DEVICE_CLIENT_ID, DEVICE_NAME, DEVICE_STATUS, DEVICE_LOCATION,</para>
    /// <para>GROUPNAME, BATTERY_LEVEL, ENROLLED_ON, LAST_SYNC_ON,</para>
    /// </summary>
    /// <remarks></remarks>
    /// <returns></returns>
    public static DataTable GetAllDevices_Report()
    {
        var query_dt = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            sql.AppendLine("SELECT ");
            sql.AppendLine("device.ID, device.deviceMACAdd, device.DEVICENAME, device.MODELNAME,  ");
            sql.AppendLine("device.DEVICESTATUS, device.BATTERYLEVEL, ");
            sql.AppendLine(" device.CONNECTIONSTATUS, device.TOUCHSCREENSTATUS, device.CARDREADERSPERDAY, ");
            sql.AppendLine("CONCAT(CONCAT(device.LATITUDE, ','), device.LONGITUDE) as DEVICE_LOCATION, ");
            sql.AppendLine("device.enrollDatetime, device.lastSyncDatetime, ");
            sql.AppendLine("device.FirmwareVersion, device.FirmwareBatteryStatus, device.FirmwareBatteryLevel, device.FirmwareSerial, ");
            sql.AppendLine("deviceGroup.GROUPNAME ");
            sql.AppendLine("FROM ");
            sql.AppendLine("ANDROIDMDM_DEVICES device, ANDROIDMDM_DEVICE_GROUP deviceGroup ");
            sql.AppendLine("WHERE ");
            sql.AppendLine("device.deviceGroup = deviceGroup.GID ");

            query_dt = SqlDataCtrl_Android.GetResult(sql.ToString(), myParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return query_dt;
    }
    /// <summary>
    /// Get all android mdm device groups info for reporting module. I.e.
    /// <para>GID, GROUPNAME, GROUPDESC,</para>
    /// <para>CREATEDDATE, CREATED_BY, UPDATEDDATE, UPDATED_BY</para>
    /// </summary>
    /// <remarks></remarks>
    /// <returns></returns>
    public static DataTable GetAllDeviceGroups_Report()
    {
        var query_dt = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            sql.AppendLine("SELECT ");
            sql.AppendLine("deviceGroup.GID , deviceGroup.GROUPNAME , deviceGroup.GROUPDESC , ");
            sql.AppendLine("deviceGroup.CREATEDDATE , deviceGroup.UPDATEDDATE , ");
            sql.AppendLine("userCreate.UNAME CREATED_BY, userUpdate.UNAME UPDATED_BY ");
            sql.AppendLine("FROM ");
            sql.AppendLine("ANDROIDMDM_DEVICE_GROUP deviceGroup ");
            sql.AppendLine("left join USER_LOGIN userCreate on (deviceGroup.CREATEDBY = userCreate.AID) ");
            sql.AppendLine("left join USER_LOGIN userUpdate on (deviceGroup.UPDATEDBY = userUpdate.AID) ");

            query_dt = SqlDataCtrl_Android.GetResult(sql.ToString(), myParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return query_dt;
    }
    /// <summary>
    /// Get all android mdm apps info for reporting module. I.e.
    /// <para>APPID, APPLICATION_NAME, FPATH, VER,</para>
    /// <para>CREATED_ON, CREATED_BY, UPDATED_ON, UPDATED_BY</para>
    /// </summary>
    /// <remarks></remarks>
    /// <returns></returns>
    public static DataTable GetAllApp_Report()
    {
        DataTable query_dt = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            sql.AppendLine("SELECT app.APPID, app.APPLICATION_NAME, app.FPATH, app.VER, ");
            sql.AppendLine("app.CREATED_ON, app.UPDATED_ON, ");
            sql.AppendLine("userCreate.UNAME CREATED_BY, userUpdate.UNAME UPDATED_BY ");
            sql.AppendLine("FROM ");
            sql.AppendLine("ANDROIDMDM_APPLICATION app");
            sql.AppendLine("left join USER_LOGIN userCreate on (app.CREATED_BY = userCreate.AID) ");
            sql.AppendLine("left join USER_LOGIN userUpdate on (app.UPDATED_BY = userUpdate.AID) ");

            query_dt = SqlDataCtrl_Android.GetResult(sql.ToString(), myParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return query_dt;
    }
    /// <summary>
    /// Get all android mdm profiles info for reporting module. I.e.
    /// <para>PRID, PROFILE_NAME, FPATH, STATUS,</para>
    /// <para>CREATED_ON, CREATED_BY, LAST_UPDATED_ON, UPDATED_BY</para>
    /// </summary>
    /// <remarks></remarks>
    /// <returns></returns>
    public static DataTable GetAllProfiles_Report()
    {
        DataTable query_dt = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            sql.AppendLine("SELECT prof.PRID, prof.PROFILE_NAME, prof.FPATH, prof.STATUS, ");
            sql.AppendLine("prof.CREATED_ON, prof.LAST_UPDATED_ON, ");
            sql.AppendLine("userCreate.UNAME CREATED_BY, userUpdate.UNAME UPDATED_BY ");
            sql.AppendLine("FROM ");
            sql.AppendLine("ANDROIDMDM_PROFILE prof");
            sql.AppendLine("left join USER_LOGIN userCreate on (prof.CREATED_BY = userCreate.AID) ");
            sql.AppendLine("left join USER_LOGIN userUpdate on (prof.LAST_UPDATED_BY = userUpdate.AID) ");

            query_dt = SqlDataCtrl_Android.GetResult(sql.ToString(), myParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return query_dt;
    }
    public static DataTable AppGetAll()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select *");
            sql.AppendLine("from ANDROIDMDM_APPLICATION");
            List<Params> MyParams = new List<Params>();

            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
    public static bool AppAddEntry(string appName, string packageName, string version, string appFilePath, string status)
    {
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("insert into ANDROIDMDM_APPLICATION ");
            sql.AppendLine(" (AppID, FPATH, APPLICATION_NAME,PACKAGE_NAME, VER, UPDATED_ON) ");
            //, STATUS, CREATED_ON
            sql.AppendLine(" values ");
            sql.AppendLine(" (:guid, :appFilePath, :appName, :packageName, :version, :UpdateDT) ");
            //, :STATUS, :createDT

            MyParams.Add(new Params(":guid", "NVARCHAR", Guid.NewGuid()));
            MyParams.Add(new Params(":appFilePath", "NVARCHAR", appFilePath));
            MyParams.Add(new Params(":version", "NVARCHAR", version));
            MyParams.Add(new Params(":appName", "NVARCHAR", appName));
            MyParams.Add(new Params(":packageName", "NVARCHAR", packageName));
            //MyParams.Add(new Params(":createDT", "DATETIME", DateTime.Now));
            MyParams.Add(new Params(":UpdateDT", "DATETIME", DateTime.Now));

            bool ret = SqlDataCtrl_Android.Input(sql.ToString(), MyParams);
            return ret;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return false;
    }
    public static bool AppUpdate(int appSysID, string appName = null, string version = null, string appFilePath = null, string status = null)
    {
        //return false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("update ANDROIDMDM_APPLICATION set ");
            if (appFilePath != null)
            {
                sql.AppendLine(" FPATH = :appFilePath ,");
                MyParams.Add(new Params(":appFilePath", "NVARCHAR", appFilePath));
            }
            if (status != null)
            {
                sql.AppendLine(" STATUS = :status ,");
                MyParams.Add(new Params(":status", "NVARCHAR", status));
            }
            if (version != null)
            {
                sql.AppendLine(" VER = :version ,");
                MyParams.Add(new Params(":version", "NVARCHAR", version));
            }
            if (appName != null)
            {
                sql.AppendLine(" APPLICATIONNAME = :appName ,");
                MyParams.Add(new Params(":appName", "NVARCHAR", appName));
            }
            sql.AppendLine(" UPDATEDDATETIME = :UpdateDT ");
            MyParams.Add(new Params(":UpdateDT", "DATETIME", DateTime.Now));
            sql.AppendLine("where SYSID = :appSysID");
            MyParams.Add(new Params(":appSysID", "INT", appSysID));

            bool ret = SqlDataCtrl_Android.Input(sql.ToString(), MyParams); ;
            return ret;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return false;
    }
    public static bool AppDelete(int appSysID)//, string appName = null, string version = null, string appFilePath = null, string status = null)
    {
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("delete from ANDROIDMDM_APPLICATION where sysid = :appSysID");
            MyParams.Add(new Params(":appSysID", "INT", appSysID));

            bool ret = SqlDataCtrl_Android.Input(sql.ToString(), MyParams); ;
            return ret;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return false;
    }
    public static DataTable GetAppByID(string ApplicationGUID)//, string appName = null, string version = null, string appFilePath = null, string status = null)
    {
        DataTable ret = null;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("Select * from ANDROIDMDM_APPLICATION where AppID = :ApplicationGUID");
            MyParams.Add(new Params(":ApplicationGUID", "NVARCHAR", ApplicationGUID));

            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
    public static DataTable getAllProfiles()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select ");
            sql.AppendLine("  * ");
            sql.AppendLine("from ");
            sql.AppendLine("  ANDROIDMDM_PROFILE ");
            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
    public static DataTable GetAllDevices()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select ");
            sql.AppendLine("  ad.ID, ad.deviceMACAdd, ad.DEVICENAME, ad.DEVICESTATUS, ");
            sql.AppendLine("  ad.CONNECTIONSTATUS, ad.TOUCHSCREENSTATUS, ad.CARDREADERSPERDAY, ");
            sql.AppendLine("  ad.latitude, ad.longitude, ad.batteryLevel, ");
            sql.AppendLine("  ad.enrollDatetime, ad.lastSyncDatetime, ");
            sql.AppendLine("  ad.Restriction_LATITUDE, ad.Restriction_LONGITUDE, ad.Restriction_radius, ");
            sql.AppendLine("  adg.GROUPNAME ");
            sql.AppendLine("from ");
            sql.AppendLine("  ANDROIDMDM_DEVICES ad, ANDROIDMDM_DEVICE_GROUP adg ");
            sql.AppendLine("where ");
            sql.AppendLine("  ad.deviceGroup = adg.GID ");
            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
    public static DataTable GetAndroidOnOffCount(bool ToGetOnline)
    {
        StringBuilder sql = new StringBuilder();
        List<Params> myParams = new List<Params>();
        DataTable ret = new DataTable();
        try
        {
            sql.AppendLine(@"select count(DEVICESTATUS)as count_ 
                                from androidmdm_devices where");

            if (ToGetOnline)
                sql.AppendLine("DEVICESTATUS = '1'");
            else
                sql.AppendLine("DEVICESTATUS = '0'");

            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), myParams);
        }

        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                         System.Reflection.MethodBase.GetCurrentMethod().Name,
                         ex);
        }
        return ret;
    }
    public static bool IsProfileExists(string profileName, string prID)
    {
        bool isDifferentPrid = false;
        DataTable profilesDataTable = getAllProfiles();
        foreach (DataRow row in profilesDataTable.Rows)
        {
            string existingProfileName = row["Profile_Name"].ToString();
            string existingPrid = row["PRID"]?.ToString();

            if (profileName == existingProfileName)
            {
                isDifferentPrid = prID == null || prID.ToString() != existingPrid;

                if (isDifferentPrid)
                {
                    return isDifferentPrid;
                }
            }
        }
        return isDifferentPrid;
    }
    public static DataTable GetAllDevicesByGroupID(string deviceGroupID)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select ");
            sql.AppendLine("  * ");
            sql.AppendLine("from ");
            sql.AppendLine("  androidmdm_devices ad ");
            sql.AppendLine("where ");
            sql.AppendLine("  ad.deviceGroup = :deviceGroupID ");
            MyParams.Add(new Params(":deviceGroupID", "NVARCHAR", deviceGroupID));
            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
    public static DataTable getAllMDMDeviceGroup()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select ");
            sql.AppendLine("grp.*,");
            sql.AppendLine("ulc.uName Created_By, ulu.uName Updated_By ");
            sql.AppendLine("from ");
            sql.AppendLine("AndroidMDM_Device_Group grp");
            sql.AppendLine("left join User_Login ulc on (grp.CreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ulu on (grp.UpdatedBy = ulu.aID) ");

            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
    public static bool EditDeviceGroup(string groupID, string nameGroupDevice, string descGroupDevice, string guidCreatedBy)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("update ANDROIDMDM_DEVICE_GROUP set ");
            sql.AppendLine(" GROUPNAME = :nameDevice ,");
            sql.AppendLine(" GROUPDESC = :descDevice ,");
            sql.AppendLine(" UPDATEDBY = :guidUpdatedBy ,");
            sql.AppendLine(" UPDATEDDATE = :dateUpdated ");
            sql.AppendLine("where GID = :groupID");

            MyParams.Add(new Params(":nameDevice", "NVARCHAR", nameGroupDevice));
            MyParams.Add(new Params(":descDevice", "NVARCHAR", descGroupDevice));
            MyParams.Add(new Params(":guidUpdatedBy", "NVARCHAR", guidCreatedBy));
            MyParams.Add(new Params(":dateUpdated", "DATETIME", DateTime.Now));
            MyParams.Add(new Params(":groupID", "INT", groupID));


            ret = SqlDataCtrl_Android.Input(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
    public static bool AddDeviceGroup(string nameDevice, string descDevice, string guidCreatedBy)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("Insert into ");
            sql.AppendLine("ANDROIDMDM_DEVICE_GROUP ");
            sql.AppendLine("(GID, GROUPNAME, GROUPDESC, CREATEDBY, CREATEDDATE)  ");
            sql.AppendLine("values ");
            sql.AppendLine("(:gID, :gName, :gDesc, :guidCreatedBy, :dateCreated) ");

            MyParams.Add(new Params(":gID", "NVARCHAR", Guid.NewGuid().ToString()));
            MyParams.Add(new Params(":gName", "NVARCHAR", nameDevice));
            MyParams.Add(new Params(":gDesc", "NVARCHAR", descDevice));
            MyParams.Add(new Params(":guidCreatedBy", "NVARCHAR", guidCreatedBy));
            MyParams.Add(new Params(":dateCreated", "DATETIME", DateTime.Now));

            ret = SqlDataCtrl_Android.Input(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
    public static string InsertPushNotificationCommand(string sCommand, string userID)
    {
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("Insert into  AndroidMDM_PushNotification (command ) ");
            sql.AppendLine(" values ( @command ) ;");
            sql.AppendLine(" SELECT SCOPE_IDENTITY();");

            //string id = Guid.NewGuid().ToString();
            //MyParams.Add(new Params("@ID", "NVARCHAR", id));
            MyParams.Add(new Params("@command", "NVARCHAR", sCommand));

            int id = SqlDataCtrl_Android.InputReturnId(sql.ToString(), MyParams);
            return id.ToString();
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return "";
    }
    public static string InsertPushNotificationDevice(string notificationID, string sDeviceID, string userID)
    {
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("Insert into ");
            sql.AppendLine(" AndroidMDM_PushNotification_Devices ");
            sql.AppendLine("( NOTIFICATIONID, deviceMACAdd ) ");
            sql.AppendLine("values");
            sql.AppendLine(" (:NOTIFICATIONID, :deviceMACAdd ) ;");
            sql.AppendLine(" SELECT SCOPE_IDENTITY();");

            //string id = Guid.NewGuid().ToString();
            //MyParams.Add(new Params(":ID", "NVARCHAR", id));
            MyParams.Add(new Params(":NOTIFICATIONID", "NVARCHAR", notificationID));
            MyParams.Add(new Params(":deviceMACAdd", "NVARCHAR", sDeviceID));

            int id = SqlDataCtrl_Android.InputReturnId(sql.ToString(), MyParams);
            return id.ToString();
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return "";
    }
    public static DataTable getAllProfileRestrictionsByID(string ProfileID)
    {
        DataTable ret = null;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select ");
            sql.AppendLine("  apr.RESTRICTION_ID, CAST(apr.ACTIVE as bit) ACTIVE , T.DESCRIPTION ");
            sql.AppendLine("from ");
            sql.AppendLine("  ANDROIDMDM_PROFILE_REST apr ");
            sql.AppendLine("  INNER JOIN ANDROIDMDM_PROFILE_REST_TEMP T on T.RESTRICTION_ID = apr.RESTRICTION_ID  ");
            sql.AppendLine("where apr.Profile_ID = :id ");
            MyParams.Add(new Params(":id", "NVARCHAR", ProfileID));
            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }

    private static object _saveProfileRestrictionsLock = new object();
    public static bool saveProfileRestrictionsByID(string ProfileID, string profileName, string filePath, Dictionary<string, bool> keyRestrictionIDStatusPairs, string userID)
    {
        bool ret = false;
        lock (_saveProfileRestrictionsLock)
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            DataTable tableProfile = getProfileByID(ProfileID);

            try
            {
                if (tableProfile == null || tableProfile.Rows.Count < 1)
                {
                    sql.Clear();
                    MyParams.Clear();
                    sql.AppendLine("INSERT INTO ANDROIDMDM_PROFILE ");
                    sql.AppendLine(" ( PRID, PROFILE_NAME, Fpath, Status, Created_By, Created_On, LAST_UPDATED_BY, LAST_UPDATED_ON ) ");
                    sql.AppendLine(" values ( ");
                    sql.AppendLine(" :PRID, :PROFILE_NAME, :Fpath, :Status, :Created_By, :Created_On, :LAST_UPDATED_BY, :LAST_UPDATED_ON ");
                    sql.AppendLine(" ) ");

                    ProfileID = Guid.NewGuid().ToString();
                    MyParams.Add(new Params(":PRID", "NVARCHAR", ProfileID));
                    MyParams.Add(new Params(":PROFILE_NAME", "NVARCHAR", profileName));
                    MyParams.Add(new Params(":Fpath", "NVARCHAR", filePath));
                    MyParams.Add(new Params(":Status", "INT", 1));
                    MyParams.Add(new Params(":Created_By", "NVARCHAR", userID));
                    MyParams.Add(new Params(":Created_On", "DATETIME", DateTime.Now));
                    MyParams.Add(new Params(":LAST_UPDATED_ON", "DATETIME", DateTime.Now));
                    MyParams.Add(new Params(":LAST_UPDATED_BY", "NVARCHAR", userID));

                    ret = SqlDataCtrl_Android.Input(sql.ToString(), MyParams);

                    if (!ret) ProfileID = "";
                    //tableProfile = getProfileByID(ProfileID);
                    //if (tableProfile == null || tableProfile.Rows.Count < 1)
                    //    return false;
                    //ProfileID = tableProfile.Rows[0]["PRID"] as string;
                }

                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("update ANDROIDMDM_PROFILE ");
                sql.AppendLine(" set ");
                sql.AppendLine(" PROFILE_NAME = :profileName, ");
                sql.AppendLine(" Fpath = :Fpath, ");
                sql.AppendLine(" LAST_UPDATED_ON = :updatedDate, ");
                sql.AppendLine(" LAST_UPDATED_BY = :updatedBy ");
                sql.AppendLine("where PRID = :profileID ");
                MyParams.Add(new Params(":profileName", "NVARCHAR", profileName));
                MyParams.Add(new Params(":Fpath", "NVARCHAR", filePath));
                MyParams.Add(new Params(":updatedDate", "DATETIME", DateTime.Now));
                MyParams.Add(new Params(":updatedBy", "NVARCHAR", userID));
                MyParams.Add(new Params(":profileID", "NVARCHAR", ProfileID));
                ret = SqlDataCtrl_Android.Input(sql.ToString(), MyParams);

                DataTable ProfileRestrictionTable = getAllProfileRestrictionsByID(ProfileID);
                for (int i = 0; i < ProfileRestrictionTable.Rows.Count; ++i)
                {
                    string restrID = ProfileRestrictionTable.Rows[i]["Restriction_ID"] as string;
                    if (Convert.ToBoolean(ProfileRestrictionTable.Rows[i]["Active"]) != keyRestrictionIDStatusPairs[restrID])
                    {
                        // update existing
                        sql.Clear();
                        MyParams.Clear();
                        sql.AppendLine("update ANDROIDMDM_PROFILE_REST ");
                        sql.AppendLine(" set ");
                        sql.AppendLine(" Active = :ChangedStatus ");
                        sql.AppendLine("where PROFILE_ID = :profileID and RESTRICTION_ID = :restrID ");

                        MyParams.Add(new Params(":ChangedStatus", "INT", keyRestrictionIDStatusPairs[restrID] ? 1 : 0));

                        MyParams.Add(new Params(":profileID", "NVARCHAR", ProfileID));
                        MyParams.Add(new Params(":restrID", "NVARCHAR", restrID));
                        ret = SqlDataCtrl_Android.Input(sql.ToString(), MyParams);
                    }
                    keyRestrictionIDStatusPairs.Remove(restrID);
                }

                foreach (var pair1 in keyRestrictionIDStatusPairs)
                {
                    // add
                    sql.Clear();
                    MyParams.Clear();
                    sql.AppendLine("INSERT INTO ANDROIDMDM_PROFILE_REST ");
                    sql.AppendLine(" ( ID, Profile_ID, RESTRICTION_ID, Active, Created_By, Created_On ) ");
                    sql.AppendLine(" values ( ");
                    sql.AppendLine(" :ID, :Profile_ID, :RESTRICTION_ID, :Active, :Created_By, :Created_On ");
                    sql.AppendLine(" ) ");

                    MyParams.Add(new Params(":ID", "NVARCHAR", Guid.NewGuid()));
                    MyParams.Add(new Params(":Profile_ID", "NVARCHAR", ProfileID));
                    MyParams.Add(new Params(":RESTRICTION_ID", "NVARCHAR", pair1.Key));
                    MyParams.Add(new Params(":Active", "INT", pair1.Value ? 1 : 0));
                    MyParams.Add(new Params(":Created_By", "NVARCHAR", userID));
                    MyParams.Add(new Params(":Created_On", "DATETIME", DateTime.Now));

                    ret = SqlDataCtrl_Android.Input(sql.ToString(), MyParams);
                }

            }
            catch (Exception ex)
            {
                //Logger.LogToFile("getBusinessOperatingAll.log", ex);
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            ex);

            }
        }
        return ret;
    }
    public static DataTable getProfileByID(string id)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select ");
            sql.AppendLine("  * ");
            sql.AppendLine("from ");
            sql.AppendLine("  ANDROIDMDM_PROFILE ap ");
            sql.AppendLine("where ap.PRID = :id ");
            MyParams.Add(new Params(":id", "NVARCHAR", id));
            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
    public static DataTable getAllProfileRestrictionTemplateActive()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select ");
            sql.AppendLine("RESTRICTION_ID, DESCRIPTION ");
            sql.AppendLine("from ");
            sql.AppendLine("ANDROIDMDM_PROFILE_REST_TEMP apft ");
            sql.AppendLine("where apft.ACTIVE = 1 ");
            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
    public static DataTable GetDeviceDataUsage()
    {
        var query_dt = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            sql.AppendLine("SELECT ");
            sql.AppendLine("d.DEVICENAME, d.deviceMACAdd, g.GROUPNAME, ");
            sql.AppendLine("du.MOBILE_DATA_USED, du.WIFI_DATA_USED, ");
            sql.AppendLine("(du.MOBILE_DATA_USED + du.WIFI_DATA_USED) as TOTAL_DATA_USED, ");
            sql.AppendLine("du.LAST_UPDATED, d.DEVICESTATUS ");
            sql.AppendLine("FROM ");
            sql.AppendLine("ANDROIDMDM_DEVICES d ");
            sql.AppendLine("LEFT JOIN ANDROIDMDM_DEVICE_GROUP g ON d.deviceGroup = g.GID ");
            sql.AppendLine("LEFT JOIN ANDROIDMDM_DEVICE_DATA_USAGE du ON d.ID = du.DEVICE_ID ");
            sql.AppendLine("ORDER BY du.LAST_UPDATED DESC");

            query_dt = SqlDataCtrl_Android.GetResult(sql.ToString(), myParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return query_dt;
    }
    public static DataTable GetDeviceById(string deviceId)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select ");
            sql.AppendLine("  ad.ID, ad.deviceMACAdd, ad.DEVICENAME, ad.DEVICESTATUS, ");
            sql.AppendLine("  ad.CONNECTIONSTATUS, ad.TOUCHSCREENSTATUS, ad.CARDREADERSPERDAY, ");
            sql.AppendLine("  ad.latitude, ad.longitude, ad.BATTERYLEVEL, ");
            sql.AppendLine("  ad.enrollDatetime, ad.lastSyncDatetime, ");
            sql.AppendLine("  ad.Restriction_LATITUDE, ad.Restriction_LONGITUDE, ad.Restriction_radius ");
            sql.AppendLine("from ");
            sql.AppendLine("  ANDROIDMDM_DEVICES ad");
            sql.AppendLine("where ");
            sql.AppendLine(" ad.deviceMACAdd = :DID");
            MyParams.Add(new Params(":DID", "NVARCHAR", deviceId));
            ret = SqlDataCtrl_Android.GetResult(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }



    public static bool EditDeviceRestriction(string deviceMACAdd, string Restriction_LATITUDE, string Restriction_LONGITUDE, string Restriction_radius)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("update ANDROIDMDM_DEVICES set ");
            sql.AppendLine(" Restriction_LATITUDE = :Restriction_LATITUDE ,");
            sql.AppendLine(" Restriction_LONGITUDE = :Restriction_LONGITUDE ,");
            sql.AppendLine(" Restriction_radius = :Restriction_radius ");
            sql.AppendLine("where deviceMACAdd = :deviceMACAdd");

            MyParams.Add(new Params(":Restriction_LATITUDE", "NVARCHAR", Restriction_LATITUDE));
            MyParams.Add(new Params(":Restriction_LONGITUDE", "NVARCHAR", Restriction_LONGITUDE));
            MyParams.Add(new Params(":Restriction_radius", "NVARCHAR", Restriction_radius));
            MyParams.Add(new Params(":deviceMACAdd", "NVARCHAR", deviceMACAdd));


            ret = SqlDataCtrl_Android.Input(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        return ret;
    }
}