using System.ComponentModel.DataAnnotations;
using System.Data;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Models;
using Dashboard.Common.Business.Component;
using Dashboard.Entities;
using Dashboard.Entities.ADCB.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Telerik.SvgIcons;

namespace Alexis.Dashboard.Pages.MachineMaintenance;

public class HopperMaintenanceAddModel(IHttpContextAccessor httpContextAccessor) : BasePageModel(httpContextAccessor)
{
    private HopperMaintenanceController hopperControl = new HopperMaintenanceController();

    public string PageTitleText { get; set; }
    public string ConfirmText { get; set; }
    public string ErrorText { get; set; }
    public bool ActiveVisible { get; set; } = false;
    public string? ClientIp { get; set; }
    public List<SelectListItem> CardTemplates { get; set; }

    [BindProperty]
    public string? HID { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please enter Hopper Name.")]
    [RegularExpression(@"^[ A-Za-z0-9_@.,-]*$", ErrorMessage = "Please enter valid character only.")]
    public string HopperName { get; set; }

    [BindProperty]
    public string? Remarks { get; set; }

    [BindProperty]
    public bool Status { get; set; }

    [BindProperty]
    public List<HopperViewModel> Hoppers { get; set; }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var forwardedHeader = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        ClientIp = forwardedHeader ?? HttpContext.Connection.RemoteIpAddress?.ToString();
        getUserDetails();
        if (accessLogin)
        {
            context.Result = new RedirectToPageResult("/Login");
            return;
        }
        checkAuthorization(ModuleLogAction.Create_Hopper_Template);
    }

    public IActionResult OnGet()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            BindDropDownList();
            BindData();
            if (session.GetString("EditHopperID") != null) //edit
            {
                HID = session.GetString("EditHopperID").ToString();
                ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
                PageTitleText = "Machine Hopper > Edit";
                EditCommand(HID);
            }
            else
            {
                ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
                PageTitleText = "Machine Hopper > Add New";
            }

        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        try
        {
            var session = httpContextAccessor.HttpContext.Session;
            BindDropDownList();
            if (ModelState.IsValid)
            {
                var HID = session.GetString("EditHopperID");
                if (hopperControl.IsHopperExist(HopperName) && HID is null)
                {

                    ErrorText = "The name is already exist.";
                    ModelState.AddModelError("HopperName", ErrorText);

                }
                else
                {
                    string[,] hopper = new string[8, 4];
                    int i = 0;
                    foreach (HopperViewModel item in Hoppers)
                    {
                        hopper[i, 0] = item.CardType;
                        hopper[i, 1] = item.Bin;
                        hopper[i, 2] = item.Mask;
                        var selectedItem = CardTemplates.FirstOrDefault(x => x.Value == item.CardType);
                        if (selectedItem != null)
                        {
                            hopper[i, 3] = selectedItem.Text;
                        }
                        i++;
                    }


                    MHopper entity = new MHopper();
                    entity.HID = (HID == null) ? Guid.NewGuid() : Guid.Parse(HID.ToString());
                    entity.HName = HopperName;
                    entity.HopperArray = hopper;
                    entity.Remarks = Remarks;
                    entity.Status = Status ? 1 : 2;
                    entity.CreatedBy = _UserId;
                    entity.ApprovedBy = _UserId;
                    entity.DeclineBy = _UserId;
                    entity.UpdatedBy = _UserId;
                    entity.CreatedDate = DateTime.Now;
                    entity.ApprovedDate = DateTime.Now;
                    entity.DeclineDate = DateTime.Now;
                    entity.UpdatedDate = DateTime.Now;

                    if (HID == null)
                    {
                        Create_Process(entity);
                    }
                    else
                    {
                        Update_Process(entity);
                    }
                }
            }

            BindData();
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return Page();
    }

    private void Create_Process(MHopper entity)
    {
        bool accessAudit = hopperControl.assignHopper(entity) && hopperControl.CreateData(entity, ClientIp);
        ErrorText = accessAudit ? "Successfully Created. Please Wait for Approval." : "Failed to Create.";

        AuditLog.CreateAuditLog(entity.HName + ": " + ErrorText, AuditCategory.Kiosk_Maintenance, ModuleLogAction.Create_Hopper_Template, _UserId, accessAudit, ClientIp);
        PopUp("Alert!", ErrorText);

        ConfirmText = CRUD.GetButtonText(CRUD_action.Create);
        PageTitleText = "Machine Hopper > Add New";
    }

    private void Update_Process(MHopper entity)
    {
        bool accessAudit = hopperControl.EditData(entity, ClientIp);
        ErrorText = accessAudit ? "Successfully Edited. Please Wait for Approval." : "Failed to Update.";
        AuditLog.CreateAuditLog(entity.HName + ": " + ErrorText, AuditCategory.Kiosk_Maintenance, ModuleLogAction.Update_Hopper_Template, _UserId, accessAudit, ClientIp);
        PopUp("Alert!", ErrorText);
        HttpContext.Session.Remove("EditHopperID");
        ConfirmText = CRUD.GetButtonText(CRUD_action.Update);
        PageTitleText = "Machine Hopper > Edit";
    }

    private void BindData()
    {
        Hoppers = [];
        for (int i = 1; i <= 8; i++)
        {
            Hoppers.Add(new HopperViewModel() { CardId = (i - 1).ToString(), Hopper = "Hopper " + i });
        }
    }

    private void BindDropDownList()
    {
        DataTable data = hopperControl.getCardList();
        List<CardViewModel> list = Common.DataHelper.ConvertDataTableToList<CardViewModel>(data);
        CardTemplates = [new SelectListItem { Text = "Please Select One", Value = "" }];
        foreach (CardViewModel card in list)
        {
            CardTemplates.Add(new SelectListItem { Text = card.cDesc, Value = card.cID });
        }
    }

    private void EditCommand(string HID)
    {
        try
        {
            DataTable[] arrayTB = hopperControl.getDetailByHopperID(Guid.Parse(HID.ToString()));
            HopperName = arrayTB[0].Rows[0]["hDesc"].ToString();
            Remarks = arrayTB[0].Rows[0]["hRemarks"].ToString();
            Status = arrayTB[0].Rows[0]["HStatus"].ToString() == "Active" ? true : false;
            int a = 0;
            foreach (HopperViewModel item in Hoppers)
            {
                item.CardType = arrayTB[1].Rows[a][2].ToString();
                item.Bin = arrayTB[1].Rows[a][4].ToString();
                item.Mask = arrayTB[1].Rows[a++][5].ToString();
            }
            ActiveVisible = true;
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    public IActionResult OnGetCancel()
    {
        HttpContext.Session.Remove("EditHopperID");
        return new JsonResult(new { message = "Success" });
    }

    public IActionResult OnPostUpdateCardData(string templateId)
    {
        try
        {
            if (string.IsNullOrEmpty(templateId) || templateId == "-1")
                return new JsonResult(new { success = false });

            var controller = new CardMaintenanceControl();
            var dt = controller.getCardInfoByID(Guid.Parse(templateId));
            if (dt.Rows.Count > 0)
            {
                return new JsonResult(new
                {
                    success = true,
                    bin = dt.Rows[0]["cBin"].ToString(),
                    mask = dt.Rows[0]["cMask"].ToString()
                });
            }
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
        return new JsonResult(new { success = false });
    }
}


public class HopperViewModel
{
    [BindProperty]
    public string? CardId { get; set; }

    [BindProperty]
    public string? Hopper { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Card Type is required.")]
    public string CardType { get; set; }
    [BindProperty]
    public string? Bin { get; set; }
    [BindProperty]
    public string? Mask { get; set; }
}