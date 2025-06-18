using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Business.Component.Cryptography;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

public class UserRoleController : GlobalController
{
    public bool AuthenticateLocalUser(string username, string password)
    {
        bool ret = false;
        try
        {
            string passwordEncoded = MsgSec.EncryptString(password);
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select *");
            sql.AppendLine(" from user_login uLogin, LocalUser localU ");
            sql.AppendLine(" where uLogin.aID = localU.LoginID ");
            sql.AppendLine(" and UPPER(uLogin.uName) = UPPER(:username) and uLogin.uStatus = 1 ");
            sql.AppendLine(" and localU.lPassword = :pass ");
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":username", "NVARCHAR", username));
            MyParams.Add(new Params(":pass", "NVARCHAR", passwordEncoded));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

            if (dt.Rows.Count != 0)
            {
                ret = true;
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserRoleController_getUserDetails.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool isUserLoginEmailAvailable(string username, string email)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select * ");
            sql.AppendLine("from user_login tul, LocalUser tlu ");
            sql.AppendLine("where tul.aID = tlu.LoginID ");
            sql.AppendLine("and UPPER(tul.uName) = UPPER(:username) and tlu.LEmail = :email ");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":username", "NVARCHAR", username));
            MyParams.Add(new Params(":email", "NVARCHAR", email));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

            if (dt.Rows.Count != 0)
            {
                ret = true;
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }
    public string GetGuidForgotPasswordLocalUser(string username, string email)
    {
        string guidString = Guid.NewGuid().ToString();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select AID ");
            sql.AppendLine("from user_login ul");
            sql.AppendLine("where UPPER(ul.uName) = UPPER(:username) and UPPER(Email) = UPPER(:email) ");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":username", "NVARCHAR", username));
            MyParams.Add(new Params(":email", "NVARCHAR", email));
            string userID = dbController.Output(sql.ToString(), "connectionString", MyParams);

            if (userID == null)
                return null;

            sql.Clear();
            //LResetPasswordDateTime
            //LResetPassword
            MyParams.Clear();
            sql.AppendLine("update LocalUser set ");
            sql.AppendLine("LResetPassword = :guidString , LResetPasswordDateTime = :ResetPasswordDateTime");
            sql.AppendLine("where LoginID = :userID ");

            MyParams.Add(new Params(":userID", "NVARCHAR", userID));
            MyParams.Add(new Params(":guidString", "NVARCHAR", guidString));
            MyParams.Add(new Params(":ResetPasswordDateTime", "DATETIME", DateTime.Now));
            bool ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
            if (!ret)
                guidString = null;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return guidString;
    }

    public string GetEmailUpdateForgottenPasswordForLocalUser(string guidNotes)
    {
        if (string.IsNullOrEmpty(guidNotes) || string.IsNullOrEmpty(guidNotes))
        {
            return "";
        }
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("Select Email from USER_LOGIN where AID = (select LOGINID from LocalUser where LResetPassword =  :guidNotes )");
            MyParams.Add(new Params(":guidNotes", "NVARCHAR", guidNotes));
            return dbController.Output(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return "";
    }

    /// <summary>
    /// Provides string value from LocalUser table from LNotes field.</br>
    /// Call GetEmailUpdateForgottenPasswordForLocalUser(..) method, before current, in case you need email.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <returns>null if issue getting it</br>string empty or not empty.</returns>
    public bool UpdateForgottenPasswordForLocalUser(string guidNotes, string password)
    {
        bool ret = false;
        if (string.IsNullOrEmpty(guidNotes) || string.IsNullOrEmpty(password))
        {
            ret = false;
            return ret;
        }
        try
        {
            //LResetPasswordDateTime
            //LResetPassword
            string passwordEncoded = MsgSec.EncryptString(password);

            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select LResetPasswordDateTime from LocalUser where LResetPassword = :guidNotes");
            MyParams.Add(new Params(":guidNotes", "NVARCHAR", guidNotes));
            string datetime1 = dbController.Output(sql.ToString(), "connectionString", MyParams);

            DateTime ResetTime = DateTime.Parse(datetime1);
            if ((DateTime.Now - ResetTime).TotalMinutes < 15)
            {
                sql.Clear();
                sql.AppendLine("update LocalUser set ");
                sql.AppendLine("LPassword = :password , LResetPassword = NULL , LResetPasswordDateTime = NULL ");
                sql.AppendLine("where LResetPassword = :guidNotes ");

                MyParams.Clear();
                MyParams = new List<Params>();
                MyParams.Add(new Params(":guidNotes", "NVARCHAR", guidNotes));
                MyParams.Add(new Params(":password", "NVARCHAR", passwordEncoded));
                ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool isUserAvailable(string username)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select * from User_Login where UPPER(uName)=UPPER(:username)");
            MyParams.Add(new Params(":username", "NVARCHAR", username));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count != 0)
            {
                ret = true;
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserRoleController_isUserAvailable.log", ex);
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public Guid getUserID(string username)
    {
        Guid ret = new Guid();
        try
        {
            StringBuilder sql = new StringBuilder();
            if (username.ToUpper() == "ADMIN")
                sql.AppendLine("select aid from user_login where UPPER(uname) = UPPER(:username)");
            else
                sql.AppendLine("select aid from user_login where UPPER(uname) = UPPER(:username) and ustatus = 1");
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":username", "NVARCHAR", username));
            ret = new Guid(dbController.Output(sql.ToString(), "connectionString", MyParams));

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getUserID.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public int getLoginFlag(Guid username)
    {
        int ret = 0;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select loginflag from user_login where aID= :username");
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":username", "GUID", username));
            ret = int.Parse(dbController.Output(sql.ToString(), "connectionString", MyParams));
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getUserID.log", ex);
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public int getTroubleshoot(Guid username)
    {
        int ret = 0;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select mtroubleshoot from user_login where aID= :username");
            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":username", "GUID", username));
            ret = int.Parse(dbController.Output(sql.ToString(), "connectionString", MyParams));
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getUserID.log", ex);
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public bool setLogin(string sessionID, Guid userID)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("update User_Login set ");
            sql.AppendLine("uLastLoginDate = :uLastLoginDate , uSessionID = :uSessionID , LoginFlag = 1 ");
            sql.AppendLine("where aID=:userID ");
            myParams.Add(new Params(":uLastLoginDate", "DATETIME", DateTime.Now));
            myParams.Add(new Params(":uSessionID", "NVARCHAR", sessionID));
            myParams.Add(new Params(":userID", "GUID", userID));
            ret = dbController.Input(sql.ToString(), "connectionString", myParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserRoleController_verifyUser.log", ex);
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;

    }

    public bool setLogout(Guid userID)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("update User_Login set ");
            sql.AppendLine("LoginFlag = 0 ");
            sql.AppendLine("where aID=:userID ");
            myParams.Add(new Params(":userID", "GUID", userID));
            ret = dbController.Input(sql.ToString(), "connectionString", myParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserRoleController_verifyUser.log", ex);
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public DataTable getUserDetails(Guid userid)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select * from");
            sql.AppendLine("user_login ul, User_Roles ru, User_Permission up, Menu m");
            sql.AppendLine("where ul.rID = ru.rid and ru.rid = up.rID and up.mid = m.mid ");
            sql.AppendLine("and ul.aid = :userid");
            sql.AppendLine("and m.mgroup <> 9 order by m.mGroup, m.mType, m.mDesc ");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":userid", "NVARCHAR", userid));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserRoleController_getUserDetails.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool isUserAvailable(string username, Guid? id)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select * from User_Login where UPPER(uName)=UPPER(:username)  AND aid <> @aid");
            MyParams.Add(new Params(":username", "NVARCHAR", username));
            MyParams.Add(new Params("@aid", "GUID", id));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count == 0)
            {
                ret = true;
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserRoleController_isUserAvailable.log", ex);
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public bool isRoleAvailable(string rolename)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select * from User_Roles where rDesc= :role");
            MyParams.Add(new Params(":role", "NVARCHAR", rolename));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count != 0)
            {
                ret = true;
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserRoleController_isRoleAvailable.log", ex);

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public string getModuleNameByID(Guid moduleID)
    {
        string ret = "";
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("select mDesc from Menu ");
            sql.AppendLine("where mID=:mID ");
            myParams.Add(new Params(":mID", "GUID", moduleID));
            ret = dbController.Output(sql.ToString(), "connectionString", myParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserRoleController_verifyUser.log", ex);

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;

    }
    #region Role Create Function
    public bool InsertRole(Role role, List<ModuleViewModel> grid)
    {
        bool ret = false;
        try
        {
            if (!isRoleAvailable(role.RoleDesc))
            {
                sql.Clear();
                MyParams.Clear();

                sql.AppendLine("insert into User_Roles(rID, rDesc, rRemarks, rStatus, rCreatedDate, rCreatedby) " +
                    "values (:guid, :description, :remarks, :sta, :cDate, :cBy)");
                MyParams.Add(new Params(":guid", "NVARCHAR", role.RoleID));
                MyParams.Add(new Params(":description", "NVARCHAR", role.RoleDesc));
                MyParams.Add(new Params(":remarks", "NVARCHAR", role.Remarks));
                MyParams.Add(new Params(":sta", "INT", 0));
                MyParams.Add(new Params(":cDate", "DATETIME", role.CreatedDate));
                MyParams.Add(new Params(":cBy", "NVARCHAR", role.CreatedBy));

                if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                {
                    Insert_HomeSettingsLogout(role.RoleID);

                    sql.Clear();
                    sql.AppendLine("insert into User_Permission values (:rGuid, :mGuid, :viewer, :maker, :checker)");

                    foreach (var gvrRow in grid)
                    {
                        MyParams.Clear();
                        MyParams.Add(new Params(":rGuid", "NVARCHAR", role.RoleID));
                        MyParams.Add(new Params(":mGuid", "NVARCHAR", gvrRow.mID));
                        MyParams.Add(new Params(":viewer", "BIT", gvrRow.mView));
                        MyParams.Add(new Params(":maker", "BIT", gvrRow.mMaker));
                        MyParams.Add(new Params(":checker", "BIT", gvrRow.mChecker));

                        if (!dbController.Input(sql.ToString(), "connectionString", MyParams))
                        {
                            throw new Exception();// ("Cannot insert or update into User_Permission");
                        }
                    }
                    ret = true;
                }
                else
                {
                    throw new Exception();// ("Cannot insert or update into UserRoles");
                }
            }
            else
            {
                throw new Exception();// ("Role existed");
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("Role_Create_InsertRole.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    protected void Insert_HomeSettingsLogout(Guid gid)
    {
        try
        {
            Guid[] mID = { Guid.Parse("d99b88e4-9950-47e9-85a5-c764ef749f10"), //home
                             Guid.Parse("00286255-8082-4605-b2ae-45a136a7921a"), //mdm
                             Guid.Parse("14d80667-983d-4cc5-8c67-5df92a188d3b"), //reporting and analysis
                             //Guid.Parse("d88b88e8-9950-47e9-85a5-ea74605def85"), //download sdk
                             Guid.Parse("ad111f6c-2bf1-4d9a-b367-8be3e4581a73"), //content management
                             //Guid.Parse("2a27bce0-f99c-4db1-9c7e-cf48bfaac829"), //Agent Video Conference
                             Guid.Parse("d6ff775b-3f64-4edd-92cb-ff27c74d014f"), //User Maintenance
                             Guid.Parse("21514002-379e-4b4d-bbae-fcf3b82b4596"), //Settings
                             Guid.Parse("0fe0775e-5609-4185-a305-96f075e15c65"), //Logout

                             Guid.Parse("000a5e60-61a6-4eac-a151-963dd7f31c32"), //mdm > Android MDM
                             Guid.Parse("13c9e8a1-1399-48ed-8854-48ba2ee8fdff"), //mdm > Windows MDM
                             Guid.Parse("cabd11ed-7612-42cd-9a4d-937db21a59d5"), //mdm > iOS MDM
                             Guid.Parse("009066d5-799e-4d41-af79-6c9c5ba7ce63"), //report... > Windows MDM
                             Guid.Parse("00a55bca-1fdc-4bd5-9679-be0e03fa4db1"), //report... > Android MDM 
                             Guid.Parse("00f9a964-96c3-4ea2-9ea0-64efdc90c30f"), //report... > iOS MDM 
                             Guid.Parse("00ca7fdc-23ff-4591-8a19-51516b8ae655"), //report... > MOB 
                             Guid.Parse("00d2e4e7-f459-49f9-8334-ae228e64d380") //report... > MSF 
                            };
            foreach (Guid i in mID)
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("insert into User_Permission values (:rGuid, :mGuid, :viewer, :maker, :checker)");
                MyParams.Add(new Params(":rGuid", "NVARCHAR", gid));
                MyParams.Add(new Params(":mGuid", "NVARCHAR", i));
                MyParams.Add(new Params(":viewer", "BIT", 1));
                MyParams.Add(new Params(":maker", "BIT", 1));
                MyParams.Add(new Params(":checker", "BIT", 1));

                if (!dbController.Input(sql.ToString(), "connectionString", MyParams))
                {
                    throw new Exception("Cannot insert home, settings, logout into User_Permission");
                }
                sql.Clear();
                MyParams.Clear();
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("Insert_HomeSettingsLogout.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
    }
    public string IsValidatedToInactive(Guid id)
    {
        string ret = "";
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from user_login ul ");
            sql.AppendLine("join user_roles ur on (ul.rid = ur.rid) ");
            sql.AppendLine("where ur.rid = :rid and ul.uStatus = :status ");
            MyParams.Add(new Params(":rid", "GUID", id));
            MyParams.Add(new Params(":status", "INT", 1));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            foreach (DataRow dr in dt.Rows)
                ret += "The Role is assigned to User, " + dr["uName"].ToString() + "! The Role Cannot be Inactive! <br />";
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public bool UpdateRole(Role role)
    {
        bool ret = false, accessPermission = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update User_Roles set rupdateddate = current_timestamp, rupdatedby = :updateuser, ");
            if (!isRoleAvailable(role.RoleDesc))
            {
                sql.AppendLine("rDesc=:newRoleName, ");
                MyParams.Add(new Params(":newRoleName", "NVARCHAR", role.RoleDesc));
            }
            sql.AppendLine("rRemarks = :rRemarks, rStatus = :rStatus where rID=:gid ");
            MyParams.Add(new Params(":rRemarks", "NVARCHAR", role.Remarks));
            MyParams.Add(new Params(":rStatus", "INT", role.Status));
            MyParams.Add(new Params(":gid", "NVARCHAR", role.RoleID));
            MyParams.Add(new Params(":updateuser", "NVARCHAR", role.UpdatedBy));

            accessPermission = dbController.Input(sql.ToString(), "connectionString", MyParams);

            //sql.Clear();
            //MyParams.Clear();
            //sql.AppendLine("delete from User_Permission where rID=:rGuid");
            //MyParams.Add(new Params(":rGuid", "NVARCHAR", role.RoleID));
            //if (dbController.Input(sql.ToString(), "connectionString", MyParams))
            //{

            //sql.Clear();
            //Insert_HomeSettingsLogout(role.RoleID);
            //sql.AppendLine("insert into User_Permission values (:rGuid, :mGuid, :viewer, :maker, :checker)");
            //sql.AppendLine("update User_Permission set mView=:viewer, mMaker=:maker, mChecker=:checker where rID=:rGuid and mID=:mGuid ");
            foreach (Role.RolePermission row in role.PermissionList)
            {

                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("select * from User_Permission where rID=:rGuid and mID=:mGuid ");
                MyParams.Add(new Params(":rGuid", "NVARCHAR", row.RoleRefID));
                MyParams.Add(new Params(":mGuid", "NVARCHAR", row.MenuID));
                if (dbController.GetResult(sql.ToString(), "connectionString", MyParams).Rows.Count > 0)
                {
                    sql.Clear();
                    MyParams.Clear();
                    sql.AppendLine("update User_Permission set mView=:viewer, mMaker=:maker, mChecker=:checker where rID=:rGuid and mID=:mGuid ");
                    MyParams.Add(new Params(":rGuid", "NVARCHAR", row.RoleRefID));
                    MyParams.Add(new Params(":mGuid", "NVARCHAR", row.MenuID));
                    MyParams.Add(new Params(":viewer", "BIT", row.MView));
                    MyParams.Add(new Params(":maker", "BIT", row.MMake));
                    MyParams.Add(new Params(":checker", "BIT", row.MCheck));
                }
                else
                {
                    sql.Clear();
                    MyParams.Clear();
                    sql.AppendLine("insert into User_Permission values (:rGuid, :mGuid, :viewer, :maker, :checker)");
                    MyParams.Add(new Params(":rGuid", "NVARCHAR", row.RoleRefID));
                    MyParams.Add(new Params(":mGuid", "NVARCHAR", row.MenuID));
                    MyParams.Add(new Params(":viewer", "BIT", row.MView));
                    MyParams.Add(new Params(":maker", "BIT", row.MMake));
                    MyParams.Add(new Params(":checker", "BIT", row.MCheck));
                }

                if (!dbController.Input(sql.ToString(), "connectionString", MyParams))
                {
                    throw new Exception("Cannot insert or update into User_Permission");// ("Cannot insert or update into User_Permission");
                }
                else
                {
                    ret = true;
                }

            }
            //}
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UpdateRole.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }

        return ret;
    }
    public DataTable GetModulePermissionByRoleId(Guid RowID)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("select mChild.mID, mChild.mDesc child, mParent.mDesc parent, ");
            sql.AppendLine("UP.mView, UP.mMaker, UP.mChecker, UR.rDesc, UR.rRemarks, UR.rStatus ");
            sql.AppendLine("from menu mChild, menu mParent, User_Roles UR, User_Permission UP ");
            sql.AppendLine("where mChild.mGroup = mParent.mGroup and UR.rID = UP.rID  and UP.mID = mChild.mID ");
            sql.AppendLine("and mParent.mType=@T1 and mChild.mType=@T2 and mChild.mGroup<>@T3 and UR.rID = @rID ");
            sql.AppendLine("order by mParent.mType, mChild.mGroup, mChild.MSEQ");

            //sql.AppendLine("select mChild.mID, mChild.mDesc child, mParent.mDesc parent,  ");
            ////sql.AppendLine("nvl(Permission.mView,0) mView, nvl(Permission.mMaker,0) mMaker, nvl(Permission.mChecker,0) mChecker, Permission.rDesc, Permission.rRemarks, Permission.rStatus  ");
            //sql.AppendLine("isnull(Permission.mView,0) mView, isnull(Permission.mMaker,0) mMaker, isnull(Permission.mChecker,0) mChecker, Permission.rDesc, Permission.rRemarks, Permission.rStatus  ");
            //sql.AppendLine("from menu mChild ");
            //sql.AppendLine("join menu mParent on (mChild.mGroup = mParent.mGroup) ");
            //sql.AppendLine("left join  ");
            //sql.AppendLine("(select * from User_Permission UP ");
            //sql.AppendLine("join User_Roles UR on (UR.rID = UP.rID) ");
            //sql.AppendLine("where UR.rID = @rID) Permission on (Permission.mID = mChild.mID) ");
            //sql.AppendLine("where mParent.mType=@T1 and mChild.mType=@T2 and mChild.mGroup<>@T3 ");
            //sql.AppendLine("order by mParent.mDesc, mChild.mDesc ");

            MyParams.Add(new Params("@T1", "INT", 0));
            MyParams.Add(new Params("@T2", "INT", 1));
            MyParams.Add(new Params("@T3", "INT", 10));
            MyParams.Add(new Params("@rID", "NVARCHAR", RowID.ToString()));

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    #endregion
    #region User Create Function
    //public bool InsertUser(User user)
    //{
    //    bool ret = false;
    //    try
    //    {
    //        if (isUserAvailable(user.UserName, user.UserID))
    //        {
    //            sql.Clear();
    //            MyParams.Clear();

    //            sql.Append("insert into User_Login (aID, uName, uFullName, uCMID, rID, AgentFlag, mCardReplenishment, mChequeReplenishment, mConsumable, mSecurity, mTroubleshoot, uRemarks, uStatus, uCreatedDate, uCreatedBy, LOCALUSER) ");
    //            sql.Append("values (@aid, @uname, @uFullName, @uCMID, @rid, @aFlag, @mCardReplenishment, @mChequeReplenishment, @mConsumable, @mSecurity, @mTroubleshoot, @uRemarks, @ustatus, @cDate, @cUser, @mLocalUser)");

    //            MyParams.Add(new Params("@aid", "NVARCHAR", user.UserID.ToString()));
    //            MyParams.Add(new Params("@uname", "NVARCHAR", user.UserName));
    //            MyParams.Add(new Params("@ustatus", "INT", 0));
    //            MyParams.Add(new Params("@rid", "NVARCHAR", user.RoleID.ToString()));
    //            MyParams.Add(new Params("@cDate", "DATETIME", user.CreatedDate));
    //            MyParams.Add(new Params("@cUser", "NVARCHAR", user.CreatedBy.ToString()));
    //            MyParams.Add(new Params("@aFlag", "BIT", user.AgentFlag));
    //            MyParams.Add(new Params("@uFullName", "NVARCHAR", user.UserFullName));
    //            MyParams.Add(new Params("@uCMID", "NVARCHAR", user.UserCMID));
    //            MyParams.Add(new Params("@uRemarks", "NVARCHAR", user.Remarks));
    //            MyParams.Add(new Params("@mCardReplenishment", "BIT", user.CardReplenishment));
    //            MyParams.Add(new Params("@mChequeReplenishment", "BIT", user.ChequeReplenishment));
    //            MyParams.Add(new Params("@mConsumable", "BIT", user.ConsumableCollection));
    //            MyParams.Add(new Params("@mSecurity", "BIT", user.Security));
    //            MyParams.Add(new Params("@mTroubleshoot", "BIT", user.Troubleshoot));
    //            MyParams.Add(new Params("@mLocalUSER", "BIT", 0));

    //            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //Logger.LogToFile("InsertUser.log", ex);
    //        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
    //                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
    //                    System.Reflection.MethodBase.GetCurrentMethod().Name,
    //                    ex);
    //    }
    //    return ret;
    //}

    public string IsValidatedToInactive_User(Guid id)
    {
        string ret = "";
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from user_login ul ");
            sql.AppendLine("join user_group ug on (ul.aid = ug.aid) ");
            sql.AppendLine("join machine_group mg on (ug.kid = mg.kid) ");
            sql.AppendLine("where ul.aid = :aid and mg.kStatus = :status ");
            MyParams.Add(new Params(":aid", "GUID", id));
            MyParams.Add(new Params(":status", "INT", 1));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            foreach (DataRow dr in dt.Rows)
                ret += "The User is assigned to Machine Group, " + dr["kDesc"].ToString() + "! The User Cannot be Inactive! <br />";
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    //public bool UpdateUser(User user)
    //{
    //    bool ret = false;
    //    try
    //    {
    //        sql.Clear();
    //        MyParams.Clear();
    //        sql.Append("update User_Login set ");
    //        if (!isUserAvailable(user.UserName, user.UserID))
    //        {
    //            sql.Append("UPPER(uName) = UPPER(:newName), ");
    //            MyParams.Add(new Params(":newName", "NVARCHAR", user.UserName));
    //        }

    //        sql.Append("rID = :newRole, AgentFlag = :aFlag, uFullName = :uFullName, uCMID = :uCMID, mCardReplenishment = :mCardReplenishment, mChequeReplenishment = :mChequeReplenishment, mConsumable = :mConsumable, mSecurity = :mSecurity, mTroubleshoot = :mTroubleshoot, uRemarks = :uRemarks, uStatus = :uStatus, ");
    //        sql.Append("uUpdatedDate = :uUpdatedDate, uUpdatedBy = :uUpdatedBy ");
    //        sql.Append("where aID = :aid ");
    //        MyParams.Add(new Params(":newRole", "NVARCHAR", user.RoleID));
    //        MyParams.Add(new Params(":aFlag", "BIT", user.AgentFlag));
    //        MyParams.Add(new Params(":uFullName", "NVARCHAR", user.UserFullName));
    //        MyParams.Add(new Params(":uCMID", "NVARCHAR", user.UserCMID));
    //        MyParams.Add(new Params(":uRemarks", "NVARCHAR", user.Remarks));
    //        MyParams.Add(new Params(":ustatus", "INT", user.Status));
    //        MyParams.Add(new Params(":uUpdatedDate", "DATETIME", user.UpdatedDate));
    //        MyParams.Add(new Params(":uUpdatedBy", "GUID", user.UpdatedBy));
    //        MyParams.Add(new Params(":aid", "NVARCHAR", user.UserID));
    //        MyParams.Add(new Params(":mCardReplenishment", "BIT", user.CardReplenishment));
    //        MyParams.Add(new Params(":mChequeReplenishment", "BIT", user.ChequeReplenishment));
    //        MyParams.Add(new Params(":mConsumable", "BIT", user.ConsumableCollection));
    //        MyParams.Add(new Params(":mSecurity", "BIT", user.Security));
    //        MyParams.Add(new Params(":mTroubleshoot", "BIT", user.Troubleshoot));
    //        ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
    //    }
    //    catch (Exception ex)
    //    {
    //        //Logger.LogToFile("UpdateUser.log", ex);
    //        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
    //                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
    //                    System.Reflection.MethodBase.GetCurrentMethod().Name,
    //                    ex);
    //    }
    //    return ret;
    //}

    public DataTable GetUpdateUser(Guid updateUserGuid)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("SELECT UL.*, UR.*, UCREATE.UNAME CREATEDBYNAME , L.* FROM USER_LOGIN UL ");
            sql.AppendLine("INNER JOIN USER_ROLES UR ON UL.RID = UR.RID ");
            sql.AppendLine("INNER JOIN USER_LOGIN UCREATE ON UCREATE.AID = UL.UCREATEDBY ");
            sql.AppendLine("LEFT JOIN LOCALUSER L ON L.LOGINID = UL.AID ");
            sql.AppendLine("Where UL.aID=:aid");
            MyParams.Add(new Params(":aid", "NVARCHAR", updateUserGuid));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("GetUpdateUser.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public DataTable GetUpdateUserCY(Guid updateUserGuid)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine(@" select UR.rdesc, UL.*, UCreate.uName CreatedByName, uupdate.uname UpdatedByName
                    from   User_Login UL
                    left join User_Roles UR on (UL.rID = UR.rID) left join User_Login UCreate on (UCreate.aID = UL.uCreatedBy) left join User_Login UUpdate on (UUpdate.aID = UL.uUpdatedBy)
                    where UL.aID=:aid");
            MyParams.Add(new Params(":aid", "NVARCHAR", updateUserGuid));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("GetUpdateUser.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    #endregion



    #region User Management Function
    public DataTable GetRoleUser()
    {
        sql.Clear();
        MyParams.Clear();
        DataTable dt = new DataTable();
        DataTable dtUR = new DataTable("UserRoles");
        try
        {
            dtUR.Columns.Add("URType", typeof(string));
            dtUR.Columns.Add("ID", typeof(Guid));
            dtUR.Columns.Add("Desc", typeof(string));
            dtUR.Columns.Add("Role", typeof(string));
            dtUR.Columns.Add("Status", typeof(string));
            dtUR.Columns.Add("CreatedDate", typeof(DateTime));
            dtUR.Columns.Add("ApprovedDate", typeof(DateTime));
            dtUR.Columns.Add("DeclineDate", typeof(DateTime));
            dtUR.Columns.Add("CreatedBy", typeof(string));
            dtUR.Columns.Add("ApprovedBy", typeof(string));
            dtUR.Columns.Add("DeclinedBy", typeof(string));


            sql.AppendLine("select UR.*, UL.*, CASE UR.rStatus ");
            //sql.AppendLine("WHEN 1 THEN 'Approved' ");
            //sql.AppendLine("WHEN 2 THEN 'Declined' ");
            sql.AppendLine("WHEN 1 THEN 'Active' ");
            sql.AppendLine("WHEN 2 THEN 'Inactive' ");
            sql.AppendLine("ELSE 'Pending' ");
            sql.AppendLine("END roleStatus, CASE UL.uStatus ");
            //sql.AppendLine("WHEN 1 THEN 'Approved' ");
            //sql.AppendLine("WHEN 2 THEN 'Declined' ");
            sql.AppendLine("WHEN 1 THEN 'Active' ");
            sql.AppendLine("WHEN 2 THEN 'Inactive' ");
            sql.AppendLine("ELSE 'Pending' ");
            sql.AppendLine("END userStatus, ");
            sql.AppendLine("ULC.uName uCreatedName, ULA.uName uApprovedName, ULD.uName uDeclinedName, ");
            sql.AppendLine("URC.uName rCreatedName, URA.uName rApprovedName, URD.uName rDeclinedName ");
            sql.AppendLine("from User_Roles UR left join User_Login UL on (UR.rID = UL.rID) ");
            sql.AppendLine("left join User_Login ULC on (UL.uCreatedBy = ULC.aID) ");
            sql.AppendLine("left join User_Login ULA on (UL.uApprovedBy = ULA.aID) ");
            sql.AppendLine("left join User_Login ULD on (UL.uDeclineBy = ULD.aID) ");
            sql.AppendLine("left join User_Login URC on (UR.rCreatedBy = URC.aID) ");
            sql.AppendLine("left join User_Login URA on (UR.rApprovedBy = URA.aID) ");
            sql.AppendLine("left join User_Login URD on (UR.rDeclineBy = URD.aID) ");
            sql.AppendLine("order by UR.rDesc, UL.uName ");
            dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

            DataRow[] dr = dt.DefaultView.ToTable(true, new String[] { "rID", "rDesc", "roleStatus", "rCreatedDate", "rApprovedDate", "rDeclineDate", "rCreatedName", "rApprovedName", "rDeclinedName" }).Select();
            foreach (DataRow row in dr)
            {
                dtUR.Rows.Add("Roles", row["rID"], row["rDesc"], null, row["roleStatus"], row["rCreatedDate"], row["rApprovedDate"], row["rDeclineDate"], row["rCreatedName"], row["rApprovedName"], row["rDeclinedName"]);
            }


            dr = dt.DefaultView.ToTable(true, new String[] { "aid", "uName", "userStatus", "rDesc", "uCreatedDate", "uApprovedDate", "uDeclineDate", "uCreatedName", "uApprovedName", "uDeclinedName" }).Select("aid <> ''", "uName");
            foreach (DataRow row in dr)
            {
                dtUR.Rows.Add("User", row["aid"], row["uName"], row["rDesc"], row["userStatus"], row["uCreatedDate"], row["uApprovedDate"], row["uDeclineDate"], row["uCreatedName"], row["uApprovedName"], row["uDeclinedName"]);
            }

            if (dtUR.Rows.Count != 0)
            {
                return dtUR;
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("GetRoleUser.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return dtUR;
    }
    public DataTable GetRoles()
    {
        sql.Clear();
        MyParams.Clear();
        DataTable ret = new DataTable();
        try
        {
            sql.AppendLine("select ur.rid, ur.rdesc, ur.rcreateddate, ur.rapproveddate, ur.rdeclinedate, ur.rupdateddate, ur.rremarks");
            sql.AppendLine(", ulc.uName CreatedByName, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.UNAME updatedbyName");
            sql.AppendLine(", ur.rStatus ");
            sql.AppendLine("from User_Roles ur ");
            sql.AppendLine("left join User_Login ulc on (ur.rCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (ur.rApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (ur.rDeclineBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (ur.rUpdatedBy = ulu.aID) ");

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public DataTable GetUser()
    {
        sql.Clear();
        MyParams.Clear();
        DataTable ret = new DataTable();
        try
        {
            sql.AppendLine("select ul.aid, ul.ufullname, ul.uname, ul.ucreateddate, ul.uapproveddate, ul.udeclinedate");
            sql.AppendLine(", ul.agentflag, ul.ufullname, ul.ucmid, ul.uupdateddate, ul.ulastlogindate, ul.usessionid, ul.sessionkey");
            sql.AppendLine(", ul.uremarks, ul.loginflag, ul.mcardreplenishment, ul.mchequereplenishment");
            sql.AppendLine(", ul.mconsumable, ul.msecurity, ul.mtroubleshoot ");
            sql.AppendLine(", ur.rDesc roleName ");
            sql.AppendLine(", ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.UNAME updatedby");
            sql.AppendLine(", case ul.uStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end Status ");
            sql.AppendLine("from User_Login ul ");
            sql.AppendLine("left join User_Roles ur on (ul.rid = ur.rid) ");
            sql.AppendLine("left join User_Login ulc on (ul.uCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (ul.uApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (ul.uDeclineBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (ul.uUpdatedBy = ulu.aID) ");

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public bool ApproveDeclineRoleUser(string roleUser, string desc, Guid adminGuid, string approveDecline)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            switch (approveDecline)
            {
                case "Approve":
                    if (roleUser == "Roles")
                    {
                        sql.AppendLine("update User_Roles set rStatus = 1, rApprovedDate=:datetime, rApprovedBy=:aid where rDesc=:id");
                    }
                    else if (roleUser == "User")
                    {
                        sql.AppendLine("update User_Login set uStatus = 1, uApprovedDate=:datetime, uApprovedBy=:aid where UPPER(uName)=UPPER(:id)");
                    }
                    MyParams.Add(new Params(":aid", "NVARCHAR", adminGuid));
                    break;
                case "Decline":
                    if (roleUser == "Roles")
                    {
                        sql.AppendLine("update User_Roles set rStatus = 2, rDeclineDate=:datetime, rDeclineBy=:did where rDesc=:id");
                    }
                    else if (roleUser == "User")
                    {
                        sql.AppendLine("update User_Login set uStatus = 2, uDeclineDate=:datetime, uDeclineBy=:did where UPPER(uName)=UPPER(:id)");
                    }
                    MyParams.Add(new Params(":did", "NVARCHAR", adminGuid));
                    break;
                default:
                    break;
            }
            MyParams.Add(new Params(":id", "NVARCHAR", desc));
            MyParams.Add(new Params(":datetime", "DATETIME", DateTime.Now));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("ApproveDeclineRoleUser.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public bool DeleteRoleUser(string roleUser, string desc, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            if (roleUser == "Roles")
            {
                sql.AppendLine("delete User_Roles where rdesc=:id");
            }
            else if (roleUser == "User")
            {
                sql.AppendLine("delete User_Login where UPPER(uname)=UPPER(:id)");
            }
            MyParams.Add(new Params(":id", "NVARCHAR", desc));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("ApproveDeclineRoleUser.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public bool DeleteRoleById(Guid roleId)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("DELETE User_Roles WHERE RID=:id");
            MyParams.Add(new Params(":id", "GUID", roleId));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public bool ApproveRoleById(Guid roleId, Guid checker)
    {
        bool ret = false;
        int stat = 1;

        try
        {
            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("Update User_Roles set rStatus=:status, rApprovedDate=:datetime, rApprovedBy=:checkId");
            sql.AppendLine("where rid=:id");

            MyParams.Add(new Params(":status", "INT", stat));
            MyParams.Add(new Params(":checkId", "NVARCHAR", checker));
            MyParams.Add(new Params(":datetime", "DATETIME", DateTime.Now));
            MyParams.Add(new Params(":id", "GUID", roleId));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public bool DeleteUserById(Guid userId)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("DELETE User_Login WHERE aID=:id");
            MyParams.Add(new Params(":id", "GUID", userId));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("ApproveDeclineRoleUser.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    public bool ApproveUserById(Guid roleId, Guid checker)
    {
        bool ret = false;
        int stat = 1;

        try
        {
            sql.Clear();
            MyParams.Clear();

            sql.AppendLine("Update User_Login set uStatus=:status, uApprovedDate=:datetime, uApprovedBy=:checkId");
            sql.AppendLine("where aid=:id");

            MyParams.Add(new Params(":status", "INT", stat));
            MyParams.Add(new Params(":checkId", "NVARCHAR", checker));
            MyParams.Add(new Params(":datetime", "DATETIME", DateTime.Now));
            MyParams.Add(new Params(":id", "GUID", roleId));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    #endregion


    #region getpermission
    public static void getModulePermission(Guid userid, Guid moduleid, out bool View, out bool Maker, out bool Checker)
    {
        View = false;
        Maker = false;
        Checker = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select up.mview,up.mMaker,up.mChecker");
            sql.AppendLine("from user_login ul, user_permission up");
            sql.AppendLine("where ul.rid = up.rid");
            sql.AppendLine("and ul.aid = :userid");
            sql.AppendLine("and up.mid = :moduleid");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params(":userid", "NVARCHAR", userid));
            MyParams.Add(new Params(":moduleid", "NVARCHAR", moduleid));

            string[] tmpdata = Regex.Split(dbController.Output(sql.ToString(), "connectionString", MyParams).ToString(), "#SPLIT#");
            if (tmpdata[0].ToString() == "1")
                View = true;
            if (tmpdata[1].ToString() == "1")
                Maker = true;
            if (tmpdata[2].ToString() == "1")
                Checker = true;


        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getModulePermission.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
    }
    #endregion

    public void disabledormant()
    {
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("UPDATE USER_LOGIN SET USTATUS='2' WHERE ULASTLOGINDATE <= TRUNC(sysdate)-90");
            dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getModulePermission.log",ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
    }

    #region CY Special Local User

    public bool InsertUser(User user)
    {
        bool ret = false;
        bool ret2 = false;
        try
        {
            if (isUserAvailable(user.UserName, user.UserID))
            {
                sql.Clear();
                MyParams.Clear();
                sql.Append("insert into User_Login (aID, uName, uFullName, email, uCMID, rID, AgentFlag, mCardReplenishment, mChequeReplenishment, mConsumable, mSecurity, mTroubleshoot, uRemarks, uStatus, uCreatedDate, uCreatedBy, LocalUser ) ");
                sql.Append("values (:aid, UPPER(:uname), :uFullName, :email, :uCMID, :rid, :aFlag, :mCardReplenishment, :mChequeReplenishment, :mConsumable, :mSecurity, :mTroubleshoot, :uRemarks, :ustatus, :cDate, :cUser, :LocalUSER )");
                MyParams.Add(new Params(":aid", "NVARCHAR", user.UserID));
                MyParams.Add(new Params(":uname", "NVARCHAR", user.UserName));
                MyParams.Add(new Params(":email", "NVARCHAR", user.Email));
                MyParams.Add(new Params(":ustatus", "INT", 0));
                MyParams.Add(new Params(":rid", "NVARCHAR", user.RoleID));
                MyParams.Add(new Params(":cDate", "DATETIME", user.CreatedDate));
                MyParams.Add(new Params(":cUser", "NVARCHAR", user.CreatedBy));
                MyParams.Add(new Params(":aFlag", "BIT", user.AgentFlag));
                MyParams.Add(new Params(":uFullName", "NVARCHAR", user.UserFullName));
                MyParams.Add(new Params(":uCMID", "NVARCHAR", user.UserCMID));
                MyParams.Add(new Params(":uRemarks", "NVARCHAR", user.Remarks));
                MyParams.Add(new Params(":mCardReplenishment", "BIT", user.CardReplenishment));
                MyParams.Add(new Params(":mChequeReplenishment", "BIT", user.ChequeReplenishment));
                MyParams.Add(new Params(":mConsumable", "BIT", user.ConsumableCollection));
                MyParams.Add(new Params(":mSecurity", "BIT", user.Security));
                MyParams.Add(new Params(":mTroubleshoot", "BIT", user.Troubleshoot));
                MyParams.Add(new Params(":LocalUSER", "BIT", user.LocalUser));

                ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
                if (user.LocalUser)
                {
                    string passwordEncoded = MsgSec.EncryptString(user.Password);
                    sql.Clear();
                    MyParams.Clear();
                    sql.Append("insert into LocalUser (LoginID, lPassword) ");
                    sql.Append("values (:aid, :pass, :email)");
                    MyParams.Add(new Params(":aid", "NVARCHAR", user.UserID));
                    MyParams.Add(new Params(":pass", "NVARCHAR", passwordEncoded));
                    ret2 = dbController.Input(sql.ToString(), "connectionString", MyParams);
                }
                else
                {
                    ret2 = true;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
        return ret && ret2;
    }
    public bool UpdateUser(User user)
    {
        bool ret = false;
        bool ret2 = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.Append("update User_Login set ");
            sql.Append("rID = :newRole, AgentFlag = :aFlag, uFullName = :uFullName, Email=:uEmail, uCMID = :uCMID, mCardReplenishment = :mCardReplenishment, mChequeReplenishment = :mChequeReplenishment, mConsumable = :mConsumable, mSecurity = :mSecurity, mTroubleshoot = :mTroubleshoot, uRemarks = :uRemarks, uStatus = :uStatus, LocalUser = :LocalUser,  ");
            sql.Append("uUpdatedDate = :uUpdatedDate, uUpdatedBy = :uUpdatedBy ");
            sql.Append("where aID = :aid ");
            MyParams.Add(new Params(":newRole", "NVARCHAR", user.RoleID));
            MyParams.Add(new Params(":aFlag", "BIT", user.AgentFlag));
            MyParams.Add(new Params(":uFullName", "NVARCHAR", user.UserFullName));
            MyParams.Add(new Params(":uEmail", "NVARCHAR", user.Email));
            MyParams.Add(new Params(":uCMID", "NVARCHAR", user.UserCMID));
            MyParams.Add(new Params(":uRemarks", "NVARCHAR", user.Remarks));
            MyParams.Add(new Params(":ustatus", "INT", user.Status));
            MyParams.Add(new Params(":uUpdatedDate", "DATETIME", user.UpdatedDate));
            MyParams.Add(new Params(":uUpdatedBy", "GUID", user.UpdatedBy));
            MyParams.Add(new Params(":aid", "NVARCHAR", user.UserID));
            MyParams.Add(new Params(":mCardReplenishment", "BIT", user.CardReplenishment));
            MyParams.Add(new Params(":mChequeReplenishment", "BIT", user.ChequeReplenishment));
            MyParams.Add(new Params(":mConsumable", "BIT", user.ConsumableCollection));
            MyParams.Add(new Params(":mSecurity", "BIT", user.Security));
            MyParams.Add(new Params(":mTroubleshoot", "BIT", user.Troubleshoot));
            MyParams.Add(new Params(":LocalUSER", "BIT", user.LocalUser));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
            if (user.LocalUser)
            {
                if (user.Password != "")
                {
                    sql.Clear();
                    MyParams.Clear();
                    sql.Append("update LocalUser set ");
                    sql.Append("lPassWord = :pass where LoginID = :loginid ");
                    string passwordEncoded = MsgSec.EncryptString(user.Password);
                    MyParams.Add(new Params(":pass", "NVARCHAR", passwordEncoded));
                    MyParams.Add(new Params(":loginid", "GUID", user.UserID));
                    ret2 = dbController.Input(sql.ToString(), "connectionString", MyParams);
                }
                else
                {
                    ret2 = true;
                }
            }
            else
            {
                ret2 = true;
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UpdateUser.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
        return ret && ret2;
    }
    public bool ResetPassword(Guid UID)
    {
        //bool ret = false;
        throw new NotImplementedException("outdated method.");
        //try
        //{
        //    sql.Clear();
        //    MyParams.Clear();
        //    sql.Append("update LocalUser set ");
        //    sql.Append("lPassWord = :pass where LoginID = :loginid ");

        //    MyParams.Add(new Params(":pass", "NVARCHAR", "LmpooT0l2ok=")); //123456
        //    MyParams.Add(new Params(":loginid", "GUID", UID));
        //    ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        //}
        //catch (Exception ex)
        //{
        //    //Logger.LogToFile("UpdateUser.log", ex);
        //    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
        //                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
        //                System.Reflection.MethodBase.GetCurrentMethod().Name,
        //                ex);
        //}
        //return ret;
    }
    public bool ResetPassword(Guid UID, string newPassword)
    {
        bool ret = false;

        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.Append("update LocalUser set ");
            sql.Append("lPassWord = :pass where LoginID = :loginid ");

            MyParams.Add(new Params(":pass", "NVARCHAR", MsgSec.EncryptString(newPassword)));
            MyParams.Add(new Params(":loginid", "GUID", UID));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UpdateUser.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    #endregion
}