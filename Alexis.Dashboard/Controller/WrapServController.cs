using System.Data;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

public class WrapServController : GlobalController
{
    public bool isWrapServExist(WrapServType wsType, string main)
    {
        bool ret = false;
        try
        {
            sql.AppendLine("select m.*, sub.* ");
            sql.AppendLine("from Wrap_Serv_Main m ");
            sql.AppendLine("join Wrap_Serv_Sub sub on (m.wsID = sub.ref_wsID)");
            sql.AppendLine("where m.wsType = @type ");
            sql.AppendLine("and m.wsMain = @wsName ");
            MyParams.Add(new Params("@Type", "NVARCHAR", wsType));
            MyParams.Add(new Params("@wsName", "NVARCHAR", main));
            if (dbController.GetResult(sql.ToString(), "connectionString", MyParams).Rows.Count > 0)
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
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public DataTable getWrapServ(WrapServType wsType)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.AppendLine("select m.*, ");
            sql.AppendLine("case m.wsStatus when 0 then 'Pending' when 1 then 'Active' else 'Inactive' end txtStatus, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uName UpdatedBy ");
            sql.AppendLine("from Wrap_Serv_Main m ");
            //sql.AppendLine("join Wrap_Serv_Sub sub on (m.wsID = sub.ref_wsID)");
            sql.AppendLine("left join User_Login ulc on (m.wCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (m.wApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (m.wDeclineBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (m.wUpdatedBy = ulu.aID) ");
            sql.AppendLine("where m.wsType = @type ");
            MyParams.Add(new Params("@Type", "NVARCHAR", wsType));
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public DataTable getWrapServByID(WrapServType wsType, Guid wsID)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.AppendLine("select m.*, sub.*, ");
            sql.AppendLine("case m.wsStatus when 0 then 'Pending' when 1 then 'Active' else 'Inactive' end txtStatus, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy ");
            sql.AppendLine("from Wrap_Serv_Main m ");
            sql.AppendLine("left join Wrap_Serv_Sub sub on (m.wsID = sub.ref_wsID)");
            sql.AppendLine("left join User_Login ulc on (m.wCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (m.wApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (m.wDeclineBy = uld.aID) ");
            sql.AppendLine("where m.wsType = @type ");
            MyParams.Add(new Params("@Type", "NVARCHAR", wsType));
            //if (wsID != Guid.Empty)
            {
                sql.AppendLine("and m.wsID = @wsID ");
                MyParams.Add(new Params("@wsID", "GUID", wsID));
            }
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public DataTable getWrapServWithDetailByMainID(WrapServType wsType, Guid wsID)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.AppendLine("select m.*, sub.*, ");
            sql.AppendLine("case m.wsStatus when 0 then 'Pending' when 1 then 'Active' else 'Inactive' end txtStatus, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy ");
            sql.AppendLine("from Wrap_Serv_Main m ");
            sql.AppendLine("join Wrap_Serv_Sub sub on (m.wsID = sub.ref_wsID)");
            sql.AppendLine("left join User_Login ulc on (m.wCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (m.wApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (m.wDeclineBy = uld.aID) ");
            sql.AppendLine("where m.wsType = @type ");
            MyParams.Add(new Params("@Type", "NVARCHAR", wsType));
            //if (wsID != Guid.Empty)
            {
                sql.AppendLine("and m.wsID = @wsID ");
                MyParams.Add(new Params("@wsID", "GUID", wsID));
            }
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public DataTable getWrapServByDetailID(WrapServType wsType, Guid wsDetID)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.AppendLine("select m.*, sub.*, ");
            sql.AppendLine("case m.wsStatus when 0 then 'Pending' when 1 then 'Active' else 'Inactive' end txtStatus, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy ");
            sql.AppendLine("from Wrap_Serv_Main m ");
            sql.AppendLine("join Wrap_Serv_Sub sub on (m.wsID = sub.ref_wsID)");
            sql.AppendLine("left join User_Login ulc on (m.wCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (m.wApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (m.wDeclineBy = uld.aID) ");
            sql.AppendLine("where m.wsType = @type ");
            MyParams.Add(new Params("@Type", "NVARCHAR", wsType));
            if (wsDetID != Guid.Empty)
            {
                sql.AppendLine("and sub.wsID_Detail = @wsID_Detail ");
                MyParams.Add(new Params("@wsID_Detail", "GUID", wsDetID));
            }
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public bool insertWrapServ(AWrapServ entity)
    {
        bool ret = false;
        try
        {
            sql.AppendLine("insert into Wrap_Serv_Main ");
            sql.AppendLine("(WSID,WSMAIN,WSTYPE,WSREMARKS,WSSTATUS,WCREATEDBY,WCREATEDDATE)");
            sql.AppendLine("values ");
            sql.AppendLine("(@wsID, @wsMain, @wsType, @wsRemarks, @wsStatus, @wCreatedBy, @wCreatedDate) ");
            MyParams.Add(new Params("@wsID", "GUID", entity.WSID));
            MyParams.Add(new Params("@wsMain", "NVARCHAR", entity.WSMainName));
            MyParams.Add(new Params("@wsType", "NVARCHAR", entity.WSType));
            MyParams.Add(new Params("@wsRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@wsStatus", "INT", entity.Status));
            MyParams.Add(new Params("@wCreatedBy", "GUID", entity.CreatedBy));
            MyParams.Add(new Params("@wCreatedDate", "DATETIME", entity.CreatedDate));
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public bool insertWrapServDetail(AWrapServ entity)
    {
        bool ret = false;
        try
        {
            foreach (AWrapServ.WrapServDetails detail in entity.DetailList)
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("insert into Wrap_Serv_Sub (ref_wsID, wsID_Detail, wsDetail) ");
                sql.AppendLine("values ");
                sql.AppendLine("(@wsID, @wsID_Detail, @wsDetail) ");
                MyParams.Add(new Params("@wsID", "GUID", detail.Ref_wsID));
                MyParams.Add(new Params("@wsID_Detail", "GUID", detail.WSID_Detail));
                MyParams.Add(new Params("@wsDetail", "NVARCHAR", detail.WSDetailName));
                ret = dbController.Input(sql.ToString(), "connectionString", MyParams);

                if (!ret)
                    break;
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public bool updateWrapServ(AWrapServ entity)
    {
        bool ret = false;
        try
        {
            sql.AppendLine("update Wrap_Serv_Main ");
            sql.AppendLine("set ");
            sql.AppendLine("wsMain = @wsMain, wsRemarks = @wsRemarks, wsStatus = @wsStatus, wUpdatedBy = @wUpdatedBy, wUpdatedDate = @wUpdatedDate ");
            sql.AppendLine("where wsID = @wsID and wsType = @wsType ");
            MyParams.Add(new Params("@wsID", "GUID", entity.WSID));
            MyParams.Add(new Params("@wsMain", "NVARCHAR", entity.WSMainName));
            MyParams.Add(new Params("@wsRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@wsStatus", "INT", entity.Status));
            MyParams.Add(new Params("@wsType", "NVARCHAR", entity.WSType));
            MyParams.Add(new Params("@wUpdatedBy", "GUID", entity.UpdatedBy));
            MyParams.Add(new Params("@wUpdatedDate", "DATETIME", entity.UpdatedDate));
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public bool updateWrapServDetail(AWrapServ entity)
    {
        bool ret = false;
        try
        {
            sql.AppendLine("update Wrap_Serv_Sub set");
            sql.AppendLine("wsDetail = @wsDetail ");
            sql.AppendLine("where wsID_Detail = @wsID_Detail ");
            MyParams.Add(new Params("@wsDetail", "NVARCHAR", entity.Detail.WSDetailName));
            MyParams.Add(new Params("@wsID_Detail", "GUID", entity.Detail.WSID_Detail));
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public bool approveWrapServ(WrapServType type, Guid id, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            sql.AppendLine("update Wrap_Serv_Main ");
            sql.AppendLine("set ");
            sql.AppendLine("wsStatus = @wsStatus, wApprovedBy = @wApprovedBy, wApprovedDate = @wApprovedDate ");
            sql.AppendLine("where wsID = @wsID and wsType = @wsType ");
            MyParams.Add(new Params("@wsID", "GUID", id));
            MyParams.Add(new Params("@wsStatus", "INT", 1));
            MyParams.Add(new Params("@wsType", "NVARCHAR", type));
            MyParams.Add(new Params("@wApprovedBy", "GUID", adminGuid));
            MyParams.Add(new Params("@wApprovedDate", "DATETIME", DateTime.Now));
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public bool declineWrapServ(WrapServType type, Guid id, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            sql.AppendLine("update Wrap_Serv_Main ");
            sql.AppendLine("set ");
            sql.AppendLine("wsStatus = @wsStatus, wDeclineBy = @wDeclineBy, wDeclineDate = @wDeclineDate ");
            sql.AppendLine("where wsID = @wsID and wsType = @wsType ");
            MyParams.Add(new Params("@wsID", "GUID", id));
            MyParams.Add(new Params("@wsStatus", "INT", 2));
            MyParams.Add(new Params("@wsType", "NVARCHAR", type));
            MyParams.Add(new Params("@wApprovedBy", "GUID", adminGuid));
            MyParams.Add(new Params("@wApprovedDate", "DATETIME", DateTime.Now));
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }

    public bool deleteWrapServ(WrapServType type, Guid id, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            sql.AppendLine("delete Wrap_Serv_Main ");
            sql.AppendLine("where wsID = @wsID and wsType = @wsType ");
            MyParams.Add(new Params("@wsID", "GUID", id));
            MyParams.Add(new Params("@wsType", "NVARCHAR", type));
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
        finally
        {
            sql.Clear();
            MyParams.Clear();
        }
        return ret;
    }
}
