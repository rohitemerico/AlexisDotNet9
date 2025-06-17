using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace Alexis.Dashboard.Controller;

public class CheckerMakerController : CommonController
{
    public DataTable getCheckerMakerLog(DateTime minDate, DateTime maxDate)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            //sql.AppendLine("select * from ( ");
            sql.AppendLine("select te.*, m.mDesc, editUL.uName EditedName, checkUL.uName CheckedName, editUL.UFULLNAME EditedFName, checkUL.UFULLNAME CheckedFName from ");
            sql.AppendLine("tblEditing te ");
            sql.AppendLine("join Menu m on (te.mid = m.mid) ");
            sql.AppendLine("left join User_Login editUL on (te.editedBy = editUL.aID) ");
            sql.AppendLine("left join User_Login checkUL on (te.checkedBy = checkUL.aID) ");
            sql.AppendLine("where 1=1 ");
            sql.AppendLine("AND te.editedDate >= :editedDate AND te.editedDate <= :checkedDate ");
            sql.AppendLine("AND te.TBLNAME not in ('User_Roles', 'User_Login')");
            //sql.AppendLine("where idOriginal = :idOriginal and updateStatus = :updateStatus ");
            sql.AppendLine("order by editedDate desc ");
            //sql.AppendLine(") OneRow where ROWNUM <= 1 ");

            MyParams.Add(new Params(":editedDate", "DATETIME", minDate));
            MyParams.Add(new Params(":checkedDate", "DATETIME", maxDate));

            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count > 0)
                ret = dt.Select("", "editeddate desc").CopyToDataTable();
            else
                ret = dt;
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

    /* User Management */
    public DataTable getUserCheckerMakerLog(DateTime minDate, DateTime maxDate)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            //sql.AppendLine("select * from ( ");
            sql.AppendLine("select te.*, m.mDesc, editUL.uName EditedName, checkUL.uName CheckedName, editUL.UFULLNAME EditedFName, checkUL.UFULLNAME CheckedFName from ");
            sql.AppendLine("tblEditing te ");
            sql.AppendLine("join Menu m on (te.mid = m.mid) ");
            sql.AppendLine("left join User_Login editUL on (te.editedBy = editUL.aID) ");
            sql.AppendLine("left join User_Login checkUL on (te.checkedBy = checkUL.aID) ");
            sql.AppendLine("where 1=1 ");
            sql.AppendLine("AND te.editedDate >= :editedDate AND te.editedDate <= :checkedDate ");
            sql.AppendLine("AND te.TBLNAME in ('User_Roles', 'User_Login')");
            //sql.AppendLine("where idOriginal = :idOriginal and updateStatus = :updateStatus ");
            sql.AppendLine("order by editedDate desc ");
            //sql.AppendLine(") OneRow where ROWNUM <= 1 ");

            MyParams.Add(new Params(":editedDate", "DATETIME", minDate));
            MyParams.Add(new Params(":checkedDate", "DATETIME", maxDate));

            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count > 0)
                ret = dt.Select("", "editeddate desc").CopyToDataTable();
            else
                ret = dt;
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

    public string[] CompareAndReturnString(string oldValues, string newValues)
    {
        string[] ret = new string[4], advertisement = new string[2], usergroup = new string[2], address = new string[2], status = new string[2], remarks = new string[2];

        oldValues = oldValues.Replace("\t", "");
        newValues = newValues.Replace("\t", "");
        try
        {
            string[] tmpOld = System.Text.RegularExpressions.Regex.Split(oldValues.Trim(), "\r\n");
            string[] tmpNew = System.Text.RegularExpressions.Regex.Split(newValues.Trim(), "\r\n");

            tmpOld = RemoveDuplicates(tmpOld);
            tmpNew = RemoveDuplicates(tmpNew);

            ret[0] = oldValues;
            ret[1] = newValues;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }

        ret[0] = ret[0].Replace("#REMARKS#", "").Replace("#UPDATE#", "").Replace("#CREATE#", "");
        ret[1] = ret[1].Replace("#REMARKS#", "").Replace("#UPDATE#", "").Replace("#CREATE#", "");

        return ret;
    }

    public string[] RemoveDuplicates(string[] myList)
    {
        System.Collections.ArrayList newList = new System.Collections.ArrayList();

        foreach (string str in myList)
            if (!(newList.Contains("DDESC:") || newList.Contains("DSTATUS:")))
                newList.Add(str);
        return (string[])newList.ToArray(typeof(string));
    }

    #region Machine Checker Maker
    public static DataTable getAlertListCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select ma.*, ulc.uName CreatedName, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.UNAME updatedby, ");
            sql.AppendLine("case ma.aStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end AlertStatus ");
            sql.AppendLine("from Machine_Alert ma ");
            sql.AppendLine("left join User_Login ulc on (ma.aCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (ma.aApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (ma.aDeclineBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (ma.aUpdatedBy = ulu.aID) ");
            sql.AppendLine("where ma.AID IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'Machine_Alert' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
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
        return ret;
    }

    public static DataTable getAppListCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("Select fw.*, ");
            sql.AppendLine("case fw.agentFlag when 1 then 'Pilot' else 'UnPilot' end pilot, ");
            sql.AppendLine("case fw.status when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end fStatus ");
            sql.AppendLine("from tblFirmware fw ");
            sql.AppendLine("where fw.SYSID IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'tblFirmware' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");
            sql.AppendLine("order by fw.createdDateTime desc ");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
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

    public static DataTable getBusinessOperatingAllCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select bid,bdesc,bmonday,bTuesday,");
            sql.AppendLine("bWednesday,bThursday,bFriday,bSaturday,");
            sql.AppendLine("bSunday,bStartTime,bEndTime,bRemarks, ");
            sql.AppendLine("bCreatedDate, bApprovedDate, bDeclineDate, bupdateddate, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uName UpdatedBy, ");
            sql.AppendLine("case bstatus when 0 then 'Pending'");
            sql.AppendLine("when 1 then 'Active'");
            sql.AppendLine("else 'Inactive' end bStatus");
            sql.AppendLine("from Machine_BusinessHour mBiz ");
            sql.AppendLine("left join User_Login ulc on (mBiz.bCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (mBiz.bApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (mBiz.bDeclineBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (mBiz.bUpdatedBy = ulu.aID) ");
            sql.AppendLine("where mBiz.bid IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'Machine_BusinessHour' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
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

    public static DataTable getCardListCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select HC.*, ");
            sql.AppendLine("Case HC.cContactless when 0 then 'Dual Mode' when 1 then 'Contact Only' end txtContactless, ");
            sql.AppendLine("Case HC.cStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end txtStatus, ");
            sql.AppendLine("cUL.uName CreatedName, aUL.uName ApprovedName, dUL.uName DeclineName, uul.uname UpdatedName from Hopper_Card HC ");
            sql.AppendLine("left join User_Login cUL on (HC.cCreatedBy = cUL.aID) ");
            sql.AppendLine("left join User_Login aUL on (HC.cApprovedBy = aUL.aID) ");
            sql.AppendLine("left join User_Login dUL on (HC.cDeclineBy = dUL.aID) ");
            sql.AppendLine("left join User_Login uUL on (HC.cUpdatedBy = uul.aID) ");
            sql.AppendLine("where HC.CID IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'Hopper_Card' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
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

    public static DataTable bindMachineCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select mg.KSCREENBACKGROUND, ");
            sql.AppendLine("case m.mstatus when 0 then 'Pending' when 1 then 'Active' else 'Inactive' end mstatus, ");
            sql.AppendLine("m.mid as mid,m.mdesc as description, m.mKioskID, m.mRemarks, ");
            sql.AppendLine("m.mserial as serial,m.mCreatedDate as createddate, ");
            sql.AppendLine("m.maddress as address,m.mLatitude as latitude, ");
            sql.AppendLine("m.mLongitude as longitude, ");
            sql.AppendLine("al.aDesc as alert,mb.bDesc as businesshour, ");
            sql.AppendLine("md.dDesc as document,mg.kDesc as groupdesc,mh.hDesc as hopper, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uname UpdatedBy, m.lastupdateddate ");
            sql.AppendLine(", m.VTMVERSION ");
            sql.AppendLine("from machine m ");
            sql.AppendLine("join machine_group mg on (m.mGroupId = mg.kid) ");
            sql.AppendLine("join machine_alert al on (mg.kAlertId = al.aID) ");
            sql.AppendLine("join Machine_BusinessHour mb on (mg.kBusinessHourId = mb.bID) ");
            sql.AppendLine("join Machine_Document md on (mg.kDocumentId = md.dID) ");
            sql.AppendLine("join machine_hopper mh on (mg.kHopperId = mh.hID) ");
            sql.AppendLine("left join User_Login ulc on (m.mCreatedBy = ulc.aID)  ");
            sql.AppendLine("left join User_Login ula on (m.mApprovedBy = ula.aID)  ");
            sql.AppendLine("left join User_Login uld on (m.mDeclineBy = uld.aID)  ");
            sql.AppendLine("left join User_Login ulu on (mg.kUpdatedBy = ulu.aID)  ");
            sql.AppendLine("where m.mid IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'Machine' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
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

    public static DataTable getDocTypeAllCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select ds.*, ");//, dc.cdesc, ");
            sql.AppendLine("Case ds.dStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end txtStatus, ");
            sql.AppendLine("cUL.uName CreatedBy, aUL.uName ApprovedBy, dUL.uName DeclineBy, uul.uname UpdatedBy ");
            sql.AppendLine("from doctype_setup ds ");//join document_component dc on (ds.cComponentId = DC.CID) ");
            sql.AppendLine("left join User_Login cUL on (ds.dCreatedBy = cUL.aID) ");
            sql.AppendLine("left join User_Login aUL on (ds.dApprovedBy = aUL.aID) ");
            sql.AppendLine("left join User_Login dUL on (ds.dDeclineBy = dUL.aID) ");
            sql.AppendLine("left join User_Login uUL on (ds.dUpdatedBy = uUL.aID) ");
            sql.AppendLine("where ds.DID IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'DocType_Setup' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");
            sql.AppendLine("order by dCreatedDate ");
            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getDocSettingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable getDocSettingAllCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select md.*, mds.*, dt.dName, ");
            sql.AppendLine("case md.dStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end docStatus, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uname UpdatedBy ");
            sql.AppendLine("from machine_document md  ");
            sql.AppendLine("join machine_document_settings mds on (md.did = mds.ref_did) ");
            sql.AppendLine("left join doctype_setup dt on (mds.doctypeid = dt.did) ");
            sql.AppendLine("left join User_Login ulc on (md.dCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (md.dApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (md.dDeclineBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (md.dUpdatedBy = ulu.aID) ");
            sql.AppendLine("where md.did IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'Machine_Document' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getDocSettingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public static DataTable getAssignedGroupsCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("SELECT mg.kDesc AS description,  ");
            //sql.AppendLine("COUNT(DISTINCT(ug.aid)) AS total,  ");
            sql.AppendLine("mg.kCreatedDate AS createddate, mg.kupdateddate,  ");
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
            //sql.AppendLine("WHERE 1=1 ");
            sql.AppendLine("where 1=1 AND mg.kid IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'Machine_Group' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
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

    public static DataTable getHopperListCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select hSelect.*,cUL.uName cName,aUL.uName aName,dUL.uName dName, uul.uname  ");
            sql.AppendLine("from (  ");
            sql.AppendLine("select hID, hDesc, hRemarks,  ");
            sql.AppendLine("h1Temp, H1.cDesc HC1, h1Range, h1Mask,  ");
            sql.AppendLine("h2Temp, H2.cDesc HC2, h2Range, h2Mask,  ");
            sql.AppendLine("h3Temp, H3.cDesc HC3, h3Range, h3Mask,  ");
            sql.AppendLine("h4Temp, H4.cDesc HC4, h4Range, h4Mask,  ");
            sql.AppendLine("h5Temp, H5.cDesc HC5, h5Range, h5Mask,  ");
            sql.AppendLine("h6Temp, H6.cDesc HC6, h6Range, h6Mask,  ");
            sql.AppendLine("h7Temp, H7.cDesc HC7, h7Range, h7Mask,  ");
            sql.AppendLine("h8Temp, H8.cDesc HC8, h8Range, h8Mask,  ");
            sql.AppendLine("hCreatedDate, hApprovedDate, hDeclineDate, hupdateddate,  ");
            sql.AppendLine("hupdatedby, hCreatedBy, hApprvoedBy, hDeclineBy,  ");
            sql.AppendLine("case hStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end HStatus  ");
            sql.AppendLine("from Machine_Hopper MH  ");
            sql.AppendLine("LEFT JOIN Hopper_Card H1 ON MH.h1Temp = H1.cID  ");
            sql.AppendLine("LEFT JOIN Hopper_Card H2 ON MH.h2Temp = H2.cID  ");
            sql.AppendLine("LEFT JOIN Hopper_Card H3 ON MH.h3Temp = H3.cID  ");
            sql.AppendLine("LEFT JOIN Hopper_Card H4 ON MH.h4Temp = H4.cID  ");
            sql.AppendLine("LEFT JOIN Hopper_Card H5 ON MH.h5Temp = H5.cID  ");
            sql.AppendLine("LEFT JOIN Hopper_Card H6 ON MH.h6Temp = H6.cID  ");
            sql.AppendLine("LEFT JOIN Hopper_Card H7 ON MH.h7Temp = H7.cID  ");
            sql.AppendLine("LEFT JOIN Hopper_Card H8 ON MH.h8Temp = H8.cID  ");
            sql.AppendLine("where mh.hid IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'Machine_Hopper' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user))  ");
            sql.AppendLine("hSelect  ");
            sql.AppendLine("left join User_Login cUL on(hSelect.hCreatedBy = cUL.aID)  ");
            sql.AppendLine("left join User_Login aUL on(hSelect.hApprvoedBy = aUL.aID)  ");
            sql.AppendLine("left join User_Login dUL on(hSelect.hDeclineBy = dUL.aID)  ");
            sql.AppendLine("left join User_Login uUL on(hSelect.hupdatedby = uul.aID)  ");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
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
    #endregion

    #region User Checker Maker
    public static DataTable GetUserCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select ul.aid, ul.ufullname, ul.uname, ul.ucreateddate, ul.uapproveddate, ul.udeclinedate");
            sql.AppendLine(", ul.agentflag, ul.ufullname, ul.ucmid, ul.uupdateddate, ul.ulastlogindate, ul.usessionid, ul.sessionkey");
            sql.AppendLine(", ul.uremarks, ul.loginflag, ul.mcardreplenishment, ul.mchequereplenishment");
            sql.AppendLine(", ul.mconsumable, ul.msecurity, ul.mtroubleshoot ");
            sql.AppendLine(", ur.rDesc ");
            sql.AppendLine(", ulc.uName CreatedByName, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.UNAME UpdatedByName");
            sql.AppendLine(", case ul.uStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end Status ");
            sql.AppendLine("from User_Login ul ");
            sql.AppendLine("left join User_Roles ur on (ul.rid = ur.rid) ");
            sql.AppendLine("left join User_Login ulc on (ul.uCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (ul.uApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (ul.uDeclineBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (ul.uUpdatedBy = ulu.aID) ");
            sql.AppendLine("where ul.AID IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'User_Login' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
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

    public static DataTable GetRolesCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select ur.rid, ur.rdesc, ur.rcreateddate, ur.rapproveddate, ur.rdeclinedate, ur.rupdateddate, ur.rremarks");
            sql.AppendLine(", ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.UNAME updatedby");
            sql.AppendLine(", case ur.rStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end Status ");
            sql.AppendLine("from User_Roles ur ");
            sql.AppendLine("left join User_Login ulc on (ur.rCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (ur.rApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (ur.rDeclineBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (ur.rUpdatedBy = ulu.aID) ");
            sql.AppendLine("where ur.RID IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'User_Roles' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
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

    public static DataTable GetBranchCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select bid,bdesc,bmonday,bTuesday,");
            sql.AppendLine("bWednesday,bThursday,bFriday,bSaturday,");
            sql.AppendLine("bSunday,bOpenTime,bCloseTime,bRemarks, ");
            sql.AppendLine("bCreatedDate, bApprovedDate, bDeclinedDate, bupdateddate, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uName UpdatedBy, ");
            sql.AppendLine("case bstatus when 0 then 'Pending'");
            sql.AppendLine("when 1 then 'Active'");
            sql.AppendLine("else 'Inactive' end Status");
            sql.AppendLine("from User_Branch mBiz ");
            sql.AppendLine("left join User_Login ulc on (mBiz.bCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (mBiz.bApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (mBiz.bDeclinedBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (mBiz.bUpdatedBy = ulu.aID) ");
            sql.AppendLine("where mBiz.bid IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'User_Branch' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");

            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
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
    #endregion

    #region Content Checker Maker
    public static DataTable getAllAdvertisementCM(DataTable userDetails)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select ma.*, ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uname UpdatedBy, ");
            sql.AppendLine("case ma.aStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end AdvertStatus ");
            sql.AppendLine("from Machine_Advertisement ma ");
            sql.AppendLine("left join User_Login ulc on (ma.aCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (ma.aApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login ulu on (ma.aUpdatedBy = ulu.aID) ");
            sql.AppendLine("left join User_Login uld on (ma.aDeclineBy = uld.aID) ");
            //, Advertisement_Package aPack ");
            //sql.AppendLine("where ma.aID = aPack.Ref_advID ");
            //sql.AppendLine("where aIsBackgroundImg = @aIsBackgroundImg ");
            sql.AppendLine("where aIsBackgroundImg = @aIsBackgroundImg AND ma.AID IN ( Select IDORIGINAL from TBLEDITING where TBLNAME = 'Machine_Advertisement' AND (UPDATESTATUS = 'CREATED' OR UPDATESTATUS = 'EDITED') AND EDITEDBY != @user)");

            MyParams.Add(new Params("@aIsbackgroundImg", "BIT", 0));
            MyParams.Add(new Params("@user", "NVARCHAR", userDetails.Rows[0]["aid"].ToString()));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
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
    #endregion
}
