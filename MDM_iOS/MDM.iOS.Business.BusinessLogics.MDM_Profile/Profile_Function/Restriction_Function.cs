using System.Data;
using Dashboard.Common.Business.Component;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Common.Data.Component;
using Newtonsoft.Json;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function
{
    /// <summary>
    /// CRUD (Create, Read, Update and Delete) operations on the restrictions database table. 
    /// </summary>
    public class Restriction_Function
    {
        public static DataTable RestrictionMenu_Functionality_SelectAll()
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Restriction_Menu
                               where o.Active == 1 && o.Partition == 1
                               orderby o.RGroup, o.GroupHeader descending, o.Queue
                               select new
                               {
                                   o.RID,
                                   o.RestrictionName,
                                   o.RestrictionDesc,
                                   o.Queue,
                                   o.Active,
                                   o.RGroup,
                                   o.GroupHeader,
                                   o.NumberType,
                                   o.Partition
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

        public static string RestrictionMenu_Functionality(MDM_Restriction_Menu my_MDM_Restriction_Menu)
        {
            string ret = string.Empty;
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Restriction_Menu
                               where o.Active == 1
                               && o.RID == my_MDM_Restriction_Menu.RID
                               orderby o.RGroup, o.GroupHeader descending, o.Queue
                               select new
                               {
                                   o.RID
                                   ,
                                   o.RestrictionName
                                   ,
                                   o.RestrictionDesc
                                   ,
                                   o.Queue
                                   ,
                                   o.Active
                                   ,
                                   o.RGroup
                                   ,
                                   o.GroupHeader
                                   ,
                                   o.NumberType
                                   ,
                                   o.Partition
                               };
                    DataTable dt = LINQToDataTable.LINQResultToDataTable(data);
                    ret = dt.Rows[0]["RestrictionName"].ToString();



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

        public static DataTable RestrictionMenu_App_SelectAll()
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Restriction_Menu
                               where o.Active == 1 && o.Partition == 2
                               orderby o.RGroup, o.GroupHeader descending, o.Queue
                               select new
                               {
                                   o.RID
                                   ,
                                   o.RestrictionName
                                   ,
                                   o.RestrictionDesc
                                   ,
                                   o.Queue
                                   ,
                                   o.Active
                                   ,
                                   o.RGroup
                                   ,
                                   o.GroupHeader
                                   ,
                                   o.NumberType
                                   ,
                                   o.Partition
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

        public static DataTable RestrictionMenu_App_Advance_SelectAll(MDM_Profile_Restriction_Advance my_Profile_Restriction_apps)
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Profile_Restriction_Advance
                               where o.Profile_ID == my_Profile_Restriction_apps.Profile_ID
                               select new
                               {
                                   o.Profile_ID,
                                   o.RestrictAppUsage,
                                   o.App_Identify,
                                   o.AcceptCookies

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



        public static Boolean Main_Restriction_Insert(List<MDM_Profile_Restriction> my_Restriction_List, MDM_Profile_Restriction_Advance my_Restriction_Advance)
        {
            bool ret = false;
            try
            {
                foreach (MDM_Profile_Restriction my_Restriction in my_Restriction_List)
                {
                    if (!Restriction_Insert(my_Restriction))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                          System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "MDM_Profile_Restriction Insert Failed",
                          JsonConvert.SerializeObject(my_Restriction_List), JsonConvert.SerializeObject(my_Restriction), JsonConvert.SerializeObject(my_Restriction_Advance));
                        ret = false;
                        return ret;
                    }
                }

                if (my_Restriction_Advance.Profile_ID != Guid.Empty)
                {
                    if (Restriction_Advance_Insert(my_Restriction_Advance))
                    {
                        ret = true;
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

        public static Boolean Main_Restriction_Update(List<MDM_Profile_Restriction> my_Restriction_List, MDM_Profile_Restriction_Advance my_Restriction_Advance, Guid ProfileID)
        {
            bool ret = false;
            try
            {

                if (!Restriction_Delete(my_Restriction_List.First(), ProfileID))
                {
                    ret = false;
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                            System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "MDM_Profile_Restriction Delete Failed for update",
                            JsonConvert.SerializeObject(my_Restriction_List), JsonConvert.SerializeObject(my_Restriction_Advance));

                    return ret;
                }



                if (my_Restriction_List.First().Profile_ID != Guid.Empty)
                {
                    foreach (MDM_Profile_Restriction my_Restriction in my_Restriction_List)
                    {
                        if (!Restriction_Insert(my_Restriction))
                        {
                            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "MDM_Profile_Restriction Insert Failed for update",
                              JsonConvert.SerializeObject(my_Restriction_List), JsonConvert.SerializeObject(my_Restriction), JsonConvert.SerializeObject(my_Restriction_Advance));
                            ret = false;
                            return ret;
                        }
                    }
                }




                if (!Restriction_Advance_Delete(my_Restriction_Advance, ProfileID))
                {
                    ret = false;
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "MDM_Profile_Restriction_Advance Delete Failed for update",
                             JsonConvert.SerializeObject(my_Restriction_List), JsonConvert.SerializeObject(my_Restriction_Advance));

                    return ret;
                }

                if (my_Restriction_Advance.Profile_ID != Guid.Empty)
                {
                    if (!Restriction_Advance_Insert(my_Restriction_Advance))
                    {
                        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                              System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                              System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "MDM_Profile_Restriction_Advance Insert Failed for update",
                              JsonConvert.SerializeObject(my_Restriction_List), JsonConvert.SerializeObject(my_Restriction_Advance));
                    }
                    else
                    {
                        ret = true;


                    }

                }
                else
                {
                    ret = true;

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


        public static DataTable Restriction_SelectAll(MDM_Profile_Restriction my_Restriction)
        {
            DataTable ret = new DataTable();
            try
            {
                using (BankIslamEntities db = new BankIslamEntities())
                {
                    var data = from o in db.MDM_Profile_Restriction
                               where o.Profile_ID == my_Restriction.Profile_ID
                               select new
                               {
                                   o.Profile_ID,
                                   o.RID,
                                   o.RestrictionName,
                                   o.IsCheck
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

        protected static Boolean Restriction_Insert(MDM_Profile_Restriction my_Restriction)
        {
            bool ret = false;

            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    entities.MDM_Profile_Restriction.Add(my_Restriction);
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

        protected static Boolean Restriction_Delete(MDM_Profile_Restriction my_Restriction, Guid ProfileID)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {


                    List<MDM_Profile_Restriction> res = entities.MDM_Profile_Restriction.Where(p => p.Profile_ID == ProfileID)
                      .ToList();

                    if (res.Count == 0)
                    {
                        ret = true;
                        return ret;

                    }
                    foreach (MDM_Profile_Restriction r in res)
                    {
                        entities.MDM_Profile_Restriction.Remove(r);

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

        protected static Boolean Restriction_Advance_Insert(MDM_Profile_Restriction_Advance my_Restriction_Advance)
        {
            bool ret = false;

            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    entities.MDM_Profile_Restriction_Advance.Add(my_Restriction_Advance);
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

        protected static Boolean Restriction_Advance_Delete(MDM_Profile_Restriction_Advance my_Restriction_Advance, Guid ProfileID)
        {
            bool ret = false;
            try
            {
                using (BankIslamEntities entities = new BankIslamEntities())
                {
                    List<MDM_Profile_Restriction_Advance> ad = entities.MDM_Profile_Restriction_Advance.Where(p => p.Profile_ID == ProfileID).ToList();


                    if (ad.Count == 0)
                    {
                        ret = true;
                        return ret;
                    }

                    foreach (MDM_Profile_Restriction_Advance advance in ad)
                    {
                        entities.MDM_Profile_Restriction_Advance.Remove(advance);
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
    }
}
