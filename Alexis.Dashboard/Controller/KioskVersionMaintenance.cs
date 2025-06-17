using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

public class KioskVersionMaintenance : GlobalController
{
    public static bool isDuplicatedVersion(string ver, bool isAgent)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("Select fw.*, ");
            sql.AppendLine("case fw.status when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end fStatus ");
            sql.AppendLine("from tblFirmware fw join tblFirmData fd on (fw.sysID = fd.firmwareID)");
            sql.AppendLine("where ver = @ver and agentFlag = @agentFlag ");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@ver", "NVARCHAR", ver));
            MyParams.Add(new Params("@agentFlag", "INT", isAgent));

            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count > 0)
            {
                ret = true;
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

    public static DataTable bindTbl()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("Select fw.*, ");
            sql.AppendLine("case fw.agentFlag when 1 then 'Pilot' else 'UnPilot' end pilot, ");
            sql.AppendLine("case fw.status when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end fStatus ");
            sql.AppendLine("from tblFirmware fw join tblFirmData fd on (fw.sysID = fd.firmwareID)");
            sql.AppendLine("order by fw.createdDateTime desc ");

            List<Params> MyParams = new List<Params>();

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("bindHopper.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static bool InsertTblFirmware(MFirm entity)
    {
        bool ret = false;
        StringBuilder auditlogging = new StringBuilder();
        try
        {
            //auditlogging.AppendLine("Create Kiosk Version: " + machineName);
            //auditlogging.AppendLine("Machine Serial No : " + machineSerial.ToUpper());
            Guid mid = Guid.NewGuid();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into tblFirmware");
            sql.AppendLine("(sysid, fpath, status, createdDateTime, countDL, ver, fileSize, agentFlag, remarks)");
            sql.AppendLine("values");
            sql.AppendLine("(@sysid, @fpath, @status, @createdDateTime, @countDL, @ver, @fileSize, @agentFlag, @remarks)");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@sysid", "GUID", entity.SysID));
            MyParams.Add(new Params("@fpath", "NVARCHAR", entity.FPath));
            MyParams.Add(new Params("@status", "INT", entity.Status));
            MyParams.Add(new Params("@createdDateTime", "DATETIME", entity.CreatedDate));
            MyParams.Add(new Params("@countDL", "INT", entity.CountDL));
            MyParams.Add(new Params("@ver", "NVARCHAR", entity.Ver));
            MyParams.Add(new Params("@fileSize", "INT", entity.FSize));
            MyParams.Add(new Params("@agentFlag", "INT", entity.AgentFlag));
            MyParams.Add(new Params("@remarks", "NVARCHAR", entity.Remarks));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("KioskVersion.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            //Logger.AuditLog(auditlogging.ToString(), Logger.ModuleName.Kiosk_Maintenance, Logger.ModuleAction.Create_Kiosk, userid, ret);
        }
        return ret;
    }

    public static bool InsertTblFirmData(MFirm entity)
    {
        bool ret = false;
        StringBuilder auditlogging = new StringBuilder();
        try
        {
            //auditlogging.AppendLine("Create Kiosk Version: " + machineName);
            //auditlogging.AppendLine("Machine Serial No : " + machineSerial.ToUpper());
            Guid mid = Guid.NewGuid();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into tblFirmData");
            sql.AppendLine("(sysid, firmwareid, indicator, firmData)");
            sql.AppendLine("values");
            sql.AppendLine("(@sysid, @firmwareid, @indicator, @firmData)");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@sysid", "GUID", entity.DataPack.SysID));
            MyParams.Add(new Params("@firmwareid", "GUID", entity.DataPack.FirmwareID));
            MyParams.Add(new Params("@indicator", "INT", entity.DataPack.Indicator));
            MyParams.Add(new Params("@firmData", "NVARCHAR", entity.DataPack.Data));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("KioskVersion.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);

        }
        finally
        {
            //Logger.AuditLog(auditlogging.ToString(), Logger.ModuleName.Kiosk_Maintenance, Logger.ModuleAction.Create_Kiosk, userid, ret);
        }
        return ret;
    }

    public static bool ApproveTblFirmware(Guid sysID, Guid adminGuid)
    {
        bool ret = false;
        StringBuilder auditlogging = new StringBuilder();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update tblFirmware set ");
            sql.AppendLine("status=@status, updatedDateTime=@updatedDateTime ");
            sql.AppendLine("where sysid=@sysid ");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@sysid", "GUID", sysID));
            MyParams.Add(new Params("@status", "INT", 1));
            MyParams.Add(new Params("@updatedDateTime", "DATETIME", DateTime.Now));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("KioskVersion.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally //debugdebug
        {
            //Logger.AuditLog(auditlogging.ToString(), Logger.ModuleName.Kiosk_Maintenance, Logger.ModuleAction.Create_Kiosk, adminGuid, ret);
        }
        return ret;
    }

    public static bool deleteTblFirmware(Guid sysID, Guid adminGuid)
    {
        bool ret = false;
        StringBuilder auditlogging = new StringBuilder();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("delete tblFirmware ");
            sql.AppendLine("where sysid=@sysid ");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@sysid", "GUID", sysID));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("KioskVersion.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            //Logger.AuditLog(auditlogging.ToString(), Logger.ModuleName.Kiosk_Maintenance, Logger.ModuleAction.Create_Kiosk, userid, ret);
        }
        return ret;
    }
}
