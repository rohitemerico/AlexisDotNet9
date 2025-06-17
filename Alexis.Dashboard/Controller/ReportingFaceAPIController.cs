using System.Data;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace Alexis.Dashboard.Controller;

public class ReportingFaceAPIController : CommonController
{
    public DataTable GetFaceAPISummary(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT U.UNAME, T.ApiCallType, SUM(T.TokenUsed) AS TokenCount FROM FaceAPISummary T ");
        sql.AppendLine("INNER JOIN USER_LOGIN U ON U.AID = T.USERID ");
        sql.AppendLine("WHERE T.DateUsed >= @minDate AND T.DateUsed <= @maxDate");
        sql.AppendLine("GROUP BY U.UNAME, T.ApiCallType ");
        MyParams.Add(new Params("@minDate", "DATETIME", minDate));
        MyParams.Add(new Params("@maxDate", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }

    public DataTable GetFaceAPIDetails(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, U.UNAME  FROM FaceAPISummary T ");
        sql.AppendLine("INNER JOIN USER_LOGIN U ON U.AID = T.USERID ");
        sql.AppendLine("WHERE T.DateUsed >= @minDate AND T.DateUsed <= @maxDate");
        MyParams.Add(new Params("@minDate", "DATETIME", minDate));
        MyParams.Add(new Params("@maxDate", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }
}