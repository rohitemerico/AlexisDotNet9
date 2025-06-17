using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

public class AlertMaintenanceController : GlobalController
{
    public DataTable getAlertList()
    {
        DataTable dt = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select ma.*, ulc.uName CreatedName, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.UNAME updatedby, ");
            sql.AppendLine("case ma.aStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end AlertStatus ");
            sql.AppendLine("from Machine_Alert ma ");
            sql.AppendLine("left join User_Login ulc on (ma.aCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (ma.aApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (ma.aDeclineBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (ma.aUpdatedBy = ulu.aID) ");
            //sql.AppendLine("and ma.aStatus = 1 ");
            dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("AlertMaintenance_getAlertList.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return dt;
    }

    public bool isAlertAvailable(string alertName)
    {
        bool ret = false;
        StringBuilder sql = new StringBuilder();
        List<Params> MyParams = new List<Params>();
        try
        {
            sql.AppendLine("select * from Machine_Alert where aDesc=@alertName");
            MyParams.Add(new Params("@alertName", "NVARCHAR", alertName));
            if (dbController.GetResult(sql.ToString(), "connectionString", MyParams).Rows.Count != 0)
                ret = true;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("Alert_Maintenance_isAlertAvailable.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public string IsValidatedToInactive(Guid id)
    {
        string ret = "";
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from machine_group mg ");
            sql.AppendLine("join machine_alert ma on (mg.kAlertID = ma.aid) ");
            sql.AppendLine("where ma.aid = @id and mg.kStatus = @status ");

            MyParams.Add(new Params("@id", "GUID", id));
            MyParams.Add(new Params("@status", "INT", 1));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            foreach (DataRow dr in dt.Rows)
                ret += "The Alert is assigned to Machine Group, " + dr["kDesc"].ToString() + "! The Alert Cannot be Inactive! <br />";
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

    public bool createAlert(MAlert entity)
    {
        bool ret = false;
        try
        {
            if (!isAlertAvailable(entity.ADesc))
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("insert into Machine_Alert ");
                sql.AppendLine("(aID,aDesc,aMinCardBal,aMinChequeBal,aMinPaperBal ");
                sql.AppendLine(",aMinRejCardBal,aMinRibFrontBal,aMinRibRearBal,aMinRibTipBal,aMinChequePrintBal ");
                sql.AppendLine(",aMinChequePrintCatridge, aMinCatridgeBal ");
                sql.AppendLine(",aCardEmail,aCardSms,aCardTInterval ");
                sql.AppendLine(",aChequeEmail,aChequeSms,aChequeTInterval ");
                sql.AppendLine(",aMaintenanceEmail,aMaintenanceSms,aMaintenanceTInterval ");
                sql.AppendLine(",aSecurityEmail,aSecuritySms,aSecurityTInterval");
                sql.AppendLine(",aTroubleshootEmail,aTroubleshootSms,aTroubleShootTInterval ");
                sql.AppendLine(",aCreatedDate,aCreatedBy,aRemarks,aStatus) ");
                sql.AppendLine("values (@aid, @aName, @aMinCardBal, @aMinChequeBal, @aMinPaperBal ");
                sql.AppendLine(", @aMinRejCardBal, @aMinRibFrontBal, @aMinRibRearBal, @aMinRibTipBal, @aMinChequePrintBal ");
                sql.AppendLine(", @aMinChequePrintCatridge, @aMinCatridgeBal ");
                sql.AppendLine(", @aCardEmail, @aCardSms, @aCardTInterval ");
                sql.AppendLine(", @aChequeEmail, @aChequeSms, @aChequeTInterval ");
                sql.AppendLine(", @aMaintenanceEmail, @aMaintenanceSms, @aMaintenanceTInterval ");
                sql.AppendLine(", @aSecurityEmail, @aSecuritySms, @aSecurityTInterval");
                sql.AppendLine(", @aTroubleshootEmail, @aTroubleshootSms, @aTroubleShootTInterval ");
                sql.AppendLine(", @aCreatedDate, @aCreatedBy, @aRemarks, @aStatus) ");
                MyParams.Add(new Params("@aid", "GUID", entity.AID));
                MyParams.Add(new Params("@aName", "NVARCHAR", entity.ADesc));
                MyParams.Add(new Params("@aMinCardBal", "INT", entity.AMinCardBal));
                MyParams.Add(new Params("@aMinChequeBal", "INT", entity.AMinChequeBal));
                MyParams.Add(new Params("@aMinPaperBal", "INT", entity.AMinPaperBal));
                MyParams.Add(new Params("@aMinRejCardBal", "INT", entity.AMinRejCardBal));
                MyParams.Add(new Params("@aMinRibFrontBal", "INT", entity.ARibFrontBal));
                MyParams.Add(new Params("@aMinRibRearBal", "INT", entity.ARibRearBal));
                MyParams.Add(new Params("@aMinRibTipBal", "INT", entity.ARibTipBal));
                MyParams.Add(new Params("@aMinChequePrintBal", "INT", entity.AChequePrintBal));
                MyParams.Add(new Params("@aMinChequePrintCatridge", "INT", entity.AChequePrintCatridge));
                MyParams.Add(new Params("@aMinCatridgeBal", "INT", entity.ACatridgeBal));
                MyParams.Add(new Params("@aCardEmail", "NVARCHAR", entity.ACardEmail));
                MyParams.Add(new Params("@aCardSms", "NVARCHAR", entity.ACardSMS));
                MyParams.Add(new Params("@aCardTInterval", "INT", entity.ACardTimeInterval));
                MyParams.Add(new Params("@aChequeEmail", "NVARCHAR", entity.AChequeEmail));
                MyParams.Add(new Params("@aChequeSms", "NVARCHAR", entity.AChequeSMS));
                MyParams.Add(new Params("@aChequeTInterval", "INT", entity.AChequeTimeInterval));
                MyParams.Add(new Params("@aMaintenanceEmail", "NVARCHAR", entity.AMaintenanceEmail));
                MyParams.Add(new Params("@aMaintenanceSms", "NVARCHAR", entity.AMaintenanceSMS));
                MyParams.Add(new Params("@aMaintenanceTInterval", "INT", entity.AMaintenanceTimeInterval));
                MyParams.Add(new Params("@aSecurityEmail", "NVARCHAR", entity.ASecurityEmail));
                MyParams.Add(new Params("@aSecuritySms", "NVARCHAR", entity.ASecuritySMS));
                MyParams.Add(new Params("@aSecurityTInterval", "INT", entity.ASecurityTimeInterval));
                MyParams.Add(new Params("@aTroubleshootEmail", "NVARCHAR", entity.ATroubleShootEmail));
                MyParams.Add(new Params("@aTroubleshootSms", "NVARCHAR", entity.ATroubleShootSMS));
                MyParams.Add(new Params("@aTroubleshootTInterval", "INT", entity.ATroubleShootTimeInterval));
                MyParams.Add(new Params("@aCreatedDate", "DATETIME", entity.CreatedDate));
                MyParams.Add(new Params("@aCreatedBy", "GUID", entity.CreatedBy));
                MyParams.Add(new Params("@aRemarks", "NVARCHAR", entity.Remarks));
                MyParams.Add(new Params("@aStatus", "INT", 0));//entity.Status));
                ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("Create_Alert.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public DataTable getAlertByID(Guid alertId)
    {
        DataTable dt = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select ma.*, ulCreate.uName CreatedName, ");
            sql.AppendLine("case ma.aStatus when 0 then 'Pending' when 1 then 'Approved' when 2 then 'Declined' end AStatus ");
            sql.AppendLine("from Machine_Alert ma, User_Login ulCreate ");
            sql.AppendLine("where ma.aCreatedBy = ulCreate.aID ");
            sql.AppendLine("and ma.aID = @aid ");
            MyParams.Add(new Params("@aid", "GUID", alertId));
            dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("AlertMaintenanceController_getAlertID.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return dt;
    }

    public bool updateAlert(MAlert entity)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update Machine_Alert set ");
            if (!isAlertAvailable(entity.ADesc))
            {
                sql.AppendLine("aDesc=@aName,");
                MyParams.Add(new Params("@aName", "NVARCHAR", entity.ADesc));
            }
            sql.AppendLine("aMinCardBal=@aMinCardBal, aMinChequeBal=@aMinChequeBal, aMinPaperBal=@aMinPaperBal ");
            sql.AppendLine(",aMinRejCardBal=@aMinRejCardBal, aMinRibFrontBal=@aMinRibFrontBal, aMinRibRearBal=@aMinRibRearBal, aMinRibTipBal=@aMinRibTipBal ");//, aMinChequePrintBal=@aMinChequePrintBal ");
            sql.AppendLine(",aMinChequePrintCatridge=@aMinChequePrintCatridge, aMinCatridgeBal=@aMinCatridgeBal ");
            sql.AppendLine(",aCardEmail=@aCardEmail, aCardSms=@aCardSms, aCardTInterval=@aCardTInterval, ");
            sql.AppendLine("aChequeEmail=@aChequeEmail, aChequeSms=@aChequeSms, aChequeTInterval=@aChequeTInterval, ");
            sql.AppendLine("aMaintenanceEmail=@aMaintenanceEmail, aMaintenanceSms=@aMaintenanceSms, aMaintenanceTInterval=@aMaintenanceTInterval ");
            sql.AppendLine(",aSecurityEmail=@aSecurityEmail, aSecuritySms=@aSecuritySms, aSecurityTInterval=@aSecurityTInterval, ");
            sql.AppendLine("aTroubleshootEmail=@aTroubleshootEmail, aTroubleshootSms=@aTroubleshootSms, aTroubleshootTInterval=@aTroubleshootTInterval ");
            sql.AppendLine(", aUpdatedDate=@aUpdatedDate, aUpdatedBy=@aUpdatedBy, aRemarks=@aRemarks, aStatus=@aStatus ");
            sql.AppendLine("where aID=@aid ");
            MyParams.Add(new Params("@aid", "GUID", entity.AID));
            //MyParams.Add(new Params("@aName", "NVARCHAR", entity.ADesc));
            MyParams.Add(new Params("@aMinCardBal", "INT", entity.AMinCardBal));
            MyParams.Add(new Params("@aMinChequeBal", "INT", entity.AMinChequeBal));
            MyParams.Add(new Params("@aMinPaperBal", "INT", entity.AMinPaperBal));
            MyParams.Add(new Params("@aMinRejCardBal", "INT", entity.AMinRejCardBal));
            MyParams.Add(new Params("@aMinRibFrontBal", "INT", entity.ARibFrontBal));
            MyParams.Add(new Params("@aMinRibRearBal", "INT", entity.ARibRearBal));
            MyParams.Add(new Params("@aMinRibTipBal", "INT", entity.ARibTipBal));
            //MyParams.Add(new Params("@aMinChequePrintBal", "INT", entity.AChequePrintBal));
            MyParams.Add(new Params("@aMinChequePrintCatridge", "INT", entity.AChequePrintCatridge));
            MyParams.Add(new Params("@aMinCatridgeBal", "INT", entity.ACatridgeBal));
            MyParams.Add(new Params("@aCardEmail", "NVARCHAR", entity.ACardEmail));
            MyParams.Add(new Params("@aCardSms", "NVARCHAR", entity.ACardSMS));
            MyParams.Add(new Params("@aCardTInterval", "INT", entity.ACardTimeInterval));
            MyParams.Add(new Params("@aChequeEmail", "NVARCHAR", entity.AChequeEmail));
            MyParams.Add(new Params("@aChequeSms", "NVARCHAR", entity.AChequeSMS));
            MyParams.Add(new Params("@aChequeTInterval", "INT", entity.AChequeTimeInterval));
            MyParams.Add(new Params("@aMaintenanceEmail", "NVARCHAR", entity.AMaintenanceEmail));
            MyParams.Add(new Params("@aMaintenanceSms", "NVARCHAR", entity.AMaintenanceSMS));
            MyParams.Add(new Params("@aMaintenanceTInterval", "INT", entity.AMaintenanceTimeInterval));
            MyParams.Add(new Params("@aSecurityEmail", "NVARCHAR", entity.ASecurityEmail));
            MyParams.Add(new Params("@aSecuritySms", "NVARCHAR", entity.ASecuritySMS));
            MyParams.Add(new Params("@aSecurityTInterval", "INT", entity.ASecurityTimeInterval));
            MyParams.Add(new Params("@aTroubleshootEmail", "NVARCHAR", entity.ATroubleShootEmail));
            MyParams.Add(new Params("@aTroubleshootSms", "NVARCHAR", entity.ATroubleShootSMS));
            MyParams.Add(new Params("@aTroubleshootTInterval", "INT", entity.ATroubleShootTimeInterval));
            MyParams.Add(new Params("@aUpdatedDate", "DATETIME", entity.UpdatedDate));
            MyParams.Add(new Params("@aUpdatedBy", "GUID", entity.UpdatedBy));
            MyParams.Add(new Params("@aRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@aStatus", "INT", entity.Status));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("AlertMaintenanceController_updateAlert.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool approveAlert(Guid alertID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update Machine_Alert set ");
            sql.AppendLine("aStatus=@aStatus, aApprovedDate=@aApprovedDate, aApprovedBy=@aApprovedBy");
            sql.AppendLine("where aID=@aID ");

            MyParams.Add(new Params("@aID", "GUID", alertID));
            MyParams.Add(new Params("@aStatus", "INT", 1));
            MyParams.Add(new Params("@aApprovedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@aApprovedBy", "GUID", adminGuid));

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

    public bool declineAlert(Guid alertID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update Machine_Alert set ");
            sql.AppendLine("aStatus=@aStatus, aDeclineDate=@aDeclineDate, aDeclineBy=@aDeclineBy");
            sql.AppendLine("where aID=@aID ");

            MyParams.Add(new Params("@aID", "GUID", alertID));
            MyParams.Add(new Params("@aStatus", "INT", 2));
            MyParams.Add(new Params("@aDeclineDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@aDeclineBy", "GUID", adminGuid));

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

    public bool deleteAlert(Guid alertID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("delete Machine_Alert ");
            sql.AppendLine("where aID=@aID ");

            MyParams.Add(new Params("@aID", "GUID", alertID));

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
}
