using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

public class CardMaintenanceControl : GlobalController
{
    public bool IsCardNameExist(string cardName)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("select * from Hopper_Card HC ");
            sql.AppendLine("where HC.cDesc = @cardName ");
            MyParams.Add(new Params("@cardName", "NVARCHAR", cardName));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count != 0)
            {
                ret = true;
            }
            else
            {
                ret = false;
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


    public bool isCardAvailable(string bin)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();

            sql.AppendLine("select * from hopper_card where cbin=@username");
            MyParams.Add(new Params("@username", "NVARCHAR", bin));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count != 0)
            {
                ret = true;
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UserRoleController_isUserAvailable.log", ex);

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
            sql.AppendLine("select * from machine_hopper mh ");
            for (int i = 1; i <= 8; i++)
                sql.AppendLine("join hopper_card h" + i + " on (mh.h" + i + "temp = h" + i + ".cid) ");
            for (int i = 1; i <= 8; i++)
            {
                if (i == 1)
                    sql.Append("where ");
                else
                    sql.Append("or ");
                sql.AppendLine(" (h" + i + ".cid = @cid) ");
            }
            sql.AppendLine("and mh.hStatus = @status ");
            MyParams.Add(new Params("@cid", "GUID", id));
            MyParams.Add(new Params("@status", "INT", 1));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            foreach (DataRow dr in dt.Rows)
                ret += "The Card is assigned to Machine Hopper, " + dr["hDesc"].ToString() + "! The Card Cannot be Inactive! <br />";
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

    public DataTable getCardList()
    {
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select HC.*, ");
            sql.AppendLine("Case HC.cContactless when 0 then 'Dual Mode' when 1 then 'Contact Only' end txtContactless, ");
            sql.AppendLine("Case HC.cStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end txtStatus, ");
            sql.AppendLine("cUL.uName CreatedName, aUL.uName ApprovedName, dUL.uName DeclineName, uul.uname UpdatedName from Hopper_Card HC ");
            sql.AppendLine("left join User_Login cUL on (HC.cCreatedBy = cUL.aID) ");
            sql.AppendLine("left join User_Login aUL on (HC.cApprovedBy = aUL.aID) ");
            sql.AppendLine("left join User_Login dUL on (HC.cDeclineBy = dUL.aID) ");
            sql.AppendLine("left join User_Login uUL on (HC.cUpdatedBy = uul.aID) ");
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }

    public DataTable getCardListCM(Guid approver)
    {
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select HC.*, ");
            sql.AppendLine("Case HC.cContactless when 0 then 'Dual Mode' when 1 then 'Contact Only' end txtContactless, ");
            sql.AppendLine("Case HC.cStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end txtStatus, ");
            sql.AppendLine("cUL.uName CreatedName, aUL.uName ApprovedName, dUL.uName DeclineName, uul.uname UpdatedName from Hopper_Card HC ");
            sql.AppendLine("left join User_Login cUL on (HC.cCreatedBy = cUL.aID) ");
            sql.AppendLine("left join User_Login aUL on (HC.cApprovedBy = aUL.aID) ");
            sql.AppendLine("left join User_Login dUL on (HC.cDeclineBy = dUL.aID) ");
            sql.AppendLine("left join User_Login uUL on (HC.cUpdatedBy = uul.aID) ");
            sql.AppendLine("where HC.cStatus = 0 or (HC.cStatus = 1 and HC.cCreatedBy != @approver) ");
            MyParams.Add(new Params("@approver", "GUID", approver));

        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }

    public bool insertNewCard(MCard entity)
    {
        bool ret = false;
        try
        {
            if (!IsCardNameExist(entity.CDesc))
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("insert into Hopper_Card (cID, cDesc, cType, cContactless, cBin, cMask, cCreatedDate, cCreatedBy, cRemarks, cStatus) ");
                sql.AppendLine("values (@cID, @cDesc, @cType, @cContactless, @cBin, @cMask, @cCreatedDate, @cCreatedBy, @cRemarks, @cStatus) ");
                MyParams.Add(new Params("@cID", "GUID", entity.CID));
                MyParams.Add(new Params("@cDesc", "NVARCHAR", entity.CDesc));
                MyParams.Add(new Params("@cType", "NVARCHAR", entity.CType));
                MyParams.Add(new Params("@cContactless", "BIT", entity.cContactless));
                MyParams.Add(new Params("@cBin", "NVARCHAR", entity.CBin));
                MyParams.Add(new Params("@cMask", "NVARCHAR", entity.CMask));
                MyParams.Add(new Params("@cCreatedDate", "DATETIME", entity.CreatedDate));
                MyParams.Add(new Params("@cCreatedBy", "GUID", entity.CreatedBy));
                MyParams.Add(new Params("@cRemarks", "NVARCHAR", entity.Remarks));
                MyParams.Add(new Params("@cStatus", "INT", 0));//entity.Status));
                ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
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

    public DataTable getCardInfoByID(Guid cardID)
    {
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select hc.*, case cContactless when 0 then 'Contactless' when 1 then 'Contact Only' end Contact from Hopper_Card hc where cID=@cid ");
            MyParams.Add(new Params("@cid", "GUID", cardID));
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }

    public bool updateCard(MCard entity)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.Append("update Hopper_Card set ");
            if (!IsCardNameExist(entity.CDesc))
            {
                sql.Append("cDesc=@cdesc, ");
                MyParams.Add(new Params("@cdesc", "NVARCHAR", entity.CDesc));
            }
            sql.Append("cContactless=@cContactless, cType=@cType, cBin = @cBin, cMask=@cMask, ");
            sql.Append("cRemarks=@cRemarks, cStatus=@cStatus, cUpdatedBy=@cUpdatedBy, cUpdatedDate=@cUpdatedDate ");
            sql.AppendLine("where cID=@id ");
            MyParams.Add(new Params("@cContactless", "BIT", entity.cContactless));
            MyParams.Add(new Params("@cType", "NVARCHAR", entity.CType));
            MyParams.Add(new Params("@cBin", "NVARCHAR", entity.CBin));
            MyParams.Add(new Params("@cMask", "NVARCHAR", entity.CMask));
            MyParams.Add(new Params("@cUpdatedDate", "DATETIME", entity.UpdatedDate));
            MyParams.Add(new Params("@cUpdatedBy", "GUID", entity.UpdatedBy));
            MyParams.Add(new Params("@cRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@cStatus", "INT", entity.Status));
            MyParams.Add(new Params("@id", "GUID", entity.CID));

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

    public bool approveCard(Guid cardID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update Hopper_Card set ");
            sql.AppendLine("cStatus=@cStatus, cApprovedDate=@cApprovedDate, cApprovedBy=@cApprovedBy");
            sql.AppendLine("where cID=@cID ");

            MyParams.Add(new Params("@cID", "GUID", cardID));
            MyParams.Add(new Params("@cStatus", "INT", 1));
            MyParams.Add(new Params("@cApprovedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@cApprovedBy", "GUID", adminGuid));

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

    public bool declineCard(Guid cardID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update Hopper_Card set ");
            sql.AppendLine("cStatus=@cStatus, cDeclineDate=@cDeclineDate, cDeclineBy=@cDeclineBy");
            sql.AppendLine("where cID=@cID ");

            MyParams.Add(new Params("@cID", "GUID", cardID));
            MyParams.Add(new Params("@cStatus", "INT", 2));
            MyParams.Add(new Params("@cDeclineDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@cDeclineBy", "GUID", adminGuid));

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

    public bool deleteCard(Guid cardID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("delete Hopper_Card ");
            sql.AppendLine("where cID=@cID ");

            MyParams.Add(new Params("@cID", "GUID", cardID));

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
}
