using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data.Component;
using Dashboard.Common.Business.Component;
using MDM.iOS.Common.Data.Component;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function
{
    public class Profile_Action_Function
    {

        public static DataTable Profile_SelectAll(MDM_Profile my_Profile)
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Profile
                               where o.Profile_ID == my_Profile.Profile_ID
                               select new
                               {
                                   o.Profile_ID
                                   ,
                                   o.Profile_Name
                                   ,
                                   o.Profile_Desc
                                   ,
                                   o.Profile_GroupID
                                   ,
                                   o.Profile_Status
                                   ,
                                   o.LastUpdateDate
                                   ,
                                   o.PlistContent
                                   ,
                                   o.Profile_APNS_Path
                                   ,
                                   o.Profile_Enroll_Path
                               };
                    ret = LINQToDataTable.LINQResultToDataTable(data);
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

        public static DataTable Profile_SelectAll()
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Profile_General

                               select new
                               {
                                   o.Profile_ID
                                   ,
                                   o.Name
                                   ,
                                   o.LastUpdateDate
                                   ,
                                   o.Profile_APNS_Path
                                   ,
                                   o.Profile_Enroll_Path
                               };
                    ret = LINQToDataTable.LINQResultToDataTable(data);
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

        public static Boolean Profile_Insert(MDM_Profile my_Profile)
        {
            bool ret = false;

            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    entities.MDM_Profile.Add(my_Profile);
                    if (entities.SaveChanges() != 0)
                    {
                        ret = true;
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

        public static Boolean Profile_Update(MDM_Profile my_Profile)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {

                    MDM_Profile Profile = (from c in entities.MDM_Profile
                                           where c.Profile_ID == my_Profile.Profile_ID
                                           select c).FirstOrDefault();

                    Profile = my_Profile;

                    if (entities.SaveChanges() != 0)
                    {
                        ret = true;
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

        public static Boolean Profile_Delete(MDM_Profile my_Profile)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    MDM_Profile Profile = (from c in entities.MDM_Profile
                                           where c.Profile_ID == my_Profile.Profile_ID
                                           select c).FirstOrDefault();
                    entities.MDM_Profile.Remove(Profile);
                    if (entities.SaveChanges() != 0)
                    {
                        ret = true;
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
    }
}
