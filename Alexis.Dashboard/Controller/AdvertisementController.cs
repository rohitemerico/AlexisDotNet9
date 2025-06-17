using System.Data;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

public class AdvertisementController : GlobalController
{
    public DataTable isAdvertisementExist(string fileName)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * ");
            sql.AppendLine("from Machine_Advertisement ");
            sql.AppendLine("where aName = @aName ");
            MyParams.Add(new Params("@aName", "NVARCHAR", fileName));

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public string IsValidatedToInactive(Guid id)
    {
        string ret = "";
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from machine_group mg ");
            sql.AppendLine("join ADVERTISEMENT_GROUP ag on (mg.kid = AG.GROUPID) ");
            sql.AppendLine("join machine_advertisement ma on (AG.ADVID = ma.aid) ");
            sql.AppendLine("where ma.aid = @id and mg.kStatus = @status ");

            MyParams.Add(new Params("@id", "GUID", id));
            MyParams.Add(new Params("@status", "INT", 1));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            foreach (DataRow dr in dt.Rows)
                ret += "The advertisement is assigned to Machine Group, " + dr["kDesc"].ToString() + "! The Advertisement Cannot be Inactive! <br />";
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public DataTable getAllAdvertisement()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select ma.*, ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uname UpdatedBy, ");
            sql.AppendLine("case ma.aStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end AdvertStatus ");
            sql.AppendLine("from Machine_Advertisement ma ");
            sql.AppendLine("left join User_Login ulc on (ma.aCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (ma.aApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login ulu on (ma.aUpdatedBy = ulu.aID) ");
            sql.AppendLine("left join User_Login uld on (ma.aDeclineBy = uld.aID) ");
            //, Advertisement_Package aPack ");
            //sql.AppendLine("where ma.aID = aPack.Ref_advID ");
            sql.AppendLine("where aIsBackgroundImg = @aIsBackgroundImg ");
            MyParams.Add(new Params("@aIsbackgroundImg", "BIT", 0));

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public DataTable getAdvertisementById(Guid id)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select ma.*, ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ");
            sql.AppendLine("case ma.aStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end AdvertStatus ");
            sql.AppendLine("from Machine_Advertisement ma ");
            sql.AppendLine("left join User_Login ulc on (ma.aCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (ma.aApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (ma.aDeclineBy = uld.aID) ");
            sql.AppendLine("where ma.aid = @aid ");
            MyParams.Add(new Params("@aid", "GUID", id));

            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool insertAdvert(MAdvertisement entity)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("insert into Machine_Advertisement (aID, aName, aDesc, ARELATIVEPATHURL, aDuration, aIsBackgroundImg, aCreatedDate, aCreatedBy, aRemarks, aStatus) ");
            sql.AppendLine("values (@aID, @aName, @aDesc, @aDirectory, @aDuration, @IsBackgroundImg, @aCreatedDate, @aCreatedBy, @aRemarks, @aStatus) ");
            MyParams.Add(new Params("@aID", "GUID", entity.AID));
            MyParams.Add(new Params("@aName", "NVARCHAR", entity.AName));
            MyParams.Add(new Params("@aDesc", "NVARCHAR", entity.ADesc));
            MyParams.Add(new Params("@aDirectory", "NVARCHAR", entity.ADirectory));
            MyParams.Add(new Params("@aDuration", "INT", entity.ADuration));
            MyParams.Add(new Params("@IsBackgroundImg", "BIT", entity.AIsBackgroundIMG));
            MyParams.Add(new Params("@aCreatedDate", "DATETIME", entity.CreatedDate));
            MyParams.Add(new Params("@aCreatedBy", "GUID", entity.CreatedBy));
            if (String.IsNullOrEmpty(entity.Remarks))
                MyParams.Add(new Params("@aRemarks", "NVARCHAR", DBNull.Value));
            else
                MyParams.Add(new Params("@aRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@aStatus", "INT", 0));//entity.Status));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool updateAdvert(MAdvertisement entity)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update Machine_Advertisement set ");
            if (entity.AName != "#NOT CHANGE#")
            {
                sql.AppendLine("aName=@aName, ");
                MyParams.Add(new Params("@aName", "NVARCHAR", entity.AName));
            }
            sql.AppendLine("aDesc=@aDesc, ARELATIVEPATHURL=@aDirectory, aDuration=@aDuration, aIsBackgroundImg=@aIsBackgroundImg, aUpdatedDate=@aUpdatedDate, aUpdatedBy=@aUpdatedBy, aRemarks=@aRemarks, aStatus=@aStatus");
            sql.AppendLine("where aID=@aID ");

            MyParams.Add(new Params("@aID", "GUID", entity.AID));
            MyParams.Add(new Params("@aDesc", "NVARCHAR", entity.ADesc));
            MyParams.Add(new Params("@aDirectory", "NVARCHAR", entity.ADirectory));
            MyParams.Add(new Params("@aDuration", "INT", entity.ADuration));
            MyParams.Add(new Params("@aIsBackgroundImg", "BIT", entity.AIsBackgroundIMG));
            MyParams.Add(new Params("@aUpdatedDate", "DATETIME", entity.UpdatedDate));
            MyParams.Add(new Params("@aUpdatedBy", "GUID", entity.UpdatedBy));
            MyParams.Add(new Params("@aRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@aStatus", "INT", 1));

            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool updateAdvertPackage(MAdvertisement entity)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("delete from ADVERTISEMENT_PACKAGE ");
            sql.AppendLine("where ref_AdvId = @aID ");
            MyParams.Add(new Params("@aID", "GUID", entity.AID));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
            {
                sql.Clear();
                MyParams.Clear();
                string fileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", entity.ADirectory);
                int totalIndicator = GetTotalIndicator(fileName);

                sql.AppendLine("update Machine_Advertisement set ");
                sql.AppendLine("aTotal=@aTotal, aStatus=@aStatus, aUpdatedDate=@aUpdatedDate, aUpdatedBy=@aUpdatedBy");
                sql.AppendLine("where aID=@aID ");
                MyParams.Add(new Params("@aID", "GUID", entity.AID));
                MyParams.Add(new Params("@aTotal", "INT", totalIndicator));
                MyParams.Add(new Params("@aStatus", "INT", 1));
                MyParams.Add(new Params("@aUpdatedDate", "DATETIME", entity.UpdatedDate));
                MyParams.Add(new Params("@aUpdatedBy", "GUID", entity.UpdatedBy));

                if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                {
                    byte[] MyByte = System.IO.File.ReadAllBytes(fileName);
                    string bytes = Convert.ToBase64String(MyByte);
                    int onepart = 45000;
                    int indicator = bytes.Length / onepart;
                    int startindex = 0;
                    int endindex = onepart;

                    sql.Clear();
                    MyParams.Clear();
                    sql.AppendLine("insert into ADVERTISEMENT_PACKAGE ");
                    sql.AppendLine("(ref_AdvId , agIndicator , agData) ");
                    sql.AppendLine("values (@ref_AdvId, @agIndicator, @agData)");

                    string data = string.Empty;

                    for (int x = 0; x <= indicator; x++)
                    {
                        if (x == indicator)
                        {
                            data = bytes.Substring(startindex, bytes.Length - startindex);
                        }
                        else
                        {
                            data = bytes.Substring(startindex, onepart);
                        }
                        startindex = (onepart * (x + 1));
                        MyParams.Add(new Params("@ref_AdvId", "GUID", entity.AID));
                        MyParams.Add(new Params("@agIndicator", "INT", x));
                        //MyParams.Add(new Params("@agData", "NCLOB", data));
                        MyParams.Add(new Params("@agData", "NVARCHAR", data));
                        ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
                        MyParams.Clear();
                    }
                }
            }


        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool approveAdvert(Guid advertID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select aID, ARELATIVEPATHURL as aDirectory from Machine_Advertisement ");
            sql.AppendLine("where aID = @aID ");
            MyParams.Add(new Params("@aID", "GUID", advertID));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

            if (dt.Rows.Count > 0)
            {
                sql.Clear();
                MyParams.Clear();
                string fileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", dt.Rows[0]["aDirectory"].ToString());
                int totalIndicator = GetTotalIndicator(fileName);

                sql.AppendLine("update Machine_Advertisement set ");
                sql.AppendLine("aTotal=@aTotal, aStatus=@aStatus, aApprovedDate=@aApprovedDate, aApprovedBy=@aApprovedBy");
                sql.AppendLine("where aID=@aID ");
                MyParams.Add(new Params("@aID", "GUID", advertID));
                MyParams.Add(new Params("@aTotal", "INT", totalIndicator));
                MyParams.Add(new Params("@aStatus", "INT", 1));
                MyParams.Add(new Params("@aApprovedDate", "DATETIME", DateTime.Now));
                MyParams.Add(new Params("@aApprovedBy", "GUID", adminGuid));

                if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                {
                    sql.Clear();
                    MyParams.Clear();
                    sql.AppendLine("delete from ADVERTISEMENT_PACKAGE ");
                    sql.AppendLine("where ref_AdvId = @ref_AdvId");
                    MyParams.Add(new Params("@ref_AdvId", "GUID", advertID));

                    if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                    {
                        byte[] MyByte = System.IO.File.ReadAllBytes(fileName);
                        string bytes = Convert.ToBase64String(MyByte);
                        int onepart = 45000;
                        int indicator = bytes.Length / onepart;
                        int startindex = 0;
                        int endindex = onepart;

                        sql.Clear();
                        MyParams.Clear();
                        sql.AppendLine("insert into ADVERTISEMENT_PACKAGE ");
                        sql.AppendLine("(ref_AdvId , agIndicator , agData) ");
                        sql.AppendLine("values (@ref_AdvId, @agIndicator, @agData)");

                        string data = string.Empty;

                        for (int x = 0; x <= indicator; x++)
                        {
                            if (x == indicator)
                            {
                                data = bytes.Substring(startindex, bytes.Length - startindex);
                            }
                            else
                            {
                                data = bytes.Substring(startindex, onepart);
                            }
                            startindex = (onepart * (x + 1));
                            MyParams.Add(new Params("@ref_AdvId", "GUID", advertID));
                            MyParams.Add(new Params("@agIndicator", "INT", x));
                            //MyParams.Add(new Params("@agData", "NCLOB", data));
                            MyParams.Add(new Params("@agData", "NVARCHAR", data));
                            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
                            MyParams.Clear();
                        }
                    }
                }
            }


        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool declineAdvert(Guid advertID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update Machine_Advertisement set ");
            sql.AppendLine("aStatus=@aStatus, aDeclinedDate=@aDeclinedDate, aDeclineBy=@aDeclineBy");
            sql.AppendLine("where aID=@aID ");

            MyParams.Add(new Params("@aID", "GUID", advertID));
            MyParams.Add(new Params("@aStatus", "INT", 2));
            MyParams.Add(new Params("@aDeclinedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@aDeclineBy", "GUID", adminGuid));

            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool deleteAdvert(Guid advertID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("delete Machine_Advertisement ");
            sql.AppendLine("where aID=@aID ");

            MyParams.Add(new Params("@aID", "GUID", advertID));

            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                System.Reflection.MethodBase.GetCurrentMethod().Name,
                ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    private int GetTotalIndicator(string filepath)
    {
        int ret = 1;
        try
        {
            byte[] mybyte = null;
            //change logic just incase the file is being read by another process
            while (true)
            {
                try
                {
                    mybyte = System.IO.File.ReadAllBytes(filepath);
                    break;
                }
                catch
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
            string tmp = Convert.ToBase64String(mybyte);
            ret = tmp.Length / 45000;
        }
        catch (Exception ex)
        {
            Logger.LogToFile("GetTotalIndicator.log", ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }
}