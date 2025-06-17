using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

public class HopperMaintenanceController : GlobalController
{

    public bool IsHopperExist(string hopperName)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("select * from Machine_Hopper where hDesc = @hName ");
            MyParams.Add(new Params("@hName", "NVARCHAR", hopperName));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count != 0)
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
            sql.AppendLine("join machine_hopper mh on (mg.kHopperID = mh.hid) ");
            sql.AppendLine("where mh.hid = @id and mg.kStatus = @status ");

            MyParams.Add(new Params("@id", "GUID", id));
            MyParams.Add(new Params("@status", "INT", 1));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            foreach (DataRow dr in dt.Rows)
                ret += "The Hopper is assigned to Machine Group, " + dr["kDesc"].ToString() + "! The Hopper Cannot be Inactive! <br />";
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

    public DataTable getHopperList()
    {
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine(@"select hSelect.*,cUL.uName CreatedBy,aUL.uName aName,dUL.uName dName, uul.uname UpdatedBy  
from(
select hID, hDesc, hRemarks, h1Temp, H1.cDesc HC1, h1Range, h1Mask, h2Temp, H2.cDesc HC2, h2Range, h2Mask
, h3Temp, H3.cDesc HC3, h3Range, h3Mask, h4Temp, H4.cDesc HC4, h4Range, h4Mask, h5Temp, H5.cDesc HC5, h5Range, h5Mask, h6Temp, H6.cDesc HC6, h6Range, h6Mask,
h7Temp, H7.cDesc HC7, h7Range, h7Mask, h8Temp, H8.cDesc HC8, h8Range, h8Mask, hCreatedDate, hApprovedDate, hDeclineDate, hupdateddate, hupdatedby,
hCreatedBy, hApprvoedBy, hDeclineBy,case hStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end HStatus
from Machine_Hopper MH
LEFT JOIN Hopper_Card H1 ON MH.h1Temp = H1.cID
LEFT JOIN Hopper_Card H2 ON MH.h2Temp = H2.cID
LEFT JOIN Hopper_Card H3 ON MH.h3Temp = H3.cID
LEFT JOIN Hopper_Card H4 ON MH.h4Temp = H4.cID
LEFT JOIN Hopper_Card H5 ON MH.h5Temp = H5.cID
LEFT JOIN Hopper_Card H6 ON MH.h6Temp = H6.cID
LEFT JOIN Hopper_Card H7 ON MH.h7Temp = H7.cID
LEFT JOIN Hopper_Card H8 ON MH.h8Temp = H8.cID
) hSelect
left join User_Login cUL on(hSelect.hCreatedBy = cUL.aID)
left join User_Login aUL on(hSelect.hApprvoedBy = aUL.aID)
left join User_Login dUL on(hSelect.hDeclineBy = dUL.aID)
left join User_Login uUL on(hSelect.hupdatedby = uul.aID)");
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

    public DataTable getCardList()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select cID, cDesc, cContactless from Hopper_Card where cStatus=@stat");
            MyParams.Add(new Params("@stat", "INT", 1));

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
        return dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    }

    public bool assignHopper(MHopper entity)
    {
        bool ret = false;
        Guid newGuid;
        try
        {
            if (!IsHopperExist(entity.HName))
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("insert into Machine_Hopper ");
                sql.AppendLine("(hID, hDesc, ");
                for (int i = 1; i <= 8; i++)
                {
                    if (Guid.TryParse(entity.HopperArray[i - 1, 0], out newGuid))
                    {
                        sql.AppendLine("h" + i + "temp, h" + i + "Range, h" + i + "Mask, ");
                    }
                }
                sql.AppendLine("hCreatedDate, hCreatedBy, hRemarks, hStatus) ");
                sql.AppendLine("values (@hid, @hdesc, ");
                for (int i = 0; i < 8; i++)
                {
                    if (Guid.TryParse(entity.HopperArray[i, 0], out newGuid))
                    {
                        sql.AppendLine("@temp" + i + ", @Range" + i + ", @Mask" + i + ", ");
                    }
                }
                sql.AppendLine("@hcDate, @hcBy, @hRemarks, @hstatus) ");

                MyParams.Add(new Params("@hid", "GUID", entity.HID));
                MyParams.Add(new Params("@hdesc", "NVARCHAR", entity.HName));

                for (int i = 0; i < 8; i++)
                {
                    if (Guid.TryParse(entity.HopperArray[i, 0], out newGuid))
                    {
                        MyParams.Add(new Params("@temp" + i, "GUID", Guid.Parse(entity.HopperArray[i, 0])));
                        MyParams.Add(new Params("@Range" + i, "NVARCHAR", entity.HopperArray[i, 1]));
                        MyParams.Add(new Params("@Mask" + i, "NVARCHAR", entity.HopperArray[i, 2]));
                    }
                }
                MyParams.Add(new Params("@hcDate", "DATETIME", entity.CreatedDate));
                MyParams.Add(new Params("@hcBy", "GUID", entity.CreatedBy));
                MyParams.Add(new Params("@hRemarks", "NVARCHAR", entity.Remarks));
                MyParams.Add(new Params("@hstatus", "INT", 0));//entity.Status));

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

    public bool updateHopper(MHopper entity)
    {
        bool ret = false; Guid newGuid;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update Machine_Hopper set ");

            if (!IsHopperExist(entity.HName))
            {
                sql.AppendLine("hDesc = @hdesc, ");
                MyParams.Add(new Params("@hdesc", "NVARCHAR", entity.HName));
            }
            for (int i = 1; i <= 8; i++)
            {
                if (Guid.TryParse(entity.HopperArray[i - 1, 0], out newGuid))
                {
                    sql.AppendLine("h" + i + "temp = @temp" + i + ",  ");
                    sql.AppendLine("h" + i + "Range = @Range" + i + ", ");
                    //if (i != 8)
                    //{
                    sql.AppendLine("h" + i + "Mask =  @Mask" + i + ", ");
                    //}
                    //else
                    //{
                    //    sql.AppendLine("h" + i + "Mask =  @Mask" + i + " ");
                    //}
                }
            }
            sql.AppendLine("hRemarks = @hRemarks, hStatus = @hStatus, hUpdatedBy = @hUpdatedBy, hUpdatedDate = @hUpdatedDate ");
            sql.AppendLine(" where hID = @hid ");

            for (int i = 0; i < 8; i++)
            {
                if (Guid.TryParse(entity.HopperArray[i, 0], out newGuid))
                {
                    MyParams.Add(new Params("@temp" + (i + 1), "GUID", Guid.Parse(entity.HopperArray[i, 0])));
                    MyParams.Add(new Params("@Range" + (i + 1), "NVARCHAR", entity.HopperArray[i, 1]));
                    MyParams.Add(new Params("@Mask" + (i + 1), "NVARCHAR", entity.HopperArray[i, 2]));
                }
            }
            MyParams.Add(new Params("@hUpdatedDate", "DATETIME", entity.UpdatedDate));
            MyParams.Add(new Params("@hUpdatedBy", "GUID", entity.UpdatedBy));
            MyParams.Add(new Params("@hRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@hStatus", "INT", entity.Status));
            MyParams.Add(new Params("@hid", "GUID", entity.HID));

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

    public DataTable[] getDetailByHopperID(Guid hopperID)
    {
        DataTable[] dtArray = new DataTable[2];
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine(@"select hSelect.*,cUL.uName cName,aUL.uName aName,dUL.uName dName, uul.uname  
                                from(
                                select hID, hDesc, hRemarks, h1Temp, H1.cDesc HC1, h1Range, h1Mask, h2Temp, H2.cDesc HC2, h2Range, h2Mask
                                , h3Temp, H3.cDesc HC3, h3Range, h3Mask, h4Temp, H4.cDesc HC4, h4Range, h4Mask, h5Temp, H5.cDesc HC5, h5Range, h5Mask, h6Temp, H6.cDesc HC6, h6Range, h6Mask,
                                h7Temp, H7.cDesc HC7, h7Range, h7Mask, h8Temp, H8.cDesc HC8, h8Range, h8Mask, hCreatedDate, hApprovedDate, hDeclineDate, hupdateddate, hupdatedby,
                                hCreatedBy, hApprvoedBy, hDeclineBy,hStatus HStatID,case hStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end HStatus
                                from Machine_Hopper MH
                                LEFT JOIN Hopper_Card H1 ON MH.h1Temp = H1.cID
                                LEFT JOIN Hopper_Card H2 ON MH.h2Temp = H2.cID
                                LEFT JOIN Hopper_Card H3 ON MH.h3Temp = H3.cID
                                LEFT JOIN Hopper_Card H4 ON MH.h4Temp = H4.cID
                                LEFT JOIN Hopper_Card H5 ON MH.h5Temp = H5.cID
                                LEFT JOIN Hopper_Card H6 ON MH.h6Temp = H6.cID
                                LEFT JOIN Hopper_Card H7 ON MH.h7Temp = H7.cID
                                LEFT JOIN Hopper_Card H8 ON MH.h8Temp = H8.cID
                                WHERE MH.hid = @hid) hSelect
                                left join User_Login cUL on(hSelect.hCreatedBy = cUL.aID)
                                left join User_Login aUL on(hSelect.hApprvoedBy = aUL.aID)
                                left join User_Login dUL on(hSelect.hDeclineBy = dUL.aID)
                                left join User_Login uUL on(hSelect.hupdatedby = uul.aID)");
            MyParams.Add(new Params("@hid", "GUID", hopperID));

            DataTable editTB = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

            dtArray[0] = editTB;
            dtArray[1] = restructureTable(editTB);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return dtArray;
    }

    public DataTable restructureTable(DataTable dt)
    {
        DataTable detailTB = new DataTable("DetailTable");
        try
        {
            detailTB.Columns.Add("hID", typeof(Guid));
            detailTB.Columns.Add("Hopper", typeof(string));
            detailTB.Columns.Add("HopperCardID", typeof(Guid));
            detailTB.Columns.Add("HopperCard", typeof(string));
            detailTB.Columns.Add("HopperRange", typeof(string));
            detailTB.Columns.Add("HopperMask", typeof(string));

            DataRow[] detailRow = dt.Select();
            foreach (DataRow row in detailRow)
            {
                for (int i = 1; i <= 8; i++)
                {
                    detailTB.Rows.Add(row["hID"], "Hopper " + i, row["h" + i + "Temp"], row["HC" + i], row["h" + i + "Range"], row["h" + i + "Mask"]);
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
        return detailTB;
    }

    public bool approveHopper(Guid hopID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update Machine_Hopper set ");
            sql.AppendLine("hStatus=@hStatus, hApprovedDate=@hApprovedDate, hApprvoedBy=@hApprovedBy");
            sql.AppendLine("where hID=@hID ");

            MyParams.Add(new Params("@hID", "GUID", hopID));
            MyParams.Add(new Params("@hStatus", "INT", 1));
            MyParams.Add(new Params("@hApprovedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@hApprovedBy", "GUID", adminGuid));

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

    public bool declineHopper(Guid hopID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update Machine_Hopper set ");
            sql.AppendLine("hStatus=@hStatus, hDeclineDate=@hDeclineDate, hDeclineBy=@hDeclineBy");
            sql.AppendLine("where hID=@hID ");

            MyParams.Add(new Params("@hID", "GUID", hopID));
            MyParams.Add(new Params("@hStatus", "INT", 2));
            MyParams.Add(new Params("@hDeclineDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@hDeclineBy", "GUID", adminGuid));

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

    public bool deleteHopper(Guid hopID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("delete Machine_Hopper ");
            sql.AppendLine("where hID=@hID ");

            MyParams.Add(new Params("@hID", "GUID", hopID));

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