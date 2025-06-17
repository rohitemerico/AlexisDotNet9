using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace Alexis.Dashboard.Controller;

public static class AuditLogController
{
    public static DataTable getBindGrid(DateTime startDate, DateTime endDate, Guid userId, Guid moduleId)
    {
        DataTable ret = new DataTable();
        try
        {
            List<Params> MyParams = new List<Params>();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select m.mDesc as module,at.aDate as audit_date, at.aid, at.aDesc as description,at.aAction as action,at.aStatus as status, at.aSourceIP as sourceIP, at.aDestinationIP as destinationIP, ul.uName as executed");
            sql.AppendLine(",CASE WHEN aStatus = 1 THEN 'Passed' ELSE 'Failed' END AS status2");
            sql.AppendLine("from audit_trail at");
            sql.AppendLine("left join menu m ON at.mid = m.mID");
            sql.AppendLine("left join user_login ul on at.userid = ul.aid");
            //sql.AppendLine("and at.aDate between :startDate");
            //sql.AppendLine("and :endDate");
            if (moduleId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                sql.AppendLine("and at.mID = :moduleId");
                MyParams.Add(new Params(":moduleId", "GUID", moduleId));
            }
            if (userId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                sql.AppendLine("and at.userid = :userID");
                MyParams.Add(new Params(":userID", "GUID", userId));
            }
            sql.AppendLine("where at.aDate >= :startDate AND at.aDate <= :endDate ");
            sql.AppendLine("order by at.aDate desc");
            MyParams.Add(new Params(":startDate", "DATETIME", startDate));
            MyParams.Add(new Params(":endDate", "DATETIME", endDate));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getBindGrid.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

}
