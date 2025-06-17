using System.Data;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace Alexis.Dashboard.Controller;

public class ReportingAITokenController : CommonController
{
    public DataTable GetAITokenSummary(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT U.UNAME, T.TokenType, SUM(T.TokenCount) AS TokenCount FROM AITOKENSUMMARY T ");
        sql.AppendLine("INNER JOIN USER_LOGIN U ON U.AID = T.USERID ");
        sql.AppendLine("WHERE DateUsed >= @minDate AND DateUsed <= @maxDate ");
        sql.AppendLine("GROUP BY U.UNAME, T.TokenType ");
        MyParams.Add(new Params("@minDate", "DATETIME", minDate));
        MyParams.Add(new Params("@maxDate", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }

    public DataTable GetAITokenDetails(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, U.UNAME FROM AITOKENSUMMARY T ");
        sql.AppendLine("INNER JOIN USER_LOGIN U ON U.AID = T.USERID ");
        sql.AppendLine("WHERE T.DateUsed >= @minDate AND T.DateUsed <= @maxDate");
        MyParams.Add(new Params("@minDate", "DATETIME", minDate));
        MyParams.Add(new Params("@maxDate", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }
}