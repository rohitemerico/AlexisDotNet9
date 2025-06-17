using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_XML
{
    public class Cellular_XML
    {
        /// <summary>
        /// The helper function to generate the cellular section XML to be included in the mobileconfig file. 
        /// </summary>
        /// <param name="my_cellular"></param>
        /// <returns></returns>
        public static string Cellular_XMLGenerator(MDM_Profile_Cellular my_cellular)
        {
            string ret = string.Empty;
            try
            {

                ret = "<dict>";

                if (my_cellular.ConfiguredAPNType.ToUpper() == "DATA APN" || my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT AND DATA APNS")
                {
                    ret += @"<key>APNs</key>
                               <array>
                                <dict>";
                    if ((my_cellular.DataAPN_Name != null) && (my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT APN" || my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT AND DATA APNS"))
                    {
                        ret += @"<key>Name</key>
                                 <string>" + my_cellular.DataAPN_Name + @"</string>";
                    }

                    if ((my_cellular.DataAPN_Password != null) && (my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT APN" || my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT AND DATA APNS"))
                    {
                        ret += @"<key>Password</key>
                                 <string>" + my_cellular.DataAPN_Password + @"</string>";
                    }


                    if ((my_cellular.DataAPN_ProxyServer != null) && (my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT APN" || my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT AND DATA APNS"))
                    {
                        ret += @"<key>ProxyServer</key>
                                 <string>" + my_cellular.DataAPN_UserName + @"</string>";
                    }

                    if ((my_cellular.DataAPN_UserName != null) && (my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT APN" || my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT AND DATA APNS"))
                    {
                        ret += @"<key>Username</key>
                                 <string>" + my_cellular.DataAPN_UserName + @"</string>";
                    }

                    ret += @" </dict>
                              </array>";

                }


                if (my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT APN" || my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT AND DATA APNS")
                {
                    ret += @"<key>AttachAPN</key>
                                <dict>";

                    if ((my_cellular.DataAPN_Name != null) && (my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT APN" || my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT AND DATA APNS"))
                    {
                        ret += @"<key>Name</key>
                                 <string>" + my_cellular.DataAPN_Name + @"</string>";
                    }

                    if ((my_cellular.DataAPN_Password != null) && (my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT APN" || my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT AND DATA APNS"))
                    {
                        ret += @"<key>Password</key>
                                 <string>" + my_cellular.DataAPN_Password + @"</string>";
                    }


                    if ((my_cellular.DataAPN_ProxyServer != null) && (my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT APN" || my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT AND DATA APNS"))
                    {
                        ret += @"<key>ProxyServer</key>
                                 <string>" + my_cellular.DataAPN_UserName + @"</string>";
                    }

                    if ((my_cellular.DataAPN_UserName != null) && (my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT APN" || my_cellular.ConfiguredAPNType.ToUpper() == "DEFAULT AND DATA APNS"))
                    {
                        ret += @"<key>Username</key>
                                 <string>" + my_cellular.DataAPN_UserName + @"</string>";
                    }

                    ret += @" </dict>";
                }


                ret += @" <key>PayloadDescription</key>
                               <string>Configures cellular data settings</string>
                               <key>PayloadDisplayName</key>
                               <string>Cellular</string>
                               <key>PayloadIdentifier</key>
                               <string>com.apple.cellular." + Guid.NewGuid() + @"</string>
                               <key>PayloadType</key>
                               <string>com.apple.cellular</string>
                               <key>PayloadUUID</key>
                               <string>" + Guid.NewGuid() + @"</string>
                               <key>PayloadVersion</key>
                               <integer>1</integer>
                                </dict>";



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
