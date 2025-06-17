using System.Data;
using System.DirectoryServices;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Common.Data.Component;
using MDM.iOS.Entities;
using MDM.iOS.Entities.Generic;
using MDM.iOS.Entities.MDM;
using MDM.iOS.Entities.Monitoring;

public class MonitoringDefault : MonitoringBase
{
    private SearchResult result = null;

    public override void Dispose() { }

    /// <summary>
    /// Verify if the user is authenticated into the dashboard web application and returns a boolean value. 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public override bool IsAuthenticated(GenericRequestEn entity)
    {
        bool ret = false;
        try
        {
            string domainAndUsername = entity.AD_Domain + @"\" + entity.LoginEn.Username;

            DirectoryEntry entry = new DirectoryEntry(entity.AD_Path, domainAndUsername, entity.LoginEn.PWD);

            //Bind to the native AdsObject to force authentication.
            object obj = entry.NativeObject;

            DirectorySearcher search = new DirectorySearcher(entry);

            search.Filter = "(SAMAccountName=" + entity.LoginEn.Username + ")";
            //search.PropertiesToLoad.Add("cn");
            result = search.FindOne();

            if (null == result)
            {
                ret = false;
            }
            else
            {
                ret = true;
            }

            string _path = result.Path;
            string _filterAttribute = (string)result.Properties["cn"][0];

        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ex);
            ret = false;
        }

        return ret;
    }

    /// <summary>
    /// Retrieve LDAP information from database. 
    /// </summary>
    /// <returns></returns>
    public override string GetLdapInfo()
    {
        string ret = "";
        try
        {
            string propertiesStr = "";
            foreach (string name in result.Properties.PropertyNames)
            {
                for (int i = 0; i < result.Properties[name].Count; i++)
                    propertiesStr += name + ": " + result.Properties[name][i] + Environment.NewLine;
            }

            ret = propertiesStr;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ex);
            ret = "";
        }

        return ret;
    }

    /// <summary>
    /// Retrieve the online and offline iPads in the form of counter. 
    /// </summary>
    /// <param name="onOroff"></param>
    /// <returns></returns>
    public override DataTable GetipadOnlineOfflineCount(string onOroff)
    {
        StringBuilder sql = new StringBuilder();
        DataTable ret = new DataTable();
        try
        {
            sql.AppendLine("select count(ipadstatus)as count_ from tblmachine where machinestatus = 1");

            if (onOroff.ToUpper() == "ONLINE")
            {
                sql.AppendLine("and ipadstatus =1");
            }

            if (onOroff.ToUpper() == "OFFLINE")
            {
                sql.AppendLine("and ipadstatus =0");
            }
            ret = SqlDataControl.GetResult(sql.ToString(), new List<Params>());

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


    /// <summary>
    /// Update the lost mode information of a particular iPad machine in the MDM Machine Maintenance page. 
    /// </summary>
    /// <param name="MachineSerial"></param>
    /// <param name="LostModeEnabled"></param>
    /// <returns></returns>
    public override bool UpdateLostMode(string MachineSerial, bool LostModeEnabled)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("update tblMachine set ");

            if (LostModeEnabled)
            {
                sql.AppendLine("LostModeEnabled= 1");
            }
            else
            {
                sql.AppendLine("LostModeEnabled= 0");
            }

            sql.AppendLine("where MachineSerial = @MachineSerial");

            MyParams.Add(new Params("@MachineSerial", "NVARCHAR", MachineSerial));

            ret = SqlDataControl.Input(sql.ToString(), MyParams);
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

    /// <summary>
    /// Update the single app mode information of a particular iPad machine in the MDM Machine Maintenance page. 
    /// </summary>
    /// <param name="MachineSerial"></param>
    /// <param name="singleAppModeEnabled"></param>
    /// <returns></returns>
    public override bool UpdateSingleAppMode(string MachineSerial, bool singleAppModeEnabled)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("update tblMachine set ");

            if (singleAppModeEnabled)
            {
                sql.AppendLine("SingleAppModeEnabled= 1");
            }
            else
            {
                sql.AppendLine("SingleAppModeEnabled= 0");
            }

            sql.AppendLine("where MachineSerial = @MachineSerial");

            MyParams.Add(new Params("@MachineSerial", "NVARCHAR", MachineSerial));

            ret = SqlDataControl.Input(sql.ToString(), MyParams);
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


    /// <summary>
    /// Update the latitude and longitude of the iPad machine when the device is in the lost mode. 
    /// </summary>
    /// <param name="lat"></param>
    /// <param name="lng"></param>
    /// <param name="UDID"></param>
    /// <returns></returns>
    public override bool UpdateLostLatLng(string lat, string lng, string UDID)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("update tblMachine set ");
            sql.AppendLine("LostLatitude=@lat, LostLongitude=@lng");
            sql.AppendLine("where MachineUDID = @UDID");

            MyParams.Add(new Params("@lat", "NVARCHAR", lat));
            MyParams.Add(new Params("@lng", "NVARCHAR", lng));
            MyParams.Add(new Params("@UDID", "NVARCHAR", UDID));



            ret = SqlDataControl.Input(sql.ToString(), MyParams);
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

    /// <summary>
    /// Retrieve the latitude and longitude of the iPad machine when the device is in the lost mode. 
    /// </summary>
    /// <param name="UDID"></param>
    /// <returns></returns>
    public override DataTable GetLostLatLng(string UDID)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select LostLatitude, LostLongitude");
            sql.AppendLine("from tblMachine");
            sql.AppendLine("where MachineUDID = @UDID");

            MyParams.Add(new Params("@UDID", "NVARCHAR", UDID));

            //ret = SqlDataControl.Input(sql.ToString(), MyParams);
            ret = SqlDataControl.GetResult(sql.ToString(), MyParams);

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


    /// <summary>
    /// Retrieve all machine details and information from the database table tblMachine in the form of return value 
    /// of DeviceEn object. 
    /// </summary>
    /// <param name="machineSerial"></param>
    /// <returns></returns>
    public override DeviceEn GetDeviceDetails(string machineSerial)
    {
        DeviceEn ret = new DeviceEn();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("select * ");
            sql.AppendLine("from tblMachine");
            sql.AppendLine("where MachineSerial = @Serial ");
            MyParams.Add(new Params("@Serial", "NVARCHAR", machineSerial));

            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);

            if (dt.Rows.Count == 1)
            {
                ret.DeviceSerial = dt.Rows[0]["MachineSerial"].ToString();
                ret.DeviceName = dt.Rows[0]["MachineName"].ToString();
                ret.ComponentStatus = Convert.ToInt32(dt.Rows[0]["ComponentStatus"]);
                ret.ComponentBattLvl = Convert.ToInt32(dt.Rows[0]["ComponentBattLevel"]);
                ret.ComponentCardReaderStatus = Convert.ToBoolean(dt.Rows[0]["ComponentCardReaderStatus"]);
                ret.ComponentThumbStatus = Convert.ToBoolean(dt.Rows[0]["ComponentThumbStatus"]);
                try
                {
                    ret.LastInitializeTime = Convert.ToDateTime(dt.Rows[0]["MonitorRecDatetime"]);

                }
                catch
                {
                    ret.LastInitializeTime = DateTime.Now;
                }
                try
                {
                    ret.ComponentLastAlertTime = Convert.ToDateTime(dt.Rows[0]["ComponentAlertTime"]);
                }
                catch
                {
                    ret.ComponentLastAlertTime = DateTime.Now;
                }

                //ret.MachineDataSignal = dt.Rows[0]["MachineDataSignal"].ToString();
                // ret.MachineBattLvl = Convert.ToInt32(dt.Rows[0]["MachineBattLevel"]);
                //ret.ComponentBattLvl = Convert.ToInt32(dt.Rows[0]["ComponentBattLevel"]);
                //ret.ComponentCardReaderStatus = Convert.ToBoolean(dt.Rows[0]["ComponentCardReaderStatus"]);
                //ret.ComponentThumbStatus = Convert.ToBoolean(dt.Rows[0]["ComponentThumbStatus"]);
                // ret.MachineLastPing = Convert.ToDateTime(dt.Rows[0]["MachineLastPing"]);
                // ret.ComponentLastAlertTime = Convert.ToDateTime(dt.Rows[0]["ComponentLastAlertTime"]);
                //ret.ResolvedTime = Convert.ToDateTime(dt.Rows[0]["ResolvedTime"]);
                //ret.LastInitializeTime = Convert.ToDateTime(dt.Rows[0]["LastInitializeTime"]);
                // ret.LastMonitoringUpdateTime = Convert.ToDateTime(dt.Rows[0]["LastMonitoringUpdateTime"]);
            }
            else
            {
                ret = null;
            }
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

    // Kenneth added on 2017 May 03
    // return machine Datatable 
    /// <summary>
    /// Retrieve all machine details and information from the database table tblMachine in the form of return value 
    /// of DataTable object. The retrieval parameter is based on serial number of the machine.  
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    public override DataTable GetDeviceDetails_dt(DeviceEn machine)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("select *,");
            sql.AppendLine("(Case when MachineStatus=1 then 'Active'");
            sql.AppendLine("when MachineStatus=0 then 'Non Active' end )");
            sql.AppendLine(" as strStatus");
            sql.AppendLine("from tblMachine");
            sql.AppendLine("where MachineSerial = @Serial ");
            MyParams.Add(new Params("@Serial", "NVARCHAR", machine.DeviceSerial));

            ret = SqlDataControl.GetResult(sql.ToString(), MyParams);


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

    /// <summary>
    /// Get the location history of the machine. 
    /// </summary>
    /// <param name="myParams"></param>
    /// <returns></returns>
    public override DataTable GetLocationHistory(List<Params> myParams)
    {
        DataTable ret = new DataTable();
        StringBuilder sql = new StringBuilder();

        try
        {

            sql.AppendLine(@"select tlh.MachineName, tlh.MachineSerial, concat(Longitude,' , ',latitude ) as Location, CreatedDate from tblLocationHistory tlh");
            sql.AppendLine("where 1=1");
            foreach (Params subParams in myParams)
            {
                if (subParams.dataName.ToUpper() == "@MACHINESERIAL")
                {
                    sql.AppendLine("and machineserial = @machineserial");
                }

                if (subParams.dataName.ToUpper() == "@DATETO")
                {
                    sql.AppendLine("and createddate between @dateto");

                }

                if (subParams.dataName.ToUpper() == "@DATEFROM")
                {
                    sql.AppendLine("and @datefrom");

                }


            }

            sql.AppendLine(" order by CreatedDate desc ");

            ret = SqlDataControl.GetResult(sql.ToString(), myParams);


        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ex);
        }

        finally
        {
            sql.Clear();

        }

        return ret;

    }

    /// <summary>
    /// Get the security information of the machine from database table tblmachine 
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    public override bool UpdateDeviceDetailSecurityInfo(DeviceEn machine)
    {
        bool ret = false;

        try
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"UPDATE tblMachine
                             SET PasscodePresent = @PasscodePresent, 
                                 PasscodeCompliant = @PasscodeCompliant, 
                                 PasscodeCompliantWithProfiles = @PasscodeCompliantWithProfiles
                                 WHERE MachineUDID = @UDID");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@PasscodePresent", "BIT", machine.PasscodePresent));
            MyParams.Add(new Params("@PasscodeCompliant", "BIT", machine.PasscodeCompliant));
            MyParams.Add(new Params("@PasscodeCompliantWithProfiles", "BIT", machine.PasscodeCompliantWithProfiles));
            MyParams.Add(new Params("@UDID", "NVARCHAR", machine.DeviceUDID));

            ret = SqlDataControl.Input(sql.ToString(), MyParams);

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

    /// <summary>
    /// Get the specific machine detail information used to display the inner popup modal of the 
    /// MDM Machine Maintenance page. 
    /// </summary>
    /// <param name="Params"></param>
    /// <returns></returns>
    public override DataTable GetDeviceDetailForDisplay(List<Params> Params)
    {
        DataTable ret = new DataTable();

        try
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"SELECT M.MachineName, M.MachineImei, M.MachineSerial, M.BuildVersion, M.OSVersion,
                                    M.MachineStatus, M.AvailableDevice_Capacity, M.DeviceCapacity, 
                                    M.PasscodePresent, M.PasscodeCompliant, M.PasscodeCompliantWithProfiles, 
                                    M.WifiMAC, M.BluetoothMAC, M.IsRoaming, M.isSupervised,
                                    M.FirmwareVersion, M.FirmwareBatteryStatus, M.FirmwareBatteryLevel, M.FirmwareSerial, 
                                    MD.Latitude, MD.Longitude
                            FROM tblMachine M
                            JOIN User_Branch UB ON UB.bid = M.BranchId
                            LEFT JOIN tblMachineDetails MD ON MD.MachineSerial = M.MachineSerial
                            LEFT JOIN MDM_Enrollment En ON En.UDID = M.MachineUDID
                            WHERE (MachineStatus= 1)  ");


            foreach (Params subparams in Params)
            {
                if (subparams.dataName.ToUpper() == "@MACHINESERIAL")
                {
                    sql.AppendLine("and M.MachineSerial like @machineserial");
                }
            }

            sql.AppendLine("order by M.MachineName, UB.bDesc");
            ret = SqlDataControl.GetResult(sql.ToString(), Params);

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

    /// <summary>
    /// Get the machine certificate information from the database by joining the results of two tables. 
    /// </summary>
    /// <param name="cert"></param>
    /// <returns></returns>
    public override DataTable GetDeviceDetailCerts(DeviceCertificateEn cert)
    {
        DataTable ret = new DataTable();

        try
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"SELECT UDID, CertName, IsIdentityCert
                                FROM tblMachine A
                                JOIN MDM_Certificates B ON A.MachineUDID =  B.UDID
                                WHERE B.UDID=@machineUDID
                                ");

            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineUDID", "NVARCHAR", cert.UDID));

            ret = SqlDataControl.GetResult(sql.ToString(), myParams);

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

    /// <summary>
    /// Retrieve the list of installed third applications of a particular machine by joining
    /// the results of two tables. 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public override DataTable GetDeviceDetailInstalledApps(InstalledAppEn app)
    {
        DataTable ret = new DataTable();

        try
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"SELECT *
                             FROM tblMachine A
                             JOIN MDM_InstalledApps B ON A.MachineUDID =  B.UDID
                             WHERE B.UDID=@machineUDID");

            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineUDID", "NVARCHAR", app.UDID));

            ret = SqlDataControl.GetResult(sql.ToString(), myParams);

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

    /// <summary>
    /// Retrieve the list of available operating system updates of a particular machine. 
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public override DataTable GetDeviceDetailOSUpdates(OSUpdateEn update)
    {
        DataTable ret = new DataTable();

        try
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"SELECT *
                             FROM tblMachine A
                             JOIN MDM_OS_Updates B ON A.MachineUDID =  B.UDID
                             WHERE B.UDID=@machineUDID");

            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineUDID", "NVARCHAR", update.UDID));

            ret = SqlDataControl.GetResult(sql.ToString(), myParams);

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

    /// <summary>
    /// Get the machine certificate information from the database.  
    /// </summary>
    /// <param name="cert"></param>
    /// <returns></returns>
    public override DataTable GetDeviceCerts(DeviceCertificateEn cert)
    {
        DataTable ret = new DataTable();

        try
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"SELECT *
                             FROM MDM_Certificates
                             WHERE UDID=@machineUDID");

            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineUDID", "NVARCHAR", cert.UDID));

            ret = SqlDataControl.GetResult(sql.ToString(), myParams);

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

    /// <summary>
    /// Get the machine application information from the database.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public override DataTable GetDeviceApps(InstalledAppEn app)
    {
        DataTable ret = new DataTable();

        try
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"SELECT *
                             FROM MDM_InstalledApps
                             WHERE UDID=@machineUDID");

            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineUDID", "NVARCHAR", app.UDID));

            ret = SqlDataControl.GetResult(sql.ToString(), myParams);

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


    /// <summary>
    /// Retrieve the particular machine's pending OS Updates from database table. 
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public override DataTable GetDevicePendingUpdates(OSUpdateEn update)
    {
        DataTable ret = new DataTable();

        try
        {
            StringBuilder sql = new StringBuilder();

            // check for pending updates only. 
            sql.AppendLine(@"SELECT *
                             FROM MDM_Pending_OS_Updates
                             WHERE UDID=@machineUDID AND UpdateInstalled=0
                             ORDER BY RequestTime DESC");

            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineUDID", "NVARCHAR", update.UDID));


            ret = SqlDataControl.GetResult(sql.ToString(), myParams);

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


    /// <summary>
    /// Retrieve all the enrolled, active machines that are checked in the MDM server. 
    /// </summary>
    /// <param name="Params"></param>
    /// <returns></returns>
    public override DataTable GetDeviceDetails_All_Active(List<Params> Params)
    {
        DataTable ret = new DataTable();

        try
        {
            StringBuilder sql = new StringBuilder();
            //sql.AppendLine("select * ,");
            //sql.AppendLine("(Case when MachineStatus=1 then 'Active'");
            //sql.AppendLine("when MachineStatus=0 then 'Non Active' end )");
            //sql.AppendLine(" as strStatus");
            //sql.AppendLine("from tblMachine");
            //sql.AppendLine("where MachineStatus= 1 ");
            //            sql.AppendLine(@"select * , ub.bDesc,
            //(Case when MachineStatus=1 then 'Active'
            //when MachineStatus=0 then 'Non Active' end )
            // as strStatus
            // from tblMachine 
            // join User_branch ub
            // on BranchId = ub.bID
            //where MachineStatus= 1 ");

            sql.AppendLine(@"SELECT *, B.bDesc,CONCAT(ISNULL((C.Latitude) + ' , ',''), (C.Longitude)) AS Location,
                                (CASE WHEN MachineStatus=1 THEN 'Active' WHEN MachineStatus=0 THEN 'Non Active' END ) AS strStatus,
                                (CASE WHEN IpadStatus=1 THEN 'Active' WHEN IpadStatus=0 THEN 'Non Active' END ) AS IStatus , 
                                (CASE WHEN AppStatus=1 THEN 'Online' WHEN AppStatus=0 THEN 'Offline' END ) AS AStatus, 
                                (CASE WHEN Componentstatus=1 THEN 'Online' WHEN Componentstatus=0 THEN 'Alert' END ) AS Cstatus,
                                (CASE WHEN ComponentCardReaderStatus=1 THEN 'Online' WHEN ComponentCardReaderStatus=0 THEN 'Offline' END ) AS ccrdStatus,
                                (CASE WHEN ComponentThumbStatus=1 THEN 'Online' WHEN ComponentThumbStatus=0 THEN 'Offline' END ) AS tStatus, iPadBattLevel as IBLevel,
                                (CASE WHEN ComponentBattLevel IS NULL THEN '' ELSE (Convert(varchar(10),ComponentBattLevel) + ' %') END) AS CBLevel
                            FROM tblMachine A
                            JOIN User_Branch B ON BranchId =  B.bid
                            LEFT JOIN tblMachineDetails C ON A.MachineSerial = C.MachineSerial
                            LEFT JOIN MDM_Enrollment D ON A.MachineUDID = D.UDID
                            WHERE (MachineStatus= 1) AND (ipadStatus= 1) ");

            foreach (Params subparams in Params)
            {
                if (subparams.dataName.ToUpper() == "@MACHINESERIAL")
                {
                    sql.AppendLine("and A.machineserial like @machineserial");
                }

                if (subparams.dataName.ToUpper() == "@BRANCHID")
                {
                    sql.AppendLine("and A.BranchID=@branchid");
                }

                if (subparams.dataName.ToUpper() == "@MACHINENAME")
                {
                    sql.AppendLine("and Machinename like @machinename");
                }

                if (subparams.dataName.ToUpper() == "@BID")
                {
                    sql.AppendLine("and B.bID like @bID");
                }

                if (subparams.dataName.ToUpper() == "@BDESC")
                {
                    sql.AppendLine("and B.bDesc like @bdesc");
                }

            }

            sql.AppendLine("order by MachineName,B.bDesc");
            ret = SqlDataControl.GetResult(sql.ToString(), Params);


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

    /// <summary>
    /// Retrieve the list of branches that have profiles assigned to them. 
    /// </summary>
    /// <param name="Params"></param>
    /// <returns></returns>
    // BRYAN 
    public override DataTable GetBranchesWithProfiles(List<Params> Params)
    {

        DataTable ret = new DataTable();

        try
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"SELECT B.bID, B.bDesc, E.Profile_ID,
                                (CASE WHEN MachineStatus=1 THEN 'Active' WHEN MachineStatus=0 THEN 'Non Active' END ) AS strStatus,
                                (CASE WHEN IpadStatus=1 THEN 'Active' WHEN IpadStatus=0 THEN 'Non Active' END ) AS IStatus , 
                                (CASE WHEN AppStatus=1 THEN 'Online' WHEN AppStatus=0 THEN 'Offline' END ) AS AStatus, 
                                (CASE WHEN Componentstatus=1 THEN 'Online' WHEN Componentstatus=0 THEN 'Alert' END ) AS Cstatus,
                                (CASE WHEN ComponentCardReaderStatus=1 THEN 'Online' WHEN ComponentCardReaderStatus=0 THEN 'Offline' END ) AS ccrdStatus,
                                (CASE WHEN ComponentThumbStatus=1 THEN 'Online' WHEN ComponentThumbStatus=0 THEN 'Offline' END ) AS tStatus, iPadBattLevel as IBLevel,
                                (CASE WHEN ComponentBattLevel IS NULL THEN '' ELSE (Convert(varchar(10),ComponentBattLevel) + ' %') END) AS CBLevel
                            FROM tblMachine A
                            JOIN User_Branch B ON BranchId =  B.bid
                            LEFT JOIN tblMachineDetails C ON A.MachineSerial = C.MachineSerial
                            LEFT JOIN MDM_Enrollment D ON A.MachineUDID = D.UDID
                            LEFT JOIN MDM_Profile_General_BranchID E ON B.bid = E.Branch_ID
                            LEFT JOIN MDM_Profile_General F ON F.Profile_ID = E.Profile_ID

                            WHERE (MachineStatus= 1 OR MachineStatus = 0) AND pStatus IN (1,5)");


            sql.AppendLine("order by B.bDesc");
            ret = SqlDataControl.GetResult(sql.ToString(), Params);
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


    /// <summary>
    /// Get the machine details information based on the parameter UDID. The return value 
    /// is in the form of DataTable. 
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    public override DataTable GetDeviceDetails_UDID_dt(DeviceEn machine)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("select * ");
            sql.AppendLine("from tblMachine");
            sql.AppendLine("where MachineUDID = @MachineUDID ");
            MyParams.Add(new Params("@MachineUDID", "NVARCHAR", machine.DeviceUDID));


            ret = SqlDataControl.GetResult(sql.ToString(), MyParams);


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

    /// <summary>
    /// Insert all machine details and information into the database. 
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    public override bool InsertDevice(DeviceEn machine)
    {
        bool ret = false;
        StringBuilder sql = new StringBuilder();
        List<Params> MyParams = new List<Params>();
        try
        {
            sql.AppendLine("INSERT INTO ");
            sql.AppendLine("tblMachine");
            sql.AppendLine(@"([MachineImei]
                          ,[MachineSerial]
                          ,[MachineUDID]
                          ,[MachineName]
                          ,[BranchId]
                          ,[MachineStatus]
                          ,[OSVersion]
                          ,[DeviceCapacity]
                          ,[AvailableDevice_Capacity]
                          ,[IsSupervised]
                          ,[LostModeEnabled]
                          ,[SingleAppModeEnabled]
                          ,[BuildVersion]
                          ,[WifiMAC]
                          ,[BluetoothMAC]
                          ,[IsRoaming]
                           )");
            sql.AppendLine("values");
            sql.AppendLine(@"(     @MachineImei
                                  ,@MachineSerial
                                  ,@MachineUDID
                                  ,@MachineName
                                  ,@BranchId
                                  ,@MachineStatus
                                  ,@OSVersion
                                  ,@DeviceCapacity
                                  ,@AvailableDevice_Capacity
                                  ,@IsSupervised
                                  ,@LostModeEnabled
                                  ,@SingleAppModeEnabled
                                  ,@BuildVersion
                                  ,@WifiMAC
                                  ,@BluetoothMAC
                                  ,@IsRoaming
                                  )");


            MyParams.Add(new Params("@MachineImei", "NVARCHAR", machine.DeviceImei));
            MyParams.Add(new Params("@MachineSerial", "NVARCHAR", machine.DeviceSerial));
            MyParams.Add(new Params("@MachineUDID", "NVARCHAR", machine.DeviceUDID));
            MyParams.Add(new Params("@MachineName", "NVARCHAR", machine.DeviceName));
            MyParams.Add(new Params("@BranchId", "GUID", machine.BranchId));
            MyParams.Add(new Params("@MachineStatus", "INT", machine.DeviceStatus));
            MyParams.Add(new Params("@OSVersion", "NVARCHAR", machine.OsVersion));
            MyParams.Add(new Params("@DeviceCapacity", "NVARCHAR", machine.DeviceCapacity));
            MyParams.Add(new Params("@AvailableDevice_Capacity", "NVARCHAR", machine.AvailableDevice_Capacity));
            MyParams.Add(new Params("@IsSupervised", "BIT", machine.IsSupervised));
            MyParams.Add(new Params("@LostModeEnabled", "BIT", machine.LostModeEnabled));
            MyParams.Add(new Params("@SingleAppModeEnabled", "BIT", machine.SingleAppModeEnabled));
            MyParams.Add(new Params("@BuildVersion", "NVARCHAR", machine.BuildVersion));
            MyParams.Add(new Params("@WifiMAC", "NVARCHAR", machine.WifiMAC));
            MyParams.Add(new Params("@BluetoothMAC", "NVARCHAR", machine.BluetoothMAC));
            MyParams.Add(new Params("@IsRoaming", "BIT", machine.IsRoaming));


            ret = SqlDataControl.Input(sql.ToString(), MyParams);

        }
        catch (Exception ex)
        {
            ret = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                             ex.ToString());
        }

        return ret;
    }

    /// <summary>
    /// Insert the machine cert entities of a particular machine based on the UDID. 
    /// </summary>
    /// <param name="cert"></param>
    /// <returns></returns>
    public override bool InsertDeviceCerts(DeviceCertificateEn cert)
    {
        bool ret = false;
        StringBuilder sql = new StringBuilder();
        List<Params> MyParams = new List<Params>();
        try
        {
            sql.AppendLine("INSERT INTO ");
            sql.AppendLine("MDM_Certificates");
            sql.AppendLine(@"([ID]
                          ,[UDID]
                          ,[CertName]
                          ,[IsIdentityCert]
                           )");

            sql.AppendLine("values");
            sql.AppendLine(@"(     @ID
                                  ,@UDID
                                  ,@CertName
                                  ,@IsIdentityCert
                                  )");


            MyParams.Add(new Params("@ID", "GUID", cert.ID));
            MyParams.Add(new Params("@UDID", "NVARCHAR", cert.UDID));
            MyParams.Add(new Params("@CertName", "NVARCHAR", cert.CertName));
            MyParams.Add(new Params("@IsIdentityCert", "BIT", cert.IsIdentityCert));

            ret = SqlDataControl.Input(sql.ToString(), MyParams);

        }
        catch (Exception ex)
        {
            ret = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                             ex.ToString());
        }

        return ret;
    }

    /// <summary>
    /// Insert the installed application entities of a particular machine based on the UDID. 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public override bool InsertDeviceApps(InstalledAppEn app)
    {
        bool ret = false;
        StringBuilder sql = new StringBuilder();
        List<Params> MyParams = new List<Params>();
        try
        {
            sql.AppendLine("INSERT INTO ");
            sql.AppendLine("MDM_InstalledApps");
            sql.AppendLine(@"([ID]
                          ,[UDID]
                          ,[AppName]
                          ,[Version]
                          ,[Identifier]
                           )");

            sql.AppendLine("values");
            sql.AppendLine(@"(     @ID
                                  ,@UDID
                                  ,@AppName
                                  ,@Version
                                  ,@Identifier
                                  )");


            MyParams.Add(new Params("@ID", "GUID", app.ID));
            MyParams.Add(new Params("@UDID", "NVARCHAR", app.UDID));
            MyParams.Add(new Params("@AppName", "NVARCHAR", app.AppName));
            MyParams.Add(new Params("@Version", "NVARCHAR", app.Version));
            MyParams.Add(new Params("@Identifier", "NVARCHAR", app.Identifier));

            ret = SqlDataControl.Input(sql.ToString(), MyParams);

        }
        catch (Exception ex)
        {
            ret = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                             ex.ToString());
        }

        return ret;
    }

    /// <summary>
    /// Insert the available machine OS updates entities of a particular machine. 
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public override bool InsertDeviceOSUpdates(OSUpdateEn update)
    {
        bool ret = false;
        StringBuilder sql = new StringBuilder();
        List<Params> MyParams = new List<Params>();
        try
        {
            sql.AppendLine("INSERT INTO ");
            sql.AppendLine("MDM_OS_Updates");
            sql.AppendLine(@"([ID]
                          ,[UDID]
                          ,[HumanReadableName]
                          ,[ProductKey]
                          ,[ProductVersion]
                          ,[RestartRequired]
                           )");

            sql.AppendLine("values");
            sql.AppendLine(@"(     @ID
                                  ,@UDID
                                  ,@HumanReadableName
                                  ,@ProductKey
                                  ,@ProductVersion
                                  ,@RestartRequired
                                  )");

            MyParams.Add(new Params("@ID", "GUID", update.ID));
            MyParams.Add(new Params("@UDID", "NVARCHAR", update.UDID));
            MyParams.Add(new Params("@HumanReadableName", "NVARCHAR", update.HumanReadableName));
            MyParams.Add(new Params("@ProductKey", "NVARCHAR", update.ProductKey));
            MyParams.Add(new Params("@ProductVersion", "NVARCHAR", update.ProductVersion));
            MyParams.Add(new Params("@RestartRequired", "BIT", update.RestartRequired));


            ret = SqlDataControl.Input(sql.ToString(), MyParams);

        }
        catch (Exception ex)
        {
            ret = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                             ex.ToString());
        }

        return ret;
    }

    /// <summary>
    /// Delete all certificate entries of a particular ipad machine. 
    /// </summary>
    /// <param name="cert"></param>
    /// <returns></returns>
    public override bool DeleteDeviceCerts(DeviceCertificateEn cert)
    {
        bool ret = false;
        StringBuilder sql = new StringBuilder();
        List<Params> MyParams = new List<Params>();
        try
        {
            sql.AppendLine(@"DELETE FROM MDM_Certificates 
                             WHERE UDID=@UDID");

            MyParams.Add(new Params("@UDID", "NVARCHAR", cert.UDID));


            ret = SqlDataControl.Input(sql.ToString(), MyParams);

        }
        catch (Exception ex)
        {
            ret = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                             ex.ToString());
        }

        return ret;
    }

    /// <summary>
    /// Delete all installed application entries of a particular ipad machine. 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public override bool DeleteDeviceApps(InstalledAppEn app)
    {
        bool ret = false;
        StringBuilder sql = new StringBuilder();
        List<Params> MyParams = new List<Params>();
        try
        {
            sql.AppendLine(@"DELETE FROM MDM_InstalledApps 
                             WHERE UDID=@UDID");

            MyParams.Add(new Params("@UDID", "NVARCHAR", app.UDID));

            ret = SqlDataControl.Input(sql.ToString(), MyParams);

        }
        catch (Exception ex)
        {
            ret = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                             ex.ToString());
        }

        return ret;
    }

    /// <summary>
    /// Delete all machine OS update entries of a particular ipad machine. 
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public override bool DeleteDeviceOSUpdates(OSUpdateEn update)
    {
        bool ret = false;
        StringBuilder sql = new StringBuilder();
        List<Params> MyParams = new List<Params>();
        try
        {
            sql.AppendLine(@"DELETE FROM MDM_OS_Updates 
                             WHERE UDID=@UDID");

            MyParams.Add(new Params("@UDID", "NVARCHAR", update.UDID));

            ret = SqlDataControl.Input(sql.ToString(), MyParams);

        }
        catch (Exception ex)
        {
            ret = false;
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                             ex.ToString());
        }

        return ret;
    }


    // Kenneth added on 2017 May 03
    // standard all update function

    /// <summary>
    /// Update machine information based on machine serial number parameter. 
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    public override bool UpdateDevice_ws(DeviceEn machine)
    {
        bool ret = false;
        StringBuilder sql = new StringBuilder();
        List<Params> myParams = new List<Params>();
        try
        {

            //if (!string.IsNullOrEmpty(machine.IPadBattLevel))
            //{
            //    string[] a = machine.IPadBattLevel.Split('.');
            //    machine.IPadBattLevel = a[1].Substring(0, 2).ToString();
            //}

            DeviceEn my_Machine = new DeviceEn();
            my_Machine.DeviceSerial = machine.DeviceSerial;
            DataTable dt = GetDeviceDetails_dt(my_Machine);

            List<DeviceEn> my_Machine_List = dt.AsEnumerable().Select(row => new DeviceEn
            {
                DeviceImei = (machine.DeviceImei != null ? machine.DeviceImei : row.Field<string>("MachineImei")),
                DeviceSerial = (machine.DeviceSerial != null ? machine.DeviceSerial : row.Field<string>("MachineSerial")),
                DeviceUDID = (machine.DeviceUDID != null ? machine.DeviceUDID : row.Field<string>("MachineUDID")),
                DeviceName = (machine.DeviceName != null ? machine.DeviceName : row.Field<string>("MachineName")),
                BranchId = (machine.BranchId != Guid.Empty ? machine.BranchId : row.Field<Guid>("BranchId")),
                DeviceStatus = (machine.DeviceStatus != null ? machine.DeviceStatus : row.Field<int>("MachineStatus")),
                iPadStatus = (machine.iPadStatus != null ? machine.iPadStatus : row.Field<int>("IpadStatus")),
                appStatus = (machine.appStatus != null ? machine.appStatus : row.Field<int>("appStatus")),
                ComponentStatus = (machine.ComponentStatus != null ? machine.ComponentStatus : row.Field<int>("componentStatus")),

                MonitoringPushDatetime = ((machine.MonitoringPushDatetime == DateTime.MinValue || machine.MonitoringPushDatetime == null) ? row.Field<DateTime?>("MonitorPushDatetime") : machine.MonitoringPushDatetime),
                MonitoringRecDatetime = ((machine.MonitoringRecDatetime == DateTime.MinValue || machine.MonitoringRecDatetime == null) ? row.Field<DateTime?>("MonitorRecDatetime") : machine.MonitoringRecDatetime),
                ApnPushDatetime = ((machine.ApnPushDatetime == DateTime.MinValue || machine.ApnPushDatetime == null) ? row.Field<DateTime?>("ApnPushDatetime") : machine.ApnPushDatetime),
                ApnRecDatetime = ((machine.ApnRecDatetime == DateTime.MinValue || machine.ApnRecDatetime == null) ? row.Field<DateTime?>("ApnRecDatetime") : machine.ApnRecDatetime),
                ResolvedTime = ((machine.ResolvedTime == DateTime.MinValue || machine.ResolvedTime == null) ? row.Field<DateTime?>("ResolvedTime") : machine.ResolvedTime),
                ComponentAlertTime = ((machine.ComponentAlertTime == DateTime.MinValue || machine.ComponentAlertTime == null) ? row.Field<DateTime?>("ComponentAlertTime") : machine.ComponentAlertTime),
                ComponentLastAlertTime = ((machine.ComponentLastAlertTime == DateTime.MinValue || machine.ComponentLastAlertTime == null) ? row.Field<DateTime?>("ComponentLastAlertTime") : machine.ComponentLastAlertTime),
                LastInitializeTime = ((machine.LastInitializeTime == DateTime.MinValue || machine.LastInitializeTime == null) ? row.Field<DateTime?>("LastInitializeTime") : machine.LastInitializeTime),

                DeviceDataSignal = (machine.DeviceDataSignal != null ? machine.DeviceDataSignal : row.Field<string>("MachineDataSignal")),
                IPadBattLevel = (machine.IPadBattLevel != null ? machine.IPadBattLevel : row.Field<string>("iPadBattLevel")),
                ComponentBattLvl = (machine.ComponentBattLvl != null ? machine.ComponentBattLvl : row.Field<int>("ComponentBattLevel")),
                ComponentCardReaderStatus = (machine.ComponentCardReaderStatus != null ? machine.ComponentCardReaderStatus : row.Field<bool>("ComponentCardReaderStatus")),
                ComponentThumbStatus = (machine.ComponentThumbStatus != null ? machine.ComponentThumbStatus : row.Field<bool>("ComponentThumbStatus")),
                MsfAppID = (machine.MsfAppID != null ? machine.MsfAppID : row.Field<string>("MsfAppID")),
                OsVersion = (machine.OsVersion != null ? machine.OsVersion : row.Field<string>("OSVersion")),
                DeviceCapacity = (machine.DeviceCapacity != null ? machine.DeviceCapacity : row.Field<string>("DeviceCapacity")),
                AvailableDevice_Capacity = (machine.AvailableDevice_Capacity != null ? machine.AvailableDevice_Capacity : row.Field<string>("AvailableDevice_Capacity")),
                IsSupervised = (machine.IsSupervised != null ? machine.IsSupervised : row.Field<bool>("IsSupervised")),
                SingleAppModeEnabled = (machine.SingleAppModeEnabled != null ? machine.SingleAppModeEnabled : row.Field<bool>("SingleAppModeEnabled")),
                BuildVersion = (machine.BuildVersion != null ? machine.BuildVersion : row.Field<string>("BuildVersion")),
                WifiMAC = (machine.WifiMAC != null ? machine.WifiMAC : row.Field<string>("WifiMAC")),
                BluetoothMAC = (machine.BluetoothMAC != null ? machine.BluetoothMAC : row.Field<string>("BluetoothMAC")),
                IsRoaming = (machine.IsRoaming != null ? machine.IsRoaming : row.Field<bool>("IsRoaming"))

            }).ToList();



            //            List<t> target = dt.AsEnumerable()
            //.Select(row => new T
            //{
            //     // assuming column 0's type is Nullable<long>
            //     ID = row.Field<long?>(0).GetValueOrDefault()
            //     Name = String.IsNullOrEmpty(row.Field<string>(1))
            //     ? "not found"
            //     : row.Field<string>(1)
            //})
            //.ToList();


            sql.AppendLine("Update tblMachine");
            sql.AppendLine(@"set MachineImei=@MachineImei
                              ,MachineUDID=@MachineUDID
                              ,MachineName=@MachineName
                              ,BranchId=@BranchId
                              ,MachineStatus=@MachineStatus
                              ,iPadStatus=@iPadStatus
                              ,appStatus=@appStatus
                              ,componentStatus=@componentStatus
                              ,MonitorPushDatetime=@MonitorPushDatetime
                              ,MonitorRecDatetime=@MonitorRecDatetime
                              ,ApnPushDatetime=@ApnPushDatetime
                              ,ApnRecDatetime=@ApnRecDatetime
                              ,ResolvedTime=@ResolvedTime
                              ,ComponentAlertTime=@ComponentAlertTime
                              ,ComponentLastAlertTime=@ComponentLastAlertTime
                              ,LastInitializeTime=@LastInitializeTime
                              ,MachineDataSignal=@MachineDataSignal
                              ,iPadBattLevel=@iPadBattLevel
                              ,ComponentBattLevel=@ComponentBattLevel
                              ,ComponentCardReaderStatus=@ComponentCardReaderStatus
                              ,ComponentThumbStatus=@ComponentThumbStatus
                              ,MsfAppID=@MsfAppID
                              ,OSVersion=@OSVersion
                              ,DeviceCapacity=@DeviceCapacity
                              ,AvailableDevice_Capacity=@AvailableDevice_Capacity
                              ,IsSupervised=@IsSupervised
                              ,BuildVersion=@BuildVersion 
                              ,WifiMAC=@WifiMAC
                              ,BluetoothMAC=@BluetoothMAC
                              ,IsRoaming=@IsRoaming
                              ,SingleAppModeEnabled=@SingleAppModeEnabled
                             
    ");

            sql.AppendLine("WHERE MachineSerial=@MachineSerial");

            myParams.Add(new Params("@MachineSerial", "NVARCHAR", my_Machine_List.First().DeviceSerial));
            myParams.Add(new Params("@MachineImei", "NVARCHAR", my_Machine_List.First().DeviceImei));
            myParams.Add(new Params("@MachineUDID", "NVARCHAR", my_Machine_List.First().DeviceUDID));
            myParams.Add(new Params("@MachineName", "NVARCHAR", my_Machine_List.First().DeviceName));
            myParams.Add(new Params("@BranchId", "GUID", my_Machine_List.First().BranchId));
            myParams.Add(new Params("@MachineStatus", "INT", my_Machine_List.First().DeviceStatus));
            myParams.Add(new Params("@iPadStatus", "INT", my_Machine_List.First().iPadStatus));
            myParams.Add(new Params("@appStatus", "INT", my_Machine_List.First().appStatus));
            myParams.Add(new Params("@componentStatus", "INT", my_Machine_List.First().ComponentStatus));
            myParams.Add(new Params("@MonitorPushDatetime", "DATETIME", my_Machine_List.First().MonitoringPushDatetime));
            myParams.Add(new Params("@MonitorRecDatetime", "DATETIME", my_Machine_List.First().MonitoringRecDatetime));
            myParams.Add(new Params("@ApnPushDatetime", "DATETIME", my_Machine_List.First().ApnPushDatetime));
            myParams.Add(new Params("@ApnRecDatetime", "DATETIME", my_Machine_List.First().ApnRecDatetime));
            myParams.Add(new Params("@ResolvedTime", "DATETIME", my_Machine_List.First().ResolvedTime));
            myParams.Add(new Params("@ComponentAlertTime", "DATETIME", my_Machine_List.First().ComponentAlertTime));
            myParams.Add(new Params("@ComponentLastAlertTime", "DATETIME", my_Machine_List.First().ComponentLastAlertTime));
            myParams.Add(new Params("@LastInitializeTime", "DATETIME", my_Machine_List.First().LastInitializeTime));
            myParams.Add(new Params("@MachineDataSignal", "NVARCHAR", my_Machine_List.First().DeviceDataSignal));
            myParams.Add(new Params("@iPadBattLevel", "NVARCHAR", my_Machine_List.First().IPadBattLevel));
            myParams.Add(new Params("@ComponentBattLevel", "INT", my_Machine_List.First().ComponentBattLvl));
            myParams.Add(new Params("@ComponentCardReaderStatus", "BIT", my_Machine_List.First().ComponentCardReaderStatus));
            myParams.Add(new Params("@ComponentThumbStatus", "BIT", my_Machine_List.First().ComponentThumbStatus));
            myParams.Add(new Params("@MsfAppID", "NVARCHAR", my_Machine_List.First().MsfAppID));
            myParams.Add(new Params("@OSVersion", "NVARCHAR", my_Machine_List.First().OsVersion));
            myParams.Add(new Params("@DeviceCapacity", "NVARCHAR", my_Machine_List.First().DeviceCapacity));
            myParams.Add(new Params("@AvailableDevice_Capacity", "NVARCHAR", my_Machine_List.First().AvailableDevice_Capacity));
            myParams.Add(new Params("@IsSupervised", "BIT", my_Machine_List.First().IsSupervised));
            myParams.Add(new Params("@SingleAppModeEnabled", "BIT", my_Machine_List.First().SingleAppModeEnabled));
            myParams.Add(new Params("@BuildVersion", "NVARCHAR", my_Machine_List.First().BuildVersion));
            myParams.Add(new Params("@WifiMAC", "NVARCHAR", my_Machine_List.First().WifiMAC));
            myParams.Add(new Params("@BluetoothMAC", "NVARCHAR", my_Machine_List.First().BluetoothMAC));
            myParams.Add(new Params("@IsRoaming", "BIT", my_Machine_List.First().IsRoaming));


            ret = SqlDataControl.Input(sql.ToString(), myParams);


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

    /// <summary>
    /// Update machine information based on UDID parameter. 
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    public override bool UpdateDevice_ws_byUDID(DeviceEn machine)
    {
        bool ret = false;
        StringBuilder sql = new StringBuilder();
        List<Params> myParams = new List<Params>();
        try
        {
            DeviceEn my_Machine = new DeviceEn();
            my_Machine.DeviceUDID = machine.DeviceUDID;

            DataTable dt = GetDeviceDetails_UDID_dt(my_Machine);

            List<DeviceEn> my_Machine_List = dt.AsEnumerable().Select(row => new DeviceEn
            {
                //MachineImei = (machine.MachineImei != null ? machine.MachineImei : row.Field<string>("MachineImei")),
                DeviceSerial = (machine.DeviceSerial != null ? machine.DeviceSerial : row.Field<string>("MachineSerial")),
                DeviceUDID = (machine.DeviceUDID != null ? machine.DeviceUDID : row.Field<string>("MachineUDID")),
                DeviceName = (machine.DeviceName != null ? machine.DeviceName : row.Field<string>("MachineName")),
                BranchId = (machine.BranchId != Guid.Empty ? machine.BranchId : row.Field<Guid>("BranchId")),
                DeviceStatus = (machine.DeviceStatus != null ? machine.DeviceStatus : row.Field<int>("MachineStatus")),
                iPadStatus = (machine.iPadStatus != null ? machine.iPadStatus : row.Field<int>("IpadStatus")),
                appStatus = (machine.appStatus != null ? machine.appStatus : row.Field<int>("appStatus")),
                ComponentStatus = (machine.ComponentStatus != null ? machine.ComponentStatus : row.Field<int>("componentStatus")),

                MonitoringPushDatetime = ((machine.MonitoringPushDatetime == DateTime.MinValue || machine.MonitoringPushDatetime == null) ? row.Field<DateTime?>("MonitorPushDatetime") : machine.MonitoringPushDatetime),
                MonitoringRecDatetime = ((machine.MonitoringRecDatetime == DateTime.MinValue || machine.MonitoringRecDatetime == null) ? row.Field<DateTime?>("MonitorRecDatetime") : machine.MonitoringRecDatetime),
                ApnPushDatetime = ((machine.ApnPushDatetime == DateTime.MinValue || machine.ApnPushDatetime == null) ? row.Field<DateTime?>("ApnPushDatetime") : machine.ApnPushDatetime),
                ApnRecDatetime = ((machine.ApnRecDatetime == DateTime.MinValue || machine.ApnRecDatetime == null) ? row.Field<DateTime?>("ApnRecDatetime") : machine.ApnRecDatetime),
                ResolvedTime = ((machine.ResolvedTime == DateTime.MinValue || machine.ResolvedTime == null) ? row.Field<DateTime?>("ResolvedTime") : machine.ResolvedTime),
                ComponentAlertTime = ((machine.ComponentAlertTime == DateTime.MinValue || machine.ComponentAlertTime == null) ? row.Field<DateTime?>("ComponentAlertTime") : machine.ComponentAlertTime),
                ComponentLastAlertTime = ((machine.ComponentLastAlertTime == DateTime.MinValue || machine.ComponentLastAlertTime == null) ? row.Field<DateTime?>("ComponentLastAlertTime") : machine.ComponentLastAlertTime),
                LastInitializeTime = ((machine.LastInitializeTime == DateTime.MinValue || machine.LastInitializeTime == null) ? row.Field<DateTime?>("LastInitializeTime") : machine.LastInitializeTime),

                DeviceDataSignal = (machine.DeviceDataSignal != null ? machine.DeviceDataSignal : row.Field<string>("MachineDataSignal")),
                IPadBattLevel = (machine.IPadBattLevel != null ? machine.IPadBattLevel : row.Field<string>("iPadBattLevel")),
                ComponentBattLvl = (machine.ComponentBattLvl != null ? machine.ComponentBattLvl : row.Field<int>("ComponentBattLevel")),
                ComponentCardReaderStatus = (machine.ComponentCardReaderStatus != null ? machine.ComponentCardReaderStatus : row.Field<bool>("ComponentCardReaderStatus")),
                ComponentThumbStatus = (machine.ComponentThumbStatus != null ? machine.ComponentThumbStatus : row.Field<bool>("ComponentThumbStatus")),
                MsfAppID = (machine.MsfAppID != null ? machine.MsfAppID : row.Field<string>("MsfAppID")),
                OsVersion = (machine.OsVersion != null ? machine.OsVersion : row.Field<string>("OSVersion")),
                DeviceCapacity = (machine.DeviceCapacity != null ? machine.DeviceCapacity : row.Field<string>("DeviceCapacity")),
                AvailableDevice_Capacity = (machine.AvailableDevice_Capacity != null ? machine.AvailableDevice_Capacity : row.Field<string>("AvailableDevice_Capacity")),
                EraseDateTime = (machine.EraseDateTime != null ? machine.EraseDateTime : row.Field<DateTime>("EraseDateTime"))
            }).ToList();


            //            List<t> target = dt.AsEnumerable()
            //.Select(row => new T
            //{
            //     // assuming column 0's type is Nullable<long>
            //     ID = row.Field<long?>(0).GetValueOrDefault()
            //     Name = String.IsNullOrEmpty(row.Field<string>(1))
            //     ? "not found"
            //     : row.Field<string>(1)
            //})
            //.ToList();


            sql.AppendLine("Update tblMachine");
            sql.AppendLine(@"set
                               MachineSerial=@MachineSerial  
                          
                              ,MachineUDID=@MachineUDID
                              ,MachineName=@MachineName
                              ,BranchId=@BranchId
                              ,MachineStatus=@MachineStatus
                              ,iPadStatus=@iPadStatus
                              ,appStatus=@appStatus
                              ,componentStatus=@componentStatus
                              ,MonitorPushDatetime=@MonitorPushDatetime
                              ,MonitorRecDatetime=@MonitorRecDatetime
                              ,ApnPushDatetime=@ApnPushDatetime
                              ,ApnRecDatetime=@ApnRecDatetime
                              ,ResolvedTime=@ResolvedTime
                              ,ComponentAlertTime=@ComponentAlertTime
                              ,ComponentLastAlertTime=@ComponentLastAlertTime
                              ,LastInitializeTime=@LastInitializeTime
                              ,MachineDataSignal=@MachineDataSignal
                              ,iPadBattLevel=@iPadBattLevel
                              ,ComponentBattLevel=@ComponentBattLevel
                              ,ComponentCardReaderStatus=@ComponentCardReaderStatus
                              ,ComponentThumbStatus=@ComponentThumbStatus
                              ,MsfAppID=@MsfAppID
                              ,OSVersion=@OSVersion
                              ,DeviceCapacity=@DeviceCapacity
                              ,AvailableDevice_Capacity=@AvailableDevice_Capacity
                              , EraseDateTime=@EraseDateTime");
            sql.AppendLine("WHERE MachineUDID=@MachineUDID");

            myParams.Add(new Params("@MachineSerial", "NVARCHAR", my_Machine_List.First().DeviceSerial));
            //myParams.Add(new Params("@MachineImei", "NVARCHAR", my_Machine_List.First().MachineImei));
            myParams.Add(new Params("@MachineUDID", "NVARCHAR", my_Machine_List.First().DeviceUDID));
            myParams.Add(new Params("@MachineName", "NVARCHAR", my_Machine_List.First().DeviceName));
            myParams.Add(new Params("@BranchId", "GUID", my_Machine_List.First().BranchId));
            myParams.Add(new Params("@MachineStatus", "INT", my_Machine_List.First().DeviceStatus));
            myParams.Add(new Params("@iPadStatus", "INT", my_Machine_List.First().iPadStatus));
            myParams.Add(new Params("@appStatus", "INT", my_Machine_List.First().appStatus));
            myParams.Add(new Params("@componentStatus", "INT", my_Machine_List.First().ComponentStatus));
            myParams.Add(new Params("@MonitorPushDatetime", "DATETIME", my_Machine_List.First().MonitoringPushDatetime));
            myParams.Add(new Params("@MonitorRecDatetime", "DATETIME", my_Machine_List.First().MonitoringRecDatetime));
            myParams.Add(new Params("@ApnPushDatetime", "DATETIME", my_Machine_List.First().ApnPushDatetime));
            myParams.Add(new Params("@ApnRecDatetime", "DATETIME", my_Machine_List.First().ApnRecDatetime));
            myParams.Add(new Params("@ResolvedTime", "DATETIME", my_Machine_List.First().ResolvedTime));
            myParams.Add(new Params("@ComponentAlertTime", "DATETIME", my_Machine_List.First().ComponentAlertTime));
            myParams.Add(new Params("@ComponentLastAlertTime", "DATETIME", my_Machine_List.First().ComponentLastAlertTime));
            myParams.Add(new Params("@LastInitializeTime", "DATETIME", my_Machine_List.First().LastInitializeTime));
            myParams.Add(new Params("@MachineDataSignal", "NVARCHAR", my_Machine_List.First().DeviceDataSignal));
            myParams.Add(new Params("@iPadBattLevel", "NVARCHAR", my_Machine_List.First().IPadBattLevel));
            myParams.Add(new Params("@ComponentBattLevel", "INT", my_Machine_List.First().ComponentBattLvl));
            myParams.Add(new Params("@ComponentCardReaderStatus", "BIT", my_Machine_List.First().ComponentCardReaderStatus));
            myParams.Add(new Params("@ComponentThumbStatus", "BIT", my_Machine_List.First().ComponentThumbStatus));
            myParams.Add(new Params("@MsfAppID", "NVARCHAR", my_Machine_List.First().MsfAppID));
            myParams.Add(new Params("@OSVersion", "NVARCHAR", my_Machine_List.First().OsVersion));
            myParams.Add(new Params("@DeviceCapacity", "NVARCHAR", my_Machine_List.First().DeviceCapacity));
            myParams.Add(new Params("@AvailableDevice_Capacity", "NVARCHAR", my_Machine_List.First().AvailableDevice_Capacity));
            myParams.Add(new Params("@EraseDateTime", "DATETIME", my_Machine_List.First().EraseDateTime));



            ret = SqlDataControl.Input(sql.ToString(), myParams);


        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                            ex.ToString());
        }

        return ret;
    }


    /// <summary>
    /// Update the iPad Jacket firmware information of a particular machine based on the UDID parameter. 
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    public override bool UpdateDeviceComponentStatus(DeviceEn machine)
    {
        bool ret = false;

        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();


            // Update the firmware status to be displayed from the MDM Machine Maintenance page. 
            sql.AppendLine(@"update tblmachine 
                             set FirmwareVersion = @FirmwareVersion, FirmwareBatteryStatus = @FirmwareBatteryStatus, FirmwareBatteryLevel = @FirmwareBatteryLevel, FirmwareSerial = @FirmwareSerial
                             where MachineSerial = @MachineSerial");

            MyParams.Add(new Params("@MachineSerial", "NVARCHAR", machine.DeviceSerial));
            MyParams.Add(new Params("@FirmwareVersion", "NVARCHAR", machine.FirmwareVersion));
            MyParams.Add(new Params("@FirmwareBatteryStatus", "NVARCHAR", machine.FirmwareBatteryStatus));
            MyParams.Add(new Params("@FirmwareBatteryLevel", "NVARCHAR", machine.FirmwareBatteryLevel));
            MyParams.Add(new Params("@FirmwareSerial", "NVARCHAR", machine.FirmwareSerial));


            ret = SqlDataControl.Input(sql.ToString(), MyParams);

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


    /// <summary>
    /// Update the iPad Jacket firmware application's unique push device token of a particular machine based on the serial number parameter. 
    /// </summary>
    /// <param name="machine"></param>
    /// <returns></returns>
    public override bool UpdateFirmwareAppDeviceToken(DeviceEn machine)
    {
        bool ret = false;

        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();


            // Update the firmware status to be displayed from the MDM Machine Maintenance page. 
            sql.AppendLine(@"update tblmachine          
                             set AppDeviceToken = @AppDeviceToken
                             where MachineSerial = @MachineSerial");

            MyParams.Add(new Params("@AppDeviceToken", "NVARCHAR", machine.AppDeviceToken));
            MyParams.Add(new Params("@MachineSerial", "NVARCHAR", machine.DeviceSerial));

            ret = SqlDataControl.Input(sql.ToString(), MyParams);

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


    /// <summary>
    /// Retrieve all the available device tokens stored for the firmware SDK application on the machines. 
    /// </summary>
    /// <returns></returns>
    public override List<string> GetAllFirmwareAppDeviceToken()
    {
        var deviceTokenArray = new List<string>();

        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();


            // Update the firmware status to be displayed from the MDM Machine Maintenance page. 
            sql.AppendLine(@"select AppDeviceToken from tblMachine");


            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);

            // run a for loop and store all device tokens retrieved in an arraylist of strings. 
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["AppDeviceToken"] != null && dt.Rows[i]["AppDeviceToken"].ToString().Length > 0)
                        deviceTokenArray.Add(dt.Rows[i]["AppDeviceToken"].ToString());
                }
            }
        }

        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ex);
        }
        return deviceTokenArray;
    }




    public override string GetFirmwareAppDeviceToken(DeviceEn machine)
    {
        string ret = "";

        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();


            // Update the firmware status to be displayed from the MDM Machine Maintenance page. 
            sql.AppendLine(@"select AppDeviceToken from tblMachine          
                             where MachineSerial = @MachineSerial");

            MyParams.Add(new Params("@MachineSerial", "NVARCHAR", machine.DeviceSerial));

            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);

            if (dt.Rows.Count > 0 && dt.Rows[0]["AppDeviceToken"] != null)
                ret = dt.Rows[0]["AppDeviceToken"].ToString();

            else
                ret = "";

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


    /// <summary>
    /// This method will be called two times when an OS update is performed by 
    /// an ipad machine. The first time is called when the ipad starts installing the 
    /// OS updates, and second time being the ipad finished installing OS updates. 
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public override bool UpdateOSInstallStatusbyMachine(OSUpdateEn update)
    {
        bool ret = false;

        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();


            // if we start the OS installation
            if (!update.UpdateInstalled)
            {
                sql.AppendLine("insert into MDM_Pending_OS_Updates (ID, UDID, ProductKey, ProductVersion, UpdateInstalled, RequestTime )");
                sql.AppendLine("values (@ID,@UDID,@ProductKey,@ProductVersion, @UpdateInstalled, @RequestTime)");

                MyParams.Add(new Params("@ID", "GUID", Guid.NewGuid()));
                MyParams.Add(new Params("@UDID", "NVARCHAR", update.UDID));
                MyParams.Add(new Params("@ProductKey", "NVARCHAR", update.ProductKey));
                MyParams.Add(new Params("@ProductVersion", "NVARCHAR", update.ProductVersion));
                MyParams.Add(new Params("@UpdateInstalled", "BIT", update.UpdateInstalled));
                MyParams.Add(new Params("@RequestTime", "DATETIME", DateTime.Now));

            }

            // if OS update is received by end machine, then we marked the boolean as complete. 
            else
            {
                sql.AppendLine(@"UPDATE MDM_Pending_OS_Updates
                                SET UpdateInstalled = 1
                                    WHERE UDID = @UDID");

                MyParams.Add(new Params("@UDID", "NVARCHAR", update.UDID));
            }


            ret = SqlDataControl.Input(sql.ToString(), MyParams);

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


    /// select only one last record of machine's endtime, if null, update it
    public override bool CheckEndTime(string machineSerial)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select top 1 activityID, machineImei, startTime, endTime ");
            sql.AppendLine("from tblMonitoringActivity ");
            sql.AppendLine("where MachineSerial = @MachineSerial ");
            sql.AppendLine("order by startTime desc ");
            MyParams.Add(new Params("@MachineSerial", "NVARCHAR", machineSerial));
            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["endTime"] == null)
                {
                    sql.Clear();
                    MyParams.Clear();
                    sql.AppendLine("update tblMonitoringActivity set ");
                    sql.AppendLine("endTime = @endTime ");
                    sql.AppendLine("where ActivityID = @ActivityID and machineSerial = @machineSerial ");
                    MyParams.Add(new Params("@endTime", "DATETIME", Convert.ToDateTime(dt.Rows[0]["startTime"])));
                    MyParams.Add(new Params("@ActivityID", "NVARCHAR", dt.Rows[0]["ActivityID"].ToString()));
                    MyParams.Add(new Params("@machineSerial", "NVARCHAR", dt.Rows[0]["machineSerial"].ToString()));

                    ret = SqlDataControl.Input(sql.ToString(), MyParams);
                }
                else
                {
                    ret = true;
                }
            }
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

    /// Insert tblActivity without endtime
    public override bool InsertMonitoringActivity(string machineSerial)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            //sql.AppendLine("update tblMachine set ");
            //sql.AppendLine("LastInitializeTime = @LastInitializeTime  ");
            //sql.AppendLine("where MachineImei = @MachineImei ");
            //sql.AppendLine("#SPLIT#");
            sql.AppendLine("insert into tblMonitoringActivity ");
            sql.AppendLine("(ActivityID, machineSerial, StartTime) ");
            sql.AppendLine("values ");
            sql.AppendLine("(@ActivityID, @machineSerial, @StartTime) ");

            //MyParams.Add(new Params("@LastInitializeTime", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@machineSerial", "NVARCHAR", machineSerial));
            MyParams.Add(new Params("@ActivityID", "NVARCHAR", Guid.NewGuid().ToString()));
            MyParams.Add(new Params("@StartTime", "DATETIME", DateTime.Now));

            ret = SqlDataControl.Input(sql.ToString(), MyParams);
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

    /// select only one latest activity which by default the end time should be null
    public override MonitoringActivityEn GetLastMonitoringActivity(string machineSerial)
    {
        MonitoringActivityEn ret = new MonitoringActivityEn();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select top 1 ActivityID, machineImei, startTime, endTime ");
            sql.AppendLine("from tblMonitoringActivity ");
            sql.AppendLine("where machineSerial = @machineSerial ");
            //sql.AppendLine("and endTime is null ");
            sql.AppendLine("order by startTime desc ");

            //MyParams.Add(new Params("@ActivityID", "NVARCHAR", activityID));
            MyParams.Add(new Params("@machineSerial", "NVARCHAR", machineSerial));

            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);
            if (dt.Rows.Count > 0)
            {
                ret.ActivityID = dt.Rows[0]["ActivityID"].ToString();
                ret.DeviceIMEI = dt.Rows[0]["MachineImei"].ToString();
                ret.StartTime = Convert.ToDateTime(dt.Rows[0]["StartTime"]);
            }

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

    /// Update machine's endtime
    public override bool UpdateMonitoringActivity(string activityID, string machineSerial)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("update tblMonitoringActivity set ");
            sql.AppendLine("EndTime = @EndTime ");
            sql.AppendLine("where ActivityID = @ActivityID and machineSerial = @machineSerial ");

            MyParams.Add(new Params("@ActivityID", "NVARCHAR", activityID));
            MyParams.Add(new Params("@machineSerial", "NVARCHAR", machineSerial));
            MyParams.Add(new Params("@EndTime", "DATETIME", DateTime.Now));

            ret = SqlDataControl.Input(sql.ToString(), MyParams);
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

    /// <summary>
    /// Insert the location history of a particular machine. 
    /// </summary>
    /// <param name="items"></param>
    private void insertLocationHistory(DeviceEn items)
    {
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("insert into tblLocationHistory (MachineSerial,MachineName,Latitude,Longitude,Createddate)");
            sql.AppendLine("values (@MachineSerial,@name,@lat,@lon,@created)");

            MyParams.Add(new Params("@MachineSerial", "NVARCHAR", items.DeviceSerial));
            MyParams.Add(new Params("@name", "NVARCHAR", items.DeviceName));
            MyParams.Add(new Params("@lat", "NVARCHAR", items.Latitude));
            MyParams.Add(new Params("@lon", "NVARCHAR", items.Longitude));
            MyParams.Add(new Params("@created", "DATETIME", items.MonitoringRecDatetime));

            SqlDataControl.Input(sql.ToString(), MyParams);

        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                          System.Reflection.MethodBase.GetCurrentMethod().Name,
                          ex);
        }
    }


    /// <summary>
    /// Insert log entries of a machine with machine details related to component battery 
    /// level, card reader status, biometric reader and location. 
    /// </summary>
    /// <param name="items"></param>
    private void logAlert(DeviceEn items)
    {
        try
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder desc = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            desc.AppendLine("Component Battery Lvl : " + items.ComponentBattLvl);
            desc.AppendLine("Card Reader Status : " + items.ComponentCardReaderStatus.ToString());
            desc.AppendLine("Bio Metric Reader : " + items.ComponentThumbStatus.ToString());
            desc.AppendLine("Location : " + items.Latitude + "," + items.Longitude);

            sql.AppendLine("insert into tblAlert (machineName, MachineSerial, description, alertDatetime)");
            sql.AppendLine("values (@name,@MachineSerial,@desc,@adate)");

            //string desc = "ComponentBattLvl : ";
            MyParams.Add(new Params("@name", "NVARCHAR", items.DeviceName));
            MyParams.Add(new Params("@MachineSerial", "NVARCHAR", items.DeviceSerial));
            MyParams.Add(new Params("@desc", "NVARCHAR", desc.ToString()));
            MyParams.Add(new Params("@adate", "DATETIME", items.MonitoringRecDatetime));

            SqlDataControl.Input(sql.ToString(), MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                          System.Reflection.MethodBase.GetCurrentMethod().Name,
                          ex);
        }
    }

}
