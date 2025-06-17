using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Common.Data.Component;
using MDM.iOS.Entities.MDM;
using Newtonsoft.Json;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Enroll
{
    /// <summary>
    /// Actual implementation of retrieving or updating database by implementing the MDM_EnrollmentBase abstract class.
    /// </summary>
    public class MDM_EnrollmentDefault : MDM_EnrollmentBase
    {
        /// <summary>
        /// Get enrollment data specified by the profile ID. 
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        public override DataTable GetData_EnrollmentDataByProfileID(Guid PID)
        {
            DataTable ret = new DataTable();

            StringBuilder sql = new StringBuilder();

            List<Params> myParams = new List<Params>();

            try
            {
                sql.Clear();
                sql.AppendLine(@"SELECT * FROM MDM_PROFILE_GENERAL where Profile_ID = @PROFILEID");

                myParams.Add(new Params("@PROFILEID", "GUID", PID));
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
        /// Retrieve all the enrollment records except those that are checked into the MDM server and completed the enrollment. 
        /// </summary>
        /// <returns></returns>
        public override DataTable GetData_Enrollment()
        {
            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();


            try
            {
                sql.Clear();

                sql.AppendLine("Select *,");
                sql.AppendLine("(Case when E.MDMStatus=1 then 'ACCEPTED' when E.MDMStatus=2 then 'PENDING' when E.MDMStatus=3 then 'PROFILE MISSING' else 'REJECTED' End) as StrStatus ,  m.MachineName ");
                sql.AppendLine("from MDM_Enrollment E");
                sql.AppendLine("left join tblMachine M");
                sql.AppendLine("on E.IMEI=M.MachineIMEI");
                sql.AppendLine("where MDMStatus not in (4)");
                sql.AppendLine("order by createdDateTime desc");

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="En_Enroll"></param>
        /// <returns></returns>
        public override DataTable GetData_Enrollment(En_Enrollment En_Enroll)
        {

            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            try
            {
                sql.Clear();

                sql.AppendLine("Select *,");
                sql.AppendLine("(Case when E.MDMStatus=1 then 'ACCEPTED' when E.MDMStatus=2 then 'RESET CERTIFICATE' else 'REJECTED' End) as StrStatus ");
                sql.AppendLine("from MDM_Enrollment E");
                sql.AppendLine("left join tblMachine M");
                sql.AppendLine("on E.IMEI=M.MachineIMEI");
                sql.AppendLine("where mdmid=@mdmid");

                myParams.Add(new Params("@mdmid", "GUID", En_Enroll.MDMID));
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
        /// Get the enrollment data using MDM ID as the parameter. 
        /// </summary>
        /// <param name="En_Enroll"></param>
        /// <returns></returns>
        public override DataTable GetData_Enrollment_byMdmID(En_Enrollment En_Enroll)
        {
            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                sql.AppendLine("SELECT *,ISNULL(MdmAllowEnrollStatus,'-1') as strMdmAllowEnrollStatus");
                sql.AppendLine("FROM MDM_Enrollment");
                sql.AppendLine("WHERE MdmID=@MdmID");

                myParams.Add(new Params("@MdmID", "GUID", En_Enroll.MDMID));
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
        /// Get the enrollment data using IMEI as the parameter. 
        /// </summary>
        /// <param name="En_Enroll"></param>
        /// <returns></returns>
        public override DataTable GetData_Enrollment_byImei(En_Enrollment En_Enroll)
        {
            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                sql.AppendLine("SELECT *,ISNULL(MdmAllowEnrollStatus,'-1') as strMdmAllowEnrollStatus");
                sql.AppendLine("FROM MDM_Enrollment");
                sql.AppendLine("WHERE IMEi=@IMEI");

                myParams.Add(new Params("@IMEI", "NVARCHAR", En_Enroll.IMEI));
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
        /// Get the MDM_ID of the machine using UDID as the parameter. 
        /// </summary>
        /// <param name="En_Enroll"></param>
        /// <returns></returns>
        public override Guid GetData_Enrollment_byUDID_getMDMID(En_Enrollment En_Enroll)
        {
            Guid ret = Guid.Empty;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            DataTable dt = new DataTable();
            try
            {
                sql.AppendLine("select MDMID from MDM_Enrollment ME");
                sql.AppendLine("WHERE ME.UDID=@UDID");

                myParams.Add(new Params("@UDID", "NVARCHAR", En_Enroll.UDID));
                dt = SqlDataControl.GetResult(sql.ToString(), myParams);

                if (dt.Rows.Count > 0)
                {
                    ret = Guid.Parse(dt.Rows[0]["MDMID"].ToString());
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

        /// <summary>
        /// Get the enrollment data by UDID. 
        /// </summary>
        /// <param name="En_Enroll"></param>
        /// <returns></returns>
        public override DataTable GetData_Enrollment_byUDID(En_Enrollment En_Enroll)
        {
            DataTable ret = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                sql.AppendLine("SELECT *,ISNULL(MdmAllowEnrollStatus,'-1') as strMdmAllowEnrollStatus");
                sql.AppendLine("FROM MDM_Enrollment");
                sql.AppendLine("WHERE UDID=@UDID");

                // no results are actually obtained, why? Nothing is being retrieved. 
                myParams.Add(new Params("@UDID", "NVARCHAR", En_Enroll.UDID));
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
        /// Retrieve profile ID of a specific machine from the enrollment table. 
        /// </summary>
        /// <param name="En_Enroll"></param>
        /// <returns></returns>
        public override Guid GetData_Enrollment_Machine_ProfileID(En_Enrollment En_Enroll)
        {
            Guid ret = Guid.Empty;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            DataTable dt = new DataTable();
            try
            {
                sql.AppendLine("select MGB.Profile_ID from MDM_Enrollment ME");
                sql.AppendLine("left join MDM_Profile_General_BranchID MGB");
                sql.AppendLine("on ME.BranchID=MGB.Branch_ID");
                sql.AppendLine(" and MGB.cProfile_ID = MGB.Profile_ID");
                sql.AppendLine("WHERE ME.UDID=@UDID");

                myParams.Add(new Params("@UDID", "NVARCHAR", En_Enroll.UDID));
                dt = SqlDataControl.GetResult(sql.ToString(), myParams);

                if (dt.Rows.Count > 0)
                {
                    ret = Guid.Parse(dt.Rows[0]["Profile_ID"].ToString());
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

        /// <summary>
        /// Inserts a new enrollment entry based on the given Mdm ID primary key. 
        /// </summary>
        /// <param name="En_Enroll"></param>
        /// <returns></returns>
        public override bool Enrollment_Insert(En_Enrollment En_Enroll)
        {

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name, ',', JsonConvert.SerializeObject(En_Enroll));


            bool ret = false;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                sql.AppendLine("Insert into MDM_Enrollment");
                sql.AppendLine(@"([MdmID]
                                  ,[UDID]
                                  ,[IMEI]
                                  ,[SerialNo]
                                  ,[PushMagic]
                                  ,[Topic]
                                  ,[Token]
                                  ,[UnlockToken]
                                  ,[MdmStatus]
                                  ,[MdmPath]
                                  ,[MdmAllowEnrollStatus]
                                  ,[TempGroupID]
                                  ,[CreatedDateTime]
                                  ,[LastMdmCheckInDateTime]
                                  ,[LastApprovedDateTime]
                                  ,[LastModifiedDateTime]
                                  ,[LastRejectDateTime]
                                  ,[LastApprovedUser]
                                  ,[LastRejectedUser])");
                sql.AppendLine("values");
                sql.AppendLine(@"(     @MdmID
                                      ,@UDID
                                      ,@IMEI
                                      ,@SerialNo
                                      ,@PushMagic
                                      ,@Topic
                                      ,@Token
                                      ,@UnlockToken
                                      ,@MdmStatus
                                      ,@MdmPath
                                      ,@MdmAllowEnrollStatus
                                      ,@TempGroupID
                                      ,@CreatedDateTime
                                      ,@LastMdmCheckInDateTime
                                      ,@LastApprovedDateTime
                                      ,@LastModifiedDateTime
                                      ,@LastRejectDateTime
                                      ,@LastApprovedUser
                                      ,@LastRejectedUser)");

                myParams.Add(new Params("@MdmID", "GUID", Guid.NewGuid()));
                myParams.Add(new Params("@UDID", "NVARCHAR", En_Enroll.UDID));
                myParams.Add(new Params("@IMEI", "NVARCHAR", En_Enroll.IMEI));
                myParams.Add(new Params("@SerialNo", "NVARCHAR", En_Enroll.SerialNo));
                myParams.Add(new Params("@PushMagic", "NVARCHAR", (En_Enroll.PushMagic)));
                myParams.Add(new Params("@Topic", "NVARCHAR", En_Enroll.Topic));
                myParams.Add(new Params("@Token", "NVARCHAR", En_Enroll.Token));
                myParams.Add(new Params("@UnlockToken", "NVARCHAR", En_Enroll.UnlockToken));
                myParams.Add(new Params("@MdmPath", "NVARCHAR", En_Enroll.MdmPath));
                myParams.Add(new Params("@MdmStatus", "INT", En_Enroll.MdmStatus));
                myParams.Add(new Params("@MdmAllowEnrollStatus", "INT", En_Enroll.MdmAllowEnrollStatus));
                myParams.Add(new Params("@TempGroupID", "NVARCHAR", En_Enroll.TempGroupID));
                myParams.Add(new Params("@CreatedDateTime", "DATETIME", En_Enroll.CreatedDateTime));
                myParams.Add(new Params("@LastMdmCheckInDateTime", "DATETIME", En_Enroll.LastMdmCheckInDateTime));
                myParams.Add(new Params("@LastApprovedDateTime", "DATETIME", En_Enroll.LastApprovedDateTime));
                myParams.Add(new Params("@LastModifiedDateTime", "DATETIME", En_Enroll.LastModifiedDateTime));
                myParams.Add(new Params("@LastRejectDateTime", "DATETIME", En_Enroll.LastRejectDateTime));
                myParams.Add(new Params("@LastApprovedUser", "GUID", En_Enroll.LastApprovedUser));
                myParams.Add(new Params("@LastRejectedUser", "GUID", En_Enroll.LastRejectedUser));
                ret = SqlDataControl.Input(sql.ToString(), myParams);


            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name, ',',
                                ex.ToString(), JsonConvert.SerializeObject(En_Enroll));
            }

            return ret;
        }

        /// <summary>
        /// Update a particular enrollment entry based on the given Mdm ID primary key. 
        /// </summary>
        /// <param name="En_Enroll"></param>
        /// <returns></returns>
        public override bool Enrollment_Update(En_Enrollment En_Enroll)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {

                En_Enrollment my_Enroll = new En_Enrollment();
                my_Enroll.MDMID = En_Enroll.MDMID;
                DataTable dt = GetData_Enrollment_byMdmID(my_Enroll);

                List<En_Enrollment> my_Enroll_List = dt.AsEnumerable().Select(row => new En_Enrollment
                {

                    MDMID = (En_Enroll.MDMID != null ? En_Enroll.MDMID : row.Field<Guid>("MDMID"))
                            ,
                    BranchID = (En_Enroll.BranchID != null ? En_Enroll.BranchID : row.Field<Guid?>("BranchID"))
                            ,
                    UDID = (En_Enroll.UDID != null ? En_Enroll.UDID : row.Field<string>("UDID"))
                            ,
                    IMEI = (En_Enroll.IMEI != null ? En_Enroll.IMEI : row.Field<string>("IMEI"))
                            ,
                    PushMagic = (En_Enroll.PushMagic != null ? En_Enroll.PushMagic : row.Field<string>("PushMagic"))
                            ,
                    Topic = (En_Enroll.Topic != null ? En_Enroll.Topic : row.Field<string>("Topic"))
                            ,
                    Token = (En_Enroll.Token != null ? En_Enroll.Token : row.Field<string>("Token"))
                            ,
                    UnlockToken = (En_Enroll.UnlockToken != null ? En_Enroll.UnlockToken : row.Field<string>("UnlockToken"))
                            ,
                    MdmStatus = (En_Enroll.MdmStatus != null ? En_Enroll.MdmStatus : row.Field<int>("MdmStatus"))
                            ,
                    MdmPath = (En_Enroll.MdmPath != null ? En_Enroll.MdmPath : row.Field<string>("MdmPath"))
                            ,
                    MdmAllowEnrollStatus = (En_Enroll.MdmAllowEnrollStatus != null ? En_Enroll.MdmAllowEnrollStatus : row.Field<int>("MdmAllowEnrollStatus"))
                            ,
                    TempGroupID = (En_Enroll.TempGroupID != null ? En_Enroll.TempGroupID : row.Field<string>("TempGroupID"))
                            ,
                    CreatedDateTime = (En_Enroll.CreatedDateTime != null ? En_Enroll.CreatedDateTime : row.Field<DateTime?>("CreatedDateTime"))
                            ,
                    LastMdmCheckInDateTime = (En_Enroll.LastMdmCheckInDateTime != null ? En_Enroll.LastMdmCheckInDateTime : row.Field<DateTime?>("LastMdmCheckInDateTime"))
                            ,
                    LastApprovedDateTime = (En_Enroll.LastApprovedDateTime != null ? En_Enroll.LastApprovedDateTime : row.Field<DateTime?>("LastApprovedDateTime"))
                            ,
                    LastModifiedDateTime = (En_Enroll.LastModifiedDateTime != null ? En_Enroll.LastModifiedDateTime : row.Field<DateTime?>("LastModifiedDateTime"))
                            ,
                    LastRejectDateTime = (En_Enroll.LastRejectDateTime != null ? En_Enroll.LastRejectDateTime : row.Field<DateTime?>("LastRejectDateTime"))
                            ,
                    LastApprovedUser = (En_Enroll.LastApprovedUser != null ? En_Enroll.LastApprovedUser : row.Field<Guid?>("LastApprovedUser"))
                            ,
                    LastRejectedUser = (En_Enroll.LastRejectedUser != null ? En_Enroll.LastRejectedUser : row.Field<Guid?>("LastRejectedUser"))

                }).ToList();



                //            List<t> target = dt.AsEnumerable()
                //.Select(row => new T
                //{
                //     // assuming column 0's type is Nullable<long>
                //     ID = row.Field<long?>(0).GetValueOrDefault()
                //     Name = String.IsNullOrEmpty(row.Field<string>(1))
                //     ? "not found"
                //     : row.Field<string>(1)
                //})
                //.ToList();


                sql.AppendLine("Update MDM_Enrollment");
                sql.AppendLine(@"set [MdmID]=@MdmID,
                            [BranchID]=@BranchId
                                  ,[UDID]=@UDID
                                  ,[IMEI]=@IMEI
                                  ,[PushMagic]=@PushMagic
                                  ,[Topic]=@Topic
                                  ,[Token]=@Token
                                  ,[UnlockToken]=@UnlockToken
                                  ,[MdmStatus]=@MdmStatus
                                  ,[MdmPath]=@MdmPath
                                  ,[MdmAllowEnrollStatus]=@MdmAllowEnrollStatus
                                  ,[TempGroupID]=@TempGroupID
                                  ,[CreatedDateTime]=@CreatedDateTime
                                  ,[LastMdmCheckInDateTime]=@LastMdmCheckInDateTime
                                  ,[LastApprovedDateTime]=@LastApprovedDateTime
                                  ,[LastModifiedDateTime]=@LastModifiedDateTime
                                  ,[LastRejectDateTime]=@LastRejectDateTime
                                  ,[LastApprovedUser]=@LastApprovedUser
                                  ,[LastRejectedUser]=@LastRejectedUser");
                sql.AppendLine("WHERE [MdmID]=@MdmID");


                myParams.Add(new Params("@MdmID", "GUID", my_Enroll_List.First().MDMID));
                myParams.Add(new Params("@BranchId", "GUID", my_Enroll_List.First().BranchID));
                myParams.Add(new Params("@UDID", "NVARCHAR", my_Enroll_List.First().UDID));
                myParams.Add(new Params("@IMEI", "NVARCHAR", my_Enroll_List.First().IMEI));
                myParams.Add(new Params("@PushMagic", "NVARCHAR", my_Enroll_List.First().PushMagic));
                myParams.Add(new Params("@Topic", "NVARCHAR", my_Enroll_List.First().Topic));
                myParams.Add(new Params("@UnlockToken", "NVARCHAR", my_Enroll_List.First().UnlockToken));
                myParams.Add(new Params("@Token", "NVARCHAR", my_Enroll_List.First().Token));
                myParams.Add(new Params("@MdmStatus", "INT", my_Enroll_List.First().MdmStatus));
                myParams.Add(new Params("@MdmPath", "NVARCHAR", my_Enroll_List.First().MdmPath));
                myParams.Add(new Params("@MdmAllowEnrollStatus", "INT", my_Enroll_List.First().MdmAllowEnrollStatus));
                myParams.Add(new Params("@TempGroupID", "NVARCHAR", my_Enroll_List.First().TempGroupID));
                myParams.Add(new Params("@CreatedDateTime", "DATETIME", my_Enroll_List.First().CreatedDateTime));
                myParams.Add(new Params("@LastMdmCheckInDateTime", "DATETIME", my_Enroll_List.First().LastMdmCheckInDateTime));
                myParams.Add(new Params("@LastModifiedDateTime", "DATETIME", my_Enroll_List.First().LastModifiedDateTime));
                myParams.Add(new Params("@LastRejectDateTime", "DATETIME", my_Enroll_List.First().LastRejectDateTime));
                myParams.Add(new Params("@LastApprovedDateTime", "DATETIME", my_Enroll_List.First().LastApprovedDateTime));
                myParams.Add(new Params("@LastApprovedUser", "GUID", my_Enroll_List.First().LastApprovedUser));
                myParams.Add(new Params("@LastRejectedUser", "GUID", my_Enroll_List.First().LastRejectedUser));
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

        /// <summary>
        /// Delete a particular enrollment entry based on the given Mdm ID primary key. 
        /// </summary>
        /// <param name="En_Enroll"></param>
        /// <returns></returns>
        public override bool Enrollment_Delete(En_Enrollment En_Enroll)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                sql.AppendLine("Delete MDM_Enrollment");
                sql.AppendLine("WHERE [MdmID]=@MdmID");


                myParams.Add(new Params("@MdmID", "GUID", En_Enroll.MDMID));
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

        /// <summary>
        /// Used to update the enrollment status. 
        /// </summary>
        /// <param name="En_Enroll"></param>
        /// <returns></returns>
        public override bool Enrollment_UpdateStatus(En_Enrollment En_Enroll)
        {
            bool ret = false;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            try
            {
                // check if admin rejects the iPad enrollment. 
                if (En_Enroll.MdmStatus == 0)
                {
                    sql.AppendLine("Update MDM_Enrollment set mdmStatus=@mdmStatus,lastRejectDateTime=@lastRejectedDateTime,lastRejectedUser=@lastRejectedUser where mdmid=@mdmid");
                    myParams.Add(new Params("@mdmStatus", "INT", En_Enroll.MdmStatus));
                    myParams.Add(new Params("@lastRejectedDateTime", "DATETIME", En_Enroll.LastRejectDateTime));
                    myParams.Add(new Params("@lastRejectedUser", "GUID", En_Enroll.LastRejectedUser));

                }

                // check if admin approves the iPad enrollment. 
                else if (En_Enroll.MdmStatus == 1)
                {
                    sql.AppendLine("Update MDM_Enrollment set mdmStatus=@mdmStatus, BranchID=@branchid, lastApprovedDateTime=@lastApprovedDateTime,MdmAllowEnrollStatus=@MdmAllowEnrollStatus,lastApprovedUser=@lastApprovedUser where mdmid=@mdmid");
                    myParams.Add(new Params("@branchid", "GUID", En_Enroll.BranchID));
                    myParams.Add(new Params("@mdmStatus", "INT", En_Enroll.MdmStatus));
                    myParams.Add(new Params("@lastApprovedDateTime", "DATETIME", En_Enroll.LastApprovedDateTime));
                    myParams.Add(new Params("@lastApprovedUser", "GUID", En_Enroll.LastApprovedUser));
                    myParams.Add(new Params("@MdmAllowEnrollStatus", "INT", En_Enroll.MdmAllowEnrollStatus));
                }

                //Mdm Status 2 means that the MDM enrollment is pending. 
                else if (En_Enroll.MdmStatus == 2)
                {
                    sql.AppendLine("Update MDM_Enrollment set mdmStatus=@mdmStatus where mdmid=@mdmid");
                    myParams.Add(new Params("@mdmStatus", "INT", En_Enroll.MdmStatus));
                }
                else
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                ',', "En_Enroll.mdmStatus out of range", En_Enroll.MdmStatus);
                }

                myParams.Add(new Params("@mdmid", "GUID", En_Enroll.MDMID));

                // We are making changes to the database via the web. 
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
    }
}
