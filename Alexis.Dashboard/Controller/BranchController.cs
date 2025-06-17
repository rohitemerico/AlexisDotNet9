using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

[Serializable]
public class BranchController : GlobalController
{
    public static Branch getBusinessOperating(Guid templateId)
    {
        Branch ret = new Branch();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select * from User_Branch");
            sql.AppendLine("where bid = :bid");
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":bid", "GUID", templateId));

            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

            ret.Bid = templateId;
            ret.TemplateName = dt.Rows[0]["bdesc"].ToString();
            ret.Monday = Convert.ToBoolean(dt.Rows[0]["bmonday"]);
            ret.Tuesday = Convert.ToBoolean(dt.Rows[0]["btuesday"]);
            ret.Wednesday = Convert.ToBoolean(dt.Rows[0]["bwednesday"]);
            ret.Thursday = Convert.ToBoolean(dt.Rows[0]["bthursday"]);
            ret.Friday = Convert.ToBoolean(dt.Rows[0]["bfriday"]);
            ret.Saturday = Convert.ToBoolean(dt.Rows[0]["bsaturday"]);
            ret.Sunday = Convert.ToBoolean(dt.Rows[0]["bsunday"]);
            ret.Starttime = TimeSpan.Parse(dt.Rows[0]["bOpenTime"].ToString());
            ret.Endtime = TimeSpan.Parse(dt.Rows[0]["bCloseTime"].ToString());
            ret.Remarks = dt.Rows[0]["bRemarks"].ToString();
            ret.Status = Convert.ToInt32(dt.Rows[0]["bStatus"]);


        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperating", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable getBusinessOperatingAll()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select bid,bdesc,bmonday,bTuesday,");
            sql.AppendLine("bWednesday,bThursday,bFriday,bSaturday,");
            sql.AppendLine("bSunday,bOpenTime,bCloseTime,bRemarks, bstatus,");
            sql.AppendLine("bCreatedDate, bApprovedDate, bDeclinedDate, bupdateddate, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uName UpdatedBy, ");
            sql.AppendLine("case bstatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end STATUS ");
            sql.AppendLine("from User_Branch branch ");
            sql.AppendLine("left join User_Login ulc on (branch.bCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (branch.bApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (branch.bDeclinedBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (branch.bUpdatedBy = ulu.aID) ");
            List<Params> MyParams = new List<Params>();
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }
    public static DataTable getNewBusinessOperatingToday()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select bid,bdesc,bmonday,bTuesday,");
            sql.AppendLine("bWednesday,bThursday,bFriday,bSaturday,");
            sql.AppendLine("bSunday,bOpenTime,bCloseTime,bRemarks, bstatus,");
            sql.AppendLine("bCreatedDate, bApprovedDate, bDeclinedDate, bupdateddate, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uName UpdatedBy ");
            sql.AppendLine("from User_Branch branch ");
            sql.AppendLine("left join User_Login ulc on (branch.bCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (branch.bApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (branch.bDeclinedBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (branch.bUpdatedBy = ulu.aID) ");
            sql.AppendLine("where branch.bCreatedBy > :createDate");
            List<Params> MyParams = new List<Params>();

            MyParams.Add(new Params(":createDate", "DATETIME", DateTime.Today.AddDays(-1)));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable getBusinessOperatingAllX()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select bid,bdesc,bmonday,bTuesday,");
            sql.AppendLine("bWednesday,bThursday,bFriday,bSaturday,");
            sql.AppendLine("bSunday,bOpenTime,bCloseTime,bRemarks, bstatus,");
            sql.AppendLine("bCreatedDate, bApprovedDate, bDeclinedDate, bupdateddate, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uName UpdatedBy, ");
            sql.AppendLine("case bstatus when 0 then 'Pending'");
            sql.AppendLine("when 1 then 'Active'");
            sql.AppendLine("else 'Inactive' end bStatus");
            sql.AppendLine("from User_Branch branch ");
            sql.AppendLine("left join User_Login ulc on (branch.bCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (branch.bApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (branch.bDeclinedBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (branch.bUpdatedBy = ulu.aID) ");
            List<Params> MyParams = new List<Params>();
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable getBusinessOperatingById(Guid id)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select bid,bdesc,bmonday,bTuesday,");
            sql.AppendLine("bWednesday,bThursday,bFriday,bSaturday,");
            sql.AppendLine("bSunday,bOpenTime,bCloseTime, ");
            sql.AppendLine("bCreatedDate, bApprovedDate, bDeclinedDate, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ");
            sql.AppendLine("bRemarks,bstatus bstatid,");
            sql.AppendLine("case bstatus when 0 then 'Pending'");
            sql.AppendLine("when 1 then 'Active'");
            sql.AppendLine("else 'Inactive' end bStatus");
            sql.AppendLine("from User_Branch branch ");
            sql.AppendLine("left join User_Login ulc on (branch.bCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (branch.bApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (branch.bDeclinedBy = uld.aID) ");
            sql.AppendLine("where branch.bid = :bid");
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":bid", "GUID", id));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBusinessOperatingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static bool isBranchExist(string branchName)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select count(*) from User_Branch where UPPER(bdesc) = :tname");
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":tname", "NVARCHAR", branchName.ToUpper()));
            if (dbController.Count(sql.ToString(), "connectionString", MyParams) == 0)
                ret = true;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("isTemplateValid.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }


    public static bool isBranchAvailable(string branchName)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select * from User_Branch where bdesc= :tname");
            MyParams.Add(new Params(":tname", "NVARCHAR", branchName));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count == 0)
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
        }
        return ret;
    }


    public static string IsValidatedToInactive(Guid id)
    {
        string ret = "";
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("select * from machine_group mg ");
            sql.AppendLine("join User_Branch mb on (mg.kBusinessHourID = mb.bid) ");
            sql.AppendLine("where mb.bid = :id and mg.kStatus = :status ");

            MyParams.Add(new Params(":id", "GUID", id));
            MyParams.Add(new Params(":status", "INT", 1));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            foreach (DataRow dr in dt.Rows)
                ret += "The Business Operating Hour is assigned to Machine Group, " + dr["kDesc"].ToString() + "! The Business Operating Hour Cannot be Inactive! <br />";
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

    public static bool CreateBranch(Branch Items, string ClientIp)
    {
        bool ret = false;
        StringBuilder auditLogging = new StringBuilder();
        try
        {
            auditLogging.AppendLine("Create " + Items.TemplateName);
            string days = string.Empty;
            if (Items.Sunday)
                days += "Sunday,";
            if (Items.Monday)
                days += "Monday,";
            if (Items.Tuesday)
                days += "Tuesday,";
            if (Items.Wednesday)
                days += "Wednesday,";
            if (Items.Thursday)
                days += "Thursday,";
            if (Items.Friday)
                days += "Friday,";
            if (Items.Saturday)
                days += "Saturday,";
            days = days.Length != 0 ? days.Substring(0, days.Length - 1) : "";
            auditLogging.AppendLine("Days : " + days);
            auditLogging.AppendLine("Start Time : " + Items.Starttime);
            auditLogging.AppendLine("End Time : " + Items.Endtime);

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into User_Branch");
            sql.AppendLine("(bid,bdesc,bmonday,btuesday,bwednesday,bthursday,bfriday,bsaturday,bsunday,bOpenTime,bCloseTime,bcreateddate,bcreatedby,bremarks,bstatus)");
            sql.AppendLine("values");
            sql.AppendLine("(:bid,:bdesc,:bmonday,:btuesday,:bwednesday,:bthursday,:bfriday,:bsaturday,:bsunday,:bOpenTime,:bCloseTime,:bcreateddate,:bcreatedby,:bremarks,:bStatus)");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":bid", "GUID", Items.Bid));
            MyParams.Add(new Params(":bdesc", "NVARCHAR", Items.TemplateName));
            MyParams.Add(new Params(":bmonday", "BIT", Items.Monday));
            MyParams.Add(new Params(":btuesday", "BIT", Items.Tuesday));
            MyParams.Add(new Params(":bwednesday", "BIT", Items.Wednesday));
            MyParams.Add(new Params(":bthursday", "BIT", Items.Thursday));
            MyParams.Add(new Params(":bfriday", "BIT", Items.Friday));
            MyParams.Add(new Params(":bsaturday", "BIT", Items.Saturday));
            MyParams.Add(new Params(":bsunday", "BIT", Items.Sunday));
            MyParams.Add(new Params(":bOpenTime", "NVARCHAR", Items.Starttime.ToString()));
            MyParams.Add(new Params(":bCloseTime", "NVARCHAR", Items.Endtime.ToString()));
            MyParams.Add(new Params(":bcreateddate", "DATETIME", Items.CreatedDate));
            MyParams.Add(new Params(":bremarks", "NVARCHAR", Items.Remarks));
            MyParams.Add(new Params(":bStatus", "INT", 0));
            MyParams.Add(new Params(":bcreatedby", "GUID", Items.CreatedBy));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("CreateBusinessHour.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            AuditLog.CreateAuditLog(auditLogging.ToString(), AuditCategory.System_User_Maintenance, ModuleLogAction.Create_BranchMaintenance, Items.CreatedBy, ret, ClientIp);
        }
        return ret;
    }

    public static bool UpdateBranch(Branch Items, string ClientIp)
    {
        bool ret = false;
        StringBuilder auditLogging = new StringBuilder();
        try
        {
            beforeAudit(getBusinessOperating(Items.Bid));

            auditLogging.AppendLine("Update Business Operating " + Items.TemplateName);

            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update User_Branch set ");
            if (isBranchAvailable(Items.TemplateName))
            {
                sql.AppendLine("bdesc=:bdesc, ");
                MyParams.Add(new Params(":bdesc", "NVARCHAR", Items.TemplateName));
            }
            sql.AppendLine("bmonday=:bmonday,btuesday=:btuesday,bwednesday=:bwednesday, ");
            sql.AppendLine("bthursday=:bthursday,bfriday=:bfriday,bsaturday=:bsaturday,bsunday=:bsunday, ");
            sql.AppendLine("bOpenTime=:bOpenTime,bCloseTime=:bCloseTime,bUpdatedDate=:bUpdatedDate,bUpdatedBy=:bUpdatedBy,bRemarks=:bRemarks,bStatus=:bStatus");
            sql.AppendLine("where bid=:bid");

            MyParams.Add(new Params(":bid", "GUID", Items.Bid));
            MyParams.Add(new Params(":bmonday", "BIT", Items.Monday));
            MyParams.Add(new Params(":btuesday", "BIT", Items.Tuesday));
            MyParams.Add(new Params(":bwednesday", "BIT", Items.Wednesday));
            MyParams.Add(new Params(":bthursday", "BIT", Items.Thursday));
            MyParams.Add(new Params(":bfriday", "BIT", Items.Friday));
            MyParams.Add(new Params(":bsaturday", "BIT", Items.Saturday));
            MyParams.Add(new Params(":bsunday", "BIT", Items.Sunday));
            MyParams.Add(new Params(":bOpenTime", "NVARCHAR", Items.Starttime.ToString()));
            MyParams.Add(new Params(":bCloseTime", "NVARCHAR", Items.Endtime.ToString()));
            MyParams.Add(new Params(":bUpdatedDate", "DATETIME", Items.UpdatedDate));
            MyParams.Add(new Params(":bUpdatedBy", "GUID", Items.UpdatedBy));
            MyParams.Add(new Params(":bRemarks", "NVARCHAR", Items.Remarks));
            MyParams.Add(new Params(":bStatus", "INT", Items.Status));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;

            afterAudit(getBusinessOperating(Items.Bid));
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UpdateBusinessHour.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            //if (CheckUpdated() != "")
            //{
            //    auditLogging.AppendLine(CheckUpdated());
            //    ret = true;
            //}
            //else
            //    ret = false;

            AuditLog.CreateAuditLog(auditLogging.ToString(), AuditCategory.System_User_Maintenance, ModuleLogAction.Update_BranchMaintenance, Items.UpdatedBy, ret, ClientIp);
        }
        return ret;
    }

    public static bool UpdatePendingBizHour(Guid bizID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update User_Branch set ");
            sql.AppendLine("bStatus=:bStatus,bUpdatedDate=:bUpdatedDate,bUpdatedBy=:bUpdatedBy");
            sql.AppendLine("where bID=:bID ");

            MyParams.Add(new Params(":bID", "GUID", bizID));
            MyParams.Add(new Params(":bStatus", "INT", 0));
            MyParams.Add(new Params(":bUpdatedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params(":bUpdatedBy", "GUID", adminGuid));

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

    public static bool approveBizHour(Guid bizID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update user_branch set ");
            sql.AppendLine("bStatus=:bStatus, bApprovedDate=:bApprovedDate, bApprovedBy=:bApprovedBy");
            sql.AppendLine("where bID=:bID ");

            MyParams.Add(new Params(":bID", "GUID", bizID));
            MyParams.Add(new Params(":bStatus", "INT", 1)); //pending to active
            MyParams.Add(new Params(":bApprovedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params(":bApprovedBy", "GUID", adminGuid));

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

    public static bool declineBizHour(Guid bizID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update User_Branch set ");
            sql.AppendLine("bStatus=:bStatus, bDeclinedDate=:bDeclinedDate, bDeclinedBy=:bDeclinedBy");
            sql.AppendLine("where bID=:bID ");

            MyParams.Add(new Params(":bID", "GUID", bizID));
            MyParams.Add(new Params(":bStatus", "INT", 2));
            MyParams.Add(new Params(":bDeclinedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params(":bDeclinedBy", "GUID", adminGuid));

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

    public static bool deleteBizHour(Guid bizID)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("delete User_Branch");
            sql.AppendLine("where bID=:bID ");

            MyParams.Add(new Params(":bID", "GUID", bizID));

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

    #region update description management
    static List<string> beforeUpdate = new List<string>();
    static List<string> afterUpdate = new List<string>();

    protected static void beforeAudit(Branch item)
    {
        try
        {
            beforeUpdate.Clear();
            beforeUpdate.Add(item.TemplateName);
            beforeUpdate.Add(item.Monday.ToString());
            beforeUpdate.Add(item.Tuesday.ToString());
            beforeUpdate.Add(item.Wednesday.ToString());
            beforeUpdate.Add(item.Thursday.ToString());
            beforeUpdate.Add(item.Friday.ToString());
            beforeUpdate.Add(item.Saturday.ToString());
            beforeUpdate.Add(item.Sunday.ToString());
            string[] tmpstartTime = item.Starttime.ToString().Split(':');
            string[] tmpendTime = item.Endtime.ToString().Split(':');

            beforeUpdate.Add(tmpstartTime[0].ToString());
            beforeUpdate.Add(tmpstartTime[1].ToString());
            beforeUpdate.Add(tmpendTime[0].ToString());
            beforeUpdate.Add(tmpendTime[1].ToString());
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
    }

    protected static void afterAudit(Branch item)
    {
        try
        {
            afterUpdate.Clear();
            afterUpdate.Add(item.TemplateName);
            afterUpdate.Add(item.Monday.ToString());
            afterUpdate.Add(item.Tuesday.ToString());
            afterUpdate.Add(item.Wednesday.ToString());
            afterUpdate.Add(item.Thursday.ToString());
            afterUpdate.Add(item.Friday.ToString());
            afterUpdate.Add(item.Saturday.ToString());
            afterUpdate.Add(item.Sunday.ToString());
            string[] tmpstartTime = item.Starttime.ToString().Split(':');
            string[] tmpendTime = item.Endtime.ToString().Split(':');

            afterUpdate.Add(tmpstartTime[0].ToString());
            afterUpdate.Add(tmpstartTime[1].ToString());
            afterUpdate.Add(tmpendTime[0].ToString());
            afterUpdate.Add(tmpendTime[1].ToString());
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
    }

    protected static string CheckUpdated()
    {
        string ret = "";
        try
        {
            StringBuilder oldUpdateAudit = new StringBuilder();
            StringBuilder newUpdateAudit = new StringBuilder();

            string[] bHourContent = { "", "Working Day: Monday: ", "Working Day: Tuesday: ", "Working Day: Wednesday: ", "Working Day: Thursday: ", "Working Day: Friday: ", "Working Day: Saturday: ", "Working Day: Sunday: ", "Start Time: ", "", "End Time: ", "" };

            for (int i = 0; i < afterUpdate.Count; i++)
            {
                if (i == 0)
                {
                    if (beforeUpdate[i].ToString() != afterUpdate[i].ToString())
                    {
                        oldUpdateAudit.AppendLine("OLD: Business Operating Name: " + beforeUpdate[i].ToString());
                        newUpdateAudit.AppendLine("NEW: Business Operating Name: " + afterUpdate[i].ToString());
                    }
                }
                else if (bHourContent[i] == "Start Time: " || bHourContent[i] == "End Time: ")
                {
                    if (beforeUpdate[i].ToString() != afterUpdate[i].ToString()
                        || beforeUpdate[i + 1].ToString() != afterUpdate[i + 1].ToString())
                    {
                        oldUpdateAudit.AppendLine("OLD: " + bHourContent[i] + beforeUpdate[i].ToString() + ": " + beforeUpdate[i + 1].ToString());
                        newUpdateAudit.AppendLine("NEW: " + bHourContent[i] + afterUpdate[i].ToString() + ": " + afterUpdate[i + 1].ToString());
                        i++;
                    }
                }
                else if (beforeUpdate[i].ToString() != afterUpdate[i].ToString())
                {
                    oldUpdateAudit.AppendLine("OLD: " + bHourContent[i] + beforeUpdate[i].ToString());
                    newUpdateAudit.AppendLine("NEW: " + bHourContent[i] + afterUpdate[i].ToString());
                }
            }
            if (oldUpdateAudit.ToString() != "")
                ret = oldUpdateAudit.ToString() + "\n" + newUpdateAudit.ToString();
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
    #endregion

}