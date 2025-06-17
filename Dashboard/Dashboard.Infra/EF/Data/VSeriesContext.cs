using Dashboard.Infra.EF.Models;
using Dashboard.Infra.EF.Models.Ad;
using Dashboard.Infra.EF.Models.iOS;
using Dashboard.Infra.EF.Models.Reporting;
using Dashboard.Infra.EF.Models.UserRoleBranch;
using MDM.Windows.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Infra.EF.Data;

public partial class VSeriesContext : DbContext
{

    public VSeriesContext(DbContextOptions<VSeriesContext> options) : base(options) { }

    //public VSeriesContext() : base("name=ModelContext")
    //{
    //}
    #region windows mdm
    public DbSet<WinEnrollment> WinEnrollments { get; set; }

    public DbSet<WinDev> WinDevs { get; set; }
    public DbSet<WinDevDetail> WinDevDetails { get; set; }
    public DbSet<WinDevStatus> WinDevStatuses { get; set; }
    public DbSet<WinDevLocation> WinDevLocations { get; set; }
    public DbSet<WinProfile> WinProfiles { get; set; }

    public DbSet<WinDevProfileAssignment> WinDevProfileAssignments { get; set; }
    #endregion
    public virtual DbSet<tbl_app_mob_translog> tbl_app_mob_translog { get; set; }
    public virtual DbSet<tbl_app_msf_translog> tbl_app_msf_translog { get; set; }
    public virtual DbSet<MACHINE_ADVERTISEMENT> MACHINE_ADVERTISEMENT { get; set; }
    public virtual DbSet<ANDROIDMDM_APPLICATION> ANDROIDMDM_APPLICATION { get; set; }
    public virtual DbSet<LOCALUSER> LOCALUSERs { get; set; }
    public virtual DbSet<MACHINE> MACHINEs { get; set; }
    public virtual DbSet<MDM_HttpProxy> MDM_HttpProxy { get; set; }
    public virtual DbSet<MDM_InstalledApplicationList> MDM_InstalledApplicationList { get; set; }
    public virtual DbSet<MDM_InstalledApps> MDM_InstalledApps { get; set; }
    public virtual DbSet<MDM_Profile> MDM_Profile { get; set; }
    public virtual DbSet<MDM_Profile_Cellular> MDM_Profile_Cellular { get; set; }
    public virtual DbSet<MDM_Profile_General> MDM_Profile_General { get; set; }
    public virtual DbSet<MDM_Profile_General_BranchID> MDM_Profile_General_BranchID { get; set; }
    public virtual DbSet<MDM_Profile_Passcode> MDM_Profile_Passcode { get; set; }
    public virtual DbSet<MDM_Profile_Restriction> MDM_Profile_Restriction { get; set; }
    public virtual DbSet<MDM_Profile_Restriction_Advance> MDM_Profile_Restriction_Advance { get; set; }
    public virtual DbSet<MDM_Profile_VPN> MDM_Profile_VPN { get; set; }
    public virtual DbSet<MDM_Profile_WIFI> MDM_Profile_WIFI { get; set; }
    public virtual DbSet<MDM_Restriction_Menu> MDM_Restriction_Menu { get; set; }
    public virtual DbSet<MENU> MENUs { get; set; }
    public virtual DbSet<TBLAGENTVIDEO> TBLAGENTVIDEOs { get; set; }
    public virtual DbSet<tblMachine> tblMachines { get; set; }
    public virtual DbSet<TblMonitoringDevices> TblMonitoringDevices { get; set; }
    public virtual DbSet<TRANSACTION_LOG> TRANSACTION_LOG { get; set; }
    public virtual DbSet<User_Branch> User_Branch { get; set; }
    public virtual DbSet<USER_LOGIN> USER_LOGIN { get; set; }
    public virtual DbSet<USER_ROLES> USER_ROLES { get; set; }
    public virtual DbSet<MDM_Certificates> MDM_Certificates { get; set; }
    public virtual DbSet<MDM_Commands> MDM_Commands { get; set; }
    public virtual DbSet<MDM_OS_Updates> MDM_OS_Updates { get; set; }
    public virtual DbSet<MDM_Pending_OS_Updates> MDM_Pending_OS_Updates { get; set; }
    public virtual DbSet<MDM_Profile_CertFile> MDM_Profile_CertFile { get; set; }
    public virtual DbSet<MDM_Profile_ServicesURL> MDM_Profile_ServicesURL { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Configure WinDevID as FK for WinDevDetail
        //modelBuilder.Entity<WinDev>()
        //    .HasRequired(w => w.WinDevDetail)
        //    .WithRequiredPrincipal(w => w.WinDev)
        //    .WillCascadeOnDelete();
        //// Configure WinDevID as FK for WinDevStatus
        //modelBuilder.Entity<WinDev>()
        //    .HasRequired(w => w.WinDevStatus)
        //    .WithRequiredPrincipal(w => w.WinDev)
        //    .WillCascadeOnDelete();
        //// Configure WinDevID as FK for WinDevLocation
        //modelBuilder.Entity<WinDev>()
        //    .HasRequired(w => w.WinDevLocation)
        //    .WithRequiredPrincipal(w => w.WinDev)
        //    .WillCascadeOnDelete();
        //// Configure WinDevID as FK for WinDevProfileAssignment
        //modelBuilder.Entity<WinDev>()
        //    .HasRequired(w => w.WinDevProfileAssignment)
        //    .WithRequiredPrincipal(w => w.WinDev)
        //    .WillCascadeOnDelete();





        modelBuilder.Entity<MDM_Profile_ServicesURL>()
        .HasKey(e => new { e.Profile_ID, e.GroupID });

        modelBuilder.Entity<tbl_app_mob_translog>()
            .Property(e => e.UserName)
            .IsUnicode(false);

        modelBuilder.Entity<tbl_app_msf_translog>()
            .Property(e => e.UserName)
            .IsUnicode(false);

        modelBuilder.Entity<MACHINE_ADVERTISEMENT>()
            .Property(e => e.ACREATEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<MACHINE_ADVERTISEMENT>()
            .Property(e => e.AAPPROVEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<MACHINE_ADVERTISEMENT>()
            .Property(e => e.ADECLINEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<MACHINE_ADVERTISEMENT>()
            .Property(e => e.ASTATUS)
            .HasPrecision(10, 0);

        modelBuilder.Entity<MACHINE_ADVERTISEMENT>()
            .Property(e => e.AUPDATEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<MACHINE_ADVERTISEMENT>()
            .Property(e => e.ATOTAL)
            .HasPrecision(10, 0);

        modelBuilder.Entity<ANDROIDMDM_APPLICATION>()
            .Property(e => e.APPID)
            .IsUnicode(false);

        modelBuilder.Entity<ANDROIDMDM_APPLICATION>()
            .Property(e => e.APPLICATION_NAME)
            .IsUnicode(false);

        modelBuilder.Entity<ANDROIDMDM_APPLICATION>()
            .Property(e => e.FPATH)
            .IsUnicode(false);

        modelBuilder.Entity<ANDROIDMDM_APPLICATION>()
            .Property(e => e.STATUS)
            .HasPrecision(1, 0);

        modelBuilder.Entity<ANDROIDMDM_APPLICATION>()
            .Property(e => e.VER)
            .HasPrecision(18, 3);

        modelBuilder.Entity<ANDROIDMDM_APPLICATION>()
            .Property(e => e.CREATED_ON)
            .HasPrecision(0);

        modelBuilder.Entity<ANDROIDMDM_APPLICATION>()
            .Property(e => e.CREATED_BY)
            .IsUnicode(false);

        modelBuilder.Entity<ANDROIDMDM_APPLICATION>()
            .Property(e => e.UPDATED_ON)
            .HasPrecision(6);

        modelBuilder.Entity<ANDROIDMDM_APPLICATION>()
            .Property(e => e.UPDATED_BY)
            .IsUnicode(false);

        modelBuilder.Entity<LOCALUSER>()
            .Property(e => e.LEMAIL)
            .IsUnicode(false);

        modelBuilder.Entity<LOCALUSER>()
            .Property(e => e.LRESETPASSWORD)
            .IsUnicode(false);

        modelBuilder.Entity<LOCALUSER>()
            .Property(e => e.LRESETPASSWORDDATETIME)
            .HasPrecision(6);

        modelBuilder.Entity<MACHINE>()
            .Property(e => e.MCREATEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<MACHINE>()
            .Property(e => e.MAPPROVEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<MACHINE>()
            .Property(e => e.MDECLINEDATE)
            .HasPrecision(6);

        modelBuilder.Entity<MACHINE>()
            .Property(e => e.MACTIVATEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<MACHINE>()
            .Property(e => e.MSTATUS)
            .HasPrecision(10, 0);

        modelBuilder.Entity<MACHINE>()
            .Property(e => e.LASTUPDATEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<MACHINE>()
            .Property(e => e.MONITORINGSTATUS)
            .HasPrecision(10, 0);

        modelBuilder.Entity<MACHINE>()
            .Property(e => e.MARKERSHAPE)
            .IsUnicode(false);

        modelBuilder.Entity<MDM_Profile>()
            .Property(e => e.PlistContent)
            .IsUnicode(false);

        modelBuilder.Entity<MENU>()
            .Property(e => e.MTYPE)
            .HasPrecision(10, 0);

        modelBuilder.Entity<MENU>()
            .Property(e => e.MGROUP)
            .HasPrecision(10, 0);

        modelBuilder.Entity<MENU>()
            .Property(e => e.MSEQ)
            .HasPrecision(10, 0);

        modelBuilder.Entity<MENU>()
            .Property(e => e.TEST)
            .HasPrecision(18, 0);

        modelBuilder.Entity<TRANSACTION_LOG>()
            .Property(e => e.TSTATUS)
            .HasPrecision(10, 0);

        modelBuilder.Entity<TRANSACTION_LOG>()
            .Property(e => e.TRANSDATE)
            .HasPrecision(6);

        modelBuilder.Entity<User_Branch>()
            .Property(e => e.FTPpath)
            .IsUnicode(false);

        modelBuilder.Entity<USER_LOGIN>()
            .Property(e => e.USTATUS)
            .HasPrecision(10, 0);

        modelBuilder.Entity<USER_LOGIN>()
            .Property(e => e.UCREATEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<USER_LOGIN>()
            .Property(e => e.UAPPROVEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<USER_LOGIN>()
            .Property(e => e.UDECLINEDATE)
            .HasPrecision(6);

        modelBuilder.Entity<USER_LOGIN>()
            .Property(e => e.AGENTFLAG)
            .HasPrecision(1, 0);

        modelBuilder.Entity<USER_LOGIN>()
            .Property(e => e.UUPDATEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<USER_LOGIN>()
            .Property(e => e.ULASTLOGINDATE)
            .HasPrecision(6);

        modelBuilder.Entity<USER_ROLES>()
            .Property(e => e.RSTATUS)
            .HasPrecision(10, 0);

        modelBuilder.Entity<USER_ROLES>()
            .Property(e => e.RCREATEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<USER_ROLES>()
            .Property(e => e.RAPPROVEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<USER_ROLES>()
            .Property(e => e.RDECLINEDATE)
            .HasPrecision(6);

        modelBuilder.Entity<USER_ROLES>()
            .Property(e => e.RUPDATEDDATE)
            .HasPrecision(6);

        modelBuilder.Entity<MDM_Commands>()
            .Property(e => e.Name)
            .IsUnicode(false);
    }
}
