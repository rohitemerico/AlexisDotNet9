using System.Data;
using System.Data.SqlClient;
using Alexis.Common;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Business.Component.Cryptography;
using Dashboard.Entities.ADCB;

namespace Dashboard.Common.Data.Component
{
    public class SqlData_Dashboard
    {
        /// <summary>
        /// Input data to prevent data injection dynamically. pass in sql string, the db name, and the list of parameters with the data in the list
        /// Insertion and updating can be used to call this method
        /// </summary>
        /// <param name="strCommand">sql query</param>
        /// <param name="dbname">databse name value configured in app.config or web.config</param>
        /// <param name="MyParams">the list of parameters to be added in sql query to prevent injection</param>
        /// <returns>true if successfully inserted, false when failed. On false, check server log</returns>
        public static bool Input(string strCommand, string dbname, List<Params> MyParams)
        {
            string connString = MsgSec.DecryptString(ConfigHelper.GetConnectionString(dbname));
            bool ret = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand(strCommand, conn, trans))
                        {
                            //cmd.BindByName = true;
                            for (int i = 0; i < MyParams.Count; i++)
                                GetCommand(MyParams[i].dataName, MyParams[i].dataType, MyParams[i].dataValue, cmd);

                            cmd.ExecuteNonQuery();
                            trans.Commit();
                            ret = true;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Logger.LogToFile("InputDB.log", ',', ex.Message, strCommand);
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            strCommand,
                            ex);
                if (ex.Message.Contains("Incorrect syntax near") ||
                    ex.Message.Contains("Invalid column name") ||
                    ex.Message.Contains("Must declare the scalar variable"))
                    ret = true;
            }
            return ret;

        }

        /// <summary>
        /// Output Data. supply sql string and dbname from config file. to get data from columns split by '#SPLIT#'. to get data by row split by '#NXTL#' prevent data injection
        /// for any selection query, use this method
        /// </summary>
        /// <param name="strCommand">sql query</param>
        /// <param name="dbname">databse name value configured in app.config or web.config</param>
        /// <param name="MyParams">the list of parameters to be added in sql query to prevent injection</param>
        /// <returns>returns a plain string. if there is more than one row split with #NXLI# and if more than one column split with #SPLIT#</returns>
        public static string Output(string strCommand, string dbname, List<Params> MyParams)
        {
            string connString = MsgSec.DecryptString(ConfigHelper.GetConnectionString(dbname));
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
                //Logger.LogToFile("DB.log", strCommand, ex);
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
        /// <param name="dbname">databse name value configured in app.config or web.config</param>
        /// <param name="MyParams">the list of parameters to be added in sql query to prevent injection</param>
        /// <returns>A DataTable of selected records</returns>
        public static DataTable GetResult(string strCommand, string dbname, List<Params> MyParams)
        {
            ////for encryption
            //string XX = MsgSec.EncrpytString("Data Source=localhost;Initial Catalog=emericoadcb;Integrated Security=True;");

            string connString = MsgSec.DecryptString(ConfigHelper.GetConnectionString(dbname));
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection myConn = new SqlConnection(connString))
                {
                    myConn.Open();
                    using (SqlCommand myCmd = new SqlCommand(strCommand, myConn))
                    {
                        for (int x = 0; x < MyParams.Count; x++)
                            GetCommand(MyParams[x].dataName, MyParams[x].dataType, MyParams[x].dataValue, myCmd);
                        myCmd.CommandTimeout = 180;
                        using (SqlDataAdapter sda = new SqlDataAdapter(myCmd))
                        {
                            sda.Fill(dt);
                        }
                    }

                    myConn.Close();
                }
            }


            catch (Exception ex)
            {
                //Logger.LogToFile("DataTableDAC.log", strCommand, ex);
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            strCommand,
                            ex);
            }

            return dt;

        }

        [Obsolete]
        public static string GetJsonResult(string strCommand, string dbname, List<Params> MyParams)
        {
            ////for encryption
            //string XX = MsgSec.EncrpytString("Data Source=localhost;Initial Catalog=emericoadcb;Integrated Security=True;");

            string connString = MsgSec.DecryptString(ConfigHelper.GetConnectionString(dbname));
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection myConn = new SqlConnection(connString))
                {
                    myConn.Open();
                    using (SqlCommand myCmd = new SqlCommand(strCommand, myConn))
                    {
                        for (int x = 0; x < MyParams.Count; x++)
                            GetCommand(MyParams[x].dataName, MyParams[x].dataType, MyParams[x].dataValue, myCmd);
                        myCmd.CommandTimeout = 180;
                        using (SqlDataAdapter sda = new SqlDataAdapter(myCmd))
                        {
                            sda.Fill(dt);
                        }
                    }

                    myConn.Close();
                }
            }


            catch (Exception ex)
            {
                //Logger.LogToFile("DataTableDAC.log", strCommand, ex);
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            strCommand,
                            ex);
            }

            return dt.Rows[0][0].ToString();

        }

        /// <summary>
        /// To get count to return int
        /// use this method to select count
        /// </summary>
        /// <param name="strCommand">sql query</param>
        /// <param name="dbname">databse name value configured in app.config or web.config</param>
        /// <param name="MyParams">the list of parameters to be added in sql query to prevent injection</param>
        /// <returns>gets the count from selection</returns>
        public static int Count(string strCommand, string dbname, List<Params> MyParams)
        {
            int ret = 0;
            string connString = MsgSec.DecryptString(ConfigHelper.GetConnectionString(dbname));

            try
            {
                using (SqlConnection myConn = new SqlConnection(connString))
                {
                    myConn.Open();
                    using (SqlCommand myCmd = new SqlCommand(strCommand, myConn))
                    {
                        for (int x = 0; x < MyParams.Count; x++)
                            GetCommand(MyParams[x].dataName, MyParams[x].dataType, MyParams[x].dataValue, myCmd);
                        ret = Convert.ToInt32(myCmd.ExecuteScalar().ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogToFile("CountDB.log", ',', ex.Message, strCommand);
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
            try
            {
                switch (DataType)
                {
                    case "STRING":
                        cmd.Parameters.Add(DataName, SqlDbType.Text).Value = DataValue ?? DBNull.Value;
                        break;
                    case "INT":
                    case "INT32":
                        cmd.Parameters.Add(DataName, SqlDbType.Int).Value = DataValue;
                        break;
                    case "DATE":
                    case "DATETIME":
                        //Avoid outside the range of Sql Servers DATETIME data type
                        //c# default datetime not nullable

                        DateTime rngMin = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                        DateTime rngMax = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;

                        DateTime cDateMin = DateTime.MinValue;
                        DateTime cDateMax = DateTime.MinValue;

                        try
                        {
                            DateTime dataValue = DateTime.Parse(DataValue.ToString());

                            if (dataValue.Date == cDateMin.Date) dataValue = rngMin;
                            if (dataValue.Date == cDateMax.Date) dataValue = rngMax;

                            cmd.Parameters.Add(DataName, SqlDbType.DateTime).Value = dataValue;
                        }
                        catch
                        {
                            cmd.Parameters.Add(DataName, SqlDbType.DateTime).Value = DataValue ?? DBNull.Value;
                        }


                        break;
                    case "GUID":
                        cmd.Parameters.Add(DataName, SqlDbType.UniqueIdentifier).Value = DataValue;
                        break;
                    case "INT64":
                    case "BIGINT":
                        cmd.Parameters.Add(DataName, SqlDbType.BigInt).Value = DataValue;
                        break;
                    case "DECIMAL":
                        cmd.Parameters.Add(DataName, SqlDbType.Decimal).Value = DataValue;
                        break;
                    case "NVARCHAR":
                    case "NCLOB":
                        cmd.Parameters.Add(DataName, SqlDbType.NVarChar).Value = DataValue ?? DBNull.Value;
                        break;
                    case "BIT":
                        cmd.Parameters.Add(DataName, SqlDbType.Bit).Value = DataValue;
                        break;
                    case "FLOAT":
                        cmd.Parameters.Add(DataName, SqlDbType.Float).Value = DataValue;
                        break;
                    case "TIME":
                        cmd.Parameters.Add(DataName, SqlDbType.Time).Value = DataValue;
                        break;
                    default:
                        Logger.LogToFile("unknowntype.log", ',', DataName, DataType, DataValue);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogToFile("GetCommand.log", ex);
            }
        }
    }
}

