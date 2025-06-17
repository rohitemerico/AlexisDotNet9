using System.Data;
using Dashboard.Entities.ADCB;
using MDM.iOS.Entities.Dashboard;

//namespace MDM.iOS.Business.BusinessLogic.Dashboard.UserManagement;
public abstract class UserManageBase
{
    protected static System.DirectoryServices.DirectoryEntry entry;

    public abstract void getModulePermission(Guid userid, Guid moduleid, out bool View, out bool Maker, out bool Checker);

    public abstract bool isActDirAuthenticated(string adPath, string adDomain, string username, string pwd);//, out string path);

    public abstract bool LoginVerificationForSA(string username);

    public abstract bool isActDirUserExists(string username);

    public abstract DataTable getUserNamebyUserId(Guid id);

    //public abstract DataTable getUserNamebystaffID(Guid staffID);

    public abstract UIUserEn getAllUserDetailsReturnEntity(Guid userID);

    public abstract bool isUserExist(string username);

    public abstract bool isActiveUserAvailable(string username);

    public abstract bool setLogin(string sessionID, Guid userID);

    public abstract bool setLogout(Guid userID);

    public abstract bool setSuspend(Guid userID);

    public abstract int increaseLoginAttempt(int loginAttempt, Guid userID);

    public abstract DataTable getUsers(List<Params> myParams);

    public abstract UIUserEn getUserDetails(Guid userID);

    public abstract Guid getUserID(string username);

    public abstract List<UIUserEn> getUserIDBySupervisorID(Guid supervisorID);

    public abstract string getSessionID(Guid userID);

    public abstract bool insertUser(UIUserEn entity);

    public abstract bool updateUser(UIUserEn entity);


    public abstract bool isRoleExist(string roleName);

    public abstract DataTable getMenu();

    public abstract DataTable getRoles();

    public abstract DataTable getActiveRoles();

    public abstract UIRoleEn getRolesWithPermission(Guid roleID);

    public abstract bool insertRoles(UIRoleEn entity);

    public abstract bool insertRolesPermission(UIRoleEn entity);

    public abstract bool updateRoles(UIRoleEn entity);

    public abstract bool updateRolesPermission(UIRoleEn entity);

    public abstract bool deleteRoles(UIRoleEn entity);

    public abstract bool deleteRolesPermission(UIRoleEn entity);




    public abstract bool isBranchExist(string branchName);

    public abstract bool isBranchHasUserType(Guid branchID, string userType);

    public abstract DataTable getBranches(List<Params> list);

    public abstract DataTable getActiveBranches();

    public abstract UIBranchEn getBranch(Guid branchID);

    public abstract DataTable getBranchDT(Guid branchID);

    public abstract DataTable getDefaultBankBranch();

    public abstract bool insertBranch(UIBranchEn entity);

    public abstract bool updateBranch(UIBranchEn entity);

    public abstract bool deleteBranch(UIBranchEn entity);

    public abstract DataTable getBranchwithAssignRestriction(string? Params);

    public abstract DataTable getBankBranch_FilterbyMDMProfile(string branchID);

}
