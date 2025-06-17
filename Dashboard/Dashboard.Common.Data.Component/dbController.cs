using System.Data;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using DbProviderHandler;
using Newtonsoft.Json;

namespace Dashboard.Common.Data.Component;

public class dbController
{
    /// <summary>
    /// Call method SqlData_Dashboard.GetResult() after handling logic for connection string. 
    /// </summary>
    /// <param name="strCommand"></param>
    /// <param name="dbname"></param>
    /// <param name="MyParams"></param>
    /// <returns></returns>
    public static DataTable GetResult(string strCommand, string dbname, List<Params> MyParams)
    {
        DataTable ret = new DataTable();
        try
        {
            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;

            ret = SqlData_Dashboard.GetResult(strCommand, dbname, MyParams);
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
    /// Call method SqlData_Dashboard.GetResult() after handling logic for connection string. 
    /// </summary>
    /// <param name="strCommand"></param>
    /// <param name="dbname"></param>
    /// <param name="MyParams"></param>
    /// <returns></returns>
    [Obsolete]
    public static string GetJsonResult(string strCommand, string dbname, List<Params> MyParams)
    {
        string json = null;
        try
        {
            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;

            json = SqlData_Dashboard.GetJsonResult(strCommand, dbname, MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name,
                     strCommand, ex);
        }

        return json;
    }

    public static bool Input(string strCommand, string dbname, List<Params> MyParams)
    {
        bool isSuccessful = false;

        try
        {
            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;

            isSuccessful = SqlData_Dashboard.Input(strCommand, dbname, MyParams);
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

    public static string Output(string strCommand, string dbname, List<Params> MyParams)
    {
        string data = string.Empty;
        try
        {
            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;

            data = SqlData_Dashboard.Output(strCommand, dbname, MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ',', strCommand, JsonConvert.SerializeObject(MyParams), "Exception Error: " + ex.ToString());
        }

        return data;
    }

    public static int Count(string strCommand, string dbname, List<Params> MyParams)
    {
        int ret = 0;
        try
        {
            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;

            ret = SqlData_Dashboard.Count(strCommand, dbname, MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                                strCommand, JsonConvert.SerializeObject(MyParams),
                                "Exception Error: " + ex.ToString());
        }

        return ret;
    }

}
