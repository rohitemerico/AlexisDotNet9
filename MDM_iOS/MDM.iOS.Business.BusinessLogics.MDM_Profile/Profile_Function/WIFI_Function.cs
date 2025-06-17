using System.Data;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Common.Data.Component;
using Newtonsoft.Json;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function
{
    /// <summary>
    /// CRUD (Create, Read, Update and Delete) operations on the Wi-Fi database table. 
    /// </summary>
    public class WIFI_Function
    {

        #region Main_Wifi
        public static DataTable Wifi_SelectAll(MDM_Profile_WIFI my_Wifi)
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Profile_WIFI
                               where o.Profile_ID == my_Wifi.Profile_ID
                               select new
                               {
                                   o.Profile_ID,
                                   o.Profile_WIFI_ID,
                                   o.ServiceSetIdentifier,
                                   o.HiddenNetwork,
                                   o.AutoJoin,
                                   o.DisableCaptiveNetworkDetection,
                                   o.ProxySetup,
                                   o.ServerIPAddress,
                                   o.ServerPort,
                                   o.Username,
                                   o.Password,
                                   o.ServerProxyURL,
                                   o.SecurityType,
                                   o.SecurityTypePassword,
                                   o.NetworkType,
                                   o.FastLaneQosMarking,
                                   o.EnableQosMarking,
                                   o.WhitelistAppleAudioVideoCalling,
                                   o.App_Identifity,
                                   o.ProxyPACFallbackAllowed

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

        public static Boolean Main_Wifi_Insert(List<MDM_Profile_WIFI> my_wifi_List)
        {
            bool ret = false;

            try
            {
                foreach (MDM_Profile_WIFI my_WIFI in my_wifi_List)
                {
                    if (!Wifi_Insert(my_WIFI))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "WIFI",
                             JsonConvert.SerializeObject(my_wifi_List));
                        return false;
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

        public static Boolean Main_Wifi_Update(List<MDM_Profile_WIFI> my_wifi_List, Guid ProfileID)
        {
            bool ret = false;

            try
            {

                if (!Wifi_Delete(my_wifi_List.First(), ProfileID))
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Wifi_Delete",
                                 JsonConvert.SerializeObject(my_wifi_List));
                    return false;
                }

                if (my_wifi_List.First().Profile_ID == Guid.Empty)
                {
                    ret = true;
                    return ret;

                }

                foreach (MDM_Profile_WIFI my_wifi in my_wifi_List)
                {
                    if (!Wifi_Insert(my_wifi))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Wifi_Update",
                                 JsonConvert.SerializeObject(my_wifi_List));
                        return false;
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

        #region Wifi
        protected static Boolean Wifi_Insert(MDM_Profile_WIFI my_wifi)
        {
            bool ret = false;

            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    entities.MDM_Profile_WIFI.Add(my_wifi);
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

        protected static Boolean Wifi_Update(MDM_Profile_WIFI my_wifi)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {




                    MDM_Profile_WIFI wifi = (from c in entities.MDM_Profile_WIFI
                                             where c.Profile_WIFI_ID == my_wifi.Profile_WIFI_ID
                                             select c).FirstOrDefault();


                    wifi = my_wifi;

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

        protected static Boolean Wifi_Delete(MDM_Profile_WIFI my_wifi, Guid ProfileID)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    List<MDM_Profile_WIFI> wifi = (from c in entities.MDM_Profile_WIFI
                                                   where c.Profile_ID == ProfileID
                                                   select c).ToList();
                    if (wifi.Count == 0)
                    {
                        ret = true;
                        return ret;

                    }

                    foreach (MDM_Profile_WIFI wi in wifi)
                    {
                        entities.MDM_Profile_WIFI.Remove(wi);
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
