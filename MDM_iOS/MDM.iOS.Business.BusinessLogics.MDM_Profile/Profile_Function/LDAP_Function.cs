using System.Data;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Common.Data.Component;
using Newtonsoft.Json;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function
{
    /// <summary>
    /// CRUD (Create, Read, Update and Delete) operations on the LDAP database table. 
    /// </summary>
    public class LDAP_Function
    {

        #region Main_LDAP_Function

        public static DataTable LDAP_SelectAll(MDM_Profile_LDAP my_LDAP)
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Profile_LDAP
                               where o.Profile_ID == my_LDAP.Profile_ID
                               select new
                               {
                                   o.Profile_LDAP_ID,
                                   o.Profile_ID,
                                   o.AccountDescription,
                                   o.AccountUsername,
                                   o.AccountPassword,
                                   o.AccountHostname,
                                   o.UseSSL
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

        public static DataTable LDAP_SearchSettings_SelectAll(MDM_Profile_LDAP_SearchSettings my_LDAP_SearchSettings)
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Profile_LDAP_SearchSettings
                               where o.Profile_LDAP_ID == my_LDAP_SearchSettings.Profile_LDAP_ID
                               select new
                               {
                                   o.Profile_LDAP_ID
                                   ,
                                   o.ID
                                   ,
                                   o.Scope
                                   ,
                                   o.SearchBase
                                   ,
                                   o.Description
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

        public static Boolean Main_LDAP_Insert(List<MDM_Profile_LDAP> my_LDAP_List, List<MDM_Profile_LDAP_SearchSettings> my_LDAP_SearchSettings_List)
        {
            bool ret = false;
            try
            {
                foreach (MDM_Profile_LDAP my_LDAP in my_LDAP_List)
                {
                    if (!LDAP_Insert(my_LDAP))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "LDAP_Insert", JsonConvert.SerializeObject(my_LDAP),
                                 JsonConvert.SerializeObject(my_LDAP_List), JsonConvert.SerializeObject(my_LDAP_SearchSettings_List));
                        return false;
                    }
                }

                if (my_LDAP_SearchSettings_List.First().Profile_LDAP_ID != Guid.Empty)
                {

                    foreach (MDM_Profile_LDAP_SearchSettings my_LDAP_SearchSettings in my_LDAP_SearchSettings_List)
                    {
                        if (!LDAP_SearchSettings_Insert(my_LDAP_SearchSettings))
                        {
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "LDAP_SearchSettings_Insert", JsonConvert.SerializeObject(my_LDAP_SearchSettings),
                                     JsonConvert.SerializeObject(my_LDAP_List), JsonConvert.SerializeObject(my_LDAP_SearchSettings));
                            return false;
                        }
                    }
                }
                return true;



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

        public static Boolean Main_LDAP_Update(List<MDM_Profile_LDAP> my_LDAP_List, List<MDM_Profile_LDAP_SearchSettings> my_LDAP_SearchSettings_List, Guid ProfileID)
        {
            bool ret = false;
            try
            {
                if (!LADP_Delete(my_LDAP_List.First(), ProfileID))
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "LDAP_Update#Delete_LDAP",
                                 JsonConvert.SerializeObject(my_LDAP_List), JsonConvert.SerializeObject(my_LDAP_SearchSettings_List));
                    ret = false;
                    return ret;
                }

                if (my_LDAP_SearchSettings_List.Count > 0)
                {
                    if (!LDAP_SearchSettings_Delete(my_LDAP_SearchSettings_List))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "LDAP_Update",
                                     JsonConvert.SerializeObject(my_LDAP_List), JsonConvert.SerializeObject(my_LDAP_SearchSettings_List));
                        return false;
                    }
                }



                if (my_LDAP_List.First().Profile_ID != Guid.Empty)
                {
                    foreach (MDM_Profile_LDAP my_LDAP in my_LDAP_List)
                    {
                        if (!LDAP_Insert(my_LDAP))
                        {
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "LDAP_Update#Insert_LDAP", JsonConvert.SerializeObject(my_LDAP),
                                     JsonConvert.SerializeObject(my_LDAP_List), JsonConvert.SerializeObject(my_LDAP_SearchSettings_List));
                            return false;
                        }
                    }



                    if (my_LDAP_SearchSettings_List.First().Profile_LDAP_ID != Guid.Empty)
                    {
                        foreach (MDM_Profile_LDAP_SearchSettings my_LDAP_SearchSettings in my_LDAP_SearchSettings_List)
                        {
                            if (!LDAP_SearchSettings_Insert(my_LDAP_SearchSettings))
                            {
                                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "LDAP_SearchSettings_Insert", JsonConvert.SerializeObject(my_LDAP_SearchSettings),
                                         JsonConvert.SerializeObject(my_LDAP_List), JsonConvert.SerializeObject(my_LDAP_SearchSettings));
                                return false;
                            }




                        }
                    }
                }

                ret = true;



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

        #region LDAP

        protected static Boolean LDAP_Insert(MDM_Profile_LDAP my_LDAP)
        {
            bool ret = false;

            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    entities.MDM_Profile_LDAP.Add(my_LDAP);
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

        protected static Boolean LDAP_Update(MDM_Profile_LDAP my_LDAP)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {

                    MDM_Profile_LDAP LDAP = (from c in entities.MDM_Profile_LDAP
                                             where c.Profile_ID == my_LDAP.Profile_ID
                                             select c).FirstOrDefault();

                    LDAP = my_LDAP;

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

        protected static Boolean LADP_Delete(MDM_Profile_LDAP my_LDAP, Guid ProfileID)
        {
            bool ret = false;
            try
            {



                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    List<MDM_Profile_LDAP> LDAP = (from c in entities.MDM_Profile_LDAP
                                                   where c.Profile_ID == ProfileID
                                                   select c).ToList();

                    if (LDAP.Count == 0)
                    {
                        ret = true;
                        return ret;
                    }

                    foreach (MDM_Profile_LDAP l in LDAP)
                    {
                        entities.MDM_Profile_LDAP.Remove(l);
                    }

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

        #endregion

        #region LDAP_SearchSettings

        protected static Boolean LDAP_SearchSettings_Insert(MDM_Profile_LDAP_SearchSettings my_LDAP_SearchSettings)
        {
            bool ret = false;

            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    entities.MDM_Profile_LDAP_SearchSettings.Add(my_LDAP_SearchSettings);
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

        protected static Boolean LDAP_SearchSettings_Update(MDM_Profile_LDAP_SearchSettings my_LDAP_SearchSettings)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {

                    MDM_Profile_LDAP_SearchSettings LDAP_SearchSettings = (from c in entities.MDM_Profile_LDAP_SearchSettings
                                                                           where c.Profile_LDAP_ID == my_LDAP_SearchSettings.Profile_LDAP_ID
                                                                           select c).FirstOrDefault();

                    LDAP_SearchSettings = my_LDAP_SearchSettings;

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

        protected static Boolean LDAP_SearchSettings_Delete(List<MDM_Profile_LDAP_SearchSettings> my_LDAP_SearchSettings)
        {
            bool ret = false;
            try
            {

                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    foreach (MDM_Profile_LDAP_SearchSettings ldapSS in my_LDAP_SearchSettings)
                    {
                        List<MDM_Profile_LDAP_SearchSettings> LDAP_SearchSettings = (from c in entities.MDM_Profile_LDAP_SearchSettings
                                                                                     where c.Profile_LDAP_ID == ldapSS.Profile_LDAP_ID
                                                                                     select c).ToList();

                        if (LDAP_SearchSettings.Count == 0)
                        {
                            ret = true;


                        }
                        foreach (MDM_Profile_LDAP_SearchSettings ss in LDAP_SearchSettings)
                        {
                            entities.MDM_Profile_LDAP_SearchSettings.Remove(ss);
                        }

                    }


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

        #endregion


    }
}
