using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Common.Data.Component;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_XML
{

    /// <summary>
    /// The helper function to generate the VPN section XML to be included in the mobileconfig file. 
    /// </summary>
    public static class VPN_XML
    {
        public static string VPN_XMLGenerator(List<MDM_Profile_VPN> my_list_VPN)
        {
            string ret = string.Empty;
            try
            {
                foreach (MDM_Profile_VPN my_VPN in my_list_VPN)
                {


                    switch (my_VPN.ConnectionType.ToUpper())
                    {
                        case "IPSEC":
                            {
                                ret += IPSEC(my_VPN);
                                break;
                            }
                        case "L2TP":
                            {
                                ret += L2TP(my_VPN);
                                break;
                            }
                        case "IKEV2":
                            {
                                ret += IKEV2(my_VPN);
                                break;
                            }
                    }

                }

            }
            catch (Exception ex)
            {
                ret = @"ERROR";
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ex);
            }


            return ret;
        }



        public static string IPSEC(MDM_Profile_VPN my_VPN)
        {
            string ret = string.Empty;
            try
            {
                ret += @"  <dict><key>IPSec</key>       
                                <dict>
				                <key>AuthenticationMethod</key>
				                <string>" + (my_VPN.IPSEC_MachineAuthentication == "Shared Secret / Group Name" ? my_VPN.IPSEC_MachineAuthentication = "SharedSecret" : my_VPN.IPSEC_MachineAuthentication) + @"</string>";

                if (my_VPN.IPSEC_UseHybridAuthentication != null)
                {
                    if (my_VPN.IPSEC_UseHybridAuthentication.ToLower() == "true")
                    {
                        ret += @" <key>LocalIdentifier</key>
				                               <string>" + my_VPN.IPSEC_GroupName + @"[hybrid]</string>";
                    }
                    else
                    {
                        ret += @"<key>LocalIdentifier</key>
				                             <string>" + my_VPN.IPSEC_GroupName + @"</string>";
                    }
                }


                ret += @"<key>LocalIdentifierType</key>
				                <string>KeyID</string>
                                <key>RemoteAddress</key>
				                <string>" + my_VPN.IPSEC_Server + @"</string>";

                if (my_VPN.IPSEC_SharedSecret != null)
                {
                    ret += @"<key>SharedSecret</key>
				                <data>
				                " + Des_Base64.ConvertByteToBase64String(my_VPN.IPSEC_SharedSecret).ToString() + @"
				                </data>";
                }

                ret += @"<key>XAuthEnabled</key>
				                <integer>1</integer>
				                <key>XAuthName</key>
				                <string>" + my_VPN.IPSEC_Account + @"</string>
				                <key>XAuthPassword</key>
				                <string>" + my_VPN.IPSEC_Account_Password + @"</string>";

                if (my_VPN.IPSEC_PromptForPassword != null && my_VPN.IPSEC_PromptForPassword.ToLower() == "true")
                {
                    ret += @"<key>XAuthPasswordEncryption</key>
				                <string>Prompt</string>";
                }
                ret += @"</dict>
                            <key>IPv4</key>
                            <dict>
                                <key>OverridePrimary</key>
                               <integer>1</integer>
                            </dict>
                            <key>PayloadDescription</key>
                            <string>Configures VPN settings</string>
                            <key>PayloadDisplayName</key>
                            <string>VPN</string>
                            <key>PayloadIdentifier</key>
                            <string>com.apple.vpn.managed." + Guid.NewGuid() + @"</string>
                            <key>PayloadType</key>
                            <string>com.apple.vpn.managed</string>
                            <key>PayloadUUID</key>
                            <string>" + Guid.NewGuid() + @"</string>
                            <key>PayloadVersion</key>
                            <integer>1</integer>
                            <key>Proxies</key>
                            <dict>";

                //<integer>" + (my_VPN.IPSEC_PromptForPassword != null && my_VPN.IPSEC_PromptForPassword != "true" ? 1 : 0) + @"</integer>

                if (my_VPN.IPSEC_ProxySetup.ToUpper() == "AUTOMATIC")
                {
                    ret += @" <key>HTTPEnable</key>
				                        <integer>0</integer>
				                        <key>HTTPSEnable</key>
				                        <integer>0</integer>
				                        <key>ProxyAutoConfigEnable</key>
				                        <true/>
				                        <key>ProxyAutoConfigURLString</key>
				                        <string>" + my_VPN.IPSEC_ProxyServerURL + "</string>";
                }
                else if (my_VPN.IPSEC_ProxySetup.ToUpper() == "MANUAL")
                {
                    ret += @" <key>HTTPEnable</key>
                                <true/>
                                <key>HTTPProxy</key>
                                <string>" + my_VPN.IPSEC_Proxy_Server + @"</string>
                                <key>HTTPProxyPassword</key>
                                <string>" + my_VPN.IPSEC_Password + @"</string>
                                <key>HTTPProxyUsername</key>
                                <string>" + my_VPN.IPSEC_Authentication + @"</string>
                                <key>HTTPSEnable</key>
                                <true/>
                                <key>HTTPSProxy</key>
                                <string>" + my_VPN.IPSEC_Proxy_Server + @"</string>";
                }
                else
                {
                    ret += @"<key>HTTPEnable</key>
				                    <integer>0</integer>
				                    <key>HTTPSEnable</key>
				                    <integer>0</integer>";
                }


                ret += @"</dict>
                            <key>UserDefinedName</key>
                            <string>" + my_VPN.ConnectionName + @"</string>
                            <key>VPNType</key>
                            <string>IPSec</string>
                            </dict>";


            }
            catch (Exception ex)
            {
                ret = @"ERROR";
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ex);
            }

            return ret;
        }

        public static string IKEV2(MDM_Profile_VPN my_VPN)
        {
            string ret = string.Empty;
            try
            {



                if (my_VPN.IKEV2_AlwaysOnVPN.ToLower() != "true")
                {
                    ret += @"<dict>
                             <key>DisconnectOnIdle</key>
			                <integer>" + my_VPN.IKEV2_DisconnectOnIdle + @"</integer>
			                <key>DisconnectOnIdleTimer</key>
			                <integer>3615</integer>
			                <key>IKEv2</key>";
                }
                else
                {
                    ret += @"<dict>
			                <key>AlwaysOn</key>
			                <dict>
                                <key>AllowCaptiveWebSheet</key>
				                <" + my_VPN.IKEV2_AllowTrafficFromCaptiveWebSheetOutsideTheVPNTunnel.ToLower() + @"
                                 />
				                <key>AllowAllCaptiveNetworkPlugins</key>
				                <" + (my_VPN.IKEV2_AllowTrafficFromAllCaptiveNetworkingAppsOustsideVPNTunnel != null ? my_VPN.IKEV2_AllowTrafficFromAllCaptiveNetworkingAppsOustsideVPNTunnel.ToLower() : ((my_VPN.IKEV2_AllowTrafficFromAllCaptiveNetworkingAppsOustsideVPNTunnel.ToLower() == "false") ? "false" : "true")) + @"/>";

                    if (my_VPN.IKEV2_CaptiveNetworkingAppBundleIdentifiers != null)
                    {
                        string[] App_Bundle = my_VPN.IKEV2_CaptiveNetworkingAppBundleIdentifiers.Split(',');


                        if (App_Bundle[0] != "")
                        {
                            ret += @"<key>AllowedCaptiveNetworkPlugins</key>
				                                          <array>";
                            foreach (string app_bun in App_Bundle)
                            {
                                ret += @"<dict>
						                                            <key>BundleIdentifier</key>
						                                            <string>" + app_bun + @"</string>
					                                            </dict>";
                            }
                        }

                    }

                    ret += @"<key>ServiceExceptions</key>
				                <array/>
				                <key>TunnelConfigurations</key>
				                <array>";
                }


                ret += @" <key>IKEv2</key>       
                                 <dict>
						                <key>AuthenticationMethod</key>
						                <string>SharedSecret</string>
						                <key>ChildSecurityAssociationParameters</key>
						                <dict>
							                <key>DiffieHellmanGroup</key>
							                <integer>" + (my_VPN.IKEV2_Child_DiffleHellmanGroup != null ? my_VPN.IKEV2_Child_DiffleHellmanGroup : my_VPN.IKEV2_IKESA_DiffieHellmanGroup) + @"</integer>
							                <key>EncryptionAlgorithm</key>
							                <string>" + (my_VPN.IKEV2_Child_EncryptionAlgorithm != null ? my_VPN.IKEV2_Child_EncryptionAlgorithm : my_VPN.IKEV2_IKESA_EncryptionAlgorithm) + @"</string>
							                <key>IntegrityAlgorithm</key>
							                <string>" + (my_VPN.IKEV2_Child_IntegrityAlgorithm != null ? my_VPN.IKEV2_Child_IntegrityAlgorithm : my_VPN.IKEV2_IKESA_IntegrityAlgorithm) + @"</string>
							                <key>LifeTimeInMinutes</key>
							                <integer>" + (my_VPN.IKEV2_Child_LifetimeInMinutes != null ? my_VPN.IKEV2_Child_LifetimeInMinutes : my_VPN.IKEV2_IKESA_LifeTimeInMinutes) + @"</integer>
						                </dict>
						                <key>DeadPeerDetectionRate</key>
						                <string>Medium</string>";

                if (my_VPN.IKEV2_UserSameTunnelConfigurationForCellularAndWifi != null)
                {
                    if (my_VPN.IKEV2_UserSameTunnelConfigurationForCellularAndWifi.ToLower() == "true")
                    {
                        ret += @"     <key>DisableMOBIKE</key>
						                        <" + (my_VPN.IKEV2_DisableMobilityAndMultihoming != null ? my_VPN.IKEV2_DisableMobilityAndMultihoming.ToLower() : ((my_VPN.IKEV2_DisableMobilityAndMultihoming.ToLower() == "false") ? "false" : "true")) + @"/>
						                        <key>DisableRedirect</key>
						                        <" + (my_VPN.IKEV2_DisableRedirects != null ? my_VPN.IKEV2_DisableRedirects.ToLower() : ((my_VPN.IKEV2_DisableRedirects.ToLower() == "false") ? "false" : "true")) + @"/>
						                        <key>EnableCertificateRevocationCheck</key>
						                        <" + (my_VPN.IKEV2_EnableCertificateRevocationCheck != null ? my_VPN.IKEV2_EnableCertificateRevocationCheck.ToLower() : ((my_VPN.IKEV2_EnableCertificateRevocationCheck.ToLower() == "false") ? "false" : "true")) + @"/>
						                        <key>EnablePFS</key>
						                        <" + (my_VPN.IKEV2_EnablePerfectDForwordSecrecy != null ? my_VPN.IKEV2_EnablePerfectDForwordSecrecy.ToLower() : ((my_VPN.IKEV2_EnablePerfectDForwordSecrecy.ToLower() == "false") ? "false" : "true")) + @"/>
						                        <key>ExtendedAuthEnabled</key>
						                        <" + (my_VPN.IKEV2_EnableEAP != null ? my_VPN.IKEV2_EnableEAP.ToLower() : ((my_VPN.IKEV2_EnableEAP.ToLower() == "false") ? "false" : "true")) + @"/>
						                        <key>IKESecurityAssociationParameters</key>
						                        <dict>
							                        <key>DiffieHellmanGroup</key>
							                        <integer>" + my_VPN.IKEV2_IKESA_DiffieHellmanGroup + @"</integer>
							                        <key>EncryptionAlgorithm</key>
							                        <string>" + my_VPN.IKEV2_IKESA_EncryptionAlgorithm + @"</string>
							                        <key>IntegrityAlgorithm</key>
							                        <string>" + my_VPN.IKEV2_IKESA_IntegrityAlgorithm + @"</string>
							                        <key>LifeTimeInMinutes</key>
							                        <integer>" + my_VPN.IKEV2_IKESA_LifeTimeInMinutes + @"</integer>
						                        </dict>
						                        <key>Interfaces</key>
						                        <array>
							                        <string>Cellular</string>
						                        </array>
						                        <key>LocalIdentifier</key>
						                        <string>" + my_VPN.IKEV2_LocalIdentifier + @"</string>
						                        <key>NATKeepAliveInterval</key>
						                        <integer>" + my_VPN.IKEV2_NATKeepAliveInternal + @"</integer>
						                        <key>NATKeepAliveOffloadEnable</key>
						                        <" + (my_VPN.IKEV2_EnableNATKeepaliveWhileTheDeviceIsAsleep != null ? my_VPN.IKEV2_EnableNATKeepaliveWhileTheDeviceIsAsleep.ToLower() : ((my_VPN.IKEV2_EnableNATKeepaliveWhileTheDeviceIsAsleep.ToLower() == "false") ? "false" : "true")) + @"/>
						                        <key>ProtocolType</key>
						                        <string>IKEv2</string>
						                        <key>RemoteAddress</key>
						                        <string>" + my_VPN.IPSEC_Server + @"</string>
						                        <key>RemoteIdentifier</key>
						                        <string>" + my_VPN.IKEV2_RemoteIdentifier + @"</string>
						                        <key>SharedSecret</key>
						                        <string>" + my_VPN.IKEV2_SharedSecret + @"</string>
						                        <key>UseConfigurationAttributeInternalIPSubnet</key>
						                        <false/>
					                        </dict>
					                        <dict>
						                        <key>AuthenticationMethod</key>
						                        <string>Certificate</string>";
                        if (my_VPN.IKEV2_DisableMobilityAndMultihoming != null)
                        {
                            ret += @"<key>DisableMOBIKE</key>
                           <" + my_VPN.IKEV2_DisableMobilityAndMultihoming.ToLower() == "true" ? "true" : "false" + @"/>";
                        }

                        if (my_VPN.IKEV2_DisableRedirects != null)
                        {
                            ret += @"<key>DisableRedirect</key>
                                        <" + my_VPN.IKEV2_DisableRedirects.ToLower() == "true" ? "true" : "false" + @"/>";

                        }
                        if (my_VPN.IKEV2_EnableCertificateRevocationCheck != null)
                        {
                            ret += @"<key>EnableCertificateRevocationCheck</key>
                                        <" + my_VPN.IKEV2_EnableCertificateRevocationCheck.ToLower() == "true" ? "true" : "false" + @"/>";

                        }
                        if (my_VPN.IKEV2_EnablePerfectDForwordSecrecy != null)
                        {
                            ret += @"<key>EnablePFS</key>
                                        <" + my_VPN.IKEV2_EnablePerfectDForwordSecrecy.ToLower() == "true" ? "true" : "false" + @"/>";
                        }
                        ret += @"<key>ExtendedAuthEnabled</key>
						                        <false/>
						                        <key>Interfaces</key>
						                        <array>
							                        <string>WiFi</string>
						                        </array>
                                            ";
                    }
                    else
                    {
                        if (my_VPN.IKEV2_DisableMobilityAndMultihoming != null)
                        {

                            ret += @"<key>DisableMOBIKE</key>
                           <" + my_VPN.IKEV2_DisableMobilityAndMultihoming.ToLower() == "true" ? "true" : "false" + @"/>";
                        }

                        if (my_VPN.IKEV2_DisableRedirects != null)
                        {
                            ret += @"<key>DisableRedirect</key>
                                        <" + my_VPN.IKEV2_DisableRedirects.ToLower() == "true" ? "true" : "false" + @"/>";

                        }
                        if (my_VPN.IKEV2_EnableCertificateRevocationCheck != null)
                        {
                            ret += @"<key>EnableCertificateRevocationCheck</key>
                                        <" + my_VPN.IKEV2_EnableCertificateRevocationCheck.ToLower() == "true" ? "true" : "false" + @"/>";

                        }
                        if (my_VPN.IKEV2_EnablePerfectDForwordSecrecy != null)
                        {
                            ret += @"<key>EnablePFS</key>
                                        <" + my_VPN.IKEV2_EnablePerfectDForwordSecrecy.ToLower() == "true" ? "true" : "false" + @"/>";
                        }

                        ret += @"<key>ExtendedAuthEnabled</key>
                                        <false/>
                                        <key>IKESecurityAssociationParameters</key>
                                        <dict>
                                            <key>DiffieHellmanGroup</key>
                                            <integer>" + my_VPN.IKEV2_IKESA_DiffieHellmanGroup + @"</integer>
                                            <key>EncryptionAlgorithm</key>
                                            <string>" + my_VPN.IKEV2_IKESA_EncryptionAlgorithm + @"</string>
                                            <key>IntegrityAlgorithm</key>
                                            <string>" + my_VPN.IKEV2_IKESA_IntegrityAlgorithm + @"</string>
                                            <key>LifeTimeInMinutes</key>
                                            <integer>" + my_VPN.IKEV2_IKESA_LifeTimeInMinutes + @"</integer>
                                        </dict>
                                        <key>Interfaces</key>
                                        <array>
                                            <string>Cellular</string>
                                            <string>WiFi</string>
                                        </array>";
                    }

                }

                ret += @"<key>LocalIdentifier</key>
						                <string>" + my_VPN.IKEV2_LocalIdentifier + @"</string>
						                <key>NATKeepAliveInterval</key>
						                <integer>" + my_VPN.IKEV2_NATKeepAliveInternal + @"</integer>
						                <key>NATKeepAliveOffloadEnable</key>
						                <" + (my_VPN.IKEV2_EnableNATKeepaliveWhileTheDeviceIsAsleep == null ? "false" : "true") + @"/>
						                <key>ProtocolType</key>
						                <string>IKEv2</string>
						                <key>RemoteAddress</key>
						                <string>" + my_VPN.IKEV2_Server + @"</string>
						                <key>RemoteIdentifier</key>
						                <string>" + my_VPN.IKEV2_RemoteIdentifier + @"</string>
						                <key>SharedSecret</key>
						                <string>" + my_VPN.IKEV2_SharedSecret + @"</string>
						                <key>UseConfigurationAttributeInternalIPSubnet</key>
						                <true/>
					                </dict>
				                </array>
				                <key>UIToggleEnabled</key>
				                <true/>
			                </dict>
			                <key>IPv4</key>
			                <dict>
				                <key>OverridePrimary</key>
				                <integer>1</integer>
			                </dict>
			                <key>PayloadDescription</key>
			                <string>Configures VPN settings</string>
			                <key>PayloadDisplayName</key>
			                <string>VPN</string>
			                <key>PayloadIdentifier</key>
			                <string>com.apple.vpn.managed." + Guid.NewGuid() + @"</string>
			                <key>PayloadType</key>
			                <string>com.apple.vpn.managed</string>
			                <key>PayloadUUID</key>
			                <string>" + Guid.NewGuid() + @"</string>
			                <key>PayloadVersion</key>
			                <integer>1</integer>
			                <key>Proxies</key>
			                <dict>
				                <key>HTTPEnable</key>
				                <integer>0</integer>
				                <key>HTTPSEnable</key>
				                <integer>0</integer>
			                </dict>
			                <key>UserDefinedName</key>
			                <string>" + my_VPN.ConnectionName + @"</string>
			                <key>VPNType</key>
			                <string>" + (my_VPN.IKEV2_AlwaysOnVPN.ToLower() == "true" ? "AlwaysOn" : "IKEv2") + @"AlwaysOn</string>
		                </dict>";


            }
            catch (Exception ex)
            {
                ret = @"ERROR";
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ex);
            }

            return ret;
        }

        public static string L2TP(MDM_Profile_VPN my_VPN)
        {
            string ret = string.Empty;
            try
            {
                ret += @"<dict>
			                <key>IPSec</key>
			                <dict>
				                <key>AuthenticationMethod</key>
				                <string>SharedSecret</string>
				                <key>LocalIdentifierType</key>
				                <string>KeyID</string>
                            </dict>";
                if (my_VPN.L2TP_SharedSecret != null)
                {
                    if (my_VPN.L2TP_SharedSecret.ToLower() == "true")
                    {
                        ret += @"<key>SharedSecret</key>
				                <data>
				                " + Des_Base64.ConvertByteToBase64String(my_VPN.L2TP_SharedSecret).ToString() + @"
				                </data>
			            </dict>";
                    }
                }
                ret += @"<key>IPv4</key>
			                <dict>
				                <key>OverridePrimary</key>
                                <integer>0</integer>
                             </dict>
			                <key>PPP</key>";

                //				                <integer>" + (my_VPN.L2TP_PromptForPassword != null && my_VPN.L2TP_PromptForPassword == "true" ? 1 : 0) + @"</integer>


                if (my_VPN.L2TP_UserAuthentication_RSASecurID != null)
                {
                    if (my_VPN.L2TP_UserAuthentication_RSASecurID.ToLower() == "true")
                    {
                        ret += @"<dict><key>AuthEAPPlugins</key>
				                                    <array>
					                                    <string>EAP-RSA</string>
				                                    </array>
				                                    <key>AuthName</key>
				                                    <string>" + my_VPN.L2TP_Account + @"</string>
				                                    <key>AuthProtocol</key>
				                                    <array>
					                                    <string>EAP</string>
				                                    </array>
				                                    <key>CommRemoteAddress</key>
				                                    <string>" + my_VPN.L2TP_Server + @"</string></dict>";
                    }
                    else
                    {
                        ret += @"<dict>";

                        if (my_VPN.L2TP_Account != null)
                        {
                            ret += @"<key>AuthName</key>
				                        <string>" + my_VPN.L2TP_Account + @"</string>";
                        }

                        if (my_VPN.L2TP_UserAuthentication_Password != null)
                        {
                            ret += @" <key>AuthPassword</key>
				                        <string>" + my_VPN.L2TP_UserAuthentication_Password + @"</string>";

                        }
                        ret += @" <key>CommRemoteAddress</key>
				                        <string>" + my_VPN.L2TP_Server + @"</string>
			                            </dict>";
                    }
                }



                ret += @"<key>PayloadDescription</key>
			                <string>Configures VPN settings</string>
			                <key>PayloadDisplayName</key>
			                <string>VPN</string>
			                <key>PayloadIdentifier</key>
			                <string>com.apple.vpn.managed." + Guid.NewGuid() + @"</string>
			                <key>PayloadType</key>
			                <string>com.apple.vpn.managed</string>
			                <key>PayloadUUID</key>
			                <string>" + Guid.NewGuid() + @"</string>
			                <key>PayloadVersion</key>
			                <integer>1</integer>
			                <key>Proxies</key>
			                <dict>";


                if (my_VPN.L2TP_ProxySetup.ToUpper() == "AUTOMATIC")
                {
                    ret += @" <key>HTTPEnable</key>
				                                <integer>0</integer>
				                                <key>HTTPSEnable</key>
				                                <integer>0</integer>
				                                <key>ProxyAutoConfigEnable</key>
				                                <true/>
				                                <key>ProxyAutoConfigURLString</key>
				                                <string>" + my_VPN.L2TP_ProxySetupURL + "</string>";
                }
                else if (my_VPN.L2TP_ProxySetup.ToUpper() == "MANUAL")
                {
                    ret += @" <key>HTTPEnable</key>
                                        <true/>
                                        <key>HTTPProxy</key>
                                        <string>" + my_VPN.L2TP_ProxySetup + @"</string>
                                        <key>HTTPProxyPassword</key>
                                        <string>" + my_VPN.L2TP_ProxySetup_Password + @"</string>
                                        <key>HTTPProxyUsername</key>
                                        <string>" + my_VPN.L2TP_ProxySetup_Authentication + @"</string>
                                        <key>HTTPSEnable</key>
                                        <true/>
                                        <key>HTTPSProxy</key>
                                        <string>" + my_VPN.L2TP_ProxySetup_Server + @"</string>";
                }
                else
                {
                    ret += @"<key>HTTPEnable</key>
				                            <integer>0</integer>
				                            <key>HTTPSEnable</key>
				                            <integer>0</integer>";
                }

                ret += @"</dict>
			                <key>UserDefinedName</key>
			                <string>" + my_VPN.ConnectionName + @"</string>
			                <key>VPNType</key>
			                <string>L2TP</string>
		                </dict>";

            }
            catch (Exception ex)
            {
                ret = @"ERROR";
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                                 ex);
            }

            return ret;
        }
    }

}
