using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using Microsoft.Data.SqlClient;

namespace MDM.iOS.Common.Data.Component;
/// <summary>
/// This class is mainly used for the business logic and managing data, i.e get/post
/// </summary>
public class SqlData
{
    public static string data = null;
    public static string err = null;

    /// <summary>
    /// This method is used to store data in the database based on the specified parameter of the 
    /// SQL query. 
    /// </summary>
    /// <param name="strCommand">sql query</param>
    /// <param name="MyParams">the list of parameters to be added in sql query to prevent injection</param>
    /// <returns>boolean value of true or false</returns>
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

                        using (SqlCommand mycmd = new SqlCommand(strCmd[i], myconn))
                        {
                            for (int x = 0; x < MyParams.Count; x++)
                            {
                                //MyParams[x].dataValue = null;
                                GetCommand(MyParams[x].dataName, MyParams[x].dataType, MyParams[x].dataValue, mycmd);
                            }
                            mycmd.ExecuteNonQuery();
                        }
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

    /// <summary>
    /// for any selection query, use this method
    /// </summary>
    /// <param name="strCommand">sql query</param>
    /// <param name="MyParams"></param>
    /// <returns>returns a plain string. if there is more than one row split with #NXLI# and if more than one column split with #SPLIT#</returns>
    public static string Output(string strCommand, List<Params> MyParams, string connString)
    {
        string data = string.Empty;
        try
        {
            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                using (SqlCommand myCmd = new SqlCommand(strCommand, myConn))
                {
                    for (int x = 0; x < MyParams.Count; x++)
                        GetCommand(MyParams[x].dataName, MyParams[x].dataType, MyParams[x].dataValue, myCmd);
                    using (SqlDataReader sdr = myCmd.ExecuteReader())
                    {
                        if (sdr.HasRows)
                        {
                            //loop columns + rows
                            if (sdr.FieldCount > 1)
                            {
                                while (sdr.Read())
                                {
                                    for (int i = 0; i < sdr.FieldCount; i++)
                                    {
                                        if (i == sdr.FieldCount - 1)
                                            data += sdr[i].ToString() + "#NXTLI#";
                                        else
                                            data += sdr[i].ToString() + "#SPLIT#";
                                    }
                                }
                            }
                            else //Loop rows
                            {
                                while (sdr.Read())
                                {
                                    data += sdr[0].ToString() + "#NXTLI#";
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        strCommand,
                        ex);
        }

        if (!string.IsNullOrEmpty(data))
        {
            if (data.Trim().EndsWith("#"))
            {
                data = data.Trim().Substring(0, data.Length - 7);
            }
        }

        return data;

    }

    /// <summary>
    /// for selection of large records use this method
    /// </summary>
    /// <param name="strCommand">sql query</param>
    /// <param name="MyParams">the list of parameters to be added in sql query to prevent injection</param>
    /// <returns>A DataTable of selected records</returns>
    public static DataTable GetResult(string strCommand, List<Params> MyParams, string connString)
    {
        DataTable dt = new DataTable();
        try
        {

            using (SqlConnection myConn = new SqlConnection(connString))
            {
                // this part throws an exception, double check this portion
                myConn.Open();

                // there's an error over here. 
                using (SqlCommand myCmd = new SqlCommand(strCommand, myConn))
                {
                    for (int x = 0; x < MyParams.Count; x++)
                        GetCommand(MyParams[x].dataName, MyParams[x].dataType, MyParams[x].dataValue, myCmd);
                    using (SqlDataAdapter sda = new SqlDataAdapter(myCmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }
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
    /// use this method to select count
    /// </summary>
    /// <param name="strCommand">sql query</param>
    /// <param name="MyParams">the list of parameters to be added in sql query to prevent injection</param>
    /// <returns>gets the count from selection</returns>
    [Obsolete]
    public static int Count(string strCommand, List<Params> MyParams, string connString)
    {
        int ret = 0;
        try
        {
            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                using (SqlCommand myCmd = new SqlCommand(strCommand, myConn))
                {
                    for (int x = 0; x < MyParams.Count; x++)
                        GetCommand(MyParams[x].dataName, MyParams[x].dataType, MyParams[x].dataValue, myCmd);
                    ret = (int)myCmd.ExecuteScalar();
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        strCommand,
                        ex);
        }

        return ret;
    }

    private static void GetCommand(string DataName, string DataType, object DataValue, SqlCommand cmd)
    {
        //DateTime? MyNullableDate;
        //MyNullableDate = null;

        try
        {
            switch (DataType)
            {
                case "LIKE":
                    cmd.Parameters.AddWithValue(DataName, string.Format("%{0}%", DataValue));
                    break;
                case "STRING":
                    cmd.Parameters.Add(DataName, SqlDbType.Text).Value = (DataValue != null ? DataValue : DBNull.Value);
                    break;
                case "INT":
                case "INT32":
                    cmd.Parameters.Add(DataName, SqlDbType.Int).Value = (DataValue != null ? DataValue : DBNull.Value);
                    break;
                case "DATE":
                    string mydate = "01/01/0001";
                    DateTime cDataVlue1 = Convert.ToDateTime(mydate);
                    if (Convert.ToDateTime(DataValue) == cDataVlue1)
                    {
                        cmd.Parameters.Add(DataName, SqlDbType.Date).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add(DataName, SqlDbType.Date).Value = DataValue;
                    }

                    break;
                case "DATETIME":

                    string date = "01/01/0001";

                    DateTime cDataVlue = Convert.ToDateTime(date);
                    DateTime c = DateTime.Now;
                    try
                    {// This portion of the code needs some detailed debugging. 
                     // ask shaun 2molo
                        c = DateTime.ParseExact(DataValue.ToString(), "dd-MMM-yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        c = Convert.ToDateTime(DataValue);
                    }

                    if (c.Date == cDataVlue.Date)
                    {
                        cmd.Parameters.Add(DataName, SqlDbType.DateTime).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add(DataName, SqlDbType.DateTime).Value = c;
                    }


                    break;
                case "GUID":

                    if (DataValue == null)
                    {
                        cmd.Parameters.Add(DataName, SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters.Add(DataName, SqlDbType.UniqueIdentifier).Value = new System.Data.SqlTypes.SqlGuid(DataValue.ToString());
                    }


                    break;
                case "INT64":
                case "BIGINT":
                    cmd.Parameters.Add(DataName, SqlDbType.BigInt).Value = (DataValue != null ? DataValue : DBNull.Value);
                    break;
                case "DECIMAL":
                    cmd.Parameters.Add(DataName, SqlDbType.Decimal).Value = (DataValue != null ? DataValue : DBNull.Value);
                    break;
                case "NVARCHAR":
                    cmd.Parameters.Add(DataName, SqlDbType.NVarChar).Value = (DataValue != null ? DataValue : DBNull.Value);
                    break;
                case "BIT":
                    cmd.Parameters.Add(DataName, SqlDbType.Bit).Value = (DataValue != null ? DataValue : DBNull.Value);
                    break;
                case "FLOAT":
                    cmd.Parameters.Add(DataName, SqlDbType.Float).Value = (DataValue != null ? DataValue : DBNull.Value);
                    break;
                case "TIME":
                    cmd.Parameters.Add(DataName, SqlDbType.Time).Value = (DataValue != null ? DataValue : DBNull.Value);
                    break;
                case "BYTE":
                    cmd.Parameters.Add(DataName, SqlDbType.VarBinary).Value = (DataValue != null ? DataValue : DBNull.Value);
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
