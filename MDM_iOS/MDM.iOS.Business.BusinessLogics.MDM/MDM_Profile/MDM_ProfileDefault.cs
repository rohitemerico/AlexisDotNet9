using System.Configuration;
using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Business.BusinessLogics.MDM.MDM_Restriction;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Common.Data.Component;
using MDM.iOS.Entities.MDM;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Profile
{
    public class MDM_ProfileDefault : MDM_ProfileBase
    {
        protected static MDM_RestrictionBase My_FRestrictionBase = MDM_RestrictionFactory.Create(ConfigurationManager.AppSettings.Get("Provider"));
        public static StringBuilder sql = new StringBuilder();
        public static List<Params> myParams = new List<Params>();
        public static int IpadProfileID = 0;

        /// <summary>
        /// Retrieve all general profile, order by created date in desc
        /// </summary>
        /// <returns></returns>
        public override DataTable GetAllProfile()
        {
            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            try
            {
                sql.AppendLine("Select p.*, ulc.uName Created_By, ulu.UNAME Updated_By ,(Case when p.pstatus=1 then 'Active' when p.pstatus=2 then 'Inactive' when p.pstatus=3 then 'Edited' when p.pstatus=0 then 'Pending' else '' End) as Status  from MDM_Profile_General p ");
                sql.AppendLine("left join User_Login ulc on (p.CreatedBy = ulc.aID) ");
                sql.AppendLine("left join User_Login ulu on (p.LastUpdateBy = ulu.aID) ");
                sql.AppendLine("Where p.pstatus <= 3 ");
                sql.AppendLine("Order by p.pStatus asc");
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

        /// <summary>
        /// Retrieve all general profile, order by created date in desc
        /// </summary>
        /// <returns></returns>
        public override DataTable GetProfiles()
        {
            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            try
            {
                sql.AppendLine("Select * from MDM_Profile_General where pstatus in (1,5)");
                sql.AppendLine("Order by pStatus asc");
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

        /// <summary>
        /// Retrieve the general profiles by profile ID. This method will be called when the console 
        /// is in the process of generating the mobileconfig files. 
        /// </summary>
        /// <param name="My_En_MDM_Profile"></param>
        /// <returns></returns>
        public override DataTable GetProfile(En_MDM_Profile My_En_MDM_Profile)
        {
            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            try
            {
                sql.AppendLine("Select * from MDM_Profile_General tip");
                sql.AppendLine("where tip.profile_ID=@profile_ID");
                myParams.Add(new Params("@profile_ID", "GUID", My_En_MDM_Profile.profile_ID));
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

        /// <summary>
        /// Retrieve the general profiles by branch ID. 
        /// </summary>
        /// <param name="BranchId"></param>
        /// <returns></returns>
        public override DataTable GetProfileByBranch(Guid BranchId)
        {
            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            try
            {
                sql.AppendLine(@"select UB.bDesc as BranchDesc,MPG.Profile_ID,MPG.Name,MPG.* 
                                from MDM_Profile_General_BranchID MPGB
                                left join MDM_Profile_General  MPG
                                on MPGB.Profile_ID=MPG.Profile_ID
                                left join User_Branch UB
                                on MPGB.Branch_ID=UB.bID");

                sql.AppendLine("where MPGB.Branch_ID = @Branch_ID and pstatus in (1,5)");
                myParams.Add(new Params("@Branch_ID", "GUID", BranchId));
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

        /// <summary>
        /// Retrieve all branches with the same profile id
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <returns></returns>
        public override DataTable GetBranchByProfileId(Guid ProfileId)
        {
            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            try
            {
                sql.AppendLine(@"select userBranch.bDesc 
                                from MDM_Profile_General_BranchID profGen
                                left join User_Branch userBranch
                                on profGen.Branch_ID = userBranch.bID");

                sql.AppendLine("where profGen.Profile_ID = @profile_ID");
                sql.AppendLine("order by bDesc asc");
                myParams.Add(new Params("@profile_ID", "GUID", ProfileId));
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

        /// <summary>
        /// Update Type = CHECKERMAKER / UPDATEOLDDATASTATUS.
        /// CHECKERMAKER - update entire entity.
        /// UPDATEOLDDATASTATUS - update profile status only
        /// </summary>
        /// <param name="MY_MDM_Profile_General"></param>
        /// <param name="UpdateType"></param>
        /// <returns></returns>
        public override bool UpdateProfileGeneralByUpdateType(MDM_Profile_General MY_MDM_Profile_General, string UpdateType)
        {
            bool ret = false;

            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                if (UpdateType.ToUpper() == "CHECKERMAKER")
                {
                    sql.AppendLine(@"update MDM_Profile_General set ");
                    if (MY_MDM_Profile_General.pStatus.HasValue)
                    {
                        sql.AppendLine("pstatus = @pstatus ,");
                        myParams.Add(new Params("@pstatus", "INT", MY_MDM_Profile_General.pStatus));
                    }
                    if (MY_MDM_Profile_General.LastUpdateDate.HasValue)
                    {
                        sql.AppendLine("LastUpdateDate = @LastUpdateDate ,");
                        myParams.Add(new Params("@LastUpdateDate", "DATETIME", MY_MDM_Profile_General.LastUpdateDate));
                    }
                    if (MY_MDM_Profile_General.LastUpdateBy.HasValue)
                    {
                        sql.AppendLine("LastUpdateBy = @LastUpdateBy ,");
                        myParams.Add(new Params("@LastUpdateBy", "GUID", MY_MDM_Profile_General.LastUpdateBy));
                    }
                    if (MY_MDM_Profile_General.CProfileId.HasValue)
                    {
                        sql.AppendLine("CProfileId = @CProfileId ,");
                        myParams.Add(new Params("@CProfileId", "GUID", MY_MDM_Profile_General.CProfileId));
                    }
                    if (MY_MDM_Profile_General.Profile_APNS_Path != null)
                    {
                        sql.AppendLine("Profile_APNS_Path = @Profile_APNS_Path ,");
                        myParams.Add(new Params("@Profile_APNS_Path", "NVARCHAR", MY_MDM_Profile_General.Profile_APNS_Path));
                    }
                    if (MY_MDM_Profile_General.Profile_Enroll_Path != null)
                    {
                        sql.AppendLine("Profile_Enroll_Path = @Profile_Enroll_Path ,");
                        myParams.Add(new Params("@Profile_Enroll_Path", "NVARCHAR", MY_MDM_Profile_General.Profile_Enroll_Path));
                    }

                    string a = sql.ToString().Trim();
                    a = a.Remove(a.Length - 1, 1);
                    sql.Clear();
                    sql.AppendLine(a + " where Profile_ID = @pid");
                    myParams.Add(new Params("@pid", "GUID", MY_MDM_Profile_General.Profile_ID));
                }

                else if (UpdateType.ToUpper() == "UPDATEOLDDATASTATUS")
                {
                    sql.AppendLine(@"update MDM_Profile_General set pstatus = @pstat where profile_Id = @pid");
                    myParams.Add(new Params("@pstat", "INT", MY_MDM_Profile_General.pStatus));
                    myParams.Add(new Params("@pid", "GUID", MY_MDM_Profile_General.Profile_ID));
                }
                ret = SqlDataControl.Input(sql.ToString(), myParams);
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ex);
            }
            finally
            {
                sql.Clear();
                myParams.Clear();
            }
            return ret;
        }

        /// <summary>
        /// For branches with the same profile (ID), update cprofile id
        /// profile id = first created, cprofile id = edited over time
        /// </summary>
        /// <param name="MY_MDM_Profile_General_BranchID"></param>
        /// <returns></returns>
        public override bool UpdateProfileGeneralBranchByID(MDM_Profile_General_BranchID MY_MDM_Profile_General_BranchID)
        {
            bool ret = false;

            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                sql.AppendLine(@"update MDM_Profile_General_BranchID set cProfile_ID = @cpid where Profile_ID = @pid");
                myParams.Add(new Params("@cpid", "GUID", MY_MDM_Profile_General_BranchID.cProfile_ID));
                myParams.Add(new Params("@pid", "GUID", MY_MDM_Profile_General_BranchID.Profile_ID));

                ret = SqlDataControl.Input(sql.ToString(), myParams);
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ex);
            }
            finally
            {
                sql.Clear();
                myParams.Clear();
            }
            return ret;
        }
    }
}
