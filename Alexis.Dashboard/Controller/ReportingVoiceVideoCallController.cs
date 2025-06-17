using System.Data;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace Alexis.Dashboard.Controller;

public class ReportingVoiceVideoCallController : CommonController
{
    public DataTable VoiceVideoCallTransactionsDetails(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, U.UNAME  FROM VoiceVideoCallTransactions T ");
        sql.AppendLine("INNER JOIN USER_LOGIN U ON U.AID = T.USERID ");
        sql.AppendLine("WHERE CreatedAt >= @minDate AND CreatedAt <= @maxDate");
        MyParams.Add(new Params("@minDate", "DATETIME", minDate));
        MyParams.Add(new Params("@maxDate", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }

    public DataTable VoiceVideoCallTransactionsSummary(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT U.UNAME, T.CallType, SUM(T.DurationSeconds) AS DurationSeconds FROM VoiceVideoCallTransactions T ");
        sql.AppendLine("INNER JOIN USER_LOGIN U ON U.AID = T.USERID ");
        sql.AppendLine("WHERE CreatedAt >= @minDate AND CreatedAt <= @maxDate");
        sql.AppendLine("GROUP BY U.UNAME, T.CallType ");
        MyParams.Add(new Params("@minDate", "DATETIME", minDate));
        MyParams.Add(new Params("@maxDate", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }
}
