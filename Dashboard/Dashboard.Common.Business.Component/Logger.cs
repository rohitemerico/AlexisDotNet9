using System.Configuration;
using System.Text;
using Alexis.Common;
using Dashboard.Common.Business.Component.Cryptography;
using Oracle.ManagedDataAccess.Client;

namespace Dashboard.Common.Business.Component;

public class Logger
{
    protected static string CreateFilePath(string LogType)
    {
        string PagePath = ConfigHelper.LogDirectory + @"\" + DateTime.Now.ToString("yyyyMMdd");
        string LogPath = PagePath + @"\" + LogType;

        if (!Directory.Exists(PagePath))
        {
            Directory.CreateDirectory(PagePath);
        }

        if (!Directory.Exists(LogPath))
        {
            Directory.CreateDirectory(LogPath);
        }



        return LogPath;
    }


    #region LogToFile(string LogType,string PageName,string MethodName,Exception ex)
    public static void LogToFile(string LogType, string PageName, string MethodName, Exception ex)
    {
        string LogDirectory = CreateFilePath(LogType) + @"\";

        LogToFile(PageName + "_" + MethodName + ".log",
            "Logging Time: " + DateTime.Now.ToString() + Environment.NewLine +
            "ExceptionMessage: " + ex.Message + Environment.NewLine +
            "StackTrace: " + ex.StackTrace + Environment.NewLine +
            "Source: " + ex.Source, LogDirectory);
    }
    #endregion


    #region LogToFile(string LogType, string PageName, string MethodName,char delimiter, params object[] list)
    /// <summary>
    /// Log multiple data into the file by passing parameters
    /// </summary>
    /// <param name="filename">the name of the file (do not include the directory</param>
    public static int LogToFile(string LogType, string PageName, string MethodName, char delimiter, params object[] list)
    {
        string LogDirectory = CreateFilePath(LogType) + @"\";

        // form delimitered string
        string s = "";
        int i = 0;
        for (i = 0; i < list.Length - 1; i++)
        {
            s = s + list[i] + delimiter;
        }
        s = s + list[i];

        // log to file
        return LogToFile(PageName + "_" + MethodName + ".log", s, LogDirectory);
    }
    #endregion


    #region LogToFile(string LogType,string PageName,string MethodName, string faultyData, Exception ex)
    /// <summary>
    /// Log the error into File
    /// </summary>
    /// <param name="filename">the name of the file (do not include the directory</param>
    /// <param name="queryCommand">string command of a query</param>
    /// <param name="ex">exception</param>
    public static void LogToFile(string LogType, string PageName, string MethodName, string faultyData, Exception ex)
    {
        string LogDirectory = CreateFilePath(LogType) + @"\";

        LogToFile(PageName + "_" + MethodName + ".log",
            "Logging Time: " + DateTime.Now.ToString() + Environment.NewLine +
            "FaultyData: " + faultyData + Environment.NewLine +
            "ExceptionMessage: " + ex.Message + Environment.NewLine +
            "StackTrace: " + ex.StackTrace + Environment.NewLine +
            "Source: " + ex.Source, LogDirectory);
    }
    #endregion


    #region LogToFile(string filename, string faultyData, Exception ex)
    /// <summary>
    /// Log the error into File
    /// </summary>
    /// <param name="filename">the name of the file (do not include the directory</param>
    /// <param name="queryCommand">string command of a query</param>
    /// <param name="ex">exception</param>
    public static void LogToFile(string filename, string faultyData, Exception ex)
    {
        string LogDirectory = ConfigurationManager.AppSettings["LogDirectory"];

        LogToFile(filename,
            "Logging Time: " + DateTime.Now.ToString() + Environment.NewLine +
            "FaultyData: " + faultyData + Environment.NewLine +
            "ExceptionMessage: " + ex.Message + Environment.NewLine +
            "StackTrace: " + ex.StackTrace + Environment.NewLine +
            "Source: " + ex.Source, LogDirectory);
    }
    #endregion


    #region LogToFile(string filename, Exception ex)
    /// <summary>
    /// Log the error into file
    /// </summary>
    /// <param name="filename">the name of the file (do not include the directory</param>
    /// <param name="ex">exception</param>
    public static void LogToFile(string filename, Exception ex)
    {
        string LogDirectory = ConfigurationManager.AppSettings["ExLogDirectory"];

        LogToFile(filename,
            "Logging Time: " + DateTime.Now.ToString() + Environment.NewLine +
            "ExceptionMessage: " + ex.Message + Environment.NewLine +
            "StackTrace: " + ex.StackTrace + Environment.NewLine +
            "Source: " + ex.Source, LogDirectory);
    }
    #endregion


    #region LogToFile(string filename, string s)
    private static int LogToFile(string filename, string s, string logDirectory)
    {
        //StreamWriter txt = File.AppendText(logDirectory + filename);
        if (string.IsNullOrEmpty(logDirectory))
        {
            logDirectory = Directory.GetCurrentDirectory() + "\\";
        }

        Directory.CreateDirectory(logDirectory);

        int ret = -1;
        int i = 0;
        string nameOfFile = Path.GetFileNameWithoutExtension(filename);
        string fileExtension = Path.GetExtension(filename);
        filename = nameOfFile + "_" + DateTime.Now.ToString("ddMMyyyy") + fileExtension;

        while (true)
        {

            if (i == 20)//set retry to 20 to prevent unlimited loop from happening if directory cannot be found
            {
                break;
            }
            try
            {
                StreamWriter txt = File.AppendText(logDirectory + filename);
                txt.WriteLine(s);
                txt.WriteLine("\n----------------------------------------------------------------------------");
                txt.Close();
                ret = 1;
                break;
            }
            catch
            {
                //Do nothing let it loop and 
                i++;
                continue;
            }

        }

        return ret;
    }
    #endregion


    #region LogToFile(string filename, char delimeter, params string[] list,string name )
    /// <summary>
    /// Log multiple data into the file by passing parameters
    /// </summary>
    /// <param name="filename">the name of the file (do not include the directory</param>
    public static int LogToFile(string filename, char delimiter, params object[] list)
    {
        //string LogDirectory = ConfigurationManager.AppSettings["LogDirectory"];

        string LogDirectory = CreateFilePath(filename) + @"\";

        // form delimitered string
        string s = "";
        int i = 0;
        for (i = 0; i < list.Length - 1; i++)
        {
            s = s + list[i] + delimiter;
        }
        s = s + list[i];

        // log to file
        return LogToFile(filename, s, LogDirectory);
    }
    #endregion


    #region LogToFile(string LogType, string PageName, string MethodName, string message)
    public static void LogToFile(string LogType, string PageName, string MethodName, string message)
    {
        string LogDirectory = CreateFilePath(LogType) + @"\";

        //LogToFile(PageName + "_" + MethodName + ".log",
        //    "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] - (" + PageName + "_" + MethodName + ") - [Thread: " + System.Threading.Thread.CurrentThread.ManagedThreadId + "] - [DEBUG] " + message
        //    , LogDirectory);
        LogToFile(PageName + "_" + MethodName + ".log",
            "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] - (" + PageName + "_" + MethodName + ") - " + Environment.NewLine + message
            , LogDirectory);
    }
    #endregion


    public static void WebServiceLog(string Description, WebServiceAction Action, string MachineSn, bool Status, string EncryptedData, string DecryptedData)
    {
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO WEBSERVICE_LOG");
            sql.AppendLine("(LOG_ID,LOG_DESC,LOG_RAW,LOG_DECRYPT,LOG_METHOD,LOG_STATUS,MACHINESN,LOG_DATE)");
            sql.AppendLine("VALUES");
            sql.AppendLine("(:LOGID,:LOGDESC,:LOGRAW,:LOGDECRYPT,:LOGMETHOD,:LOGSTATUS,:MACHINESN,:LOGDATE)");

            string connString = MsgSec.DecryptString(ConfigurationManager.AppSettings.Get("connectionString"));

            using (OracleConnection myconn = new OracleConnection(connString))
            {
                myconn.Open();
                OracleTransaction trans = myconn.BeginTransaction();
                using (OracleCommand cmd = new OracleCommand(sql.ToString(), myconn))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(":LOGID", OracleDbType.NVarchar2).Value = Guid.NewGuid();
                    cmd.Parameters.Add(":LOGDESC", OracleDbType.NVarchar2).Value = Description;
                    cmd.Parameters.Add(":LOGRAW", OracleDbType.Varchar2).Value = EncryptedData;
                    cmd.Parameters.Add(":LOGDECRYPT", OracleDbType.Varchar2).Value = DecryptedData;
                    cmd.Parameters.Add(":LOGMETHOD", OracleDbType.Varchar2).Value = Action;

                    cmd.Parameters.Add(":MACHINESN", OracleDbType.Varchar2).Value = MachineSn;
                    cmd.Parameters.Add(":LOGDATE", OracleDbType.TimeStamp).Value = DateTime.Now;
                    if (Status)
                        cmd.Parameters.Add(":LOGSTATUS", OracleDbType.Int32).Value = 1;
                    else
                        cmd.Parameters.Add(":LOGSTATUS", OracleDbType.Int32).Value = 0;
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
            }

        }
        catch (Exception ex)
        {
            LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
    }

    public enum WebServiceAction
    {
        Monitoring_Stat,
        Monitoring_Alarm,
        DownloadAdvertisement,
        Monitoring_GetAdvertisementList,
        DownloadKioskSetting,
        DownloadAgentSetting,
        DownloadKioskOrAgentSetting,
        DownloadAllActiveAdvertisementSetting,
        PrinterPaperJam,
        ShutterGateUnauthoriseAccess
    }

}