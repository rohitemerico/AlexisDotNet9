using System.Data;
using System.Text;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace MDM.Win.Business.BusinessLogics.MDM;

public class WinDeviceController
{
    List<Params> MyParams = new List<Params>();

    public DataTable GetAllClientDevices()
    {
        var query = @"select * from WinDevs , WinDevDetails, WinDevStatuses, WinDevLocations";
        MyParams.Clear();
        var dt = dbController.GetResult(query.ToString(), "connectionString", MyParams);
        return dt;
    }

    public int GetDeviceCntByStatus(bool isOnline)
    {
        var query = @"
                        SELECT d.*, dd.*, ds.*, dl.*
                        FROM WinDevs d
                        INNER JOIN WinDevDetails dd ON d.ID = dd.WinDevID
                        INNER JOIN WinDevStatuses ds ON d.ID = ds.WinDevID
                        INNER JOIN WinDevLocations dl ON d.ID = dl.WinDevID
                        WHERE d.ID IN (
                        SELECT en.Source_DevID
                        FROM WinEnrollments en
                        WHERE en.EnrollStatus = 1
                        )
                        AND ds.IsOnline = @p__linq__0
                        ORDER BY d.Name DESC";
        MyParams.Clear();
        MyParams.Add(new Params("@p__linq__0", "BIT", isOnline));
        var dt = dbController.GetResult(query.ToString(), "connectionString", MyParams);
        return dt.Rows.Count;

    }

    public static DataTable GetWindowsOnOffCount(bool ToGetOnline)
    {
        StringBuilder sql = new();
        List<Params> myParams = [];
        DataTable ret = new DataTable();
        sql.AppendLine(@"select count(IsOnline) as count_ from WinDevStatuses where");

        if (ToGetOnline)
            sql.AppendLine("IsOnline = '1'");
        else
            sql.AppendLine("IsOnline = '0'");
        ret = dbController.GetResult(sql.ToString(), "connectionString", myParams);
        return ret;
    }

}
