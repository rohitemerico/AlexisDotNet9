using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dashboard.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ANDROIDMDM_APPLICATION",
                columns: table => new
                {
                    APPID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    APPLICATION_NAME = table.Column<string>(type: "varchar(4000)", unicode: false, maxLength: 4000, nullable: false),
                    FPATH = table.Column<string>(type: "varchar(4000)", unicode: false, maxLength: 4000, nullable: false),
                    STATUS = table.Column<decimal>(type: "numeric(1,0)", precision: 1, scale: 0, nullable: true),
                    VER = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true),
                    CREATED_ON = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: true),
                    CREATED_BY = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    UPDATED_ON = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    UPDATED_BY = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ANDROIDMDM_APPLICATION", x => x.APPID);
                });

            migrationBuilder.CreateTable(
                name: "LOCALUSER",
                columns: table => new
                {
                    LOGINID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    LPASSWORD = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LEMAIL = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    LRESETPASSWORD = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    LRESETPASSWORDDATETIME = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOCALUSER", x => x.LOGINID);
                });

            migrationBuilder.CreateTable(
                name: "MACHINE",
                columns: table => new
                {
                    MID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    MDESC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MSERIAL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MADDRESS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MLATITUDE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MLONGITUDE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MGROUPID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    MCREATEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    MAPPROVEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    MDECLINEDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    MAPPROVEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    MCREATEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    MDECLINEBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    MACTIVATEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    MSTATUS = table.Column<decimal>(type: "numeric(10,0)", precision: 10, scale: 0, nullable: true),
                    LASTUPDATEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    SKMESSAGE = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SKPIN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MKIOSKID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SESSIONKEY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MSTATIONID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IPADDRESS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PORTNUMBER = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MREMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MONITORINGSTATUS = table.Column<decimal>(type: "numeric(10,0)", precision: 10, scale: 0, nullable: true),
                    VTMVERSION = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MPILOT = table.Column<double>(type: "float", nullable: true),
                    MARKERSHAPE = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MACHINE", x => x.MID);
                });

            migrationBuilder.CreateTable(
                name: "MACHINE_ADVERTISEMENT",
                columns: table => new
                {
                    AID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ADESC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ACREATEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    AAPPROVEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    ADECLINEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    ACREATEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AAPPROVEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ADECLINEBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ASTATUS = table.Column<decimal>(type: "numeric(10,0)", precision: 10, scale: 0, nullable: true),
                    AABSOLUTEPATH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ARELATIVEPATHURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ANAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AEXTENSIONTYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ADURATION = table.Column<double>(type: "float", nullable: true),
                    AUPDATEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AUPDATEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    ATOTAL = table.Column<decimal>(type: "numeric(10,0)", precision: 10, scale: 0, nullable: true),
                    AISBACKGROUNDIMG = table.Column<bool>(type: "bit", nullable: false),
                    AREMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MACHINE_ADVERTISEMENT", x => x.AID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Certificates",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UDID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CertName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsIdentityCert = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Certificates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Commands",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Commands", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_HttpProxy",
                columns: table => new
                {
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProxyType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProxyServer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Port = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AllowByPassingProxy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_HttpProxy", x => x.Profile_ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_InstalledApplicationList",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UDID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BundleSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DynamicSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShortVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_InstalledApplicationList", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_InstalledApps",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UDID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AppName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_InstalledApps", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_OS_Updates",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UDID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HumanReadableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RestartRequired = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_OS_Updates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Pending_OS_Updates",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UDID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdateInstalled = table.Column<bool>(type: "bit", nullable: true),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Pending_OS_Updates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile",
                columns: table => new
                {
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Profile_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Profile_Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profile_GroupID = table.Column<int>(type: "int", nullable: true),
                    Profile_Status = table.Column<int>(type: "int", nullable: true),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlistContent = table.Column<string>(type: "text", unicode: false, nullable: false),
                    Profile_APNS_Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profile_Enroll_Path = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile", x => x.Profile_ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile_Cellular",
                columns: table => new
                {
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfiguredAPNType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultAPN_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultAPN_AuthenticationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultAPN_UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultAPN_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataAPN_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataAPN_AuthenticationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataAPN_UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataAPN_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataAPN_ProxyServer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile_Cellular", x => x.Profile_ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile_CertFile",
                columns: table => new
                {
                    IpadProfile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CertPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CertFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertVersion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile_CertFile", x => x.IpadProfile_ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile_General",
                columns: table => new
                {
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConsentMessage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Security = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuthorizationPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AutomaticallyRemoveProfile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AutomaticallyRemoveProfile_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AutomaticallyRemoveProfile_Days = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AutomaticallyRemoveProfile_Hours = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Branch_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Profile_APNS_Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profile_Enroll_Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    pStatus = table.Column<int>(type: "int", nullable: true),
                    LastUpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile_General", x => x.Profile_ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile_General_BranchID",
                columns: table => new
                {
                    IID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    cProfile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Branch_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile_General_BranchID", x => x.IID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile_Passcode",
                columns: table => new
                {
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AllowSimpleValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Requirealphanumericvalue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinimumPasscodeLength = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinimumNumberOfComplexCharacters = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaximumPasscodeAge = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaximumAutoLock = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasscodeHistory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaximumGracePeriod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaximumNumberOfFailedAttempts = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile_Passcode", x => x.Profile_ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile_Restriction",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RestrictionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsCheck = table.Column<bool>(type: "bit", nullable: true),
                    Queue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile_Restriction", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile_Restriction_Advance",
                columns: table => new
                {
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcceptCookies = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RestrictAppUsage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    App_Identify = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile_Restriction_Advance", x => x.Profile_ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile_ServicesURL",
                columns: table => new
                {
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupID = table.Column<int>(type: "int", nullable: false),
                    CheckInURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckOutURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnrollmentURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile_ServicesURL", x => new { x.Profile_ID, x.GroupID });
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile_VPN",
                columns: table => new
                {
                    Profile_VPN_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConnectionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_Server = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_Account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_Account_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_MachineAuthentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_SharedSecret = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_UseHybridAuthentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_PromptForPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_ProxySetup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_Proxy_Server = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_Proxy_Port = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_Authentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_Authentication_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IPSEC_ProxyServerURL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_SendAllTrafficThroughVPN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_AlwaysOnVPN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_AllowUserToDisableAutomaticConnection = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_UserSameTunnelConfigurationForCellularAndWifi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_Server = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_RemoteIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_LocalIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_MachineAuthentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_SharedSecret = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_EnableEAP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_EnableNATKeepaliveWhileTheDeviceIsAsleep = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_NATKeepAliveInternal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_DeadPeerDetectionRate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_DisableRedirects = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_DisableMobilityAndMultihoming = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_UseIPv4IPv6InternalSubnetAttributes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_EnablePerfectDForwordSecrecy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_EnableCertificateRevocationCheck = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_IKESA_EncryptionAlgorithm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_IKESA_IntegrityAlgorithm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_IKESA_DiffieHellmanGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_IKESA_LifeTimeInMinutes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiServer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiRemoteIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiLocalIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiMachineAuthentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiSharedSecret = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiEnableEAP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiEnableNATKeepaliveWhileTheDeviceIsAsleep = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiNATKeepAliveInternal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiDeadPeerDetectionRate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiDisableRedirects = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiDisableMobilityAndMultihoming = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiUseIPv4IPv6InternalSubnetAttributes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiEnablePerfectDForwordSecrecy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiEnableCertificateRevocationCheck = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularServer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularRemoteIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularLocalIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularMachineAuthentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularSharedSecret = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularEnableEAP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularEnableNATKeepaliveWhileTheDeviceIsAsleep = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularNATKeepAliveInternal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularDeadPeerDetectionRate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularDisableRedirects = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularDisableMobilityAndMultihoming = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularUseIPv4IPv6InternalSubnetAttributes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularEnablePerfectDForwordSecrecy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularEnableCertificateRevocationCheck = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_Child_EncryptionAlgorithm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_Child_IntegrityAlgorithm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_Child_DiffleHellmanGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_Child_LifetimeInMinutes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_ServiceExceptions_VoiceMail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_ServiceExceptions_AirPrint = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_AllowTrafficFromCaptiveWebSheetOutsideTheVPNTunnel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_AllowTrafficFromAllCaptiveNetworkingAppsOustsideVPNTunnel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_DisconnectOnIdle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_DisconnectOnIdle_Minutes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_DisconnectOnIdle_Seconds = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CaptiveNetworkingAppBundleIdentifiers = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IKEV2_Account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_Account_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_ProxySetup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_ProxySetup_Server = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_ProxySetup_Port = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_ProxySetup_Authentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_ProxySetup_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_ProxySetup_ProxyServerUrl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_EAP_Authentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiEAP_Authentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularEAP_Authentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_Server = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_Account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_UserAuthentication_RSASecurID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_UserAuthentication_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_SendAllTrafficeThroughVPN = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_MachineAuthentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_SharedSecret = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_UseHybridAuthentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_PromptForPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_ProxySetup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_ProxySetup_Server = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_ProxySetup_Port = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_ProxySetup_Authentication = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_ProxySetup_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    L2TP_ProxySetupURL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_CellularAccount_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IKEV2_WifiAccount_Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile_VPN", x => x.Profile_VPN_ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Profile_WIFI",
                columns: table => new
                {
                    Profile_WIFI_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Profile_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceSetIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HiddenNetwork = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AutoJoin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisableCaptiveNetworkDetection = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProxySetup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecurityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SecurityTypePassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NetworkType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FastLaneQosMarking = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EnableQosMarking = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WhitelistAppleAudioVideoCalling = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServerIPAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServerPort = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServerProxyURL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    App_Identifity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProxyPACFallbackAllowed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Profile_WIFI", x => x.Profile_WIFI_ID);
                });

            migrationBuilder.CreateTable(
                name: "MDM_Restriction_Menu",
                columns: table => new
                {
                    RID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RestrictionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RestrictionDesc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Queue = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<int>(type: "int", nullable: true),
                    RGroup = table.Column<int>(type: "int", nullable: true),
                    GroupHeader = table.Column<int>(type: "int", nullable: true),
                    NumberType = table.Column<int>(type: "int", nullable: true),
                    Partition = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDM_Restriction_Menu", x => x.RID);
                });

            migrationBuilder.CreateTable(
                name: "MENU",
                columns: table => new
                {
                    MID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    MTYPE = table.Column<decimal>(type: "numeric(10,0)", precision: 10, scale: 0, nullable: true),
                    MDESC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MPATH = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MGROUP = table.Column<decimal>(type: "numeric(10,0)", precision: 10, scale: 0, nullable: true),
                    MSEQ = table.Column<decimal>(type: "numeric(10,0)", precision: 10, scale: 0, nullable: true),
                    TEST = table.Column<decimal>(type: "decimal(18,0)", precision: 18, scale: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU", x => x.MID);
                });

            migrationBuilder.CreateTable(
                name: "TBLAGENTVIDEO",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JsonSetting = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBLAGENTVIDEO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TRANSACTION_LOG",
                columns: table => new
                {
                    TRANSID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    TTYPE = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    TSTATUS = table.Column<decimal>(type: "numeric(10,0)", precision: 10, scale: 0, nullable: true),
                    MSERIAL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TRANSDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    TDESC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    USERID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    MKIOSKID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SESSIONID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CUSTID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CUSTTYPE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    REMARKS = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TRANSACTION_LOG", x => x.TRANSID);
                });

            migrationBuilder.CreateTable(
                name: "User_Branch",
                columns: table => new
                {
                    bID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    bDesc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    bStatus = table.Column<int>(type: "int", nullable: true),
                    bMonday = table.Column<bool>(type: "bit", nullable: true),
                    bTuesday = table.Column<bool>(type: "bit", nullable: true),
                    bWednesday = table.Column<bool>(type: "bit", nullable: true),
                    bThursday = table.Column<bool>(type: "bit", nullable: true),
                    bFriday = table.Column<bool>(type: "bit", nullable: true),
                    bSaturday = table.Column<bool>(type: "bit", nullable: true),
                    bSunday = table.Column<bool>(type: "bit", nullable: true),
                    bOpenTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    bCloseTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    bCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    bApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    bDeclinedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    bUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    bCreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bDeclinedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    bUpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FTPpath = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    FTPUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FTPpassword = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    bBimbBranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    bBimbTellerID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    bBimbControlUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    bBimbBranchRCSFolderName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    bRemarks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Branch", x => x.bID);
                });

            migrationBuilder.CreateTable(
                name: "USER_LOGIN",
                columns: table => new
                {
                    AID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UNAME = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    USTATUS = table.Column<decimal>(type: "numeric(10,0)", precision: 10, scale: 0, nullable: true),
                    RID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UCREATEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    UAPPROVEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    UDECLINEDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    UCREATEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UAPPROVEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UDECLINEBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AGENTFLAG = table.Column<decimal>(type: "numeric(1,0)", precision: 1, scale: 0, nullable: true),
                    UFULLNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UCMID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    UUPDATEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    UUPDATEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ULASTLOGINDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    USESSIONID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SESSIONKEY = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UREMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LOGINFLAG = table.Column<double>(type: "float", nullable: true),
                    MCARDREPLENISHMENT = table.Column<double>(type: "float", nullable: true),
                    MCHEQUEREPLENISHMENT = table.Column<double>(type: "float", nullable: true),
                    MCONSUMABLE = table.Column<double>(type: "float", nullable: true),
                    MSECURITY = table.Column<double>(type: "float", nullable: true),
                    MTROUBLESHOOT = table.Column<double>(type: "float", nullable: true),
                    LOCALUSER = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_LOGIN", x => x.AID);
                });

            migrationBuilder.CreateTable(
                name: "USER_ROLES",
                columns: table => new
                {
                    RID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RDESC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RSTATUS = table.Column<decimal>(type: "numeric(10,0)", precision: 10, scale: 0, nullable: true),
                    RCREATEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    RAPPROVEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    RDECLINEDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    RCREATEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RAPPROVEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RDECLINEBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RUPDATEDBY = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    RUPDATEDDATE = table.Column<DateTime>(type: "datetime2(6)", precision: 6, nullable: true),
                    RREMARKS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ROLES", x => x.RID);
                });

            migrationBuilder.CreateTable(
                name: "tblMachine",
                columns: table => new
                {
                    MachineSerial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MachineImei = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MachineUDID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MachineName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsSupervised = table.Column<bool>(type: "bit", nullable: true),
                    SingleAppModeEnabled = table.Column<bool>(type: "bit", nullable: true),
                    LostModeEnabled = table.Column<bool>(type: "bit", nullable: true),
                    LostLatitude = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LostLongitude = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MachineStatus = table.Column<int>(type: "int", nullable: true),
                    iPadStatus = table.Column<int>(type: "int", nullable: true),
                    appStatus = table.Column<int>(type: "int", nullable: true),
                    componentStatus = table.Column<int>(type: "int", nullable: true),
                    MonitorPushDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MonitorRecDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApnPushDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApnRecDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ComponentAlertTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ComponentLastAlertTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastInitializeTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineDataSignal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    iPadBattLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ComponentBattLevel = table.Column<int>(type: "int", nullable: true),
                    ComponentCardReaderStatus = table.Column<bool>(type: "bit", nullable: true),
                    ComponentThumbStatus = table.Column<bool>(type: "bit", nullable: true),
                    MsfAppID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OSVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeviceCapacity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AvailableDevice_Capacity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BuildVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasscodePresent = table.Column<bool>(type: "bit", nullable: true),
                    PasscodeCompliant = table.Column<bool>(type: "bit", nullable: true),
                    PasscodeCompliantWithProfiles = table.Column<bool>(type: "bit", nullable: true),
                    WifiMAC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BluetoothMAC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsRoaming = table.Column<bool>(type: "bit", nullable: true),
                    EraseDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirmwareVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirmwareBatteryStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirmwareBatteryLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirmwareSerial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirmwareExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppDeviceToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMachine", x => x.MachineSerial);
                    table.ForeignKey(
                        name: "FK_tblMachine_User_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "User_Branch",
                        principalColumn: "bID");
                });

            migrationBuilder.CreateTable(
                name: "tbl_app_mob_translog",
                columns: table => new
                {
                    TransID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransStatus = table.Column<int>(type: "int", nullable: false),
                    TransDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StaffID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NRIC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountNo = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    PinIssuance = table.Column<bool>(type: "bit", nullable: true),
                    TblMachineMachineSerial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_app_mob_translog", x => x.TransID);
                    table.ForeignKey(
                        name: "FK_tbl_app_mob_translog_tblMachine_TblMachineMachineSerial",
                        column: x => x.TblMachineMachineSerial,
                        principalTable: "tblMachine",
                        principalColumn: "MachineSerial",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_app_msf_translog",
                columns: table => new
                {
                    TransID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransStatus = table.Column<int>(type: "int", nullable: false),
                    TransDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StaffID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NRIC = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountNo = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    PinIssuance = table.Column<bool>(type: "bit", nullable: true),
                    TblMachineMachineSerial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_app_msf_translog", x => x.TransID);
                    table.ForeignKey(
                        name: "FK_tbl_app_msf_translog_tblMachine_TblMachineMachineSerial",
                        column: x => x.TblMachineMachineSerial,
                        principalTable: "tblMachine",
                        principalColumn: "MachineSerial",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TblMonitoringDevices",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    TblMachineMachineSerial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Uptime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Downtime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUptime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastCaptured = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Uptime_Seconds = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Downtime_Seconds = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblMonitoringDevices", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TblMonitoringDevices_tblMachine_TblMachineMachineSerial",
                        column: x => x.TblMachineMachineSerial,
                        principalTable: "tblMachine",
                        principalColumn: "MachineSerial",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_app_mob_translog_TblMachineMachineSerial",
                table: "tbl_app_mob_translog",
                column: "TblMachineMachineSerial");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_app_msf_translog_TblMachineMachineSerial",
                table: "tbl_app_msf_translog",
                column: "TblMachineMachineSerial");

            migrationBuilder.CreateIndex(
                name: "IX_tblMachine_BranchId",
                table: "tblMachine",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TblMonitoringDevices_TblMachineMachineSerial",
                table: "TblMonitoringDevices",
                column: "TblMachineMachineSerial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ANDROIDMDM_APPLICATION");

            migrationBuilder.DropTable(
                name: "LOCALUSER");

            migrationBuilder.DropTable(
                name: "MACHINE");

            migrationBuilder.DropTable(
                name: "MACHINE_ADVERTISEMENT");

            migrationBuilder.DropTable(
                name: "MDM_Certificates");

            migrationBuilder.DropTable(
                name: "MDM_Commands");

            migrationBuilder.DropTable(
                name: "MDM_HttpProxy");

            migrationBuilder.DropTable(
                name: "MDM_InstalledApplicationList");

            migrationBuilder.DropTable(
                name: "MDM_InstalledApps");

            migrationBuilder.DropTable(
                name: "MDM_OS_Updates");

            migrationBuilder.DropTable(
                name: "MDM_Pending_OS_Updates");

            migrationBuilder.DropTable(
                name: "MDM_Profile");

            migrationBuilder.DropTable(
                name: "MDM_Profile_Cellular");

            migrationBuilder.DropTable(
                name: "MDM_Profile_CertFile");

            migrationBuilder.DropTable(
                name: "MDM_Profile_General");

            migrationBuilder.DropTable(
                name: "MDM_Profile_General_BranchID");

            migrationBuilder.DropTable(
                name: "MDM_Profile_Passcode");

            migrationBuilder.DropTable(
                name: "MDM_Profile_Restriction");

            migrationBuilder.DropTable(
                name: "MDM_Profile_Restriction_Advance");

            migrationBuilder.DropTable(
                name: "MDM_Profile_ServicesURL");

            migrationBuilder.DropTable(
                name: "MDM_Profile_VPN");

            migrationBuilder.DropTable(
                name: "MDM_Profile_WIFI");

            migrationBuilder.DropTable(
                name: "MDM_Restriction_Menu");

            migrationBuilder.DropTable(
                name: "MENU");

            migrationBuilder.DropTable(
                name: "tbl_app_mob_translog");

            migrationBuilder.DropTable(
                name: "tbl_app_msf_translog");

            migrationBuilder.DropTable(
                name: "TBLAGENTVIDEO");

            migrationBuilder.DropTable(
                name: "TblMonitoringDevices");

            migrationBuilder.DropTable(
                name: "TRANSACTION_LOG");

            migrationBuilder.DropTable(
                name: "USER_LOGIN");

            migrationBuilder.DropTable(
                name: "USER_ROLES");

            migrationBuilder.DropTable(
                name: "tblMachine");

            migrationBuilder.DropTable(
                name: "User_Branch");
        }
    }
}
