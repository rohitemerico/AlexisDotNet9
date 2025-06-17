using System.Configuration;
using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Common.Data.Component;
using MDM.iOS.Entities.MDM;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_App
{
    /// <summary>
    /// Actual implementation of retrieving or updating database by implementing the MDM_AppDefault abstract class.
    /// </summary>
    public class MDM_AppDefault : MDM_AppBase
    {
        public static MDM_AppBase My_FMDM_AppBase = MDM_AppFactory.Create(ConfigurationManager.AppSettings.Get("Provider"));


        /// <summary>
        /// Get the list of applications uploaded onto the server. 
        /// </summary>
        /// <param name="paramsfilter"></param>
        /// <returns></returns>
        public override DataTable GetMDM_APP(string paramsfilter)
        {
            DataTable ret = new DataTable();
            try
            {
                StringBuilder sql = new StringBuilder();
                List<Params> myParams = new List<Params>();


                sql.AppendLine(@"select ma.AppID,ma.Name,ma.[Desc],ma.Version,
                  ma.Path,ma.Identifier,ma.Icon_FilePath, ma.ExpirationDate,
                 ma.CreatedDate,ma.ApprovedDate,ma.DeclineDate,ma.UpdatedDate,
                 ma.CreatedBy,ma.ApprovedBy,ma.DeclinedBy,ma.UpdatedBy,
                  (case(case(select top 1 tblStatus from Data_Trail ut where ut.tblId=ma.AppID order by ut.editedDate desc)
                  when 3 then 3 else [Status] end) when 2 then 'Pending' when 1 then 'Active' when 0 then 'Inactive' when 3 then 'Edited' end) as [status] ,
				  case(select top 1 tblStatus from Data_Trail ut where ut.tblId=ma.AppID order by ut.editedDate desc)
                  when 3 then 3 else [Status] end as rstatus
				 from MDM_APP ma");

                sql.AppendLine(paramsfilter);
                sql.AppendLine("order by ma.CreatedDate");

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
        /// Get the list of uploaded applications and display in the MDM Checker Maker and Push App page. 
        /// </summary>
        /// <returns></returns>
        public override DataTable GetMDM_APP()
        {
            DataTable ret = new DataTable();

            try
            {
                StringBuilder sql = new StringBuilder();
                List<Params> myParams = new List<Params>();

                sql.AppendLine(@"select ma.AppID,ma.Name,ma.[Desc],ma.Version,
                  ma.Path,ma.Identifier, ma.Icon_FilePath, ma.ExpirationDate,
                 ma.CreatedDate,ma.ApprovedDate,ma.DeclineDate,ma.UpdatedDate,
                 ma.CreatedBy,ma.ApprovedBy,ma.DeclinedBy,ma.UpdatedBy,
                  (case(case(select top 1 tblStatus from Data_Trail ut where ut.tblId=ma.AppID order by ut.editedDate desc)
                  when 3 then 3 else [Status] end) when 2 then 'Pending' when 1 then 'Active' when 0 then 'Inactive' when 3 then 'Edited' end) as [status] ,
				  case(select top 1 tblStatus from Data_Trail ut where ut.tblId=ma.AppID order by ut.editedDate desc)
                  when 3 then 3 else [Status] end as rstatus
				 from MDM_APP ma
                  order by ma.CreatedDate");


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
        /// Retrieve the application record based on the application ID. 
        /// </summary>
        /// <param name="My_AppID"></param>
        /// <returns></returns>
        public override DataTable GetMDM_APP_APPID(Guid My_AppID)
        {
            DataTable ret = new DataTable();
            try
            {
                StringBuilder sql = new StringBuilder();
                List<Params> myParams = new List<Params>();

                sql.AppendLine("SELECT top 1 * FROM [MDM_APP]");
                sql.AppendLine("WHERE AppID=@AppID");

                myParams.Add(new Params("@AppID", "GUID", My_AppID));

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
        /// Validate if the application name exists. 
        /// </summary>
        /// <param name="My_En_MDMApp"></param>
        /// <returns></returns>
        public override bool CheckNameExists(En_MDMApp My_En_MDMApp)
        {
            bool ret = false;
            try
            {
                StringBuilder sql = new StringBuilder();
                List<Params> myParams = new List<Params>();

                sql.AppendLine("SELECT 1 FROM [MDM_APP] WHERE [Name] = @Name");

                myParams.Add(new Params("@Name", "NVARCHAR", My_En_MDMApp.name));

                DataTable dt = SqlDataControl.GetResult(sql.ToString(), myParams);

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

        /// <summary>
        /// Insert an application record with details into the database. 
        /// </summary>
        /// <param name="My_En_MDMApp"></param>
        /// <returns></returns>
        public override bool AppInsertIntoDb(En_MDMApp My_En_MDMApp)
        {

            bool ret = false;
            try
            {
                StringBuilder sqlbuilder = new StringBuilder();

                sqlbuilder.AppendLine("INSERT INTO MDM_APP");
                sqlbuilder.AppendLine("(");
                sqlbuilder.AppendLine(@"[AppID]
                                          ,[Name]
                                          ,[Desc]
                                          ,[Version]
                                          ,[Status]
                                          ,[Path]
                                          ,[Identifier]
                                          ,[Icon_FilePath]
                                          ,[CreatedDate]
                                          ,[ApprovedDate]
                                          ,[DeclineDate]
                                          ,[UpdatedDate]
                                          ,[CreatedBy]
                                          ,[ApprovedBy]
                                          ,[DeclinedBy]
                                          ,[UpdatedBy]
,[ExpirationDate]");
                sqlbuilder.AppendLine(")Values(");
                sqlbuilder.AppendLine(@" @AppID
                                          ,@Name
                                          ,@Desc
                                          ,@Version
                                          ,@Status
                                          ,@Path
                                          ,@Identifier
                                          ,@Icon_FilePath
                                          ,@CreatedDate
                                          ,@ApprovedDate
                                          ,@DeclineDate
                                          ,@UpdatedDate
                                          ,@CreatedBy
                                          ,@ApprovedBy
                                          ,@DeclinedBy
                                          ,@UpdatedBy
, @eDate");
                sqlbuilder.AppendLine(")");

                List<Params> myParams = new List<Params>();
                myParams.Add(new Params("@AppID", "GUID", My_En_MDMApp.appId));
                myParams.Add(new Params("@Name", "NVARCHAR", My_En_MDMApp.name));
                myParams.Add(new Params("@Desc", "NVARCHAR", My_En_MDMApp.desc));
                myParams.Add(new Params("@Version", "NVARCHAR", My_En_MDMApp.version));
                myParams.Add(new Params("@Status", "INT", My_En_MDMApp.status));
                myParams.Add(new Params("@Path", "NVARCHAR", My_En_MDMApp.path));
                myParams.Add(new Params("@Identifier", "NVARCHAR", My_En_MDMApp.identifier));
                myParams.Add(new Params("@Icon_FilePath", "NVARCHAR", My_En_MDMApp.icon_FilePath));
                myParams.Add(new Params("@CreatedDate", "DATETIME", My_En_MDMApp.createdDate));
                myParams.Add(new Params("@ApprovedDate", "DATETIME", My_En_MDMApp.approvedDate));
                myParams.Add(new Params("@DeclineDate", "DATETIME", My_En_MDMApp.declineDate));
                myParams.Add(new Params("@UpdatedDate", "DATETIME", My_En_MDMApp.updatedDate));
                myParams.Add(new Params("@CreatedBy", "GUID", My_En_MDMApp.createdBy));
                myParams.Add(new Params("@ApprovedBy", "GUID", My_En_MDMApp.approvedBy));
                myParams.Add(new Params("@DeclinedBy", "GUID", My_En_MDMApp.declineBy));
                myParams.Add(new Params("@UpdatedBy", "GUID", My_En_MDMApp.updatedBy));
                myParams.Add(new Params("@eDate", "DATETIME", My_En_MDMApp.expDate));
                if (SqlDataControl.Input(sqlbuilder.ToString(), myParams))
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

        /// <summary>
        /// Approve the usage of an application. 
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public override bool ApproveApplication(Guid appID, string identifier)
        {
            bool ret = false;

            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            try
            {
                DataTable check = checkIdentifierNotPending(identifier);

                if (check.Rows.Count > 0)
                {
                    DisabledPreviousApplication(check);
                }

                sql.AppendLine("update tblMDM_APP set status = 1 where appid = @appid");
                myParams.Add(new Params("@appid", "GUID", appID));
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
        /// Helper function for the approve application method. 
        /// </summary>
        /// <param name="Identifier"></param>
        /// <returns></returns>
        public override DataTable checkIdentifierNotPending(string Identifier)
        {
            DataTable dt = new DataTable();
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            try
            {
                sql.AppendLine("select * from tblMDM_APP where identifier = @identifier and status <> 0");
                myParams.Add(new Params("@identifier", "NVARCHAR", Identifier));
                dt = SqlDataControl.GetResult(sql.ToString(), myParams);

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


            return dt;

        }

        /// <summary>
        /// Disables a previously uploaded application. 
        /// </summary>
        /// <param name="dt"></param>
        public override void DisabledPreviousApplication(DataTable dt)
        {
            //bool ret = false;
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sql.AppendLine("update tblMDM_APP set status = 4 where appid = @appid");
                    myParams.Add(new Params("@appid", "GUID", dt.Rows[i]["APPID"]));
                    SqlDataControl.Input(sql.ToString(), myParams);
                }

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

            //return ret;
        }

        /// <summary>
        /// Decline the request of approval of a particular application. 
        /// </summary>
        /// <param name="Appid"></param>
        /// <returns></returns>
        public override bool DeclineApplication(Guid Appid)
        {
            bool ret = false;

            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            try
            {
                sql.AppendLine("update tblMDM_APP set status = 2 where appid = @appid");
                myParams.Add(new Params("@appid", "GUID", Appid));
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

        public override bool AppInstallSummaryInsertIntoDb(En_MDMAppInstallationSummary summary)
        {

            bool ret = false;
            try
            {
                StringBuilder sqlbuilder = new StringBuilder();

                sqlbuilder.AppendLine("INSERT INTO MDM_AppInstallationSummary");
                sqlbuilder.AppendLine("(");
                sqlbuilder.AppendLine(@"[ID],[APPID],[MACHINEID],[STATUS],[INSTALLTYPE],[CREATEDDATE],[CREATEDBY]");
                sqlbuilder.AppendLine(")Values(");
                sqlbuilder.AppendLine(@" @ID,@AppID,@MachineID,@Status,@InstallType,@CreatedDate,@CreatedBy");
                sqlbuilder.AppendLine(")");

                List<Params> myParams = new List<Params>();
                myParams.Add(new Params("@ID", "GUID", summary.ID));
                myParams.Add(new Params("@AppID", "GUID", summary.AppID));
                myParams.Add(new Params("@MachineID", "NVARCHAR", summary.MachineID));
                myParams.Add(new Params("@Status", "INT", summary.Status));
                myParams.Add(new Params("@InstallType", "INT", summary.InstallType));
                myParams.Add(new Params("@CreatedDate", "DATETIME", summary.CreatedDate));
                myParams.Add(new Params("@CreatedBy", "GUID", summary.CreatedBy));
                if (SqlDataControl.Input(sqlbuilder.ToString(), myParams))
                {
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return ret;
        }

    }
}
