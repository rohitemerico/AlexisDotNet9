using System.Data;
using System.Text;
using Alexis.Dashboard.Controller;
using Alexis.Dashboard.Helper;
using Dashboard.Common.Business.Component;
using Dashboard.Common.Data.Component;
using Dashboard.Entities.ADCB;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alexis.Dashboard.Models;

public abstract class BasePageModel : PageModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserRoleController userControl = new UserRoleController();
    public bool View, Maker, Checker;
    public DataTable UserDetails { get; private set; }

    public Guid _UserId = Guid.Empty;
    public bool accessLogin = false;

    public BasePageModel(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void getUserDetails()
    {
        try
        {
            var session = _httpContextAccessor.HttpContext.Session;

            // Retrieve session data
            UserDetails = SessionHelper.GetObject<DataTable>(session, "User_Det");

            if (UserDetails == null)
            {
                session.Clear();
                accessLogin = true;
            }
            else
            {
                if (UserDetails.Rows.Count > 0)
                {
                    _UserId = new Guid(UserDetails.Rows[0]["aid"].ToString());
                    ViewData["User"] = "Welcome, " + UserDetails.Rows[0]["uname"].ToString();
                    ViewData["LastLogin"] = "Last Login on " + UserDetails.Rows[0]["ULASTLOGINDATE"].ToString();
                    BindMenus(UserDetails);
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
    }

    public void getModulePermissionByModuleID(Guid moduleid)
    {
        try
        {
            UserRoleController.getModulePermission(Guid.Parse(UserDetails.Rows[0][0].ToString()), moduleid, out View, out Maker, out Checker);
        }
        catch (Exception ex)
        {
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                               System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                               System.Reflection.MethodBase.GetCurrentMethod().Name,
                               ex);
            HPLog.WriteLog(ex.Message, "ERROR");
        }
    }

    public bool checkAuthorization(ModuleLogAction auth)
    {
        Guid moduleId = Module.GetModuleId(auth);
        getModulePermissionByModuleID(moduleId);
        return View;
    }

    List<string> MenuLayer3ToExclude = new List<string>
    {
        //Module.GetModuleId(ModuleLogAction.Create_UserMaintenance).ToString(), /*user add_edit*/
        //Module.GetModuleId(ModuleLogAction.Create_RoleMaintenance).ToString(), /*role add_edit*/
        //Module.GetModuleId(ModuleLogAction.Create_Branch).ToString(), /*branch add_edit*/
        //Module.GetModuleId(ModuleLogAction.Create_Advertisement).ToString() /*advertisement add_edit*/
    };
    private class MenuType
    {
        public static string MainMenu { get => "0"; }
        public static string SubMenu1 { get => "2"; }
        public static string SubMenu2_PageContent { get => "1"; }
    }
    protected void BindMenus(DataTable detMenu)
    {
        try
        {
            StringBuilder sql = new StringBuilder();
            List<Params> myParams = new List<Params>();

            sql.Append("select m.mid, m.mdesc, m.mPath, m.mgroup, m.mseq, m.mtype, up.mview, up.mmaker, up.mChecker ");
            sql.Append("from user_login ul, User_Roles ur, User_Permission up, menu m ");
            sql.Append("where ul.rid = ur.rid ");
            sql.Append("and ur.rID = up.rID ");
            sql.Append("and up.mID = m.mID ");
            sql.Append("and ur.rStatus = 1 "); /*approved role*/
            sql.Append("and up.mview = 1 "); /*allowed to view*/
            sql.Append("and ul.aid = :userID ");
            sql.Append("order by m.mgroup, m.mseq ");
            myParams.Add(new Params(":userID", "NVARCHAR", detMenu.Rows[0]["aid"]));
            detMenu = dbController.GetResult(sql.ToString(), "connectionString", myParams);

            string prevGroupMenu = "";
            string currGroupMenu = "";
            string nextGroupMenu = "";
            bool CreatingCaretGroupSub = false;
            StringBuilder menuBuilder = new StringBuilder();

            // loop all in oracle db
            for (int i = 0; i < detMenu.Rows.Count; i++)
            {
                currGroupMenu = detMenu.Rows[i]["mgroup"].ToString();
                nextGroupMenu = (i + 1 == detMenu.Rows.Count) ? "" : detMenu.Rows[i + 1]["mgroup"].ToString();

                if (currGroupMenu != prevGroupMenu) //if new group
                {
                    var caretGroupId = detMenu.Rows[i]["mgroup"].ToString() + "_" + detMenu.Rows[i]["mseq"].ToString();
                    var caretGroupName = detMenu.Rows[i]["mdesc"].ToString();

                    //check if new group has sub menu
                    if (nextGroupMenu != currGroupMenu) //no sub menu
                    {
                        if (!String.IsNullOrEmpty(detMenu.Rows[i]["mpath"].ToString()))
                            menuBuilder.AppendLine("<li class=\"menu_layer1\"><a href ="
                                + @Url.Content($"~/{detMenu.Rows[i]["mpath"]}")
                                + ">"
                                + detMenu.Rows[i]["mdesc"].ToString()
                                + "</a></li> ");
                        else
                            menuBuilder.AppendLine("<li class=\"menu_layer1\"><a href =\"#\">"
                                + detMenu.Rows[i]["mdesc"].ToString()
                                + "</a></li> ");
                    }
                    else //has sub menu
                    {
                        menuBuilder.AppendLine("<li class=\"menu_layer1\"><a href = \"#submenu_" + caretGroupId + "\" data-toggle=\"collapse\" aria-expanded=\"true\" aria-controls=\"collapseExample\">"
                            + caretGroupName + "<i class=\"fa fa-chevron-right\" style=\"float: right\"></i><i class=\"fa fa-chevron-down\" style=\"float: right;\"></i></a>");
                        menuBuilder.AppendLine("<ul class=\"collapse list-unstyled\" id=\"submenu_" + caretGroupId + "\" > "); //Note, opening ul tag 

                    }
                }
                else  // if same group
                {
                    if (!MenuLayer3ToExclude.Contains(detMenu.Rows[i]["mid"].ToString()))
                    {
                        var caretGroupSubId = detMenu.Rows[i]["mgroup"].ToString() + "_" + detMenu.Rows[i]["mseq"].ToString();
                        var caretGroupSubName = detMenu.Rows[i]["mdesc"].ToString();
                        if (detMenu.Rows[i]["mtype"].ToString() == MenuType.SubMenu1) //MainMenu > [SubMenu1] > SubMenu2_PageContent
                        {
                            CreatingCaretGroupSub = true;

                            menuBuilder.AppendLine("<li class=\"menu_layer2\"><a href = \"#submenu_" + caretGroupSubId + "\" data-toggle=\"collapse\" aria-expanded=\"true\" aria-controls=\"collapseExample\">"
                                + caretGroupSubName + "<i class=\"fa fa-chevron-right\" style=\"float: right\"></i><i class=\"fa fa-chevron-down\" style=\"float: right;\"></i></a>");
                            menuBuilder.AppendLine("<ul class=\"collapse list-unstyled\" id=\"submenu_" + caretGroupSubId + "\" > "); //Note, opening ul tag 
                        }
                        else //MainMenu > SubMenu2_PageContent 
                        {
                            if (!CreatingCaretGroupSub) //MainMenu > [SubMenu2_PageContent]
                                menuBuilder.AppendLine("<li class=\"menu_layer2\"><a href =" + @Url.Content($"~/{detMenu.Rows[i]["mpath"]}") + ">" + detMenu.Rows[i]["mdesc"].ToString() + "</a></li>");
                            else //MainMenu > SubMenu1 > [SubMenu2_PageContent]
                                menuBuilder.AppendLine("<li class=\"menu_layer3\"><a href ="
                                + @Url.Content($"~/{detMenu.Rows[i]["mpath"]}")
                                + ">"
                                + detMenu.Rows[i]["mdesc"].ToString()
                                + "</a></li>");
                        }
                    }
                    if (detMenu.Rows[i + 1]["mtype"].ToString() != MenuType.SubMenu2_PageContent && CreatingCaretGroupSub)
                    {
                        menuBuilder.AppendLine("</ul>"); //closing ul for sub caret
                        CreatingCaretGroupSub = false; //reset
                    }

                    if (nextGroupMenu != currGroupMenu)
                        menuBuilder.AppendLine("</ul>"); //closing ul for main caret
                }

                prevGroupMenu = currGroupMenu;
            }

            ViewData["SideBar_Menu"] = menuBuilder.ToString();
        }
        catch (Exception ex)
        {
            //Logger.LogToFile("Master_BindMenu.log", ex);
            HPLog.WriteLog(ex.Message, "ERROR");
            Logger.LogToFile(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        ex);
        }
    }

    public void PopUp(string caption, string message)
    {
        TempData["ModalTitle"] = caption;
        TempData["ModalMessage"] = message;
        TempData["ShowModal"] = true;
    }

    public void SaveUploadToFile(string path, IFormFile upload)
    {
        using (FileStream stream = new(path, FileMode.Create))
        {
            upload.CopyTo(stream);
        }
    }

    public static string CreatedDirIfNotExist(string directory)
    {
        if (!String.IsNullOrEmpty(directory))
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

        return directory;
    }
}
