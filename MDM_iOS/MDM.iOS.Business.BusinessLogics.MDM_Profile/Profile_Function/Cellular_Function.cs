using System.Data;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Common.Data.Component;
using Newtonsoft.Json;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function
{
    /// <summary>
    /// CRUD (Create, Read, Update and Delete) operations on the cellular database table. 
    /// </summary>
    public class Cellular_Function
    {
        public static DataTable Cellular_SelectAll(MDM_Profile_Cellular my_Cellular)
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Profile_Cellular
                               where o.Profile_ID == my_Cellular.Profile_ID
                               select new
                               {
                                   o.Profile_ID
                                   ,
                                   o.ConfiguredAPNType
                                   ,
                                   o.DefaultAPN_Name
                                   ,
                                   o.DefaultAPN_AuthenticationType
                                   ,
                                   o.DefaultAPN_UserName
                                   ,
                                   o.DefaultAPN_Password
                                   ,
                                   o.DataAPN_Name
                                   ,
                                   o.DataAPN_AuthenticationType
                                   ,
                                   o.DataAPN_UserName
                                   ,
                                   o.DataAPN_Password
                                   ,
                                   o.DataAPN_ProxyServer
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

        public static Boolean Cellular_Delete_Insert(MDM_Profile_Cellular my_Cellular, Guid ProfileID)
        {
            bool ret = false;

            try
            {

                if (Cellular_Delete(my_Cellular, ProfileID))
                {
                    if (my_Cellular.Profile_ID != Guid.Empty)
                    {
                        if (Cellular_Insert(my_Cellular))
                        {
                            ret = true;
                        }


                        else
                        {
                            ret = false;
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Insert_Cellular_Problems",
                                             JsonConvert.SerializeObject(my_Cellular));
                        }


                    }

                    else if (my_Cellular.Profile_ID == Guid.Empty)
                    {
                        ret = true;
                        return ret;
                    }
                }
                else
                {
                    ret = false;
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Delete_Cellular_Problems",
                                     JsonConvert.SerializeObject(my_Cellular));
                }


            }
            catch (Exception ex)
            {
                ret = false;
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ex);
            }


            return ret;
        }

        public static Boolean Cellular_Insert(MDM_Profile_Cellular my_cellular)
        {
            bool ret = false;

            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    entities.MDM_Profile_Cellular.Add(my_cellular);
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

        public static Boolean Cellular_Update(MDM_Profile_Cellular my_cellular)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {

                    MDM_Profile_Cellular cellular = (from c in entities.MDM_Profile_Cellular
                                                     where c.Profile_ID == my_cellular.Profile_ID
                                                     select c).FirstOrDefault();

                    cellular = my_cellular;

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

        public static Boolean Cellular_Delete(MDM_Profile_Cellular my_cellular, Guid ProfileID)
        {
            bool ret = false;
            try
            {

                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    MDM_Profile_Cellular cellular = (from c in entities.MDM_Profile_Cellular
                                                     where c.Profile_ID == ProfileID
                                                     select c).FirstOrDefault();

                    if (cellular == null)
                    {
                        ret = true;
                        return ret;

                    }

                    entities.MDM_Profile_Cellular.Remove(cellular);
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
