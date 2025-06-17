using System.Configuration;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_XML
{
    public class General_XML
    {
        /// <summary>
        /// The helper function to generate the general section XML to be included in the mobileconfig file. 
        /// </summary>
        /// <param name="my_General"></param>
        /// <returns></returns>
        public static string General_XMLGenerator(MDM_Profile_General my_General)// ERROR
        {
            string ret = string.Empty;

            try
            {
                //header
                ret += @"<?xml version='1.0' encoding='UTF-8'?>
                        <!DOCTYPE plist PUBLIC '-//Apple//DTD PLIST 1.0//EN' 'http://www.apple.com/DTDs/PropertyList-1.0.dtd'>
                        <plist version='1.0'>
                        <dict>";

                //Consent Text
                ret += @"<key>ConsentText</key>
                         <dict>
                          <key>default</key>
                          <string>" + (my_General.ConsentMessage == null ? "" : my_General.ConsentMessage) + @"</string>
                         </dict>";


                //Automatically remove profile with intervalAd
                /*if (my_General.AutomaticallyRemoveProfile.ToUpper() == "AFTER INTERVAL" && (my_General.AutomaticallyRemoveProfile_Days != null || my_General.AutomaticallyRemoveProfile_Hours != null))
                {
                    int sec = 0;

                    if (my_General.AutomaticallyRemoveProfile_Days != string.Empty)
                    {

                        sec += Int32.Parse(my_General.AutomaticallyRemoveProfile_Days) * 60 * 60 * 24; // 60 sec for 1 min, 60 min for 1 hours,24 hours for one day
                    }

                    if (my_General.AutomaticallyRemoveProfile_Hours != string.Empty)
                    {
                        sec += Int32.Parse(my_General.AutomaticallyRemoveProfile_Hours) * 60 * 60; // 60 sec for 1 min, 60 min for 1 hours
                    }

                    ret += @"<key>DurationUntilRemoval</key>
                            <integer>" + sec.ToString() + @"</integer>";

                }*/

                //Automatically remove profile with 



                // Paylload Content
                ret += @"<key>PayloadContent</key>
                         <array>";


                ret += "@@@@@@@@";  // 8 * @ for replace other payload
                ret += "</array>";


                ret += @" <key>PayloadDescription</key>
                                         <string>" + (my_General.Description == null ? "" : my_General.Description) + @"</string>
                                         <key>PayloadDisplayName</key>
                                         <string>" + ConfigurationManager.AppSettings["PayloadDisplayName"] + @"</string>
                                         <key>PayloadIdentifier</key>
                                         <string>" + ConfigurationManager.AppSettings["APNS_PayLoadIdentifier"] + @"</string>
                                         <key>PayloadOrganization</key>
                                         <string>" + ConfigurationManager.AppSettings["Organization_Name"] + @"</string>";

                // Remarks it first, further study on WithAuthorization and Never if needed
                //if (my_General.Security.ToUpper() == "WITH AUTHORIZATION" || my_General.Security.ToUpper() == "ALWAYS")
                //{
                ret += @"<key>PayloadRemovalDisallowed</key>
<false/>";
                //                }
                //                else if (my_General.Security.ToUpper() == "NEVER")
                //                {
                //                    ret += @"<key>PayloadRemovalDisallowed</key>
                //<true/>";
                //                }
                //                else
                //                {
                //                    ret = @"ERROR,Security Type is null";
                //                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                //                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                //                     System.Reflection.MethodBase.GetCurrentMethod().Name,
                //                     ',', "Error of Security Type", "Security Type is null", "Security Type =" + my_General.Security + "");

                //                    return ret;
                //                }


                ret += @"<key>PayloadType</key>
                                         <string>Configuration</string>
                                         <key>PayloadUUID</key>
                                         <string>" + Guid.NewGuid() + @"</string>
                                         <key>PayloadVersion</key>
                                         <integer>1</integer>";


                //if (my_General.AutomaticallyRemoveProfile.ToUpper() == "ON DATE" && my_General.AutomaticallyRemoveProfile_Date != null)
                //{

                //    string time_UTC = DateTime.UtcNow.ToLongTimeString();
                //    string date_picker = Convert.ToDateTime(my_General.AutomaticallyRemoveProfile_Date).ToLongDateString();
                //    string dt_UTCformat = DateTime.SpecifyKind(Convert.ToDateTime(date_picker + " " + time_UTC), DateTimeKind.Utc).ToString("yyyy-MM-ddTHH:mm:ssZ");

                //    ret += @"<key>RemovalDate</key>
                //                     <date>" + dt_UTCformat + @"</date>";
                //}

                ret += @"</dict>
                                </plist>";

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

        public static string GeneralRemoveProfile_XMLGenerator(MDM_Profile_General my_General)
        {
            string ret = string.Empty;
            try
            {


                if (my_General.Security.ToUpper() != null)
                {
                    if (my_General.Security.ToUpper() == "WITH AUTHORIZATION" && my_General.Security != null)
                    {

                        if (my_General.AuthorizationPassword != null)
                        {
                            Guid key = Guid.NewGuid();
                            ret += @"<dict>
                                   <key>PayloadDescription</key>
                                   <string>Configures a password for profile removal</string>
                                   <key>PayloadDisplayName</key>
                                   <string>Profile Removal</string>
                                   <key>PayloadIdentifier</key>
                                   <string>com.apple.profileRemovalPassword." + key + @"</string>
                                   <key>PayloadType</key>
                                   <string>com.apple.profileRemovalPassword</string>
                                   <key>PayloadUUID</key>
                                   <string>" + key + @"</string>
                                   <key>PayloadVersion</key>
                                   <integer>1</integer>
                                   <key>RemovalPassword</key>
                                   <string>" + my_General.AuthorizationPassword + @"</string>
                                  </dict>";
                        }
                        else
                        {
                            ret = @"ERROR, Invalid with Authorization Password";
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().Name,
                                         ',', "Error of GeneralRemoveProfile_XMLGenerator", "GeneralRemoveProfile_XMLGenerator", "");

                            return ret;
                        }

                        //}
                        //else
                        //{
                        //    ret = @"ERROR,Security Type is null";
                        //    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        //                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        //                 System.Reflection.MethodBase.GetCurrentMethod().Name,
                        //                 ',', "Error of Security Type", "Security Type is null", "Security Type =" + my_General.Security + "");

                        //    return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().Name,
                                         ',', "Error of Security Type", "Security Type is null", "Security Type =" + my_General.Security + "");
            }
            return ret;
        }

    }
}


