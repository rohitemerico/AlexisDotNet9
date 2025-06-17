using System.Data;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
namespace Alexis.Dashboard.Controller;

public class ReportController : CommonController
{
    public DataTable GetAppInstallationSummaryAndroid(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, M.* , A.APPLICATION_NAME as APPLICATION ");
        sql.AppendLine("FROM ANDROIDMDM_AppInstallationSummary T ");
        sql.AppendLine("INNER JOIN ANDROIDMDM_DEVICES  M ON T.deviceMACAdd = M.deviceMACAdd ");
        sql.AppendLine("INNER JOIN ANDROIDMDM_APPLICATION  A ON T.APPID = A.APPID ");
        sql.AppendLine("where T.CREATED_ON >= @p__linq__0 ");
        sql.AppendLine("AND T.CREATED_ON <= @p__linq__1 ");
        sql.AppendLine("AND T.CREATED_ON IS NOT NULL ");
        MyParams.Add(new Params("@p__linq__0", "DATETIME", minDate));
        MyParams.Add(new Params("@p__linq__1", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }
    public DataTable GetDeviceLocationSummaryAndroid(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, M.*  ");
        sql.AppendLine("FROM ANDROIDMDM_DeviceLocationSummary T ");
        sql.AppendLine("INNER JOIN ANDROIDMDM_DEVICES  M ON T.deviceMACAdd = M.deviceMACAdd ");
        sql.AppendLine("where T.SyncTIME >= @p__linq__0 ");
        sql.AppendLine("AND T.SyncTIME <= @p__linq__1 ");
        sql.AppendLine("AND T.SyncTIME IS NOT NULL ");
        MyParams.Add(new Params("@p__linq__0", "DATETIME", minDate));
        MyParams.Add(new Params("@p__linq__1", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }
    public DataTable GetDeviceLocationSummaryWindows(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, M.*  ");
        sql.AppendLine("FROM WIN_DeviceLocationSummary T ");
        sql.AppendLine("INNER JOIN WINDEVDETAILS  M ON T.SMBIOSSERIALNUMBER = M.SMBIOSSERIALNUMBER ");
        sql.AppendLine("where T.SyncTIME >= @p__linq__0 ");
        sql.AppendLine("AND T.SyncTIME <= @p__linq__1 ");
        sql.AppendLine("AND T.SyncTIME IS NOT NULL ");
        MyParams.Add(new Params("@p__linq__0", "DATETIME", minDate));
        MyParams.Add(new Params("@p__linq__1", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }
    public DataTable GetDeviceLocationSummaryiOS(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, M.*  ");
        sql.AppendLine("FROM MDM_DeviceLocationSummary T ");
        sql.AppendLine("INNER JOIN tblMachine  M ON T.MACHINEID = M.MachineUDID ");
        sql.AppendLine("where T.SyncTIME >= @p__linq__0 ");
        sql.AppendLine("AND T.SyncTIME <= @p__linq__1 ");
        sql.AppendLine("AND T.SyncTIME IS NOT NULL ");
        MyParams.Add(new Params("@p__linq__0", "DATETIME", minDate));
        MyParams.Add(new Params("@p__linq__1", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }
    public DataTable GetAppInstallationSummaryiOS(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, M.* , A.NAME as APPLICATION ");
        sql.AppendLine("FROM MDM_AppInstallationSummary T ");
        sql.AppendLine("INNER JOIN tblMachine  M ON T.MACHINEID = M.MachineUDID  ");
        sql.AppendLine("INNER JOIN MDM_APP  A ON T.APPID = A.APPID ");
        sql.AppendLine("where T.CREATEDDATE >= @p__linq__0 ");
        sql.AppendLine("AND T.CREATEDDATE <= @p__linq__1 ");
        sql.AppendLine("AND T.CREATEDDATE IS NOT NULL ");
        MyParams.Add(new Params("@p__linq__0", "DATETIME", minDate));
        MyParams.Add(new Params("@p__linq__1", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }
    public DataTable GetAppInstallationSummaryWindows(DateTime minDate, DateTime maxDate)
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT T.*, M.* , A.ApplicationName as APPLICATION ");
        sql.AppendLine("FROM Win_AppInstallationSummary T ");
        sql.AppendLine("INNER JOIN WINDEVDETAILS  M ON T.SMBIOSSERIALNUMBER = M.SMBIOSSERIALNUMBER  ");
        sql.AppendLine("INNER JOIN WinApp  A ON T.APPID = A.ID ");
        sql.AppendLine("where T.CREATEDDATE >= @p__linq__0 ");
        sql.AppendLine("AND T.CREATEDDATE <= @p__linq__1 ");
        sql.AppendLine("AND T.CREATEDDATE IS NOT NULL ");
        MyParams.Add(new Params("@p__linq__0", "DATETIME", minDate));
        MyParams.Add(new Params("@p__linq__1", "DATETIME", maxDate));
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }
    public DataTable GetDeviceDataUsageiOS()
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT ");
        sql.AppendLine("d.MachineName, d.MachineSerial, ");
        sql.AppendLine("du.MOBILE_DATA_USED, du.WIFI_DATA_USED, ");
        sql.AppendLine("(du.MOBILE_DATA_USED + du.WIFI_DATA_USED) as TOTAL_DATA_USED, ");
        sql.AppendLine("du.LAST_UPDATED ");
        sql.AppendLine("FROM ");
        sql.AppendLine("tblMachine d ");
        sql.AppendLine("LEFT JOIN MDM_DEVICE_DATA_USAGE du ON d.MachineUDID = du.MachineUDID ");
        sql.AppendLine("ORDER BY du.LAST_UPDATED DESC");
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);

    }
    public DataTable GetDeviceDataUsageWindows()
    {
        sql.Clear();
        MyParams.Clear();
        sql.AppendLine("SELECT ");
        sql.AppendLine("d.DNSComputerName, d.SMBIOSSerialNumber, ");
        sql.AppendLine("du.MOBILE_DATA_USED, du.WIFI_DATA_USED, ");
        sql.AppendLine("(du.MOBILE_DATA_USED + du.WIFI_DATA_USED) as TOTAL_DATA_USED, ");
        sql.AppendLine("du.LAST_UPDATED ");
        sql.AppendLine("FROM ");
        sql.AppendLine("WinDevDetails d ");
        sql.AppendLine("LEFT JOIN WIN_DEVICE_DATA_USAGE du ON d.SMBIOSSerialNumber = du.SMBIOSSerialNumber ");
        sql.AppendLine("ORDER BY du.LAST_UPDATED DESC");
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);

    }
    public DataTable Get_Android_DeviceComponent(DateTime minDate, DateTime maxDate)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            string sqlStr = @"SELECT mc.*, ma.deviceName, mCom.ComponentMISC Component 
            FROM [ANDROIDMDM_MONITORINGCOMPONENT] mc 
            JOIN AndroidMDM_Devices ma ON mc.deviceMACAdd = ma.deviceMACAdd 
            JOIN AndroidMDM_Device_Components mCom on mc.componentID = mCom.componentID and mc.deviceMACAdd = mCom.deviceMACAdd 
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
    public DataTable Get_Windows_DeviceComponent(DateTime minDate, DateTime maxDate)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            string sqlStr = @"SELECT mc.*, m.DNSComputerName, mCom.ComponentMISC Component 
            FROM [Win_MONITORINGCOMPONENT] mc 
            JOIN WinDevDetails  M ON mc.SMBIOSSerialNumber = M.SMBIOSSerialNumber 
            JOIN Win_Device_Components mCom on mc.componentID = mCom.componentID and mc.SMBIOSSerialNumber = mCom.SMBIOSSerialNumber 
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
}