using System.Data;
using System.Text;
using Alexis.Dashboard;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;
using Dashboard.Infra.EF.Models.Ad;

public class GlobalController
{
    protected StringBuilder sql = new StringBuilder();
    protected List<Params> MyParams = new List<Params>();

    #region DataEditing
    public bool EditData(Object entity, string ClientIp)
    {
        bool ret = false;

        UserRoleController urController = new UserRoleController();
        AlertMaintenanceController alertController = new AlertMaintenanceController();
        AdvertisementController advertController = new AdvertisementController();
        CardMaintenanceControl cardController = new CardMaintenanceControl();
        DocumentSettingController documentController = new DocumentSettingController();
        HopperMaintenanceController hopperController = new HopperMaintenanceController();
        WrapServController wsController = new WrapServController();
        MasterSettingController msController = new MasterSettingController();
        try
        {
            Guid idOriginal = Guid.Empty;
            Guid mid = Guid.Empty;
            string tblName = "";
            string oldValues = "";
            string newValues = "";
            Guid editedBy = Guid.Empty;
            switch (entity.ToString().Split('.').Last())
            {
                case "Role":
                    Role role = (Role)entity;
                    idOriginal = role.RoleID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_RoleMaintenance);
                    tblName = "User_Roles";
                    editedBy = role.UpdatedBy;
                    oldValues = DBToString(urController.GetModulePermissionByRoleId(role.RoleID).DefaultView.ToTable(false, "RDESC", "RREMARKS", "RSTATUS", "MID", "CHILD", "MVIEW", "MMAKER", "MCHECKER"));
                    newValues = ProcessToString(entity);
                    break;
                case "User":
                    User user = (User)entity;
                    idOriginal = user.UserID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_UserMaintenance);
                    tblName = "User_Login";
                    editedBy = user.UpdatedBy;
                    oldValues = DBToString(new DataView(urController.GetUpdateUser(user.UserID)).ToTable(false, "UNAME", "UFULLNAME", "UCMID", "RID", "RDESC", "AGENTFLAG", "MCARDREPLENISHMENT", "MCHEQUEREPLENISHMENT", "MCONSUMABLE", "MSECURITY", "MTROUBLESHOOT", "LOCALUSER", "LPASSWORD", "LEMAIL", "UREMARKS", "USTATUS"));
                    newValues = ProcessToString(entity);
                    break;
                case "Branch":
                    Branch branch = (Branch)entity;
                    idOriginal = branch.Bid;
                    mid = Module.GetModuleId(ModuleLogAction.Update_BranchMaintenance);
                    tblName = "User_Branch";
                    editedBy = branch.UpdatedBy;
                    oldValues = DBToString(BranchController.getBusinessOperatingById(branch.Bid).DefaultView.ToTable(false, "BDESC", "BMONDAY", "BTUESDAY", "BWEDNESDAY", "BTHURSDAY", "BFRIDAY", "BSATURDAY", "BSUNDAY", "BOPENTIME", "BCLOSETIME", "BREMARKS", "BSTATID"));
                    newValues = ProcessToString(entity);
                    break;
                case "MAlert":
                    MAlert alert = (MAlert)entity;
                    idOriginal = alert.AID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_Alert_Template);
                    tblName = "Machine_Alert";
                    editedBy = alert.UpdatedBy;
                    oldValues = DBToString(new DataView(alertController.getAlertByID(idOriginal)).ToTable(false, "ADESC", "AMINCARDBAL", "AMINCHEQUEBAL", "AMINPAPERBAL", "AMINREJCARDBAL", "AMINRIBFRONTBAL", "AMINRIBREARBAL", "AMINRIBTIPBAL", "AMINCHEQUEPRINTBAL", "AMINCHEQUEPRINTCATRIDGE", "AMINCATRIDGEBAL", "ACARDEMAIL", "ACARDSMS", "ACARDTINTERVAL", "ACHEQUEEMAIL", "ACHEQUESMS", "ACHEQUETINTERVAL", "AMAINTENANCEEMAIL", "AMAINTENANCESMS", "AMAINTENANCETINTERVAL", "ASECURITYEMAIL", "ASECURITYSMS", "ASECURITYTINTERVAL", "ATROUBLESHOOTEMAIL", "ATROUBLESHOOTSMS", "ATROUBLESHOOTTINTERVAL", "AREMARKS", "ASTATUS"));
                    newValues = ProcessToString(entity);
                    break;
                case "MGroup":
                    MGroup group = (MGroup)entity;
                    idOriginal = group.KID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_Group_Template);
                    tblName = "Machine_Group";
                    editedBy = group.UpdatedBy;

                    DataTable dt = GroupMaintenanceController.getAssignedGroupsByID(idOriginal);
                    DataTable dtGroup = dt.DefaultView.ToTable(true, "DESCRIPTION", "KSCREENBACKGROUND", "BACKGROUNDIMAGE", "ADIRECTORY", "ALERTID", "ALERT", "BID", "BUSINESSHOUR", "DID", "DOCUMENT", "HID", "HOPPER", "KREMARKS", "KSTATUS");
                    DataTable dtAdvert = dt.DefaultView.ToTable(false, "ADVERTID", "ADVERTISEMENT", "SEQORDER");
                    dtAdvert.DefaultView.Sort = "seqOrder asc";
                    dtAdvert = dtAdvert.DefaultView.ToTable(false, "ADVERTID");
                    DataTable dtAll = new DataTable("dbTable");
                    dtAll.Columns.Add("DESCRIPTION", typeof(string));
                    dtAll.Columns.Add("KSCREENBACKGROUND", typeof(string));
                    dtAll.Columns.Add("BACKGROUNDIMAGE", typeof(string));
                    dtAll.Columns.Add("ADIRECTORY", typeof(string));
                    dtAll.Columns.Add("ALERTID", typeof(string));
                    dtAll.Columns.Add("ALERT", typeof(string));
                    dtAll.Columns.Add("BID", typeof(string));
                    dtAll.Columns.Add("BUSINESSHOUR", typeof(string));
                    dtAll.Columns.Add("DID", typeof(string));
                    dtAll.Columns.Add("DOCUMENT", typeof(string));
                    dtAll.Columns.Add("HID", typeof(string));
                    dtAll.Columns.Add("HOPPER", typeof(string));
                    dtAll.Columns.Add("KREMARKS", typeof(string));
                    dtAll.Columns.Add("KSTATUS", typeof(string));
                    dtAll.Columns.Add("ADVERT", typeof(string));
                    dtAll.Rows.Add(dtGroup.Rows[0]["DESCRIPTION"].ToString(),
                        dtGroup.Rows[0]["KSCREENBACKGROUND"].ToString(),
                        dtGroup.Rows[0]["BACKGROUNDIMAGE"].ToString(),
                        dtGroup.Rows[0]["ADIRECTORY"].ToString(),
                        dtGroup.Rows[0]["ALERTID"].ToString(),
                        dtGroup.Rows[0]["ALERT"].ToString(),
                        dtGroup.Rows[0]["BID"].ToString(),
                        dtGroup.Rows[0]["BUSINESSHOUR"].ToString(),
                        dtGroup.Rows[0]["DID"].ToString(),
                        dtGroup.Rows[0]["DOCUMENT"].ToString(),
                        dtGroup.Rows[0]["HID"].ToString(),
                        dtGroup.Rows[0]["HOPPER"].ToString(),
                        dtGroup.Rows[0]["KREMARKS"].ToString(),
                        dtGroup.Rows[0]["KSTATUS"].ToString(),
                        "\r\n#ADVERTISEMENT#\r\n" + DBToString(dtAdvert).Replace("#UPDATE#", "") + "#ADVERTISEMENT#"
                        );
                    oldValues = DBToString(dtAll);
                    newValues = ProcessToString(entity);
                    break;
                case "MAdvertisement":
                    MAdvertisement advert = (MAdvertisement)entity;
                    idOriginal = advert.AID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_AdvertisementManagement);
                    tblName = "Machine_Advertisement";
                    editedBy = advert.UpdatedBy;
                    oldValues = DBToString(advertController.getAdvertisementById(idOriginal).DefaultView.ToTable(false, "ANAME", "ADESC", "ARELATIVEPATHURL", "ADURATION", "AREMARKS", "ASTATUS"));
                    newValues = ProcessToString(entity);
                    break;
                case "MCard":
                    MCard card = (MCard)entity;
                    idOriginal = card.CID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_Card_Maintenance);
                    tblName = "Hopper_Card";
                    editedBy = card.UpdatedBy;
                    oldValues = DBToString(cardController.getCardInfoByID(card.CID).DefaultView.ToTable(false, "CDESC", "CCONTACTLESS", "CTYPE", "CBIN", "CMASK", "CREMARKS", "CSTATUS"));
                    newValues = ProcessToString(entity);
                    break;
                case "MDocument":
                    MDocument document = (MDocument)entity;
                    editedBy = document.UpdatedBy;
                    if (document.ComponentID == "" || document.ComponentID == null)
                    {
                        idOriginal = document.DID;
                        mid = Module.GetModuleId(ModuleLogAction.Update_Document_Template);
                        tblName = "Machine_Document";
                        oldValues = DBToString(documentController.getDocSettingById(document.DID).DefaultView.ToTable(true, "DDESC", "DREMARKS", "DSTATUS"))
                            + //"#UPDATE#" + Environment.NewLine + 
                            DBToString(documentController.getDocSettingById(document.DID).DefaultView.ToTable(false, "DDESC", "DSTATUS", "DOCTYPEID", "DNAME", "SWALLOW", "PRINT"));
                        newValues = ProcessToString(entity);
                    }
                    else
                    {
                        idOriginal = document.DID;
                        mid = Module.GetModuleId(ModuleLogAction.Update_DocType_Template);
                        tblName = "DocType_Setup";
                        oldValues = DBToString(documentController.getDocTypeById(document.DID).DefaultView.ToTable(false, "DNAME", "CCOMPONENTID", "DREMARKS", "DSTATUS"));
                        newValues = ProcessToString(entity);
                    }
                    break;
                case "MHopper":
                    MHopper hopper = (MHopper)entity;
                    idOriginal = hopper.HID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_Hopper_Template);
                    tblName = "Machine_Hopper";
                    editedBy = hopper.UpdatedBy;
                    oldValues = DBToString(hopperController.getDetailByHopperID(hopper.HID)[0].DefaultView.ToTable(false, "HDESC", "HC1", "H1TEMP", "H1RANGE", "H1MASK", "HC2", "H2TEMP", "H2RANGE", "H2MASK", "HC3", "H3TEMP", "H3RANGE", "H3MASK", "HC4", "H4TEMP", "H4RANGE", "H4MASK", "HC5", "H5TEMP", "H5RANGE", "H5MASK", "HC6", "H6TEMP", "H6RANGE", "H6MASK", "HC7", "H7TEMP", "H7RANGE", "H7MASK", "HC8", "H8TEMP", "H8RANGE", "H8MASK", "HREMARKS", "HSTATID"));
                    newValues = ProcessToString(entity);
                    break;
                case "MBizHour":
                    MBizHour bizHour = (MBizHour)entity;
                    idOriginal = bizHour.Bid;
                    mid = Module.GetModuleId(ModuleLogAction.Update_BusinessHour_Template);
                    tblName = "Machine_BusinessHour";
                    editedBy = bizHour.UpdatedBy;
                    oldValues = DBToString(BusinessHourMaintenanceController.getBusinessOperatingById(bizHour.Bid).DefaultView.ToTable(false, "BDESC", "BMONDAY", "BTUESDAY", "BWEDNESDAY", "BTHURSDAY", "BFRIDAY", "BSATURDAY", "BSUNDAY", "BSTARTTIME", "BENDTIME", "BREMARKS", "BSTATID"));
                    newValues = ProcessToString(entity);
                    break;
                case "MKiosk":
                    MKiosk kiosk = (MKiosk)entity;
                    idOriginal = kiosk.MachineID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_Kiosk);
                    tblName = "Machine";
                    editedBy = kiosk.UpdatedBy;

                    DataTable dtMI = KioskCreateMaintenanceController.getMachineItemsById(kiosk.MachineID);
                    DataTable dtKiosk = new DataTable("dbTable");
                    dtKiosk.Columns.Add("DESCRIPTION", typeof(string));
                    dtKiosk.Columns.Add("SERIAL", typeof(string));
                    dtKiosk.Columns.Add("MKIOSKID", typeof(string));
                    dtKiosk.Columns.Add("MSTATIONID", typeof(string));
                    dtKiosk.Columns.Add("ADDRESS", typeof(string));
                    dtKiosk.Columns.Add("LATITUDE", typeof(string));
                    dtKiosk.Columns.Add("LONGITUDE", typeof(string));
                    dtKiosk.Columns.Add("MGROUPID", typeof(string));
                    dtKiosk.Columns.Add("GROUPDESC", typeof(string));
                    dtKiosk.Columns.Add("MREMARKS", typeof(string));
                    dtKiosk.Columns.Add("MSTATID", typeof(string));
                    dtKiosk.Columns.Add("IPADDRESS", typeof(string));
                    dtKiosk.Columns.Add("PORTNUMBER", typeof(string));
                    //dtKiosk.Columns.Add("MPILOT", typeof(string));
                    dtKiosk.Rows.Add(dtMI.Rows[0]["DESCRIPTION"].ToString(),
                        dtMI.Rows[0]["SERIAL"].ToString(),
                        dtMI.Rows[0]["MKIOSKID"].ToString(),
                        dtMI.Rows[0]["MSTATIONID"].ToString(),
                        "\r\n#ADDRESS#\r\n" + dtMI.Rows[0]["ADDRESS"].ToString() + "\t\r\n#ADDRESS#",
                        dtMI.Rows[0]["LATITUDE"].ToString(),
                        dtMI.Rows[0]["LONGITUDE"].ToString(),
                        dtMI.Rows[0]["MGROUPID"].ToString(),
                        dtMI.Rows[0]["GROUPDESC"].ToString(),
                        dtMI.Rows[0]["MREMARKS"].ToString(),
                        dtMI.Rows[0]["MSTATID"].ToString(),
                        dtMI.Rows[0]["IPADDRESS"].ToString(),
                        dtMI.Rows[0]["PORTNUMBER"].ToString()
                        //dtMI.Rows[0]["MPILOT"].ToString()
                        );

                    oldValues = DBToString(dtKiosk);
                    newValues = ProcessToString(entity);
                    break;
                case "AWrapServ":
                    AWrapServ wrapServ = (AWrapServ)entity;
                    idOriginal = wrapServ.WSID;
                    if (wrapServ.WSType == WrapServType.Serv)
                        mid = Module.GetModuleId(ModuleLogAction.Update_Service_Template);
                    else
                        mid = Module.GetModuleId(ModuleLogAction.Update_Wrap_Template);
                    tblName = "Wrap_Serv_Main";
                    editedBy = wrapServ.UpdatedBy;
                    if (wrapServ.Detail.WSID_Detail == Guid.Empty)
                    {
                        //idOriginal = wrapServ.WSID;
                        oldValues = DBToString(wsController.getWrapServByID(wrapServ.WSType, wrapServ.WSID).DefaultView.ToTable(true, "WSMAIN", "WSTYPE", "WSREMARKS", "WSSTATUS"));//, "WSID_DETAIL", "WSDETAIL"));
                    }
                    else
                    {
                        //idOriginal = wrapServ.Detail.WSID_Detail;
                        oldValues = DBToString(wsController.getWrapServByDetailID(wrapServ.WSType, wrapServ.Detail.WSID_Detail).DefaultView.ToTable(false, "WSMAIN", "WSTYPE", "WSID_DETAIL", "WSDETAIL"));
                    }
                    newValues = ProcessToString(entity);
                    break;
                case "MACHINE_ADVERTISEMENT":
                    MACHINE_ADVERTISEMENT advert1 = (MACHINE_ADVERTISEMENT)entity;
                    idOriginal = advert1.AID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_AdvertisementManagement);
                    tblName = "Machine_Advertisement";
                    editedBy = (String.IsNullOrEmpty(advert1.AUPDATEDBY)) ? Guid.Empty : Guid.Parse(advert1.AUPDATEDBY);
                    oldValues = DBToString(advertController.getAdvertisementById(idOriginal).DefaultView.ToTable(false, "ANAME", "ADESC", "ARELATIVEPATHURL", "ADURATION", "AREMARKS", "ASTATUS"));
                    newValues = ProcessToString(entity);
                    break;
                case "AMasterSettings":
                    AMasterSettings masterSetting = (AMasterSettings)entity;
                    idOriginal = Module.GetModuleId(ModuleLogAction.View_MasterSetting);
                    mid = Module.GetModuleId(ModuleLogAction.Update_MasterSetting);
                    tblName = "tblMasterSetting";
                    editedBy = masterSetting.UpdatedBy;
                    oldValues = DBToString(msController.GetDTMasterSettings().DefaultView.ToTable(false, "PWORD"));
                    newValues = ProcessToString(entity);
                    break;
            }
            //StoreToFile(oldValues, "dtDBToString");
            //StoreToFile(newValues, "dtProcessToString");

            string[] compare = CompareOldAndNew(oldValues, newValues);
            oldValues = compare[0];
            newValues = compare[1];

            //StoreToFile(compare[0], "OldString");
            //StoreToFile(compare[1], "NewString");

            if (newValues != "" && newValues != null)
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("insert into tblEditing");
                sql.AppendLine("(idOriginal,tblName,oldValues,newValues,editedDate,updateStatus,editedBy,makerIP,mID,ActionName)");
                sql.AppendLine("values");
                sql.AppendLine("(:idOriginal, :tblName, :oldValues, :newValues, :editedDate, :updateStatus, :editedBy, :makerIP, :mID, :actName)");
                MyParams.Add(new Params(":idOriginal", "GUID", idOriginal));
                MyParams.Add(new Params(":tblName", "NVARCHAR", tblName));
                MyParams.Add(new Params(":oldValues", "NCLOB", oldValues));
                MyParams.Add(new Params(":newValues", "NCLOB", newValues));
                MyParams.Add(new Params(":editedDate", "DATETIME", DateTime.Now));
                MyParams.Add(new Params(":updateStatus", "NVARCHAR", "EDITED"));
                MyParams.Add(new Params(":editedBy", "GUID", editedBy));
                MyParams.Add(new Params(":makerIP", "NVARCHAR", ClientIp));
                MyParams.Add(new Params(":mID", "GUID", mid));
                MyParams.Add(new Params(":actName", "NVARCHAR", "Update"));

                ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
            }
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    private string DBToString(DataTable dt)
    {
        string ret = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.ToLower().Contains("remarks"))
                    {
                        sb.AppendLine(dc.ColumnName + ": \r\n#REMARKS#\r\n" + dr[dc.ColumnName] + "\t\r\n#REMARKS#");
                    }
                    else
                    {
                        sb.AppendLine(dc.ColumnName + ": " + dr[dc.ColumnName] + "\t");
                    }
                }
                sb.AppendLine("#UPDATE#");
                sb.Append(Environment.NewLine);
            }
            ret = sb.ToString();
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    private string ProcessToString(Object entity)
    {
        string ret = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            switch (entity.ToString().Split('.').Last())
            {
                case "Role":
                    Role role = (Role)entity;
                    foreach (var gvrRow in (List<ModuleViewModel>)role.Items)
                    {
                        Guid mID = Guid.Parse(gvrRow.mID);
                        sb.AppendLine("RDESC" + ": " + role.RoleDesc + "\t");
                        sb.AppendLine("RREMARKS" + ": ");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine(role.Remarks + "\t");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine("RSTATUS" + ": " + Convert.ToInt32(role.Status) + "\t");
                        sb.AppendLine("MID" + ": " + mID + "\t");
                        sb.AppendLine("CHILD" + ": " + gvrRow.child + "\t");
                        sb.AppendLine("MVIEW" + ": " + Convert.ToInt32(gvrRow.mView) + "\t");
                        sb.AppendLine("MMAKER" + ": " + Convert.ToInt32(gvrRow.mMaker) + "\t");
                        sb.AppendLine("MCHECKER" + ": " + Convert.ToInt32(gvrRow.mChecker) + "\t");
                        sb.AppendLine("#UPDATE#");
                        sb.Append(Environment.NewLine);
                    }
                    break;
                case "User":
                    User user = (User)entity;
                    sb.AppendLine("UNAME" + ": " + user.UserName + "\t");
                    sb.AppendLine("UFULLNAME" + ": " + user.UserFullName + "\t");
                    sb.AppendLine("UCMID" + ": " + user.UserCMID + "\t");
                    sb.AppendLine("RID" + ": " + user.RoleID + "\t");
                    sb.AppendLine("RDESC" + ": " + user.RoleDesc + "\t");
                    sb.AppendLine("AGENTFLAG" + ": " + Convert.ToInt32(user.AgentFlag) + "\t");
                    sb.AppendLine("MCARDREPLENISHMENT" + ": " + Convert.ToInt32(user.CardReplenishment) + "\t");
                    sb.AppendLine("MCHEQUEREPLENISHMENT" + ": " + Convert.ToInt32(user.ChequeReplenishment) + "\t");
                    sb.AppendLine("MCONSUMABLE" + ": " + Convert.ToInt32(user.ConsumableCollection) + "\t");
                    sb.AppendLine("MSECURITY" + ": " + Convert.ToInt32(user.Security) + "\t");
                    sb.AppendLine("MTROUBLESHOOT" + ": " + Convert.ToInt32(user.Troubleshoot) + "\t");
                    sb.AppendLine("LOCALUSER" + ": " + Convert.ToInt32(user.LocalUser) + "\t");
                    sb.AppendLine("LPASSWORD" + ": " + user.Password + "\t");
                    sb.AppendLine("LEMAIL" + ": " + user.Email + "\t");
                    sb.AppendLine("UREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(user.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("USTATUS" + ": " + Convert.ToInt32(user.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "Branch":
                    Branch branch = (Branch)entity;
                    sb.AppendLine("BDESC" + ": " + branch.TemplateName + "\t");
                    sb.AppendLine("BMONDAY" + ": " + Convert.ToInt32(branch.Monday) + "\t");
                    sb.AppendLine("BTUESDAY" + ": " + Convert.ToInt32(branch.Tuesday) + "\t");
                    sb.AppendLine("BWEDNESDAY" + ": " + Convert.ToInt32(branch.Wednesday) + "\t");
                    sb.AppendLine("BTHURSDAY" + ": " + Convert.ToInt32(branch.Thursday) + "\t");
                    sb.AppendLine("BFRIDAY" + ": " + Convert.ToInt32(branch.Friday) + "\t");
                    sb.AppendLine("BSATURDAY" + ": " + Convert.ToInt32(branch.Saturday) + "\t");
                    sb.AppendLine("BSUNDAY" + ": " + Convert.ToInt32(branch.Sunday) + "\t");
                    sb.AppendLine("BOPENTIME" + ": " + branch.Starttime + "\t");
                    sb.AppendLine("BCLOSETIME" + ": " + branch.Endtime + "\t");
                    sb.AppendLine("BREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(branch.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("BSTATID" + ": " + Convert.ToInt32(branch.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MAlert":
                    MAlert alert = (MAlert)entity;
                    sb.AppendLine("ADESC" + ": " + alert.ADesc + "\t");
                    sb.AppendLine("AMINCARDBAL" + ": " + alert.AMinCardBal + "\t");
                    sb.AppendLine("AMINCHEQUEBAL" + ": " + alert.AMinChequeBal + "\t");
                    sb.AppendLine("AMINPAPERBAL" + ": " + alert.AMinPaperBal + "\t");
                    sb.AppendLine("AMINREJCARDBAL" + ": " + alert.AMinRejCardBal + "\t");
                    sb.AppendLine("AMINRIBFRONTBAL" + ": " + alert.ARibFrontBal + "\t");
                    sb.AppendLine("AMINRIBREARBAL" + ": " + alert.ARibRearBal + "\t");
                    sb.AppendLine("AMINRIBTIPBAL" + ": " + alert.ARibTipBal + "\t");
                    sb.AppendLine("AMINCHEQUEPRINTBAL" + ": " + alert.AChequePrintBal + "\t");
                    sb.AppendLine("AMINCHEQUEPRINTCATRIDGE" + ": " + alert.AChequePrintCatridge + "\t");
                    sb.AppendLine("AMINCATRIDGEBAL" + ": " + alert.ACatridgeBal + "\t");

                    sb.AppendLine("ACARDEMAIL" + ": " + alert.ACardEmail + "\t");
                    sb.AppendLine("ACARDSMS" + ": " + alert.ACardSMS + "\t");
                    sb.AppendLine("ACARDTINTERVAL" + ": " + alert.ACardTimeInterval + "\t");
                    sb.AppendLine("ACHEQUEEMAIL" + ": " + alert.AChequeEmail + "\t");
                    sb.AppendLine("ACHEQUESMS" + ": " + alert.AChequeSMS + "\t");
                    sb.AppendLine("ACHEQUETINTERVAL" + ": " + alert.AChequeTimeInterval + "\t");
                    sb.AppendLine("AMAINTENANCEEMAIL" + ": " + alert.AMaintenanceEmail + "\t");
                    sb.AppendLine("AMAINTENANCESMS" + ": " + alert.AMaintenanceSMS + "\t");
                    sb.AppendLine("AMAINTENANCETINTERVAL" + ": " + alert.AMaintenanceTimeInterval + "\t");
                    sb.AppendLine("ASECURITYEMAIL" + ": " + alert.ASecurityEmail + "\t");
                    sb.AppendLine("ASECURITYSMS" + ": " + alert.ASecuritySMS + "\t");
                    sb.AppendLine("ASECURITYTINTERVAL" + ": " + alert.ASecurityTimeInterval + "\t");
                    sb.AppendLine("ATROUBLESHOOTEMAIL" + ": " + alert.ATroubleShootEmail + "\t");
                    sb.AppendLine("ATROUBLESHOOTSMS" + ": " + alert.ATroubleShootSMS + "\t");
                    sb.AppendLine("ATROUBLESHOOTTINTERVAL" + ": " + alert.ATroubleShootTimeInterval + "\t");
                    sb.AppendLine("AREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(alert.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("ASTATUS" + ": " + Convert.ToInt32(alert.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MGroup":
                    MGroup group = (MGroup)entity;
                    sb.AppendLine("DESCRIPTION" + ": " + group.KDesc + "\t");
                    sb.AppendLine("KSCREENBACKGROUND" + ": " + group.KScreenBackground.AID + "\t");
                    sb.AppendLine("BACKGROUNDIMAGE" + ": " + group.KScreenBackground.AName + "\t");
                    sb.AppendLine("ADIRECTORY" + ": " + group.KScreenBackground.ADirectory + "\t");
                    sb.AppendLine("ALERTID" + ": " + group.KAlertID + "\t");
                    sb.AppendLine("ALERT" + ": " + group.KAlertDesc + "\t");
                    sb.AppendLine("BID" + ": " + group.KBusinessHourID + "\t");
                    sb.AppendLine("BUSINESSHOUR" + ": " + group.KBusinessHourDesc + "\t");
                    sb.AppendLine("DID" + ": " + group.KDocumentID + "\t");
                    sb.AppendLine("DOCUMENT" + ": " + group.KDocumentDesc + "\t");
                    sb.AppendLine("HID" + ": " + group.KHopperID + "\t");
                    sb.AppendLine("HOPPER" + ": " + group.KHopperDesc + "\t");
                    sb.AppendLine("KREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(group.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("KSTATUS" + ": " + Convert.ToInt32(group.Status) + "\t");
                    sb.AppendLine("ADVERT" + ": ");
                    sb.AppendLine("#ADVERTISEMENT#");
                    foreach (var listItem in group.AdvIds)
                    {
                        sb.AppendLine("ADVERTID" + ": " + listItem + "\t");
                        //sb.AppendLine("ADVERTISEMENT" + ": " + listItem + "\t");
                        sb.AppendLine(Environment.NewLine);
                    }
                    sb.AppendLine("#ADVERTISEMENT#");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MAdvertisement":
                    MAdvertisement advert = (MAdvertisement)entity;
                    sb.AppendLine("ANAME" + ": " + advert.AName + "\t");
                    sb.AppendLine("ADESC" + ": " + advert.ADesc + "\t");
                    sb.AppendLine("ARELATIVEPATHURL" + ": " + advert.ADirectory + "\t");
                    sb.AppendLine("ADURATION" + ": " + advert.ADuration + "\t");
                    sb.AppendLine("AREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(advert.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("ASTATUS" + ": " + Convert.ToInt32(advert.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MCard":
                    MCard card = (MCard)entity;
                    sb.AppendLine("CDESC" + ": " + card.CDesc + "\t");
                    sb.AppendLine("CCONTACTLESS" + ": " + Convert.ToInt32(card.cContactless) + "\t");
                    sb.AppendLine("CTYPE" + ": " + card.CType + "\t");
                    sb.AppendLine("CBIN" + ": " + card.CBin + "\t");
                    sb.AppendLine("CMASK" + ": " + card.CMask + "\t");
                    sb.AppendLine("CREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(card.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("CSTATUS" + ": " + Convert.ToInt32(card.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MDocument":
                    MDocument document = (MDocument)entity;
                    if (document.ComponentID == "" || document.ComponentID == null)
                    {
                        sb.AppendLine("DDESC" + ": " + document.DName + "\t");
                        sb.AppendLine("DREMARKS" + ": ");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine(document.Remarks + "\t");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine("DSTATUS" + ": " + Convert.ToInt32(document.Status) + "\t");
                        sb.AppendLine("#UPDATE#" + Environment.NewLine);

                        for (int i = 1; i < document.DocComponent.Count; i++)
                        {
                            sb.AppendLine("DDESC" + ": " + document.DName + "\t");
                            sb.AppendLine("DSTATUS" + ": " + Convert.ToInt32(document.Status) + "\t");
                            sb.AppendLine("DOCTYPEID" + ": " + document.DocComponent[i++] + "\t");
                            sb.AppendLine("DNAME" + ": " + document.DocComponent[i++] + "\t");
                            sb.AppendLine("SWALLOW" + ": " + Convert.ToInt32(Convert.ToBoolean(document.DocComponent[i++])) + "\t");
                            sb.AppendLine("PRINT" + ": " + Convert.ToInt32(Convert.ToBoolean(document.DocComponent[i])) + "\t");
                            sb.AppendLine("#UPDATE#" + Environment.NewLine);
                        }
                    }
                    else
                    {
                        sb.AppendLine("DNAME" + ": " + document.DName + "\t");
                        sb.AppendLine("CCOMPONENTID" + ": " + document.ComponentID + "\t");
                        sb.AppendLine("DREMARKS" + ": ");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine(document.Remarks + "\t");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine("DSTATUS" + ": " + Convert.ToInt32(document.Status) + "\t");
                        sb.AppendLine("#UPDATE#");
                        sb.Append(Environment.NewLine);
                    }
                    break;
                case "MHopper":
                    MHopper hopper = (MHopper)entity;
                    sb.AppendLine("HDESC" + ": " + hopper.HName + "\t");
                    for (int i = 0; i < 8; i++)
                    {
                        sb.AppendLine("HC" + (i + 1) + "" + ": " + hopper.HopperArray[i, 3] + "\t");
                        sb.AppendLine("H" + (i + 1) + "TEMP" + ": " + hopper.HopperArray[i, 0] + "\t");
                        sb.AppendLine("H" + (i + 1) + "RANGE" + ": " + hopper.HopperArray[i, 1] + "\t");
                        sb.AppendLine("H" + (i + 1) + "MASK" + ": " + hopper.HopperArray[i, 2] + "\t");
                    }
                    sb.AppendLine("HREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(hopper.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("HSTATID" + ": " + Convert.ToInt32(hopper.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MBizHour":
                    MBizHour bizHour = (MBizHour)entity;
                    sb.AppendLine("BDESC" + ": " + bizHour.TemplateName + "\t");
                    sb.AppendLine("BMONDAY" + ": " + Convert.ToInt32(bizHour.Monday) + "\t");
                    sb.AppendLine("BTUESDAY" + ": " + Convert.ToInt32(bizHour.Tuesday) + "\t");
                    sb.AppendLine("BWEDNESDAY" + ": " + Convert.ToInt32(bizHour.Wednesday) + "\t");
                    sb.AppendLine("BTHURSDAY" + ": " + Convert.ToInt32(bizHour.Thursday) + "\t");
                    sb.AppendLine("BFRIDAY" + ": " + Convert.ToInt32(bizHour.Friday) + "\t");
                    sb.AppendLine("BSATURDAY" + ": " + Convert.ToInt32(bizHour.Saturday) + "\t");
                    sb.AppendLine("BSUNDAY" + ": " + Convert.ToInt32(bizHour.Sunday) + "\t");
                    sb.AppendLine("BSTARTTIME" + ": " + bizHour.Starttime + "\t");
                    sb.AppendLine("BENDTIME" + ": " + bizHour.Endtime + "\t");
                    sb.AppendLine("BREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(bizHour.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("BSTATID" + ": " + Convert.ToInt32(bizHour.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MKiosk":
                    MKiosk kiosk = (MKiosk)entity;
                    sb.AppendLine("DESCRIPTION" + ": " + kiosk.MachineDescription + "\t");
                    sb.AppendLine("SERIAL" + ": " + kiosk.MachineSerial + "\t");
                    sb.AppendLine("MKIOSKID" + ": " + kiosk.MachineKioskID + "\t");
                    sb.AppendLine("MSTATIONID" + ": " + kiosk.MachineStationID + "\t");
                    sb.AppendLine("ADDRESS" + ": ");
                    sb.AppendLine("#ADDRESS#");
                    sb.AppendLine(kiosk.MachineAddress + "\t");
                    sb.AppendLine("#ADDRESS#");
                    sb.AppendLine("LATITUDE" + ": " + kiosk.MachineLatitude + "\t");
                    sb.AppendLine("LONGITUDE" + ": " + kiosk.MachineLongtitude + "\t");
                    sb.AppendLine("MGROUPID" + ": " + kiosk.MachineGroupID + "\t");
                    sb.AppendLine("GROUPDESC" + ": " + kiosk.MachineGroupDesc + "\t");
                    sb.AppendLine("MREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(kiosk.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("MSTATID" + ": " + Convert.ToInt32(kiosk.Status) + "\t");
                    sb.AppendLine("IPADDRESS" + ": " + kiosk.MacIP + "\t");
                    sb.AppendLine("PORTNUMBER" + ": " + kiosk.MacPort + "\t");
                    //sb.AppendLine("MPILOT" + ": " + kiosk.MacPilot + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "AWrapServ":
                    AWrapServ wrapServ = (AWrapServ)entity;
                    sb.AppendLine("WSMAIN" + ": " + wrapServ.WSMainName + "\t");
                    sb.AppendLine("WSTYPE" + ": " + wrapServ.WSType + "\t");

                    if (wrapServ.Detail.WSID_Detail == Guid.Empty)
                    {
                        sb.AppendLine("WSREMARKS" + ": ");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine(wrapServ.Remarks + "\t");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine("WSSTATUS" + ": " + Convert.ToInt32(wrapServ.Status) + "\t");
                    }
                    if (wrapServ.Detail.WSID_Detail != Guid.Empty)
                    {
                        sb.AppendLine("WSID_DETAIL" + ": " + wrapServ.Detail.WSID_Detail + "\t");
                        sb.AppendLine("WSDETAIL" + ": " + wrapServ.Detail.WSDetailName + "\t");
                    }
                    sb.AppendLine("#UPDATE#");
                    break;
                case "MACHINE_ADVERTISEMENT":
                    MACHINE_ADVERTISEMENT advert1 = (MACHINE_ADVERTISEMENT)entity;
                    sb.AppendLine("ANAME" + ": " + advert1.ANAME + "\t");
                    sb.AppendLine("ADESC" + ": " + advert1.ADESC + "\t");
                    sb.AppendLine("ADIRECTORY" + ": " + advert1.ARELATIVEPATHURL + "\t");
                    sb.AppendLine("ADURATION" + ": " + advert1.ADURATION + "\t");
                    sb.AppendLine("AREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(advert1.AREMARKS + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("ASTATUS" + ": " + Convert.ToInt32(advert1.ASTATUS) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "AMasterSettings":
                    AMasterSettings masterSetting = (AMasterSettings)entity;
                    sb.AppendLine("PWORD" + ": " + masterSetting.PWord + "\t");
                    sb.AppendLine("#UPDATE#");
                    break;
            }
            ret = sb.ToString();

        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    private string[] CompareOldAndNew(string oldValues, string newValues)
    {
        string[] ret = new string[2];
        bool isDif = false;
        try
        {
            string[] tmpOld = System.Text.RegularExpressions.Regex.Split(oldValues, "#UPDATE#");
            string[] tmpNew = System.Text.RegularExpressions.Regex.Split(newValues, "#UPDATE#");
            for (int i = 0; i < tmpNew.Length; i++)
            {
                if (tmpOld.Length != tmpNew.Length)
                {
                    isDif = true;
                }
                else if (tmpOld[i].Contains("#ADVERTISEMENT#") && tmpNew[i].Contains("#ADVERTISEMENT#"))// && tmpOld[i].Contains("#USERGROUP#") && tmpNew[i].Contains("#USERGROUP#"))
                {
                    string[] tmpAdvertOld = System.Text.RegularExpressions.Regex.Split(tmpOld[i], "#ADVERTISEMENT#");
                    string[] tmpAdvertNew = System.Text.RegularExpressions.Regex.Split(tmpNew[i], "#ADVERTISEMENT#");
                    //string[] tmpUserOld = System.Text.RegularExpressions.Regex.Split(tmpOld[i], "#USERGROUP#");
                    //string[] tmpUserNew = System.Text.RegularExpressions.Regex.Split(tmpNew[i], "#USERGROUP#");
                    if (tmpAdvertOld[1].Length != tmpAdvertNew[1].Length)// || tmpUserOld[1].Length != tmpUserNew[1].Length)
                    {
                        isDif = true;
                    }
                }
                else if (tmpOld[i].Contains("#ADDRESS#") && tmpNew[i].Contains("#ADDRESS#"))
                {
                    string[] tmpAdressOld = System.Text.RegularExpressions.Regex.Split(tmpOld[i], "#ADDRESS#");
                    string[] tmpAdressNew = System.Text.RegularExpressions.Regex.Split(tmpNew[i], "#ADDRESS#");
                    if (tmpAdressOld[1].Length != tmpAdressNew[1].Length)
                    {
                        isDif = true;
                    }
                }

                if (tmpOld.Length != tmpNew.Length)
                {
                    isDif = true;
                }
                else if (tmpOld[i].Contains("#REMARKS#") && tmpNew[i].Contains("#REMARKS#"))
                {
                    string[] tmpRemarksOld = System.Text.RegularExpressions.Regex.Split(tmpOld[i], "#REMARKS#");
                    string[] tmpRemarksNew = System.Text.RegularExpressions.Regex.Split(tmpNew[i], "#REMARKS#");
                    if (tmpRemarksOld[1].Length != tmpRemarksNew[1].Length)
                    {
                        isDif = true;
                    }
                }

                if (!isDif)
                {
                    string[] tmpOld1 = System.Text.RegularExpressions.Regex.Split(tmpOld[i], "\r\n");
                    string[] tmpNew1 = System.Text.RegularExpressions.Regex.Split(tmpNew[i], "\r\n");
                    for (int j = 0; j < tmpNew1.Length; j++)
                    {
                        string[] tmpOld2 = System.Text.RegularExpressions.Regex.Split(tmpOld1[j], ": ");
                        string[] tmpNew2 = System.Text.RegularExpressions.Regex.Split(tmpNew1[j], ": ");
                        for (int k = 0; k < tmpNew2.Length; k++)
                        {
                            if (tmpOld2[k].Trim() != tmpNew2[k].Trim() && tmpNew2[k].Trim() != "#NOT CHANGE#")
                            {
                                isDif = true;
                            }
                        }
                    }
                }

                if (isDif)
                {
                    if (tmpOld.Length < tmpNew.Length)
                    {
                        if (i < tmpOld.Length)
                        {
                            ret[0] += tmpOld[i] + "#UPDATE#";
                            ret[1] += tmpNew[i] + "#UPDATE#";
                        }
                        else
                        {
                            ret[1] += tmpNew[i] + "#UPDATE#";
                        }
                    }
                    else if (tmpOld.Length > tmpNew.Length)
                    {
                        if (i < tmpNew.Length)
                        {
                            ret[0] += tmpOld[i] + "#UPDATE#";
                            ret[1] += tmpNew[i] + "#UPDATE#";
                        }
                        else
                        {
                            ret[0] += tmpOld[i] + "#UPDATE#";
                        }
                    }
                    else
                    {
                        ret[0] += tmpOld[i] + "#UPDATE#";
                        ret[1] += tmpNew[i] + "#UPDATE#";
                    }
                    isDif = false;
                }
            }

        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    //get the most recent edited data (only) from the same module id
    public DataTable GetEdititngData(Guid id)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from ( ");
            sql.AppendLine("select te.*, Row_Number() Over (order by editedDate desc) As RowNum, editUL.uName EditedName, checkUL.uName CheckedName from ");
            sql.AppendLine("tblEditing te ");
            sql.AppendLine("left join User_Login editUL on (te.editedBy = editUL.aID) ");
            sql.AppendLine("left join User_Login checkUL on (te.checkedBy = checkUL.aID) ");
            sql.AppendLine("where idOriginal = @idOriginal and (updateStatus = @updateStatus or updateStatus = @createdStatus) ");
            //sql.AppendLine("order by editedDate desc ");
            sql.AppendLine(") OneRow where ROWNUM <= 1 ");
            MyParams.Add(new Params("@idOriginal", "GUID", id));
            MyParams.Add(new Params("@updateStatus", "NVARCHAR", "EDITED"));
            MyParams.Add(new Params("@createdStatus", "NVARCHAR", "CREATED"));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    //get all edited data from the same module id
    public DataTable GetAllPendingEditingData(Guid id)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();

            string query = @"
                            select te.*, editUL.uName EditedName, checkUL.uName CheckedName  
                            from tblEditing te 
                            left join User_Login editUL on (te.editedBy = editUL.aID)
                            left join User_Login checkUL on (te.checkedBy = checkUL.aID) 
                            where idOriginal = @idOriginal 
                            and ((te.CHECKEDDATE is null) or (te.UPDATESTATUS = @updateStatus) or (te.UPDATESTATUS = @createdStatus)) 
                            ";
            sql.AppendLine(query);

            MyParams.Add(new Params("@idOriginal", "GUID", id));
            MyParams.Add(new Params("@updateStatus", "NVARCHAR", "EDITED"));
            MyParams.Add(new Params("@createdStatus", "NVARCHAR", "CREATED"));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public bool DeclineEditing(Guid id, Guid checkerID, string remarks, string ClientIp)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update tblEditing set updateStatus = :updateStatus, remarks = :remarks, checkedDate = :checkedDate, checkedBy = :checkedBy, checkerIP = :checkerIP ");
            sql.AppendLine("where idoriginal = :idOriginal and updateStatus <> :updateStatus1 ");
            sql.AppendLine("and updateStatus <> :updateStatus2 and updateStatus <> :updateStatus3 ");
            //sql.AppendLine("and editedDate = ( ");
            //sql.AppendLine("select * from (select editeddate from tblediting order by EDITEDDATE desc) ");
            //sql.AppendLine("OneRow where ROWNUM <= 1) ");
            MyParams.Add(new Params(":idOriginal", "GUID", id));
            MyParams.Add(new Params(":updateStatus", "NVARCHAR", "DECLINE"));
            MyParams.Add(new Params(":remarks", "NVARCHAR", remarks));
            MyParams.Add(new Params(":updateStatus1", "NVARCHAR", "APPROVE"));
            MyParams.Add(new Params(":updateStatus2", "NVARCHAR", "CREATED"));
            MyParams.Add(new Params(":updateStatus3", "NVARCHAR", "REJECT"));
            MyParams.Add(new Params(":checkedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params(":checkedBy", "GUID", checkerID));
            MyParams.Add(new Params(":checkerIP", "NVARCHAR", ClientIp));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public bool ApproveEditing(Guid id, Guid checkerID, string remarks, string ClientIp)
    {
        bool ret = false;
        try
        {
            var time = DateTime.Now;


            //DeclineCreating(id, checkerID, "");
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update tblEditing set updateStatus = @updateStatus, remarks = @remarks, checkedDate = @checkedDate, checkedBy = @checkedBy, checkerIP = @checkerIP  ");
            sql.AppendLine("where idoriginal = @idOriginal and updateStatus = @updateStatus1 ");

            MyParams.Add(new Params("@idOriginal", "GUID", id));
            MyParams.Add(new Params("@updateStatus", "NVARCHAR", "APPROVE"));
            MyParams.Add(new Params("@updateStatus1", "NVARCHAR", "EDITED"));
            MyParams.Add(new Params("@checkedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@checkedBy", "GUID", checkerID));
            MyParams.Add(new Params("@remarks", "NVARCHAR", remarks));
            //MyParams.Add(new Params("@checkerIP", "NVARCHAR", HttpContext.Current.Request.UserHostAddress.ToString()));
            //MyParams.Add(new Params("@checkerIP", "NVARCHAR", HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == string.Empty ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] : HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]));
            MyParams.Add(new Params("@checkerIP", "NVARCHAR", ClientIp));

            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");

            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public bool IsEditingDataExist(Guid id)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from ( ");
            sql.AppendLine("select te.*, Row_Number() Over (order by editedDate desc) As ROWNUM, editUL.uName EditedName, checkUL.uName CheckedName from ");
            sql.AppendLine("tblEditing te ");
            sql.AppendLine("left join User_Login editUL on (te.editedBy = editUL.aID) ");
            sql.AppendLine("left join User_Login checkUL on (te.checkedBy = checkUL.aID) ");
            sql.AppendLine("where idOriginal = @idOriginal and (updateStatus = @updateStatus or updateStatus = @updateStatus1) ");
            //sql.AppendLine("order by editedDate desc ");
            sql.AppendLine(") OneRow where ROWNUM <= 1 ");
            MyParams.Add(new Params("@idOriginal", "GUID", id));
            MyParams.Add(new Params("@updateStatus", "NVARCHAR", "EDITED"));
            MyParams.Add(new Params("@updateStatus1", "NVARCHAR", "CREATED"));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count > 0)
            {
                ret = true;
            }
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public string GetEditorIfEditingDataExist(Guid id)
    {
        string ret = "";
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from ( ");
            sql.AppendLine("select te.*, Row_Number() Over (order by editedDate desc) As RowNum, editUL.uName EditedName, checkUL.uName CheckedName from ");
            sql.AppendLine("tblEditing te ");
            sql.AppendLine("left join User_Login editUL on (te.editedBy = editUL.aID) ");
            sql.AppendLine("left join User_Login checkUL on (te.checkedBy = checkUL.aID) ");
            sql.AppendLine("where idOriginal = @idOriginal and (updateStatus = @updateStatus or updateStatus = @updateStatus1) ");
            //sql.AppendLine("order by editedDate desc ");
            sql.AppendLine(") OneRow where ROWNUM <= 1 ");
            MyParams.Add(new Params("@idOriginal", "GUID", id));
            MyParams.Add(new Params("@updateStatus", "NVARCHAR", "EDITED"));
            MyParams.Add(new Params("@updateStatus1", "NVARCHAR", "CREATED"));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            if (dt.Rows.Count > 0)
            {
                ret = dt.Rows[0]["EditedName"].ToString();
            }
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }
    #endregion

    #region DataCreating
    public bool CreateData(Object entity, string ClientIp)
    {
        bool ret = false;
        UserRoleController urController = new UserRoleController();
        AdvertisementController advertController = new AdvertisementController();
        MasterSettingController msController = new MasterSettingController();
        BranchController branchController = new BranchController();
        try
        {
            Guid idOriginal = Guid.Empty;
            Guid mid = Guid.Empty;
            string tblName = "";
            string oldValues = "";
            string newValues = "";
            Guid editedBy = Guid.Empty;

            switch (entity.ToString().Split('.').Last())
            {
                case "Role":
                    Role role = (Role)entity;
                    idOriginal = role.RoleID;
                    mid = Module.GetModuleId(ModuleLogAction.Create_RoleMaintenance);
                    tblName = "User_Roles";
                    editedBy = role.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
                case "User":
                    User user = (User)entity;
                    idOriginal = user.UserID;
                    mid = Module.GetModuleId(ModuleLogAction.Create_UserMaintenance);
                    tblName = "User_Login";
                    editedBy = user.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
                case "Branch":
                    Branch branch = (Branch)entity;
                    idOriginal = branch.Bid;
                    mid = Module.GetModuleId(ModuleLogAction.Create_BranchMaintenance);
                    tblName = "User_Branch";
                    editedBy = branch.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
                case "MAlert":
                    MAlert alert = (MAlert)entity;
                    idOriginal = alert.AID;
                    mid = Module.GetModuleId(ModuleLogAction.Create_Alert_Template);
                    tblName = "Machine_Alert";
                    editedBy = alert.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
                case "MFirm":
                    MFirm firm = (MFirm)entity;
                    idOriginal = firm.SysID;
                    mid = Module.GetModuleId(ModuleLogAction.Create_Application);
                    tblName = "tblFirmware";
                    editedBy = firm.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
                case "MGroup":
                    MGroup group = (MGroup)entity;
                    idOriginal = group.KID;
                    mid = Module.GetModuleId(ModuleLogAction.Create_Group_Template);
                    tblName = "Machine_Group";
                    editedBy = group.UpdatedBy;
                    //DataTable dt = GroupMaintenanceController.getAssignedGroupsByID(idOriginal);
                    //DataTable dtGroup = dt.DefaultView.ToTable(true, "DESCRIPTION", "KSCREENBACKGROUND", "backgroundImage", "aDirectory", "alertID", "alert", "bID", "businesshour", "dID", "document", "hID", "hopper", "kRemarks", "kStatus");
                    //DataTable dtAdvert = dt.DefaultView.ToTable(false, "advertID", "advertisement", "seqOrder");
                    //dtAdvert.DefaultView.Sort = "seqOrder asc";
                    //dtAdvert = dtAdvert.DefaultView.ToTable(false, "ADVERTID", "ADVERTISEMENT");
                    //DataTable dtAll = new DataTable("dbTable");
                    //dtAll.Columns.Add("DESCRIPTION", typeof(string));
                    //dtAll.Columns.Add("KSCREENBACKGROUND", typeof(string));
                    //dtAll.Columns.Add("BACKGROUNDIMAGE", typeof(string));
                    //dtAll.Columns.Add("ADIRECTORY", typeof(string));
                    //dtAll.Columns.Add("ALERTID", typeof(string));
                    //dtAll.Columns.Add("ALERT", typeof(string));
                    //dtAll.Columns.Add("BID", typeof(string));
                    //dtAll.Columns.Add("BUSINESSHOUR", typeof(string));
                    //dtAll.Columns.Add("DID", typeof(string));
                    //dtAll.Columns.Add("DOCUMENT", typeof(string));
                    //dtAll.Columns.Add("HID", typeof(string));
                    //dtAll.Columns.Add("HOPPER", typeof(string));
                    //dtAll.Columns.Add("KREMARKS", typeof(string));
                    //dtAll.Columns.Add("KSTATUS", typeof(string));
                    //dtAll.Columns.Add("ADVERT", typeof(string));
                    //dtAll.Rows.Add(
                    //    dtGroup.Rows[0]["DESCRIPTION"].ToString(),
                    //    dtGroup.Rows[0]["KSCREENBACKGROUND"].ToString(),
                    //    dtGroup.Rows[0]["BACKGROUNDIMAGE"].ToString(),
                    //    dtGroup.Rows[0]["ADIRECTORY"].ToString(),
                    //    dtGroup.Rows[0]["ALERTID"].ToString(),
                    //    dtGroup.Rows[0]["ALERT"].ToString(),
                    //    dtGroup.Rows[0]["BID"].ToString(),
                    //    dtGroup.Rows[0]["BUSINESSHOUR"].ToString(),
                    //    dtGroup.Rows[0]["DID"].ToString(),
                    //    dtGroup.Rows[0]["DOCUMENT"].ToString(),
                    //    dtGroup.Rows[0]["HID"].ToString(),
                    //    dtGroup.Rows[0]["HOPPER"].ToString(),
                    //    dtGroup.Rows[0]["KREMARKS"].ToString(),
                    //    dtGroup.Rows[0]["KSTATUS"].ToString(),
                    //    "\r\n#ADVERTISEMENT#\r\n" + DBToString(dtAdvert).Replace("#UPDATE#", "") + "#ADVERTISEMENT#"
                    //    );
                    newValues = ProcessUPString(entity);
                    break;
                case "MAdvertisement":
                    MAdvertisement advert = (MAdvertisement)entity;
                    idOriginal = advert.AID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_AdvertisementManagement);
                    tblName = "Machine_Advertisement";
                    editedBy = advert.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
                case "MCard":
                    MCard card = (MCard)entity;
                    idOriginal = card.CID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_Card_Maintenance);
                    tblName = "Hopper_Card";
                    editedBy = card.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
                case "MDocument":
                    MDocument document = (MDocument)entity;
                    editedBy = document.UpdatedBy;
                    if (document.ComponentID == "" || document.ComponentID == null)
                    {
                        idOriginal = document.DID;
                        mid = Module.GetModuleId(ModuleLogAction.Update_Document_Template);
                        tblName = "Machine_Document";
                        newValues = ProcessUPString(entity);
                    }
                    else
                    {
                        idOriginal = document.DID;
                        mid = Module.GetModuleId(ModuleLogAction.Update_DocType_Template);
                        tblName = "DocType_Setup";
                        newValues = ProcessUPString(entity);
                    }
                    break;
                case "MHopper":
                    MHopper hopper = (MHopper)entity;
                    idOriginal = hopper.HID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_Hopper_Template);
                    tblName = "Machine_Hopper";
                    editedBy = hopper.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
                case "MBizHour":
                    MBizHour bizHour = (MBizHour)entity;
                    idOriginal = bizHour.Bid;
                    mid = Module.GetModuleId(ModuleLogAction.Update_BusinessHour_Template);
                    tblName = "Machine_BusinessHour";
                    editedBy = bizHour.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
                case "MKiosk":
                    MKiosk kiosk = (MKiosk)entity;
                    idOriginal = kiosk.MachineID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_Kiosk);
                    tblName = "Machine";
                    editedBy = kiosk.UpdatedBy;

                    DataTable dtMI = KioskCreateMaintenanceController.getMachineItemsById(kiosk.MachineID);
                    DataTable dtKiosk = new DataTable("dbTable");
                    dtKiosk.Columns.Add("DESCRIPTION", typeof(string));
                    dtKiosk.Columns.Add("SERIAL", typeof(string));
                    dtKiosk.Columns.Add("MKIOSKID", typeof(string));
                    dtKiosk.Columns.Add("MSTATIONID", typeof(string));
                    dtKiosk.Columns.Add("ADDRESS", typeof(string));
                    dtKiosk.Columns.Add("LATITUDE", typeof(string));
                    dtKiosk.Columns.Add("LONGITUDE", typeof(string));
                    dtKiosk.Columns.Add("MGROUPID", typeof(string));
                    dtKiosk.Columns.Add("GROUPDESC", typeof(string));
                    dtKiosk.Columns.Add("MREMARKS", typeof(string));
                    dtKiosk.Columns.Add("MSTATID", typeof(string));
                    dtKiosk.Columns.Add("IPADDRESS", typeof(string));
                    dtKiosk.Columns.Add("PORTNUMBER", typeof(string));
                    //dtKiosk.Columns.Add("MPILOT", typeof(string));
                    dtKiosk.Rows.Add(dtMI.Rows[0]["DESCRIPTION"].ToString(),
                        dtMI.Rows[0]["SERIAL"].ToString(),
                        dtMI.Rows[0]["MKIOSKID"].ToString(),
                        dtMI.Rows[0]["MSTATIONID"].ToString(),
                        "\r\n#ADDRESS#\r\n" + dtMI.Rows[0]["ADDRESS"].ToString() + "\t\r\n#ADDRESS#",
                        dtMI.Rows[0]["LATITUDE"].ToString(),
                        dtMI.Rows[0]["LONGITUDE"].ToString(),
                        dtMI.Rows[0]["MGROUPID"].ToString(),
                        dtMI.Rows[0]["GROUPDESC"].ToString(),
                        dtMI.Rows[0]["MREMARKS"].ToString(),
                        dtMI.Rows[0]["MSTATID"].ToString(),
                        dtMI.Rows[0]["IPADDRESS"].ToString(),
                        dtMI.Rows[0]["PORTNUMBER"].ToString()
                        //dtMI.Rows[0]["MPILOT"].ToString()
                        );

                    newValues = ProcessUPString(entity);
                    break;
                case "AWrapServ":
                    AWrapServ wrapServ = (AWrapServ)entity;
                    idOriginal = wrapServ.WSID;
                    if (wrapServ.WSType == WrapServType.Serv)
                        mid = Module.GetModuleId(ModuleLogAction.Update_Service_Template);
                    else
                        mid = Module.GetModuleId(ModuleLogAction.Update_Wrap_Template);
                    tblName = "Wrap_Serv_Main";
                    editedBy = wrapServ.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
                case "MACHINE_ADVERTISEMENT":
                    MACHINE_ADVERTISEMENT advert1 = (MACHINE_ADVERTISEMENT)entity;
                    idOriginal = advert1.AID;
                    mid = Module.GetModuleId(ModuleLogAction.Update_AdvertisementManagement);
                    tblName = "MACHINE_ADVERTISEMENT";
                    editedBy = (String.IsNullOrEmpty(advert1.AUPDATEDBY)) ? Guid.Empty : Guid.Parse(advert1.AUPDATEDBY);
                    newValues = ProcessUPString(entity);
                    break;
                case "AMasterSettings":
                    AMasterSettings masterSetting = (AMasterSettings)entity;
                    idOriginal = Module.GetModuleId(ModuleLogAction.View_MasterSetting);
                    mid = Module.GetModuleId(ModuleLogAction.Update_MasterSetting);
                    tblName = "tblMasterSetting";
                    editedBy = masterSetting.UpdatedBy;
                    newValues = ProcessUPString(entity);
                    break;
            }
            //StoreToFile(oldValues, "dtDBToString");
            //StoreToFile(newValues, "dtProcessUPString");

            //string[] compare = CompareOldAndNew(oldValues, newValues);
            //oldValues = compare[0];
            //newValues = compare[1];

            //StoreToFile(compare[0], "OldString");
            //StoreToFile(compare[1], "NewString");

            if (newValues != "" && newValues != null)
            {

                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("insert into tblEditing");
                sql.AppendLine("(idOriginal,tblName,oldValues,newValues,editedDate,updateStatus,editedBy,makerIP,mID, ActionName)");
                sql.AppendLine("values");
                sql.AppendLine("(:idOriginal, :tblName, :oldValues, :newValues, :editedDate, :updateStatus, :editedBy, :makerIP, :mID, :actName)");
                MyParams.Add(new Params(":idOriginal", "GUID", idOriginal));
                MyParams.Add(new Params(":tblName", "NVARCHAR", tblName));
                MyParams.Add(new Params(":oldValues", "NCLOB", oldValues));
                MyParams.Add(new Params(":newValues", "NCLOB", newValues));
                MyParams.Add(new Params(":editedDate", "DATETIME", DateTime.Now));
                MyParams.Add(new Params(":updateStatus", "NVARCHAR", "CREATED"));
                MyParams.Add(new Params(":editedBy", "GUID", editedBy));
                MyParams.Add(new Params("@makerIP", "NVARCHAR", ClientIp));
                MyParams.Add(new Params(":mID", "GUID", mid));
                MyParams.Add(new Params(":actName", "NVARCHAR", "Create"));

                ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
            }
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    private string ProcessUPString(Object entity)
    {
        string ret = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            switch (entity.ToString().Split('.').Last())
            {
                case "Role":
                    Role role = (Role)entity;
                    foreach (var gvrRow in (List<ModuleViewModel>)role.Items)
                    {
                        Guid mID = Guid.Parse(gvrRow.mID);
                        sb.AppendLine("RDESC" + ": " + role.RoleDesc + "\t");
                        sb.AppendLine("RREMARKS" + ": ");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine(role.Remarks + "\t");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine("RSTATUS" + ": " + "0\t"); //Convert.ToInt32(role.Status) + "\t");
                        sb.AppendLine("MID" + ": " + mID + "\t");
                        sb.AppendLine("CHILD" + ": " + gvrRow.child + "\t");
                        sb.AppendLine("MVIEW" + ": " + Convert.ToInt32(gvrRow.mView) + "\t");
                        sb.AppendLine("MMAKER" + ": " + Convert.ToInt32(gvrRow.mMaker) + "\t");
                        sb.AppendLine("MCHECKER" + ": " + Convert.ToInt32(gvrRow.mChecker) + "\t");
                        sb.AppendLine("#CREATE#");
                        sb.Append(Environment.NewLine);
                    }
                    break;
                case "User":
                    User user = (User)entity;
                    sb.AppendLine("UNAME" + ": " + user.UserName + "\t");
                    sb.AppendLine("UFULLNAME" + ": " + user.UserFullName + "\t");
                    sb.AppendLine("UCMID" + ": " + user.UserCMID + "\t");
                    sb.AppendLine("RID" + ": " + user.RoleID + "\t");
                    sb.AppendLine("RDESC" + ": " + user.RoleDesc + "\t");
                    sb.AppendLine("AGENTFLAG" + ": " + Convert.ToInt32(user.AgentFlag) + "\t");
                    sb.AppendLine("MCARDREPLENISHMENT" + ": " + Convert.ToInt32(user.CardReplenishment) + "\t");
                    sb.AppendLine("MCHEQUEREPLENISHMENT" + ": " + Convert.ToInt32(user.ChequeReplenishment) + "\t");
                    sb.AppendLine("MCONSUMABLE" + ": " + Convert.ToInt32(user.ConsumableCollection) + "\t");
                    sb.AppendLine("MSECURITY" + ": " + Convert.ToInt32(user.Security) + "\t");
                    sb.AppendLine("MTROUBLESHOOT" + ": " + Convert.ToInt32(user.Troubleshoot) + "\t");
                    sb.AppendLine("LOCALUSER" + ": " + Convert.ToInt32(user.LocalUser) + "\t");
                    sb.AppendLine("LPASSWORD" + ": " + user.Password + "\t");
                    sb.AppendLine("LEMAIL" + ": " + user.Email + "\t");
                    sb.AppendLine("UREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(user.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("USTATUS" + ": " + "0\t");
                    sb.AppendLine("#CREATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "Branch":
                    Branch branch = (Branch)entity;
                    sb.AppendLine("BDESC" + ": " + branch.TemplateName + "\t");
                    sb.AppendLine("BMONDAY" + ": " + Convert.ToDecimal(branch.Monday) + "\t");
                    sb.AppendLine("BTUESDAY" + ": " + Convert.ToDecimal(branch.Tuesday) + "\t");
                    sb.AppendLine("BWEDNESDAY" + ": " + Convert.ToDecimal(branch.Wednesday) + "\t");
                    sb.AppendLine("BTHURSDAY" + ": " + Convert.ToDecimal(branch.Thursday) + "\t");
                    sb.AppendLine("BFRIDAY" + ": " + Convert.ToDecimal(branch.Friday) + "\t");
                    sb.AppendLine("BSATURDAY" + ": " + Convert.ToDecimal(branch.Saturday) + "\t");
                    sb.AppendLine("BSUNDAY" + ": " + Convert.ToDecimal(branch.Sunday) + "\t");
                    sb.AppendLine("BOPENTIME" + ": " + branch.Starttime + "\t");
                    sb.AppendLine("BCLOSETIME" + ": " + branch.Endtime + "\t");
                    sb.AppendLine("BREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(branch.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("BSTATID" + ": " + "0\t"); //Convert.ToInt32(branch.Status) + "\t");
                    sb.AppendLine("#CREATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MAlert":
                    MAlert alert = (MAlert)entity;
                    sb.AppendLine("ADESC" + ": " + alert.ADesc + "\t");
                    sb.AppendLine("AMINCARDBAL" + ": " + alert.AMinCardBal + "\t");
                    sb.AppendLine("AMINCHEQUEBAL" + ": " + alert.AMinChequeBal + "\t");
                    sb.AppendLine("AMINPAPERBAL" + ": " + alert.AMinPaperBal + "\t");
                    sb.AppendLine("AMINREJCARDBAL" + ": " + alert.AMinRejCardBal + "\t");
                    sb.AppendLine("AMINRIBFRONTBAL" + ": " + alert.ARibFrontBal + "\t");
                    sb.AppendLine("AMINRIBREARBAL" + ": " + alert.ARibRearBal + "\t");
                    sb.AppendLine("AMINRIBTIPBAL" + ": " + alert.ARibTipBal + "\t");
                    sb.AppendLine("AMINCHEQUEPRINTBAL" + ": " + alert.AChequePrintBal + "\t");
                    sb.AppendLine("AMINCHEQUEPRINTCATRIDGE" + ": " + alert.AChequePrintCatridge + "\t");
                    sb.AppendLine("AMINCATRIDGEBAL" + ": " + alert.ACatridgeBal + "\t");

                    sb.AppendLine("ACARDEMAIL" + ": " + alert.ACardEmail + "\t");
                    sb.AppendLine("ACARDSMS" + ": " + alert.ACardSMS + "\t");
                    sb.AppendLine("ACARDTINTERVAL" + ": " + alert.ACardTimeInterval + "\t");
                    sb.AppendLine("ACHEQUEEMAIL" + ": " + alert.AChequeEmail + "\t");
                    sb.AppendLine("ACHEQUESMS" + ": " + alert.AChequeSMS + "\t");
                    sb.AppendLine("ACHEQUETINTERVAL" + ": " + alert.AChequeTimeInterval + "\t");
                    sb.AppendLine("AMAINTENANCEEMAIL" + ": " + alert.AMaintenanceEmail + "\t");
                    sb.AppendLine("AMAINTENANCESMS" + ": " + alert.AMaintenanceSMS + "\t");
                    sb.AppendLine("AMAINTENANCETINTERVAL" + ": " + alert.AMaintenanceTimeInterval + "\t");
                    sb.AppendLine("ASECURITYEMAIL" + ": " + alert.ASecurityEmail + "\t");
                    sb.AppendLine("ASECURITYSMS" + ": " + alert.ASecuritySMS + "\t");
                    sb.AppendLine("ASECURITYTINTERVAL" + ": " + alert.ASecurityTimeInterval + "\t");
                    sb.AppendLine("ATROUBLESHOOTEMAIL" + ": " + alert.ATroubleShootEmail + "\t");
                    sb.AppendLine("ATROUBLESHOOTSMS" + ": " + alert.ATroubleShootSMS + "\t");
                    sb.AppendLine("ATROUBLESHOOTTINTERVAL" + ": " + alert.ATroubleShootTimeInterval + "\t");
                    sb.AppendLine("AREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(alert.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("ASTATUS" + ": " + Convert.ToInt32(alert.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MFirm":
                    MFirm firm = (MFirm)entity;
                    sb.AppendLine("FPATH" + ": " + firm.FPath + "\t");
                    sb.AppendLine("COUNTDL" + ": " + firm.CountDL + "\t");
                    sb.AppendLine("VER" + ": " + firm.Ver + "\t");
                    sb.AppendLine("FILESIZE" + ": " + firm.FSize + "\t");
                    sb.AppendLine("AGENTFLAG" + ": " + firm.AgentFlag + "\t");
                    sb.AppendLine("REMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(firm.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("STATUS" + ": " + "1\t"); //Convert.ToInt32(alert.Status) + "\t");
                    sb.AppendLine("#CREATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MGroup":
                    MGroup group = (MGroup)entity;
                    sb.AppendLine("DESCRIPTION" + ": " + group.KDesc + "\t");
                    sb.AppendLine("KSCREENBACKGROUND" + ": " + group.KScreenBackground.AID + "\t");
                    sb.AppendLine("BACKGROUNDIMAGE" + ": " + group.KScreenBackground.AName + "\t");
                    sb.AppendLine("ADIRECTORY" + ": " + group.KScreenBackground.ADirectory + "\t");
                    sb.AppendLine("ALERTID" + ": " + group.KAlertID + "\t");
                    sb.AppendLine("ALERT" + ": " + group.KAlertDesc + "\t");
                    sb.AppendLine("BID" + ": " + group.KBusinessHourID + "\t");
                    sb.AppendLine("BUSINESSHOUR" + ": " + group.KBusinessHourDesc + "\t");
                    sb.AppendLine("DID" + ": " + group.KDocumentID + "\t");
                    sb.AppendLine("DOCUMENT" + ": " + group.KDocumentDesc + "\t");
                    sb.AppendLine("HID" + ": " + group.KHopperID + "\t");
                    sb.AppendLine("HOPPER" + ": " + group.KHopperDesc + "\t");
                    sb.AppendLine("KREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(group.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("KSTATUS" + ": " + Convert.ToInt32(group.Status) + "\t");
                    sb.AppendLine("ADVERT" + ": ");
                    sb.AppendLine("#ADVERTISEMENT#");
                    foreach (var listItem in group.AdvIds)
                    {
                        sb.AppendLine("ADVERTID" + ": " + listItem + "\t");
                        //sb.AppendLine("ADVERTISEMENT" + ": " + listItem + "\t");
                        sb.AppendLine(Environment.NewLine);
                    }
                    sb.AppendLine("#ADVERTISEMENT#");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MAdvertisement":
                    MAdvertisement advert = (MAdvertisement)entity;
                    sb.AppendLine("ANAME" + ": " + advert.AName + "\t");
                    sb.AppendLine("ADESC" + ": " + advert.ADesc + "\t");
                    sb.AppendLine("ADIRECTORY" + ": " + advert.ADirectory + "\t");
                    sb.AppendLine("ADURATION" + ": " + advert.ADuration + "\t");
                    sb.AppendLine("AREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(advert.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("ASTATUS" + ": " + Convert.ToInt32(advert.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MCard":
                    MCard card = (MCard)entity;
                    sb.AppendLine("CDESC" + ": " + card.CDesc + "\t");
                    sb.AppendLine("CCONTACTLESS" + ": " + Convert.ToInt32(card.cContactless) + "\t");
                    sb.AppendLine("CTYPE" + ": " + card.CType + "\t");
                    sb.AppendLine("CBIN" + ": " + card.CBin + "\t");
                    sb.AppendLine("CMASK" + ": " + card.CMask + "\t");
                    sb.AppendLine("CREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(card.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("CSTATUS" + ": " + Convert.ToInt32(card.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MDocument":
                    MDocument document = (MDocument)entity;
                    if (document.ComponentID == "" || document.ComponentID == null)
                    {
                        sb.AppendLine("DDESC" + ": " + document.DName + "\t");
                        sb.AppendLine("DREMARKS" + ": ");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine(document.Remarks + "\t");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine("DSTATUS" + ": " + Convert.ToInt32(document.Status) + "\t");
                        sb.AppendLine("#UPDATE#" + Environment.NewLine);

                        for (int i = 1; i < document.DocComponent.Count; i++)
                        {
                            sb.AppendLine("DDESC" + ": " + document.DName + "\t");
                            sb.AppendLine("DSTATUS" + ": " + Convert.ToInt32(document.Status) + "\t");
                            sb.AppendLine("DOCTYPEID" + ": " + document.DocComponent[i++] + "\t");
                            sb.AppendLine("DNAME" + ": " + document.DocComponent[i++] + "\t");
                            sb.AppendLine("SWALLOW" + ": " + Convert.ToInt32(Convert.ToBoolean(document.DocComponent[i++])) + "\t");
                            sb.AppendLine("PRINT" + ": " + Convert.ToInt32(Convert.ToBoolean(document.DocComponent[i])) + "\t");
                            sb.AppendLine("#UPDATE#" + Environment.NewLine);
                        }
                    }
                    else
                    {
                        sb.AppendLine("DNAME" + ": " + document.DName + "\t");
                        sb.AppendLine("CCOMPONENTID" + ": " + document.ComponentID + "\t");
                        sb.AppendLine("DREMARKS" + ": ");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine(document.Remarks + "\t");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine("DSTATUS" + ": " + Convert.ToInt32(document.Status) + "\t");
                        sb.AppendLine("#UPDATE#");
                        sb.Append(Environment.NewLine);
                    }
                    break;
                case "MHopper":
                    MHopper hopper = (MHopper)entity;
                    sb.AppendLine("HDESC" + ": " + hopper.HName + "\t");
                    for (int i = 0; i < 8; i++)
                    {
                        sb.AppendLine("HC" + (i + 1) + "" + ": " + hopper.HopperArray[i, 3] + "\t");
                        sb.AppendLine("H" + (i + 1) + "TEMP" + ": " + hopper.HopperArray[i, 0] + "\t");
                        sb.AppendLine("H" + (i + 1) + "RANGE" + ": " + hopper.HopperArray[i, 1] + "\t");
                        sb.AppendLine("H" + (i + 1) + "MASK" + ": " + hopper.HopperArray[i, 2] + "\t");
                    }
                    sb.AppendLine("HREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(hopper.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("HSTATID" + ": " + Convert.ToInt32(hopper.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MBizHour":
                    MBizHour bizHour = (MBizHour)entity;
                    sb.AppendLine("BDESC" + ": " + bizHour.TemplateName + "\t");
                    sb.AppendLine("BMONDAY" + ": " + Convert.ToInt32(bizHour.Monday) + "\t");
                    sb.AppendLine("BTUESDAY" + ": " + Convert.ToInt32(bizHour.Tuesday) + "\t");
                    sb.AppendLine("BWEDNESDAY" + ": " + Convert.ToInt32(bizHour.Wednesday) + "\t");
                    sb.AppendLine("BTHURSDAY" + ": " + Convert.ToInt32(bizHour.Thursday) + "\t");
                    sb.AppendLine("BFRIDAY" + ": " + Convert.ToInt32(bizHour.Friday) + "\t");
                    sb.AppendLine("BSATURDAY" + ": " + Convert.ToInt32(bizHour.Saturday) + "\t");
                    sb.AppendLine("BSUNDAY" + ": " + Convert.ToInt32(bizHour.Sunday) + "\t");
                    sb.AppendLine("BSTARTTIME" + ": " + bizHour.Starttime + "\t");
                    sb.AppendLine("BENDTIME" + ": " + bizHour.Endtime + "\t");
                    sb.AppendLine("BREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(bizHour.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("BSTATID" + ": " + Convert.ToInt32(bizHour.Status) + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "MKiosk":
                    MKiosk kiosk = (MKiosk)entity;
                    sb.AppendLine("DESCRIPTION" + ": " + kiosk.MachineDescription + "\t");
                    sb.AppendLine("SERIAL" + ": " + kiosk.MachineSerial + "\t");
                    sb.AppendLine("MKIOSKID" + ": " + kiosk.MachineKioskID + "\t");
                    sb.AppendLine("MSTATIONID" + ": " + kiosk.MachineStationID + "\t");
                    sb.AppendLine("ADDRESS" + ": ");
                    sb.AppendLine("#ADDRESS#");
                    sb.AppendLine(kiosk.MachineAddress + "\t");
                    sb.AppendLine("#ADDRESS#");
                    sb.AppendLine("LATITUDE" + ": " + kiosk.MachineLatitude + "\t");
                    sb.AppendLine("LONGITUDE" + ": " + kiosk.MachineLongtitude + "\t");
                    sb.AppendLine("MGROUPID" + ": " + kiosk.MachineGroupID + "\t");
                    sb.AppendLine("GROUPDESC" + ": " + kiosk.MachineGroupDesc + "\t");
                    sb.AppendLine("MREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(kiosk.Remarks + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("MSTATID" + ": " + Convert.ToInt32(kiosk.Status) + "\t");
                    sb.AppendLine("IPADDRESS" + ": " + kiosk.MacIP + "\t");
                    sb.AppendLine("PORTNUMBER" + ": " + kiosk.MacPort + "\t");
                    //sb.AppendLine("MPILOT" + ": " + kiosk.MacPilot + "\t");
                    sb.AppendLine("#UPDATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "AWrapServ":
                    AWrapServ wrapServ = (AWrapServ)entity;
                    sb.AppendLine("WSMAIN" + ": " + wrapServ.WSMainName + "\t");
                    sb.AppendLine("WSTYPE" + ": " + wrapServ.WSType + "\t");

                    if (wrapServ.Detail.WSID_Detail == Guid.Empty)
                    {
                        sb.AppendLine("WSREMARKS" + ": ");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine(wrapServ.Remarks + "\t");
                        sb.AppendLine("#REMARKS#");
                        sb.AppendLine("WSSTATUS" + ": " + Convert.ToInt32(wrapServ.Status) + "\t");
                    }
                    if (wrapServ.Detail.WSID_Detail != Guid.Empty)
                    {
                        sb.AppendLine("WSID_DETAIL" + ": " + wrapServ.Detail.WSID_Detail + "\t");
                        sb.AppendLine("WSDETAIL" + ": " + wrapServ.Detail.WSDetailName + "\t");
                    }
                    sb.AppendLine("#UPDATE#");
                    break;
                case "MACHINE_ADVERTISEMENT":
                    MACHINE_ADVERTISEMENT advert1 = (MACHINE_ADVERTISEMENT)entity;
                    sb.AppendLine("ANAME" + ": " + advert1.ANAME + "\t");
                    sb.AppendLine("ADESC" + ": " + advert1.ADESC + "\t");
                    sb.AppendLine("ADIRECTORY" + ": " + advert1.ARELATIVEPATHURL + "\t");
                    sb.AppendLine("ADURATION" + ": " + advert1.ADURATION + "\t");
                    sb.AppendLine("AREMARKS" + ": ");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine(advert1.AREMARKS + "\t");
                    sb.AppendLine("#REMARKS#");
                    sb.AppendLine("ASTATUS" + ": " + "0\t"); //Convert.ToInt32(advert.Status) + "\t");
                    sb.AppendLine("#CREATE#");
                    sb.Append(Environment.NewLine);
                    break;
                case "AMasterSettings":
                    AMasterSettings masterSetting = (AMasterSettings)entity;
                    sb.AppendLine("MONTHCHEQUEMAX" + ": " + masterSetting.MonthChequeMax + "\t");
                    sb.AppendLine("VDNS" + ": " + masterSetting.VDNs + "\t");
                    sb.AppendLine("ENGLISH" + ": " + masterSetting.English + "\t");
                    sb.AppendLine("ARABIC" + ": " + masterSetting.Arabic + "\t");
                    sb.AppendLine("Password" + ": " + masterSetting.PWord + "\t");
                    sb.AppendLine("#CREATE#");
                    break;
            }
            ret = sb.ToString();

        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public bool RejectCreating(Guid id, Guid checkerID, string remarks, string ClientIp)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update tblEditing set updateStatus = :updateStatus, remarks = :remarks, checkedDate = :checkedDate, checkedBy = :checkedBy, checkerIP = :checkerIP ");
            sql.AppendLine("where idoriginal = :idOriginal and updateStatus = :updateStatus1 ");
            MyParams.Add(new Params(":idOriginal", "GUID", id));
            MyParams.Add(new Params(":updateStatus", "NVARCHAR", "REJECT"));
            MyParams.Add(new Params(":updateStatus1", "NVARCHAR", "CREATED"));
            MyParams.Add(new Params(":remarks", "NVARCHAR", remarks));
            MyParams.Add(new Params(":checkedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params(":checkedBy", "GUID", checkerID));
            //MyParams.Add(new Params(":checkerIP", "NVARCHAR", HttpContext.Current.Request.UserHostAddress.ToString()));
            MyParams.Add(new Params(":checkerIP", "NVARCHAR", ClientIp));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    public bool ApproveCreating(Guid id, Guid checkerID, string remarks, string ClientIp)
    {
        bool ret = false;
        try
        {

            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update tblEditing set updateStatus = @updateStatus, remarks = @remarks, checkedDate = @checkedDate, checkedBy = @checkedBy, checkerIP = @checkerIP  ");
            sql.AppendLine("where idoriginal = @idOriginal and updateStatus = @updateStatus1 ");

            MyParams.Add(new Params("@idOriginal", "GUID", id));
            MyParams.Add(new Params("@updateStatus", "NVARCHAR", "APPROVE"));
            MyParams.Add(new Params("@updateStatus1", "NVARCHAR", "CREATED"));
            MyParams.Add(new Params("@checkedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@checkedBy", "GUID", checkerID));
            MyParams.Add(new Params("@remarks", "NVARCHAR", remarks));
            MyParams.Add(new Params("@checkerIP", "NVARCHAR", ClientIp));
            ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    #endregion


    #region DataGeneral

    public DataTable GetMakerActionOnPending(Guid id)
    {
        DataTable ret = new DataTable();
        try
        {
            MyParams.Clear();
            string myQuery = @"
                                SELECT TBLNAME, ACTIONNAME, UPDATESTATUS, CHECKEDDATE, CHECKEDBY, NEWVALUES
                                    FROM TBLEDITING
                                    WHERE IDORIGINAL = :idOriginal 
                                    AND (UPDATESTATUS = 'CREATED' OR CHECKEDDATE IS NULL OR CHECKEDBY IS NULL)
                            "; //note actionname = maker action, updatestatus = latest data status

            MyParams.Add(new Params(":idOriginal", "GUID", id));
            ret = dbController.GetResult(myQuery, "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
        return ret;
    }

    #endregion
}