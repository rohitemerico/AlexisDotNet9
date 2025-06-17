using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace Alexis.Dashboard.Controller;

public class UserRolePageLoad
{
    protected StringBuilder sql = new StringBuilder();
    protected List<Params> MyParams = new List<Params>();

    public DataTable GetListOfModule()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select mChild.mID, mChild.mDesc child, mParent.mDesc parent ");
            sql.AppendLine(", CONVERT(DECIMAL(1, 0), 0) mView, CONVERT(DECIMAL(1, 0), 0) mMaker, CONVERT(DECIMAL(1, 0), 0) mChecker");
            sql.AppendLine("from menu mChild, menu mParent ");
            sql.AppendLine("where mChild.mGroup = mParent.mGroup ");
            sql.AppendLine("and mParent.mType=:T1 and mChild.mType=:T2 and mChild.mGroup<>:T3 ");
            sql.AppendLine("order by mChild.mGroup, mChild.mSeq");

            MyParams.Add(new Params(":T1", "INT", 0));
            MyParams.Add(new Params(":T2", "INT", 1));
            MyParams.Add(new Params(":T3", "INT", 10));

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserRolePageLoad_GetListOfModule.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public DataTable RoleCreate_Load()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select UR.rID, UR.rDesc, UR.rRemarks, UR.rStatus, case ur.rStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end Status, ");
            sql.AppendLine("UR.rCreatedDate, UR.rApprovedDate, UR.rDeclineDate, ");
            sql.AppendLine("UR.rCreatedby, UR.rApprovedBy, UR.rDeclineBy ");
            sql.AppendLine(", UCreate.uName CreatedBy, UR.rUpdatedDate, uupdate.uname updatedby ");
            sql.AppendLine("from User_Roles UR ");
            sql.AppendLine("left join User_Login UCreate on (UCreate.aID = UR.rCreatedby)");
            sql.AppendLine("left join User_Login UUpdate on (UUpdate.aID = UR.rUpdatedby)");
            sql.AppendLine("order by UR.rDesc");

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("RoleCreate_PageLoad.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public DataTable UserCreate_Load2CY()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select UR.rdesc, UL.*, UCreate.uName CreatedByName, uupdate.uname UpdatedByName");
            sql.AppendLine("from User_Login UL left join User_Roles UR on (UL.rID = UR.rID) ");
            sql.AppendLine("left join User_Login UCreate on (UCreate.aID = UL.uCreatedBy)");
            sql.AppendLine("left join User_Login UUpdate on (UUpdate.aID = UL.uUpdatedBy)");
            //sql.AppendLine("where UR.rStatus=1");

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserCreate_PageLoad.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public DataTable UserRole_DDLLoad()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select UR.*");
            sql.AppendLine("from User_Roles UR");
            sql.AppendLine("where UR.rStatus=1");

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserCreate_PageLoad.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
}
