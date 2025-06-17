using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Text;
using System.Text.RegularExpressions;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Common.Business.Component;
using MDM.iOS.Common.Data.Component;
using MDM.iOS.Entities;
using MDM.iOS.Entities.Dashboard;
using Newtonsoft.Json;

/// <summary>
/// The class is used for authentication and user management purposes. 
/// </summary>
public class UserManageDefault : UserManageBase
{

    public override void getModulePermission(Guid userid, Guid moduleid, out bool View, out bool Maker, out bool Checker)
    {
        View = false;
        Maker = false;
        Checker = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select up.mview,up.mMaker,up.mChecker");
            sql.AppendLine("from user_login ul ");
            sql.AppendLine("join user_permission up on (ul.rid = up.rid) ");
            sql.AppendLine("where ul.uid = @userid ");
            sql.AppendLine("and up.mid = @moduleid");

            List<Params> MyParams = new List<Params>();
            MyParams.Add(new Params("@userid", "GUID", userid));
            MyParams.Add(new Params("@moduleid", "GUID", moduleid));

            string[] tmpdata = Regex.Split(SqlDataControl.Output(sql.ToString(), MyParams).ToString(), "#SPLIT#");
            //if (tmpdata[0].ToString() == "1")
            //    View = true;
            //if (tmpdata[1].ToString() == "1")
            //    Maker = true;
            //if (tmpdata[2].ToString() == "1")
            //    Checker = true;
            View = Convert.ToBoolean(tmpdata[0].ToString());
            Maker = Convert.ToBoolean(tmpdata[1].ToString());
            Checker = Convert.ToBoolean(tmpdata[2].ToString());

        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
    }

    /// Used to Check is the username exist in the Active Directory
    public override bool isActDirAuthenticated(string adPath, string adDomain, string username, string pwd)//, out string path)
    {
        bool ret = false;
        //path = adPath;
        try
        {
            string domainAndUsername = adDomain + @"\" + username;

            entry = new DirectoryEntry(adPath, domainAndUsername, pwd);
            //HttpContext.Current.Session["ADEntry"] = entry;

            //Bind to the native AdsObject to force authentication.
            object obj = entry.NativeObject;

            DirectorySearcher search = new DirectorySearcher(entry);

            search.Filter = "(SAMAccountName=" + username + ")";
            //search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            if (null == result)
            {
                ret = false;
            }
            else
            {
                //path = result.Path;
                ret = true;
            }

        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ex);
            ret = false;
        }

        return ret;
    }


    public override DataTable getUserNamebyUserId(Guid iD)
    {
        DataTable ret = new DataTable();
        StringBuilder sql = new StringBuilder();
        List<Params> myParams = new List<Params>();

        try
        {
            sql.AppendLine(@"select uName from User_Login where aID = @uid");
            myParams.Add(new Params("@uid", "GUID", iD));
            ret = SqlDataControl.GetResult(sql.ToString(), myParams);
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

    public override bool isActDirUserExists(string username)
    {
        bool ret = false;
        try
        {
            //string domainAndUsername = adDomain + @"\" + username;

            //DirectoryEntry entry = new DirectoryEntry(adPath, domainAndUsername, pwd);
            //DirectoryEntry entry = 

            using (DirectorySearcher searcher = new DirectorySearcher(entry))
            {
                searcher.Filter = "(&(objectClass=user)(sAMAccountName=" + username + "))";

                using (SearchResultCollection results = searcher.FindAll())
                {
                    if (results.Count > 0)
                        ret = true;
                    else
                        ret = false;
                }
            }

        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ex);
            ret = false;
        }

        return ret;
    }

    #region Users

    public override bool isUserExist(string username)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select aID, uName, uStatus, rID, bID, uUserType, uisLogin, uisReset, uisSuspend, uLoginAttempt, uLastLoginDate, uSessionID, uCreatedDate, uApprovedDate, uDeclinedDate, uUpdatedDate, uCreatedBy, uApprovedBy, uDeclinedBy, uUpdatedBy, staffID ");
            sql.AppendLine("from User_Login where uName=@username ");
            MyParams.Add(new Params("@username", "NVARCHAR", username));
            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);
            if (dt.Rows.Count > 0)
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
        }
        return ret;
    }

    /// Use to Check SA 
    public override bool LoginVerificationForSA(string password)
    {
        bool ret = false;

        string path = ConfigurationManager.AppSettings["IniPath"];
        try
        {

            // cant read path. 
            StreamReader sw = new StreamReader(path + "PasswordHistorySA.ini");

            string ret_string = sw.ReadLine();
            sw.Close();
            IniEntity ini = JsonConvert.DeserializeObject<IniEntity>(ret_string);

            /*const string masterKey = "d7552dd2dc2c40d8a75ba66e"; 
            string password_compare = Crypt.TDESEncrypt(password, masterKey); 

            if (ini.newpassword == password_compare)
            {
                ret = true;
            }*/
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


    /// Use to Check is the username exist in the Database
    public override bool isActiveUserAvailable(string username)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select aID, uName, uStatus, rID, bID, uUserType, uisLogin, uisReset, uisSuspend, uLoginAttempt, uLastLoginDate, uSessionID, uCreatedDate, uApprovedDate, uDeclinedDate, uUpdatedDate, uCreatedBy, uApprovedBy, uDeclinedBy, uUpdatedBy, staffID "); //add back "groupID"
            sql.AppendLine("from User_Login where uName=@username and ustatus = 1 ");
            MyParams.Add(new Params("@username", "NVARCHAR", username));

            // when stepped into this code, it gives an exception.
            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);
            if (dt.Rows.Count > 0)
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
        }
        return ret;
    }

    public override bool setLogin(string sessionID, Guid userID)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("update User_Login set ");
            sql.AppendLine("uLastLoginDate = @uLastLoginDate , uSessionID = @uSessionID , uisLogin = 1, uLoginAttempt = 0 ");
            sql.AppendLine("where aID=@userID ");
            myParams.Add(new Params("@uLastLoginDate", "DATETIME", DateTime.Now));
            myParams.Add(new Params("@uSessionID", "NVARCHAR", sessionID));
            myParams.Add(new Params("@userID", "GUID", userID));
            ret = SqlDataControl.Input(sql.ToString(), myParams);
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

    public override bool setLogout(Guid userID)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("update User_Login set ");
            sql.AppendLine("uisLogin = 0 ");
            sql.AppendLine("where aID=@userID ");
            myParams.Add(new Params("@userID", "GUID", userID));
            ret = SqlDataControl.Input(sql.ToString(), myParams);
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

    public override bool setSuspend(Guid userID)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("update User_Login set ");
            sql.AppendLine("uisSuspend = @uisSuspend ");
            sql.AppendLine("where aID=@userID ");
            myParams.Add(new Params("@uisSuspend", "BIT", true));
            myParams.Add(new Params("@userID", "GUID", userID));
            ret = SqlDataControl.Input(sql.ToString(), myParams);
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

    public override int increaseLoginAttempt(int loginAttempt, Guid userID)
    {
        int ret = loginAttempt + 1;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("update User_Login set ");
            sql.AppendLine("uLoginAttempt = @uLoginAttempt ");
            sql.AppendLine("where aID=@userID ");
            myParams.Add(new Params("@uLoginAttempt", "INT", ret));
            myParams.Add(new Params("@userID", "GUID", userID));
            if (SqlDataControl.Input(sql.ToString(), myParams))
            {

            }
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

    public override DataTable getUsers(List<Params> lParams)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            //List<Params> MyParams = new List<Params>();

            sql.AppendLine("select ul.aID, ul.uName, ul.rID, ul.bID, ul.uUserType, ul.uisLogin, ul.uisReset, ul.uisSuspend, ul.uLoginAttempt, ul.uLastLoginDate, ul.uSessionID, ul.uCreatedDate, ul.uApprovedDate, ul.uDeclinedDate, ul.uUpdatedDate, ul.uCreatedBy, ul.uApprovedBy, ul.uDeclinedBy, ul.uUpdatedBy ");
            sql.AppendLine(", SYSDATETIME() CurrentDate ");
            sql.AppendLine(", case (select top 1 tblstatus from Data_Trail ut where ut.tblid = ul.uid order by ut.editeddate DESC) ");
            sql.AppendLine("when 3 then 3 else ustatus end uStatus, ");
            sql.AppendLine("case ");
            sql.AppendLine("(case (select top 1 tblstatus from Data_Trail ut where ut.tblid = ul.uid order by ut.editeddate DESC) when 3 then 3 else ustatus end) ");
            sql.AppendLine("when 0 then 'Suspended' when 1 then 'Active' when 2 then 'Pending' when 3 then 'Edited' end Status, ");
            sql.AppendLine("ul.uname CreatedBy, ur.rDesc RoleName ");
            sql.AppendLine("from User_Login ul ");
            sql.AppendLine("join User_Roles ur on (ul.rID = ur.rID) ");
            sql.AppendLine("Where 1=1");

            foreach (Params subparams in lParams)
            {
                if (subparams.dataName.ToUpper() == "@UNAME")
                {
                    sql.AppendLine("and ul.uname like @uname");
                }

                if (subparams.dataName.ToUpper() == "@RID")
                {
                    sql.AppendLine("and ul.rid= @rid");
                }

                // 07-09-2018 Sean : Add filter by user status
                if (subparams.dataName.ToUpper() == "@USTATUS")
                {
                    sql.AppendLine("and case (select top 1 tblstatus from Data_Trail ut where ut.tblid = ul.uid order by ut.editeddate DESC) ");
                    sql.AppendLine("when 3 then 3 else ustatus end = @ustatus");
                }
            }
            //sql.AppendLine("where uStatus = 1 ");
            sql.AppendLine("order by uname");

            ret = SqlDataControl.GetResult(sql.ToString(), lParams);
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
    public override UIUserEn getAllUserDetailsReturnEntity(Guid userID)
    {
        UIUserEn ret = new UIUserEn();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select aID, uName, uStatus, rID, bID, uemail, fullname, staffID, uUserType, uisLogin, uisReset, uisSuspend, uLoginAttempt, uLastLoginDate, uSessionID, uCreatedDate, uApprovedDate, uDeclinedDate, uUpdatedDate, uCreatedBy, uApprovedBy, uDeclinedBy, uUpdatedBy ");
            sql.AppendLine(", SYSDATETIME() CurrentDate ");
            sql.AppendLine("from User_Login ");
            sql.AppendLine("where aID=@userID ");
            MyParams.Add(new Params("@userID", "GUID", userID));
            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);
            if (dt.Rows.Count == 1)
            {
                ret.UserID = Guid.Parse(dt.Rows[0]["aID"].ToString());
                ret.UserName = dt.Rows[0]["uName"].ToString();
                ret.Status = Convert.ToInt32(dt.Rows[0]["uStatus"]);
                ret.RoleID = Guid.Parse(dt.Rows[0]["rID"].ToString());
                ret.BranchID = Guid.Parse(dt.Rows[0]["bID"].ToString());
                ret.FullName = dt.Rows[0]["Fullname"].ToString();
                ret.Email = dt.Rows[0]["uEmail"].ToString();
                ret.StaffID = dt.Rows[0]["staffID"].ToString();
                //ret.GroupID = dt.Rows[0]["groupID"].ToString();
                ret.UserType = dt.Rows[0]["uUserType"].ToString();
                ret.IsLogin = Convert.ToBoolean(dt.Rows[0]["uisLogin"]);
                ret.Reset = Convert.ToBoolean(dt.Rows[0]["uisReset"]);
                ret.IsSuspend = Convert.ToBoolean(dt.Rows[0]["uisSuspend"]);
                ret.LoginAttempt = Convert.ToInt32(dt.Rows[0]["uLoginAttempt"]);
                ret.LastLoginDate = Convert.ToDateTime(dt.Rows[0]["uLastLoginDate"]);
                ret.SessionID = dt.Rows[0]["uSessionID"].ToString();
                ret.CreatedDate = Convert.ToDateTime(dt.Rows[0]["uCreatedDate"]);
                ret.ApprovedDate = Convert.ToDateTime(dt.Rows[0]["uApprovedDate"]);
                ret.DeclinedDate = Convert.ToDateTime(dt.Rows[0]["uDeclinedDate"]);
                ret.UpdatedDate = Convert.ToDateTime(dt.Rows[0]["uUpdatedDate"]);
                ret.CreatedBy = Guid.Parse(dt.Rows[0]["uCreatedBy"].ToString());
                ret.ApprovedBy = Guid.Parse(dt.Rows[0]["uApprovedBy"].ToString());
                ret.DeclinedBy = Guid.Parse(dt.Rows[0]["uDeclinedBy"].ToString());
                ret.UpdatedBy = Guid.Parse(dt.Rows[0]["uUpdatedBy"].ToString());
                ret.CurrentDate = Convert.ToDateTime(dt.Rows[0]["CurrentDate"]);

                ret.ReturnStatus = "SUCCESS";
            }
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


    public override UIUserEn getUserDetails(Guid userID)
    {
        UIUserEn ret = new UIUserEn();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select aID, uName, uStatus, rID, bID, uemail, fullname, staffID, uUserType, uisLogin, uisReset, uisSuspend, uLoginAttempt, uLastLoginDate, uSessionID, uCreatedDate, uApprovedDate, uDeclinedDate, uUpdatedDate, uCreatedBy, uApprovedBy, uDeclinedBy, uUpdatedBy ");
            sql.AppendLine(", SYSDATETIME() CurrentDate ");
            sql.AppendLine("from User_Login ");
            sql.AppendLine("where aID=@userID and ustatus = 1 ");
            MyParams.Add(new Params("@userID", "GUID", userID));
            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);
            if (dt.Rows.Count == 1)
            {
                ret.UserID = Guid.Parse(dt.Rows[0]["aID"].ToString());
                ret.UserName = dt.Rows[0]["uName"].ToString();
                ret.Status = Convert.ToInt32(dt.Rows[0]["uStatus"]);
                ret.RoleID = Guid.Parse(dt.Rows[0]["rID"].ToString());
                ret.BranchID = Guid.Parse(dt.Rows[0]["bID"].ToString());
                ret.FullName = dt.Rows[0]["Fullname"].ToString();
                ret.Email = dt.Rows[0]["uEmail"].ToString();
                ret.StaffID = dt.Rows[0]["staffID"].ToString();
                //ret.GroupID = dt.Rows[0]["groupID"].ToString();
                ret.UserType = dt.Rows[0]["uUserType"].ToString();
                ret.IsLogin = Convert.ToBoolean(dt.Rows[0]["uisLogin"]);
                ret.Reset = Convert.ToBoolean(dt.Rows[0]["uisReset"]);
                ret.IsSuspend = Convert.ToBoolean(dt.Rows[0]["uisSuspend"]);
                ret.LoginAttempt = Convert.ToInt32(dt.Rows[0]["uLoginAttempt"]);
                ret.LastLoginDate = Convert.ToDateTime(dt.Rows[0]["uLastLoginDate"]);
                ret.SessionID = dt.Rows[0]["uSessionID"].ToString();
                ret.CreatedDate = Convert.ToDateTime(dt.Rows[0]["uCreatedDate"]);
                ret.ApprovedDate = Convert.ToDateTime(dt.Rows[0]["uApprovedDate"]);
                ret.DeclinedDate = Convert.ToDateTime(dt.Rows[0]["uDeclinedDate"]);
                ret.UpdatedDate = Convert.ToDateTime(dt.Rows[0]["uUpdatedDate"]);
                ret.CreatedBy = Guid.Parse(dt.Rows[0]["uCreatedBy"].ToString());
                ret.ApprovedBy = Guid.Parse(dt.Rows[0]["uApprovedBy"].ToString());
                ret.DeclinedBy = Guid.Parse(dt.Rows[0]["uDeclinedBy"].ToString());
                ret.UpdatedBy = Guid.Parse(dt.Rows[0]["uUpdatedBy"].ToString());
                ret.CurrentDate = Convert.ToDateTime(dt.Rows[0]["CurrentDate"]);

                ret.ReturnStatus = "SUCCESS";
            }
            else
            {
                //ret.ReturnStatusCode = 404;
                ret.ReturnStatus = "User does not exist or duplicated user exist!";
            }
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

    public override Guid getUserID(string username)
    {
        Guid ret = Guid.Empty;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select aID ");
            sql.AppendLine("from User_Login ");
            sql.AppendLine("where uName=@uName and ustatus = 1 ");
            MyParams.Add(new Params("@uName", "NVARCHAR", username));
            string id = SqlDataControl.Output(sql.ToString(), MyParams);
            if (!string.IsNullOrEmpty(id))
            {
                ret = Guid.Parse(id.Trim());
            }
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

    public override List<UIUserEn> getUserIDBySupervisorID(Guid branchID)
    {
        List<UIUserEn> ret = new List<UIUserEn>();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select aID, uName ");
            sql.AppendLine("from User_Login ");
            sql.AppendLine("where bID=@bID and ustatus = 1 and uUserType <> 'supervisor' ");
            MyParams.Add(new Params("@bID", "GUID", branchID));
            ret = AssignEntity.CreateListFromTable<UIUserEn>(SqlDataControl.GetResult(sql.ToString(), MyParams));
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

    public override string getSessionID(Guid userID)
    {
        string ret = "";
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("select uSessionID from User_Login ");
            sql.AppendLine("where aID=@userID ");
            myParams.Add(new Params("@userID", "GUID", userID));
            ret = SqlDataControl.Output(sql.ToString(), myParams);
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

    public override bool insertUser(UIUserEn entity)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("insert into User_Login ");
            sql.AppendLine("(aID, uName, uStatus, rID, bID, uUserType, ");
            sql.AppendLine("uisLogin, uisReset, uisSuspend, uLoginAttempt, uLastLoginDate, uSessionID, ");
            sql.AppendLine("uCreatedDate, uApprovedDate, uDeclinedDate, uUpdatedDate, ");
            sql.AppendLine("uCreatedBy, uApprovedBy, uDeclinedBy, uUpdatedBy, FullName, uEmail, staffID) ");
            sql.AppendLine("VALUES ");
            sql.AppendLine("(@aID, @uName, @uStatus, @rID, @bID, @uUserType, ");
            sql.AppendLine("@uisLogin, @uisReset, @uisSuspend, @uLoginAttempt, @uLastLoginDate, @uSessionID, ");
            sql.AppendLine("@uCreatedDate, @uApprovedDate, @uDeclinedDate, @uUpdatedDate, ");
            sql.AppendLine("@uCreatedBy, @uApprovedBy, @uDeclinedBy, @uUpdatedBy, @FullName, @uEmail, @staffID) ");
            myParams.Add(new Params("@aID", "GUID", entity.UserID));
            myParams.Add(new Params("@uName", "NVARCHAR", entity.UserName));
            myParams.Add(new Params("@uStatus", "INT", entity.Status));
            myParams.Add(new Params("@rID", "GUID", entity.RoleID));
            myParams.Add(new Params("@bID", "GUID", entity.BranchID));
            myParams.Add(new Params("@uUserType", "NVARCHAR", entity.UserType));
            myParams.Add(new Params("@uisLogin", "BIT", entity.IsLogin));
            myParams.Add(new Params("@uisReset", "BIT", entity.Reset));
            myParams.Add(new Params("@uisSuspend", "BIT", entity.IsSuspend));
            myParams.Add(new Params("@uLoginAttempt", "INT", entity.LoginAttempt));
            myParams.Add(new Params("@uLastLoginDate", "DATETIME", entity.LastLoginDate));
            myParams.Add(new Params("@uSessionID", "NVARCHAR", entity.SessionID));
            myParams.Add(new Params("@uCreatedDate", "DATETIME", entity.CreatedDate));
            myParams.Add(new Params("@uApprovedDate", "DATETIME", entity.ApprovedDate));
            myParams.Add(new Params("@uDeclinedDate", "DATETIME", entity.DeclinedDate));
            myParams.Add(new Params("@uUpdatedDate", "DATETIME", entity.UpdatedDate));
            myParams.Add(new Params("@uCreatedBy", "GUID", entity.CreatedBy));
            myParams.Add(new Params("@uApprovedBy", "GUID", entity.ApprovedBy));
            myParams.Add(new Params("@uDeclinedBy", "GUID", entity.DeclinedBy));
            myParams.Add(new Params("@uUpdatedBy", "GUID", entity.UpdatedBy));
            myParams.Add(new Params("@FullName", "NVARCHAR", entity.FullName));
            myParams.Add(new Params("@uEmail", "NVARCHAR", entity.Email));
            myParams.Add(new Params("@staffID", "NVARCHAR", entity.StaffID));
            //myParams.Add(new Params("@groupID", "NVARCHAR", entity.GroupID));
            ret = SqlDataControl.Input(sql.ToString(), myParams);
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

    public override bool updateUser(UIUserEn entity)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("update User_Login set ");
            sql.AppendLine("uName=@uName, uStatus=@uStatus, rID=@rID, bID=@bID, uUserType=@uUserType, ");
            //sql.AppendLine("uisLogin=@uisLogin, uisReset=@uisReset, uisSuspend=@uisSuspend, uLoginAttempt=@uLoginAttempt, uLastLoginDate=@uLastLoginDate, uSessionID=@uSessionID, ");
            sql.AppendLine("uUpdatedDate=@uUpdatedDate, ");
            sql.AppendLine("uUpdatedBy=@uUpdatedBy, ");
            sql.AppendLine("uEmail=@uEmail, ");
            sql.AppendLine("FullName=@fullname ");
            sql.AppendLine("where aID=@aID ");
            myParams.Add(new Params("@aID", "GUID", entity.UserID));
            myParams.Add(new Params("@uName", "NVARCHAR", entity.UserName));
            myParams.Add(new Params("@uStatus", "INT", entity.Status));
            myParams.Add(new Params("@rID", "GUID", entity.RoleID));
            myParams.Add(new Params("@bID", "GUID", entity.BranchID));
            myParams.Add(new Params("@uUserType", "NVARCHAR", entity.UserType));
            myParams.Add(new Params("@uEmail", "NVARCHAR", entity.Email));
            myParams.Add(new Params("@fullname", "NVARCHAR", entity.FullName));
            //myParams.Add(new Params("@uisLogin", "BIT", entity.IsLogin));
            //myParams.Add(new Params("@uisReset", "BIT", entity.Reset));
            //myParams.Add(new Params("@uisSuspend", "BIT", entity.IsSuspend));
            //myParams.Add(new Params("@uLoginAttempt", "INT", entity.LoginAttempt));
            //myParams.Add(new Params("@uLastLoginDate", "DATETIME", entity.LastLoginDate));
            //myParams.Add(new Params("@uSessionID", "NVARCHAR", entity.SessionID));
            //myParams.Add(new Params("@uCreatedDate", "DATETIME", entity.CreatedDate));
            //myParams.Add(new Params("@uApprovedDate", "DATETIME", entity.ApprovedDate));
            //myParams.Add(new Params("@uDeclinedDate", "DATETIME", entity.DeclinedDate));
            myParams.Add(new Params("@uUpdatedDate", "DATETIME", entity.UpdatedDate));
            //myParams.Add(new Params("@uCreatedBy", "GUID", entity.CreatedBy));
            //myParams.Add(new Params("@uApprovedBy", "GUID", entity.ApprovedBy));
            //myParams.Add(new Params("@uDeclinedBy", "GUID", entity.DeclinedBy));
            myParams.Add(new Params("@uUpdatedBy", "GUID", entity.UpdatedBy));
            ret = SqlDataControl.Input(sql.ToString(), myParams);
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


    #region Roles

    public override bool isRoleExist(string roleName)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT ur.rID, ur.rDesc, ur.rStatus, ");
            sql.AppendLine("case ur.rStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end status, ");
            sql.AppendLine("ur.rCreatedDate, ur.rApprovedDate, ur.rDeclinedDate, ur.rUpdatedDate, ");
            sql.AppendLine("ur.rCreatedBy, ur.rApprovedBy, ur.rDeclinedBy, ur.rUpdatedBy ");
            sql.AppendLine("from User_Roles ur ");
            //sql.AppendLine("join User_Permission up on (ur.rID = up.rID) ");
            //sql.AppendLine("join Menu m on (up.mID = m.mID) ");
            sql.AppendLine("where ur.rDesc = @rDesc ");
            sql.AppendLine("order by ur.rCreatedDate desc ");
            MyParams.Add(new Params("@rDesc", "NVARCHAR", roleName));
            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);
            if (dt.Rows.Count > 0)
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
        }
        return ret;
    }

    public override DataTable getMenu()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT mID, mType, mDesc, mPath, mGroup, mSeq ");
            sql.AppendLine("from Menu ");
            sql.AppendLine("where mGroup <> 9000 ");
            sql.AppendLine("order by mGroup asc, mType asc, mSeq asc ");
            ret = SqlDataControl.GetResult(sql.ToString(), MyParams);
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

    public override DataTable getRoles()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT ur.rID, ur.rDesc, ");
            sql.AppendLine("ur.rCreatedDate, ur.rApprovedDate, ur.rDeclinedDate, ur.rUpdatedDate, ");
            sql.AppendLine("ur.rCreatedBy, ur.rApprovedBy, ur.rDeclinedBy, ur.rUpdatedBy ");
            sql.AppendLine(", case (select top 1 tblstatus from Data_Trail ut where ut.tblid = ur.rid order by ut.editeddate DESC) ");
            sql.AppendLine("when 3 then 3 else rstatus end rStatus, ");
            sql.AppendLine("case ");
            sql.AppendLine("(case (select top 1 tblstatus from Data_Trail ut where ut.tblid = ur.rid order by ut.editeddate DESC) when 3 then 3 else rstatus end) ");
            sql.AppendLine("when 0 then 'Suspended' when 1 then 'Active' when 2 then 'Pending' when 3 then 'Edited' end Status, ");
            sql.AppendLine("ul.uname CreatedBy ");
            sql.AppendLine("from User_Roles ur ");
            sql.AppendLine("join User_Login ul on (ur.rCreatedby = ul.aID) ");

            //sql.AppendLine("join User_Permission up on (ur.rID = up.rID) ");
            //sql.AppendLine("join Menu m on (up.mID = m.mID) ");
            //sql.AppendLine("where ur.rStatus = 1 ");
            sql.AppendLine("order by ur.rCreatedDate desc ");
            ret = SqlDataControl.GetResult(sql.ToString(), MyParams);
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

    public override DataTable getActiveRoles()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT ur.rID, ur.rDesc, ur.rStatus, ");
            sql.AppendLine("case ur.rStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end status, ");
            sql.AppendLine("ur.rCreatedDate, ur.rApprovedDate, ur.rDeclinedDate, ur.rUpdatedDate, ");
            sql.AppendLine("ur.rCreatedBy, ur.rApprovedBy, ur.rDeclinedBy, ur.rUpdatedBy ");
            sql.AppendLine("from User_Roles ur ");
            //sql.AppendLine("join User_Permission up on (ur.rID = up.rID) ");
            //sql.AppendLine("join Menu m on (up.mID = m.mID) ");
            sql.AppendLine("where ur.rStatus = 1 ");
            sql.AppendLine("order by ur.rCreatedDate desc ");
            ret = SqlDataControl.GetResult(sql.ToString(), MyParams);
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

    public override UIRoleEn getRolesWithPermission(Guid roleID)
    {
        UIRoleEn ret = new UIRoleEn();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT ur.rID, ur.rDesc, ur.rStatus, ");
            sql.AppendLine("up.rID, up.mID, up.mChecker, up.mMaker, up.mView, m.mDesc ");
            sql.AppendLine("from User_Roles ur ");
            sql.AppendLine("join User_Permission up on (ur.rID = up.rID) ");
            sql.AppendLine("join Menu m on (up.mID = m.mID) ");
            sql.AppendLine("where");
            //sql.AppendLine("ur.rStatus = 1 and");
            sql.AppendLine(" ur.rID = @rID ");
            sql.AppendLine("order by m.mGroup asc, m.mType asc, m.mSeq asc ");
            MyParams.Add(new Params("@rID", "GUID", roleID));
            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ret.RoleID = Guid.Parse(dt.Rows[0]["rID"].ToString());
                ret.RoleDesc = dt.Rows[0]["rDesc"].ToString();
                ret.Status = Convert.ToInt32(dt.Rows[0]["rStatus"]);

                UIPermissionAccessEn pEn = new UIPermissionAccessEn();
                pEn.RoleID = Guid.Parse(dt.Rows[0]["rID"].ToString());
                pEn.MenuID = Guid.Parse(dt.Rows[i]["mID"].ToString());
                pEn.MenuDesc = dt.Rows[i]["mDesc"].ToString();
                pEn.View = Convert.ToBoolean(dt.Rows[i]["mView"]);
                pEn.Maker = Convert.ToBoolean(dt.Rows[i]["mMaker"]);
                pEn.Checker = Convert.ToBoolean(dt.Rows[i]["mChecker"]);

                ret.PermissionAccessList.Add(pEn);
            }
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

    public override bool insertRoles(UIRoleEn entity)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("insert into User_Roles ");
            sql.AppendLine("(rID, rDesc, rStatus, rCreatedDate, rCreatedBy) ");
            sql.AppendLine("VALUES ");
            sql.AppendLine("(@rID, @rDesc, @rStatus, @rCreatedDate, @rCreatedBy) ");
            myParams.Add(new Params("@rID", "GUID", entity.RoleID));
            myParams.Add(new Params("@rDesc", "NVARCHAR", entity.RoleDesc));
            myParams.Add(new Params("@rStatus", "INT", entity.Status));
            myParams.Add(new Params("@rCreatedDate", "DATETIME", entity.CreatedDate));
            myParams.Add(new Params("@rCreatedBy", "GUID", entity.CreatedBy));
            ret = SqlDataControl.Input(sql.ToString(), myParams);
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

    public override bool insertRolesPermission(UIRoleEn entity)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            foreach (UIPermissionAccessEn pEn in entity.PermissionAccessList)
            {
                sql.Clear();
                myParams.Clear();
                sql.AppendLine("insert into User_Permission ");
                sql.AppendLine("(rID, mID, mView, mMaker, mChecker) ");
                sql.AppendLine("VALUES ");
                sql.AppendLine("(@rID, @mID, @mView, @mMaker, @mChecker) ");
                myParams.Add(new Params("@rID", "GUID", pEn.RoleID));
                myParams.Add(new Params("@mID", "GUID", pEn.MenuID));
                myParams.Add(new Params("@mView", "BIT", pEn.View));
                myParams.Add(new Params("@mMaker", "BIT", pEn.Maker));
                myParams.Add(new Params("@mChecker", "BIT", pEn.Checker));
                ret = SqlDataControl.Input(sql.ToString(), myParams);

                if (!ret)
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
        return ret;

    }

    public override bool updateRoles(UIRoleEn entity)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("update User_Roles set ");
            sql.AppendLine("rDesc=@rDesc, rStatus=@rStatus, rUpdatedDate=@rUpdatedDate, rUpdatedBy=@rUpdatedBy ");
            sql.AppendLine("where rID=@rID ");
            myParams.Add(new Params("@rID", "GUID", entity.RoleID));
            myParams.Add(new Params("@rDesc", "NVARCHAR", entity.RoleDesc));
            myParams.Add(new Params("@rStatus", "INT", entity.Status));
            myParams.Add(new Params("@rUpdatedDate", "DATETIME", entity.UpdatedDate));
            myParams.Add(new Params("@rUpdatedBy", "GUID", entity.UpdatedBy));
            ret = SqlDataControl.Input(sql.ToString(), myParams);
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

    public override bool updateRolesPermission(UIRoleEn entity)
    {
        bool ret = false;
        try
        {
            if (deleteRolesPermission(entity))
            {
                ret = insertRolesPermission(entity);
            }
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

    public override bool deleteRoles(UIRoleEn entity)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("delete from User_Roles ");
            sql.AppendLine("where rID=@rID ");
            myParams.Add(new Params("@rID", "GUID", entity.RoleID));
            ret = SqlDataControl.Input(sql.ToString(), myParams);
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

    public override bool deleteRolesPermission(UIRoleEn entity)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("delete from User_Permission ");
            sql.AppendLine("where rID=@rID ");
            myParams.Add(new Params("@rID", "GUID", entity.RoleID));
            ret = SqlDataControl.Input(sql.ToString(), myParams);
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


    #region Branch

    public override bool isBranchExist(string branchName)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT ub.bID, ub.bDesc, ub.bOpenTime, ub.bCloseTime, ub.bStatus, ");
            sql.AppendLine("case ub.bStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end status, ");
            sql.AppendLine("ub.bCreatedDate, ub.bApprovedDate, ub.bDeclinedDate, ub.bUpdatedDate, ");
            sql.AppendLine("ub.bCreatedBy, ub.bApprovedBy, ub.bDeclinedBy, ub.bUpdatedBy ");
            sql.AppendLine("from User_Branch ub ");
            sql.AppendLine("where ub.bDesc = @bDesc ");
            sql.AppendLine("order by ub.bCreatedDate desc ");
            MyParams.Add(new Params("@bDesc", "NVARCHAR", branchName));

            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);
            if (dt.Rows.Count > 0)
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
        }
        return ret;
    }

    public override bool isBranchHasUserType(Guid branchID, string userType)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select aID, uName, uStatus, rID, bID, uUserType, uisLogin, uisReset, uisSuspend, uLoginAttempt, uLastLoginDate, uSessionID, uCreatedDate, uApprovedDate, uDeclinedDate, uUpdatedDate, uCreatedBy, uApprovedBy, uDeclinedBy, uUpdatedBy ");
            sql.AppendLine("from User_Login where bID=@bID and uUserType=@uUserType ");
            MyParams.Add(new Params("@bID", "GUID", branchID));
            MyParams.Add(new Params("@uUserType", "NVARCHAR", userType));

            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);
            if (dt.Rows.Count > 0)
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
        }
        return ret;
    }

    public override DataTable getBranches(List<Params> list)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            //List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT ub.bID, ub.bDesc, ub.bOpenTime, ub.bCloseTime, ");
            sql.AppendLine("ub.bMonday, ub.bTuesday, ub.bWednesday, ub.bThursday, ub.bFriday, ub.bSaturday, ub.bSunday, ");
            sql.AppendLine("ub.bCreatedDate, ub.bApprovedDate, ub.bDeclinedDate, ub.bUpdatedDate, ");
            sql.AppendLine("ub.bCreatedBy, ub.bApprovedBy, ub.bDeclinedBy, ub.bUpdatedBy, ");
            sql.AppendLine("ulc.UNAME CreatedBy, ula.UNAME ApprovedBy, uld.UNAME DeclinedBy, ulu.UNAME UpdatedBy ");
            sql.AppendLine(", bStatus, ");
            sql.AppendLine("case bstatus when 0 then 'Pending' when 1 then 'Active' else 'Inactive' end Status, ");
            sql.AppendLine("ul.uname CreatedBy ");
            sql.AppendLine("from User_Branch ub ");
            sql.AppendLine("join User_Login ul on (ub.bCreatedby = ul.aID) ");
            sql.AppendLine("left join User_Login ulc on (ub.bCreatedby = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (ub.bApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (ub.bDeclinedBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (ub.bUpdatedBy = ulu.aID) ");
            sql.AppendLine("where 1=1");

            foreach (Params subparams in list)
            {
                if (subparams.dataName.ToUpper() == "@BRANCHNAME")
                {
                    sql.AppendLine("and ub.bDesc like @branchname");
                }


            }

            sql.AppendLine("order by ub.bdesc asc ");
            ret = SqlDataControl.GetResult(sql.ToString(), list);
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

    public override DataTable getActiveBranches()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT ub.bID, ub.bDesc, ub.bOpenTime, ub.bCloseTime, ub.bStatus, ");
            sql.AppendLine("ub.bMonday, ub.bTuesday, ub.bWednesday, ub.bThursday, ub.bFriday, ub.bSaturday, ub.bSunday, ");
            //sql.AppendLine("case ub.bStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end status, ");
            sql.AppendLine("ub.bCreatedDate, ub.bApprovedDate, ub.bDeclinedDate, ub.bUpdatedDate, ");
            sql.AppendLine("ub.bCreatedBy, ub.bApprovedBy, ub.bDeclinedBy, ub.bUpdatedBy ");
            sql.AppendLine(", case (select top 1 tblstatus from Data_Trail ut where ut.tblid = ub.bid order by ut.editeddate DESC) ");
            sql.AppendLine("when 3 then 3 else ub.bstatus end bStatus, ");
            sql.AppendLine("case ");
            sql.AppendLine("(case (select top 1 tblstatus from Data_Trail ut where ut.tblid = ub.bid order by ut.editeddate DESC) when 3 then 3 else ub.bstatus end) ");
            sql.AppendLine("when 0 then 'Suspended' when 1 then 'Active' when 2 then 'Pending' when 3 then 'Edited' end Status, ");
            sql.AppendLine("ul.uname CreatedBy ");
            sql.AppendLine("from User_Branch ub ");
            sql.AppendLine("join User_Login ul on (ub.bCreatedby = ul.aID) ");
            sql.AppendLine("where ub.bStatus = 1 ");
            sql.AppendLine("order by ub.bCreatedDate desc ");
            ret = SqlDataControl.GetResult(sql.ToString(), MyParams);
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

    public override UIBranchEn getBranch(Guid branchID)
    {
        UIBranchEn ret = new UIBranchEn();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT ub.bID, ub.bDesc, ub.bOpenTime, ub.bCloseTime, ub.bStatus, ");
            sql.AppendLine("ub.bMonday, ub.bTuesday, ub.bWednesday, ub.bThursday, ub.bFriday, ub.bSaturday, ub.bSunday, ub.bRemarks, ");
            sql.AppendLine("case ub.bStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end status, ");
            sql.AppendLine("ub.bCreatedDate, ub.bApprovedDate, ub.bDeclinedDate, ub.bUpdatedDate, ");
            sql.AppendLine("ub.bCreatedBy, ub.bApprovedBy, ub.bDeclinedBy, ub.bUpdatedBy ");
            sql.AppendLine("from User_Branch ub ");
            sql.AppendLine("where ub.bID = @bID ");
            sql.AppendLine("order by ub.bCreatedDate desc ");
            MyParams.Add(new Params("@bID", "GUID", branchID));
            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);

            if (dt.Rows.Count > 0)
            {
                ret.BranchID = Guid.Parse(dt.Rows[0]["bID"].ToString());
                ret.BranchDesc = dt.Rows[0]["bDesc"].ToString();
                ret.Monday = Convert.ToBoolean(dt.Rows[0]["bMonday"]);
                ret.Tuesday = Convert.ToBoolean(dt.Rows[0]["bTuesday"]);
                ret.Wednesday = Convert.ToBoolean(dt.Rows[0]["bWednesday"]);
                ret.Thursday = Convert.ToBoolean(dt.Rows[0]["bThursday"]);
                ret.Friday = Convert.ToBoolean(dt.Rows[0]["bFriday"]);
                ret.Saturday = Convert.ToBoolean(dt.Rows[0]["bSaturday"]);
                ret.Sunday = Convert.ToBoolean(dt.Rows[0]["bSunday"]);
                ret.OpenTime = TimeSpan.Parse(dt.Rows[0]["bOpenTime"].ToString());
                ret.CloseTime = TimeSpan.Parse(dt.Rows[0]["bCloseTime"].ToString());
                ret.Status = Convert.ToInt32(dt.Rows[0]["bStatus"]);
                ret.Remarks = dt.Rows[0]["bRemarks"].ToString();

            }

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

    public override DataTable getBranchDT(Guid branchID)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT ub.bID, ub.bDesc, ub.bOpenTime, ub.bCloseTime, ub.bStatus, ");
            sql.AppendLine("ub.bMonday, ub.bTuesday, ub.bWednesday, ub.bThursday, ub.bFriday, ub.bSaturday, ub.bSunday, ");
            sql.AppendLine("case ub.bStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end status, ");
            sql.AppendLine("ub.bCreatedDate, ub.bApprovedDate, ub.bDeclinedDate, ub.bUpdatedDate, ");
            sql.AppendLine("ub.bCreatedBy, ub.bApprovedBy, ub.bDeclinedBy, ub.bUpdatedBy ");
            sql.AppendLine("from User_Branch ub ");
            sql.AppendLine("where ub.bID = @bID ");
            sql.AppendLine("order by ub.bCreatedDate desc ");
            MyParams.Add(new Params("@bID", "GUID", branchID));
            ret = SqlDataControl.GetResult(sql.ToString(), MyParams);
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

    public override DataTable getDefaultBankBranch()
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("SELECT SettingType, SettingValues");
            sql.AppendLine("FROM defaultMaintenanceLookUp");
            sql.AppendLine("WHERE SettingType = @SettingType ");
            sql.AppendLine("ORDER BY SettingType");
            MyParams.Add(new Params("@SettingType", "NVARCHAR", "BRANCHLIST"));
            DataTable dt = SqlDataControl.GetResult(sql.ToString(), MyParams);

            if (dt.Rows.Count > 0)
            {
                string jsonStr = dt.Rows[0]["SettingValues"].ToString();
                iPadSettings en = JsonConvert.DeserializeObject<iPadSettings>(jsonStr);

                ret.Columns.Add("BranchCode", typeof(String));
                ret.Columns.Add("BranchName", typeof(String));

                foreach (Items item in en.Item)
                {
                    ret.Rows.Add(item.Value, item.Desc);
                }
            }


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

    public override DataTable getBranchwithAssignRestriction(string? Params)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            /*sql.AppendLine(@"select distinct CONCAT(UB.bDesc, ' / ' ,PROF.Name) as Branch_Profile, 
                            MGB.Profile_ID, PROF.Name as [Profile_Desc], MGB.Branch_ID, UB.bDesc as [Branch_Desc]
                            from MDM_Profile_General_BranchID MGB
                            left join User_Branch UB 
                            on MGB.Branch_ID=UB.bID
                            left join MDM_Profile_General PROF
                            on PROF.Profile_ID = MGB.Profile_ID
                            where PROF.pStatus = 1
                            " + Params +
                            " Order by UB.bDesc" );*/

            sql.AppendLine(@"select distinct PROF.Name as [Profile_Desc], MGB.Branch_ID, UB.bDesc as [Branch_Desc]
                            from MDM_Profile_General_BranchID MGB
                            left join User_Branch UB 
                            on MGB.Branch_ID=UB.bID
                            left join MDM_Profile_General PROF
                            on PROF.Profile_ID = MGB.Profile_ID
                            where PROF.pStatus = 1
                            " + Params +
                            " Order by UB.bDesc");

            //note: pstatus = 1, active / approved only (not archived or pending changes)
            ret = SqlDataControl.GetResult(sql.ToString(), new List<Params>());

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

    public override DataTable getBankBranch_FilterbyMDMProfile(string branchID)
    {
        DataTable ret = new DataTable();
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();



            sql.AppendLine(@"select * from User_Branch
where bID not in
(select a.Branch_ID from MDM_Profile_General_BranchID a where a.Branch_ID is not null and a.Profile_ID = a.cProfile_ID)");
            sql.AppendLine("union");

            sql.AppendLine("select * from User_Branch where bid in(" + branchID + ")");
            sql.AppendLine("order by bdesc");



            ret = SqlDataControl.GetResult(sql.ToString(), MyParams);

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






    public override bool insertBranch(UIBranchEn entity)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("insert into User_Branch ");
            sql.AppendLine("(bID, bDesc, bMonday, bTuesday, bWednesday, bThursday, bFriday, bSaturday, bSunday, bOpenTime, bCloseTime, bStatus, bCreatedDate, bCreatedBy, bRemarks) ");
            sql.AppendLine("VALUES ");
            sql.AppendLine("(@bID, @bDesc, @bMonday, @bTuesday, @bWednesday, @bThursday, @bFriday, @bSaturday, @bSunday, @bOpenTime, @bCloseTime, @bStatus, @bCreatedDate, @bCreatedBy, @bRemarks) ");
            myParams.Add(new Params("@bID", "GUID", entity.BranchID));
            myParams.Add(new Params("@bDesc", "NVARCHAR", entity.BranchDesc));
            myParams.Add(new Params("@bMonday", "BIT", entity.Monday));
            myParams.Add(new Params("@bTuesday", "BIT", entity.Tuesday));
            myParams.Add(new Params("@bWednesday", "BIT", entity.Wednesday));
            myParams.Add(new Params("@bThursday", "BIT", entity.Thursday));
            myParams.Add(new Params("@bFriday", "BIT", entity.Friday));
            myParams.Add(new Params("@bSaturday", "BIT", entity.Saturday));
            myParams.Add(new Params("@bSunday", "BIT", entity.Sunday));
            myParams.Add(new Params("@bOpenTime", "TIME", entity.OpenTime));
            myParams.Add(new Params("@bCloseTime", "TIME", entity.CloseTime));
            myParams.Add(new Params("@bStatus", "INT", entity.Status));
            myParams.Add(new Params("@bCreatedDate", "DATETIME", entity.CreatedDate));
            myParams.Add(new Params("@bCreatedBy", "GUID", entity.CreatedBy));
            myParams.Add(new Params("@bRemarks", "NVARCHAR", entity.Remarks));

            ret = SqlDataControl.Input(sql.ToString(), myParams);
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

    public override bool updateBranch(UIBranchEn entity)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("update User_Branch set ");
            sql.AppendLine("bDesc=@bDesc, bMonday=@bMonday, bTuesday=@bTuesday, bWednesday=@bWednesday, bThursday=@bThursday, bFriday=@bFriday, bSaturday=@bSaturday, bSunday=@bSunday, bOpenTime=@bOpenTime, bCloseTime=@bCloseTime, bStatus=@bStatus, bUpdatedDate=@bUpdatedDate, bUpdatedBy=@bUpdatedBy, bRemarks=@bRemarks ");
            sql.AppendLine("where bID=@bID ");
            myParams.Add(new Params("@bID", "GUID", entity.BranchID));
            myParams.Add(new Params("@bDesc", "NVARCHAR", entity.BranchDesc));
            myParams.Add(new Params("@bMonday", "BIT", entity.Monday));
            myParams.Add(new Params("@bTuesday", "BIT", entity.Tuesday));
            myParams.Add(new Params("@bWednesday", "BIT", entity.Wednesday));
            myParams.Add(new Params("@bThursday", "BIT", entity.Thursday));
            myParams.Add(new Params("@bFriday", "BIT", entity.Friday));
            myParams.Add(new Params("@bSaturday", "BIT", entity.Saturday));
            myParams.Add(new Params("@bSunday", "BIT", entity.Sunday));
            myParams.Add(new Params("@bOpenTime", "TIME", entity.OpenTime));
            myParams.Add(new Params("@bCloseTime", "TIME", entity.CloseTime));
            myParams.Add(new Params("@bStatus", "INT", entity.Status));
            myParams.Add(new Params("@bUpdatedDate", "DATETIME", entity.UpdatedDate));
            myParams.Add(new Params("@bUpdatedBy", "GUID", entity.UpdatedBy));
            myParams.Add(new Params("@bRemarks", "NVARCHAR", entity.Remarks));

            ret = SqlDataControl.Input(sql.ToString(), myParams);
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

    public override bool deleteBranch(UIBranchEn entity)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            sql.AppendLine("delete from User_Branch ");
            sql.AppendLine("where bID=@bID ");
            myParams.Add(new Params("@bID", "GUID", entity.BranchID));
            ret = SqlDataControl.Input(sql.ToString(), myParams);
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

}
