using System.Data;
using System.Text;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Dashboard.Entities.ADCB.Dashboard;

namespace Alexis.Dashboard.Controller;

public class DocumentSettingController : GlobalController
{

    //public DataTable getComponent()
    //{
    //    DataTable ret = new DataTable("DocumentType");
    //    try
    //    {
    //        sql.Clear();
    //        MyParams.Clear();
    //        sql.AppendLine("select cID, cDesc ");
    //        sql.AppendLine("from document_component dc ");
    //        sql.AppendLine("order by cDesc");
    //        ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
    //    }
    //    catch (Exception ex)
    //    {
    //        //Logger.LogToFile("getDocType.log", ex);
    //        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
    //                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
    //                    System.Reflection.MethodBase.GetCurrentMethod().Name,
    //                    ex);
    //    }
    //    return ret;
    //}

    //public DataTable getDocType()
    //{
    //    DataTable ret = new DataTable("DocumentType");
    //    try
    //    {
    //        sql.Clear();
    //        MyParams.Clear();
    //        sql.AppendLine("select cID, cDesc from Document_Component order by cDesc");
    //        DataTable tblComponent = dbController.GetResult(sql.ToString(), "connectionString", MyParams);

    //        ret.Columns.Add("DocumentType", typeof(string));
    //        ret.Columns.Add("ComponentGuid", typeof(Guid));
    //        ret.Columns.Add("Component", typeof(string));

    //        ret.Rows.Add("Passport", Guid.Parse(tblComponent.Rows[2][0].ToString()), tblComponent.Rows[2][1]);
    //        ret.Rows.Add("EIDA ID", Guid.Parse(tblComponent.Rows[1][0].ToString()), tblComponent.Rows[1][1]);
    //        ret.Rows.Add("ID", Guid.Parse(tblComponent.Rows[1][0].ToString()), tblComponent.Rows[1][1]);
    //        ret.Rows.Add("Income", Guid.Parse(tblComponent.Rows[0][0].ToString()), tblComponent.Rows[0][1]);
    //        ret.Rows.Add("Application Form", Guid.Parse(tblComponent.Rows[0][0].ToString()), tblComponent.Rows[0][1]);
    //        ret.Rows.Add("Employment Letter", Guid.Parse(tblComponent.Rows[0][0].ToString()), tblComponent.Rows[0][1]);
    //        ret.Rows.Add("Support Document", Guid.Parse(tblComponent.Rows[0][0].ToString()), tblComponent.Rows[0][1]);
    //    }
    //    catch (Exception ex)
    //    {
    //        //Logger.LogToFile("getDocType.log", ex);
    //        Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
    //                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
    //                    System.Reflection.MethodBase.GetCurrentMethod().Name,
    //                    ex);
    //    }
    //    return ret;
    //}

    public DataTable getDocSettingAll()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            //sql.AppendLine("select md.*, ");
            //sql.AppendLine("case md.dStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end docStatus, ");
            //sql.AppendLine("passDesc.cDesc Pass, eidaDesc.cDesc Eida, idDesc.cDesc ID, incomeDesc.cDesc Income, ");
            //sql.AppendLine("appDesc.cDesc App, empDesc.cDesc Emp, suppDesc.cDesc Supp, ");
            //sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy ");
            //sql.AppendLine("from Machine_Document md ");
            //sql.AppendLine("left join Document_Component passDesc on (md.dPassportComponent = passDesc.cID) ");
            //sql.AppendLine("left join Document_Component eidaDesc on (md.dEidaComponent = eidaDesc.cID) ");
            //sql.AppendLine("left join Document_Component idDesc on (md.dIdComponent = idDesc.cID) ");
            //sql.AppendLine("left join Document_Component incomeDesc on (md.dIncomeComponent = incomeDesc.cID) ");
            //sql.AppendLine("left join Document_Component appDesc on (md.dAppFormComponent = appDesc.cID) ");
            //sql.AppendLine("left join Document_Component empDesc on (md.dEmploymentComponent = empDesc.cID) ");
            //sql.AppendLine("left join Document_Component suppDesc on (md.dSupportDocComponent = suppDesc.cID) ");
            //sql.AppendLine("left join User_Login ulc on (md.dCreatedBy = ulc.aID) ");
            //sql.AppendLine("left join User_Login ula on (md.dApprovedBy = ula.aID) ");
            //sql.AppendLine("left join User_Login uld on (md.dDeclineBy = uld.aID) ");
            sql.AppendLine("select md.*, mds.*, dt.dName, ");
            sql.AppendLine("case md.dStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end docStatus, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy, ulu.uname UpdatedBy ");
            sql.AppendLine("from machine_document md  ");
            sql.AppendLine("join machine_document_settings mds on (md.did = mds.ref_did) ");
            sql.AppendLine("left join doctype_setup dt on (mds.doctypeid = dt.did) ");
            sql.AppendLine("left join User_Login ulc on (md.dCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (md.dApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (md.dDeclineBy = uld.aID) ");
            sql.AppendLine("left join User_Login ulu on (md.dUpdatedBy = ulu.aID) ");
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getDocSettingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public DataTable getDocSettingById(Guid docID)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            //sql.AppendLine("select md.*, ");
            //sql.AppendLine("case md.dStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end docStatus, ");
            //sql.AppendLine("passDesc.cDesc Pass, eidaDesc.cDesc Eida, idDesc.cDesc ID, incomeDesc.cDesc Income, ");
            //sql.AppendLine("appDesc.cDesc App, empDesc.cDesc Emp, suppDesc.cDesc Supp, ulCreate.uName Creator ");
            //sql.AppendLine("from Machine_Document md ");
            //sql.AppendLine("left join Document_Component passDesc on (md.dPassportComponent = passDesc.cID) ");
            //sql.AppendLine("left join Document_Component eidaDesc on (md.dEidaComponent = eidaDesc.cID) ");
            //sql.AppendLine("left join Document_Component idDesc on (md.dIdComponent = idDesc.cID) ");
            //sql.AppendLine("left join Document_Component incomeDesc on (md.dIncomeComponent = incomeDesc.cID) ");
            //sql.AppendLine("left join Document_Component appDesc on (md.dAppFormComponent = appDesc.cID) ");
            //sql.AppendLine("left join Document_Component empDesc on (md.dEmploymentComponent = empDesc.cID) ");
            //sql.AppendLine("left join Document_Component suppDesc on (md.dSupportDocComponent = suppDesc.cID) ");
            //sql.AppendLine("left join User_Login ulCreate on (md.dCreatedBy = ulCreate.aID) ");
            sql.AppendLine("select md.*, mds.*, dt.dName, ");
            sql.AppendLine("case md.dStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end docStatus, ");
            sql.AppendLine("ulc.uName CreatedBy, ula.uName ApprovedBy, uld.uName DeclinedBy ");
            sql.AppendLine("from machine_document md  ");
            sql.AppendLine("join machine_document_settings mds on (md.did = mds.ref_did) ");
            sql.AppendLine("left join doctype_setup dt on (mds.doctypeid = dt.did) ");
            sql.AppendLine("left join User_Login ulc on (md.dCreatedBy = ulc.aID) ");
            sql.AppendLine("left join User_Login ula on (md.dApprovedBy = ula.aID) ");
            sql.AppendLine("left join User_Login uld on (md.dDeclineBy = uld.aID) ");
            sql.AppendLine("where md.dId=@dID ");
            sql.AppendLine("order by dt.dCreatedDate ");
            MyParams.Add(new Params("@dID", "GUID", docID));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getDocSettingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public DataTable getDocTypeAll()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select ds.*, ");//, dc.cdesc, ");
            sql.AppendLine("Case ds.dStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end txtStatus, ");
            sql.AppendLine("cUL.uName CreatedBy, aUL.uName ApprovedBy, dUL.uName DeclineBy, uul.uname UpdatedBy ");
            sql.AppendLine("from doctype_setup ds ");//join document_component dc on (ds.cComponentId = DC.CID) ");
            sql.AppendLine("left join User_Login cUL on (ds.dCreatedBy = cUL.aID) ");
            sql.AppendLine("left join User_Login aUL on (ds.dApprovedBy = aUL.aID) ");
            sql.AppendLine("left join User_Login dUL on (ds.dDeclineBy = dUL.aID) ");
            sql.AppendLine("left join User_Login uUL on (ds.dUpdatedBy = uUL.aID) ");
            sql.AppendLine("order by dCreatedDate ");
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getDocSettingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public DataTable getDocTypeList()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select ds.*, ");//, dc.cdesc, ");
            sql.AppendLine("Case ds.dStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end txtStatus, ");
            sql.AppendLine("cUL.uName CreatedBy, aUL.uName ApprovedBy, dUL.uName DeclineBy ");
            sql.AppendLine("from doctype_setup ds ");//join document_component dc on (ds.cComponentId = DC.CID) ");
            sql.AppendLine("left join User_Login cUL on (ds.dCreatedBy = cUL.aID) ");
            sql.AppendLine("left join User_Login aUL on (ds.dApprovedBy = aUL.aID) ");
            sql.AppendLine("left join User_Login dUL on (ds.dDeclineBy = dUL.aID) ");
            //sql.AppendLine("where ds.dStatus = @dStatus ");
            sql.AppendLine("order by dCreatedDate ");
            //MyParams.Add(new Params("@dStatus", "INT", 1));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getDocSettingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }


    public DataTable getActDocTypeList()
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select ds.*, ");//, dc.cdesc, ");
            sql.AppendLine("Case ds.dStatus when 0 then 'Pending' when 1 then 'Active' when 2 then 'Inactive' end txtStatus ");
            //sql.AppendLine("cUL.uName CreatedBy, aUL.uName ApprovedBy, dUL.uName DeclineBy ");
            sql.AppendLine("from doctype_setup ds ");//join document_component dc on (ds.cComponentId = DC.CID) ");

            sql.AppendLine("where ds.dStatus = 1 ");
            sql.AppendLine("order by dCreatedDate ");
            //MyParams.Add(new Params("@dStatus", "INT", 1));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getDocSettingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public DataTable getDocTypeById(Guid docID)
    {
        DataTable ret = new DataTable();
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select ds.* ");//, dc.cdesc, ");
            sql.AppendLine("from doctype_setup ds ");//join document_component dc on (ds.cComponentId = DC.CID) ");
            sql.AppendLine("where ds.dId=@dID ");
            //sql.AppendLine("left join User_Login ulc on (md.dCreatedBy = ulc.aID) ");
            //sql.AppendLine("left join User_Login ula on (md.dApprovedBy = ula.aID) ");
            //sql.AppendLine("left join User_Login uld on (md.dDeclineBy = uld.aID) ");
            MyParams.Add(new Params("@dID", "GUID", docID));
            ret = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("getDocSettingAll.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool isDocumentTemplateExist(string tempName)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from Machine_Document where dDesc = @dDesc");
            MyParams.Add(new Params("@dDesc", "NVARCHAR", tempName));
            if (dbController.GetResult(sql.ToString(), "connectionString", MyParams).Rows.Count != 0)
                ret = true;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("isDocumentTemplateExist.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool isDocumentTypeExist(string tempName)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from DocType_Setup where dName = @dName");
            MyParams.Add(new Params("@dName", "NVARCHAR", tempName));
            if (dbController.GetResult(sql.ToString(), "connectionString", MyParams).Rows.Count != 0)
                ret = true;
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("isDocumentTemplateExist.log", ex);
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
            sql.AppendLine("select md.dDesc, dt.* ");
            sql.AppendLine("from machine_document md ");
            sql.AppendLine("join machine_document_settings mds on (md.did = mds.ref_did) ");
            sql.AppendLine("join doctype_setup dt on (mds.doctypeid = dt.did) ");
            sql.AppendLine("where dt.did = @did and md.dStatus = @status ");

            MyParams.Add(new Params("@did", "GUID", id));
            MyParams.Add(new Params("@status", "INT", 1));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            foreach (DataRow dr in dt.Rows)
                ret += "The DocType is assigned to Document Setting, " + dr["dDesc"].ToString() + "! The DocType Cannot be Inactive!";
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

    public string IsValidatedToInactive_Document(Guid id)
    {
        string ret = "";
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("select * from machine_group mg ");
            sql.AppendLine("join machine_document md on (mg.kDocumentID = md.did) ");
            sql.AppendLine("where md.did = @id and mg.kStatus = @status ");

            MyParams.Add(new Params("@id", "GUID", id));
            MyParams.Add(new Params("@status", "INT", 1));
            DataTable dt = dbController.GetResult(sql.ToString(), "connectionString", MyParams);
            foreach (DataRow dr in dt.Rows)
                ret += "The Document is assigned to Machine Group, " + dr["kDesc"].ToString() + "! The Document Cannot be Inactive! <br />";
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

    public bool CreateDocument(MDocument entity)
    {
        bool ret = false;
        try
        {
            //sql.Clear();
            //MyParams.Clear();
            //sql.AppendLine("insert into Machine_Document ");
            //sql.AppendLine("(dID, dDesc, ");
            ////sql.AppendLine("dPassportComponent, dEidaComponent, dIdComponent, dIncomeComponent, ");
            ////sql.AppendLine("dAppFormComponent, dEmploymentComponent, dSupportDocComponent, ");
            ////sql.AppendLine("dPassportSwallow, dEidaSwallow, dIdSwallow, dIncomeSwallow, ");
            ////sql.AppendLine("dAppFormSwallow, dEmploymentDocSwallow, dSupportDocSwallow, ");
            ////sql.AppendLine("dPassportPrint, dEidaPrint, dIdPrint, dIncomePrint, ");
            ////sql.AppendLine("dAppFormPrint, dEmploymentDocPrint, dSupportDocPrint, ");
            //sql.AppendLine("dCreatedDate, dCreatedBy, dRemarks, dStatus) ");
            //sql.AppendLine("values (@dID, @dDesc, ");//@dPass, @dEida, @dIdCom, @dIncome, @dApp, @dEmploy, @dSupport, ");
            ////sql.AppendLine("@sPass, @sEida, @sId, @sIncome, @sApp, @sEmploy, @sSupport, ");
            ////sql.AppendLine("@pPass, @pEida, @pId, @pIncome, @pApp, @pEmploy, @pSupport, ");
            //sql.AppendLine("@dDate, @dBy, @dRemarks, @dStatus) ");
            //MyParams.Add(new Params("@dID", "GUID", entity.DID));
            //MyParams.Add(new Params("@dDesc", "NVARCHAR", entity.DName));
            ////MyParams.Add(new Params("@dPass", "GUID", Guid.Parse(entity.DocComponent[1])));
            ////MyParams.Add(new Params("@dEida", "GUID", Guid.Parse(entity.DocComponent[4])));
            ////MyParams.Add(new Params("@dIdCom", "GUID", Guid.Parse(entity.DocComponent[7])));
            ////MyParams.Add(new Params("@dIncome", "GUID", Guid.Parse(entity.DocComponent[10])));
            ////MyParams.Add(new Params("@dApp", "GUID", Guid.Parse(entity.DocComponent[13])));
            ////MyParams.Add(new Params("@dEmploy", "GUID", Guid.Parse(entity.DocComponent[16])));
            ////MyParams.Add(new Params("@dSupport", "GUID", Guid.Parse(entity.DocComponent[19])));
            ////MyParams.Add(new Params("@sPass", "BIT", Convert.ToBoolean(entity.DocComponent[2])));
            ////MyParams.Add(new Params("@sEida", "BIT", Convert.ToBoolean(entity.DocComponent[5])));
            ////MyParams.Add(new Params("@sId", "BIT", Convert.ToBoolean(entity.DocComponent[8])));
            ////MyParams.Add(new Params("@sIncome", "BIT", Convert.ToBoolean(entity.DocComponent[11])));
            ////MyParams.Add(new Params("@sApp", "BIT", Convert.ToBoolean(entity.DocComponent[14])));
            ////MyParams.Add(new Params("@sEmploy", "BIT", Convert.ToBoolean(entity.DocComponent[17])));
            ////MyParams.Add(new Params("@sSupport", "BIT", Convert.ToBoolean(entity.DocComponent[20])));
            ////MyParams.Add(new Params("@pPass", "BIT", Convert.ToBoolean(entity.DocComponent[3])));
            ////MyParams.Add(new Params("@pEida", "BIT", Convert.ToBoolean(entity.DocComponent[6])));
            ////MyParams.Add(new Params("@pId", "BIT", Convert.ToBoolean(entity.DocComponent[9])));
            ////MyParams.Add(new Params("@pIncome", "BIT", Convert.ToBoolean(entity.DocComponent[12])));
            ////MyParams.Add(new Params("@pApp", "BIT", Convert.ToBoolean(entity.DocComponent[15])));
            ////MyParams.Add(new Params("@pEmploy", "BIT", Convert.ToBoolean(entity.DocComponent[18])));
            ////MyParams.Add(new Params("@pSupport", "BIT", Convert.ToBoolean(entity.DocComponent[21])));
            //MyParams.Add(new Params("@dDate", "DATETIME", entity.CreatedDate));
            //MyParams.Add(new Params("@dBy", "GUID", entity.CreatedBy));
            //MyParams.Add(new Params("@dRemarks", "NVARCHAR", entity.Remarks));
            //MyParams.Add(new Params("@dStatus", "INT", 0));//entity.Status));

            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("insert into Machine_Document ");
            sql.AppendLine("(dID, dDesc, ");
            sql.AppendLine("dCreatedDate, dCreatedBy, dRemarks, dStatus) ");
            sql.AppendLine("values (@dID, @dDesc, ");
            sql.AppendLine("@dDate, @dBy, @dRemarks, @dStatus) ");
            MyParams.Add(new Params("@dID", "GUID", entity.DID));
            MyParams.Add(new Params("@dDesc", "NVARCHAR", entity.DName));
            MyParams.Add(new Params("@dDate", "DATETIME", entity.CreatedDate));
            MyParams.Add(new Params("@dBy", "GUID", entity.CreatedBy));
            MyParams.Add(new Params("@dRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@dStatus", "INT", 0));//entity.Status));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
            {
                for (int i = 1; i < entity.DocComponent.Count; i++)
                {
                    sql.Clear();
                    MyParams.Clear();
                    sql.AppendLine("insert into Machine_Document_Settings ");
                    sql.AppendLine("(Ref_Did, doctypeID, Swallow, [Print]) ");
                    sql.AppendLine("values (@Ref_Did, @doctypeID, @Swallow, @Print)");
                    MyParams.Add(new Params("@Ref_Did", "NVARCHAR", entity.DID.ToString()));
                    MyParams.Add(new Params("@doctypeID", "NVARCHAR", entity.DocComponent[i++].ToString()));
                    i++;
                    MyParams.Add(new Params("@Swallow", "BIT", Convert.ToBoolean(entity.DocComponent[i++])));
                    MyParams.Add(new Params("@Print", "BIT", Convert.ToBoolean(entity.DocComponent[i])));

                    ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
                }
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("CreateDocument.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool CreateDocType(MDocument entity)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("insert into DocType_Setup ");
            sql.AppendLine("(dID, dName, cComponentId, dRemarks, dStatus, dCreatedDate, dCreatedBy) ");
            sql.AppendLine("values (@dID, @dName, @cComponentId, @dRemarks, @dStatus, @dCreatedDate, @dCreatedBy) ");
            MyParams.Add(new Params("@dID", "GUID", entity.DID));
            MyParams.Add(new Params("@dName", "NVARCHAR", entity.DName));
            MyParams.Add(new Params("@cComponentId", "NVARCHAR", entity.ComponentID));
            MyParams.Add(new Params("@dRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@dStatus", "INT", 0));//entity.Status));
            MyParams.Add(new Params("@dCreatedDate", "DATETIME", entity.CreatedDate));
            MyParams.Add(new Params("@dCreatedBy", "GUID", entity.CreatedBy));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;


        }
        catch (Exception ex)
        {
            //Logger.LogToFile("CreateDocument.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool UpdateDocument(MDocument entity)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update Machine_Document set ");
            sql.AppendLine("dUpdatedDate=@dDate , dUpdatedBy=@dBy, dRemarks=@dRemarks, dStatus=@dStatus ");
            sql.AppendLine("where dID=@dID ");
            MyParams.Add(new Params("@dID", "GUID", entity.DID));
            MyParams.Add(new Params("@dDesc", "NVARCHAR", entity.DName));
            MyParams.Add(new Params("@dDate", "DATETIME", entity.UpdatedDate));
            MyParams.Add(new Params("@dBy", "GUID", entity.UpdatedBy));
            MyParams.Add(new Params("@dRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@dStatus", "INT", entity.Status));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
            {
                sql.Clear();
                MyParams.Clear();
                sql.AppendLine("delete from Machine_Document_Settings where Ref_Did = @Ref_Did ");
                MyParams.Add(new Params("@Ref_Did", "GUID", entity.DID));

                if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                {
                    for (int i = 1; i < entity.DocComponent.Count; i++)
                    {
                        sql.Clear();
                        MyParams.Clear();
                        sql.AppendLine("insert into Machine_Document_Settings ");
                        sql.AppendLine("(Ref_Did, doctypeID, Swallow, [Print] ");
                        sql.AppendLine(") values (");
                        sql.AppendLine("@Ref_Did, @doctypeID, @Swallow, @Print) ");
                        MyParams.Add(new Params("@Ref_Did", "GUID", entity.DID));
                        MyParams.Add(new Params("@doctypeID", "NVARCHAR", entity.DocComponent[i++]));
                        MyParams.Add(new Params("@Swallow", "BIT", Convert.ToBoolean(Convert.ToInt32(entity.DocComponent[i++]))));
                        MyParams.Add(new Params("@Print", "BIT", Convert.ToBoolean(Convert.ToInt32(entity.DocComponent[i]))));

                        ret = dbController.Input(sql.ToString(), "connectionString", MyParams);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UpdateDocument.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool UpdateDocType(MDocument entity)
    {
        bool ret = false;
        try
        {
            sql.Clear();
            MyParams.Clear();
            sql.AppendLine("update DocType_Setup set ");
            sql.AppendLine("dName = @dName, ");
            sql.AppendLine("cComponentId = @cComponentId, ");
            sql.AppendLine("dRemarks = @dRemarks, ");
            sql.AppendLine("dStatus = @dStatus, ");
            sql.AppendLine("dUpdatedDate = @dUpdatedDate, ");
            sql.AppendLine("dUpdatedBy = @dUpdatedBy ");
            sql.AppendLine("where dID = @dID");

            MyParams.Add(new Params("@dID", "GUID", entity.DID));
            MyParams.Add(new Params("@dName", "NVARCHAR", entity.DName));
            MyParams.Add(new Params("@cComponentId", "NVARCHAR", entity.ComponentID));
            MyParams.Add(new Params("@dRemarks", "NVARCHAR", entity.Remarks));
            MyParams.Add(new Params("@dStatus", "INT", entity.Status));
            MyParams.Add(new Params("@dUpdatedDate", "DATETIME", entity.UpdatedDate));
            MyParams.Add(new Params("@dUpdatedBy", "GUID", entity.UpdatedBy));

            if (dbController.Input(sql.ToString(), "connectionString", MyParams))
                ret = true;

        }
        catch (Exception ex)
        {
            //Logger.LogToFile("UpdateDocument.log", ex);
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return ret;
    }

    public bool approveDoc(Guid docID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update Machine_Document set ");
            sql.AppendLine("dStatus=@dStatus, dApprovedDate=@dApprovedDate, dApprovedBy=@dApprovedBy");
            sql.AppendLine("where dID=@dID ");

            MyParams.Add(new Params("@dID", "GUID", docID));
            MyParams.Add(new Params("@dStatus", "INT", 1));
            MyParams.Add(new Params("@dApprovedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@dApprovedBy", "GUID", adminGuid));

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

    public bool approveDocType(Guid docID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update DocType_Setup set ");
            sql.AppendLine("dStatus=@dStatus, dApprovedDate=@dApprovedDate, dApprovedBy=@dApprovedBy");
            sql.AppendLine("where dID=@dID ");

            MyParams.Add(new Params("@dID", "GUID", docID));
            MyParams.Add(new Params("@dStatus", "INT", 1));
            MyParams.Add(new Params("@dApprovedDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@dApprovedBy", "GUID", adminGuid));

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

    public bool declineDoc(Guid docID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update Machine_Document set ");
            sql.AppendLine("dStatus=@dStatus, dDeclineDate=@dDeclineDate, dDeclineBy=@dDeclineBy");
            sql.AppendLine("where dID=@dID ");

            MyParams.Add(new Params("@dID", "GUID", docID));
            MyParams.Add(new Params("@dStatus", "INT", 2));
            MyParams.Add(new Params("@dDeclineDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@dDeclineBy", "GUID", adminGuid));

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

    public bool declineDocType(Guid docID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("update DocType_Setup set ");
            sql.AppendLine("dStatus=@dStatus, dDeclineDate=@dDeclineDate, dDeclineBy=@dDeclineBy");
            sql.AppendLine("where dID=@dID ");

            MyParams.Add(new Params("@dID", "GUID", docID));
            MyParams.Add(new Params("@dStatus", "INT", 2));
            MyParams.Add(new Params("@dDeclineDate", "DATETIME", DateTime.Now));
            MyParams.Add(new Params("@dDeclineBy", "GUID", adminGuid));

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

    public bool deleteDoc(Guid docID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("delete Machine_Document ");
            sql.AppendLine("where dID=@dID ");

            MyParams.Add(new Params("@dID", "GUID", docID));

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

    public bool deleteDocType(Guid docID, Guid adminGuid)
    {
        bool ret = false;
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> MyParams = new List<Params>();
            sql.AppendLine("delete DocType_Setup ");
            sql.AppendLine("where dID=@dID ");

            MyParams.Add(new Params("@dID", "GUID", docID));

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
