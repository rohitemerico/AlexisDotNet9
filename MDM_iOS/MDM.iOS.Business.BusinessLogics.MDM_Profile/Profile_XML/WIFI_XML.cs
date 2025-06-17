using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_XML
{

    public static class WIFI_XML
    {
        /// <summary>
        /// The helper function to generate the Wi-Fi section XML to be included in the mobileconfig file. 
        /// </summary>
        public static string WIFI_XMLGenerator(List<MDM_Profile_WIFI> my_Wifi_List)
        {
            string ret = string.Empty;

            try
            {
                foreach (MDM_Profile_WIFI my_Wifi in my_Wifi_List)
                {
                    ret += @"<dict>
                               <key>AutoJoin</key>
                               <" + ((my_Wifi.AutoJoin != null) ? my_Wifi.AutoJoin.ToLower() : ((my_Wifi.AutoJoin.ToLower() == "false") ? "false" : "true")) + @"/>
                               <key>CaptiveBypass</key>
                               <" + ((my_Wifi.DisableCaptiveNetworkDetection == null) ? my_Wifi.DisableCaptiveNetworkDetection.ToLower() : ((my_Wifi.DisableCaptiveNetworkDetection.ToLower() == "false") ? "false" : "true")) + @"/>
                               <key>EncryptionType</key> 
                               <string>" + ((my_Wifi.SecurityType.ToUpper() == "NONE") ? "None" : ((my_Wifi.SecurityType.ToUpper() == "WEP") ? "WEP" : (my_Wifi.SecurityType.ToUpper() == "WPA / WPA2 PERSONAL") ? "WPA" : "WPA2")) + @"</string>
                               <key>HIDDEN_NETWORK</key>
                               <" + ((my_Wifi.HiddenNetwork != null) ? my_Wifi.HiddenNetwork.ToLower() : ((my_Wifi.HiddenNetwork.ToLower() == "false") ? "false" : "true")) + @"/>
                               <key>IsHotspot</key>
                               <false/>
                               <key>Password</key>
                               <string>" + my_Wifi.SecurityTypePassword + @"</string>
                               <key>PayloadDescription</key>
                               <string>Configures Wi-Fi settings</string>
                               <key>PayloadDisplayName</key>
                               <string>Wi-Fi</string>
                               <key>PayloadIdentifier</key>
                               <string>com.apple.wifi.managed." + Guid.NewGuid() + @"</string>
                               <key>PayloadType</key>
                               <string>com.apple.wifi.managed</string>
                               <key>PayloadUUID</key>
                               <string>" + Guid.NewGuid() + @"</string>
                               <key>PayloadVersion</key>
                               <integer>1</integer>";
                    if (my_Wifi.ProxySetup.ToUpper() == "MANUAL")
                    {
                        ret += @"<key>ProxyPassword</key>
                                           <string>" + my_Wifi.Password + @"</string>
                                           <key>ProxyServer</key>
                                           <string>" + my_Wifi.ServerIPAddress + @"</string>
                                           <key>ProxyServerPort</key>
                                           <integer>" + my_Wifi.ServerPort + @"</integer>
                                              <key>ProxyType</key>
                                    <string>Manual</string>";
                    }
                    else if (my_Wifi.ProxySetup.ToUpper() == "AUTOMATIC")
                    {
                        if (my_Wifi.ServerProxyURL != null)
                        {
                            ret += @"<key>ProxyPACURL</key>
			               <string>" + my_Wifi.ServerProxyURL + @"</string>";
                        }

                        ret += @"<key>ProxyType</key>
                                    <string>Auto</string>";


                    }
                    else
                    {
                        ret += @"
                             <key>ProxyType</key>
                                    <string>None</string>";

                    }

                    if (my_Wifi.FastLaneQosMarking != null)
                    {
                        if (my_Wifi.FastLaneQosMarking.ToUpper() == "RESTRICT QOS MARKING")
                        {
                            ret += @"<key>QoSMarkingPolicy</key>
                                           <dict>
                                            <key>QoSMarkingAppleAudioVideoCalls</key>
                                            <" + ((my_Wifi.WhitelistAppleAudioVideoCalling == null) ? my_Wifi.WhitelistAppleAudioVideoCalling.ToLower() : ((my_Wifi.WhitelistAppleAudioVideoCalling.ToLower() == "false") ? "false" : "true")) + @"/>
                                            <key>QoSMarkingEnabled</key>
                                            <" + ((my_Wifi.EnableQosMarking == null) ? my_Wifi.EnableQosMarking.ToLower() : ((my_Wifi.EnableQosMarking.ToLower() == "false") ? "false" : "true")) + @"/>
                                            <key>QoSMarkingWhitelistedAppIdentifiers</key>
                                            <array>";

                            if (my_Wifi.App_Identifity != null)
                            {
                                string[] App_Identifity = my_Wifi.App_Identifity.Split(',');

                                if (App_Identifity[0] != "")
                                {
                                    foreach (string app_iden in App_Identifity)
                                    {
                                        ret += @"<string>" + app_iden.ToLower() + @"</string>";
                                    }
                                }

                            }
                            ret += @"</array>
                                     </dict>";

                        }

                    }
                    ret += @"<key>SSID_STR</key>
                                           <string>" + my_Wifi.ServiceSetIdentifier + @"</string>
                                          </dict>";
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
    }
}
