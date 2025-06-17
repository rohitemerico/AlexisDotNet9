using System.Data;
using System.Data.SqlClient;
using Alexis.Common;
using Dashboard.Common.Business.Component.Cryptography;

namespace Dashboard.Common.Business.Component;

public enum AuditCategory
{
    System_Auth,
    System_Settings,
    System_User_Maintenance,
    System_Content_Management,

    MDM_iOS,
    MDM_Android,
    MDM_Windows,

    MSF,
    MOB,

    User_Maintenance,
    Kiosk_Maintenance,
    Agent_Maintenance,
    Login
}

public class AuditLog
{
    /// <summary>
    /// Insert into Audit trail for every Create, Update, Approve or Decline, etc. 
    /// </summary>
    /// <param name="optionalDesc">Optional description. If on UPDATE, insert OLD values and NEW values, e.g. OLD rolename:suppert; NEW rolename:support</param>
    /// <param name="auditCategory">E.g. System_Auth for auditing system authorization and authentication.</param>
    /// <param name="action">View, Create, Update, etc. for a specific module</param>
    /// <param name="userId"></param>
    /// <param name="isSuccessful">True if no action failure, false if action failure.</param>
    /// <param name="sourceIP"></param>
    public static void CreateAuditLog(string optionalDesc, AuditCategory auditCategory, ModuleLogAction action, Guid userId, bool isSuccessful, string? sourceIP)
    {
        try
        {
            string description = $"[{auditCategory}] {optionalDesc}";

            string dbQuery = @"INSERT INTO AUDIT_TRAIL (AID,ADATE,ADESC,MID,AACTION,USERID,ASTATUS,ASOURCEIP,ADESTINATIONIP) 
                                VALUES (@AID,@ADATE,@ADESC,@MID,@AACTION,@USERID,@ASTATUS,@ASOURCEIP,@ADESTINATIONIP)";

            string connString = MsgSec.DecryptString(ConfigHelper.GetConnectionString("connectionString"));

            //MSSQL
            using (SqlConnection myconn = new SqlConnection(connString))
            {
                myconn.Open();
                SqlTransaction trans = myconn.BeginTransaction();
                using (SqlCommand cmd = new SqlCommand(dbQuery, myconn, trans))
                {
                    cmd.Parameters.Add("@AID", SqlDbType.NVarChar).Value = Guid.NewGuid().ToString();
                    cmd.Parameters.Add("@ADATE", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@ADESC", SqlDbType.NVarChar).Value = description;
                    cmd.Parameters.Add("@MID", SqlDbType.NVarChar).Value = Module.GetModuleId(action).ToString();
                    cmd.Parameters.Add("@AACTION", SqlDbType.NVarChar).Value = action.ToString().Replace('_', ' ');
                    cmd.Parameters.Add("@USERID", SqlDbType.NVarChar).Value = userId.ToString();
                    if (isSuccessful) cmd.Parameters.Add("@ASTATUS", SqlDbType.Int).Value = 1;
                    else cmd.Parameters.Add("@ASTATUS", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@ASOURCEIP", SqlDbType.NVarChar).Value = sourceIP;
                    cmd.Parameters.Add("@ADESTINATIONIP", SqlDbType.NVarChar).Value = ConfigHelper.DestinationIP;
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
    }
}
