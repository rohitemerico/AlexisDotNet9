using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;
namespace Alexis.Dashboard.Controller;

public class KioskCreateMaintenanceController : GlobalController
{
    /// <summary>
    /// check if the machine serial no exist, doesnt matter the status
    /// </summary>
    /// <param name="machineSn">dont need to upper</param>
    /// <returns>true if the machine doesnt exist</returns>
    public static bool isMachineAvailable(string machineSn)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select count(*) from machine");
            sql.AppendLine("where mserial = @serial");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@serial", "NVARCHAR", machineSn.ToUpper()));

            if (dbController.Count(sql.ToString(), "connectionString", MyParams) == 0)
                ret = true;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("isMachineAvailable.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static bool isKioskIDAvailable(string kioskID)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select count(*) from machine");
            sql.AppendLine("where mkioskID = @mkioskID");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@mkioskID", "NVARCHAR", kioskID));

            if (dbController.Count(sql.ToString(), "connectionString", MyParams) == 0)
                ret = true;
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            //Logger.LogToFile("isMachineAvailable.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    /// <summary>
    /// gets the machine group template thats approved
    /// </summary>
    /// <returns>kid,kdesc</returns>
    public static DataTable bindGroup()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select kid,kdesc");
            sql.AppendLine("from machine_group");
            sql.AppendLine("where kStatus = 1");

            List<Params> MyParams = new List<Params>();

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("bindGroup.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable bindMachine()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            //sql.AppendLine("select case m.mScreenType");
            //sql.AppendLine("when 0 then 'Black'");
            //sql.AppendLine("when 1 then 'Green'");
            //sql.AppendLine("else 'Unknown' end mscreentype,");
            //sql.AppendLine("case m.mstatus ");
            //sql.AppendLine("when 0 then 'Pending'");
            //sql.AppendLine("when 1 then 'Activated'");
            //sql.AppendLine("else 'Declined' end mstatus,");
            //sql.AppendLine("m.mid as mid,m.mdesc as description,");
            //sql.AppendLine("m.mserial as serial,m.mCreatedDate as createddate,");
            //sql.AppendLine("m.maddress as address,m.mLatitude as latitude,");
            //sql.AppendLine("m.mLongitude as longitude,ma.aDesc as advertisement,");
            //sql.AppendLine("a.aDesc as alert,mb.bDesc as businesshour,");
            //sql.AppendLine("md.dDesc as document,mg.kDesc as groupdesc,mh.hDesc as hopper");
            //sql.AppendLine("from machine m, Machine_Advertisement ma, machine_alert a,");
            //sql.AppendLine("Machine_BusinessHour mb,Machine_Document md,machine_group mg,machine_hopper mh");
            //sql.AppendLine("where m.mAdvertisementId = ma.aID");
            //sql.AppendLine("and m.mAlertId = a.aID");
            //sql.AppendLine("and m.mBusinessHourId = mb.bID");
            //sql.AppendLine("and m.mDocumentId = md.dID");
            //sql.AppendLine("and m.mGroupId = mg.kid");
            //sql.AppendLine("and m.mHopperId = mh.hID");


            sql.AppendLine("select mg.KSCREENBACKGROUND, ");
            sql.AppendLine("case m.mstatus when 0 then 'Pending' when 1 then 'Active' else 'Inactive' end mstatus, ");
            sql.AppendLine("m.mid as mid,m.mdesc as description, m.mKioskID, m.mRemarks, ");
            sql.AppendLine("m.mserial as serial,m.mCreatedDate as createddate, ");
            sql.AppendLine("m.maddress as address,m.mLatitude as latitude, ");
            sql.AppendLine("m.mLongitude as longitude, "); //ma.aDesc as advertisement, ");
            sql.AppendLine("al.aDesc as alert,mb.bDesc as businesshour, ");
            sql.AppendLine("md.dDesc as document,mg.kDesc as groupdesc,mh.hDesc as hopper, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uname UpdatedBy, m.lastupdateddate ");
            sql.AppendLine(", m.VTMVERSION ");
            sql.AppendLine("from machine m ");
            sql.AppendLine("join machine_group mg on (m.mGroupId = mg.kid) ");
            //sql.AppendLine("join ADVERTISEMENT_GROUP ag on (mg.KID = ag.GroupId) ");
            //sql.AppendLine("join Machine_Advertisement ma on (ag.advId = ma.aID) ");
            sql.AppendLine("join machine_alert al on (mg.kAlertId = al.aID) ");
            sql.AppendLine("join Machine_BusinessHour mb on (mg.kBusinessHourId = mb.bID) ");
            sql.AppendLine("join Machine_Document md on (mg.kDocumentId = md.dID) ");
            sql.AppendLine("join machine_hopper mh on (mg.kHopperId = mh.hID) ");
            sql.AppendLine("left join User_Login ulc on (m.mCreatedBy = ulc.aID)  ");
            sql.AppendLine("left join User_Login ula on (m.mApprovedBy = ula.aID)  ");
            sql.AppendLine("left join User_Login uld on (m.mDeclineBy = uld.aID)  ");
            sql.AppendLine("left join User_Login ulu on (m.mUpdatedBy = ulu.aID)  ");

            List<Params> MyParams = new List<Params>();

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("bindMachine.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }
    /// <summary>
    /// to create a new kiosk
    /// </summary>
    /// <param name="machineName">Machine Name</param>
    /// <param name="machineSerial">unique machine serial no</param>
    /// <param name="machineAddress">machine address</param>
    /// <param name="Latitude">accepts null</param>
    /// <param name="Longitude">accepts null</param>
    /// <param name="hopperId">hopper template</param>
    /// <param name="docId">document template</param>
    /// <param name="alertId">alert template</param>
    /// <param name="groupId">group template</param>
    /// <param name="advId">advetisement template</param>
    /// <param name="operationId">operation hours template</param>
    /// <param name="screenType">screen type</param>
    /// <param name="userid">the user that is creating</param>
    /// <returns>true when successfully created</returns>
    public static bool CreateKiosk(MKiosk entity, string ClientIp)
    {
        bool ret = false;
        StringBuilder auditlogging = new StringBuilder();
        try
        {
            auditlogging.AppendLine("Create Kiosk : " + entity.MachineDescription);
            auditlogging.AppendLine("Machine Serial No : " + entity.MachineSerial);

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into machine");
            sql.AppendLine("(mid,mdesc,mserial,mkioskid,mStationID,maddress,mlatitude,mlongitude,");
            sql.AppendLine("mgroupid,mcreateddate,mcreatedby,mstatus, ipAddress, PortNumber, mRemarks, monitoringStatus,");
            sql.AppendLine("mpilot)");
            sql.AppendLine("values");
            sql.AppendLine("(@mid,@mdesc,@mserial,@mkioskid,@mStationID,@maddress,@mlatitude,@mlongitude,");
            sql.AppendLine("@mgroupid,@mcreateddate,@mcreatedby,@mstatus, @IP, @Port, @mRemarks, @monitoringStatus,");
            sql.AppendLine("@mpilot)");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@mid", "GUID", entity.MachineID));
            MyParams.Add(new Params("@mdesc", "NVARCHAR", entity.MachineDescription));
            MyParams.Add(new Params("@mserial", "NVARCHAR", entity.MachineSerial));
            MyParams.Add(new Params("@mkioskid", "NVARCHAR", entity.MachineKioskID));
            MyParams.Add(new Params("@mStationID", "NVARCHAR", entity.MachineStationID));
            MyParams.Add(new Params("@maddress", "NVARCHAR", entity.MachineAddress));
            MyParams.Add(new Params("@mlatitude", "NVARCHAR", entity.MachineLatitude));
            MyParams.Add(new Params("@mlongitude", "NVARCHAR", entity.MachineLongtitude));
            MyParams.Add(new Params("@mgroupid", "GUID", entity.MachineGroupID));
            MyParams.Add(new Params("@mcreateddate", "DATETIME", entity.CreatedDate));
            MyParams.Add(new Params("@mcreatedby", "GUID", entity.CreatedBy));
            MyParams.Add(new Params("@mstatus", "INT", 0));//entity.Status));

            MyParams.Add(new Params("@IP", "NVARCHAR", entity.MacIP));
            MyParams.Add(new Params("@Port", "NVARCHAR", entity.MacPort));
            MyParams.Add(new Params("@mRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@monitoringStatus", "INT", 0));//entity.Status));
            MyParams.Add(new Params("@mpilot", "INT", 0));//entity.MacPilot));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("CreateKiosk.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            AuditLog.CreateAuditLog(auditlogging.ToString(), AuditCategory.Kiosk_Maintenance, ModuleLogAction.Create_Kiosk, entity.CreatedBy, ret, ClientIp);
        }
        return ret;
    }
    /// <summary>
    /// retrieve machine data. Split it by #SPLIT#
    /// </summary>
    /// <param name="mid">machine unique id</param>
    /// <returns>#SPLIT#</returns>
    public static string getMachineItems(Guid mid)
    {
        string ret = string.Empty;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select mg.KSCREENBACKGROUND, ");
            sql.AppendLine("case m.mstatus when 0 then 'Pending' when 1 then 'Active' else 'Inactive' end mstatus, ");
            sql.AppendLine("m.mid as mid,m.mdesc as description, m.mKioskID, m.mStationID, ");
            sql.AppendLine("m.mserial as serial,m.mCreatedDate as createddate, ");
            sql.AppendLine("m.maddress as address,m.mLatitude as latitude, ");
            sql.AppendLine("m.mLongitude as longitude, "); //ma.aDesc as advertisement, ");
            sql.AppendLine("al.aDesc as alert,mb.bDesc as businesshour, ");
            sql.AppendLine("md.dDesc as document,mg.kDesc as groupdesc,mh.hDesc as hopper, ");

            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ");
            sql.AppendLine("m.ipAddress, m.portNumber, m.mRemarks ");
            sql.AppendLine(", m.VTMVERSION, m.mpilot ");

            sql.AppendLine("from machine m ");
            sql.AppendLine("join machine_group mg on (m.mGroupId = mg.kid) ");
            //sql.AppendLine("join ADVERTISEMENT_GROUP ag on (mg.KID = ag.GroupId) ");
            //sql.AppendLine("join Machine_Advertisement ma on (ag.advId = ma.aID) ");
            sql.AppendLine("join machine_alert al on (mg.kAlertId = al.aID) ");
            sql.AppendLine("join Machine_BusinessHour mb on (mg.kBusinessHourId = mb.bID) ");
            sql.AppendLine("join Machine_Document md on (mg.kDocumentId = md.dID) ");
            sql.AppendLine("join machine_hopper mh on (mg.kHopperId = mh.hID) ");
            sql.AppendLine("left join User_Login ulc on (m.mCreatedBy = ulc.aID)  ");
            sql.AppendLine("left join User_Login ula on (m.mApprovedBy = ula.aID)  ");
            sql.AppendLine("left join User_Login uld on (m.mDeclineBy = uld.aID)  ");
            sql.AppendLine("where m.mid = @mid");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@mid", "GUID", mid));
            ret = dbController.Output(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getMachineItem.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable getMachineItemsById(Guid mid)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select mg.KSCREENBACKGROUND, ");
            sql.AppendLine("m.mstatus mstatID, m.mGroupID, ");
            sql.AppendLine("case m.mstatus when 0 then 'Pending' when 1 then 'Active' else 'Inactive' end mstatus, ");
            sql.AppendLine("m.mid as mid,m.mdesc as description, m.mKioskID, m.mStationID, ");
            sql.AppendLine("m.mserial as serial,m.mCreatedDate as createddate, ");
            sql.AppendLine("m.maddress as address,m.mLatitude as latitude, ");
            sql.AppendLine("m.mLongitude as longitude, "); //ma.aDesc as advertisement, ");
            sql.AppendLine("m.ipAddress, m.portNumber, m.mRemarks, ");
            sql.AppendLine("al.aDesc as alert,mb.bDesc as businesshour, ");
            sql.AppendLine("md.dDesc as document,mg.kDesc as groupdesc,mh.hDesc as hopper, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ");
            sql.AppendLine("m.mpilot ");
            sql.AppendLine("from machine m ");
            sql.AppendLine("join machine_group mg on (m.mGroupId = mg.kid) ");
            //sql.AppendLine("join ADVERTISEMENT_GROUP ag on (mg.KID = ag.GroupId) ");
            //sql.AppendLine("join Machine_Advertisement ma on (ag.advId = ma.aID) ");
            sql.AppendLine("join machine_alert al on (mg.kAlertId = al.aID) ");
            sql.AppendLine("join Machine_BusinessHour mb on (mg.kBusinessHourId = mb.bID) ");
            sql.AppendLine("join Machine_Document md on (mg.kDocumentId = md.dID) ");
            sql.AppendLine("join machine_hopper mh on (mg.kHopperId = mh.hID) ");
            sql.AppendLine("left join User_Login ulc on (m.mCreatedBy = ulc.aID)  ");
            sql.AppendLine("left join User_Login ula on (m.mApprovedBy = ula.aID)  ");
            sql.AppendLine("left join User_Login uld on (m.mDeclineBy = uld.aID)  ");
            sql.AppendLine("where m.mid = @mid");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@mid", "GUID", mid));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getMachineItem.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static bool updateMachine(MKiosk entity)
    {
        bool ret = false;
        StringBuilder auditLogging = new StringBuilder();
        try
        {
            //auditLogging.AppendLine("Update Machine Serial No : " + entity.MachineSerial);
            //auditLogging.AppendLine("Old Values : ");

            //string oldstring = string.Empty;
            //for (int i = 0; i < oldvalues.Length; i++)
            //{
            //    if (i != 2)
            //        oldstring += oldvalues[i].ToString() + ",";
            //}
            //auditLogging.AppendLine(oldstring.Substring(oldstring.Length - 1, 1));

            //string newstrings = string.Empty;
            //auditLogging.AppendLine("New Values : ");
            //for (int x = 0; x < newvalues.Length; x++)
            //    newstrings += newvalues[x].ToString() + ",";
            //auditLogging.AppendLine(newstrings.Substring(newstrings.Length - 1, 1));

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update machine set");
            sql.AppendLine("mdesc = @mdesc,");
            sql.AppendLine("mkioskid = @mkioskid,");
            sql.AppendLine("mStationID = @mStationID,");
            sql.AppendLine("maddress = @maddress,");
            sql.AppendLine("mlatitude = @mlatitude,");
            sql.AppendLine("mlongitude = @mlongitude,");
            sql.AppendLine("mgroupid = @mgroupid,");
            sql.AppendLine("mRemarks = @mRemarks,");
            sql.AppendLine("mstatus = @mstatus,");
            sql.AppendLine("ipAddress = @IP,");
            sql.AppendLine("portNumber = @Port,");
            sql.AppendLine("mUpdatedBy = @mUpdatedBy,");
            sql.AppendLine("lastupdateddate = @mUpdatedDate");
            sql.AppendLine("where mid = @mid");

            List<Params> MyParams = new List<Params>();

            MyParams.Add(new Params("@mdesc", "NVARCHAR", entity.MachineDescription));
            MyParams.Add(new Params("@mkioskid", "NVARCHAR", entity.MachineKioskID));
            MyParams.Add(new Params("@mStationID", "NVARCHAR", entity.MachineStationID));
            MyParams.Add(new Params("@maddress", "NVARCHAR", entity.MachineAddress));
            MyParams.Add(new Params("@mlatitude", "NVARCHAR", entity.MachineLatitude));
            MyParams.Add(new Params("@mlongitude", "NVARCHAR", entity.MachineLongtitude));
            MyParams.Add(new Params("@mgroupid", "GUID", entity.MachineGroupID));
            MyParams.Add(new Params("@mRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@mstatus", "INT", entity.Status));
            MyParams.Add(new Params("@mid", "GUID", entity.MachineID));
            MyParams.Add(new Params("@IP", "NVARCHAR", entity.MacIP));
            MyParams.Add(new Params("@Port", "NVARCHAR", entity.MacPort));
            MyParams.Add(new Params("@mUpdatedDate", "DATETIME", entity.UpdatedDate));
            MyParams.Add(new Params("@mUpdatedBy", "GUID", entity.UpdatedBy));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UpdateMachine.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            //Logger.AuditLog(auditLogging.ToString(), Logger.ModuleName.Kiosk_Maintenance, Logger.ModuleAction.Update_Kiosk, entity.UpdatedBy, ret);
        }
        return ret;
    }

    public static bool updatePilotKiosk(string kioskID, bool isChecked)
    {
        bool ret = false;
        StringBuilder auditLogging = new StringBuilder();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update machine set");
            sql.AppendLine("mPilot = @mpilot");
            sql.AppendLine("where mKioskID = @mKioskID");

            List<Params> MyParams = new List<Params>();

            MyParams.Add(new Params("@mpilot", "INT", isChecked));
            MyParams.Add(new Params("@mKioskID", "NVARCHAR", kioskID));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            //Logger.AuditLog(auditLogging.ToString(), Logger.ModuleName.Kiosk_Maintenance, Logger.ModuleAction.Update_Kiosk, entity.UpdatedBy, ret);
        }
        return ret;
    }

    public static bool approveMachine(Guid machineID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update Machine set ");
            sql.AppendLine("mStatus=@mStatus, mApprovedDate=@mApprovedDate, mApprovedBy=@mApprovedBy, ");
            sql.AppendLine("monitoringStatus=@monitoringStatus ");
            sql.AppendLine("where mID=@mID ");

            MyParams.Add(new Params("@mID", "GUID", machineID));
            MyParams.Add(new Params("@mStatus", "INT", 1));
            MyParams.Add(new Params("@mApprovedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@mApprovedBy", "GUID", adminGuid));
            MyParams.Add(new Params("@monitoringStatus", "INT", 1));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("select mserial from Machine where mID=@mID ");
                MyParams.Add(new Params("@mID", "GUID", machineID));
                DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
                if (dt.Rows.Count > 0)
                {
                    sql.Clear();
                    MyParams.Clear();
                    sql.AppendLine("delete from Machine_Components where mserial = @mserial ");
                    MyParams.Add(new Params("@mserial", "NVARCHAR", dt.Rows[0]["mserial"].ToString()));
                    if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                    {
                        bool insertAll = true;
                        //int lengthOfComponentName = Enum.GetNames(typeof(ComponentName)).Length;
                        for (int i = 1; i <= 19; i++)
                        {
                            sql.Clear();
                            MyParams.Clear();
                            sql.AppendLine("insert into Machine_Components ");
                            sql.AppendLine("(mserial, componentID, componentMISC, status) values ");
                            sql.AppendLine("(@mserial, @componentID, @componentMISC, @status) ");
                            MyParams.Add(new Params("@mserial", "NVARCHAR", dt.Rows[0]["mserial"].ToString()));
                            MyParams.Add(new Params("@componentID", "INT", i));
                            MyParams.Add(new Params("@componentMISC", "NVARCHAR", (ComponentName)i));
                            MyParams.Add(new Params("@status", "NVARCHAR", "ACTIVE"));
                            if (!dbController.Input(sql.ToString(), "connectionString", MyParams))
                            {
                                insertAll = false;
                                break;
                            }
                        }
                        ret = insertAll;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }

        return ret;
    }

    public static bool declineMachine(Guid machineID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update Machine set ");
            sql.AppendLine("mStatus=@mStatus, mDeclineDate=@mDeclineDate, mDeclineBy=@mDeclineBy");
            sql.AppendLine("where mID=@mID ");

            MyParams.Add(new Params("@mID", "GUID", machineID));
            MyParams.Add(new Params("@mStatus", "INT", 2));
            MyParams.Add(new Params("@mDeclineDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@mDeclineBy", "GUID", adminGuid));

            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static bool deleteMachine(Guid machineID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("delete Machine ");
            sql.AppendLine("where mID=@mID ");

            MyParams.Add(new Params("@mID", "GUID", machineID));

            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static string getMachineItems(string KioskID)
    {
        string ret = string.Empty;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select mg.KSCREENBACKGROUND, ");
            sql.AppendLine("case m.mstatus when 0 then 'Pending' when 1 then 'Active' else 'Inactive' end mstatus, ");
            sql.AppendLine("m.mid as mid,m.mdesc as description, m.mKioskID, m.mStationID, ");
            sql.AppendLine("m.mserial as serial,m.mCreatedDate as createddate, ");
            sql.AppendLine("m.maddress as address,m.mLatitude as latitude, ");
            sql.AppendLine("m.mLongitude as longitude, ");
            sql.AppendLine("al.aDesc as alert,mb.bDesc as businesshour, ");
            sql.AppendLine("md.dDesc as document,mg.kDesc as groupdesc,mh.hDesc as hopper, ");

            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ");
            sql.AppendLine("m.ipAddress, m.portNumber, m.mRemarks ");
            sql.AppendLine(", m.VTMVERSION, m.mpilot ");

            sql.AppendLine("from machine m ");
            sql.AppendLine("join machine_group mg on (m.mGroupId = mg.kid) ");
            sql.AppendLine("join machine_alert al on (mg.kAlertId = al.aID) ");
            sql.AppendLine("join Machine_BusinessHour mb on (mg.kBusinessHourId = mb.bID) ");
            sql.AppendLine("join Machine_Document md on (mg.kDocumentId = md.dID) ");
            sql.AppendLine("join machine_hopper mh on (mg.kHopperId = mh.hID) ");
            sql.AppendLine("left join User_Login ulc on (m.mCreatedBy = ulc.aID)  ");
            sql.AppendLine("left join User_Login ula on (m.mApprovedBy = ula.aID)  ");
            sql.AppendLine("left join User_Login uld on (m.mDeclineBy = uld.aID)  ");
            sql.AppendLine("where m.mkioskid = @mid");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@mid", "NVARCHAR", KioskID));
            ret = dbController.Output(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getMachineItem.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }
}