using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

public class GroupMaintenanceController : GlobalController
{
    public static DataTable getUsers()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select ul.aid as aid,ul.uname as uname");
            sql.AppendLine("from user_login ul");
            sql.AppendLine("where ul.uStatus = 1");
            sql.AppendLine("order by ul.uName ");
            List<Params> MyParams = new List<Params>();

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getUsers.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable getAssignedGroups()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT mg.kDesc           AS description,  ");
            //sql.AppendLine("COUNT(DISTINCT(ug.aid)) AS total,  ");
            sql.AppendLine("mg.kCreatedDate         AS createddate, mg.kupdateddate,  ");
            sql.AppendLine("mg.kStatus, mg.kRemarks, ");
            sql.AppendLine("CASE mg.kstatus WHEN 0 THEN 'Pending' WHEN 1 THEN 'Active' WHEN 2 THEN 'Inactive' END AS status,  ");
            sql.AppendLine("mg.kid AS kid,  ");
            sql.AppendLine("mg.KSCREENBACKGROUND,  ");
            sql.AppendLine("maBack.aName as backgroundImage, ");
            //sql.AppendLine("ma.aID as advertID, ");
            //sql.AppendLine("ma.aDesc as advertisement,  ");
            sql.AppendLine("al.aID as alertID, ");
            sql.AppendLine("al.aDesc as alert, ");
            sql.AppendLine("mb.bID, ");
            sql.AppendLine("mb.bDesc as businesshour,  ");
            sql.AppendLine("md.dID, ");
            sql.AppendLine("md.dDesc as document, ");
            sql.AppendLine("mh.hID, ");
            sql.AppendLine("mh.hDesc as hopper, ");
            sql.AppendLine("ulc.uName CreatedBy,  ");
            sql.AppendLine("ula.uName ApprovedBy,  ");
            sql.AppendLine("uld.uName DeclinedBy, ulu.uname UpdatedBy ");
            //sql.AppendLine("FROM user_group ug  ");
            sql.AppendLine("FROM machine_group mg");// ON (ug.kID = mg.kid) ");
            sql.AppendLine("JOIN machine_alert al ON (mg.kAlertId = al.aID) ");
            sql.AppendLine("JOIN Machine_BusinessHour mb ON (mg.kBusinessHourId = mb.bID) ");
            sql.AppendLine("JOIN Machine_Document md ON (mg.kDocumentId = md.dID) ");
            sql.AppendLine("JOIN machine_hopper mh ON (mg.kHopperId = mh.hID) ");
            //sql.AppendLine("JOIN ADVERTISEMENT_GROUP ag on (mg.KID = ag.GroupId) ");
            //sql.AppendLine("JOIN Machine_Advertisement ma ON (ag.advId = ma.aID) ");
            sql.AppendLine("LEFT JOIN Machine_Advertisement maBack ON (mg.KSCREENBACKGROUND = maBack.aID) ");
            sql.AppendLine("LEFT JOIN User_Login ulc ON (mg.kCreatedBy = ulc.aID) ");
            sql.AppendLine("LEFT JOIN User_Login ula ON (mg.kApprovedBy = ula.aID) ");
            sql.AppendLine("LEFT JOIN User_Login uld ON (mg.kDeclineBy = uld.aID) ");
            sql.AppendLine("LEFT JOIN User_Login ulu ON (mg.kupdatedby = ulu.aID) ");
            sql.AppendLine("WHERE 1=1 ");
            //sql.AppendLine("GROUP BY  mg.kDesc, mg.kCreatedDate,  mg.kupdateddate,mg.kstatus, mg.kRemarks, mg.kid, ulc.uName, ula.uName, uld.uName, ulu.uname,   ");
            //sql.AppendLine("mg.KSCREENBACKGROUND,maBack.aName,");
            ////sql.AppendLine("ma.aID,ma.aDesc,");
            //sql.AppendLine("al.aID,al.aDesc, ");
            //sql.AppendLine("mb.bID,mb.bDesc,md.dID,md.dDesc,mh.hID,mh.hDesc  ");
            List<Params> MyParams = new List<Params>();

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getAssignedGroups.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable getAssignedGroupsByID(Guid id)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT mg.kDesc           AS description,  ");
            //sql.AppendLine("COUNT(DISTINCT(ug.aid)) AS total,  ");
            sql.AppendLine("mg.kCreatedDate         AS createddate,  ");
            sql.AppendLine("mg.kStatus, mg.kRemarks, ");
            sql.AppendLine("CASE mg.kstatus WHEN 0 THEN 'Pending' WHEN 1 THEN 'Active' WHEN 2 THEN 'Inactive' END AS status,  ");
            sql.AppendLine("mg.kid AS kid,  ");
            sql.AppendLine("mg.KSCREENBACKGROUND,  ");
            sql.AppendLine("maBack.aName as backgroundImage, ");
            sql.AppendLine("maBack.ARELATIVEPATHURL aDirectory,");
            sql.AppendLine("ma.aID as advertID, ");
            sql.AppendLine("ma.aDesc as advertisement,  ");
            sql.AppendLine("al.aID as alertID, ");
            sql.AppendLine("al.aDesc as alert, ");
            sql.AppendLine("mb.bID, ");
            sql.AppendLine("mb.bDesc as businesshour,  ");
            sql.AppendLine("md.dID, ");
            sql.AppendLine("md.dDesc as document, ");
            sql.AppendLine("mh.hID, ");
            sql.AppendLine("mh.hDesc as hopper, ");
            sql.AppendLine("ulc.uName CreatedBy,  ");
            sql.AppendLine("ula.uName ApprovedBy,  ");
            sql.AppendLine("uld.uName DeclinedBy, ");
            sql.AppendLine("ag.sequence SeqOrder ");
            //sql.AppendLine("FROM user_group ug  ");
            sql.AppendLine("From machine_group mg ");//mg ON (ug.kID = mg.kid) ");
            sql.AppendLine("JOIN machine_alert al ON (mg.kAlertId = al.aID) ");
            sql.AppendLine("JOIN Machine_BusinessHour mb ON (mg.kBusinessHourId = mb.bID) ");
            sql.AppendLine("JOIN Machine_Document md ON (mg.kDocumentId = md.dID) ");
            sql.AppendLine("JOIN machine_hopper mh ON (mg.kHopperId = mh.hID) ");
            sql.AppendLine("JOIN ADVERTISEMENT_GROUP ag on (mg.KID = ag.GroupId) ");
            sql.AppendLine("JOIN Machine_Advertisement ma ON (ag.advId = ma.aID) ");
            sql.AppendLine("LEFT JOIN Machine_Advertisement maBack ON (mg.KSCREENBACKGROUND = maBack.aID) ");
            sql.AppendLine("LEFT JOIN User_Login ulc ON (mg.kCreatedBy = ulc.aID) ");
            sql.AppendLine("LEFT JOIN User_Login ula ON (mg.kApprovedBy = ula.aID) ");
            sql.AppendLine("LEFT JOIN User_Login uld ON (mg.kDeclineBy = uld.aID) ");
            sql.AppendLine("WHERE 1=1 AND mg.kid = @kid ");
            //sql.AppendLine("GROUP BY  mg.kDesc, mg.kCreatedDate, mg.kstatus, mg.kRemarks, mg.kid, ulc.uName, ula.uName, uld.uName,  ");
            //sql.AppendLine("mg.KSCREENBACKGROUND,maBack.aName,maBack.aDirectory,ma.aID,ma.aDesc,al.aID,al.aDesc, ");
            //sql.AppendLine("mb.bID,mb.bDesc,md.dID,md.dDesc,mh.hID,mh.hDesc,ag.sequence  ");
            sql.AppendLine("order by ma.aDesc ");
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@kid", "GUID", id));

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getAssignedGroups.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable getOrderedAdvByGroupID(Guid id)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select madv.*, madv.adesc describe, ag.sequence seqOrder from ");
            sql.AppendLine("machine_advertisement madv left join advertisement_group ag on (madv.aid = ag.advid) ");
            sql.AppendLine("where AG.GROUPID = @kid and madv.aStatus = 1 ");
            sql.AppendLine("union all ");
            sql.AppendLine("select madv.*, madv.adesc describe, null seqOrder from ");
            sql.AppendLine("machine_advertisement madv ");
            sql.AppendLine("where madv.aStatus = 1 and madv.aisbackgroundimg = 0 and madv.aid not in ");
            sql.AppendLine("(select advid from advertisement_group where groupid = @kid) ");
            sql.AppendLine("order by seqorder, describe ");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@kid", "GUID", id));

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getAssignedGroups.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");

        }
        return ret;
    }

    public static DataTable getAdvByGroupID(Guid id)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            //sql.AppendLine("select madv.*, madv.adesc describe, ag.sequence seqOrder from ");
            //sql.AppendLine("machine_advertisement madv left join advertisement_group ag on (madv.aid = ag.advid) ");
            //sql.AppendLine("where AG.GROUPID = @kid and madv.aStatus = 1 ");
            //sql.AppendLine("union all ");
            sql.AppendLine("select madv.*, madv.adesc describe, null seqOrder from ");
            sql.AppendLine("machine_advertisement madv ");
            sql.AppendLine("where madv.aStatus = 1 and madv.aisbackgroundimg = 0 and madv.aid in ");
            sql.AppendLine("(select advid from advertisement_group where groupid = @kid) ");
            sql.AppendLine("order by seqorder, describe ");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@kid", "GUID", id));

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getAssignedGroups.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable getUserFromGroup(Guid kid)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select ul.aID as userid, ul.uName as username");
            sql.AppendLine("from User_Login ul, user_group ug");
            sql.AppendLine("where ul.aid = ug.aID");
            sql.AppendLine("and ug.kID = @kid");
            sql.AppendLine("order by ul.uName ");


            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@kid", "GUID", kid));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getUserFromGroup.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static bool isGroupAvailable(string templateName, Guid? id)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select * from machine_group where UPPER(kdesc) = @tname AND kid <> @kid");
            sql.AppendLine("where UPPER(kdesc) = @tName");
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@tName", "NVARCHAR", templateName.ToUpper()));
            MyParams.Add(new Params("@kid", "GUID", id));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count == 0)
            {
                ret = true;
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("isGroupAvailable.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
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
            sql.AppendLine("select * from machine m ");
            sql.AppendLine("join machine_group mg on (m.mGroupID = mg.kid) ");
            sql.AppendLine("where mg.kid = @id and m.mStatus = @status ");

            MyParams.Add(new Params("@id", "GUID", id));
            MyParams.Add(new Params("@status", "INT", 1));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            foreach (DataRow dr in dt.Rows)
                ret += "The Group is assigned to Machine, " + dr["mDesc"].ToString() + "! The Group Cannot be Inactive! <br />";
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

    /// <summary>
    /// gets hopper template thats approved
    /// </summary>
    /// <returns>hid,hdesc</returns>
    public static DataTable bindHopper()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select hid,hdesc");
            sql.AppendLine("from machine_hopper");
            sql.AppendLine("where hStatus = 1");

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
    /// <summary>
    /// get document template thats approved
    /// </summary>
    /// <returns>did,ddesc</returns>
    public static DataTable bindDoccument()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select did,ddesc");
            sql.AppendLine("from Machine_Document");
            sql.AppendLine("where dStatus  = 1");

            List<Params> MyParams = new List<Params>();

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("bindDocument.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }
    /// <summary>
    /// get alert template thats approved
    /// </summary>
    /// <returns>aid,adesc</returns>
    public static DataTable bindAlert()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select aid, adesc");
            sql.AppendLine("from machine_alert");
            sql.AppendLine("where aStatus = 1");

            List<Params> MyParams = new List<Params>();

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("bindAlert.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }
    /// <summary>
    /// gets the advertisement template thats approved
    /// </summary>
    /// <returns>aid,adesc</returns>
    public static DataTable bindAdvertisement()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select aid,adesc");
            sql.AppendLine("from Machine_Advertisement");
            sql.AppendLine("where astatus = 1 and AISBACKGROUNDIMG=0 order by adesc");

            List<Params> MyParams = new List<Params>();

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("bindAdvertisement.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }
    /// <summary>
    /// gets the operating hour template thats approved
    /// </summary>
    /// <returns>bi,bdesc</returns>
    public static DataTable bindOperatingHour()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select bid, bdesc");
            sql.AppendLine("from Machine_BusinessHour");
            sql.AppendLine("where bstatus = 1");

            List<Params> MyParams = new List<Params>();

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("bindOperatingHour.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static bool updateGroup(MGroup entity)
    {
        bool ret = false;
        //StringBuilder auditLogging = new StringBuilder();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update machine_group");
            sql.AppendLine("set kRemarks = @kRemarks, kstatus = @KSTATUS,");
            sql.AppendLine("KHOPPERID = @KHOPPERID, KDOCUMENTID = @KDOCUMENTID, KALERTID = @KALERTID, KBUSINESSHOURID = @KBUSINESSHOURID, KSCREENBACKGROUND = @KSCREENBACKGROUND, KUPDATEDBY = @KUPDATEDBY, KUPDATEDDATE = @KUPDATEDDATE ");
            sql.AppendLine("where kid = @kid");
            //auditLogging.AppendLine("Update Group " + entity.KDesc);
            //auditLogging.AppendLine("Old Value :");
            //auditLogging.AppendLine(OldValues.Value);
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@kid", "GUID", entity.KID));
            MyParams.Add(new Params("@KHOPPERID", "GUID", entity.KHopperID));
            MyParams.Add(new Params("@KDOCUMENTID", "GUID", entity.KDocumentID));
            MyParams.Add(new Params("@KALERTID", "GUID", entity.KAlertID));
            MyParams.Add(new Params("@KBUSINESSHOURID", "GUID", entity.KBusinessHourID));
            MyParams.Add(new Params("@KSCREENBACKGROUND", "GUID", entity.KScreenBackground.AID));
            MyParams.Add(new Params("@kRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@KSTATUS", "INT", entity.Status));
            MyParams.Add(new Params("@KUPDATEDBY", "GUID", entity.UpdatedBy));
            MyParams.Add(new Params("@KUPDATEDDATE", "DATETIME", entity.UpdatedDate));
            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
            {
                //sql.Clear();
                //MyParams.Clear();
                //sql.AppendLine("delete user_group where kid=@kid");
                //MyParams.Add(new Params("@kid", "GUID", entity.KID));
                //dbController.Input(sql.ToString(), "connectionString", MyParams);

                //auditLogging.AppendLine("New Value :");
                string newValue = string.Empty;
                //foreach (Guid item in (List<Guid>)entity.Items)
                //{
                //    sql.Clear();
                //    MyParams.Clear();
                //    sql.AppendLine("insert into user_group (kid,aid) values (@kid,@aid)");
                //    MyParams.Add(new Params("@kid", "GUID", entity.KID));
                //    MyParams.Add(new Params("@aid", "GUID", item));
                //    dbController.Input(sql.ToString(), "connectionString", MyParams);
                //}
                //auditLogging.AppendLine(newValue.Remove(newValue.Length - 1, 1));
                //ret = true;

                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("delete ADVERTISEMENT_GROUP where GroupId=@kid");
                MyParams.Add(new Params("@kid", "GUID", entity.KID));
                if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                {
                    int index = 0;
                    foreach (var listItem in entity.AdvIds)
                    {
                        sql.Clear();
                        MyParams.Clear();
                        sql.AppendLine("insert into advertisement_group (GROUPID, ADVID, SEQUENCE) values (@kid, @advid, @sequence)");
                        MyParams.Add(new Params("@kid", "GUID", entity.KID));
                        MyParams.Add(new Params("@advid", "GUID", Guid.Parse(listItem)));
                        MyParams.Add(new Params("@sequence", "INT", index++));
                        ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
                    }
                }

                if (new AdvertisementController().updateAdvert(entity.KScreenBackground))
                {
                    ret = new AdvertisementController().updateAdvertPackage(entity.KScreenBackground);
                }
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("updateGroup.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            //Logger.AuditLog(auditLogging.ToString(), Logger.ModuleName.Kiosk_Maintenance, Logger.ModuleAction.Update_Group_Template, entity.UpdatedBy, ret);
        }
        return ret;
    }

    public static bool createGroup(MGroup entity, string ClientIp)
    {
        bool ret = false;
        StringBuilder auditLogging = new StringBuilder();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into machine_group");
            sql.AppendLine(@"(kid,kdesc,
                KHOPPERID,KDOCUMENTID,KALERTID,KBUSINESSHOURID,KSCREENBACKGROUND,
                kcreateddate,kcreatedby,kRemarks,kstatus)");
            sql.AppendLine("values");
            sql.AppendLine(@"(@kid,@kdesc,
                @KHOPPERID,@KDOCUMENTID,@KALERTID,@KBUSINESSHOURID,@KSCREENBACKGROUND,
                @kcreateddate,@kcreatedby,@kRemarks,@kStatus)");
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@kid", "GUID", entity.KID));
            MyParams.Add(new Params("@kdesc", "NVARCHAR", entity.KDesc));
            MyParams.Add(new Params("@KHOPPERID", "GUID", entity.KHopperID));
            MyParams.Add(new Params("@KDOCUMENTID", "GUID", entity.KDocumentID));
            MyParams.Add(new Params("@KALERTID", "GUID", entity.KAlertID));
            MyParams.Add(new Params("@KBUSINESSHOURID", "GUID", entity.KBusinessHourID));
            MyParams.Add(new Params("@KSCREENBACKGROUND", "GUID", entity.KScreenBackground.AID));
            MyParams.Add(new Params("@kcreateddate", "DATETIME", entity.CreatedDate));
            MyParams.Add(new Params("@kcreatedby", "GUID", entity.CreatedBy));
            MyParams.Add(new Params("@kRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@kStatus", "INT", 0));//entity.Status));

            auditLogging.AppendLine("Create Group " + entity.KDesc);

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
            {
                //foreach (ListItem listItem in (ListItemCollection)entity.Items2)
                int index = 0;
                foreach (var listItem in entity.AdvIds)
                {
                    //if (listItem.Selected)
                    //{
                    sql.Clear();
                    MyParams.Clear();
                    sql.AppendLine("insert into advertisement_group (GROUPID, ADVID, SEQUENCE) values (@kid, @advid, @sequence) ");
                    MyParams.Add(new Params("@kid", "GUID", entity.KID));
                    MyParams.Add(new Params("@advid", "GUID", Guid.Parse(listItem)));
                    MyParams.Add(new Params("@sequence", "INT", index++));
                    ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
                    //}
                }

                ret = new AdvertisementController().insertAdvert(entity.KScreenBackground);
            }

            //if (ret)
            //{
            //    auditLogging.AppendLine("Add User :");
            //    string newValue = string.Empty;
            //    foreach (GridDataItem item in ((RadGrid)entity.Items).MasterTableView.Items)
            //    {
            //        CheckBox chk = (CheckBox)item["chbxSelection"].Controls[0];
            //        string gbc = item.GetDataKeyValue("aid").ToString();
            //        string username = item.GetDataKeyValue("uname").ToString();
            //        if (chk.Checked)
            //        {
            //            sql.Clear();
            //            MyParams.Clear();
            //            sql.AppendLine("insert into user_group (kid,aid) values (@kid,@aid)");
            //            MyParams.Add(new Params("@kid", "GUID", entity.KID));
            //            MyParams.Add(new Params("@aid", "GUID", Guid.Parse(gbc)));
            //            dbController.Input(sql.ToString(), "connectionString", MyParams);
            //            newValue += username + ",";

            //        }
            //    }
            //    auditLogging.AppendLine(newValue.Remove(newValue.Length - 1, 1));
            //    ret = true;
            //}
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("createGroup.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        finally
        {
            AuditLog.CreateAuditLog(auditLogging.ToString(), AuditCategory.Kiosk_Maintenance, ModuleLogAction.Create_Group_Template, entity.CreatedBy, ret, ClientIp);
        }
        return ret;
    }

    public static string getTemplateName(Guid kid)
    {
        string ret = string.Empty;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select kdesc");
            sql.AppendLine("from machine_group");
            sql.AppendLine("where kid=@kid");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@kid", "GUID", kid));
            ret = dbController.Output(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getTemplateName.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static bool approveGroup(Guid groupID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update Machine_Group set ");
            sql.AppendLine("kStatus=@kStatus, kApprovedDate=@kApprovedDate, kApprovedBy=@kApprovedBy");
            sql.AppendLine("where kID=@kID ");

            MyParams.Add(new Params("@kID", "GUID", groupID));
            MyParams.Add(new Params("@kStatus", "INT", 1));
            MyParams.Add(new Params("@kApprovedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@kApprovedBy", "GUID", adminGuid));

            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);

            if (ret)
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("select * ");
                sql.AppendLine("from machine_group mg ");
                sql.AppendLine("join machine_advertisement ma on (mg.kScreenBackground = ma.aID) ");
                sql.AppendLine("where mg.kID = @kID and ma.AIsBackgroundIMG = @AIsBackgroundIMG ");
                MyParams.Add(new Params("@kID", "GUID", groupID));
                MyParams.Add(new Params("@AIsBackgroundIMG", "BIT", true));
                DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
                if (dt.Rows.Count > 0)
                {
                    ret = new AdvertisementController().approveAdvert(Guid.Parse(dt.Rows[0]["kScreenBackground"].ToString()), adminGuid);
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

    public static bool declineGroup(Guid groupID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update Machine_Group set ");
            sql.AppendLine("kStatus=@kStatus, kDeclineDate=@kDeclineDate, kDeclineBy=@kDeclineBy");
            sql.AppendLine("where kID=@kID ");

            MyParams.Add(new Params("@kID", "GUID", groupID));
            MyParams.Add(new Params("@kStatus", "INT", 2));
            MyParams.Add(new Params("@kDeclineDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@kDeclineBy", "GUID", adminGuid));

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

    public static bool deleteGroup(Guid groupID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("delete Machine_Group ");
            sql.AppendLine("where kID=@kID ");

            MyParams.Add(new Params("@kID", "GUID", groupID));

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