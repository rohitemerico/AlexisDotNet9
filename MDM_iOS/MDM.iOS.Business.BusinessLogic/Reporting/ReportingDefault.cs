using System.Data;
using System.Reflection;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Common.Data.Component;

namespace MDM.iOS.Business.BusinessLogic.Reporting;

public class ReportingDefault : ReportingBase
{
    public override void Dispose() { }
    public override DataTable GetAllDevices_Report()
    {
        DataTable query_dt = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"SELECT M.MachineUDID, M.MachineName, M.MachineImei, M.MachineSerial, M.BuildVersion, M.OSVersion, M.ModelName,
                                    M.MachineStatus, M.AvailableDevice_Capacity, M.DeviceCapacity, 
                                    M.PasscodePresent, M.PasscodeCompliant, M.PasscodeCompliantWithProfiles, 
                                    M.WifiMAC, M.BluetoothMAC, M.IsRoaming, M.isSupervised,
                                    M.FirmwareVersion, M.FirmwareBatteryStatus, M.FirmwareBatteryLevel, M.FirmwareSerial, 
                                    UB.bDesc,
                                    CONCAT(MD.Latitude,',',MD.Longitude) as Location
                                    FROM tblMachine M
                                    JOIN User_Branch UB ON UB.bid = M.BranchId
                                    LEFT JOIN tblMachineDetails MD ON MD.MachineSerial = M.MachineSerial
                                    LEFT JOIN MDM_Enrollment En ON En.UDID = M.MachineUDID
                                    WHERE (MachineStatus= 1 OR MachineStatus = 0) 
                                    ORDER BY M.MachineName, UB.bDesc ");

            query_dt = SqlDataControl.GetResult(sql.ToString(), new List<Params>());

        }
        catch (Exception ex)
        {
            Logger.LogToFile(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
        }
        return query_dt;
    }
    public override DataTable GetDeviceCert_Report(string deviceUDID)
    {
        DataTable query_dt = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine(@"SELECT CERT.CertName, CERT.IsIdentityCert
                                FROM MDM_Certificates CERT 
                                JOIN tblMachine M ON M.MachineUDID =  CERT.UDID
                                WHERE CERT.UDID=@machineUDID
                                ");

            List<Params> myParams = new List<Params>();
            myParams.Add(new Params("@machineUDID", "NVARCHAR", deviceUDID));

            query_dt = SqlDataControl.GetResult(sql.ToString(), myParams);

        }
        catch (Exception ex)
        {
            Logger.LogToFile(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
        }
        return query_dt;
    }
    public override DataTable GetAllDeviceGroups_Report() { return null; }
    public override DataTable GetAllApp_Report() { return null; }
    public override DataTable GetAllProfiles_Report() { return null; }

}
