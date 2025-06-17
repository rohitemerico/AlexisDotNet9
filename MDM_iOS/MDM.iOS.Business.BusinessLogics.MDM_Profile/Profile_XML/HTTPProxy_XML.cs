using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_XML
{
    public class HTTPProxy_XML
    {

        /// <summary>
        /// The helper function to generate the HTTP Proxy XML to be included in the mobileconfig file. 
        /// </summary>
        /// <param name="my_HttpProxy"></param>
        /// <returns></returns>

        public static string HTTP_Proxy_XML(MDM_HttpProxy my_HttpProxy)// ERROR
        {
            string ret = string.Empty;

            try
            {
                ret += @"<dict>
                   <key>PayloadDescription</key>
                   <string>Global HTTP Proxy</string>
                   <key>PayloadDisplayName</key>
                   <string>Global HTTP Proxy</string>
                   <key>PayloadIdentifier</key>
                   <string>com.apple.proxy.http.global.A2F8183A-B6FC-4DAC-8405-377351D5E589</string>
                   <key>PayloadType</key>
                   <string>com.apple.proxy.http.global</string>
                   <key>PayloadUUID</key>
                   <string>A2F8183A-B6FC-4DAC-8405-377351D5E589</string>
                   <key>PayloadVersion</key>
                   <integer>1</integer>
                   <key>ProxyCaptiveLoginAllowed</key>
                   <" + my_HttpProxy.AllowByPassingProxy.ToString() + @"/>
                   <key>ProxyPassword</key>
                   <string>" + my_HttpProxy.Password + @"</string>
                   <key>ProxyServer</key>
                   <string>" + my_HttpProxy.ProxyServer + @"</string>
                   <key>ProxyServerPort</key>
                   <integer>" + my_HttpProxy.Port + @"</integer>
                   <key>ProxyType</key>
                   <string>" + my_HttpProxy.ProxyType + @"</string>
                   <key>ProxyUsername</key>
                   <string>" + my_HttpProxy.Username + @"</string>
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
