using System.Data;
using System.Text;
using Alexis.Common;
using Dashboard.Common.Business.Component;
using Dashboard.Entities.ADCB;
using MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
using MDM.iOS.Common.Data.Component;
using Newtonsoft.Json;

namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.Profile_Function;

/// <summary>
/// CRUD (Create, Read, Update and Delete) operations on the General database table. 
/// </summary>
public class General_Function
{

    #region General

    public static DataTable General_SelectAll(MDM_Profile_General my_general)
    {
        DataTable ret = new DataTable();
        try
        {
            using (BankIslamEntities db = new BankIslamEntities())
            {
                List<MDM_Profile_General> dataList;

                if (my_general.Profile_ID != Guid.Empty)
                {
                    dataList = db.MDM_Profile_General
                                 .Where(o => o.Profile_ID == my_general.Profile_ID)
                                 .OrderByDescending(o => o.CreatedDate)
                                 .ToList();
                }
                else
                {
                    dataList = db.MDM_Profile_General.AsEnumerable().ToList();
                }

                ret = DataHelper.ToDataTable(dataList);
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
        return ret;
    }



    //public static DataTable General_SelectAll(MDM_Profile_General my_general)
    //{
    //    DataTable ret = new DataTable();
    //    try
    //    {
    //        using (BankIslamEntities db = new BankIslamEntities())
    //        {
    //            if (my_general.Profile_ID != Guid.Empty)
    //            {

    //                var data = from o in db.MDM_Profile_General

    //                           where o.Profile_ID == my_general.Profile_ID
    //                           orderby o.CreatedDate descending
    //                           select new
    //                           {
    //                               o.Profile_ID,
    //                               o.Name,
    //                               o.Identifier,
    //                               //o.Description,
    //                               //o.AuthorizationPassword,
    //                               //o.AutomaticallyRemoveProfile,
    //                               //o.AutomaticallyRemoveProfile_Date,
    //                               //o.AutomaticallyRemoveProfile_Days,
    //                               //o.AutomaticallyRemoveProfile_Hours,
    //                               //o.ConsentMessage,
    //                               //o.Organization,
    //                               //o.Security,
    //                               //o.Branch_ID,
    //                               //o.LastUpdateDate,
    //                               //o.pStatus,
    //                               //o.LastUpdateBy,
    //                               //o.CProfileId,
    //                               //o.CreatedDate,
    //                               //o.CreatedBy

    //                           };
    //                ret = LINQToDataTable.LINQResultToDataTable(data);
    //            }
    //            else
    //            {
    //                var data = from o in db.MDM_Profile_General
    //                           select new
    //                           {
    //                               o.Profile_ID,
    //                               o.Name,
    //                               o.Identifier,
    //                               //o.Description,
    //                               //o.AuthorizationPassword,
    //                               //o.AutomaticallyRemoveProfile,
    //                               //o.AutomaticallyRemoveProfile_Date,
    //                               //o.AutomaticallyRemoveProfile_Days,
    //                               //o.AutomaticallyRemoveProfile_Hours,
    //                               //o.ConsentMessage,
    //                               //o.Organization,
    //                               //o.Security,
    //                               //o.Branch_ID,
    //                               //o.LastUpdateDate,
    //                               //o.pStatus,
    //                               //o.LastUpdateBy,
    //                               //o.CProfileId,
    //                               //o.CreatedBy,
    //                               //o.CreatedDate
    //                           };
    //                ret = LINQToDataTable.LINQResultToDataTable(data);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
    //    }
    //    return ret;
    //}

    public static DataTable General_Retrieve_BranchID(Guid profileid)
    {
        DataTable ret = new DataTable();
        try
        {
            using (BankIslamEntities db = new BankIslamEntities())
            {
                var data = from o in db.MDM_Profile_General_BranchID
                           join c in db.User_Branch on o.Branch_ID equals c.bID
                           where o.Profile_ID == profileid
                           select new { c.bDesc };
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

    public static Boolean General_Delete_Insert(MDM_Profile_General my_General)
    {
        bool ret = false;
        try
        {
            if (General_Delete(my_General))
            {
                if (General_Insert(my_General))
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Insert_General_Problems",
                                     JsonConvert.SerializeObject(my_General));
                }
            }
            else
            {
                ret = false;
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Delete_General_Problems",
                                 JsonConvert.SerializeObject(my_General));
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

    public static Boolean General_Insert(MDM_Profile_General my_general)
    {
        bool ret = false;
        try
        {
            using (BankIslamEntities entities = new BankIslamEntities())
            {
                entities.MDM_Profile_General.Add(my_general);
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

    public static Boolean General_Update(MDM_Profile_General my_general, string UpdateType)
    {

        bool ret = false;

        StringBuilder sql = new StringBuilder();
        List<Params> myParams = new List<Params>();
        // BankIslamEntities entities = new BankIslamEntities();
        try
        {
            if (UpdateType.ToUpper() == "CHECKERMAKER")
            {
                sql.AppendLine(@"update MDM_Profile_General set ");
                if (my_general.pStatus.HasValue)
                {
                    sql.AppendLine("pstatus = @pstatus ,");
                    myParams.Add(new Params("@pstatus", "INT", my_general.pStatus));
                }
                if (my_general.LastUpdateDate.HasValue)
                {
                    sql.AppendLine("LastUpdateDate = @LastUpdateDate ,");
                    myParams.Add(new Params("@LastUpdateDate", "DATETIME", my_general.LastUpdateDate));
                }

                if (my_general.LastUpdateBy.HasValue)
                {
                    sql.AppendLine("LastUpdateBy = @LastUpdateBy ,");
                    myParams.Add(new Params("@LastUpdateBy", "GUID", my_general.LastUpdateBy));
                }

                if (my_general.CProfileId.HasValue)
                {
                    sql.AppendLine("CProfileId = @CProfileId ,");
                    myParams.Add(new Params("@CProfileId", "GUID", my_general.CProfileId));
                }
                if (my_general.Profile_APNS_Path != null)
                {
                    sql.AppendLine("Profile_APNS_Path = @Profile_APNS_Path ,");
                    myParams.Add(new Params("@Profile_APNS_Path", "NVARCHAR", my_general.Profile_APNS_Path));
                }
                if (my_general.Profile_Enroll_Path != null)
                {
                    sql.AppendLine("Profile_Enroll_Path = @Profile_Enroll_Path ,");
                    myParams.Add(new Params("@Profile_Enroll_Path", "NVARCHAR", my_general.Profile_Enroll_Path));
                }

                string a = sql.ToString().Trim();
                a = a.Remove(a.Length - 1, 1);
                sql.Clear();
                sql.AppendLine(a + " where Profile_ID = @pid");
                myParams.Add(new Params("@pid", "GUID", my_general.Profile_ID));
            }


            else if (UpdateType.ToUpper() == "UPDATEOLDDATASTATUS")
            {
                sql.AppendLine(@"update MDM_Profile_General set pstatus = @pstat where profile_Id = @pid");
                myParams.Add(new Params("@pstat", "INT", my_general.pStatus));
                myParams.Add(new Params("@pid", "GUID", my_general.Profile_ID));

            }
            ret = SqlDataControl.Input(sql.ToString(), myParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                             System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                             System.Reflection.MethodBase.GetCurrentMethod().Name,
                             ex);
        }

        finally
        {

            sql.Clear();
            myParams.Clear();
        }
        return ret;
    }

    public static Boolean General_Delete(MDM_Profile_General my_general)
    {
        bool ret = false;
        try
        {
            using (BankIslamEntities entities = new BankIslamEntities())
            {
                MDM_Profile_General General = (from c in entities.MDM_Profile_General
                                               where c.Profile_ID == my_general.Profile_ID
                                               select c).FirstOrDefault();
                entities.MDM_Profile_General.Remove(General);
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


    #region General_Branch

    public static DataTable General_Branch_SelectAll(MDM_Profile_General_BranchID my_general_branch)
    {
        DataTable ret = new DataTable();
        try
        {

            using (BankIslamEntities db = new BankIslamEntities())
            {
                var data = from o in db.MDM_Profile_General_BranchID
                           where o.Profile_ID == my_general_branch.Profile_ID

                           select new
                           {
                               o.Profile_ID,
                               o.Branch_ID
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

    public static Boolean General_Branch_Delete_Insert(List<MDM_Profile_General_BranchID> my_general_branch)
    {
        bool ret = false;

        try
        {
            foreach (MDM_Profile_General_BranchID branch in my_general_branch)
            {
                if (!General_BranchID_Delete(branch, my_general_branch))
                {
                    ret = false;
                    Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                     System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Delete_General_Problems",
                                     JsonConvert.SerializeObject(my_general_branch));
                }
            }
            if (General_BranchID_Insert(my_general_branch))
            {
                ret = true;
            }
            else
            {
                ret = false;
                Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                                 System.Reflection.MethodBase.GetCurrentMethod().Name, ',', "Insert_General_Problems",
                                 JsonConvert.SerializeObject(my_general_branch));
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

    public static Boolean General_BranchID_Insert(List<MDM_Profile_General_BranchID> my_general_branch)
    {
        bool ret = false;
        try
        {
            if (my_general_branch == null || !my_general_branch.Any())
                return false;
            using (BankIslamEntities entities = new BankIslamEntities())
            {
                foreach (MDM_Profile_General_BranchID general_branch in my_general_branch)
                {
                    entities.MDM_Profile_General_BranchID.Add(general_branch);
                }
                if (entities.SaveChanges() > 0)
                {
                    ret = true;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
        }
        return ret;
    }

    public static Boolean General_BranchID_Delete(MDM_Profile_General_BranchID my_general_branch, List<MDM_Profile_General_BranchID> generalbranchlist)
    {
        bool ret = false;
        try
        {

            using (BankIslamEntities entities = new BankIslamEntities())
            {

                List<MDM_Profile_General_BranchID> Gen_Branch = (from c in entities.MDM_Profile_General_BranchID
                                                                 where c.Profile_ID == my_general_branch.Profile_ID
                                                                 select c).ToList();

                if (Gen_Branch.Count == 0)
                {
                    ret = true;
                    return ret;

                }

                foreach (MDM_Profile_General_BranchID branch in Gen_Branch)
                {
                    entities.MDM_Profile_General_BranchID.Remove(branch);
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

    public static Boolean General_BranchID_Update(MDM_Profile_General_BranchID my_general_branch)
    {

        bool ret = false;
        BankIslamEntities entities = new BankIslamEntities();
        try
        {
            List<MDM_Profile_General_BranchID> branch_Old = (from c in entities.MDM_Profile_General_BranchID
                                                             where c.Profile_ID == my_general_branch.Profile_ID
                                                             select c).ToList();


            foreach (MDM_Profile_General_BranchID gen_branch in branch_Old)
            {

                gen_branch.Profile_ID = my_general_branch.Profile_ID;
                gen_branch.cProfile_ID = my_general_branch.cProfile_ID;
            }



            //branch_Old.cProfile_ID = branch.cProfile_ID;
            //branch_Old



            if (entities.SaveChanges() != 0)
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
    #endregion
}
