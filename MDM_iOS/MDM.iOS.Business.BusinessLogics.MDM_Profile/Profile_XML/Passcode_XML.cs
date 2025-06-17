using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_XML
{
    public static class Passcode_XML
    {

        /// <summary>
        /// The helper function to generate the passcode section XML to be included in the mobileconfig file. 
        /// </summary>
        /// <param name="my_Passcode"></param>
        /// <returns></returns>
        public static string Passcode_XMLGenerator(MDM_Profile_Passcode my_Passcode)
        {
            string ret = string.Empty;
            try
            {
                Guid key = Guid.NewGuid();
                ret += @"<dict>
                               <key>PayloadDescription</key>
                               <string>Configures passcode settings</string>
                               <key>PayloadDisplayName</key>
                               <string>Passcode</string>
                               <key>PayloadIdentifier</key>
                               <string>com.apple.mobiledevice.passwordpolicy." + key + @"</string>
                               <key>PayloadType</key>
                               <string>com.apple.mobiledevice.passwordpolicy</string>
                               <key>PayloadUUID</key>
                               <string>" + key + @"</string>
                               <key>PayloadVersion</key>
                               <integer>1</integer>
                               <key>allowSimple</key>
                               <" + (my_Passcode.AllowSimpleValue == null ? "false" : my_Passcode.AllowSimpleValue.ToLower()) + @"/>
                               <key>forcePIN</key>
                               <true/>";

                if (my_Passcode.MaximumNumberOfFailedAttempts != null)
                {
                    ret += @"<key>maxFailedAttempts</key>
                           <integer>" + my_Passcode.MaximumNumberOfFailedAttempts.ToLower() + @"</integer>";
                }

                if (my_Passcode.MaximumGracePeriod != null)
                {
                    ret += @"<key>maxGracePeriod</key>
                               <integer>" + my_Passcode.MaximumGracePeriod + @"</integer>";
                }

                if (my_Passcode.MaximumAutoLock != null)
                {
                    ret += @"<key>maxInactivity</key>
                            <integer>" + my_Passcode.MaximumAutoLock + @"</integer>";
                }

                if (my_Passcode.MaximumPasscodeAge != null)
                {
                    ret += @"<key>maxPINAgeInDays</key>
                            <integer>" + my_Passcode.MaximumPasscodeAge + @"</integer>";
                }

                if (my_Passcode.MinimumNumberOfComplexCharacters != null)
                {
                    ret += @"<key>minComplexChars</key>
                               <integer>" + my_Passcode.MinimumNumberOfComplexCharacters + @"</integer>";
                }

                if (my_Passcode.MinimumPasscodeLength != null)
                {
                    ret += @"<key>minLength</key>
                               <integer>" + my_Passcode.MinimumPasscodeLength + @"</integer>";
                }

                if (my_Passcode.PasscodeHistory != null)
                {
                    ret += @"<key>pinHistory</key>
                             <integer>" + my_Passcode.PasscodeHistory + @"</integer>";
                }

                ret += @" <key>requireAlphanumeric</key>
                               <" + (my_Passcode.Requirealphanumericvalue == null ? "false" : my_Passcode.Requirealphanumericvalue.ToLower()) + @"/>
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
