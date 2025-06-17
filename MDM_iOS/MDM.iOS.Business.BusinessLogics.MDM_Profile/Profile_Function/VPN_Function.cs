using System.Data;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Common.Data.Component;
using Newtonsoft.Json;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function
{
    /// <summary>
    /// CRUD (Create, Read, Update and Delete) operations on the VPN database table. 
    /// </summary>
    public class VPN_Function
    {
        #region Main_VPN

        public static DataTable VPN_SelectAll(MDM_Profile_VPN my_VPN)
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Profile_VPN
                               where o.Profile_ID == my_VPN.Profile_ID
                               select new
                               {
                                   o.Profile_VPN_ID
                                   ,
                                   o.Profile_ID
,
                                   o.ConnectionName
,
                                   o.ConnectionType
,
                                   o.IPSEC_Server
,
                                   o.IPSEC_Account
,
                                   o.IPSEC_Account_Password
,
                                   o.IPSEC_MachineAuthentication
,
                                   o.IPSEC_GroupName
,
                                   o.IPSEC_SharedSecret
,
                                   o.IPSEC_UseHybridAuthentication
,
                                   o.IPSEC_PromptForPassword
,
                                   o.IPSEC_ProxySetup
,
                                   o.IPSEC_Proxy_Server
,
                                   o.IPSEC_Proxy_Port
,
                                   o.IPSEC_Authentication
,
                                   o.IPSEC_Authentication_Password
,
                                   o.IPSEC_Password
,
                                   o.IPSEC_ProxyServerURL
,
                                   o.IKEV2_SendAllTrafficThroughVPN
,
                                   o.IKEV2_AlwaysOnVPN
,
                                   o.IKEV2_AllowUserToDisableAutomaticConnection
,
                                   o.IKEV2_UserSameTunnelConfigurationForCellularAndWifi
,
                                   o.IKEV2_Server
,
                                   o.IKEV2_RemoteIdentifier
,
                                   o.IKEV2_LocalIdentifier
,
                                   o.IKEV2_MachineAuthentication
,
                                   o.IKEV2_SharedSecret
,
                                   o.IKEV2_EnableEAP
,
                                   o.IKEV2_EnableNATKeepaliveWhileTheDeviceIsAsleep
,
                                   o.IKEV2_NATKeepAliveInternal
,
                                   o.IKEV2_DeadPeerDetectionRate
,
                                   o.IKEV2_DisableRedirects
,
                                   o.IKEV2_DisableMobilityAndMultihoming
,
                                   o.IKEV2_UseIPv4IPv6InternalSubnetAttributes
,
                                   o.IKEV2_EnablePerfectDForwordSecrecy
,
                                   o.IKEV2_EnableCertificateRevocationCheck
,
                                   o.IKEV2_IKESA_EncryptionAlgorithm
,
                                   o.IKEV2_IKESA_IntegrityAlgorithm
,
                                   o.IKEV2_IKESA_DiffieHellmanGroup
,
                                   o.IKEV2_IKESA_LifeTimeInMinutes
,
                                   o.IKEV2_WifiServer
,
                                   o.IKEV2_WifiRemoteIdentifier
,
                                   o.IKEV2_WifiLocalIdentifier
,
                                   o.IKEV2_WifiMachineAuthentication
,
                                   o.IKEV2_WifiSharedSecret
,
                                   o.IKEV2_WifiEnableEAP
,
                                   o.IKEV2_WifiEnableNATKeepaliveWhileTheDeviceIsAsleep
,
                                   o.IKEV2_WifiNATKeepAliveInternal
,
                                   o.IKEV2_WifiDeadPeerDetectionRate
,
                                   o.IKEV2_WifiDisableRedirects
,
                                   o.IKEV2_WifiDisableMobilityAndMultihoming
,
                                   o.IKEV2_WifiUseIPv4IPv6InternalSubnetAttributes
,
                                   o.IKEV2_WifiEnablePerfectDForwordSecrecy
,
                                   o.IKEV2_WifiEnableCertificateRevocationCheck
,
                                   o.IKEV2_CellularServer
,
                                   o.IKEV2_CellularRemoteIdentifier
,
                                   o.IKEV2_CellularLocalIdentifier
,
                                   o.IKEV2_CellularMachineAuthentication
,
                                   o.IKEV2_CellularSharedSecret
,
                                   o.IKEV2_CellularEnableEAP
,
                                   o.IKEV2_CellularEnableNATKeepaliveWhileTheDeviceIsAsleep
,
                                   o.IKEV2_CellularNATKeepAliveInternal
,
                                   o.IKEV2_CellularDeadPeerDetectionRate
,
                                   o.IKEV2_CellularDisableRedirects
,
                                   o.IKEV2_CellularDisableMobilityAndMultihoming
,
                                   o.IKEV2_CellularUseIPv4IPv6InternalSubnetAttributes
,
                                   o.IKEV2_CellularEnablePerfectDForwordSecrecy
,
                                   o.IKEV2_CellularEnableCertificateRevocationCheck
,
                                   o.IKEV2_Child_EncryptionAlgorithm
,
                                   o.IKEV2_Child_IntegrityAlgorithm
,
                                   o.IKEV2_Child_DiffleHellmanGroup
,
                                   o.IKEV2_Child_LifetimeInMinutes
,
                                   o.IKEV2_ServiceExceptions_VoiceMail
,
                                   o.IKEV2_ServiceExceptions_AirPrint
,
                                   o.IKEV2_AllowTrafficFromCaptiveWebSheetOutsideTheVPNTunnel
,
                                   o.IKEV2_AllowTrafficFromAllCaptiveNetworkingAppsOustsideVPNTunnel
,
                                   o.IKEV2_DisconnectOnIdle
,
                                   o.IKEV2_DisconnectOnIdle_Minutes
,
                                   o.IKEV2_DisconnectOnIdle_Seconds
,
                                   o.IKEV2_CaptiveNetworkingAppBundleIdentifiers
,
                                   o.IKEV2_Account
,
                                   o.IKEV2_Account_Password
,
                                   o.IKEV2_ProxySetup
,
                                   o.IKEV2_ProxySetup_Server
,
                                   o.IKEV2_ProxySetup_Port
,
                                   o.IKEV2_ProxySetup_Authentication
,
                                   o.IKEV2_ProxySetup_Password
,
                                   o.IKEV2_ProxySetup_ProxyServerUrl
,
                                   o.IKEV2_EAP_Authentication
,
                                   o.IKEV2_WifiEAP_Authentication
,
                                   o.IKEV2_CellularEAP_Authentication
,
                                   o.L2TP_Server
,
                                   o.L2TP_Account
,
                                   o.L2TP_UserAuthentication_RSASecurID
,
                                   o.L2TP_UserAuthentication_Password
,
                                   o.L2TP_SendAllTrafficeThroughVPN
,
                                   o.L2TP_MachineAuthentication
,
                                   o.L2TP_GroupName
,
                                   o.L2TP_SharedSecret
,
                                   o.L2TP_UseHybridAuthentication
,
                                   o.L2TP_PromptForPassword
,
                                   o.L2TP_ProxySetup
,
                                   o.L2TP_ProxySetup_Server
,
                                   o.L2TP_ProxySetup_Port
,
                                   o.L2TP_ProxySetup_Authentication
,
                                   o.L2TP_ProxySetup_Password
,
                                   o.L2TP_ProxySetupURL
,
                                   o.IKEV2_CellularAccount
,
                                   o.IKEV2_CellularAccount_Password
,
                                   o.IKEV2_WifiAccount
,
                                   o.IKEV2_WifiAccount_Password






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

        public static Boolean Main_VPN_Insert(List<MDM_Profile_VPN> my_VPN_List)
        {
            bool ret = false;

            try
            {
                foreach (MDM_Profile_VPN my_VPN in my_VPN_List)
                {
                    if (!VPN_Insert(my_VPN))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "VPN_Insert",
                                 JsonConvert.SerializeObject(my_VPN_List));
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

        public static Boolean Main_VPN_Update(List<MDM_Profile_VPN> my_VPN_List, Guid ProfileID)
        {
            bool ret = false;

            try
            {

                if (!VPN_Delete(my_VPN_List.First(), ProfileID))
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "VPN_Delete",
                                     JsonConvert.SerializeObject(my_VPN_List));
                    return false;

                }

                if (my_VPN_List.First().Profile_ID == Guid.Empty)
                {
                    ret = true;
                    return ret;
                }

                foreach (MDM_Profile_VPN my_VPN in my_VPN_List)
                {
                    if (!VPN_Insert(my_VPN))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "VPN_Update",
                                 JsonConvert.SerializeObject(my_VPN_List));
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

        #region VPN

        protected static Boolean VPN_Insert(MDM_Profile_VPN my_VPN)
        {
            bool ret = false;

            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    entities.MDM_Profile_VPN.Add(my_VPN);
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

        protected static Boolean VPN_Update(MDM_Profile_VPN my_VPN)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {

                    MDM_Profile_VPN VPN = (from c in entities.MDM_Profile_VPN
                                           where c.Profile_ID == my_VPN.Profile_ID
                                           select c).FirstOrDefault();

                    VPN = my_VPN;

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

        protected static Boolean VPN_Delete(MDM_Profile_VPN my_VPN, Guid ProfileID)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    List<MDM_Profile_VPN> VPN = (from c in entities.MDM_Profile_VPN
                                                 where c.Profile_ID == ProfileID
                                                 select c).ToList();
                    if (VPN.Count == 0)
                    {
                        ret = true;
                        return ret;
                    }

                    foreach (MDM_Profile_VPN v in VPN)
                    {

                        entities.MDM_Profile_VPN.Remove(v);

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
