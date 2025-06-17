using System.Data;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace Alexis.Dashboard.Controller;

public class MachineComponentController : CommonController
{

    public DataTable Get_iOS_DeviceComponent(DateTime minDate, DateTime maxDate)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            string sqlStr = @"SELECT mc.*, ma.MachineName MachineName, mCom.ComponentMISC Component 
            FROM tblMonitoringComponent mc 
            JOIN tblMachine ma ON mc.mserial = ma.MachineSerial 
            JOIN Machine_Components mCom on mc.componentID = mCom.componentID and mc.mSerial = mCom.mSerial 
            WHERE mc.uptime is not null 
            AND mc.alerttime >= @alerttime AND mc.uptime <= @uptime ";
            sql.AppendLine(sqlStr);
            MyParams.Add(new Params("@alerttime", "DATETIME", minDate));
            MyParams.Add(new Params("@uptime", "DATETIME", maxDate));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public DataTable getMachineComponent(string group, string machine, string location, DateTime minDate, DateTime maxDate)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("SELECT mc.*, ma.mDesc MachineName, mCom.ComponentMISC Component ");
            sql.AppendLine("FROM tblMonitoringComponent mc ");
            sql.AppendLine("JOIN machine ma ON mc.mserial = ma.mserial ");
            sql.AppendLine("JOIN machine_group mg on ma.mGroupID = mg.kID ");
            sql.AppendLine("JOIN Machine_Components mCom on mc.componentID = mCom.componentID and mc.mSerial = mCom.mSerial ");
            sql.AppendLine("WHERE mc.uptime is not null ");
            sql.AppendLine("AND mc.alerttime >= @alerttime AND mc.uptime <= @uptime ");



            MyParams.Add(new Params("@alerttime", "DATETIME", minDate));
            MyParams.Add(new Params("@uptime", "DATETIME", maxDate));

            //if (group != "%")
            //{
            //    sql.AppendLine("AND mg.kID = @kID");
            //    MyParams.Add(new Params("@kID", "GUID", group));
            //}
            //if (machine != "%")
            //{
            //    sql.AppendLine("AND ma.mID = @mID");
            //    MyParams.Add(new Params("@mID", "GUID", machine));
            //}
            //if (location != "")
            //{
            //    sql.AppendLine("AND ma.mAddress like N'%' || @address || '%'");
            //    MyParams.Add(new Params("@address", "NVARCHAR", location));
            //}

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

    public DataTable getMachineComponentByID(string id)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("SELECT mc.*, ma.mDesc MachineName, mCom.ComponentMISC Component ");
            sql.AppendLine("FROM tblMonitoringComponent mc ");
            sql.AppendLine("JOIN machine ma ON mc.mserial = ma.mserial ");
            sql.AppendLine("JOIN machine_group mg on ma.mGroupID = mg.kID ");
            sql.AppendLine("JOIN Machine_Components mCom on mc.componentID = mCom.componentID and mc.mSerial = mCom.mSerial ");
            sql.AppendLine("WHERE mc.transID = @transID ");

            MyParams.Add(new Params("@transID", "GUID", id)); //GUIDCHANGE

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

    public string getMachineComponentString(string group, string machine, string location, DateTime minDate, DateTime maxDate)
    {
        string ret = "";
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("SELECT mc.*, ma.mDesc MachineName, mCom.ComponentMISC Component ");
            sql.AppendLine("FROM tblMonitoringComponent mc ");
            sql.AppendLine("JOIN machine ma ON mc.mserial = ma.mserial ");
            sql.AppendLine("JOIN machine_group mg on ma.mGroupID = mg.kID ");
            sql.AppendLine("JOIN Machine_Components mCom on mc.componentID = mCom.componentID and mc.mSerial = mCom.mSerial ");
            sql.AppendLine("WHERE mc.uptime is not null ");
            sql.AppendLine("AND mc.alerttime >= '" + minDate + "' AND mc.uptime <= '" + maxDate + "' ");


            //if (group != "%")
            //{
            //    sql.AppendLine("AND mg.kID = '" + group + "'");
            //}
            //if (machine != "%")
            //{
            //    sql.AppendLine("AND ma.mID = '" + machine + "'");
            //}
            //if (location != "")
            //{
            //    sql.AppendLine("AND ma.mAddress like N'%" + location + "%'");
            //}

            ret = sql.ToString();
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