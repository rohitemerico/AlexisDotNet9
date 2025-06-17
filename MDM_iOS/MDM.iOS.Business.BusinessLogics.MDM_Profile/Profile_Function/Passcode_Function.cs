using System.Data;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Common.Data.Component;
using Newtonsoft.Json;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function
{
    /// <summary>
    /// CRUD (Create, Read, Update and Delete) operations on the passcode database table. 
    /// </summary>
    public class Passcode_Function
    {

        public static DataTable Passcode_SelectAll(MDM_Profile_Passcode my_Passcode)
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Profile_Passcode
                               where o.Profile_ID == my_Passcode.Profile_ID
                               select new
                               {
                                   o.Profile_ID,
                                   o.AllowSimpleValue,
                                   o.Requirealphanumericvalue,
                                   o.MinimumPasscodeLength,
                                   o.MinimumNumberOfComplexCharacters,
                                   o.MaximumPasscodeAge,
                                   o.MaximumAutoLock,
                                   o.PasscodeHistory,
                                   o.MaximumGracePeriod,
                                   o.MaximumNumberOfFailedAttempts
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


        public static Boolean Passcode_Delete_Insert(MDM_Profile_Passcode my_Passcode, Guid ProfileID)
        {
            bool ret = false;

            try
            {

                if (Passcode_Delete(my_Passcode, ProfileID))
                {
                    if (my_Passcode.Profile_ID != Guid.Empty)
                    {
                        if (Passcode_Insert(my_Passcode))
                        {
                            ret = true;
                        }
                        else
                        {
                            ret = false;
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Insert_Passcode_Problems",
                                             JsonConvert.SerializeObject(my_Passcode));
                        }
                    }
                    else if (my_Passcode.Profile_ID == Guid.Empty)
                    {
                        ret = true;
                    }

                }
                else
                {
                    ret = false;
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Delete_Passcode_Problems",
                                     JsonConvert.SerializeObject(my_Passcode));
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

        public static Boolean Passcode_Insert(MDM_Profile_Passcode my_Passcode)
        {
            bool ret = false;

            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    entities.MDM_Profile_Passcode.Add(my_Passcode);
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

        public static Boolean Passcode_Update(MDM_Profile_Passcode my_Passcode)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {

                    MDM_Profile_Passcode Passcode = (from c in entities.MDM_Profile_Passcode
                                                     where c.Profile_ID == my_Passcode.Profile_ID
                                                     select c).FirstOrDefault();

                    Passcode = my_Passcode;

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

        public static Boolean Passcode_Delete(MDM_Profile_Passcode my_Passcode, Guid ProfileID)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    MDM_Profile_Passcode Passcode = (from c in entities.MDM_Profile_Passcode
                                                     where c.Profile_ID == ProfileID
                                                     select c).FirstOrDefault();
                    if (Passcode == null)
                    {
                        ret = true;
                        return ret;
                    }
                    entities.MDM_Profile_Passcode.Remove(Passcode);
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
