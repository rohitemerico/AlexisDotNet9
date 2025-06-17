using System.Configuration;
using System.Data;
using System.Text;
using Alexis.Common;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.Class;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_XML;
using Newtonsoft.Json;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Controller;

public class ProfileController
{
    /// <summary>
    /// Insert the profile information such as general, cellular, passcode information into the database.
    /// If successful, generate XML format for all profile information sections and store the newly created
    /// mobileconfig filepath in the database for retrieval later. 
    /// </summary>
    /// <param name="my_MDM_Full_Profile"></param>
    /// <returns></returns>
    public static Boolean Insert_All(MDM_Full_Profile my_MDM_Full_Profile)
    {
        bool ret = false;
        string Profile_APNS = string.Empty;
        string Profile_Enroll = string.Empty;
        string partProfile = string.Empty;
        string strRet = string.Empty;
        string ApnsPartProfile = string.Empty;

        try
        {
            #region General
            if (General_Function.General_Insert(my_MDM_Full_Profile.mDM_Profile_General))
            {
                if (General_Function.General_BranchID_Insert(my_MDM_Full_Profile.mDM_Profile_General_BranchID))
                {
                    strRet = General_XML.General_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_General);

                    if (strRet.ToUpper() == "ERROR")
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "General_XML",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_General));

                        return false;
                    }
                    else
                    {
                        // also add the general xml in apns profile, we remove this original implementation
                        //Profile_APNS += strRet;
                    }
                }
            }
            else
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "General_INSERT",
                                 JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_General));
                return false;
            }
            #endregion

            #region Cellular
            if (my_MDM_Full_Profile.mDM_Profile_Cellular.Profile_ID != Guid.Empty)
            {
                if (Cellular_Function.Cellular_Insert(my_MDM_Full_Profile.mDM_Profile_Cellular))
                {
                    strRet = Cellular_XML.Cellular_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_Cellular);
                    if (strRet.ToUpper() == "ERROR")
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Cellular_XML",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Cellular));
                        return false;
                    }
                    else
                    {
                        partProfile += strRet;
                        ApnsPartProfile += strRet;

                    }


                }
                else
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Cellular_INSERT",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Cellular));
                    return false;
                }
            }
            #endregion

            #region Passcode
            if (my_MDM_Full_Profile.mDM_Profile_Passcode.Profile_ID != Guid.Empty)
            {
                if (Passcode_Function.Passcode_Insert(my_MDM_Full_Profile.mDM_Profile_Passcode))
                {
                    strRet = Passcode_XML.Passcode_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_Passcode);
                    if (strRet.ToUpper() == "ERROR")
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Passcode_XML",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Passcode));
                        return false;
                    }
                    else
                    {
                        partProfile += strRet;
                        ApnsPartProfile += strRet;
                    }


                }
                else
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Passcode_INSERT",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Passcode));
                    return false;
                }
            }
            #endregion

            #region VPN
            if (my_MDM_Full_Profile.mDM_Profile_VPN_list.Count > 0)
            {
                if (my_MDM_Full_Profile.mDM_Profile_VPN_list.First().Profile_ID != Guid.Empty)
                {
                    if (VPN_Function.Main_VPN_Insert(my_MDM_Full_Profile.mDM_Profile_VPN_list))
                    {
                        strRet = VPN_XML.VPN_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_VPN_list);
                        if (strRet.ToUpper() == "ERROR")
                        {
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "VPN_XML",
                                         JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_VPN_list));
                            return false;
                        }
                        else
                        {
                            partProfile += strRet;
                            ApnsPartProfile += strRet;
                        }


                    }
                    else
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "VPN_INSERT",
                                         JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_VPN_list));
                        return false;
                    }
                }
            }
            #endregion

            #region WIFI
            if (my_MDM_Full_Profile.mDM_Profile_WIFI_list.Count > 0)
            {
                if (my_MDM_Full_Profile.mDM_Profile_WIFI_list.First().Profile_ID != Guid.Empty)
                {
                    if (WIFI_Function.Main_Wifi_Insert(my_MDM_Full_Profile.mDM_Profile_WIFI_list))
                    {
                        strRet = WIFI_XML.WIFI_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_WIFI_list);
                        if (strRet.ToUpper() == "ERROR")
                        {
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "WIFI_XML",
                                         JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_WIFI_list));
                            return false;
                        }
                        else
                        {
                            partProfile += strRet;
                            ApnsPartProfile += strRet;
                        }


                    }
                    else
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "WIFI_INSERT",
                                         JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_WIFI_list));
                        return false;
                    }
                }
            }
            #endregion


            #region Restriction
            if (my_MDM_Full_Profile.mDM_Profile_Restriction_list.Count > 0)
            {
                if (my_MDM_Full_Profile.mDM_Profile_Restriction_list.First().Profile_ID != Guid.Empty)
                {
                    // This part has a slow response. 
                    if (Restriction_Function.Main_Restriction_Insert(my_MDM_Full_Profile.mDM_Profile_Restriction_list, my_MDM_Full_Profile.mDM_Profile_Restriction_Advance))
                    {
                        strRet = Restriction_XML.Restriction_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_Restriction_list, my_MDM_Full_Profile.mDM_Profile_Restriction_Advance);
                        if (strRet.ToUpper() == "ERROR")
                        {
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Restriction_XML",
                                         JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Restriction_list), JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Restriction_Advance));
                            return false;
                        }
                        else
                        {
                            partProfile += strRet;
                            ApnsPartProfile += strRet;
                        }


                    }
                    else
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                         System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Restriction_INSERT",
                                         JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Restriction_list), JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Restriction_Advance));
                        return false;
                    }
                }
            }
            #endregion

            string payload = @"</array><key>PayloadDisplayName</key>
	                                <string>RestrictionProfile</string>
	                                <key>PayloadIdentifier</key>
	                                <string>Emerico-MacBook-Pro.D84CB0DD-DCDD-4FB6-95B5-3CC944442D93</string>
	                                <key>PayloadRemovalDisallowed</key>
	                                <false/>
	                                <key>PayloadType</key>
	                                <string>Configuration</string>
	                                <key>PayloadUUID</key>
	                                <string>6879BF02-F36A-4B17-B62C-B8E8BBFA1FA7</string>
	                                <key>PayloadVersion</key>
	                                <integer>1</integer></dict>";

            Profile_APNS = "<dict><key>PayloadContent</key><array>" + partProfile + payload;


            #region File Save
            string Profile_APNS_Directory = ConfigHelper.Profile_APNS_Path;
            string Profile_APNS_Path = Profile_APNS_Directory + "\\" + my_MDM_Full_Profile.mDM_Profile_General.Profile_ID + ".plist";

            if (!Directory.Exists(Profile_APNS_Directory))
                Directory.CreateDirectory(Profile_APNS_Directory);
            else
            {
                // Delete File when file exists
                if (File.Exists(Profile_APNS_Path))
                {
                    File.Delete(Profile_APNS_Path);
                }
            }

            // save in  temparary folder before transfer to server
            using (FileStream fs = File.Create(Profile_APNS_Path))
            {
                Byte[] file = new UTF8Encoding(true).GetBytes(Profile_APNS);

                fs.Write(file, 0, file.Length);
            }

            //Save the file to diff server
            //File_PassSoapClient my_PassSoap = new File_PassSoapClient();
            //En_MDM_FilePass my_En_MDM_FilePass_APNS = FileToByteArray(Path.GetFileName(Profile_APNS_Path), Profile_APNS_Path, "APNS");

            //ret = my_PassSoap.Plist_Pass(my_En_MDM_FilePass_APNS);
            //if (!ret)
            //{
            //    return ret;
            //}

            #endregion

            #region Update Profile for Profile_APNS_Path and Profile_Enroll_Path


            MDM_Profile_General my_General = new MDM_Profile_General();
            my_General = my_MDM_Full_Profile.mDM_Profile_General;
            my_General.Profile_APNS_Path = @"E:\IOS_MDM\Publishers\FilePass\plist\APNS\" + Path.GetFileName(Profile_APNS_Path); //path important for generating .mobileconfig (mobile config) from .plist (payload content)


            ret = General_Function.General_Update(my_General, "CHECKERMAKER");
            #endregion
            ret = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
        return ret;
    }


    /// <summary>
    /// Update a specified profile's information based on profile ID such as general, cellular, passcode information into the database.
    /// If successful, generate XML format for all profile information sections and store the newly created
    /// mobileconfig filepath in the database for retrieval later. 
    /// </summary>
    /// <param name="my_MDM_Full_Profile"></param>
    /// <returns></returns>
    public static Boolean Update_All(MDM_Full_Profile my_MDM_Full_Profile)
    {
        bool ret = false;
        string Profile_APNS = string.Empty;
        string Profile_Enroll = string.Empty;
        string partProfile = string.Empty;
        string strRet = string.Empty;
        string strRet_ANPSPRofile = string.Empty;
        Guid deProfileID = my_MDM_Full_Profile.mDM_Profile_General.Profile_ID;
        string ApnsPartProfile = string.Empty;

        try
        {
            #region General
            if (General_Function.General_Update(my_MDM_Full_Profile.mDM_Profile_General, ""))
            {
                if (General_Function.General_Branch_Delete_Insert(my_MDM_Full_Profile.mDM_Profile_General_BranchID))
                {
                    strRet_ANPSPRofile = General_XML.General_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_General);
                    if (strRet.ToUpper() == "ERROR")
                    {


                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "General_XML",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_General));


                        return false;
                    }
                    else
                    {
                        Profile_APNS += strRet_ANPSPRofile;
                    }
                }
                else
                {
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "General_INSERT",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_General));
                    return false;
                }
            }

            #endregion

            #region Cellular

            if (Cellular_Function.Cellular_Delete_Insert(my_MDM_Full_Profile.mDM_Profile_Cellular, deProfileID))
            {
                if (my_MDM_Full_Profile.mDM_Profile_Cellular.Profile_ID != Guid.Empty)
                {
                    strRet = Cellular_XML.Cellular_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_Cellular);
                    if (strRet.ToUpper() == "ERROR")
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Cellular_XML",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Cellular));
                        return false;
                    }

                    else
                    {
                        partProfile += strRet;
                        ApnsPartProfile += strRet;
                    }

                }

            }
            else
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Cellular_INSERT",
                                 JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Cellular));
                return false;
            }




            #endregion

            #region Passcode
            if (Passcode_Function.Passcode_Delete_Insert(my_MDM_Full_Profile.mDM_Profile_Passcode, deProfileID))
            {
                if (my_MDM_Full_Profile.mDM_Profile_Passcode.Profile_ID != Guid.Empty)
                {
                    strRet = Passcode_XML.Passcode_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_Passcode);
                    if (strRet.ToUpper() == "ERROR")
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Passcode_XML",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Passcode));
                        return false;
                    }

                    else
                    {
                        partProfile += strRet;
                        ApnsPartProfile += strRet;
                    }

                }

            }

            else
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Passcode_INSERT",
                                 JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Passcode));
                return false;
            }
            #endregion

            #region LDAP


            //if (LDAP_Function.Main_LDAP_Update(my_MDM_Full_Profile.mDM_Profile_LDAP_list, my_MDM_Full_Profile.mDM_Profile_LDAP_SearchSettings_List, deProfileID))
            //{
            //    if (my_MDM_Full_Profile.mDM_Profile_LDAP_list.First().Profile_ID != Guid.Empty)
            //    {
            //        strRet = LDAP_XML.LDAP_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_LDAP_list, my_MDM_Full_Profile.mDM_Profile_LDAP_SearchSettings_List);
            //        if (strRet.ToUpper() == "ERROR")
            //        {
            //            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
            //                         System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            //                         System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "LDAP_XML",
            //                         JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_LDAP_list), JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_LDAP_SearchSettings_List));
            //            return false;
            //        }
            //        else
            //        {
            //            partProfile += strRet;
            //            //ApnsPartProfile += strRet;
            //        }
            //    }

            //}
            //else
            //{
            //    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
            //                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            //                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "LDAP_INSERT",
            //                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_LDAP_list), JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_LDAP_SearchSettings_List));
            //    return false;
            //}
            #endregion

            #region VPN
            if (VPN_Function.Main_VPN_Update(my_MDM_Full_Profile.mDM_Profile_VPN_list, deProfileID))
            {
                if (my_MDM_Full_Profile.mDM_Profile_VPN_list.First().Profile_ID != Guid.Empty)
                {
                    strRet = VPN_XML.VPN_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_VPN_list);
                    if (strRet.ToUpper() == "ERROR")
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "VPN_XML",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_VPN_list));
                        return false;
                    }
                    else
                    {
                        partProfile += strRet;
                        // ApnsPartProfile += strRet;
                    }
                }


            }
            else
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "VPN_INSERT",
                                 JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_VPN_list));
                return false;
            }


            #endregion

            #region GeneralRemove

            strRet = General_XML.GeneralRemoveProfile_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_General);

            if (strRet.ToUpper() == "ERROR")
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Restriction_XML",
                             JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Cellular));
                return false;
            }
            else
            {
                partProfile += strRet;
                //ApnsPartProfile += strRet;
            }


            #endregion

            #region WIFI

            if (WIFI_Function.Main_Wifi_Update(my_MDM_Full_Profile.mDM_Profile_WIFI_list, deProfileID))
            {
                if (my_MDM_Full_Profile.mDM_Profile_WIFI_list.First().Profile_ID != Guid.Empty)
                {
                    strRet = WIFI_XML.WIFI_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_WIFI_list);
                    if (strRet.ToUpper() == "ERROR")
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "WIFI_XML",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_WIFI_list));
                        return false;
                    }

                    else
                    {
                        partProfile += strRet;
                        //ApnsPartProfile += strRet;
                    }
                }

            }
            else
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "WIFI_INSERT",
                                 JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_WIFI_list));
                return false;
            }

            #endregion


            #region Restriction

            if (Restriction_Function.Main_Restriction_Update(my_MDM_Full_Profile.mDM_Profile_Restriction_list, my_MDM_Full_Profile.mDM_Profile_Restriction_Advance, deProfileID))
            {
                if (my_MDM_Full_Profile.mDM_Profile_Restriction_list.First().Profile_ID != Guid.Empty)
                {

                    strRet = Restriction_XML.Restriction_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_Restriction_list, my_MDM_Full_Profile.mDM_Profile_Restriction_Advance);
                    if (strRet.ToUpper() == "ERROR")
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Restriction_XML",
                                     JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Cellular));
                        return false;
                    }
                    else
                    {
                        partProfile += strRet;
                        // ApnsPartProfile += strRet;

                    }


                }

            }

            else
            {
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Restriction_INSERT",
                                 JsonConvert.SerializeObject(my_MDM_Full_Profile.mDM_Profile_Restriction_Advance));
                return false;
            }
            #endregion



            #region Profile for enrollment
            Profile_Enroll = ProfileEnroll_XML.ProfileEnroll_XMLGenerator(my_MDM_Full_Profile.mDM_Profile_General);

            #endregion


            Profile_Enroll = Profile_Enroll.Replace("@@@@@@@@", partProfile);

            Profile_APNS = Profile_APNS.Replace("@@@@@@@@", partProfile);

            #region File Save
            string Profile_APNS_Path = ConfigurationManager.AppSettings["Profile_APNS_Path"] + "/" + my_MDM_Full_Profile.mDM_Profile_General.Profile_ID + ".plist";
            string Profile_Enroll_Path = ConfigurationManager.AppSettings["Profile_Enroll_Path"] + "/" + my_MDM_Full_Profile.mDM_Profile_General.Profile_ID + ".plist";


            // Delete File when file exists
            if (File.Exists(Profile_APNS_Path))
            {
                File.Delete(Profile_APNS_Path);
            }

            if (File.Exists(Profile_Enroll_Path))
            {
                File.Delete(Profile_Enroll_Path);
            }

            // save in  temparary folder
            using (FileStream fs = File.Create(Profile_APNS_Path))
            {
                Byte[] file = new UTF8Encoding(true).GetBytes(Profile_APNS);

                fs.Write(file, 0, file.Length);
            }

            using (FileStream fs = File.Create(Profile_Enroll_Path))
            {
                Byte[] file = new UTF8Encoding(true).GetBytes(Profile_Enroll);

                fs.Write(file, 0, file.Length);
            }

            #endregion

            //Save the file to diff server

            //File_PassSoapClient my_PassSoap = new File_PassSoapClient();

            //En_MDM_FilePass my_En_MDM_FilePass_APNS = FileToByteArray(my_MDM_Full_Profile.mDM_Profile_General.Profile_ID.ToString() + ".plist", Profile_APNS_Path, "APNS");
            //En_MDM_FilePass my_En_MDM_FilePass_Enroll = FileToByteArray(my_MDM_Full_Profile.mDM_Profile_General.Profile_ID.ToString() + ".plist", Profile_Enroll_Path, "Enroll");

            //string str = JsonConvert.SerializeObject(my_En_MDM_FilePass_APNS);

            //ret = my_PassSoap.Plist_Pass(my_En_MDM_FilePass_APNS);
            //if (!ret)
            //{
            //    return ret;
            //}

            //ret = my_PassSoap.Plist_Pass(my_En_MDM_FilePass_Enroll);
            //if (!ret)
            //{
            //    return ret;
            //}


            #region Update Profile for Profile_APNS_Path and Profile_Enroll_Path
            MDM_Profile_General my_Profile_General = new MDM_Profile_General();
            my_Profile_General.Profile_ID = my_MDM_Full_Profile.mDM_Profile_General.Profile_ID;

            DataTable dt = General_Function.General_SelectAll(my_Profile_General);

            foreach (DataRow dr in dt.Rows)
            {
                my_Profile_General.Profile_ID = Guid.Parse(dr["Profile_ID"].ToString());
                my_Profile_General.Name = dr["Name"].ToString();
                my_Profile_General.Identifier = dr["Identifier"].ToString();
                my_Profile_General.Organization = dr["Organization"].ToString();
                my_Profile_General.Description = dr["Description"].ToString();
                my_Profile_General.ConsentMessage = dr["ConsentMessage"].ToString();
                my_Profile_General.Security = dr["Security"].ToString();
                my_Profile_General.AuthorizationPassword = dr["AuthorizationPassword"].ToString(); ;
                my_Profile_General.AutomaticallyRemoveProfile = dr["AutomaticallyRemoveProfile"].ToString();
                if (my_Profile_General.AutomaticallyRemoveProfile.ToUpper() != "NEVER")
                {
                    my_Profile_General.AutomaticallyRemoveProfile_Date = DateTime.Parse(dr["AutomaticallyRemoveProfile_Date"].ToString());
                    my_Profile_General.AutomaticallyRemoveProfile_Days = dr["AutomaticallyRemoveProfile_Days"].ToString();
                    my_Profile_General.AutomaticallyRemoveProfile_Hours = dr["AutomaticallyRemoveProfile_Hours"].ToString();
                }


                my_Profile_General.Profile_APNS_Path = Profile_APNS_Path;
                my_Profile_General.Profile_Enroll_Path = Profile_Enroll_Path;
            }

            General_Function.General_Update(my_Profile_General, "");

            #endregion

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

    /// <summary>
    /// Converts the plist file to byte array format before sending to a different server. 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="filePath"></param>
    /// <param name="PlistCategory"></param>
    /// <returns></returns>
    //public static En_MDM_FilePass FileToByteArray(string fileName, string filePath, string PlistCategory)
    //{
    //    //byte[] fileContent = null;

    //    En_MDM_FilePass my_MDM_FilePass = new En_MDM_FilePass();
    //    my_MDM_FilePass.plist_Category = PlistCategory;
    //    my_MDM_FilePass.fileNameWithExtension = fileName;
    //    my_MDM_FilePass.fileContent = File.ReadAllBytes(filePath);
    //    return my_MDM_FilePass;
    //}

}
