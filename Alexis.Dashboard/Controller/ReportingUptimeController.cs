using System.Data;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace Alexis.Dashboard.Controller;

//Note: Downtime - Uptime = (total) Uptime_Seconds 
//Note: LastUptime - Downtime = (total) Downtime_Seconds 

public class ReportingUptimeController : CommonController
{
    public DataTable GetDeviceUptimeiOS(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, M.*  ");
        sql.AppendLine("FROM TBLMONITORINGDEVICES T ");
        sql.AppendLine("INNER JOIN TBLMACHINE  M ON T.TBLMACHINEMACHINESERIAL = M.MACHINESERIAL ");
        sql.AppendLine("where T.LASTCAPTURED >= @p__linq__0 ");
        sql.AppendLine("AND T.LASTCAPTURED <= @p__linq__1 ");
        sql.AppendLine("AND T.LASTUPTIME IS NOT NULL ");
        MyParams.Add(new Params("@p__linq__0", "DATETIME", minDate));
        MyParams.Add(new Params("@p__linq__1", "DATETIME", maxDate));

        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        //return LINQToDataTable.LINQResultToDataTable(query);

    }
    public DataTable GetDeviceUptimeAndroid(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, M.*  ");
        sql.AppendLine("FROM ANDROIDMDM_MONITORINGDEVICES T ");
        sql.AppendLine("INNER JOIN ANDROIDMDM_DEVICES  M ON T.DID = M.deviceMACAdd ");
        sql.AppendLine("where T.LASTCAPTURED >= @p__linq__0 ");
        sql.AppendLine("AND T.LASTCAPTURED <= @p__linq__1 ");
        sql.AppendLine("AND T.LASTUPTIME IS NOT NULL ");
        MyParams.Add(new Params("@p__linq__0", "DATETIME", minDate));
        MyParams.Add(new Params("@p__linq__1", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }


    public DataTable GetDeviceUptimeWindows(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, M.*  ");
        sql.AppendLine("FROM WinMONITORINGDEVICES T ");
        sql.AppendLine("INNER JOIN WinDevDetails  M ON T.SMBIOSSerialNumber = M.SMBIOSSerialNumber ");
        sql.AppendLine("where T.LASTCAPTURED >= @p__linq__0 ");
        sql.AppendLine("AND T.LASTCAPTURED <= @p__linq__1 ");
        sql.AppendLine("AND T.LASTUPTIME IS NOT NULL ");
        MyParams.Add(new Params("@p__linq__0", "DATETIME", minDate));
        MyParams.Add(new Params("@p__linq__1", "DATETIME", maxDate));

        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        //return LINQToDataTable.LINQResultToDataTable(query);

    }
}