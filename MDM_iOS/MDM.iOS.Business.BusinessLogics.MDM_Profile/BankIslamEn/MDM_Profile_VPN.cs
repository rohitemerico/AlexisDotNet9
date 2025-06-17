using System.ComponentModel.DataAnnotations;
namespace MDM.iOS.Business.BusinessLogics.MDM_Profile.BankIslamEn;
public partial class MDM_Profile_VPN
{
    [Key]
    public Guid Profile_VPN_ID { get; set; }

    public Guid Profile_ID { get; set; }

    [StringLength(50)]
    public string? ConnectionName { get; set; }

    [StringLength(50)]
    public string? ConnectionType { get; set; }

    [StringLength(50)]
    public string? IPSEC_Server { get; set; }

    [StringLength(50)]
    public string? IPSEC_Account { get; set; }

    [StringLength(50)]
    public string? IPSEC_Account_Password { get; set; }

    [StringLength(50)]
    public string? IPSEC_MachineAuthentication { get; set; }

    [StringLength(50)]
    public string? IPSEC_GroupName { get; set; }

    [StringLength(50)]
    public string? IPSEC_SharedSecret { get; set; }

    [StringLength(50)]
    public string? IPSEC_UseHybridAuthentication { get; set; }

    [StringLength(50)]
    public string? IPSEC_PromptForPassword { get; set; }

    [StringLength(50)]
    public string? IPSEC_ProxySetup { get; set; }

    [StringLength(50)]
    public string? IPSEC_Proxy_Server { get; set; }

    [StringLength(50)]
    public string? IPSEC_Proxy_Port { get; set; }

    [StringLength(50)]
    public string? IPSEC_Authentication { get; set; }

    [StringLength(50)]
    public string? IPSEC_Authentication_Password { get; set; }

    [StringLength(50)]
    public string? IPSEC_Password { get; set; }

    [StringLength(50)]
    public string? IPSEC_ProxyServerURL { get; set; }

    [StringLength(50)]
    public string? IKEV2_SendAllTrafficThroughVPN { get; set; }

    [StringLength(50)]
    public string? IKEV2_AlwaysOnVPN { get; set; }

    [StringLength(50)]
    public string? IKEV2_AllowUserToDisableAutomaticConnection { get; set; }

    [StringLength(50)]
    public string? IKEV2_UserSameTunnelConfigurationForCellularAndWifi { get; set; }

    [StringLength(50)]
    public string? IKEV2_Server { get; set; }

    [StringLength(50)]
    public string? IKEV2_RemoteIdentifier { get; set; }

    [StringLength(50)]
    public string? IKEV2_LocalIdentifier { get; set; }

    [StringLength(50)]
    public string? IKEV2_MachineAuthentication { get; set; }

    [StringLength(50)]
    public string? IKEV2_SharedSecret { get; set; }

    [StringLength(50)]
    public string? IKEV2_EnableEAP { get; set; }

    [StringLength(50)]
    public string? IKEV2_EnableNATKeepaliveWhileTheDeviceIsAsleep { get; set; }

    [StringLength(50)]
    public string? IKEV2_NATKeepAliveInternal { get; set; }

    [StringLength(50)]
    public string? IKEV2_DeadPeerDetectionRate { get; set; }

    [StringLength(50)]
    public string? IKEV2_DisableRedirects { get; set; }

    [StringLength(50)]
    public string? IKEV2_DisableMobilityAndMultihoming { get; set; }

    [StringLength(50)]
    public string? IKEV2_UseIPv4IPv6InternalSubnetAttributes { get; set; }

    [StringLength(50)]
    public string? IKEV2_EnablePerfectDForwordSecrecy { get; set; }

    [StringLength(50)]
    public string? IKEV2_EnableCertificateRevocationCheck { get; set; }

    [StringLength(50)]
    public string? IKEV2_IKESA_EncryptionAlgorithm { get; set; }

    [StringLength(50)]
    public string? IKEV2_IKESA_IntegrityAlgorithm { get; set; }

    [StringLength(50)]
    public string? IKEV2_IKESA_DiffieHellmanGroup { get; set; }

    [StringLength(50)]
    public string? IKEV2_IKESA_LifeTimeInMinutes { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiServer { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiRemoteIdentifier { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiLocalIdentifier { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiMachineAuthentication { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiSharedSecret { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiEnableEAP { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiEnableNATKeepaliveWhileTheDeviceIsAsleep { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiNATKeepAliveInternal { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiDeadPeerDetectionRate { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiDisableRedirects { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiDisableMobilityAndMultihoming { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiUseIPv4IPv6InternalSubnetAttributes { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiEnablePerfectDForwordSecrecy { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiEnableCertificateRevocationCheck { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularServer { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularRemoteIdentifier { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularLocalIdentifier { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularMachineAuthentication { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularSharedSecret { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularEnableEAP { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularEnableNATKeepaliveWhileTheDeviceIsAsleep { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularNATKeepAliveInternal { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularDeadPeerDetectionRate { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularDisableRedirects { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularDisableMobilityAndMultihoming { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularUseIPv4IPv6InternalSubnetAttributes { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularEnablePerfectDForwordSecrecy { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularEnableCertificateRevocationCheck { get; set; }

    [StringLength(50)]
    public string? IKEV2_Child_EncryptionAlgorithm { get; set; }

    [StringLength(50)]
    public string? IKEV2_Child_IntegrityAlgorithm { get; set; }

    [StringLength(50)]
    public string? IKEV2_Child_DiffleHellmanGroup { get; set; }

    [StringLength(50)]
    public string? IKEV2_Child_LifetimeInMinutes { get; set; }

    [StringLength(50)]
    public string? IKEV2_ServiceExceptions_VoiceMail { get; set; }

    [StringLength(50)]
    public string? IKEV2_ServiceExceptions_AirPrint { get; set; }

    [StringLength(50)]
    public string? IKEV2_AllowTrafficFromCaptiveWebSheetOutsideTheVPNTunnel { get; set; }

    [StringLength(50)]
    public string? IKEV2_AllowTrafficFromAllCaptiveNetworkingAppsOustsideVPNTunnel { get; set; }

    [StringLength(50)]
    public string? IKEV2_DisconnectOnIdle { get; set; }

    [StringLength(50)]
    public string? IKEV2_DisconnectOnIdle_Minutes { get; set; }

    [StringLength(50)]
    public string? IKEV2_DisconnectOnIdle_Seconds { get; set; }

    [StringLength(500)]
    public string? IKEV2_CaptiveNetworkingAppBundleIdentifiers { get; set; }

    [StringLength(50)]
    public string? IKEV2_Account { get; set; }

    [StringLength(50)]
    public string? IKEV2_Account_Password { get; set; }

    [StringLength(50)]
    public string? IKEV2_ProxySetup { get; set; }

    [StringLength(50)]
    public string? IKEV2_ProxySetup_Server { get; set; }

    [StringLength(50)]
    public string? IKEV2_ProxySetup_Port { get; set; }

    [StringLength(50)]
    public string? IKEV2_ProxySetup_Authentication { get; set; }

    [StringLength(50)]
    public string? IKEV2_ProxySetup_Password { get; set; }

    [StringLength(50)]
    public string? IKEV2_ProxySetup_ProxyServerUrl { get; set; }

    [StringLength(50)]
    public string? IKEV2_EAP_Authentication { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiEAP_Authentication { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularEAP_Authentication { get; set; }

    [StringLength(50)]
    public string? L2TP_Server { get; set; }

    [StringLength(50)]
    public string? L2TP_Account { get; set; }

    [StringLength(50)]
    public string? L2TP_UserAuthentication_RSASecurID { get; set; }

    [StringLength(50)]
    public string? L2TP_UserAuthentication_Password { get; set; }

    [StringLength(50)]
    public string? L2TP_SendAllTrafficeThroughVPN { get; set; }

    [StringLength(50)]
    public string? L2TP_MachineAuthentication { get; set; }

    [StringLength(50)]
    public string? L2TP_GroupName { get; set; }

    [StringLength(50)]
    public string? L2TP_SharedSecret { get; set; }

    [StringLength(50)]
    public string? L2TP_UseHybridAuthentication { get; set; }

    [StringLength(50)]
    public string? L2TP_PromptForPassword { get; set; }

    [StringLength(50)]
    public string? L2TP_ProxySetup { get; set; }

    [StringLength(50)]
    public string? L2TP_ProxySetup_Server { get; set; }

    [StringLength(50)]
    public string? L2TP_ProxySetup_Port { get; set; }

    [StringLength(50)]
    public string? L2TP_ProxySetup_Authentication { get; set; }

    [StringLength(50)]
    public string? L2TP_ProxySetup_Password { get; set; }

    [StringLength(50)]
    public string? L2TP_ProxySetupURL { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularAccount { get; set; }

    [StringLength(50)]
    public string? IKEV2_CellularAccount_Password { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiAccount { get; set; }

    [StringLength(50)]
    public string? IKEV2_WifiAccount_Password { get; set; }
}
