using System.Data;
using Alexis.Common;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Business.Component.Cryptography;
using Dashboard.Entities.ADCB;
using DbProviderHandler;
using Newtonsoft.Json;

namespace MDM.Android.Common.Data.Component;

public class SqlDataCtrl_Android
{
    /// <summary>
    /// Call method SqlData_Android.GetResult() after handling logic for connection string. 
    /// </summary>
    /// <param name="strCommand"></param>
    /// <param name="MyParams"></param>
    /// <returns></returns>
    public static DataTable GetResult(string strCommand, List<Params> MyParams)
    {
        DataTable ret = new DataTable();
        string connString;

        try
        {
            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;

            connString = MsgSec.DecryptString(ConfigHelper.GetConnectionString("connectionString"));

            ret = SqlData_Android.GetResult(strCommand, MyParams, connString);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name,
                     strCommand, ex);
        }

        return ret;
    }


    /// <summary>
    /// Call method SqlData_Android.Input() after handling logic for connection string. 
    /// </summary>
    /// <param name="strCommand"></param>
    /// <param name="MyParams"></param>
    /// <returns></returns>
    public static bool Input(string strCommand, List<Params> MyParams)
    {
        bool isSuccessful = false;
        string connString;

        try
        {
            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;

            connString = MsgSec.DecryptString(ConfigHelper.GetConnectionString("connectionString"));

            isSuccessful = SqlData_Android.Input(strCommand, MyParams, connString);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                                strCommand, JsonConvert.SerializeObject(MyParams),
                                "Exception Error: " + ex.ToString());
        }

        return isSuccessful;
    }

    public static int InputReturnId(string strCommand, List<Params> MyParams)
    {
        int isSuccessful = 0;
        string connString;

        try
        {
            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;

            connString = MsgSec.DecryptString(ConfigHelper.GetConnectionString("connectionString"));

            isSuccessful = SqlData_Android.InputReturnId(strCommand, MyParams, connString);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                                strCommand, JsonConvert.SerializeObject(MyParams),
                                "Exception Error: " + ex.ToString());
        }

        return isSuccessful;
    }
}
