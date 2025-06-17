using System.Data;
using Alexis.Common;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Business.Component.Cryptography;
using Dashboard.Entities.ADCB;
using DbProviderHandler;
using Newtonsoft.Json;


namespace MDM.iOS.Common.Data.Component;
/// <summary>
/// This class is just to determine which db connection strings to use.
/// </summary>
public class SqlDataControl
{

    /// <summary>
    /// Retrieves a selection of large database records. 
    /// </summary>
    /// <param name="strCommand">sql query</param>
    /// <param name="MyParams">the list of parameters to be added in sql query to prevent injection</param>
    /// <returns>A DataTable of selected records</returns>
    public static DataTable GetResult(string strCommand, List<Params> MyParams)
    {
        DataTable ret = new DataTable();
        string connString;

        try
        {
            var connString_Enc = ConfigHelper.GetConnectionString("connectionString_iOS_MDM_staging_db");

            if (String.IsNullOrEmpty(connString_Enc))
            {
                connString_Enc = ConfigHelper.GetConnectionString("connectionString"); //default
                connString = MsgSec.DecryptString(connString_Enc);
            }
            else
                connString = connString_Enc;


            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;


            ret = SqlData.GetResult(strCommand, MyParams, connString);
        }
        catch (Exception ex)
        {

            ret = new DataTable();
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name,
                     strCommand, ex);
        }

        return ret;
    }


    /// <summary>
    /// This method is used to input and store data in the database based on the specified parameter of the 
    /// SQL query. 
    /// </summary>
    /// <param name="strCommand">sql query</param>
    /// <param name="MyParams">the list of parameters to be added in sql query to prevent injection</param>
    /// <returns>boolean value of true or false</returns>
    public static bool Input(string strCommand, List<Params> MyParams)
    {
        bool ret = false;
        string connString;

        try
        {
            var connString_Enc = ConfigHelper.GetConnectionString("connectionString_iOS_MDM_staging_db");

            if (String.IsNullOrEmpty(connString_Enc))
            {
                connString_Enc = ConfigHelper.GetConnectionString("connectionString"); //default
                connString = MsgSec.DecryptString(connString_Enc);
            }
            else
                connString = connString_Enc;


            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;


            ret = SqlData.Input(strCommand, MyParams, connString);
        }
        catch (Exception ex)
        {
            ret = false;

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                                strCommand, JsonConvert.SerializeObject(MyParams),
                                "Exception Error: " + ex.ToString());
        }

        return ret;
    }


    /// <summary>
    /// for any selection query, use this method
    /// </summary>
    /// <param name="strCommand">sql query</param>
    /// <param name="MyParams"></param>
    /// <returns>returns a plain string. if there is more than one row split with #NXLI# and if more than one column split with #SPLIT#</returns>
    public static string Output(string strCommand, List<Params> MyParams)
    {
        string ret = "";
        string connString;

        try
        {
            var connString_Enc = ConfigHelper.GetConnectionString("connectionString_iOS_MDM_staging_db");

            if (String.IsNullOrEmpty(connString_Enc))
            {
                connString_Enc = ConfigHelper.GetConnectionString("connectionString"); //default
                connString = MsgSec.DecryptString(connString_Enc);
            }
            else
                connString = connString_Enc;


            ConvertedQuery converted = DbConvert.ConvertOracleParam(strCommand, MyParams);
            strCommand = converted.StrCommand;
            MyParams = converted.ParameterList;


            ret = SqlData.Output(strCommand, MyParams, connString);
        }
        catch (Exception ex)
        {
            ret = "";

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                                strCommand, JsonConvert.SerializeObject(MyParams),
                                "Exception Error: " + ex.ToString());
        }

        return ret;
    }

}
