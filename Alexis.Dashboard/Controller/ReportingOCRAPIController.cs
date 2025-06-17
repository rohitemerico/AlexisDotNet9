using System.Data;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace Alexis.Dashboard.Controller;

public class ReportingOCRAPIController : CommonController
{
    public DataTable GetOCRAPISummary(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT U.UNAME, T.ApiCallType, SUM(T.TokenUsed) AS TokenUsed FROM OCRAPISummary T ");
        sql.AppendLine("INNER JOIN USER_LOGIN U ON U.AID = T.USERID ");
        sql.AppendLine("WHERE T.DateUsed >= @minDate AND T.DateUsed <= @maxDate");
        sql.AppendLine("GROUP BY U.UNAME, T.ApiCallType ");
        MyParams.Add(new Params("@minDate", "DATETIME", minDate));
        MyParams.Add(new Params("@maxDate", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }

    public DataTable GetOCRAPIDetails(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, U.UNAME  FROM OCRAPISummary T ");
        sql.AppendLine("INNER JOIN USER_LOGIN U ON U.AID = T.USERID ");
        sql.AppendLine("WHERE T.DateUsed >= @minDate AND T.DateUsed <= @maxDate");
        MyParams.Add(new Params("@minDate", "DATETIME", minDate));
        MyParams.Add(new Params("@maxDate", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }
}