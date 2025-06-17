using System.Data;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

public class MasterSettingController : GlobalController
{
    public DataTable GetDTMasterSettings()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select PWORD from tblMasterSetting ");

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

    public AMasterSettings GetMasterSettings()
    {
        AMasterSettings ret = new AMasterSettings();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from tblMasterSetting ");

            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count > 0)
            {
                ret.MonthChequeMax = Convert.ToInt32(dt.Rows[0]["MonthChequeMax"].ToString());
                ret.VDNs = dt.Rows[0]["VDNs"].ToString();
                ret.English = dt.Rows[0]["English"].ToString();
                ret.Arabic = dt.Rows[0]["Arabic"].ToString();
                ret.PWord = dt.Rows[0]["PWord"].ToString();
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

    public bool SetMasterSettings(AMasterSettings entity)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update tblMasterSetting set ");
            sql.AppendLine("MonthChequeMax=:MonthChequeMax, VDNs=:VDNs, English=:English, Arabic=:Arabic, PWord=:PWord ");
            MyParams.Add(new Params(":MonthChequeMax", "INT", entity.MonthChequeMax));
            MyParams.Add(new Params(":VDNs", "NVARCHAR", entity.VDNs));
            MyParams.Add(new Params(":English", "NVARCHAR", entity.English));
            MyParams.Add(new Params(":Arabic", "NVARCHAR", entity.Arabic));
            MyParams.Add(new Params(":PWord", "NVARCHAR", entity.PWord));
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