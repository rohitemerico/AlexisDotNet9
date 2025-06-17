using System.Data;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Newtonsoft.Json;

namespace Alexis.Dashboard.Controller;

public class MOBController : CommonController
{
    //all transaction types
    public List<string> GetAllTransactionTypes()
    {
        var query = @"SELECT DISTINCT TransDesc FROM tbl_app_mob_translog";
        sql.Clear();
        MyParams.Clear();
        var dt = dbController.GetResult(query.ToString(), "connectionString", MyParams);
        List<string> transTypes = new List<string>();
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                transTypes.Add(row["TransDesc"].ToString());
            }
        }
        return transTypes;
    }

    //all product transactions only
    public DataTable GetProductTransactions(DateTime minDate, DateTime maxDate)
    {
        var query = @"
                    SELECT 
                    t.TransStatus, t.TransDate, t.TransDesc, t.UserName , t.TblMachineMachineSerial , b.bDesc 
                    FROM   tbl_app_mob_translog AS t
                    LEFT OUTER JOIN tblMachine AS m ON t.TblMachineMachineSerial = m.MachineSerial
                    LEFT OUTER JOIN User_Branch AS b ON m.BranchId = b.bID
                    WHERE t.TransDate >= @p__linq__0 AND t.TransDate <= @p__linq__1 AND  LOWER(t.TransType) ='product'
                    ORDER BY t.TransDate DESC
                    ";
        sql.Clear();
        MyParams.Clear();
        MyParams.Add(new Params("@p__linq__0", "DATETIME", minDate));
        MyParams.Add(new Params("@p__linq__1", "DATETIME", maxDate));

        var dt = dbController.GetResult(query.ToString(), "connectionString", MyParams);

        //Status in String type
        dt.Columns.Add("tStatus", typeof(string));
        for (int i = 0; i < dt.Rows.Count; i++)
            dt.Rows[i]["tStatus"] = (dt.Rows[i]["TransStatus"].ToString() == "1") ? "Passed" : "Failed";

        return dt;

    }

    //transaction summary (group by MOB.TransDesc, Device.MachineName) mainly for dashboard, return jSON result
    #region Temporary Classes

    #endregion
    public string GetTransTypeSummaryByDays(int daysFromNow)
    {
        var query = @" 
                    SELECT MOB.TransDesc, Device.MachineName, 
                    COUNT(*) TotalTrans, 
                    COUNT(CASE WHEN TransStatus = 0 THEN 1 ELSE NULL END) AS TotalFailed, 
                    COUNT(CASE WHEN TransStatus = 1 THEN 1 ELSE NULL END) AS TotalPassed
                    FROM tbl_app_mob_translog MOB
                    LEFT JOIN tblMachine Device on Device.MachineSerial = MOB.TblMachineMachineSerial
                    WHERE MOB.TransDate BETWEEN (GETDATE() - @p__linq__0) AND GETDATE()
                    GROUP BY MOB.TransDesc, Device.MachineName ORDER BY MOB.TransDesc, Device.MachineName";

        sql.Clear();
        MyParams.Clear();
        MyParams.Add(new Params("@p__linq__0", "INT", daysFromNow));
        var dt = dbController.GetResult(query.ToString(), "connectionString", MyParams);
        //Initialize
        var transCollection = new List<Temp_TransObj>();
        var deviceCollection = new List<Temp_Device>();
        if (dt.Rows.Count == 0)
        {
            //No device data, only return all transaction types
            var typeList = GetAllTransactionTypes();
            foreach (var type in typeList)
            {
                var newTransObj = new Temp_TransObj()
                {
                    TransDesc = type,
                    Device = deviceCollection
                };
                transCollection.Add(newTransObj);
            }
        }
        else
        {
            //Initialize
            string transType = dt.Rows[0]["TransDesc"].ToString();
            //Iterate
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string currentTransType = dt.Rows[i]["TransDesc"].ToString();

                if (transType == currentTransType)
                {
                    var newDevice = new Temp_Device()
                    {
                        MachineName = dt.Rows[i]["MachineName"].ToString(),
                        TotalTrans = dt.Rows[i]["TotalTrans"].ToString(),
                        TotalFailed = dt.Rows[i]["TotalFailed"].ToString(),
                        TotalPassed = dt.Rows[i]["TotalPassed"].ToString()
                    };
                    deviceCollection.Add(newDevice); //keep adding different devices of the same trans type 
                }
                else
                {
                    var newTransObj = new Temp_TransObj()
                    {
                        TransDesc = transType,
                        Device = deviceCollection
                    };
                    transCollection.Add(newTransObj); //add trans obj for the prev device collection

                    transType = currentTransType; //new trans type

                    deviceCollection = new List<Temp_Device>(); //new device collection
                    var newDevice = new Temp_Device()
                    {
                        MachineName = dt.Rows[i]["MachineName"].ToString(),
                        TotalTrans = dt.Rows[i]["TotalTrans"].ToString(),
                        TotalFailed = dt.Rows[i]["TotalFailed"].ToString(),
                        TotalPassed = dt.Rows[i]["TotalPassed"].ToString()
                    };
                    deviceCollection.Add(newDevice);
                }
                if (i == dt.Rows.Count - 1)
                {
                    var newTransObj = new Temp_TransObj()
                    {
                        TransDesc = transType,
                        Device = deviceCollection
                    };
                    transCollection.Add(newTransObj); //add trans obj for the last device collection
                }
            }
        }
        var jSON = JsonConvert.SerializeObject(transCollection, Formatting.Indented);
        return jSON;
    }

    //all transaction summary (not grouped by type) mainly for dashboard, return jSON result
    public string GetAllTransSummaryByDays(int daysFromNow)
    {
        var query = @" 
                    SELECT Device.MachineName, 
                    COUNT(*) TotalTrans, 
                    COUNT(CASE WHEN TransStatus = 0 THEN 1 ELSE NULL END) AS TotalFailed, 
                    COUNT(CASE WHEN TransStatus = 1 THEN 1 ELSE NULL END) AS TotalPassed
                    FROM tbl_app_mob_translog MOB
                    LEFT JOIN tblMachine Device on Device.MachineSerial = MOB.TblMachineMachineSerial
                    WHERE MOB.TransDate BETWEEN (GETDATE() - @p__linq__0) AND GETDATE()
                    GROUP BY Device.MachineName";
        sql.Clear();
        MyParams.Clear();
        MyParams.Add(new Params("@p__linq__0", "INT", daysFromNow));
        var dt = dbController.GetResult(query.ToString(), "connectionString", MyParams);
        string jSON;
        if (dt.Rows.Count == 0)
            jSON = JsonConvert.SerializeObject(new Temp_Device(), Formatting.Indented);
        else
            jSON = JsonConvert.SerializeObject(dt, Formatting.Indented);
        return jSON;
    }

    private class Temp_TransObj
    {
        public string TransDesc { get; set; }
        public List<Temp_Device> Device { get; set; }
    }
    private class Temp_Device
    {
        public string MachineName { get; set; }
        public string TotalTrans { get; set; }
        public string TotalFailed { get; set; }
        public string TotalPassed { get; set; }
    }
}
