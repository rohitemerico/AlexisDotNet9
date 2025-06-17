using System.Data;
using System.Text.RegularExpressions;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using Microsoft.Data.SqlClient;

namespace MDM.Android.Common.Data.Component;

public class SqlData_Android
{
    /// <summary>
    /// Fetch/read data.
    /// </summary>
    /// <param name="strCommand">SQL Query</param>
    /// <param name="MyParams">List of parameters to be added in sql query to prevent injection.</param>
    /// <param name="connString">Connection String</param>
    /// <returns></returns>
    public static DataTable GetResult(string strCommand, List<Params> MyParams, string connString)
    {
        DataTable dt = new DataTable();
        try
        {
            using SqlConnection myConn = new SqlConnection(connString);
            myConn.Open();

            using SqlCommand myCmd = new SqlCommand(strCommand, myConn);
            for (int x = 0; x < MyParams.Count; x++)
                GetCommand(MyParams[x].dataName, MyParams[x].dataType, MyParams[x].dataValue, myCmd);

            //myCmd.CommandTimeout = 180;

            using SqlDataAdapter sda = new SqlDataAdapter(myCmd);
            sda.Fill(dt);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        strCommand,
                        ex);
        }

        return dt;
    }


    /// <summary>
    /// Put/write data in the db.
    /// </summary>
    /// <param name="strCommand">SQL Query</param>
    /// <param name="MyParams">List of parameters to be added in sql query to prevent injection.</param>
    /// <param name="connString">Connection String</param>
    /// <returns>Action successful (true) or failed (false)</returns>
    public static bool Input(string strCommand, List<Params> MyParams, string connString)
    {
        bool ret = false;
        string[] strCmd = Regex.Split(strCommand, "#SPLIT#");
        try
        {
            while (true)
            {
                using (SqlConnection myconn = new SqlConnection(connString))
                {
                    myconn.Open();
                    for (int i = 0; i < strCmd.Length; i++)
                    {

                        using SqlCommand mycmd = new SqlCommand(strCmd[i], myconn);
                        for (int x = 0; x < MyParams.Count; x++)
                            GetCommand(MyParams[x].dataName, MyParams[x].dataType, MyParams[x].dataValue, mycmd);

                        mycmd.ExecuteNonQuery();
                    }
                }
                ret = true;
                break;
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        "Apple: " + strCommand, ex);
        }

        return ret;
    }

    public static int InputReturnId(string strCommand, List<Params> MyParams, string connString)
    {
        int ret = 0;
        string[] strCmd = Regex.Split(strCommand, "#SPLIT#");
        try
        {
            while (true)
            {
                using (SqlConnection myconn = new SqlConnection(connString))
                {
                    myconn.Open();
                    for (int i = 0; i < strCmd.Length; i++)
                    {

                        using SqlCommand mycmd = new SqlCommand(strCmd[i], myconn);
                        for (int x = 0; x < MyParams.Count; x++)
                            GetCommand(MyParams[x].dataName, MyParams[x].dataType, MyParams[x].dataValue, mycmd);

                        var result = mycmd.ExecuteScalar();
                        ret = Convert.ToInt32(result);
                    }
                }

                break;
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        "Apple: " + strCommand, ex);
        }

        return ret;
    }

    private static void GetCommand(string DataName, string DataType, object DataValue, SqlCommand cmd)
    {
        try
        {
            switch (DataType)
            {
                case "BIT":
                    cmd.Parameters.Add(DataName, SqlDbType.Bit).Value = DataValue;
                    break;
                case "BYTE":
                    cmd.Parameters.Add(DataName, SqlDbType.VarBinary).Value = DataValue;
                    break;
                case "DATE":
                    cmd.Parameters.Add(DataName, SqlDbType.Date).Value = DataValue;
                    break;
                case "DATETIME":
                    cmd.Parameters.Add(DataName, SqlDbType.DateTime).Value = DataValue;
                    break;
                case "DECIMAL":
                    cmd.Parameters.Add(DataName, SqlDbType.Decimal).Value = DataValue;
                    break;
                case "FLOAT":
                    cmd.Parameters.Add(DataName, SqlDbType.Float).Value = DataValue;
                    break;
                case "GUID":
                    cmd.Parameters.Add(DataName, SqlDbType.UniqueIdentifier).Value = DataValue;
                    break;
                case "INT":
                case "INT32":
                    cmd.Parameters.Add(DataName, SqlDbType.Int).Value = DataValue;
                    break;
                case "INT64":
                case "BIGINT":
                    cmd.Parameters.Add(DataName, SqlDbType.BigInt).Value = DataValue;
                    break;
                case "LIKE":
                    cmd.Parameters.AddWithValue(DataName, string.Format("%{0}%", DataValue));
                    break;
                case "NVARCHAR":
                    cmd.Parameters.Add(DataName, SqlDbType.NVarChar).Value = DataValue;
                    break;
                case "STRING":
                    cmd.Parameters.Add(DataName, SqlDbType.Text).Value = (DataValue ?? DBNull.Value);
                    break;
                case "TIME":
                    cmd.Parameters.Add(DataName, SqlDbType.Time).Value = DataValue;
                    break;
                default:
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ',', DataName, DataType, DataValue);
                    break;
            }
        }
        catch (Exception ex)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                       System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                       System.Reflection.MethodBase.GetCurrentMethod().Name,
                       ex);
        }
    }

}
