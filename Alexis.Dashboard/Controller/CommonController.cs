using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace Alexis.Dashboard.Controller
{
    public class CommonController
    {
        protected StringBuilder sql = new StringBuilder();
        protected List<Params> MyParams = new List<Params>();

        public DataTable getBindMachineGroup()
        {
            DataTable ret = new DataTable();
            try
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("SELECT * ");
                sql.AppendLine("FROM Machine_Group mg ");
                sql.AppendLine("WHERE mg.kStatus = :kStatus ");
                MyParams.Add(new Params(":kStatus", "INT", 1));

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

        public DataTable getBindMachine()
        {
            DataTable ret = new DataTable();
            try
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("SELECT * ");
                sql.AppendLine("FROM Machine ma ");
                sql.AppendLine("WHERE ma.mStatus = :mStatus ");
                MyParams.Add(new Params(":mStatus", "INT", 1));

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

        public DataTable getBindComponents()
        {
            DataTable ret = new DataTable();
            try
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("SELECT * ");
                sql.AppendLine("FROM Machine_Components mc ");
                //sql.AppendLine("WHERE mc.Status = :mStatus ");
                //MyParams.Add(new Params(":mStatus", "NVARCHAR", ONLINE));

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

        public DataTable getBindAgent()
        {
            DataTable ret = new DataTable();
            try
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("SELECT * ");
                sql.AppendLine("FROM User_Login ul ");
                sql.AppendLine("WHERE ul.uStatus = :uStatus ");
                MyParams.Add(new Params(":uStatus", "INT", 1));

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

        public DataTable getBindTransactionType()
        {
            DataTable ret = new DataTable();
            try
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("SELECT * ");
                sql.AppendLine("FROM Transaction_Type tt ");

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
    }
}
