using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Common.Data.Component;
using MDM.iOS.Entities.MDM;

namespace MDM.iOS.Business.BusinessLogics.MDM.MDM_Restriction
{
    public class MDM_RestrictionDefault : MDM_RestrictionBase
    {
        /// <summary>
        /// Retrieve all of the restriction menus. 
        /// </summary>
        /// <returns></returns>
        public override DataTable GetRestrictionMenu()
        {
            DataTable ret = new DataTable();
            try
            {
                StringBuilder sql = new StringBuilder();
                List<Params> MyParams = new List<Params>();
                sql.AppendLine("SELECT RId, RestrictionName, RestrictionDesc, Queue, Active, RGroup, GroupHeader ");
                sql.AppendLine("FROM tblMdm_Restriction_Menu ");
                sql.AppendLine("order by RGroup asc, GroupHeader desc, Queue asc ");

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

        /// <summary>
        /// Retrieve all the restriction menu groups from the database. 
        /// </summary>
        /// <returns></returns>
        public override DataTable GetRestrictionMenuGroup()
        {
            DataTable ret = new DataTable();

            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            try
            {

                sql.AppendLine("SELECT distinct RGroup ");
                sql.AppendLine("FROM tblMdm_Restriction_Menu ");
                sql.AppendLine("order by RGroup");

                ret = SqlDataControl.GetResult(sql.ToString(), MyParams);
            }
            catch (Exception ex)
            {

                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                           System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                           System.Reflection.MethodBase.GetCurrentMethod().Name,
                           ',', sql.ToString(), ex);
            }
            return ret;
        }

        /// <summary>
        /// Retrieve the restriction profile based on the ipad profile ID. 
        /// </summary>
        /// <param name="My_En_MDM_Profile"></param>
        /// <returns></returns>
        public override En_MDM_Profile Get_IpadProfile_Detail_DT(En_MDM_Profile My_En_MDM_Profile)
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            En_MDM_Profile ret = new En_MDM_Profile();
            try
            {
                sql.AppendLine("Select * from MDM_Profile_Restriction where IpadProfile_ID=@IpadProfile_ID");
                myParams.Add(new Params("@IpadProfile_ID", "INT", My_En_MDM_Profile.profile_ID));

                ret.profile_ID = My_En_MDM_Profile.profile_ID;
                DataTable dt = SqlDataControl.GetResult(sql.ToString(), myParams);
                foreach (DataRow dr in dt.Rows)
                {
                    RestrictionEn entity = new RestrictionEn();
                    entity.RID = Convert.ToInt32(dr["RID"].ToString());
                    entity.RestrictionName = dr["RestrictionName"].ToString();
                    entity.IsCheck = Convert.ToBoolean(dr["IsCheck"].ToString());
                    ret.RestrictionProperties.Add(entity);
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
        /// Retrieve all of the restriction options based on the profile ID. 
        /// </summary>
        /// <param name="My_En_MDM_Profile"></param>
        /// <returns></returns>
        public override En_MDM_Profile Get_IpadProfile_Detail_Restriction(En_MDM_Profile My_En_MDM_Profile)
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            En_MDM_Profile ret = new En_MDM_Profile();
            try
            {
                sql.AppendLine("Select ipd.IpadProfile_ID, ipd.RID, ipd.RestrictionName, ipd.IsCheck, ");
                sql.AppendLine("rm.GroupHeader, rm.RGroup");
                sql.AppendLine("from MDM_Profile_Restriction ipd join MDM_Profile_Restriction rm on (ipd.RID = rm.RId)");
                sql.AppendLine("where IpadProfile_ID=@IpadProfile_ID");
                sql.AppendLine("order by rm.RGroup, rm.GroupHeader desc");
                myParams.Add(new Params("@IpadProfile_ID", "INT", My_En_MDM_Profile.profile_ID));

                ret.profile_ID = My_En_MDM_Profile.profile_ID;
                DataTable dt = SqlDataControl.GetResult(sql.ToString(), myParams);
                foreach (DataRow dr in dt.Rows)
                {
                    RestrictionEn entity = new RestrictionEn();
                    entity.RID = Convert.ToInt32(dr["RID"].ToString());
                    entity.RestrictionName = dr["RestrictionName"].ToString();
                    entity.IsCheck = Convert.ToBoolean(dr["IsCheck"].ToString());
                    entity.GroupHeader = Convert.ToInt32(dr["GroupHeader"].ToString());
                    entity.RGroup = Convert.ToInt32(dr["RGroup"].ToString());

                    ret.RestrictionProperties.Add(entity);
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

        public override En_MDM_Profile Get_IpadProfile_Detail_Restriction_Group(En_MDM_Profile My_En_MDM_Profile)
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            En_MDM_Profile ret = new En_MDM_Profile();
            try
            {
                sql.AppendLine("Select ipd.IpadProfile_ID, ipd.RID, ipd.RestrictionName, ipd.IsCheck, ");
                sql.AppendLine("rm.GroupHeader, rm.RGroup");
                sql.AppendLine("from MDM_Profile_Restriction ipd join MDM_Profile_Restriction rm on (ipd.RID = rm.RId)");
                sql.AppendLine("where IpadProfile_ID=@IpadProfile_ID and rm.RGroup=@RGroup");
                sql.AppendLine("order by rm.GroupHeader desc");
                myParams.Add(new Params("@IpadProfile_ID", "INT", My_En_MDM_Profile.profile_ID));
                myParams.Add(new Params("@RGroup", "INT", My_En_MDM_Profile.profile_GroupID));

                ret.profile_ID = My_En_MDM_Profile.profile_ID;
                DataTable dt = SqlDataControl.GetResult(sql.ToString(), myParams);
                foreach (DataRow dr in dt.Rows)
                {
                    RestrictionEn entity = new RestrictionEn();
                    entity.RID = Convert.ToInt32(dr["RID"].ToString());
                    entity.RestrictionName = dr["RestrictionName"].ToString();
                    entity.IsCheck = Convert.ToBoolean(dr["IsCheck"].ToString());
                    entity.GroupHeader = Convert.ToInt32(dr["GroupHeader"].ToString());
                    entity.RGroup = Convert.ToInt32(dr["RGroup"].ToString());

                    ret.RestrictionProperties.Add(entity);
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

        public override En_MDM_Profile Get_IpadProfile_Detail_Restriction_MDM(En_MDM_Profile My_En_MDM_Profile)
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();
            En_MDM_Profile ret = new En_MDM_Profile();
            try
            {
                sql.AppendLine("Select ipd.IpadProfile_ID, ipd.RID, ipd.RestrictionName, ipd.IsCheck, ");
                sql.AppendLine("rm.GroupHeader, rm.RGroup");
                sql.AppendLine("from MDM_Profile_Restriction ipd join MDM_Profile_Restriction rm on (ipd.RID = rm.RId)");
                sql.AppendLine("where IpadProfile_ID=@IpadProfile_ID");
                sql.AppendLine("order by rm.Queue asc");
                myParams.Add(new Params("@IpadProfile_ID", "INT", My_En_MDM_Profile.profile_ID));

                ret.profile_ID = My_En_MDM_Profile.profile_ID;
                DataTable dt = SqlDataControl.GetResult(sql.ToString(), myParams);
                foreach (DataRow dr in dt.Rows)
                {
                    RestrictionEn entity = new RestrictionEn();
                    entity.RID = Convert.ToInt32(dr["RID"].ToString());
                    entity.RestrictionName = dr["RestrictionName"].ToString();
                    entity.IsCheck = Convert.ToBoolean(dr["IsCheck"].ToString());
                    entity.GroupHeader = Convert.ToInt32(dr["GroupHeader"].ToString());
                    entity.RGroup = Convert.ToInt32(dr["RGroup"].ToString());

                    ret.RestrictionProperties.Add(entity);
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
    }
}
