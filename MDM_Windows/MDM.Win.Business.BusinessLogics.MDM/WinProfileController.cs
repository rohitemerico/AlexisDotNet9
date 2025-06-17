using System.Data;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;

namespace MDM.Win.Business.BusinessLogics.MDM;

public class WinProfileController
{
    List<Params> MyParams = [];

    public DataTable GetAllMdmProfiles()
    {
        var query = @"select * from WinProfiles";
        MyParams.Clear();
        var dt = dbController.GetResult(query.ToString(), "connectionString", MyParams);
        return dt;
    }
}
